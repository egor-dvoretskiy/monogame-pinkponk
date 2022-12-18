using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PinkPonk.Source.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PinkPonk.Source.GUI
{
    public class MouseCoordinates : ComponentGUI
    {
        private readonly Texture2D _texture;
        private readonly SpriteFont _font;

        public MouseCoordinates(Texture2D texture, SpriteFont font)
        {
            _texture = texture;
            this._font = font;
        }

        public string Content { get; set; }

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
            spriteBatch.Draw(this._texture, vector, Color.Orange);

            if (!string.IsNullOrEmpty(this.Content))
            {
                spriteBatch.DrawString(this._font, this.Content, vector, Color.Black);
            }
        }

        public override void Update(GameTime gameTime)
        {
        }
    }
}
