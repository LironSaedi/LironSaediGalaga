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
using System.Xml.Linq;

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
        Texture2D GameOver;
        Texture2D ScoreBoard;
        Texture2D KeyBoard;
        public static Texture2D hitboxSprite;
        ArWing arwing;
        List<Enemies> enemies;
        Sprite space;
        Sprite GameOverD;
        Sprite keyBoard;
        Sprite scoreBoard;
        KeyboardState ks;
        Texture2D[] enemyImages;
        SpriteFont scoreFont;

        int score = 0;
        int lives = 1;
        List<int> highscores = new List<int>();
        bool saveScores = true;
        Random rnd = new Random();
        int level = 1;
        int rows = 0; //5 max
        int cols = 0; //15 max
        bool debugMode = false;
        bool gameOver = false;

        TimeSpan delay = TimeSpan.Zero;
        XDocument document;


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

            SoundEffect laserSound = Content.Load<SoundEffect>("laser1");
            arwing = new ArWing(arwingTexture, laser, new Vector2(100, 825), Color.White, new Vector2(5), laserSound);
            background = Content.Load<Texture2D>("SpaceBackground");
            GameOver = Content.Load<Texture2D>("GameOver");
            ScoreBoard = Content.Load<Texture2D>("leaderboard");
            KeyBoard = Content.Load<Texture2D>("keyboard-aplhabet");
            GameOverD = new Sprite(GameOver, Vector2.Zero, Color.White);
            space = new Sprite(background, Vector2.Zero, Color.White);
            scoreBoard = new Sprite(ScoreBoard, new Vector2(1300, 30), Color.White);
            keyBoard = new Sprite(KeyBoard, new Vector2(1700, 600), Color.White);
            // TODO: use this.Content to load your game content here

            document = XDocument.Load("HighScores.xml");
            foreach (XElement score in document.Elements("Scores").Elements("Score"))
            {
                highscores.Add(int.Parse(score.Value));
            }
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

        private void updateGameOver(GameTime gameTime)
        {
            if (saveScores == true)
            {
                saveScores = false;

                //save score into the list of highscores
                highscores.Add(score);
                //highscores.Insert(0, score);//hint not zero unless its the high score

                //loop thorugh highscores
                //compare score with highscores[i]
                //if score > highscores[i]
                //highscores.Insert(i, score)
                for (int i = 0; i < highscores.Count; i++)
                {
                    for (int j = 0; j < highscores.Count; j++)
                    {
                        if (highscores[i] > highscores[j])
                        {
                            int temp = highscores[i];
                            highscores[i] = highscores[j];
                            highscores[j] = temp;
                        }

                    }

                }

                document.Element("Scores").RemoveAll();

                for (int f = 0; f < 6; f++)
                {
                    if(f < highscores.Count)
                    {
                        XElement score = new XElement("Score");
                        score.Value = highscores[f].ToString();
                        document.Element("Scores").Add(score);
                        document.Save("HighScores.xml");
                    }
                    else
                    {
                        XElement score = new XElement("Score");
                        score.Value = "0";
                        document.Element("Scores").Add(score);
                        document.Save("HighScores.xml");
                    }
                }


            }

            if (ks.IsKeyDown(Keys.R))
            {

                enemies = new List<Enemies>();
                lives = 2;
                level = 1;
                rows = 0;
                cols = 0;
                score = 0;
                Spawn();
                Texture2D laser = Content.Load<Texture2D>("Laser");
                SoundEffect laserSound = Content.Load<SoundEffect>("laser1");
                arwing = new ArWing(arwingTexture, laser, new Vector2(100, 825), Color.White, new Vector2(5), laserSound);

                gameOver = false;
            }

            else if (ks.IsKeyDown(Keys.Q))
            {
                Exit();
            }
        }

        private void updateGamePlaying(GameTime gameTime)
        {
            // TODO: Add your update logic here
            saveScores = true;

            arwing.Update(ks, GraphicsDevice.Viewport);

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
                            gameOver = true;
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
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            ks = Keyboard.GetState();

            if (ks.IsKeyDown(Keys.B))
            {

                lives += 1;
            }
            if (ks.IsKeyDown(Keys.F8))
            {
                lives -= 1;
            }

            if (!gameOver)
            {
                this.updateGamePlaying(gameTime);
            }
            else
            {
                this.updateGameOver(gameTime);
            }

            base.Update(gameTime);
        }

        private void drawGameOver(GameTime gameTime)
        {
            GameOverD.Draw(spriteBatch, false);
            spriteBatch.DrawString(scoreFont, "Your Score: " + score, new Vector2(500, 100), Color.Yellow);
            scoreBoard.Draw(spriteBatch, false);
            spriteBatch.DrawString(scoreFont, "To Restart Press R To Quit Press Q", new Vector2(1000, 550), Color.Yellow);
            keyBoard.Draw(spriteBatch, false);

            // sort high scores
            // for loop though all items (twice)
            // if (item[i] < item[j])
            // swap i and j


            //int x = 3;
            //int y = 5;

            // save x before we overrite it
            //int temp = x;
            //x = y;
            //y = temp;

            int dy = 0;


            for (int i = 0; i < highscores.Count && i < 6; i++)
            {

                spriteBatch.DrawString(scoreFont, highscores[i].ToString(), new Vector2(1440, 140 + dy), Color.White);


                dy += 30;

            }
        }


        private void drawGamePlay(GameTime gameTime)
        {
            //for loop to draw them all
            for (int j = 0; j < enemies.Count; j++)
            {
                enemies[j].Draw(spriteBatch, this.debugMode);
            }

            arwing.Draw(spriteBatch, debugMode);

            spriteBatch.DrawString(scoreFont, "Score: " + score, new Vector2(10, 20), Color.ForestGreen);
            spriteBatch.DrawString(scoreFont, "Lives:" + lives, new Vector2(1500, 10), Color.ForestGreen);
            spriteBatch.DrawString(scoreFont, "Level:" + level, new Vector2(1000, 10), Color.ForestGreen);
            if (ks.IsKeyDown(Keys.G))
            {
                score += 1;
            }

        }
        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        /// 
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            spriteBatch.Begin();
            space.Draw(spriteBatch, false);

            if (!gameOver)
            {
                this.drawGamePlay(gameTime);
            }
            else
            {
                this.drawGameOver(gameTime);
            }

            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
