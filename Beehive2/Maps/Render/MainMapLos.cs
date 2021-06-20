using Microsoft.Xna.Framework;
using System;
using System.Drawing;

namespace Beehive2
{
	public partial class MainMap : BaseMap<MapTile>
	{
		/// line-of-sight calculations and glow effects

		internal void RunLos()
		{
			ResetLos();
			MapTile pt = TileByLoc(Refs.p.loc);

			foreach (MapTile t in tiles)
			{
				//if (MapTile.Distance(pt, t) <= 6) { t.los = true; }
				t.los = true;

				// this looks really inefficent but works okay on our small map
				for (double prog = 0.0; prog < 1.0; prog += 0.01)
				{
					int newx = pt.loc.X + Convert.ToInt32((t.loc.X - pt.loc.X) * prog);
					int newy = pt.loc.Y + Convert.ToInt32((t.loc.Y - pt.loc.Y) * prog);
					Loc testLoc = new Loc(newx, newy);

					if (Refs.m.TileByLoc(testLoc).clear == false)
					{
						t.los = false; break;
					}
				}
			}
		}

		internal void ResetLos()
		{
			foreach (MapTile t in tiles) { t.los = false; }
		}

		internal void RunGlows()
		{
			// reset to black
			foreach (MapTile t in tiles) { t.backCol = Color.Black; }

			// add glows for characters & pens
			foreach (Cubi c in Refs.h.roster) { Glows.AddGlow(c.loc, c.myColor); }
			Glows.AddGlow(Refs.p.loc, Refs.p.myColor);
			foreach (Loc pen in Refs.m.pents) { Glows.AddGlow(pen, Color.Purple); }
		}
	}
}