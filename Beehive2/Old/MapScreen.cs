using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using SadConsole;
using SadConsole.Input;
using Console = SadConsole.Console;

namespace Beehive2
{
	internal class MapScreen : ContainerConsole
	{
		public Console MapConsole { get; }

		public Cell PlayerGlyph { get; set; }
		private Cell _playerPositionMapGlyph;
		private Point _playerPosition;

		public Point PlayerPosition
		{
			get => _playerPosition;
			private set
			{
				// Restore map cell
				_playerPositionMapGlyph.CopyAppearanceTo(MapConsole[_playerPosition.X, _playerPosition.Y]);
				// Move player
				_playerPosition = value;
				// Save map cell
				_playerPositionMapGlyph.CopyAppearanceFrom(MapConsole[_playerPosition.X, _playerPosition.Y]);
				// Draw player
				PlayerGlyph.CopyAppearanceTo(MapConsole[_playerPosition.X, _playerPosition.Y]);
				// Redraw the map
				MapConsole.IsDirty = true;
			}
		}

		public MapScreen()
		{
			var mapConsoleWidth = (int)((Global.RenderWidth / Global.FontDefault.Size.X) * 1.0);
			var mapConsoleHeight = (int)((Global.RenderHeight / Global.FontDefault.Size.Y) * 1.0);

			// Setup map
			MapConsole = new Console(mapConsoleWidth, mapConsoleHeight);
			MapConsole.DrawBox(new Microsoft.Xna.Framework.Rectangle(0, 0, MapConsole.Width, MapConsole.Height), new Cell(Color.White, Color.DarkGray, 0));
			MapConsole.Parent = this;

			// Setup player
			PlayerGlyph = new Cell(Color.White, Color.Black, 1);
			_playerPosition = new Point(4, 4);
			_playerPositionMapGlyph = new Cell();
			_playerPositionMapGlyph.CopyAppearanceFrom(MapConsole[_playerPosition.X, _playerPosition.Y]);
			PlayerGlyph.CopyAppearanceTo(MapConsole[_playerPosition.X, _playerPosition.Y]);
		}

		public override bool ProcessKeyboard(Keyboard info)
		{
			Point newPlayerPosition = PlayerPosition;

			if (info.IsKeyPressed(Microsoft.Xna.Framework.Input.Keys.Up))
				newPlayerPosition += SadConsole.Directions.North;
			else if (info.IsKeyPressed(Microsoft.Xna.Framework.Input.Keys.Down))
				newPlayerPosition += SadConsole.Directions.South;

			if (info.IsKeyPressed(Microsoft.Xna.Framework.Input.Keys.Left))
				newPlayerPosition += SadConsole.Directions.West;
			else if (info.IsKeyPressed(Microsoft.Xna.Framework.Input.Keys.Right))
				newPlayerPosition += SadConsole.Directions.East;

			if (newPlayerPosition != PlayerPosition)
			{
				PlayerPosition = newPlayerPosition;
				return true;
			}

			return false;
		}
	}
}