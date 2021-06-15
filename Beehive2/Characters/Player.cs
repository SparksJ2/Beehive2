using System;
using System.Drawing;
using System.IO;

// TODO using System.Runtime.Serialization.Formatters.Binary; ?
using Microsoft.Xna.Framework;
using Keys = Microsoft.Xna.Framework.Input.Keys;
using SadConsole.Input;

namespace Beehive2
{
	[Serializable()]
	public partial class Player : Mobile
	{
		public int heldPillows = 0;
		public int heldCubiId = 0;
		public int viewFlow = 0;
		public Loc lastMove;
		private bool throwmode = false, placemode = false, victory = false;
		public int turnCounter = 0;

		public Player(string name, Color useColor) : base(name, useColor)
		{
			glyph = "♂";
		}

		// returns number of round passed, 0 for free actions, 1 for normal moves.
		public int HandlePlayerInput(SadConsole.Console console, Keyboard info, out bool handled)
		{
			handled = true; // TODO ignore mouse for now

			// for convenience
			MapTile here = Refs.m.TileByLoc(loc);
			Loc lastPos = loc;

			// debugging nectar report
			Console.Write("Nectar here is ");
			foreach (int i in here.nectarLevel) { Console.Write(i + ", "); }
			Console.WriteLine(".");

			if (info.IsKeyPressed(Keys.F) && heldCubiId != 0) return BoinkHeld();
			if (info.IsKeyPressed(Keys.C) && heldCubiId != 0) return CaneHeld();

			// keys to trigger visualised flows, UI only action, so quick return
			if (info.IsKeyPressed(Keys.D0)) { viewFlow = 0; return 0; }
			if (info.IsKeyPressed(Keys.D1)) { viewFlow = 1; return 0; }
			if (info.IsKeyPressed(Keys.D2)) { viewFlow = 2; return 0; }
			if (info.IsKeyPressed(Keys.D3)) { viewFlow = 3; return 0; }
			if (info.IsKeyPressed(Keys.D4)) { viewFlow = 4; return 0; }

			int timepass = 0; // actions only cause time to pass when set to >0

			if (info.IsKeyPressed(Keys.Space)) { return 1; } // allow waiting at any time

			// if we are in placemode, perform a place action in that direction
			if (placemode || info.IsKeyDown(Keys.LeftShift) || info.IsKeyDown(Keys.RightShift))
			{
				// todo place / pickup is a free action... for now
				if (info.IsKeyPressed(Keys.Down) || info.IsKeyPressed(Keys.S)) { ActionSouth(); FinishMode(); }
				if (info.IsKeyPressed(Keys.Right) || info.IsKeyPressed(Keys.D)) { ActionEast(); FinishMode(); }
				if (info.IsKeyPressed(Keys.Up) || info.IsKeyPressed(Keys.W)) { ActionNorth(); FinishMode(); }
				if (info.IsKeyPressed(Keys.Left) || info.IsKeyPressed(Keys.A)) { ActionWest(); FinishMode(); }
				if (info.IsKeyPressed(Keys.Escape)) { CancelModes(); return 0; }
			}
			// if we are in placemode, perform a throw action in that direction
			else if (throwmode || info.IsKeyDown(Keys.LeftControl) || info.IsKeyDown(Keys.RightControl))
			{
				// todo throwing is a free action... for now
				if (info.IsKeyPressed(Keys.Down) || info.IsKeyPressed(Keys.S)) { ThrowSouth(); FinishMode(); }
				if (info.IsKeyPressed(Keys.Right) || info.IsKeyPressed(Keys.D)) { ThrowEast(); FinishMode(); }
				if (info.IsKeyPressed(Keys.Up) || info.IsKeyPressed(Keys.W)) { ThrowEast(); FinishMode(); }
				if (info.IsKeyPressed(Keys.Left) || info.IsKeyPressed(Keys.A)) { ThrowWest(); FinishMode(); }
				if (info.IsKeyPressed(Keys.Escape)) { CancelModes(); }
			}
			// if we're not in a mode, process directional moves and mode requests
			else
			{
				// directional moves cost 1 turn
				if (info.IsKeyPressed(Keys.Down) || info.IsKeyPressed(Keys.S)) { RunSouth(); FinishMode(); timepass = 1; }
				if (info.IsKeyPressed(Keys.Right) || info.IsKeyPressed(Keys.D)) { RunEast(); FinishMode(); timepass = 1; }
				if (info.IsKeyPressed(Keys.Up) || info.IsKeyPressed(Keys.W)) { RunNorth(); FinishMode(); timepass = 1; }
				if (info.IsKeyPressed(Keys.Left) || info.IsKeyPressed(Keys.A)) { RunWest(); FinishMode(); timepass = 1; }

				// mode requests are free actions
				if (info.IsKeyPressed(Keys.T)) { SetThrowMode(); }
				if (info.IsKeyPressed(Keys.P)) { SetPlaceMode(); }
				if (info.IsKeyPressed(Keys.Escape)) { CancelModes(); }
			}

			// save our current location for next turn
			lastMove = Loc.SubPts(loc, lastPos);

			// starting at 1 skips player nectar processing for now
			for (int nLoop = 1; nLoop < here.nectarLevel.Length; nLoop++)
			{
				if (here.nectarLevel[nLoop] > 0)
				{
					horny += here.nectarLevel[nLoop];
					here.nectarLevel[nLoop] = 0;
				}
			}

			if (horny > 15) // having fun
			{
				Announcer.Say("Awwww yeah! *splurt*", myAlign, myColor);
				timepass += 5;
				MainMap.SplurtNectar(here, myIndex: 0);
				horny = 0;
			}

			if (!victory)
			{
				// we're duplicating this location scanning code a lot...
				// but this will be useful if we ever move jails so I'll leave it

				// get list of capture tiles
				MapTileSet jails = new MapTileSet();
				foreach (Loc l in Refs.m.pents) { jails.Add(Refs.m.TileByLoc(l)); }

				// get list of cubi locations
				MapTileSet breaker = new MapTileSet();
				foreach (Cubi c in Refs.h.roster) { breaker.Add(Refs.m.TileByLoc(c.loc)); }

				// IntersectWith to get occupied jails
				jails.IntersectWith(breaker);

				// if jails filled = total jails, we won!
				if (jails.Count == Refs.m.pents.Count)
				{
					victory = true;
					Announcer.Say("Gotcha all! And in only " + turnCounter + " turns!", myAlign, myColor);
				}
			}

			return timepass;
		}

