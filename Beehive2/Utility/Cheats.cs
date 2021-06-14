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
			Announcer.Say("Cheat: clearing nectar...", Refs.p.myAlign, Refs.p.myColor);
			Refs.m.ClearNectar();
			Refs.m.RenderMapAll();
		}

		public static void TopOffEnergy()
		{
			Announcer.Say("Cheat: topped off cubi jump energy...",
				Refs.p.myAlign, Refs.p.myColor);
			Refs.h.MaxJumpEnergy();
		}
	}
}