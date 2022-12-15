using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace GhostHunter.Screens.Content
{
    public class Melee : Sprite, ICollidable
    {
        private float _timer;

        public float LifeSpan { get; set; }



        public Melee(Texture2D texture)
          : base(texture)
        {

        }

        public override void Update(GameTime gameTime)
        {
            _timer += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (_timer >= LifeSpan)
                IsRemoved = true;
        }

        public void OnCollide(Sprite sprite)
        {
            if (sprite is Melee)
                return;

            // Enemies can't hit eachother
            if (sprite is Enemy && this.Parent is Enemy)
                return;

            // Players can't damage eachother
            if (sprite is Player && this.Parent is Player)
                return;

            // Can't hit a player if they're dead
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
