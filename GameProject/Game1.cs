using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;
using System.Collections.Generic;
using System;

using GameProject.Code;

namespace GameProject
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    class Game1 : Game
    {
        public bool gameFinished;
        public int score;
        public Vector2 worldOffset;
        public Enemy enemyFlyweight;
        public Bubble bubbleFlyweight;
        public Background backgroundFlyweight;
        public Player player;

        private SpriteBatch spriteBatch;
        private Song watery_cave_loop;
        private SpriteFont scoreFont;
        private TimeSpan timer;
        private int respawnRate;
        private Vector2 scorePosition;
        private string helpText = "Score points by avoiding the fish.\nPress Enter to Start.";
        private bool gameStarted = false;

        public Game1()
        {
            new GraphicsDeviceManager(this);
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

            if (player == null)
                player = new Player(this);

            player.scale = 1f;

            enemyFlyweight = new Enemy(this);
            bubbleFlyweight = new Bubble(this);
            backgroundFlyweight = new Background(this);

            timer = new TimeSpan(0);
            respawnRate = 400;
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
            bubbleFlyweight.LoadContent(Content);
            enemyFlyweight.LoadContent(Content);
            player.LoadContent(Content);
            scoreFont = Content.Load<SpriteFont>("score");
            backgroundFlyweight.LoadContent();
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            watery_cave_loop.Dispose();
            bubbleFlyweight.UnloadContent();
            enemyFlyweight.UnloadContent();
            player.UnloadContent();
            spriteBatch.Dispose();
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

#if DEBUG
#else
            if (MediaPlayer.State == MediaState.Stopped)
            {
                MediaPlayer.Play(watery_cave_loop);
            }
#endif

            player.Update(gameTime);
            backgroundFlyweight.Update(gameTime);
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
                    // Add an enemy to the list of enemies
                    enemyFlyweight.AddEnemy(new EnemyModel(this));
                }

                enemyFlyweight.Update(gameTime);

                bubbleFlyweight.Update(gameTime);

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

            // Calculate and apply the world/view transform
            worldOffset = new Vector2(100, 0) - new Vector2(player.position.X, 0);
            var t = Matrix.CreateTranslation(worldOffset.X, worldOffset.Y, 0);

            // Begin Drawing
            spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, t);

            backgroundFlyweight.Draw(spriteBatch);
            enemyFlyweight.Draw(spriteBatch);
            bubbleFlyweight.Draw(spriteBatch);
            player.Draw(spriteBatch);
            spriteBatch.DrawString(scoreFont, helpText + (gameStarted ? score.ToString() : ""), new Vector2(scorePosition.X - 2, scorePosition.Y) - worldOffset, Color.Black);
            spriteBatch.DrawString(scoreFont, helpText + (gameStarted ? score.ToString() : ""), scorePosition - worldOffset, Color.White);

            // End Drawing
            spriteBatch.End();

            base.Draw(gameTime);
        }

        /// <summary>
        /// Method to End the Game
        /// </summary>
        private void EndGame()
        {
            enemyFlyweight = new Enemy(this);
            bubbleFlyweight = new Bubble(this);

            helpText = "Game Over! Press Enter to Play Again.\nYour Final Score is ";
            var size = scoreFont.MeasureString(helpText + score.ToString());
            scorePosition = new Vector2((GraphicsDevice.Viewport.Width / 2) - (size.X / 2), (GraphicsDevice.Viewport.Height / 2) - (size.Y / 2));
        }

        /// <summary>
        /// Sorts an array using the timsort algorithm
        /// </summary>
        /// <param name="arr">Array to sort</param>
        /// <param name="run">Chunks</param>
        public void TimSort(List<EnemyModel> arr, int run)
        {
            int n = arr.Count;
            // Sort individual subarrays of size RUN  
            for (int i = 0; i < n; i += run)
                InsertionSort(arr, i, Math.Min((i + 31), (n - 1)));

            // start merging from size RUN (or 32). It will merge  
            // to form size 64, then 128, 256 and so on ....  
            for (int size = run; size < n; size = 2 * size)
            {
                // pick starting point of left sub array. We  
                // are going to merge arr[left..left+size-1]  
                // and arr[left+size, left+2*size-1]  
                // After every merge, we increase left by 2*size  
                for (int left = 0; left < n; left += 2 * size)
                {
                    // find ending point of left sub array  
                    // mid+1 is starting point of right sub array  
                    int mid = left + size - 1;
                    int right = Math.Min((left + 2 * size - 1), (n - 1));

                    // merge sub array arr[left.....mid] &  
                    // arr[mid+1....right]  
                    Merge(arr, left, mid, right);
                }
            }
        }

        /// <summary>
        /// Sorts an array from left index
        /// to right index which is of size of at most RUN
        /// </summary>
        /// <param name="arr">Array to be sorted</param>
        /// <param name="left">Left index to start at</param>
        /// <param name="right">Right index to end at</param>
        private void InsertionSort(List<EnemyModel> arr, int left, int right)
        {
            for (int i = left + 1; i <= right; i++)
            {
                var temp = arr[i];
                int j = i - 1;
                while (j >= left && arr[j].position.Y > temp.position.Y)
                {
                    arr[j + 1] = arr[j];
                    j--;
                }
                arr[j + 1] = temp;
            }
        }

        /// <summary>
        /// Merges the sorted Runs
        /// </summary>
        /// <param name="arr">Array to merge</param>
        /// <param name="l">Left starting point of the array</param>
        /// <param name="m">Middle point of the array</param>
        /// <param name="r">Right end point of the array</param>
        private void Merge(List<EnemyModel> arr, int l, int m, int r)
        {
            // original array is broken in two parts  
            // left and right array  
            int len1 = m - l + 1, len2 = r - m;
            var left = new EnemyModel[len1];
            var right = new EnemyModel[len2];
            for (int x = 0; x < len1; x++)
                left[x] = arr[l + x];
            for (int x = 0; x < len2; x++)
                right[x] = arr[m + 1 + x];

            int i = 0;
            int j = 0;
            int k = l;

            // after comparing, we merge those two array  
            // in larger sub array  
            while (i < len1 && j < len2)
            {
                if (left[i].position.Y <= right[j].position.Y)
                {
                    arr[k] = left[i];
                    i++;
                }
                else
                {
                    arr[k] = right[j];
                    j++;
                }
                k++;
            }

            // copy remaining elements of left, if any  
            while (i < len1)
            {
                arr[k] = left[i];
                k++;
                i++;
            }

            // copy remaining element of right, if any  
            while (j < len2)
            {
                arr[k] = right[j];
                k++;
                j++;
            }
        }
    }
}
