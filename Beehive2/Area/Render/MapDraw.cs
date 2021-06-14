using Microsoft.Xna.Framework;
using SadConsole;
using Console = SadConsole.Console;

namespace Beehive2
{
	public partial class MainMap
	{
		/// turns map data into a bitmap. complicated.
		///
		// todo de-duplicate

		public void RenderMapAll() // rm used to return Bitmap
		{
			Console con = Refs.con;

			// clear the canvas to dark
			con.DefaultBackground = Color.Black;
			con.Clear();

			// do all backgrounds here (includes flow, los, glow effects)
			Refs.m.RunLos();
			Refs.m.RunGlows();
			foreach (MapTile t in tiles) { TileDraw.AddBackgroundStuff(con, t); }

			// add walls, nectar
			foreach (MapTile t in tiles) { TileDraw.AddForegroundStuff(con, t); }

			// add specials
			// rm TileDraw.AddCharSpecial(bmp, "⛤");

			// add mobiles (player and harem)
			TileDraw.AddCharMobile(con, Refs.p);
			foreach (Cubi c in Refs.h.roster) { TileDraw.AddCharMobile(con,c); }
		}

		// point update for animations
		public void CubiSingleTileUpdate(Cubi c)
		{
			// TODO Image img = Refs.mf.MainBitmap.Image;
			// TODO TileDraw.AddCharMobile(img, c);
		}

		// point update for animations
		internal void ResetTile(Loc loc)
		{
			// rm Image img = Refs.mf.MainBitmap.Image;
			// rm SetBlank(img, Refs.m.TileByLoc(loc));
			// rm TileDraw.AddForegroundStuff(img, Refs.m.TileByLoc(loc));
		}

		// point update for animations
		public void SetBlank(MapTile t) // Image img
		{
			// rm int x1 = (t.loc.X * FrameData.multX) + FrameData.edgeX;
			int y1 = (t.loc.Y * FrameData.multY) + FrameData.edgeY;
			// rm Color useCol = Color.DarkSlateBlue;

			// rm using (var gFlow = Graphics.FromImage(img))
			// rm {
			// rm // Create a rectangle for the working area on the map
			// rm RectangleF tileRect = new RectangleF(x1, y1, FrameData.multX, FrameData.multY);
			// rm using (var flowBrush = new SolidBrush(useCol))
			// rm {
			// rm gFlow.FillRectangle(flowBrush, tileRect);
			// rm }
		}
	}
}