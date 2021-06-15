using System.Windows.Forms;
using System;
using SadConsole;
using Microsoft.Xna.Framework;
using Console = SadConsole.Console;

namespace Beehive2
{
	public class Main
	{
		public void Show()
		{
			// generate map
			Refs.p = new Player("The Protagonist", Color.Cyan);
			Refs.p.SetXY(32, 12); // todo fix hardcoded numbers

			Refs.h = new Harem();
			Refs.m = new MazeGenerator().Create(Refs.width, Refs.height);

			// draw initial map
			FlowMap.RemakeAllFlows();
			Refs.m.RenderMapAll();
			GlowTest();
			//HelpPopup();

			Player p = Refs.p;
			Cubi c = Refs.h.roster[0];
			Announcer.Say("Welcome to the underworld. Look out, they're getting away!", p.myAlign, p.myColor);
			Announcer.Say("You'll never catch meeee!", c.myAlign, c.myColor);
			Announcer.Say("We'll see about that!", p.myAlign, p.myColor);
			Announcer.Say("Whee! *giggle*", c.myAlign, c.myColor);

			c = Refs.h.roster[1];
			Announcer.Say("Run, Master! *nyhha!*", c.myAlign, c.myColor);

			c = Refs.h.roster[2];
			Announcer.Say("Chase me Master! *hehe*", c.myAlign, c.myColor);

			Refs.p.UpdateInventory();
		}

		private void HelpPopup()
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

		private void GlowTest()
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
	}
}