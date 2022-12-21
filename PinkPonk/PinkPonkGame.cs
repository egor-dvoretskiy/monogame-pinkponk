using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using PinkPonk.Source;
using PinkPonk.Source.Enums;
using PinkPonk.Source.GUI;
using PinkPonk.Source.Models;
using System;
using System.Timers;

namespace PinkPonk
{
    public class PinkPonkGame : Game
    {
        private readonly GraphicsDeviceManager _graphics;

        private SpriteBatch spriteBatch;
        private Timer prepareTimer;
        private GameField gameField;

        private DateTime lastStartedTimer;
        private string elapsedTimerSecondsContent;

        #region enums

        private GameState gameState;

        #endregion

        #region GUI

        private MainMenu mainMenu;
        private DebugWindow debug;

        #endregion

        #region colors

        private Color backgroundColor;

        #endregion

        #region content

        private SpriteFont prepareStateFont;

        #endregion

        public PinkPonkGame()
        {
            this._graphics = new GraphicsDeviceManager(this);
            this.TargetElapsedTime = new TimeSpan(333333);
            this.Content.RootDirectory = "Content";
            this.IsMouseVisible = true;

            this.backgroundColor = Color.CornflowerBlue;
            this.elapsedTimerSecondsContent = string.Empty;
        }

        protected override void Initialize()
        {
            this.gameState = GameState.MainMenu;

            this.gameField = new GameField(this.Content, this.GraphicsDevice, this.Window.ClientBounds);
            this.gameField.Score.OnGameFinish += Score_OnGameFinish;

            #region graphics initialize

            this._graphics.IsFullScreen = false;
            this._graphics.ApplyChanges();

            this.Window.AllowUserResizing = true;
            this.Window.ClientSizeChanged += Window_ClientSizeChanged;

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

            this.debug = new DebugWindow(
                new Texture2D(this.GraphicsDevice, 200, 50),
                this.Content.Load<SpriteFont>("Fonts/DebugFont")
            );

            this.prepareStateFont = Content.Load<SpriteFont>("Fonts/PrepareStateFont");
        }

        protected override void Update(GameTime gameTime)
        {
            var mouseState = Mouse.GetState();

            GraphicsDevice.Clear(this.backgroundColor);

            #region debug info

            this.debug.MouseCoordinates = $"x: {mouseState.X}, y: {mouseState.Y}";
            this.debug.GameState = $"{this.gameState}";
            this.debug.WindowBounds = $"{this.Window.ClientBounds.Width}x{this.Window.ClientBounds.Height}";

            #endregion

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
                case GameState.Start:
                    {
                        this.gameField.UpdatePrepare(this.Window.ClientBounds);
                    }
                    break;
                case GameState.Play:
                    {
                        this.gameField.Move();
                    }
                    break;
                case GameState.End:
                    {
                        this.gameState = GameState.MainMenu;
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
            this.spriteBatch.Begin();
            #region gamestate

            switch (this.gameState)
            {
                case GameState.MainMenu:
                    {
                        this.backgroundColor = Color.CornflowerBlue;

                        this.mainMenu.Draw(
                            gameTime,
                            this.spriteBatch,
                            new Rectangle(
                                Window.ClientBounds.Width / 2 - this.mainMenu.Width / 2,
                                Window.ClientBounds.Height / 2 - this.mainMenu.Height / 2,
                                this.mainMenu.Width,
                                this.mainMenu.Height
                            )
                        );
                    }
                    break;
                case GameState.Prepare:
                    {
                        this.backgroundColor = new Color(0x0D, 0x0D, 0x0D);

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
                case GameState.Start:
                    {
                        this.backgroundColor = new Color(0x0D, 0x0D, 0x0D);

                        this.gameField.DrawStart(
                            gameTime, 
                            this.spriteBatch, 
                            new Rectangle(
                                Window.ClientBounds.Width / 2,
                                Window.ClientBounds.Height,
                                Window.ClientBounds.Width,
                                Window.ClientBounds.Height
                            )
                        );

                        this.gameState = GameState.Play;
                    }
                    break;
                case GameState.Play:
                    {
                        this.backgroundColor = new Color(0x0D, 0x0D, 0x0D);

                        this.gameField.DrawMove(gameTime, this.spriteBatch);
                    }
                    break;
                case GameState.End:
                    {
                    }
                    break;
                default:
                    break;
            }

            this.debug.Draw(
                gameTime,
                this.spriteBatch,
                new Rectangle(
                    0,
                    0,
                    this.debug.Width,
                    this.debug.Height
                )
            );

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

        private void Score_OnGameFinish(Winner winner)
        {
            this.gameState = GameState.End;
        }

        private void Window_ClientSizeChanged(object sender, EventArgs e)
        {
            if (this.gameField is null)
                return;

            this.gameField.UpdateGameFieldSize(this.Window.ClientBounds);
        }
    }
}
