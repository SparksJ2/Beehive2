using Microsoft.Xna.Framework;

namespace Beehive2
{
	public class ColorExtensions
	{
		public static Color GetColByName(string nameOfColor)
		{
			var prop = typeof(Color).GetProperty(nameOfColor);
			if (prop != null)
				return (Color)prop.GetValue(null, null);
			return default(Color);
		}
	}
}