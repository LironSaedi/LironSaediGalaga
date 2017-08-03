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
        MouseState ms;
        MouseState ls;
        Texture2D[] enemyImages;
        SpriteFont scoreFont;
        GameState gameState;
        List<KeyPadKey> keyPadKeys;

        int score = 0;
        int lives = 1;

        //Make a User class: name, score
        List<User> highscores = new List<User>();




        bool saveScores = true;
        Random rnd = new Random();
        int level = 1;
        int rows = 0; //5 max
        int cols = 0; //15 max
        bool debugMode = false;
        //bool gameOver = false;

        TimeSpan delay = TimeSpan.Zero;
        XDocument document;

        string username = "";

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
            IsMouseVisible = true;
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
            SpriteFont keyFont = Content.Load<SpriteFont>("KeyboardFont");
            Texture2D keyTex = Content.Load<Texture2D>("key");

            SoundEffect laserSound = Content.Load<SoundEffect>("laser1");
            arwing = new ArWing(arwingTexture, laser, new Vector2(100, 825), Color.White, new Vector2(5), laserSound);
            background = Content.Load<Texture2D>("SpaceBackground");
            GameOver = Content.Load<Texture2D>("GameOver");
            ScoreBoard = Content.Load<Texture2D>("leaderboard");
            KeyBoard = Content.Load<Texture2D>("keyboard-aplhabet");
            GameOverD = new Sprite(GameOver, Vector2.Zero, Color.White);
            space = new Sprite(background, Vector2.Zero, Color.White);
            scoreBoard = new Sprite(ScoreBoard, new Vector2(1300, 30), Color.White);
            keyBoard = new Sprite(KeyBoard, new Vector2(700, 200), Color.White);
            // TODO: use this.Content to load your game content here

            document = XDocument.Load("HighScores.xml");
            //load attribute here
            foreach (XElement score in document.Elements("Scores").Elements("Score"))
            {
                string name = score.Attribute("name").Value;
                int number = int.Parse(score.Value);

                highscores.Add(new User(name, number));
            }

            keyPadKeys = new List<KeyPadKey>();

            //create a y position that starts at the top of the keypad
            int xPos = 200;
            int yPos = 200;

            int xOff;
            int yOff;

            for (int i = 0; i < 26; i++)
            {
                xOff = i % 7 * 76;
                yOff = i / 7 * 76;

                string value = char.ConvertFromUtf32(i + 65);
                keyPadKeys.Add(new KeyPadKey(keyTex, new Vector2(xPos + xOff, yPos + yOff), Color.Black, value, keyFont));
                //add a new KeyPadKey to the list keyPadKeys
                //the new KeyPadKey's new Rectangle's x position will be at the left most x + i % 7 * 77 
                //the y position will be added by 77 every 7 rectangles
                //the letter for each KeyPadKey will be a new char with a value of 65 + i\
            }

            gameState = GameState.Login;
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
                highscores.Add(new User(username, score));
                //highscores.Insert(0, score);//hint not zero unless its the high score

                //loop thorugh highscores
                //compare score with highscores[i]
                //if score > highscores[i]
                //highscores.Insert(i, score)
                for (int i = 0; i < highscores.Count; i++)
                {
                    for (int j = 0; j < highscores.Count; j++)
                    {
                        if (highscores[i].score > highscores[j].score)
                        {
                            User temp = highscores[i];
                            highscores[i] = highscores[j];
                            highscores[j] = temp;
                        }

                    }

                }

                document.Element("Scores").RemoveAll();

                for (int f = 0; f < highscores.Count; f++)
                {
                    XElement score = new XElement("Score", new XAttribute("name", highscores[f].name));
                    score.Value = highscores[f].score.ToString();

                    document.Element("Scores").Add(score);
                }
                document.Save("HighScores.xml");

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

                gameState = GameState.Game;
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
                            gameState = GameState.GameOver;
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

        private void updateLogin(GameTime gameTime)
        {
            if (ms.LeftButton == ButtonState.Pressed && ls.LeftButton == ButtonState.Released)
            {
                for (int i = 0; i < keyPadKeys.Count; i++)
                {
                    if (keyPadKeys[i].Hitbox.Contains(ms.X, ms.Y))
                    {
                        username += keyPadKeys[i].ToString();
                    }
                }
            }
            else if (ks.IsKeyDown(Keys.Enter))
            {
                gameState = GameState.Game;
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
            ms = Mouse.GetState();

            if (ks.IsKeyDown(Keys.B))
            {

                lives += 1;
            }
            if (ks.IsKeyDown(Keys.F8))
            {
                lives -= 1;
            }

            switch (this.gameState)
            {
                case GameState.Game:
                    this.updateGamePlaying(gameTime);
                    break;

                case GameState.GameOver:
                    this.updateGameOver(gameTime);
                    break;

                case GameState.Login:
                    this.updateLogin(gameTime);
                    break;

                default:
                    this.updateGameOver(gameTime);
                    break;
            }

            base.Update(gameTime);

            ls = Mouse.GetState();
        }

        private void drawGameOver(GameTime gameTime)
        {
            GameOverD.Draw(spriteBatch, false);
            spriteBatch.DrawString(scoreFont, $"Your Score: {score}", new Vector2(500, 100), Color.Yellow);
            scoreBoard.Draw(spriteBatch, false);
            spriteBatch.DrawString(scoreFont, "To Restart Press R To Quit Press Q", new Vector2(1000, 550), Color.Yellow);


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

            //for (int j = 0; j < username.Length && j  < 6; j++)
            // { 
            //     spriteBatch.DrawString(scoreFont, username[j].ToString(), new Vector2(1500, 140 + dy), Color.White);
            // }
            for (int i = 0; i < 6; i++)
            {
                if (i < highscores.Count)
                {
                    spriteBatch.DrawString(scoreFont, highscores[i].score.ToString(), new Vector2(1440, 140 + dy), Color.White);
                    spriteBatch.DrawString(scoreFont, highscores[i].name.ToString(), new Vector2(1600, 140 + dy), Color.White);
                }
                else
                {
                    spriteBatch.DrawString(scoreFont, "0", new Vector2(1440, 140 + dy), Color.White);
                }

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

        private void drawLogin(GameTime gameTime)
        {
            spriteBatch.DrawString(scoreFont, "Enter Your Username Here: " + this.username, new Vector2(20, 50), Color.DodgerBlue);
            spriteBatch.DrawString(scoreFont, "When You Are finished Press The ENTER KEY", new Vector2(880, 400), Color.DodgerBlue);
            for (int i = 0; i < keyPadKeys.Count; i++)
            {
                keyPadKeys[i].Draw(spriteBatch, debugMode);
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

            switch (this.gameState)
            {
                case GameState.Game:
                    this.drawGamePlay(gameTime);
                    break;

                case GameState.GameOver:
                    this.drawGameOver(gameTime);
                    break;

                case GameState.Login:
                    this.drawLogin(gameTime);
                    break;

                default:
                    this.updateGameOver(gameTime);
                    break;
            }

            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
