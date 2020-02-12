using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;
using System.Collections.Generic;
using System;

namespace GameProject
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        public Texture2D fish;
        public Texture2D fish_dart;
        public Texture2D fish_big;
        public Texture2D bubblesTexture;
        Song watery_cave_loop;
        public SoundEffect bubblesSound;
        SpriteFont scoreFont;

        Player player;
        public List<Enemy> enemies;
        public List<Bubble> bubbles;
        public bool gameFinished;

        TimeSpan timer;
        int respawnRate;
        public int score;
        Vector2 scorePosition;
        string helpText = "Score points by avoiding the fish.\nPress Enter to Start.";
        bool gameStarted = false;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
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
            gameFinished = false;
            player = new Player(this);
            enemies = new List<Enemy>();
            bubbles = new List<Bubble>();

            timer = new TimeSpan(0);
            respawnRate = 500;
            scorePosition = new Vector2(GraphicsDevice.Viewport.Width - 10, 10);

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            watery_cave_loop = Content.Load<Song>("watery_cave_loop");

            bubblesTexture = Content.Load<Texture2D>("bubbles");
            bubblesSound = Content.Load<SoundEffect>("Large Bubble");
            
            player.LoadContent(Content);

            fish = Content.Load<Texture2D>("fish");
            fish_big = Content.Load<Texture2D>("fish-big");
            fish_dart = Content.Load<Texture2D>("fish-dart");

            scoreFont = Content.Load<SpriteFont>("score");
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
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
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            if ((GamePad.GetState(PlayerIndex.One).Buttons.Start == ButtonState.Pressed 
                || Keyboard.GetState().IsKeyDown(Keys.Enter))
                && (!gameStarted || gameFinished))
            {
                gameStarted = true;
                gameFinished = false;
                score = 0;
                helpText = "Score: ";
                Initialize();
            }

            if (MediaPlayer.State == MediaState.Stopped)
            {
                MediaPlayer.Play(watery_cave_loop);
            }
            
            player.Update(gameTime);
            if (!gameStarted)
            {
                var size = scoreFont.MeasureString(helpText + score.ToString());
                scorePosition = new Vector2((GraphicsDevice.Viewport.Width / 2) - (size.X / 2), (GraphicsDevice.Viewport.Height / 2) - (size.Y / 2));
            }
            else if (!gameFinished)
            {
                timer += gameTime.ElapsedGameTime;

                while (timer.TotalMilliseconds > respawnRate)
                {
                    timer -= new TimeSpan(0, 0, 0, 0, respawnRate);
                    enemies.Add(new Enemy(this));
                }

                for (int i = 0; i < enemies.Count; i++)
                {
                    enemies[i].Update(gameTime);
                    if (!enemies[i].alive)
                    {
                        enemies.Remove(enemies[i]);
                        score++;
                    }
                }

                for (int i = 0; i < bubbles.Count; i++)
                {
                    bubbles[i].Update(gameTime);
                    if (!bubbles[i].alive)
                    {
                        bubbles.Remove(bubbles[i]);
                    }
                }
                scorePosition = new Vector2(GraphicsDevice.Viewport.Width - scoreFont.MeasureString(helpText + score.ToString()).X - 20, 10);
            }
            else
            {
                EndGame();
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

            spriteBatch.Begin();

            enemies.ForEach(
                enemy =>
                {
                    enemy.Draw(spriteBatch);
                }
            );

            bubbles.ForEach(
                bubble =>
                {
                    bubble.Draw(spriteBatch);
                }
            );

            player.Draw(spriteBatch);

            spriteBatch.DrawString(scoreFont, helpText + (gameStarted ? score.ToString() : ""), scorePosition, Color.Black);

            spriteBatch.End();

            base.Draw(gameTime);
        }

        private void EndGame()
        {
            enemies = new List<Enemy>();
            bubbles = new List<Bubble>();

            helpText = "Game Over! Press Enter to Play Again.\nYour Final Score is ";
            var size = scoreFont.MeasureString(helpText + score.ToString());
            scorePosition = new Vector2((GraphicsDevice.Viewport.Width / 2) - (size.X / 2), (GraphicsDevice.Viewport.Height / 2) - (size.Y / 2));
        }
    }
}
