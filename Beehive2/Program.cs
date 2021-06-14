using System.Windows.Forms;
using System;
using SadConsole;
using Microsoft.Xna.Framework;
using Console = SadConsole.Console;

namespace Beehive2
{
	public static class Program
	{
#pragma warning disable IDE0060 // Remove unused parameter

		private static void Main(string[] args)
#pragma warning restore IDE0060
		{
			SadConsole.Game.Create(70, 50);

			// Hook the events.
			SadConsole.Game.OnInitialize = Init;
			//SadConsole.Game.OnDraw = Draw;
			//SadConsole.Game.OnUpdate = Update;

			// Start the game.
			SadConsole.Game.Instance.Run();

			// Only happens after game end.
			SadConsole.Game.Instance.Dispose();
		}

		private static void Init()
		{
			var console = new Console(70, 50);
			Refs.con = console;

			console.IsFocused = true;
			console.Cursor.IsVisible = true;
			console.Components.Add(new MyKeyboardComponent());

			console.FillWithRandomGarbage();
			Global.CurrentScreen = console; // buffer swap here?

			Refs.main = new Main();
			Refs.main.Show();
		}

		//private static void Draw(GameTime time)
		//{
		//	// Called after a frame has been drawn.
		//}

		//private static void Update(GameTime time)
		//{
		//	// Called after each frame of logic update.
		//}
	}
}