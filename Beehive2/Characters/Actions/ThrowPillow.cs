using System.Threading;

namespace Beehive2
{
	partial class Player
	{
		/// player-throwing-pillow trajectory logic

		private void ThrowPillowMain(Loc vector)
		{
			Announcer.Say("You throw a pillow!", myAlign, myColor);

			// can't throw without pillow!
			if (heldPillows <= 0)
			{ return; }
			else
			{ heldPillows--; UpdateInventory(); }

			// determine release point of throw
			// todo check for hit on very first tile -- where to put pillow?
			Loc startloc = Loc.AddPts(this.loc, vector);
			MapTile activeTile = Refs.m.TileByLoc(startloc);
			string pillowGlyph = "O";

			// if the next tile now is a lover, extra spank stun!
			// todo consolidate long and short throws
			string moveClear = CheckClearForThrown(vector, activeTile);
			if (moveClear == "cubi")
			{
				MapTile victimTile = Refs.m.TileByLoc(Loc.AddPts(activeTile.loc, vector));
				Cubi victim = Refs.m.CubiAt(victimTile.loc);

				Announcer.Say("POINT BLANK PILLOW SPANK!", myAlign, myColor);
				victim.Spanked += 5;
				Announcer.Say("oww! *moan*", victim.myAlign, victim.myColor);
			}

			while (moveClear == "clear")
			{
				// blip activeTile with pillow symbol
				AnimatePillow(activeTile, pillowGlyph);

				// is the next tile clear?
				moveClear = CheckClearForThrown(vector, activeTile);

				// nope, it has a cubi in.
				if (moveClear == "cubi")
				{
					MapTile victimTile = Refs.m.TileByLoc(Loc.AddPts(activeTile.loc, vector));
					Cubi victim = Refs.m.CubiAt(victimTile.loc);

					MapTileSet escapes = new MapTileSet();

					if (IsVertical(vector)) { escapes = victimTile.GetPossibleMoves(Dir.DodgeVertical); }
					else { escapes = victimTile.GetPossibleMoves(Dir.DodgeHorizontal); }

					if (escapes.Count > 0)
					{
						victim.loc = MainMap.RandomFromList(escapes).loc;
						Announcer.Say("Nyahhh missed me!", victim.myAlign, victim.myColor);
						moveClear = "clear";
					}
					else
					{
						victim.Spanked += 5;
						Announcer.Say("Owwwww!", victim.myAlign, victim.myColor);
					}
				}

				// just a wall. stop here.
				if (moveClear == "wall")
				{ Announcer.Say("You didn't hit anything interesting.", myAlign, myColor); }

				// it's clear, so move activeTile up and iterate
				if (moveClear == "clear")
				{ activeTile = Refs.m.TileByLoc(Loc.AddPts(vector, activeTile.loc)); }
			}
			// leave pillow on ground to form new obstruction
			activeTile.clear = false;
			Refs.m.HealWalls();

			Refs.m.RenderMapAll();
		}

		private void AnimatePillow(MapTile activeTile, string glyph)
		{
			// todo animation code is a bit makeshift and needs to be cleaned up andmoved to Map.cs
			activeTile.clear = false;
			activeTile.gly = glyph;

			Refs.m.RenderMapAll();

			Thread.Sleep(75);
			activeTile.clear = true;
			activeTile.gly = " ";

			Refs.m.RenderMapAll();
			// todo also fix nectar drops.
		}
	}
}