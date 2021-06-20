using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Beehive2
{
	partial class Player
	{
		/// fun class for manually spanking a held cubi

		private int CaneHeld()
		{
			MapTile here = Refs.m.TileByLoc(loc);
			Cubi partner = Harem.GetId(heldCubiId);

			// todo different text on repeat spankings.
			Announcer.Say("Time for discpline!", myAlign, myColor);
			Announcer.Say("Yes! I mean no! I mean yes! Owwwww!", partner.myAlign, partner.myColor);

			partner.Spanked += 15;
			partner.AddHorny(5);

			Announcer.Say("I think I'll stay put... for now.", partner.myAlign, partner.myColor);

			return 2;
		}
	}
}