using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using tainicom.Aether.Physics2D.Dynamics;
using tainicom.Aether.Physics2D.Dynamics.Contacts;


namespace GhostHunter.Screens.Content.Managers
{
    public class EnemyManager
    {
        private float _timer;

        private List<Texture2D> _textures;

        public bool CanAdd { get; set; }

        public Melee Melee { get; set; }

        public Arrow Arrow { get; set; }

        public int MaxEnemies { get; set; }

        public float SpawnTimer { get; set; }

        public EnemyManager(ContentManager content)
        {
            _textures = new List<Texture2D>()
      {
        content.Load<Texture2D>("tiny_skelly-NESW"),
        content.Load<Texture2D>("Ghost"),
      };

            MaxEnemies = 5;
            SpawnTimer = 2.5f;
        }

        public void Update(GameTime gameTime)
        {
            _timer += (float)gameTime.ElapsedGameTime.TotalSeconds;

            CanAdd = false;

            if (_timer > SpawnTimer)
            {
                CanAdd = true;

                _timer = 0;
            }

        }

        public Enemy GetEnemy(World world)
        {
            // var texture = _textures[GameScreen.random.Next(0, _textures.Count)];

            var texture = _textures[0];

            int randomEnemy = GameScreen.random.Next(0,_textures.Count);

            var radius = 5;
            var position = new Vector2(
                GameScreen.random.Next(radius, Constants.GAME_MAX_WIDTH - radius),
                GameScreen.random.Next(radius, Constants.GAME_MAX_HEIGHT - radius)
                );

            //Adding rigid body ---------------EDITING

            Rectangle enemyBody = new Rectangle((int)position.X, (int)position.Y, 16, 16);

            var body = world.CreateCircle(radius, 1, position, BodyType.Dynamic);
            body.LinearVelocity = new Vector2(
                random.Next(-20, 20),
                random.Next(-20, 20)
                );

            body.SetRestitution(1);
            body.AngularVelocity = (float)random.NextDouble() * MathHelper.Pi - MathHelper.PiOver2;


            return new Skeleton(_textures[0] )
            {
                Colour = Color.Red,
                Melee = Melee,
                Health = 10,
                Layer = 0.2f,
                Position = new Vector2(Constants.GAME_WIDTH + texture.Width, GameScreen.Random.Next(0, Constants.GAME_HEIGHT)),
                Speed = 2 + (float)GameScreen.Random.NextDouble(),
                HitTimer = 1.5f + (float)GameScreen.Random.NextDouble(),
            };
/*
            return new Enemy(texture)
            {
                Colour = Color.Red,
                Bullet = Bullet,
                Health = 5,
                Layer = 0.2f,
                Position = new Vector2(Game1.ScreenWidth + texture.Width, Game1.Random.Next(0, Game1.ScreenHeight)),
                Speed = 2 + (float)Game1.Random.NextDouble(),
                ShootingTimer = 1.5f + (float)Game1.Random.NextDouble(),
            };
*/

        }


        public void RaiseMax()
        {
            MaxEnemies++;
        }

        public void LowerSpawnTimer()
        {
            SpawnTimer -= 0.25f;
        }

        public void RaiseSpawnTimer()
        {
            SpawnTimer += 0.25f;
        }

    }

}
