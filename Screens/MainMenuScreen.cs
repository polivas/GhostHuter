using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using SceenGame.StateManagement;




namespace SceenGame.Screens
{
    public class MainMenuScreen : MenuScreen
    {
        public MainMenuScreen() : base("Main  Menu")
        {


            var playGameMenuEntry = new MenuEntry("Play Game");
          //  var watchClip = new MenuEntry("Watch Clip");
            var optionsMenuEntry = new MenuEntry("Options");
            var exitMenuEntry = new MenuEntry("Exit");

            playGameMenuEntry.Selected += PlayGameMenuEntrySelected;
          //  watchClip.Selected += WatchClipSelected;
            optionsMenuEntry.Selected += OptionsMenuEntrySelected;
            exitMenuEntry.Selected += OnCancel;

            MenuEntries.Add(playGameMenuEntry);
         //   MenuEntries.Add(watchClip);
            MenuEntries.Add(optionsMenuEntry);
            MenuEntries.Add(exitMenuEntry);
        }

        private void PlayGameMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            
            LoadingScreen.Load(ScreenManager, true, e.PlayerIndex, new GameplayScreen());
        }

        private void WatchClipSelected(object sender, PlayerIndexEventArgs e)
        {
            LoadingScreen.Load(ScreenManager, true, e.PlayerIndex, new CutSceneScreen());

        }

        private void OptionsMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            ScreenManager.AddScreen(new OptionsMenuScreen(), e.PlayerIndex);
        }

        protected override void OnCancel(PlayerIndex playerIndex)
        {
            const string message = "Want to exit?";
            var confirmExitMessageBox = new MessageBoxScreen(message);

            confirmExitMessageBox.Accepted += ConfirmExitMessageBoxAccepted;

            ScreenManager.AddScreen(confirmExitMessageBox, playerIndex);
        }

        private void ConfirmExitMessageBoxAccepted(object sender, PlayerIndexEventArgs e)
        {
            ScreenManager.Game.Exit();
        }
    }
}
