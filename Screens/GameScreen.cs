using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using tainicom.Aether.Physics2D.Dynamics;
using tainicom.Aether.Physics2D.Collision;
using tainicom.Aether.Physics2D.Common;

using GhostHunter.StateManagement;
using GhostHunter.Screens.Content;
using GhostHunter.Screens.Content.Managers;


namespace GhostHunter.Screens.Content
{
    public class GameScreen : StateManagement.GameScreen
    {
        private ContentManager _content;
        private EnemyManager _enemyManager;

        private SpriteFont _gameFont;
        private float _pauseAlpha;
        private readonly InputAction _pauseAction;

        public static Random random = new Random(DateTime.Now.Millisecond);

        public static Random Random;

        /// <summary>
        /// The Overall game player
        /// </summary>

        public Player _player;
        public List<Enemy> _enemies;

        private List<Sprite> _sprites;

        public Vector2 CurrentPlayerPosition;

        private Texture2D _playerTexture;

        /// <summary>
        /// Enemeies in the game
        /// </summary>


        /// <summary>
        /// Game Background
        /// </summary>
        private Background _background;

        /// <summary>
        /// Health and stammina stuff
        /// </summary>
        private Texture2D _healthBar;
        public Rectangle healthBar;

        private Texture2D _staminaBar;
        public Rectangle staminaBar;

        /// <summary>
        /// The game world
        /// </summary>
        private World world;

        public static int tileSize = 16;

        Texture2D healthTexture;
        Rectangle healthRectangle;

        double healthMultiplier;
        double staminaMultiplier;



        public GameScreen()
        {
            TransitionOnTime = TimeSpan.FromSeconds(1.5);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);
            Initialize();


            _pauseAction = new InputAction(
                new[] { Buttons.Start, Buttons.Back },
                new[] { Keys.Back, Keys.Escape }, true);
        }

        /// <summary>
        /// Initilizes game
        /// </summary>
        public void Initialize()
        {

            System.Random rand = new System.Random();
            Random = new Random();

            //World Creation
            world = new World();
            world.Gravity = Vector2.Zero;

            ///Goes up
            healthMultiplier = 1.0;
            staminaMultiplier = 1.0;

            var top = 0;
            var bottom = Constants.GAME_MAX_HEIGHT;
            var left = 0;
            var right = Constants.GAME_MAX_WIDTH;

            var edges = new Body[]{
                world.CreateEdge(new Vector2(left, top), new Vector2(right, top)),
                world.CreateEdge(new Vector2(left, top), new Vector2(left, bottom)),
                world.CreateEdge(new Vector2(left, bottom), new Vector2(right, bottom)),
                world.CreateEdge(new Vector2(right, top), new Vector2(right, bottom))
             };

            foreach (var edge in edges)
            {
                edge.BodyType = BodyType.Static;
                edge.SetRestitution(1.0f);
            }

            

            /*** Create Enemies
            System.Random random = new System.Random();
            _enemies = new List<Enemy>();

            for (int i = 0; i < 5; i++) // Creates 5 enemies
            {
                var radius = 5;
                var position = new Vector2(
                    random.Next(radius, Constants.GAME_MAX_WIDTH - radius),
                    random.Next(radius, Constants.GAME_MAX_HEIGHT - radius)
                    );

                //Adding rigid body ---------------EDITING

                Rectangle enemyBody = new Rectangle(position.X,position.Y,16,16)

                var body = world.CreateCircle(radius, 1, position, BodyType.Dynamic);
                body.LinearVelocity = new Vector2(
                    random.Next(-20, 20),
                    random.Next(-20, 20)
                    );

                body.SetRestitution(1);
                body.AngularVelocity = (float)random.NextDouble() * MathHelper.Pi - MathHelper.PiOver2;

                _enemies.Add(new Enemy(position, 150, radius, body));
            }
            enemiesAlive = _enemies.Count;
         */

            //create eneimes


            //Creates  player
 //           Vector2 pos = new Vector2(105, 393);
 //           var body2 = world.CreateRectangle(16, 16, 1, pos, 0, BodyType.Dynamic);

            //Texture2D texture, Body body, Vector2 position, int health

            //_player = new Player(_playerTexture,body2, pos, 100);


            _background = new Background();
        }

