using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Beehive2
{
	internal class VisAI
	{
		// alternate version to be found in Player, as menus can't use D0 to D5 keys
		public static void VisFlowFromMenuBar(int pick)
		{
			Refs.p.viewFlow = pick;
			Refs.m.RenderMapAll();
		}
	}
}