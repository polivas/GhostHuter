using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using tainicom.Aether.Physics2D.Dynamics;
using tainicom.Aether.Physics2D.Collision;
using tainicom.Aether.Physics2D.Common;

using GhosterHunter.StateManagement;
using GhosterHunter.Screens.Content;

namespace GhosterHunter.Screens
{
    public class GameScreen : StateManagement.GameScreen
    {
        private ContentManager _content;
        private SpriteFont _gameFont;

        private float _pauseAlpha;
        private readonly InputAction _pauseAction;

        public Player _player;

        /// <summary>
        /// Enemeies in the game
        /// </summary>
        public List<Enemy> _enemies;

        /// <summary>
        /// Game Background
        /// </summary>
        private Background _background;

        /// <summary>
        /// The game world
        /// </summary>
        private World world;


        public Vector2 Position { get; set; }
        public Vector2 Velocity { get; set; }



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
            //World Creation
            world = new World();
            world.Gravity = Vector2.Zero;


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

            //Create Enemies
            System.Random random = new System.Random();
            _enemies = new List<Enemy>();

            for (int i = 0; i < 5; i++) // Creates 5 enemies
            {
                var radius = 3;
                var position = new Vector2(
                    random.Next(radius, Constants.GAME_MAX_WIDTH - radius),
                    random.Next(radius, Constants.GAME_MAX_HEIGHT - radius)
                    );

                //Adding rigid body
                var body = world.CreateCircle(radius, 1, position, BodyType.Dynamic);
                body.LinearVelocity = new Vector2(
                    random.Next(-20, 20),
                    random.Next(-20, 20)
                    );

                body.SetRestitution(1);
                body.AngularVelocity = (float)random.NextDouble() * MathHelper.Pi - MathHelper.PiOver2;

                _enemies.Add(new Enemy(position, 150, radius, body));
            }

            //Creates  player
            Vector2 pos = new Vector2(105, 393);
            var body2 = world.CreateCircle(3, 1, pos, BodyType.Dynamic);
            _player = new Player(pos,body2);


            _background = new Background();
        }

        // Load graphics content
        public override void Activate()
        {
            if (_content == null)
                _content = new ContentManager(ScreenManager.Game.Services, "Content");


            _gameFont = _content.Load<SpriteFont>("gamefont");
            _player.LoadContent(_content);
            foreach (var e in _enemies) e.LoadContent(_content);
            _background.LoadContent(_content);

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

            _player.Update(gameTime);
            _background.Update(gameTime, _player, _enemies);

            world.Step((float)gameTime.ElapsedGameTime.TotalSeconds);


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

        }

        public override void Draw(GameTime gameTime)
        {
            ScreenManager.GraphicsDevice.Clear(ClearOptions.Target, Color.CornflowerBlue, 0, 0);
            var spriteBatch = ScreenManager.SpriteBatch;


            _background.Draw(gameTime, spriteBatch, _player, _enemies);




            spriteBatch.Begin();

            Vector2 pos = new Vector2(200, 0);
            Vector2 posHealth = new Vector2(5, 0);

            string text;
            text = $"Find the Spirit ";

            spriteBatch.End();
            if (TransitionPosition > 0 || _pauseAlpha > 0)
            {
                float alpha = MathHelper.Lerp(1f - TransitionAlpha, 1f, _pauseAlpha / 2);

                ScreenManager.FadeBackBufferToBlack(alpha);
            }
        }
    }
}