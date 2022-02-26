using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using tainicom.Aether.Physics2D.Dynamics;
using tainicom.Aether.Physics2D.Dynamics.Contacts;

using Microsoft.Xna.Framework.Audio;

namespace SceenGame.Screens.Content
{
    public class SwordSprite
    {
       // private KeyboardState keyboardState;
        private Texture2D texture;
        private Vector2 flipPos = new Vector2(40, 0);
        float scale;
        private bool flipped;
        Vector2 origin;
        Body body;
        private SoundEffect birdDeath;


        /// <summary>
        /// Current postiion of the sword
        /// </summary>
        public Vector2 Position;

        public bool Colliding { get; protected set; }

        public SwordSprite(Vector2 position, Body body)
        {
            this.body = body;
            this.Position = position;
            scale = 1;
            origin = new Vector2(5, 5);
            this.body.OnCollision += CollisionHandler;
        }

        /// <summary>
        /// Loads the swords's texture
        /// </summary>
        /// <param name="contentManager">The content manager to use</param>
        public void LoadContent(ContentManager contentManager)
        {
            texture = contentManager.Load<Texture2D>("w_longsword");
                         
            birdDeath = contentManager.Load<SoundEffect>("bird");
        }


        /// <summary>
        ///  Updates the swords position relative to hunter and if needed to be flipped.
        /// </summary>
        /// <param name="gameTime">Current gametime</param>
        /// <param name="position">Postion of hunter to stay close</param>
        /// <param name="hunterFlipped">IF hunter sprite is flipped, sword will be flipped</param>
        public void Update(GameTime gameTime, Vector2 position, bool hunterFlipped)
        {
            Colliding = false;

            this.flipped = hunterFlipped;
            this.Position = position;


            if (flipped == true) this.body.Position = position - flipPos;
            else this.body.Position = position;

        }

        /// <summary>
        /// Draws the sword
        /// </summary>
        /// <param name="gametime">time in game</param>
        /// <param name="spriteBatch"> spritebatch avalible for game</param>
        public void Draw(GameTime gametime, SpriteBatch spriteBatch)
        {
            var source = new Rectangle(0 , 0, 32, 32);

            SpriteEffects spriteEffects = (flipped) ? SpriteEffects.FlipHorizontally : SpriteEffects.None;

            if (flipped == true) spriteBatch.Draw(texture, this.Position - flipPos, source, Color.White, 0f, origin, scale, spriteEffects, 0);
            else spriteBatch.Draw(texture, this.Position, source, Color.White, 0f, origin, scale, spriteEffects, 0);
            
        }

        /// <summary>
        /// Is checking if it has collided with an object
        /// </summary>
        /// <param name="fixture">curr obj</param>
        /// <param name="other">colliding obj</param>
        /// <param name="contact"></param>
        /// <returns></returns>
        bool CollisionHandler(Fixture fixture, Fixture other, Contact contact)
        {
            if (other.Body.BodyType == BodyType.Dynamic)
            {
                
                birdDeath.Play();
                Colliding = true;
                return true;
            }

            return false;
        }
    }
}
