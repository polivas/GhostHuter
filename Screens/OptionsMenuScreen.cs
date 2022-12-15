using System;
using System.Collections.Generic;
using System.Text;

namespace GhostHunter.Screens
{
    public class OptionsMenuScreen : MenuScreen
    {

        private readonly MenuEntry _musicMuted;
  

        private static bool _muted = false;


        public OptionsMenuScreen() : base("Options")
        {
            _musicMuted = new MenuEntry(string.Empty);        
            SetMenuEntryText();

            var back = new MenuEntry("Back");

            _musicMuted.Selected += UnmuteMenuEntrySelected;

            back.Selected += OnCancel;

            MenuEntries.Add(_musicMuted);

            MenuEntries.Add(back);
        }

        private void SetMenuEntryText()
        {
            _musicMuted.Text = $"Mute Music : {_muted}";

        }

        private void UnmuteMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            if (_muted == false)
            {
                ScreenManager.MuteMusic();
                _muted = true;
            }else if(_muted == true)
            {
                ScreenManager.UnmuteMusic();
                _muted = false;
            }

            SetMenuEntryText();
        }
    }
}