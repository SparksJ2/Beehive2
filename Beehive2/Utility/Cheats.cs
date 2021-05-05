using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Beehive2
{
	internal class Cheats
	{
		public static void ClearNectar()
		{
			Announcer.Announce("Cheat: clearing nectar...", Refs.p.myAlign, Refs.p.myColor);
			Refs.m.ClearNectar();
			Refs.main.UpdateMap();
		}

		public static void TopOffEnergy()
		{
			Announcer.Announce("Cheat: topped off cubi jump energy...",
				Refs.p.myAlign, Refs.p.myColor);
			Refs.h.MaxJumpEnergy();
		}
	}
}