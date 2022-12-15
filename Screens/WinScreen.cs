using System;
using System.Collections.Generic;
using System.Text;
using GhostHunter.StateManagement;

namespace GhostHunter.Screens
{

    public class WinScreen : MenuScreen
    {
        public WinScreen() : base("Win")
        {
            var quitGameMenuEntry = new MenuEntry("Quit Game");

            quitGameMenuEntry.Selected += QuitGameMenuEntrySelected;
            MenuEntries.Add(quitGameMenuEntry);
        }

        private void QuitGameMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            const string message = "Are you sure you want to return to the menu?";
            var confirmQuitMessageBox = new MessageBoxScreen(message);

            confirmQuitMessageBox.Accepted += ConfirmQuitMessageBoxAccepted;

            ScreenManager.AddScreen(confirmQuitMessageBox, ControllingPlayer);
        }

        private void ConfirmQuitMessageBoxAccepted(object sender, PlayerIndexEventArgs e)
        {
            LoadingScreen.Load(ScreenManager, false, null, new BackgroundScreen(), new MainMenuScreen());
        }
    }
}