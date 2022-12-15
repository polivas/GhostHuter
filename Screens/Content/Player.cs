using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using tainicom.Aether.Physics2D.Dynamics;
using tainicom.Aether.Physics2D.Dynamics.Contacts;
using GhostHunter.Screens.Content.Resources;
//using GhostHunter.Screens.Content.Managers;

using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;

namespace GhostHunter.Screens.Content
{
    /// <summary>
    /// Texture states for the given spritesheet, used for animation.
    /// </summary>
    public enum TextureMode
    {
        Up = 0,
        Right = 1,
        Down = 2,
        Left = 3,
        HitUp = 4,
        HitRight = 5,
        HitDown = 6,
        HitLeft = 7,
        Dead = 14,

    }


    public class Player: Character
    {
        /// <summary>
        /// Player body in the world
        /// </summary>
        public Body body;

        /// <summary>
        /// Sound to be used on melee attack
        /// </summary>
        private SoundEffect swishEffect;

        /// <summary>
        /// the direction of the hunter
        /// </summary>
        public TextureMode TextureMode;

        /// <summary>
        /// Texture of player
        /// </summary>
        private Texture2D texture;

        /// <summary>
        /// If button is currently being pressed, for idle animation
        /// </summary>
        private bool pressing = false;

        /// <summary>
        /// Timer used as clicker for animation of player sprite. Deafult = false
        /// </summary>
        private double animationTimer;

        /// <summary>
        /// Timer used as clicker for animation of player sprite. Deafult = false
        /// </summary>
        private double attackTimer;

        /// <summary>
        /// Timer used as timer for stamina duraation
        /// </summary>
        private double staminaTimer;

        /// <summary>
        /// Current frame in the animation, Deafult = 0
        /// </summary>
        private short animationFrame = 0;


        /// <summary>
        /// poition of the hunter
        /// </summary>
        public Vector2 Position;

        /// <summary>
        /// Checks if the hunter is flipped, to flip the drawn sprite
        /// </summary>
        public bool Flipped;

        public bool isAttacking;

        /// <summary>
        /// Player Health information 
        /// </summary>
        private int _health;

        /// <summary>
        /// current stamina of player
        /// </summary>
        private int _stamina;

        public Score Score { get; set; }


        /// <summary>
        /// A boolean indicating if this enemy is colliding with an object in world
        /// </summary>
        public bool Colliding
        {
            get; protected set;
        }

        /// <summary>
        /// Previous MouseState
        /// </summary>
        MouseState _priorMouse;

        /// <summary>
        /// Swish Attack compents------------- Unable to implement in time
        /// </summary>
        private Vector2 swishPositon;
        private SpriteEffects se;

        //----------------------------------------CLEAN ABOVE

        public List<Point> newLevels = new List<Point>();
        public int experiencePoints;


        /// <summary>
        /// Keeps track of the death of player
        /// </summary>
        public bool IsDead
        {
            get
            {
                return _health <= 0;
            }
        }

        //public Player(Vector2 position, Body body , double health, double stamina)
        public Player(Texture2D texture, Body body, Vector2 position, int health)
              : base(texture, body, position, health)
        {
            this.Position = position;
            this.body = body;
            this.body.OnCollision += CollisionHandler;
            this.texture = texture;

            this._health = (int) health;
            this._stamina = 100;

        }

        /// <summary>
        /// Loads the sprite texture using the provided ContentManager
        /// </summary>
        /// <param name="content">The ContentManager to load with</param>
        public void LoadContent(ContentManager content)
        {            
            swishEffect = content.Load<SoundEffect>("melee sound");
        }


