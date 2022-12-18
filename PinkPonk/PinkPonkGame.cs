using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using PinkPonk.Source.Enums;
using PinkPonk.Source.GUI;

namespace PinkPonk
{
    public class PinkPonkGame : Game
    {
        private readonly GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

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

        public PinkPonkGame()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            this.backgroundColor = Color.CornflowerBlue;
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

            base.Initialize();
        }

        protected override void LoadContent()
        {
            this._spriteBatch = new SpriteBatch(GraphicsDevice);

            this.mouseCoordinates = new MouseCoordinates(
                new Texture2D(this.GraphicsDevice, 200, 100),
                this.Content.Load<SpriteFont>("Fonts/DebugFont")
            );
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
                default:
                    break;
            }

            #endregion

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(this.backgroundColor);

            this._spriteBatch.Begin();
            #region gamestate

            switch (this.gameState)
            {
                case GameState.MainMenu:
                    {
                        this.mouseCoordinates.Draw(
                            gameTime,
                            this._spriteBatch,
                            new Vector2(0, 0));

                        this.mainMenu.Draw(
                            gameTime,
                            this._spriteBatch,
                            new Vector2(
                                Window.ClientBounds.Width / 2 - this.mainMenu.Width / 2,
                                Window.ClientBounds.Height / 2 - this.mainMenu.Height / 2
                            )
                        );
                    }
                    break;
                default:
                    break;
            }

            #endregion
            this._spriteBatch.End();

            base.Draw(gameTime);
        }

        private void MainMenu_OnQuitGame(object sender, System.EventArgs e)
        {
            this.Exit();
        }

        private void MainMenu_OnStartGame(object sender, System.EventArgs e)
        {
            //change game state
        }
    }
}