		private void FinishMode()
		{
			//Announcer.Announce("Back to the chase!", myAlign, myColor);
			placemode = false;
			throwmode = false;
		}

		private void CancelModes()
		{
			placemode = false;
			throwmode = false;
			Announcer.Say("Never mind! Back to the chase!", myAlign, myColor);
		}

		private void SetPlaceMode()
		{
			placemode = true;
			Announcer.Say("Place/pickup where? (esc to cancel)", myAlign, myColor);
		}

		private void SetThrowMode()
		{
			throwmode = true;
			Announcer.Say("Throw which way? (esc to cancel)", myAlign, myColor);
		}

		public void UpdateInventory()
		{
			// TODO inventory
			///Refs.mf.miniInventory.Text =
			//"pillows: " + heldPillows + "\n" +
			//"succubi: " + (heldCubiId == 0 ? 0 : 1);
		}

		private void RunNorth()
		{
			MapTile t = Refs.m.TileByLoc(loc).OneNorth();
			if (t.clear) loc = t.loc;
		}

		private void RunEast()
		{
			MapTile t = Refs.m.TileByLoc(loc).OneEast();
			if (t.clear) loc = t.loc;
		}

		private void RunSouth()
		{
			MapTile t = Refs.m.TileByLoc(loc).OneSouth();
			if (t.clear) loc = t.loc;
		}

		private void RunWest()
		{
			MapTile t = Refs.m.TileByLoc(loc).OneWest();
			if (t.clear) loc = t.loc;
		}

		private bool IsVertical(Loc v) => v.Y != 0;
	}
}