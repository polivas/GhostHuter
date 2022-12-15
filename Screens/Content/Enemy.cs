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

    public class Enemy : Character
    {


     //   public Skeleton Skeleton { get; private set; }

       // public Ghost Ghost { get; private set; }

      //  public Archer Archer { get; private set; }

        /// <summary>
        /// Enemeies body in the world
        /// </summary>
        public Body body;


        /// <summary>
        /// Timer for animation sequence
        /// </summary>
        private double animationTimer;

        /// <summary>
        /// Frame for animations
        /// </summary>
        private short animationFrame = 0;

        public TextureMode TextureMode;

        /// <summary>
        /// Boolean if enemy is dead
        /// </summary>
        public bool _dead
        {
            get
            {
                return _health <= 0;
            }
        }

        /// <summary>
        /// Position of enemy
        /// </summary>
 //       private Vector2 _position;

        /// <summary>
        /// Enemys velocity
        /// </summary>
        Vector2 _velocity;

        /// <summary>
        /// The current position of the enemy
        /// </summary>
//        public Vector2 Position => _position;

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
        private int _health;

        /// <summary>
        /// Current Player in World
        /// </summary>
        private Player currPlayer;

        //-------------------------------------CLEAN UPPER

        private Vector2 movement;


        private bool isWalkingTowardPlayer;

        private float _timer;

        public float DamageCoolDown = 1.75f;

        public Enemy(Texture2D texture, Body body, Vector2 position, int health)
              : base(texture, body, position, health)
        {
            this.body = body;
            this._texture = texture;
            this._position = position;

            this.body.OnCollision += CollisionHandler;
            this._health = health;
        }

        /// <summary>
        /// Updates the Sprites 
        /// </summary>
        /// <param name="gameTime"></param>
        public void Update(GameTime gameTime, Player player)
        {
            _timer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            this.body.Position = Position;

            _position += _velocity;
            Colliding = false;

            currPlayer = player;

            int playerX = (int) currPlayer.Position.X;
            int playerY = (int) currPlayer.Position.Y;

            
            
            if (_timer >= DamageCoolDown && (playerX - Position.X <= 2) && (playerY - Position.Y <= 2))
            {
                _timer = 0;

                this.Hit(currPlayer.Position, this.Position);
            }
            else
            {
                GetToPlayer();
            }

            /*
            if (_timer >= DamageCoolDown)
            {
                _timer = 0;

                //case hit, shoot, jump

            }
            */
        }

        public void GetToPlayer()
        {
            int playerX = (int)currPlayer.Position.X;
            int playerY = (int)currPlayer.Position.Y;

            if(playerX > this.Position.X)
            {
                this.Position += new Vector2(1,0);
                TextureMode = TextureMode.Right;
            }
            else
            {
                this.Position += new Vector2(-1, 0);
                TextureMode = TextureMode.Left;
               // Flipped = true;
            }

            if(playerY > this.Position.Y)
            {
                this.Position += new Vector2(0, -1);
                TextureMode = TextureMode.Up;
            }
            else
            {
                this.Position += new Vector2(0, 1);
                TextureMode = TextureMode.Down;
            }

        }

        public void Draw(GameTime gametime, SpriteBatch spriteBatch)
        {

            if (this.Colliding == false && _dead == false)
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

                /*
                if (this.body.LinearVelocity.X > 0)
                {
                    this.Flipped = false;
                }
                if (this.body.LinearVelocity.X < 0)
                {
                    this.Flipped = true;
                }
                */

                Flipped = false;

                //Draw the sprite
                var source = new Rectangle(animationFrame * 16, 0, 16, 16);

                SpriteEffects spriteEffects = (Flipped) ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
                spriteBatch.Draw(_texture, this.body.Position, source, Color.White, 0f, this.Origin, 1, spriteEffects, 0);
               
            }
        }


       /// <summary>
        /// Loads the enemies texture
        /// </summary>
        /// <param name="contentManager">The content manager to use</param>
        public void LoadContent(ContentManager contentManager)
        {
            this.LoadContent(contentManager);
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
            if ((other.Body.BodyType == BodyType.Dynamic ) )
            {
                Colliding = true; // On Player
                return true;
            }
            if (other.Body.BodyType == BodyType.Static)
            {
                Colliding = true; //On Edge
                return true;
            }

            return false;
        }

        public override void OnCollide(Sprite sprite)
        {
            // If we crash into a player that is still alive
            if (sprite is Player && !((Player)sprite).IsDead)
            {
                ((Player)sprite).Score.Value++;

                // We want to remove the ship completely
                IsRemoved = true;
            }

            // If we hit a bullet that belongs to a player      
            if (sprite is Arrow && ((Arrow)sprite).Parent is Player)
            {
                Health--;

                if (Health <= 0)
                {
                    IsRemoved = true;
                    ((Player)sprite.Parent).Score.Value++;
                }
            }

            if (sprite is Melee && ((Melee)sprite).Parent is Player)
            {
                Health--;

                if (Health <= 0)
                {
                    IsRemoved = true;
                    ((Player)sprite.Parent).Score.Value++;
                }
            }
        }
    }
}
