﻿using System;
using System.Collections.Generic;
using System.Text;
using GhosterHunter.StateManagement;

namespace GhosterHunter.Screens
{

    public class DeathScreen : MenuScreen
    {
        public DeathScreen() : base("Game Over")
        {
            var quitGameMenuEntry = new MenuEntry("Quit Game");

            quitGameMenuEntry.Selected += QuitGameMenuEntrySelected;
            MenuEntries.Add(quitGameMenuEntry);
        }

        private void QuitGameMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            const string message = "Are you sure you want to quit this game?";
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