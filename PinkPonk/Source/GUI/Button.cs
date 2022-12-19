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
        private readonly string _buttonContent;

        private readonly SpriteFont _font;
        private readonly Texture2D _textureIdle;
        private readonly Texture2D _textureHover;
        private readonly Texture2D _texturePressed;

        private MouseState currentMouseState;
        private MouseState previousMouseState;

        private bool isHovering;
        private bool isPressed;

        private Vector2 position;

        public event EventHandler Click;

        public Button(
            SpriteFont font, 
            string buttonContent, 
            Texture2D textureIdle, 
            Texture2D textureHover, 
            Texture2D texturePressed
        )
        {
            this._font = font;
            this._buttonContent = buttonContent;
            this._textureIdle = textureIdle;
            this._textureHover = textureHover;
            this._texturePressed = texturePressed;
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
            Texture2D currentTexture = this.GetTextureDueToMouseState();

            spriteBatch.Draw(
                currentTexture, 
                vector, 
                Color.White
            );

            spriteBatch.DrawString(
                this._font,
                this._buttonContent,
                new Vector2(
                    vector.X + (currentTexture.Width / 2) - (this._font.MeasureString(this._buttonContent).X / 2),
                    vector.Y + (currentTexture.Height / 2) - (this._font.MeasureString(this._buttonContent).Y / 2)
                ),
                Color.Black
            );

            this.position = vector;
        }

        public override void Update(GameTime gameTime)
        {
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

                if (this.currentMouseState.LeftButton == ButtonState.Pressed)
                    this.isPressed = true;

                if (this.currentMouseState.LeftButton == ButtonState.Released && this.previousMouseState.LeftButton == ButtonState.Pressed)
                {
                    this.Click?.Invoke(this, new EventArgs());
                    this.isPressed = false;
                }
            }
            else
            {
                this.isHovering = false;
                this.isPressed = false;
            }
               
        }

        private bool IsBoundsCrossed(Rectangle mouseRectangle, Vector2 position)
        {
            return mouseRectangle.X >= position.X &&
                mouseRectangle.X <= position.X + this._textureIdle.Width &&
                mouseRectangle.Y >= position.Y &&
                mouseRectangle.Y <= position.Y + this._textureIdle.Height;
        }

        private Texture2D GetTextureDueToMouseState()
        {
            if (this.isPressed)
                return this._texturePressed;

            if (this.isHovering)
                return this._textureHover;

            return this._textureIdle;
        }
    }
}
