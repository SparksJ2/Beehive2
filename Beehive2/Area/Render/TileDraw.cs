using System;

using Microsoft.Xna.Framework;
using SadConsole;
using Console = SadConsole.Console;
using OldConsole = System.Console;

namespace Beehive2
{
	public class TileDraw
	{
		// todo de-duplicate
		private static string[] nectarChars =
		{" ","⠂", "⡂", "⡡", "⢕", "⢝", "⣝", "⣟", "⣿" }; // TODO find substitutes

		public static void AddForegroundStuff(Console con, MapTile t) // Image img
		{
			// description for rewrite:
			//  display nectar drops
			//  display walls

			if (t.clear)  // if tile does not have a 'wall' / pillow on
			{
				// display nectar drops using deepest level
				int deepestLevel = 0;
				int deepestAmt = 0;
				int sumAmt = 0;
				for (int nLoop = 0; nLoop < t.nectarLevel.Length; nLoop++)
				{
					sumAmt += t.nectarLevel[nLoop];
					if (t.nectarLevel[nLoop] > deepestAmt)
					{
						deepestAmt = t.nectarLevel[nLoop];
						deepestLevel = nLoop;
					}
				}

				if (deepestAmt > 0)
				{
					Color nectarCol;
					if (deepestLevel == 0) { nectarCol = Refs.p.myColor; }
					else { nectarCol = Harem.GetId(deepestLevel).myColor; }

					// Color mixedCol = GetColorMix(t);

					if (sumAmt > 8) { sumAmt = 8; }
					string useNectarChar = nectarChars[sumAmt];
					//if (sumAmt > 1) { useNectarChar = nectarCharLarge; }

					//con.Print(t.loc.X, t.loc.Y, useNectarChar, nectarCol, t.backCol);
				}
			}
			// todo bigger blob for more nectar maybe?
			else // it's not marked as clear, so draw the wall

			{
				con.Print(t.loc.X, t.loc.Y, t.asc.ToString(), Color.White, Color.DarkBlue);
			}
		}

		public static void AddBackgroundStuff(Console con, MapTile t) // Image img,
		{
			// description for rewrite:
			//  if tile is not wall:
			//   show flows if set and if tile has no wall on
			//   else show player LOS

			if (t.clear)  // set flow as background only
			{
				int showFlow = Refs.p.viewFlow;
				if (showFlow > 0)
				{
					Color flowCol = Harem.GetId(showFlow).myColor;

					double flowInt = Refs.m.flows[showFlow].TileByLoc(t.loc).flow;

					int r = ByteLimit(Convert.ToInt32(flowCol.R - flowInt * 4));
					int g = ByteLimit(Convert.ToInt32(flowCol.G - flowInt * 4));
					int b = ByteLimit(Convert.ToInt32(flowCol.B - flowInt * 4));

					//Color useCol = Color.FromArgb(r, g, b);
					Color useCol = Color.FromNonPremultiplied(new Vector4(0, r, g, b));

					con.SetBackground(t.loc.X, t.loc.Y, useCol);
				}
				else // show player los instead
				{
					Color losCol = Color.Black;
					Color hidCol = Color.DarkBlue;
					con.SetBackground(t.loc.X, t.loc.Y, t.los ? losCol : hidCol);
				}
			}
		}

		private static int ByteLimit(int x)
		{
			x = x < 0 ? 0 : x;
			x = x > 255 ? 255 : x;
			return x;
		}

		public static void AddCharSpecial(string s) // Image img
		{
			// description for rewrite:
			//  add specials (currently just pentacages)

			if (s == "⛤") // set up bed
			{
				// rm using (var gBed = Graphics.FromImage(img))
				{
					// rm Bitmap bedBitmap = SpriteManager.GetSprite("⛤", Refs.m.tripSize, Color.Purple, Color.Black);

					foreach (Loc pen in Refs.m.pents)
					{
						int bedx1 = ((pen.X - 1) * FrameData.multX) + FrameData.edgeX;
						int bedy1 = ((pen.Y - 1) * FrameData.multY) + FrameData.edgeY;
						int bedx2 = FrameData.multX * 3;
						int bedy2 = FrameData.multY * 3;
						// rm RectangleF tileBed = new RectangleF(bedx1, bedy1, bedx2, bedy2);
						// rm gBed.DrawImage(bedBitmap, bedx1, bedy1);
					}
				}
			}
		}

		public static void AddCharMobile(Console con, Mobile m)
		{
			// description for rewrite:
			//  add player as '♂' char and cubi as '☿' char
			string s = m.glyph;

			// begin foreground
			if ((s == "♂" || s == "☿")) // && Refs.m.TileByLoc(m.loc).los)
			{

				if (s == "♂") { con.Print( m.loc.X, m.loc.Y, ((char)11).ToString(), m.myColor); }
				if (s == "☿") { con.Print(m.loc.X, m.loc.Y, ((char)12).ToString(), m.myColor); }
			}
		}
	}
}