using SadConsole.Components;
using SadConsole.Input;
using Microsoft.Xna.Framework;

namespace Beehive2
{
	internal class MyMouseComponent : MouseConsoleComponent
	{
		public override void ProcessMouse(SadConsole.Console console, MouseConsoleState state, out bool handled)
		{
			//if (state.IsOnConsole)
			//	console.SetBackground(
			//	state.CellPosition.X, state.CellPosition.Y,
			//	Color.White.GetRandomColor(SadConsole.Global.Random));

			handled = false;
		}
	}
}