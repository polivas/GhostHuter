using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using tainicom.Aether.Physics2D.Dynamics;
using tainicom.Aether.Physics2D.Dynamics.Contacts;


namespace GhosterHunter.Screens.Content
{
    /// <summary>
    ///  Texture states for the given spritesheet, used for animation.
    /// </summary>
    public enum ActionMode
    {
        Idle = 0,
        Right = 0,
        Left = 0,
    }

    public class Enemy
    {
        bool right;

        float playerDistance;

        public Body body;
        public float radius;

        float scale;
        Vector2 orgin;

        /// <summary>
        /// Timer for animation sequence
        /// </summary>
        private double animationTimer;

        /// <summary>
        /// Frame for animations
        /// </summary>
        private short animationFrame = 0;

        /// <summary>
        /// Boolean if enemy is dead
        /// </summary>
        private bool _dead;

        /// <summary>
        /// Position of enemy
        /// </summary>
        private Vector2 _position;

        /// <summary>
        /// Enemys velocity
        /// </summary>
        Vector2 _velocity;

        /// <summary>
        /// The current position of the enemy
        /// </summary>
        public Vector2 Position => _position;

        /// <summary>
        /// Enemys current distance from player.
        /// </summary>
        private float _distance;

        /// <summary>
        /// The previous distance from the player.
        /// </summary>
        private float _oldDistance;

        /// <summary>
        /// The enemy texture given
        /// </summary>
        private Texture2D _texture;

        /// <summary>
        /// Action State of enemy
        /// </summary>
        public ActionMode ActionMode;

        /// <summary>
        /// A boolean indicating if this enemy is colliding with an object in world
        /// </summary>
        public bool Colliding { get; protected set; }

        /// <summary>
        /// Checks if the enemy is flipped
        /// </summary>
        public bool Flipped;


        /// <summary>
        /// Enemy Health information 
        /// </summary>

        private int maxHealth = 5;
        private int currentHealth;

        public Enemy(Vector2 newPosition, float newDistance, float radius, Body body)
        {
            this.body = body;
            this.radius = radius;
            scale = 1;
            orgin = new Vector2(5, 5);
            this.body.OnCollision += CollisionHandler;

;
            _position = newPosition;
            _distance = newDistance;

            _oldDistance = _distance;
        }

        /// <summary>
        /// Updates the enemys sprite to float 
        /// </summary>
        /// <param name="gameTime"></param>
        public void Update(GameTime gameTime, Player player)
        {
            _position += _velocity;

            orgin = new Vector2(16 / 2, 16 / 2);

            if (_distance <= 0)
            {
                right = true;
            }
            else if (_distance >= _oldDistance)
            {
                right = false;
                _velocity.X = -1f;
            }

            if (right) _distance += 1; else _distance -= 1;

            playerDistance = (player.Position.X - _position.X);
            playerDistance += 150;

            if (playerDistance >= -200 & playerDistance <= 200)
            {
                if (playerDistance < -5)
                {
                    _velocity.X = -1f;
                    ActionMode = ActionMode.Left;
                }
                else if (playerDistance > 5 )
                {
                    _velocity.X = 1f;
                    ActionMode = ActionMode.Right;
                }

            }
        }


        /// <summary>
        /// Draws the animated sprite
        /// </summary>
        /// <param name="gametime">The game time</param>
        /// <param name="spriteBatch">The spritebatch to draw with</param>
        public void Draw(GameTime gametime, SpriteBatch spriteBatch)
        {


            if (Colliding == false && _dead == false)
            {
                //Update animation Timer
                animationTimer += gametime.ElapsedGameTime.TotalSeconds;

                //Update animation frame
                if (animationTimer > 0.3)
                {
                    animationFrame++;
                    if (animationFrame > 2) animationFrame = 0;
                    animationTimer -= 0.3;
                }

                if (this.body.LinearVelocity.X > 0)
                {
                    Flipped = false; //Going right
                }
                if (this.body.LinearVelocity.X < 0)
                {
                    Flipped = true; //Going left
                }


                //Draw the sprite
                var source = new Rectangle(animationFrame * 16, 0, 16, 16);

                SpriteEffects spriteEffects = (Flipped) ? SpriteEffects.FlipHorizontally : SpriteEffects.None;


                spriteBatch.Draw(_texture, body.Position, source, Color.White, 0f, orgin, scale, spriteEffects, 0);

            }
        }


        /// <summary>
        /// Loads the enemies texture
        /// </summary>
        /// <param name="contentManager">The content manager to use</param>
        public void LoadContent(ContentManager contentManager)
        {
            _texture = contentManager.Load<Texture2D>("ghost");
        }



        /// <summary>
        /// Collision Handler for the enemy class, handles any kind of collision in the created world
        /// </summary>
        /// <param name="fixture"></param>
        /// <param name="other"></param>
        /// <param name="contact"></param>
        /// <returns></returns>
        bool CollisionHandler(Fixture fixture, Fixture other, Contact contact)
        {
            if (other.Body.BodyType == BodyType.Dynamic)
            {
                Colliding = true;
                currentHealth -= 1;
                return true;
            }
            if (other.Body.BodyType == BodyType.Static)
            {
                Colliding = true;
                return true;
            }

            return false;
        }
    }
}
