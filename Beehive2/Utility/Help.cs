using System.Collections.Generic;
using System.Windows.Forms;

namespace Beehive2
{
	public static class Help
	{
		public static void ShowHelp()
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
	}
}