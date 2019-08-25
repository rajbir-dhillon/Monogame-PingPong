using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace Game1
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        public enum GameState
        {
            MainMenu,
            Options,
            Playing,
            GameOver
        }
        GameState CurrentGameState = GameState.MainMenu;

        //screen size
        int screenWidth = 800, screenHeight = 600;

        CButton btnPlay;
        CButton btnExit;
        Texture2D gamebackground;

        //game variables
        Ball ball;
        Player player1;
        Player player2;
        private SpriteFont font;

        int ballSize = 30;
        int batwidth = 10;
        int batheigth = 100;
        public static int score1 = 0;
        public static int score2 = 0;
        string winner;
        Vector2 player1pos;
        Vector2 player2pos;

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
            // TODO: Add your initialization logic here

            //enable mousepointer
            IsMouseVisible = true;

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            //load screen sizes
            graphics.PreferredBackBufferWidth = screenWidth;
            graphics.PreferredBackBufferHeight = screenHeight;

            graphics.ApplyChanges();
            IsMouseVisible = true;

            //load main menu content
            btnPlay = new CButton(Content.Load<Texture2D>("button"), graphics.GraphicsDevice);
            btnPlay.setSpoition(new Vector2(screenWidth/2, 300));
            btnExit = new CButton(Content.Load<Texture2D>("exit"), graphics.GraphicsDevice);
            btnExit.setSpoition(new Vector2(screenWidth / 2, 350));
            gamebackground = Content.Load<Texture2D>("gamebackground");
            //load score font
            font = Content.Load<SpriteFont>("font");

            // Load game content
            player1pos = new Vector2(10, (GraphicsDevice.Viewport.Height / 2) - (batheigth / 2));
            player2pos = new Vector2(GraphicsDevice.Viewport.Width - 10 - batwidth , (GraphicsDevice.Viewport.Height / 2) - (batheigth / 2));

            ball = new Ball(GraphicsDevice, spriteBatch, this, ballSize);
            player1 = new Player(GraphicsDevice, spriteBatch, this, batwidth, batheigth, player1pos);
            player2 = new Player(GraphicsDevice, spriteBatch, this, batwidth, batheigth, player2pos);

            
            ball.ResetBall();
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

            // TODO: Add your update logic here
            MouseState mouse = Mouse.GetState();
            switch (CurrentGameState)
            {
                case GameState.MainMenu:
                    if (btnPlay.isClicked == true) CurrentGameState = GameState.Playing;
                    if (btnExit.isClicked == true) Exit();
                    btnPlay.Update(mouse);
                    btnExit.Update(mouse);
                    break;

                case GameState.Playing:
                    //player 1 movement
                    if (Keyboard.GetState().IsKeyDown(Keys.W))
                        player1.KeyUp();
                    if (Keyboard.GetState().IsKeyDown(Keys.S))
                        player1.KeyDown();

                    //player 2 movement 
                    if (Keyboard.GetState().IsKeyDown(Keys.Up))
                        player2.KeyUp();
                    if (Keyboard.GetState().IsKeyDown(Keys.Down))
                        player2.KeyDown();
                    if ( Keyboard.GetState().IsKeyDown(Keys.Space) && !ball.gameRun)
                    {
                        ball.gameRun = true;
                    }
                    if (ball.gameRun)
                    {
                        ball.Update(gameTime);


                        
                        //player collision logic
                        if (ball.dirx > 0)
                        {
                            if (ball.posy >= player2.posY && ball.posy + ballSize < player2.posY + batheigth && ball.posx + ballSize >= player2.posX)
                            {
                                ball.dirx = -ball.dirx;
                            }
                        }

                        else if (ball.dirx < 0)
                        {
                            if (ball.posy >= player1.posY && ball.posy + ballSize <= player1.posY + batheigth && ball.posx <= player1.posX + batwidth)
                            {
                                ball.dirx = -ball.dirx;
                            }
                        }
                        if (score1 == 10 || score2 == 10){
                            ball.gameRun = false;
                            CurrentGameState = GameState.GameOver;
                            if (score1 < score2){
                                winner = "Player 2";
                            } else {
                                winner = "Player 1";
                            }
                        }
                    }
                    break;
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
            switch (CurrentGameState)
            {
                case GameState.MainMenu:
                    //spriteBatch.Draw(Content.Load<Texture2D>("mainmenu"), new Rectangle(0, 0, screenWidth, screenHeight), Color.White);
                    btnPlay.Draw(spriteBatch);
                    btnExit.Draw(spriteBatch);
                    break;
                case GameState.Playing:
                    spriteBatch.Draw(gamebackground, new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height), Color.White);
                    player1.Draw(gameTime);
                    player2.Draw(gameTime);
                    spriteBatch.DrawString(font, score1 + " - " + score2, new Vector2(GraphicsDevice.Viewport.Width / 2 - font.MeasureString(score1 + " - " + score2).X / 2 , 15), Color.Black);
                    ball.Draw(gameTime);
                    break;
                case GameState.GameOver:
                    spriteBatch.DrawString(font, "winner is " + winner, new Vector2(GraphicsDevice.Viewport.Width / 2 , 15), Color.Black);

                    break;
            }
            spriteBatch.End();

            base.Draw(gameTime);
        }
        public static void Player1score() => score1++;
        public static void Player2score() => score2++;
    }
}
