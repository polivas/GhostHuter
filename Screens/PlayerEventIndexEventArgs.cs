using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace GhosterHunter.Screens
{

    public class PlayerIndexEventArgs : EventArgs
    {
        public PlayerIndex PlayerIndex { get; }

        public PlayerIndexEventArgs(PlayerIndex playerIndex)
        {
            PlayerIndex = playerIndex;
        }
    }
}