using Microsoft.Xna.Framework;
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
    public class Button : ComponentGUI
    {
        private readonly Texture2D _textureIdle;
        private readonly Texture2D _textureHover;

        private MouseState currentMouseState;
        private MouseState previousMouseState;

        private bool isHovering;

        private Vector2 position;

        public event EventHandler Click;

        public Button(Texture2D textureIdle, Texture2D textureHover)
        {
            this._textureIdle = textureIdle;
            this._textureHover = textureHover;
        }

        public override float Width
        {
            get => this._textureIdle.Width;
        }

        public override float Height
        {
            get => this._textureIdle.Height;
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch, Vector2 vector)
        {
            spriteBatch.Draw(
                this.isHovering ? this._textureHover : this._textureIdle, 
                vector, 
                Color.White
            );

            this.position = vector;
        }

        public override void Update(GameTime gameTime)
        {
            this.isHovering = false;

            this.previousMouseState = this.currentMouseState;
            this.currentMouseState = Mouse.GetState();

            var mouseRectangle = new Rectangle(
                this.currentMouseState.X, 
                this.currentMouseState.Y, 
                1, 
                1
            );

            if (this.IsBoundsCrossed(mouseRectangle, this.position))
            {
                this.isHovering = true;

                if (this.currentMouseState.LeftButton == ButtonState.Released && this.previousMouseState.LeftButton == ButtonState.Pressed)
                {
                    this.Click?.Invoke(this, new EventArgs());
                }
            }
        }

        private bool IsBoundsCrossed(Rectangle mouseRectangle, Vector2 position)
        {
            return mouseRectangle.X >= position.X &&
                mouseRectangle.X <= position.X + this._textureIdle.Width &&
                mouseRectangle.Y >= position.Y &&
                mouseRectangle.Y <= position.Y + this._textureIdle.Height;
        }
    }
}
