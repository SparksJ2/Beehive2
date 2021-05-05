using Microsoft.Xna.Framework;
using SadConsole.Components;
using SadConsole.Input;

namespace Beehive2
{
	internal class MyKeyboardComponent : KeyboardConsoleComponent
	{
		public override void ProcessKeyboard(SadConsole.Console console, Keyboard info, out bool handled)
		{
			//if (info.IsKeyPressed(Microsoft.Xna.Framework.Input.Keys.Space))
			//{
			//	console.DefaultBackground = Color.White.GetRandomColor(SadConsole.Global.Random);
			//	console.Clear();
			//}

			Refs.p.HandlePlayerInput(console, info, out  handled);
			handled = true;
		}
	}
}