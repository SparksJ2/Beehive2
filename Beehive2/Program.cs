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
			FontMaster fm = Global.LoadFont("fonts/Cheepicus12.font");
			//FontMaster fm = Global.LoadFont("fonts/mdcurses16.font");
			Font cheep = fm.GetFont(Font.FontSizes.One);

			var console = new Console(70, 50);
			console.Font = cheep;

			// fix size after font change
			cheep.ResizeGraphicsDeviceManager(SadConsole.Global.GraphicsDeviceManager, 70, 50, 5, 5);
			Global.ResetRendering();

			Refs.con = console;

			console.IsFocused = true;
			console.Cursor.IsVisible = false;
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