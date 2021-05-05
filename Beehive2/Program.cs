using System.Windows.Forms;
using System;
using SadConsole;
using Microsoft.Xna.Framework;
using Console = SadConsole.Console;

namespace Beehive2
{
	public static class Program
	{
		private static void Main(string[] args)
		{
			SadConsole.Game.Create(80, 25);

			// Hook the start event so we can add consoles to the system.
			SadConsole.Game.OnInitialize = Init;

			// Start the game.
			SadConsole.Game.Instance.Run();
			SadConsole.Game.Instance.Dispose();
		}

		private static void Init()
		{
			//Global.CurrentScreen = new MapScreen();

			var console = new Console(65, 25);
			console.IsFocused = true;
			console.Components.Add(new MyKeyboardComponent());

			Global.CurrentScreen.IsFocused = true;

			Shown();
		}

		private static void Shown()
		{
			// generate map
			Refs.p = new Player("The Protagonist", Color.Cyan);
			Refs.p.SetXY(32, 12); // todo fix hardcoded numbers

			Refs.h = new Harem();
			Refs.m = new MazeGenerator().Create(65, 25);

			// draw initial map
			FlowMap.RemakeAllFlows();
			UpdateMap();
			GlowTest();
			HelpPopup();

			Player p = Refs.p;
			Cubi c = Refs.h.roster[0];
			Announce("Welcome to the underworld. Look out, they're getting away!", p.myAlign, p.myColor);
			Announce("You'll never catch meeee!", c.myAlign, c.myColor);
			Announce("We'll see about that!", p.myAlign, p.myColor);
			Announce("Whee! *giggle*", c.myAlign, c.myColor);

			c = Refs.h.roster[1];
			Announce("Run, Master! *nyhha!*", c.myAlign, c.myColor);

			c = Refs.h.roster[2];
			Announce("Chase me Master! *hehe*", c.myAlign, c.myColor);

			Refs.p.UpdateInventory();
		}

		public static void UpdateMap()
		{
			// TODO Refs.mf.MainBitmap.Image = Refs.m.AsBitmap();
			// TODO Refs.mf.Refresh();
		}

		private static void HelpPopup()
		{
			MessageBox.Show(
				"In your vast bed, tucked deep in a dreamworld, far outside time and space,\n" +
				"you play in eternal bliss with your horned lovers.\n\n" +
				"But they have escaped their pentagrams...\n" +
				"\tcatch them and bring them back home for a good spanking!\n\n" +
				"Keys:\n" +
				"\tWASD or arrow keys to move.\n" +
				"\tShift+Direction to pick up or put down various things.\n" +
				"\tCtrl+Direction to throw pillows!\n\n" +
				"Alternate controls:\n\n" +
				"\t'P' then Direction to place/pickup.\n" +
				"\t'T' then Direction to throw.\n\n" +
				"F6 and F9 to quicksave and quickload (unlikely to work between version changes!)\n" +
				"F1 to view this help again.\n\n" +
				"\tBeta features:\n" +
				"\t\t'F' to boink held Cubi.\n" +
				"\t\t'C' to discipline them. ;)"
				);
		}

		private static void GlowTest()
		{
			// check glow system is commutative
			Color c1 = Color.SlateBlue;
			Color c2 = Color.HotPink;
			//Color c3 = Color.FromArgb(0, 71, 171);
			Color c3 = Color.FromNonPremultiplied(new Vector4(0, 0, 71, 171));

			Color m1a = MainMap.GlowColOffset(c1, c2, 0.5);
			Color m1b = MainMap.GlowColOffset(m1a, c3, 0.5);

			Color m2a = MainMap.GlowColOffset(c1, c3, 0.5);
			Color m2b = MainMap.GlowColOffset(m2a, c2, 0.5);

			//MessageBox.Show(m1b + "\n" + m2b);
		}

		public static void Announce(string say, string align, Color col)
		{
			// TODO implement announce system
		}

		private static void Console_MouseMove(object sender, SadConsole.Input.MouseEventArgs e)
		{
			var console = (Console)sender;

			console.Print(1, 1, $"Mouse moving at {e.MouseState.CellPosition}          ");

			if (e.MouseState.Mouse.LeftButtonDown)
				console.Print(1, 2, $"Left button is down");
			else
				console.Print(1, 2, $"                   ");
		}
	}
}