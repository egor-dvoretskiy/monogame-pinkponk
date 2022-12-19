using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using PinkPonk.Source.Enums;
using PinkPonk.Source.GUI;
using System;
using System.Timers;

namespace PinkPonk
{
    public class PinkPonkGame : Game
    {
        private readonly GraphicsDeviceManager _graphics;

        private SpriteBatch spriteBatch;
        private Timer prepareTimer;

        private DateTime lastStartedTimer;
        private string elapsedTimerSecondsContent;

        #region enums

        private GameState gameState;

        #endregion

        #region GUI

        private MainMenu mainMenu;
        private MouseCoordinates mouseCoordinates;

        #endregion

        #region colors

        private Color backgroundColor;

        #endregion

        #region content

        private SpriteFont prepareStateFont;

        #endregion

        public PinkPonkGame()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            this.backgroundColor = Color.CornflowerBlue;
            this.elapsedTimerSecondsContent = string.Empty;
        }

        protected override void Initialize()
        {
            this.gameState = GameState.MainMenu;

            #region graphics initialize

            this._graphics.IsFullScreen = false;
            this._graphics.ApplyChanges();

            this.Window.AllowUserResizing = true;

            #endregion

            #region GUI

            this.mainMenu = new MainMenu(this.Content);

            #endregion

            #region event subscriptions

            this.mainMenu.OnStartGame += MainMenu_OnStartGame;
            this.mainMenu.OnQuitGame += MainMenu_OnQuitGame;

            #endregion

            #region timer initialize

            this.prepareTimer = new Timer();
            this.prepareTimer.AutoReset = false;
            this.prepareTimer.Enabled = false;
            this.prepareTimer.Interval = 3000;
            this.prepareTimer.Elapsed += PrepareTimer_Elapsed;

            #endregion

            base.Initialize();
        }

        protected override void LoadContent()
        {
            this.spriteBatch = new SpriteBatch(GraphicsDevice);

            this.mouseCoordinates = new MouseCoordinates(
                new Texture2D(this.GraphicsDevice, 200, 100),
                this.Content.Load<SpriteFont>("Fonts/DebugFont")
            );

            this.prepareStateFont = Content.Load<SpriteFont>("Fonts/PrepareStateFont");
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            var mouseState = Mouse.GetState();
            this.mouseCoordinates.Content = $"x: {mouseState.X}, y: {mouseState.Y}";

            #region gamestate

            switch (this.gameState)
            {
                case GameState.MainMenu:
                    {
                        this.mainMenu.Update(gameTime);
                    }
                    break;
                case GameState.Prepare:
                    {
                        this.elapsedTimerSecondsContent = (this.lastStartedTimer - DateTime.Now).TotalSeconds.ToString("f1");
                    }
                    break;
                default:
                    break;
            }

            #endregion

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(this.backgroundColor);

            this.spriteBatch.Begin();
            #region gamestate

            switch (this.gameState)
            {
                case GameState.MainMenu:
                    {
                        this.mouseCoordinates.Draw(
                            gameTime,
                            this.spriteBatch,
                            new Vector2(0, 0));

                        this.mainMenu.Draw(
                            gameTime,
                            this.spriteBatch,
                            new Vector2(
                                Window.ClientBounds.Width / 2 - this.mainMenu.Width / 2,
                                Window.ClientBounds.Height / 2 - this.mainMenu.Height / 2
                            )
                        );
                    }
                    break;
                case GameState.Prepare:
                    {
                        GraphicsDevice.Clear(Color.Black);

                        this.spriteBatch.DrawString(
                            this.prepareStateFont,
                            this.elapsedTimerSecondsContent,
                            new Vector2(
                                Window.ClientBounds.Width / 2 - (this.prepareStateFont.MeasureString(this.elapsedTimerSecondsContent).X / 2),
                                Window.ClientBounds.Height / 2 - (this.prepareStateFont.MeasureString(this.elapsedTimerSecondsContent).X / 2)
                            ),
                            Color.White
                        );
                    }
                    break;
                default:
                    break;
            }

            #endregion
            this.spriteBatch.End();

            base.Draw(gameTime);
        }

        private void MainMenu_OnQuitGame(object sender, System.EventArgs e)
        {
            this.Exit();
        }

        private void MainMenu_OnStartGame(object sender, System.EventArgs e)
        {
            this.gameState = GameState.Prepare;

            this.prepareTimer.Start();
            this.lastStartedTimer = DateTime.Now.AddSeconds(3);
        }

        private void PrepareTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            this.gameState = GameState.Start;
        }
    }
}
