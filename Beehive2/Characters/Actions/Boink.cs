using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Beehive2
{
	partial class Player
	{
		/// fun class for fucking a held cubi

		private int BoinkHeld()
		{
			MapTile here = Refs.m.TileByLoc(loc);
			Cubi partner = Harem.GetId(heldCubiId);

			AddHorny(5);
			partner.AddHorny(5);

			// todo consolidating orgasms.
			// todo more / better descriptions.
			int timepass = 0;
			if (GetHorny() > 15 && partner.GetHorny() > 15) // cumming together
			{
				Announcer.Say("Ohhh Yes! Yes! *splurt* (together)", partner.myAlign, partner.myColor);
				Announcer.Say("Awwww yeah! *splurt* (together)", myAlign, myColor);
				timepass = 8;

				MainMap.SplurtNectar(here, partner.myIdNo);
				MainMap.SplurtNectar(here, myIndex: 0);

				partner.AddHorny(-10);
				AddHorny(-10);
			}
			else if (partner.GetHorny() > 15) // partner only orgasm
			{
				Announcer.Say("Ohhh Yes! Yes! *splurt*", partner.myAlign, partner.myColor);
				timepass = 1;
				MainMap.SplurtNectar(here, partner.myIdNo);
				partner.AddHorny(-10);
			}
			else if (GetHorny() > 15) // player only orgasm
			{
				Announcer.Say("Awwww yeah! *splurt*", myAlign, myColor);
				timepass = 5;
				MainMap.SplurtNectar(here, myIndex: 0);
				AddHorny(-10);
			}
			else // nobody cums (yet)
			{
				Announcer.Say("Loud boinking noises...", myAlign, myColor);
				timepass = 1;
			}

			return timepass;
		}
	}
}