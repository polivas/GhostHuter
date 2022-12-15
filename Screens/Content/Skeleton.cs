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
    public class Skeleton : Enemy
    {
        public int Health { get; set; }
        public float Speed;


        public float HitTimer = 1.75f;


        public Skeleton(Texture2D texture, Body body, Vector2 position, int health)
            : base(texture, body, position, health)
        {
            Speed = 2f;
        }

        public virtual void OnCollide(Sprite sprite)
        {
            throw new NotImplementedException();
        }
    }
}
