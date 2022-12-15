using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;



namespace GhostHunter.Screens.Content
{
    public class Arrow : Sprite, ICollidable
    {
        private float _timer;

        public float LifeSpan { get; set; }

        public Vector2 Velocity { get; set; }

        public Arrow(Texture2D texture)
          : base(texture)
        {

        }

        public override void Update(GameTime gameTime)
        {
            _timer += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (_timer >= LifeSpan)
                IsRemoved = true;

            Position += Velocity;
        }

        public void OnCollide(Sprite sprite)
        {

            if (sprite is Arrow || sprite is Orb)
                return;

            if (sprite is Enemy && this.Parent is Enemy)
                return;

            if (sprite is Player && ((Player)sprite).IsDead)
                return;

            if (sprite is Enemy && this.Parent is Player)
            {
                IsRemoved = true;
            }

            if (sprite is Player && this.Parent is Enemy)
            {
                IsRemoved = true;
            }

        }
    }
}
