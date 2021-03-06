using System.Linq;
using System.Diagnostics;

namespace Beehive2
{
	public partial class MainMap
	{
		/// map generator specific tools
		// has cached data sets that should only be used at mapgen time!

		public MapTileSet GetNextTo(MapTile t)
		{
			var x = new MapTileSet()
			{ t.OneNorth(), t.OneSouth(), t.OneEast(), t.OneWest() };

			x.RemoveWhere(i => i == null);
			// leave border intact
			x.RemoveWhere(i => i.loc.X == 0);
			x.RemoveWhere(i => i.loc.Y == 0);
			x.RemoveWhere(i => i.loc.X == xLen - 1);
			x.RemoveWhere(i => i.loc.Y == yLen - 1);
			return x;
		}

		public MapTileSet GetClosed3Sides(MapTileSet input)
		{
			var r = new MapTileSet();
			foreach (MapTile t in input)
			{
				int sum = 0;
				if (t.OneNorth() == null || t.OneNorth().clear == false) sum++;
				if (t.OneSouth() == null || t.OneSouth().clear == false) sum++;
				if (t.OneEast() == null || t.OneEast().clear == false) sum++;
				if (t.OneWest() == null || t.OneWest().clear == false) sum++;
				if (sum >= 3) r.Add(t);
			}
			return r;
		}

		internal void MakeClearArea(Loc point1, Loc point2)
		{
			// todo note clears the area inside, not including the boundary
			MapTileSet workingList = TileList();
			workingList = workingList.Where(t =>
			   t.loc.X > point1.X &&
			   t.loc.X < point2.X).ToMapTileSet();

			workingList = workingList.Where(t =>
			  t.loc.Y > point1.Y &&
			  t.loc.Y < point2.Y).ToMapTileSet();

			foreach (MapTile t in workingList) { t.clear = true; }
		}

		internal void MarkNoTunnel(Loc point1, Loc point2)
		{
			var workingList = TileList();
			workingList = workingList.Where(t =>
			   t.loc.X > point1.X &&
			   t.loc.X < point2.X).ToMapTileSet();

			workingList = workingList.Where(t =>
			   t.loc.Y > point1.Y &&
			   t.loc.Y < point2.Y).ToMapTileSet();

			foreach (MapTile t in workingList) { t.noTunnel = true; }
		}

		public MapTileSet GetClosed5Sides(MapTileSet input)
		{
			var r = new MapTileSet();
			foreach (MapTile t in input)
			{
				int sum = 0;
				if (t.OneNorth() == null || t.OneNorth().clear == false) sum++;
				if (t.OneSouth() == null || t.OneSouth().clear == false) sum++;
				if (t.OneEast() == null || t.OneEast().clear == false) sum++;
				if (t.OneWest() == null || t.OneWest().clear == false) sum++;

				if (t.OneNorthEast() == null || t.OneNorthEast().clear == false) sum++;
				if (t.OneSouthEast() == null || t.OneSouthEast().clear == false) sum++;
				if (t.OneNorthWest() == null || t.OneNorthWest().clear == false) sum++;
				if (t.OneSouthWest() == null || t.OneSouthWest().clear == false) sum++;

				if (sum >= 5) r.Add(t);
			}
			return r;
		}

		// cached clear tiles list for maze generator. not for general use.
		private MapTileSet clearCache;

		public void AddToClearTileCache(MapTile t) => clearCache.Add(t);

		public void DelFromClearTileCache(MapTile t) => clearCache.Remove(t);

		public void InitClearTilesCache() => clearCache = TileList().Where(t => t.clear).ToMapTileSet();

		public MapTileSet GetClearTilesCache() => clearCache;

		public void DoneWithClearTileCache() => clearCache = null;

		public void ConsoleDump()
		{
			Debug.WriteLine("+--------------------+");
			for (int y = 0; y < yLen; y++)
			{
				var rowofchars = "";
				for (int x = 0; x < xLen; x++)
				{
					MapTile t = tiles[x, y];
					if (t.clear)
						rowofchars += " ";
					else
						rowofchars += t.gly;
				}
				Debug.WriteLine("|" + rowofchars + "|");
			}
			Debug.WriteLine("+--------------------+");
		}
	}
}