using SadConsole;
using Microsoft.Xna.Framework;
using Console = SadConsole.Console;

namespace Beehive2
{
	public static class Program
	{
		private static void Main(string[] args)
		{
			SadConsole.Game.Create(80, 25);

			// Hook the start event so we can add consoles to the system.
			SadConsole.Game.OnInitialize = Init;

			// Start the game.
			SadConsole.Game.Instance.Run();
			SadConsole.Game.Instance.Dispose();
		}

		private static void Init()
		{
			Global.CurrentScreen = new MapScreen();
			Global.CurrentScreen.IsFocused = true;
		}

		private static void Console_MouseMove(object sender, SadConsole.Input.MouseEventArgs e)
		{
			var console = (Console)sender;

			console.Print(1, 1, $"Mouse moving at {e.MouseState.CellPosition}          ");

			if (e.MouseState.Mouse.LeftButtonDown)
				console.Print(1, 2, $"Left button is down");
			else
				console.Print(1, 2, $"                   ");
		}
	}
}