        /// <summary>
        /// Updates the Player sprite
        /// </summary>
        /// <param name="gameTime">Game time</param>
        public override void Update(GameTime gameTime)
        {
            this.body.Position = Position;

            var currentMouseState = Mouse.GetState();
            var mousePosition = new Point(currentMouseState.X, currentMouseState.Y);

            var keyboardState = Keyboard.GetState();
            float t = (float)gameTime.ElapsedGameTime.TotalSeconds;

            pressing = false;
            isAttacking = false;

            //Basic Movement for player

            if (keyboardState.IsKeyDown(Keys.Up) || keyboardState.IsKeyDown(Keys.W))
            {
                Position += new Vector2(0, -1);
                TextureMode = TextureMode.Up;
                pressing = true;
            }

            if (keyboardState.IsKeyDown(Keys.Down) || keyboardState.IsKeyDown(Keys.S))
            {
                Position += new Vector2(0, 1);
                TextureMode = TextureMode.Down;
                pressing = true;
            }
            if (keyboardState.IsKeyDown(Keys.Left) || keyboardState.IsKeyDown(Keys.A))
            {
                Position += new Vector2(-1, 0);
                TextureMode = TextureMode.Left;
                Flipped = true;
                pressing = true;
            }

            if (keyboardState.IsKeyDown(Keys.Right) || keyboardState.IsKeyDown(Keys.D))
            {
                Position += new Vector2(1, 0);
                TextureMode = TextureMode.Right;
                Flipped = false;
                pressing = true;
            }

            //Running Speed --------- Implement stamina

            if (keyboardState.IsKeyDown(Keys.Space) && (keyboardState.IsKeyDown(Keys.Right) || keyboardState.IsKeyDown(Keys.D)))
            {
                Position.X += 1;
            }
            if (keyboardState.IsKeyDown(Keys.Space) && (keyboardState.IsKeyDown(Keys.Left) || keyboardState.IsKeyDown(Keys.A)))
            {
                Position.X -= 1;
            }

            if (keyboardState.IsKeyDown(Keys.Space) && (keyboardState.IsKeyDown(Keys.Up) || keyboardState.IsKeyDown(Keys.W)))
            {
                Position.Y -= 1;
            }
            if (keyboardState.IsKeyDown(Keys.Space) && (keyboardState.IsKeyDown(Keys.Down) || keyboardState.IsKeyDown(Keys.S)))
            {
                Position.Y += 1;
            }


            //Swish Effect, may implement as its own      Swish Attack compents------------- Unable to implement in time
            var lastMouseState = currentMouseState;

            Vector2 swoodPos;

            if (currentMouseState.LeftButton == ButtonState.Pressed && _priorMouse.LeftButton == ButtonState.Released)
            {
                swishEffect.Play();
                isAttacking = true;

                if (keyboardState.IsKeyDown(Keys.Right) || keyboardState.IsKeyDown(Keys.D))
                {
                    swishPositon = new Vector2(this.Position.X + 5, this.Position.Y);
                    se = SpriteEffects.None;
                    TextureMode = TextureMode.HitRight;
                    swishEffect.Play();
                }
                if (keyboardState.IsKeyDown(Keys.Left) || keyboardState.IsKeyDown(Keys.A))
                {
                    swishPositon = new Vector2(this.Position.X - 5, this.Position.Y);
                    se = SpriteEffects.FlipHorizontally;
                    TextureMode = TextureMode.HitLeft;
                    swishEffect.Play();
                }
                if ((keyboardState.IsKeyDown(Keys.Up) || keyboardState.IsKeyDown(Keys.W)))
                {
                    swishPositon = new Vector2(this.Position.X, this.Position.Y - 5);
                    se = SpriteEffects.FlipVertically;
                    TextureMode = TextureMode.HitUp;
                    swishEffect.Play();
                }
                if ((keyboardState.IsKeyDown(Keys.Down) || keyboardState.IsKeyDown(Keys.S)))
                {
                    swishPositon = new Vector2(this.Position.X, this.Position.Y + 5);
                    se = SpriteEffects.FlipVertically;
                    TextureMode = TextureMode.HitDown;
                    swishEffect.Play();
                }

            }

        }


        /// <summary>
        /// Draws the sprite using the supplied SpriteBatch
        /// </summary>
        /// <param name="gameTime">The game time</param>
        /// <param name="spriteBatch">The spritebatch to render with</param>
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {


            //Update animation Timer
            animationTimer += gameTime.ElapsedGameTime.TotalSeconds;

            //Update animation frame
            if (animationTimer > 0.5)
            {
                animationFrame++;
                if (animationFrame > 1) animationFrame = 0;
                animationTimer -= 0.5;
            }
            if (animationTimer > 0.5) animationTimer -= 0.5;

            var source = new Rectangle(animationFrame * 16, (int)TextureMode * 16, 16, 16);

            if (!pressing)
            {
                source = new Rectangle(animationFrame * 16, 32, 16, 16);
                spriteBatch.Draw(texture, Position, source, Color.White, 0f, new Vector2(16, 16), 1.5f, SpriteEffects.None, 0);
            }
            else
            {
                spriteBatch.Draw(texture, Position, source, Color.White, 0f, new Vector2(16, 16), 1.5f, SpriteEffects.None, 0);

            }

            Vector2 pos = new Vector2(Position.X, Position.Y - 50);



        }

        /// <summary>
        /// Collision Handler for the player class, handles any kind of collision in the created world
        /// </summary>
        /// <param name="fixture"></param>
        /// <param name="other"></param>
        /// <param name="contact"></param>
        /// <returns></returns>
        bool CollisionHandler(Fixture fixture, Fixture other, Contact contact)
        {
            Vector2 temp = other.Body.LinearVelocity;

            if (other.Body.BodyType == BodyType.Dynamic)
            {
                Colliding = true;
               // _health -= 1;
                return true;
            }

            if (other.Body.BodyType == BodyType.Static)
            {
                Colliding = true;
                return true;
            }

            return false;
        }

        public override void OnCollide(Sprite sprite)
        {
            if (IsDead)
                return;

            if (sprite is Melee && ((Melee)sprite).Parent is Enemy)
                Health--;

            if (sprite is Arrow && ((Arrow)sprite).Parent is Enemy)
                Health--;

            if (sprite is Enemy)
                Health -= 3;
        }

        /// <summary>
        /// Calculated how much EX the player should recieve
        /// </summary>
        /// <param name="howMuch">How Much EX gained</param>
        public void gainExperience(int howMuch)
        {
            if (howMuch <= 0)
                return;
            int num1 = Player.checkForLevelGain(this.experiencePoints, this.experiencePoints + howMuch);
        }

        public static int checkForLevelGain(int oldXP, int newXP)
        {
            int i = 1;

            return i;
        }

    }    
}
