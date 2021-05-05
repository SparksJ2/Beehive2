using System;
using System.Diagnostics;
using System.Threading;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Microsoft.Xna.Framework;

namespace Beehive2
{
	public class MainForm
	{
		// TODO menu stuff needs a new sadconsole menu

		//private void HelpPopupEvent(object sender, EventArgs e) => HelpPopup();

		private void MenuSaveEvent(object sender, EventArgs e) => LoadAndSave.SaveGame();

		private void MenuLoadEvent(object sender, EventArgs e) => LoadAndSave.LoadGame();

		private void MenuClearNectarEvent(object sender, EventArgs e) => Cheats.ClearNectar();

		private void MenuTopOffEnergyEvent(object sender, EventArgs e) => Cheats.TopOffEnergy();

		// inelegant but there will always be 4 only, so...
		private void MenuVisAIPlayer(object sender, EventArgs e) => VisAI.VisFlowFromMenuBar(0);

		private void MenuVisAIOne(object sender, EventArgs e) => VisAI.VisFlowFromMenuBar(1);

		private void MenuVisAITwo(object sender, EventArgs e) => VisAI.VisFlowFromMenuBar(2);

		private void MenuVisAIThree(object sender, EventArgs e) => VisAI.VisFlowFromMenuBar(3);

		private void MenuVisAIFour(object sender, EventArgs e) => VisAI.VisFlowFromMenuBar(4);

		private List<AnnounceStruct> annLines;

		private void inventoryLabel_Click(object sender, EventArgs e)
		{
		}
	}
}