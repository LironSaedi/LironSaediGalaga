using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace LironSaediGalaga
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        // Test change
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Texture2D arwingTexture;
        Texture2D background;
        public static Texture2D hitboxSprite;
        ArWing arwing;
        List<Enemies> enemies;
        Sprite space;
        KeyboardState ks;
        Texture2D[] enemyImages;
        SpriteFont scoreFont;
        SpriteFont backFont;
        int score = 0;
        int lives = 5;
        Random rnd = new Random();
        int level = 1;
        int rows = 0; //5 max
        int cols = 0; //15 max
        bool debugMode = false;

        TimeSpan delay = TimeSpan.Zero;


        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = 1800;
            graphics.PreferredBackBufferHeight = 900;
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            hitboxSprite = Content.Load<Texture2D>("WhitePixel");
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            enemyImages = new Texture2D[2];
            enemyImages[0] = Content.Load<Texture2D>("Jaewon");
            enemyImages[1] = Content.Load<Texture2D>("Alex");



            enemies = new List<Enemies>();

            Spawn();

            //enemies.Add(new Enemies(enemiesImage, new Vector2(40, 10), Color.White, 5, 5));
            //enemies.Add(new Enemies(enemiesImageAlex, new Vector2(700, 20), Color.White, 5, 5));
            //enemies.Add(new Enemies(enemiesImage, new Vector2(800, 10), Color.White, 5, 5));
            //enemies.Add(new Enemies(enemiesImage, new Vector2(700, 70), Color.White, 5, 5));
            //enemies.Add(new Enemies(enemiesImage, new Vector2(300,10), Color.White, 5, 5));
            //enemies.Add(new Enemies(enemiesImage, new Vector2(750, 20), Color.White, 5, 5));
            //enemies.Add(new Enemies(enemiesImage, new Vector2(670, 207), Color.White, 5, 5));
            //enemies.Add(new Enemies(enemiesImage, new Vector2(300, 10), Color.White, 5, 5));
            //enemies.Add(new Enemies(enemiesImage, new Vector2(720, 20), Color.White, 5, 5));

            arwingTexture = Content.Load<Texture2D>("arwing");
            Texture2D laser = Content.Load<Texture2D>("Laser");

            scoreFont = Content.Load<SpriteFont>("Scorefont");
            backFont = Content.Load<SpriteFont>("SpriteFontBack");
            SoundEffect laserSound = Content.Load<SoundEffect>("laser1");
            arwing = new ArWing(arwingTexture, laser, new Vector2(100, 825), Color.White, new Vector2(5), laserSound);
            background = Content.Load<Texture2D>("SpaceBackground");

            space = new Sprite(background, Vector2.Zero, Color.White);
            // TODO: use this.Content to load your game content here
        }

        private void Spawn()
        {
            cols++; //max 15
            rows++; //max 5

            if (rows > 5)
            {
                rows = 5;
            }

            if (cols > 15)
            {
                cols = 15;
            }
            int x = 0;
            int y = 10;
            int enemyImageIndex = 0;
            for (int j = 0; j < rows; j++)
            {
                for (int i = 0; i < cols; i++)
                {
                    enemyImageIndex = rnd.Next(0, 2);
                    int score;

                    if (enemyImageIndex == 0)

                    {
                        score = 5;
                    }
                    else
                    {
                        score = 1;
                    }
                    Color[] colors = new Color[] { Color.SpringGreen, Color.Yellow, Color.LightCyan, Color.White, Color.Red };

                    enemies.Add(new Enemies(enemyImages[enemyImageIndex], Content.Load<Texture2D>("brain"), new Vector2(x, y) + new Vector2(70), 3, colors[rnd.Next(colors.Length)], 5, 5, score, (float)(rnd.NextDouble()) / 50f, rnd));
                    x += enemyImages[enemyImageIndex].Width;
                }
                x = 0;
                y += enemyImages[0].Height;
            }
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {

            ks = Keyboard.GetState();
            // TODO: Add your update logic here

            arwing.Update(ks);

            for (int i = 0; i < arwing.Bullets.Count; i++)
            {

                if (arwing.Bullets[i].Position.Y > GraphicsDevice.Viewport.Height)
                {
                    arwing.Bullets.RemoveAt(i);
                    i--;
                }
                else
                {
                    //another for loop for enemies
                    for (int j = 0; j < enemies.Count; j++)
                    {


                        if (arwing.Bullets[i].Hitbox.Intersects(enemies[j].Hitbox))
                        {
                            score += (int)((float)enemies[j].Score / enemies[j].Scale);
                            //stop drawing enemy
                            enemies.RemoveAt(j);
                            j--;

                            //stop drawing bullet
                            arwing.Bullets.RemoveAt(i);
                            i--;

                            break;
                        }
                    }
                }

            }

            for (int i = 0; i < enemies.Count; i++)
            {

                enemies[i].Update(gameTime);
                for (int k = 0; k < enemies[i].Bullets.Count; k++)
                {
                    if (arwing.Hitbox.Intersects(enemies[i].Bullets[k].Hitbox))
                    {
                        lives--;
                        enemies[i].Bullets.Remove(enemies[i].Bullets[k]);
                        if (lives == 0)
                        {
                            Exit();
                        }

                    }
                }
            }

            if (enemies.Count == 0)
            {
                //count a timespan
                delay += gameTime.ElapsedGameTime;
                arwing.stop = true;
                if (delay > TimeSpan.FromMilliseconds(2000))
                {
                    arwing.stop = false;
                    //if timespan > some amount of time
                    level++;

                    delay = TimeSpan.Zero;
                    Spawn();
                }
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            spriteBatch.Begin();
            space.Draw(spriteBatch, false);



            //for loop to draw them all
            for (int j = 0; j < enemies.Count; j++)
            {
                enemies[j].Draw(spriteBatch, this.debugMode);
            }

            arwing.Draw(spriteBatch, debugMode);


            spriteBatch.DrawString(scoreFont, "Score: " + score, new Vector2(10, 20), Color.ForestGreen);
            spriteBatch.DrawString(backFont, "Lives:" + lives, new Vector2(1650, 10), Color.White);
            spriteBatch.DrawString(scoreFont, "Lives:" + lives, new Vector2(1650, 10), Color.ForestGreen);
            spriteBatch.DrawString(scoreFont, "Level:" + level, new Vector2(1000, 10), Color.ForestGreen);

            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