        // Load graphics content AKA Load Content
        public override void Activate()
        {
            if (_content == null)
                _content = new ContentManager(ScreenManager.Game.Services, "Content");


            _gameFont = _content.Load<SpriteFont>("gamefont");

            _healthBar = _content.Load<Texture2D>("healthbar");
            _staminaBar = _content.Load<Texture2D>("staminahbar");

            _playerTexture = _content.Load<Texture2D>("cloakandleather");


            _player.LoadContent(_content);
            //  foreach (var e in _enemies) e.LoadContent(_content);
            _background.LoadContent(_content);

            var meleeTexture = _content.Load<Texture2D>("swoosh");
            var orbTexture = _content.Load<Texture2D>("Orb");

            var meleePrefab = new Melee(meleeTexture);
            var orbPrefab = new Orb(orbTexture);

            Vector2 pos = new Vector2(105, 393);
            var body2 = world.CreateRectangle(16, 16, 1, pos, 0, BodyType.Dynamic);

            _sprites.Add(new Player(_playerTexture, body2, pos, 100)
            {
                Colour = Color.Blue,
                Position = new Vector2(100, 50),
                Layer = 0.3f,
                Melee = meleePrefab,
                Health = 100,
                Score = new Resources.Score()
                {
                    Value = 0,
                },
            });

            //_player = _sprites.Where(c => c is Player).Select(c => (Player)c).ToList();

            _enemyManager = new EnemyManager(_content)
            {
                Melee = meleePrefab,
            };

            ScreenManager.Game.ResetElapsedTime();

        }

        public override void Deactivate()
        {
            base.Deactivate();
        }

        public override void Unload()
        {
            _content.Unload();
        }
        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {

//            _player.Update(gameTime);
            _background.Update(gameTime, _player, _enemies);

            healthBar = new Rectangle(50, 20, _player.Health, 20);
            //staminaBar = new Rectangle(50, 20, player.Stamina, 20);

            world.Step((float)gameTime.ElapsedGameTime.TotalSeconds);

            CurrentPlayerPosition = _player.Position;

            foreach (var sprite in _sprites)
                sprite.Update(gameTime);

            _enemyManager.Update(gameTime);
            if (_enemyManager.CanAdd && _sprites.Where(c => c is Enemy).Count() < _enemyManager.MaxEnemies)
            {
                _sprites.Add(_enemyManager.GetEnemy());
            }


            base.Update(gameTime, otherScreenHasFocus, false);

            if (coveredByOtherScreen)
                _pauseAlpha = Math.Min(_pauseAlpha + 1f / 32, 1);
            else
                _pauseAlpha = Math.Max(_pauseAlpha - 1f / 32, 0);
        }

        public override void HandleInput(GameTime gameTime, InputState input)
        {
            if (input == null)
                throw new ArgumentNullException(nameof(input));

            int playerIndex = (int)ControllingPlayer.Value;

            var keyboardState = input.CurrentKeyboardStates[playerIndex];
            var gamePadState = input.CurrentGamePadStates[playerIndex];

            bool gamePadDisconnected = !gamePadState.IsConnected && input.GamePadWasConnected[playerIndex];

            PlayerIndex player;
            if (_pauseAction.Occurred(input, ControllingPlayer, out player) || gamePadDisconnected)
            {
                ScreenManager.AddScreen(new PauseMenuScreen(), ControllingPlayer);
            }

            if (_player.IsDead)
            {
                ScreenManager.AddScreen(new DeathScreen(), ControllingPlayer);
            }


            /***
            if (enemiesAlive == 0) //Round over
            {
                ScreenManager.AddScreen(new WinScreen(), ControllingPlayer); // Chnage to Other but win -------------------------
            }
            ***/
        }

        public override void Draw(GameTime gameTime)
        {
            ScreenManager.GraphicsDevice.Clear(ClearOptions.Target, Color.CornflowerBlue, 0, 0);
            var spriteBatch = ScreenManager.SpriteBatch;

            _background.Draw(gameTime, spriteBatch, _player, _enemies);

            spriteBatch.Begin();

            Vector2 pos = new Vector2(200, 0);
            Vector2 posHealth = new Vector2(5, 0);

            spriteBatch.End();
            if (TransitionPosition > 0 || _pauseAlpha > 0)
            {
                float alpha = MathHelper.Lerp(1f - TransitionAlpha, 1f, _pauseAlpha / 2);
                ScreenManager.FadeBackBufferToBlack(alpha);
            }
        }
    }
}