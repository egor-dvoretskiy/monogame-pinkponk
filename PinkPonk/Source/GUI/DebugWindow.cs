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
    public class DebugWindow : Component
    {
        private readonly Texture2D _texture;
        private readonly SpriteFont _font;

        public DebugWindow(Texture2D texture, SpriteFont font)
        {
            _texture = texture;
            this._font = font;
        }

        public string MouseCoordinates { get; set; }

        public string GameState { get; set; }

        public string WindowBounds { get; set; }

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
            if (!string.IsNullOrEmpty(this.MouseCoordinates))
            {
                spriteBatch.DrawString(
                    this._font, 
                    this.MouseCoordinates, 
                    new Vector2(
                        (vector.X + this.Width) / 2 - this._font.MeasureString(this.MouseCoordinates).X / 2,
                        (vector.Y + this.Height) / 2 - this._font.MeasureString(this.MouseCoordinates).Y / 2
                    ), 
                    Color.White
                );

                spriteBatch.DrawString(
                    this._font, 
                    this.GameState, 
                    new Vector2(
                        (vector.X + this.Width) / 2 - this._font.MeasureString(this.GameState).X / 2,
                        (vector.Y + this.Height) / 2 - this._font.MeasureString(this.GameState).Y / 2 + 20
                    ), 
                    Color.White
                );

                spriteBatch.DrawString(
                    this._font, 
                    this.WindowBounds, 
                    new Vector2(
                        (vector.X + this.Width) / 2 - this._font.MeasureString(this.WindowBounds).X / 2,
                        (vector.Y + this.Height) / 2 - this._font.MeasureString(this.WindowBounds).Y / 2 + 40
                    ), 
                    Color.White
                );
            }
        }

        public override void Update(GameTime gameTime)
        {
        }
    }
}
