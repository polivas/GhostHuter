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
    public class Character : Sprite, ICollidable
    {
        public int Health { get; set; }

        public int Stamina { get; set; }

        public Melee Melee { get; set; }

        public Arrow Arrow { get; set; }

        public Orb Orb { get; set; }

        public float Speed;

        public Character(Texture2D texture, Body body, Vector2 position, int health) 
            : base(texture)
        {
        }

        protected void Hit(Vector2 direction, Vector2 position)
        {
            var melee = Melee.Clone() as Melee;
            melee.Position = position;
            melee.Colour = this.Colour;
            melee.Layer = 0.1f;
            melee.LifeSpan = 0.5f;
            melee.Parent = this;

            Children.Add(melee);
        }

        protected void ShootArrow(Vector2 direction, Vector2 position, float speed)
        {
            var arrow = Arrow.Clone() as Arrow;
            arrow.Position = this.Position;
            arrow.Colour = this.Colour;
            arrow.Layer = 0.1f;
            arrow.LifeSpan = 5f;
            arrow.Velocity = new Vector2(speed, 0f);
            arrow.Parent = this;

            Children.Add(arrow);
        }

        protected void ShootOrb(Vector2 direction, Vector2 position, float speed)
        {
            var orb = Orb.Clone() as Orb;
            orb.Position = position;
            orb.Colour = this.Colour;
            orb.Layer = 0.1f;
            orb.LifeSpan = 0.5f;
            orb.Velocity = new Vector2(speed, 0f);

            orb.Parent = this;

            Children.Add(orb);
        }


        public virtual void OnCollide(Sprite sprite)
        {
            throw new NotImplementedException();
        }
    }
}
