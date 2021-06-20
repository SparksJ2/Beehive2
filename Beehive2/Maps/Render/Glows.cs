using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Beehive2
{
	internal class Glows
	{
		public static void AddGlow(Loc l, Color glowCol)
		{
			// todo what about overlapping glows?
			MapTileSet glowingTiles = new MapTileSet();

			// get glow source tile
			MapTile glowSource = Refs.m.TileByLoc(l);

			// light source tile
			glowSource.backCol = GlowColOffset(glowSource.backCol, glowCol, 0.5);

			// add source tile to tile list
			glowingTiles.Add(glowSource);

			// spread glow to nearby tiles
			Glowinator(glowCol, glowingTiles, glowSource, 0.25);
		}

		public static Color GlowColOffset(Color initial, Color glowCol, double amt)
		{
			// additive model, adds glow colour to initial colour, scaled by amt
			int r = Convert.ToInt32(initial.R + (glowCol.R * amt * 0.5));
			int g = Convert.ToInt32(initial.G + (glowCol.G * amt * 0.5));
			int b = Convert.ToInt32(initial.B + (glowCol.B * amt * 0.5));

		
			// prevent overflow
			if (r > 255) r = 255;
			if (g > 255) g = 255;
			if (b > 255) b = 255;

			return new Color(r, g, b, 255);
		}

		private static void Glowinator(Color glowCol, MapTileSet glowTiles, MapTile source, double amt)
		{
			// get tiles next to the source
			MapTileSet toLight = source.GetPossibleMoves(Dir.AllAround);

			// spread glow iteratively
			while (amt > 0.01)
			{
				MapTileSet newLight = new MapTileSet();
				
				// cycle through tiles to light up
				foreach (MapTile tl in toLight)
				{
					// if tile is clear and not already processed
					if (tl.clear && !glowTiles.Contains(tl))
					{
						// add glow to tile background colour
						tl.backCol = GlowColOffset(tl.backCol, glowCol, amt);

						// add to list of processed tiles
						glowTiles.Add(tl);

						// create list of tiles around current tile & add them to new list
						MapTileSet more = tl.GetPossibleMoves(Dir.AllAround);
						foreach (MapTile m in more) { newLight.Add(m); }
					}
				}
				
				toLight = newLight; // start on new list next cycle
				amt -= 0.15; // fade with distance
			}
		}
	}
}