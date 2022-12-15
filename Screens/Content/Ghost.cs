using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using tainicom.Aether.Physics2D.Dynamics;
using tainicom.Aether.Physics2D.Dynamics.Contacts;

namespace GhostHunter.Screens.Content
{
    public class Ghost //: Enemy
    {

        private int wasHitCounter;
        private float targetRotation;
        private bool turningRight;
        private bool seenPlayer;
        private int yOffset;
        private int yOffsetExtra;


        public Ghost()
        {
        }
/*
        public Ghost(Vector2 position)
          : base(nameof(Ghost), position)
        {
            this.slipperiness = 8;
            this.isGlider = true;
        }
*/
    }
}
