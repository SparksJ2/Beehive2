using System;

namespace Beehive2
{
	[Serializable()]
	public class FlowMap : BaseMap<FlowTile>
	{
		/// holds flowmaps used in cubi AI pathfinding

		// VS says this should be readonly? it seems to work ok.
		private readonly int level;

		public FlowMap(int xIn, int yIn, int levelIn)
		{
			xLen = xIn;
			yLen = yIn;
			level = levelIn;
			tiles = new FlowTile[xLen, yLen];
			for (int x = 0; x < xLen; x++)
			{
				for (int y = 0; y < yLen; y++)
				{
					tiles[x, y] = new FlowTile(new Loc(x, y), this);
				}
			}
			SetToNines();
		}

		public static void RemakeAllFlows()
		{
			if (Refs.m.flows == null)
			{
				MainMap m = Refs.m;
				Init(ref m.flows, m.xLen, m.yLen);
			}
			foreach (FlowMap f in Refs.m.flows)
			{
				f.RemakeFlow();
			}
		}

		internal static void Init(ref FlowMap[] flows, int xLen, int yLen)
		{
			// init all flows stuff here
			var flowsCount = Refs.h.roster.Count + 1; // 0 is for master, eventually
			flows = new FlowMap[flowsCount];
			for (int fLoop = 0; fLoop < flowsCount; fLoop++)
			{
				flows[fLoop] = new FlowMap(xLen, yLen, fLoop);
			}
		}

		internal double GetHighest()
		{
			double high = 0;
			for (int x = 0; x < xLen; x++)
			{
				for (int y = 0; y < yLen; y++)
				{
					if (tiles[x, y].flow > high && tiles[x, y].flow < 2000)
					{
						high = tiles[x, y].flow;
					}
				}
			}
			return high;
		}

		internal double GetLowest()
		{
			double low = 0;
			for (int x = 0; x < xLen; x++)
			{
				for (int y = 0; y < yLen; y++)
				{
					if (tiles[x, y].flow < low && tiles[x, y].flow > -2000)
					{
						low = tiles[x, y].flow;
					}
				}
			}
			return low;
		}

		public void SetToNines()
		{
			// expensive! :-(
			foreach (FlowTile t in tiles) { t.flow = 9999; }
		}

		public void MultFactor(double d) // heh
		{
			for (int x = 0; x < xLen; x++)
			{
				for (int y = 0; y < yLen; y++)
				{
					tiles[x, y].flow *= d;
				}
			}
		}

		public void AdjustFactor(double d) // heh
		{
			for (int x = 0; x < xLen; x++)
			{
				for (int y = 0; y < yLen; y++)
				{
					tiles[x, y].flow += d;
				}
			}
		}

		public void Reverse()
		{
			double half = GetHighest() / 2;
			double v;
			for (int x = 0; x < xLen; x++)
			{
				for (int y = 0; y < yLen; y++)
				{
					v = tiles[x, y].flow;
					v -= half; v = -v; v += half;
					tiles[x, y].flow = v;
				}
			}
		}

		public void RemakeFlow()
		{
			// target tiles get a .flow of 0, tiles 1 square from target
			//    get a .flow of 1, tiles 2 out get a .flow of 2, etc...
			// So to navigate to a target tile, just pick a tile with less
			//    flow value than where you currently are.

			// tidy up values from previous runs
			SetToNines();

			// use myAi from here
			if (level > 0) // don't do player flow yet
			{
				// which cubi are we doing this for?
				Cubi c = Harem.GetId(level);

				if (c.doJailBreak)
				{
					//Console.WriteLine("Cubi " + c.name + " thinking about " +
						//c.myJbAi.Method.ToString() + ".");
					c.myJbAi(c.TeaseDistance, this);
				}
				else
				{
					//Console.WriteLine("Cubi " + c.name + " thinking about " +
						//c.myStdAi.Method.ToString() + ".");
					c.myStdAi(c.TeaseDistance, this);
				}
			}
		}

		public FlowTileSet SeedFlowSquares()
		{
			var flowList = new FlowTileSet();
			foreach (FlowTile fs in tiles) { if (fs.flow == 0) { flowList.Add(fs); } }
			return flowList;
		}

		public FlowTileSet AllFlowSquares()
		{
			var flowList = new FlowTileSet();
			foreach (FlowTile fs in tiles) { flowList.Add(fs); }
			return flowList;
		}

		public void RunFlow(Boolean maskWalls)
		{
			FlowTileSet heads = SeedFlowSquares();

			if (maskWalls)
			{
				foreach (FlowTile fs in heads)
				{
					// mask out walls etc, we don't flow over those
					MapTile thisTile = Refs.m.TileByLoc(fs.loc);
					if (!thisTile.clear) { fs.mask = true; }
				}
			}

			// I call them heads because they 'snake' outwards from the initial point(s)
			//    but you get splits into several heads at junctions so it's a bad metaphor...

			// loop till we can't find anything more to do,
			// (or we decide we've messed up)
			bool changes = true;
			int failsafe = 0;
			int headsProcessed = 0;
			while (changes == true && failsafe < 256)
			{
				changes = false; failsafe++;

				FlowTileSet newHeads = new FlowTileSet();

				// for each active head tile...
				foreach (FlowTile headTile in heads)
				{
					// ...find the tiles next to it...
					FlowTileSet newTiles = new FlowTileSet()
					{
						headTile.OneNorth(),
						headTile.OneEast(),
						headTile.OneSouth(),
						headTile.OneWest()
					};

					// ... (ignoring any nulls) ...
					newTiles.RemoveWhere(item => item == null);

					// ... and for each one found ...
					foreach (FlowTile newFlowSq in newTiles)
					{
						// ... if we can improve the flow rating of it ...
						double delta = newFlowSq.flow - headTile.flow;

						MapTile targetTile = Refs.m.TileByLoc(headTile.loc);
						if (targetTile.clear && delta > 1.01)
						{
							// ... do so, and then make it a new head ...
							newFlowSq.flow = headTile.flow + 1;
							newHeads.Add(newFlowSq); // automatically de-duplicates
							changes = true;
						}
					}
					headsProcessed++;
				}
				// ... and next time around, we keep going using those new heads
				heads = newHeads;
				//Console.WriteLine("//Processed " + headsProcessed + "head tiles.");

				if (failsafe == 254) Console.WriteLine("!!!!!! Hit flow failsafe !!!!!!");
			}
		}
	}
}