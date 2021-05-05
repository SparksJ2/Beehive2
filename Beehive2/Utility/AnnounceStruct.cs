using System.Drawing;
using Microsoft.Xna.Framework;

namespace Beehive2
{
	internal class AnnounceStruct
	{
		/// convenient structure for holding text strings that have color and
		///    left / right alignment

		public string say;
		public string align;
		public Color color;

		public AnnounceStruct(string sayIn, string alignIn, Color colIn)
		{
			say = sayIn;
			align = alignIn;
			color = colIn;
		}
	}
}