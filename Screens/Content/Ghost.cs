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
    public class Ghost : Enemy
    {

        public float ShootingTimer = 1.75f;

        public Ghost(Texture2D texture, Body body, Vector2 position, int health)
            : base(texture, body, position, health)
        {
        }
    }
}
