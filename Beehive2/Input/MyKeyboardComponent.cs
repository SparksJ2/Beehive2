using SadConsole.Components;
using SadConsole.Input;
using System;
using System.Diagnostics;
using System.Threading;
using System.Windows.Forms;
using Microsoft.Xna.Framework;

namespace Beehive2
{
	internal class MyKeyboardComponent : KeyboardConsoleComponent
	{
		public Stopwatch betweenTurnsTimer;

		public MyKeyboardComponent()
		{
			int i = 1; // for checkpoint
		}

		public override void ProcessKeyboard(SadConsole.Console console, Keyboard info, out bool handled)
		{
			if (info.KeysPressed.Count == 0) // only interested in keypresses
			{
				handled = true;
				return;
			}

			betweenTurnsTimer = new Stopwatch();

			var sw = new Stopwatch(); sw.Start();

			// TODO only report first key potential for issues here
			Console.WriteLine("Key " + info.KeysDown[0].Key + " Pressed");
			Console.WriteLine("Starting new frame.");

			try
			{
				// TODO use proper repeat delays in Keyboard info
				if (betweenTurnsTimer.ElapsedMilliseconds < 300) { handled = true; return; }

				betweenTurnsTimer.Start();

				int timePass = Refs.p.HandlePlayerInput(console, info, out handled);

				Refs.m.HealWalls();
				Console.WriteLine("Finished HealWalls at " + sw.ElapsedMilliseconds + "ms in.");
				//Refs.m.RunLos();
				//Console.WriteLine("Finished RunLos at " + sw.ElapsedMilliseconds + "ms in.");

				FlowMap.RemakeAllFlows();
				Console.WriteLine("Finished RemakeAllFlows at " + sw.ElapsedMilliseconds + "ms in.");

				if (timePass == 0)
				{
					Refs.m.RenderMapAll();
					Console.WriteLine("Finished UpdateMap at " + sw.ElapsedMilliseconds + "ms in.");
				}
				else
				{
					while (timePass > 0)
					{
						// run ai for multiple turns if needed
						foreach (Cubi c in Refs.h.roster) { c.AiMove(); }
						Console.WriteLine("Finished AiMove at " + sw.ElapsedMilliseconds + "ms in.");

						Refs.m.SpreadNectar();
						Refs.m.RenderMapAll();
						Thread.Sleep(75);

						Console.WriteLine("Finished UpdateMap at " + sw.ElapsedMilliseconds + "ms in.");
						timePass--;
						Refs.p.turnCounter++;
					}
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show("Last chance catch of exception " + ex.GetType() +
					" with message " + ex.Message + " at " + ex.Source +
					" with trace " + ex.StackTrace);
			}

			Console.WriteLine("Total time this update = " + sw.ElapsedMilliseconds + "ms. or " +
						1000 / sw.ElapsedMilliseconds + " fps if it mattered.");

			betweenTurnsTimer.Start();

			handled = true;
		}
	}
}