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
			// create the window
			SadConsole.Game.Create(Refs.width, Refs.height);

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
			// just one console in this window for now
			var console = new Console(Refs.width, Refs.height);

			// change font
			FontMaster fm = Global.LoadFont("fonts/Cheepicus12.font");
			Font cheep = fm.GetFont(Font.FontSizes.One);
			console.Font = cheep;
			// fix size after font change
			cheep.ResizeGraphicsDeviceManager(SadConsole.Global.GraphicsDeviceManager, Refs.width, Refs.height, 0, 0);
			Global.ResetRendering();

			// console setup
			console.IsFocused = true;
			console.Cursor.IsVisible = false;
			console.Components.Add(new MyKeyboardComponent());

			// link to game window
			Global.CurrentScreen = console; // buffer swap here?

			// go to game logic startup
			Refs.con = console;
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