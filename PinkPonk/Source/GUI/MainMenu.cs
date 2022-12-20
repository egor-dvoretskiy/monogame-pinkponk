using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using PinkPonk.Source.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PinkPonk.Source.GUI
{
    public class MainMenu : Component
    {
        private readonly int _buttonOffset = 40;

        private readonly Texture2D _texture;

        private readonly Button _buttonStartGame;
        private readonly Button _buttonQuitGame;

        #region events

        public event EventHandler OnStartGame;
        public event EventHandler OnQuitGame;

        #endregion

        public MainMenu(ContentManager contentManager)
        {
            this._texture = contentManager.Load<Texture2D>("GUI/MainMenuBackground");
            var font = contentManager.Load<SpriteFont>("Fonts/ButtonFont");

            this._buttonStartGame = new Button(
                font,
                "START",
                contentManager.Load<Texture2D>("GUI/Button"),
                contentManager.Load<Texture2D>("GUI/ButtonHover"),
                contentManager.Load<Texture2D>("GUI/ButtonPressed")
            );
            this._buttonStartGame.Click += ButtonStartGameIdle_Click;

            this._buttonQuitGame = new Button(
                font,
                "QUIT",
                contentManager.Load<Texture2D>("GUI/Button"),
                contentManager.Load<Texture2D>("GUI/ButtonHover"),
                contentManager.Load<Texture2D>("GUI/ButtonPressed")
            );
            this._buttonQuitGame.Click += ButtonQuitGameIdle_Click;
        }

        public override int Width
        {
            get => this._texture.Width;
        }

        public override int Height
        {
            get => this._texture.Height;
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch, Rectangle vector)
        {
            spriteBatch.Draw(this._texture, vector, Color.White);
            this._buttonStartGame.Draw(
                gameTime, 
                spriteBatch, 
                new Rectangle(
                    vector.X + this.Width / 2 - this._buttonStartGame.Width / 2,
                    vector.Y + this.Height / 2 - this._buttonStartGame.Height / 2 - this._buttonOffset,
                    this._buttonStartGame.Width,
                    this._buttonStartGame.Height
                )
            );

            this._buttonQuitGame.Draw(
                gameTime, 
                spriteBatch,
                new Rectangle(
                    vector.X + this.Width / 2 - this._buttonQuitGame.Width / 2,
                    vector.Y + this.Height / 2 - this._buttonQuitGame.Height / 2 + this._buttonOffset,
                    this._buttonQuitGame.Width,
                    this._buttonQuitGame.Height
                )
            );
        }

        public override void Update(GameTime gameTime)
        {
            this._buttonStartGame.Update(gameTime);
            this._buttonQuitGame.Update(gameTime);
        }

        private void ButtonQuitGameIdle_Click(object sender, EventArgs e)
        {
            this.OnQuitGame?.Invoke(this, new EventArgs());
        }

        private void ButtonStartGameIdle_Click(object sender, EventArgs e)
        {
            this.OnStartGame?.Invoke(this, new EventArgs());
        }
    }
}
