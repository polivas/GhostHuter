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

using SceenGame.StateManagement;
using SceenGame.Screens.Content;

namespace SceenGame.Screens
{
    public class GameplayScreen : GameScreen
    {
        private ContentManager _content;
        private SpriteFont _gameFont;

        private World world;

        // private SpriteBatch spriteBatch;


        private BackgroundBuilder background;
        private List<BirdSprite> birds;
        private HunterSprite hunter;
        private SwordSprite sword;

        private float _pauseAlpha;
        private readonly InputAction _pauseAction;
        


        public GameplayScreen()
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

            background = new BackgroundBuilder();


            //World Creation
            world = new World();
            world.Gravity = Vector2.Zero;

            var top = 0;
            var bottom = Constants.GAME_HEIGHT;
            var left = 0;
            var right = Constants.GAME_WIDTH;

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

            //Spawn Birds/Bodies
            System.Random random = new System.Random();
            birds = new List<BirdSprite>();
            for (int i = 0; i < 5; i++)
            {
                var radius = random.Next(1, 5);
                var position = new Vector2(
                    random.Next(radius, Constants.GAME_WIDTH - radius),
                    random.Next(radius, Constants.GAME_HEIGHT - radius)
                    );

                //Adding rigid body
                var body = world.CreateCircle(radius, 1, position, BodyType.Dynamic);

                body.LinearVelocity = new Vector2(
                    random.Next(-20, 20),
                    random.Next(-20, 20)
                    );

                body.SetRestitution(1);
                body.AngularVelocity = (float)random.NextDouble() * MathHelper.Pi - MathHelper.PiOver2;
                birds.Add(new BirdSprite(radius, body));
            }

            //Spawn hunter in random location on map
            Vector2 pos = (new Vector2(150,200));
            hunter = new HunterSprite(pos);


            var swordBody = world.CreateRectangle(25, 30, 1, (pos - (new Vector2(0, 35))), 0, BodyType.Dynamic);//check back
            swordBody.LinearVelocity = new Vector2(0, 0);
            swordBody.AngularVelocity = (float)0;
            swordBody.SetRestitution(1);

            sword = new SwordSprite(pos - (new Vector2(0, 35)), swordBody);

            
        }




        // Load graphics content
        public override void Activate()
        {
            if (_content == null)
                _content = new ContentManager(ScreenManager.Game.Services, "Content");

            _gameFont = _content.Load<SpriteFont>("gamefont");

            background.LoadContent(_content);

            foreach (var birds in birds) birds.LoadContent(_content);
            sword.LoadContent(_content);

            hunter.LoadContent(_content);

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



                foreach (var bird in birds) bird.Update(gameTime);
            hunter.Update(gameTime);
            sword.Update(gameTime, (hunter.Position - (new Vector2(15, 38))), hunter.Flipped);
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

            // Our player and enemy are both actually just text strings.
            var spriteBatch = ScreenManager.SpriteBatch;

            spriteBatch.Begin();

            background.Draw(gameTime, spriteBatch);

            hunter.Draw(gameTime, spriteBatch);


            sword.Draw(gameTime, spriteBatch);

            foreach (var bird in birds) bird.Draw(gameTime, spriteBatch);

            Vector2 pos = new Vector2(200,0);

            spriteBatch.DrawString(_gameFont, "Hunt Down Birds", pos, Color.AntiqueWhite);

            spriteBatch.End();
            if (TransitionPosition > 0 || _pauseAlpha > 0)
            {
                float alpha = MathHelper.Lerp(1f - TransitionAlpha, 1f, _pauseAlpha / 2);

                ScreenManager.FadeBackBufferToBlack(alpha);
            }
        }
    }
}