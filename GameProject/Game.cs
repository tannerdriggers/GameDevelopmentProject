using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;
using System.Collections.Generic;
using System;

using GameProject.Code;
using GameProject.Code.Scrolling;

namespace GameProject
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game : Microsoft.Xna.Framework.Game
    {
        public bool gameFinished;
        public int score;

        /// <summary>
        /// Offset of the world to the camera
        /// -- It is negative to standard as it calculates the
        /// -- offset from the camera to the origin
        /// </summary>
        public Vector2 worldOffset;
        public Player player;
        public ParallaxLayer playerLayer;
        public StaticSprite scoreSprite;

        public Texture2D backgroundTexture;
        public ParallaxLayer backgroundLayer;

        public Texture2D[] midgroundTextures;
        public ParallaxLayer midgroundLayer;

        public List<Texture2D> enemyTextures;
        public ParallaxLayer enemyLayer;
        
        private SpriteFont scoreFont;
        private SpriteBatch spriteBatch;
        private Song watery_cave_loop;
        private TimeSpan timer;
        private int respawnRate;
        private Vector2 scorePosition;
        private readonly string startText = "Score points by avoiding the fish";
        private readonly string deathText = "You finished with a score of ";
        private readonly string restartText = "Press Enter or Click to Start";
        private bool gameStarted = false;
        private Random random = new Random();

        public Game()
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
#if DEBUG
            VisualDebugging.LoadContent(Content);
#endif

            // Load Audio
            watery_cave_loop = Content.Load<Song>("watery_cave_loop");


            // Load Background
            backgroundTexture = Content.Load<Texture2D>("background");
            var backgroundSprite = new StaticSprite(this, new Vector2(GraphicsDevice.Viewport.Width / 250, GraphicsDevice.Viewport.Height / 200), backgroundTexture);
            backgroundLayer = new ParallaxLayer(this);
            backgroundLayer.Sprites.Add(backgroundSprite);
            backgroundLayer.DrawOrder = 0;
            Components.Add(backgroundLayer);


            // Load Midground
            midgroundTextures = new Texture2D[]
            {
                Content.Load<Texture2D>("midground")
            };
            var midgroundSprites = new List<StaticSprite>
            {
                new StaticSprite(this, midgroundTextures[0], new Vector2(-midgroundTextures[0].Width, 0)),
                new StaticSprite(this, midgroundTextures[0]),
                new StaticSprite(this, midgroundTextures[0], new Vector2(midgroundTextures[0].Width, 0))
            };
            midgroundLayer = new ParallaxLayer(this);
            midgroundLayer.Sprites.AddRange(midgroundSprites);
            midgroundLayer.DrawOrder = 1;
            Components.Add(midgroundLayer);


            // Load Enemies
            enemyTextures = new List<Texture2D>
            {
                Content.Load<Texture2D>("entities/fish"),
                Content.Load<Texture2D>("entities/fish-big"),
                Content.Load<Texture2D>("entities/fish-dart"),
            };
            enemyLayer = new ParallaxLayer(this);
            enemyLayer.DrawOrder = 2;
            Components.Add(enemyLayer);


            // Load Player
            var playerTexture = Content.Load<Texture2D>("entities/player");
            player = new Player(this, playerTexture);
            playerLayer = new ParallaxLayer(this);
            playerLayer.Sprites.Add(player);
            playerLayer.DrawOrder = 3;
            Components.Add(playerLayer);


            // Load Foreground
            /* Nothing to load */


            // Load Fonts
            scoreFont = Content.Load<SpriteFont>("score");
            scoreSprite = new StaticSprite(this, scoreFont);
            scoreSprite.Position = new Vector2(GraphicsDevice.Viewport.Width - 10, 10);
            scoreSprite.Text = startText + "\n" + restartText;
            var scoreLayer = new ParallaxLayer(this);
            scoreLayer.Sprites.Add(scoreSprite);
            scoreLayer.DrawOrder = 5;
            Components.Add(scoreLayer);


            // Set the scroll amount
            backgroundLayer.ScrollController = new PlayerTrackingScrollController(player, 0f);
            midgroundLayer.ScrollController = new PlayerTrackingScrollController(player, 0.4f);
            playerLayer.ScrollController = new PlayerTrackingScrollController(player, 1f);
            enemyLayer.ScrollController = new PlayerTrackingScrollController(player, 1f);
            scoreLayer.ScrollController = new PlayerTrackingScrollController(player, 0f);
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
                || Keyboard.GetState().IsKeyDown(Keys.Enter)
                || Mouse.GetState().LeftButton == ButtonState.Pressed)
                && (!gameStarted || gameFinished))
            {
                gameStarted = true;
                gameFinished = false;
                score = 0;
                scoreSprite.Text = "Score: 0";
                player.Speed = new Vector2(1f, 0f);
            }

            UpdateMidgrounds();

            if (!gameStarted)
            {
                var size = scoreFont.MeasureString(deathText + score.ToString() + "\n" + restartText);
                scoreSprite.Text = startText + "\n" + restartText;
                scoreSprite.Position = new Vector2((GraphicsDevice.Viewport.Width / 2) - (size.X / 2), (GraphicsDevice.Viewport.Height / 2) - (size.Y / 2));
            }
            else if (!gameFinished)
            {
                scoreSprite.Text = "Score: " + score;
                scoreSprite.Position = new Vector2(13, 10);

                if (random.Next(100) % (15 - ((int)player.Speed.X)) == 0)
                {
                    var values = Enum.GetValues(typeof(EnumEnemyType));
                    var enemyIndex = random.Next(values.Length);
                    var randomEnemyType = (EnumEnemyType)values.GetValue(enemyIndex);
                    enemyLayer.Sprites.Add(new Enemy(this, randomEnemyType, enemyTextures[enemyIndex], new Vector2(player.Position.X + GraphicsDevice.Viewport.Width, random.Next(GraphicsDevice.Viewport.Height - 10))));
                }
            }
            else
            {
                var size = scoreFont.MeasureString(deathText + score.ToString() + "\n" + restartText);
                scoreSprite.Text = deathText + score.ToString() + "\n" + restartText;
                scoreSprite.Position = new Vector2((GraphicsDevice.Viewport.Width / 2) - (size.X / 2), (GraphicsDevice.Viewport.Height / 2) - (size.Y / 2));
            }

#if DEBUG
#else
            if (MediaPlayer.State == MediaState.Stopped)
            {
                MediaPlayer.Play(watery_cave_loop);
            }
#endif

            player.Update(gameTime);

            base.Update(gameTime);
        }

        private void UpdateMidgrounds()
        {
            var middleMidgroundSprite = midgroundLayer.Sprites.ToArray()[1];
            if (player.Position.X * 0.4 - ((PlayerTrackingScrollController)playerLayer.ScrollController).Offset > (middleMidgroundSprite.Position * middleMidgroundSprite.Scale).X)
            {
                var nextSprite = new StaticSprite(this, midgroundTextures[0], midgroundLayer.Sprites.ToArray()[2].Position + new Vector2(midgroundTextures[0].Width, 0));
                midgroundLayer.Sprites.RemoveAt(0);
                midgroundLayer.Sprites.Add(nextSprite);
            }
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            //// Calculate and apply the world/view transform
            //var t = Matrix.CreateTranslation(worldOffset.X, worldOffset.Y, 0);

            //// Begin Drawing
            //spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, t);

            

            //// End Drawing
            //spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
