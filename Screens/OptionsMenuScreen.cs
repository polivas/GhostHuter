using System;
using System.Collections.Generic;
using System.Text;

namespace SceenGame.Screens
{
    public class OptionsMenuScreen : MenuScreen
    {

        private readonly MenuEntry _musicMuted;
        private readonly MenuEntry _languageMenuEntry;


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

        // Fills in the latest values for the options screen menu text.
        private void SetMenuEntryText()
        {
            _musicMuted.Text = $" Menu Music Muted : {_muted}";

        }

        private void UnmuteMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {

            SetMenuEntryText();
        }
    }
}