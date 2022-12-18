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
    public class MainMenu : ComponentGUI
    {
        private readonly float _buttonOffset = 10;

        private readonly Texture2D _texture;

        private readonly Button _buttonStartGameIdle;
        private readonly Button _buttonQuitGameIdle;

        #region events

        public event EventHandler OnStartGame;
        public event EventHandler OnQuitGame;

        #endregion

        public MainMenu(ContentManager contentManager)
        {
            this._texture = contentManager.Load<Texture2D>("GUI/MainMenuBackground");

            this._buttonStartGameIdle = new Button(
                contentManager.Load<Texture2D>("GUI/ButtonStartIdle"),
                contentManager.Load<Texture2D>("GUI/ButtonStartHover")
            );
            this._buttonStartGameIdle.Click += ButtonStartGameIdle_Click;

            this._buttonQuitGameIdle = new Button(
                contentManager.Load<Texture2D>("GUI/ButtonQuitIdle"),
                contentManager.Load<Texture2D>("GUI/ButtonQuitHover")
            );
            this._buttonQuitGameIdle.Click += ButtonQuitGameIdle_Click;
        }

        public override float Width
        {
            get => this._texture.Width;
        }

        public override float Height
        {
            get => this._texture.Height;
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch, Vector2 vector)
        {
            spriteBatch.Draw(this._texture, vector, Color.White);

            this._buttonStartGameIdle.Draw(
                gameTime, 
                spriteBatch, 
                new Vector2(
                    vector.X + this.Width / 2 - this._buttonStartGameIdle.Width / 2,
                    vector.Y + this.Height / 2 - this._buttonStartGameIdle.Height / 2 - this._buttonOffset
                )
            );

            this._buttonQuitGameIdle.Draw(
                gameTime, 
                spriteBatch,
                new Vector2(
                    vector.X + this.Width / 2 - this._buttonQuitGameIdle.Width / 2,
                    vector.Y + this.Height / 2 - this._buttonQuitGameIdle.Height / 2 + this._buttonOffset
                )
            );
        }

        public override void Update(GameTime gameTime)
        {
            this._buttonStartGameIdle.Update(gameTime);
            this._buttonQuitGameIdle.Update(gameTime);
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
