﻿using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

using tainicom.Aether.Physics2D.Dynamics;
using tainicom.Aether.Physics2D.Collision;
using tainicom.Aether.Physics2D.Common;


namespace GhostHunter.Screens.Content
{
    public class Background : Game
    {
        public Texture2D texture;


        public Rectangle rectangle;

        //Tile stuff
        private Tilemap _tilemap;
        private Tilemap _tilemapProps;
        private Tilemap _tileWater;

        //Sprites
        private List<Enemy> _enemies;
        private Player _player;



        /// <summary>
        /// Loads sprites into the game
        /// </summary>
        /// <param name="content">The content that is to be loaded fro monogame</param>
        public void LoadContent(ContentManager content, World world)
        {
            _tilemap = new Tilemap("map.txt" , world);
            _tilemapProps = new Tilemap("propMap.txt", world);
            //_tileWater = new Tilemap("waterMap.txt");

            _tilemap.LoadContent(content);
            _tilemapProps.LoadContent(content);
           // _tileWater.LoadContent(content);
        }

        /// <summary>
        /// Updates background
        /// </summary>
        /// <param name="gameTime">An object representing time in the game</param>
        public void Update(GameTime gameTime, Player player, List<Enemy> enemies)
        {

            _player = player;
            _enemies = enemies;
            foreach (var enemy in enemies) enemy.Update(gameTime, _player);
        }

        /// <summary>
        /// Draws the sprites in the order sky-> gorund + player -> grass
        /// </summary>
        /// <param name="gameTime">An object representing time in the game</param>
        /// <param name="spriteBatch">The SpriteBatch /param>
        /// <param name="player">Player in the world</param>
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch, Player player) //_enemies
        {
            float playerX = MathHelper.Clamp(player.Position.X, 300, Constants.GAME_MAX_WIDTH - 500);
            float offsetX = 300 - playerX;

            float playerY = MathHelper.Clamp(player.Position.Y,300, Constants.GAME_MAX_HEIGHT - 200);
            float offsetY = 300 - playerY;

            Matrix transform; //Could use for cloud layer

            //Ground Layer
            transform = Matrix.CreateTranslation(offsetX, offsetY, 0);

            spriteBatch.Begin(transformMatrix: transform);
            
            _tilemap.Draw(gameTime, spriteBatch);
            //_tileWater.Draw(gameTime, spriteBatch);
            player.Draw(gameTime, spriteBatch);
            _tilemapProps.Draw(gameTime, spriteBatch);

           // foreach (var enemy in _enemies) enemy.Draw(gameTime, spriteBatch);

            spriteBatch.End();

        }

    }
}
