﻿using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;

namespace GhostHunter.StateManagement
{
    public class ScreenManager : DrawableGameComponent
    {
        private readonly List<GameScreen> _screens = new List<GameScreen>();
        private readonly List<GameScreen> _tmpScreensList = new List<GameScreen>();

        private readonly ContentManager _content;
        private readonly InputState _input = new InputState();

        private bool _isInitialized;

        public SoundEffect scrollEffect;

        public Song backgroundMusic;

        public Song HitNoise;

        public SpriteBatch SpriteBatch { get; private set; }

        public SpriteFont Font { get; private set; }

        public Texture2D BlankTexture { get; private set; }

        public ScreenManager(Game game) : base(game)
        {
            _content = new ContentManager(game.Services, "Content");
        }

        public override void Initialize()
        {
            base.Initialize();
            
            _isInitialized = true;
        }

        protected override void LoadContent()
        {
            SpriteBatch = new SpriteBatch(GraphicsDevice);


            scrollEffect = _content.Load<SoundEffect>("Menu Selection Click");
            backgroundMusic = _content.Load<Song>("Of Far Different Nature - Zwischenwelt (CC-BY)");
            //HitNoise = _content.Load<SoundEffect>("melee sound");
            

            MediaPlayer.IsRepeating = true;
            MediaPlayer.Volume = (float)0.05;
            MediaPlayer.Play(backgroundMusic);
            
            Font = _content.Load<SpriteFont>("menufont");
            BlankTexture = _content.Load<Texture2D>("blank");

            foreach (var screen in _screens)
            {
                screen.Activate();
            }
        }

        protected override void UnloadContent()
        {
            foreach (var screen in _screens)
            {
                screen.Unload();
            }
        }

        public override void Update(GameTime gameTime)
        {
            _input.Update();
            _tmpScreensList.Clear();
            _tmpScreensList.AddRange(_screens);

            bool otherScreenHasFocus = !Game.IsActive;
            bool coveredByOtherScreen = false;

            while (_tmpScreensList.Count > 0)
            {
                var screen = _tmpScreensList[_tmpScreensList.Count - 1];
                _tmpScreensList.RemoveAt(_tmpScreensList.Count - 1);

                screen.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);

                if (screen.ScreenState == ScreenState.TransitionOn || screen.ScreenState == ScreenState.Active)
                {
                    if (!otherScreenHasFocus)
                    {

                        screen.HandleInput(gameTime, _input);  
                        
                        otherScreenHasFocus = true;
                    }

                    if (!screen.IsPopup) coveredByOtherScreen = true;
                    
                }
            }
        }

        public override void Draw(GameTime gameTime)
        {
            foreach (var screen in _screens)
            {
                if (screen.ScreenState == ScreenState.Hidden) continue;

                screen.Draw(gameTime);
            }
        }

        public void AddScreen(GameScreen screen, PlayerIndex? controllingPlayer)
        {
            screen.ControllingPlayer = controllingPlayer;
            screen.ScreenManager = this;
            screen.IsExiting = false;

            
            if (_isInitialized) screen.Activate();

            _screens.Add(screen);
        }

        public void RemoveScreen(GameScreen screen)
        {
            if (_isInitialized) screen.Unload();

            _screens.Remove(screen);
            _tmpScreensList.Remove(screen);
        }


        public GameScreen[] GetScreens()
        {
            return _screens.ToArray();
        }

        public void FadeBackBufferToBlack(float alpha)
        {
            SpriteBatch.Begin();
            SpriteBatch.Draw(BlankTexture, GraphicsDevice.Viewport.Bounds, Color.Black * alpha);
            SpriteBatch.End();
        }

        public void ScrollSound()
        {
            scrollEffect.Play();
        }

        public void MuteMusic()
        {
            MediaPlayer.Pause();
        }

        public void UnmuteMusic()
        {
            MediaPlayer.Play(backgroundMusic);
        }

        public void PlayHitNoise()
        {
            MediaPlayer.Play(HitNoise);
        }

        public void Deactivate()
        {
        }

        public bool Activate()
        {
            return false;
        }
    }
}
