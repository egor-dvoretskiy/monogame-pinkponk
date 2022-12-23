using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using PinkPonk.Source.Abstract;
using PinkPonk.Source.Enums;
using System;

namespace PinkPonk.Source.Models
{
    public class Ball : Component
    {
        public const int MaxVelocity = 5;

        private readonly Texture2D _texture;
        private readonly Random _random;

        private Rectangle outsideBox;

        public Ball(ContentManager contentManager, Rectangle box)
        {
            this._texture = contentManager.Load<Texture2D>("Models/Ball");
            this.outsideBox = box;
            this._random = new Random();

            this.ResetVelocity();
        }

        public Point Position { get; private set; }

        public Point StartPosition { get; private set; }

        public Velocity Velocity { get; private set; }

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
            spriteBatch.Draw(
                this._texture,
                vector,
                Color.White
            );
        }

        public override void Update(GameTime gameTime)
        {
            throw new NotImplementedException();
        }

        public void SetPosition(Point point)
        {
            this.Position = point;
        }

        public void IncreaseVelocity(int? x = null, int? y = null)
        {
            var vel = this.Velocity;

            if (x != null)
                vel.X += (int)x;
            if (y != null)
                vel.Y += (int)y;

            // cap ball speed
            if (Math.Abs(this.Velocity.X) > MaxVelocity)
                vel.X = Math.Sign(vel.X) * MaxVelocity;
            if (Math.Abs(Velocity.Y) > MaxVelocity)
                vel.Y = Math.Sign(vel.Y) * MaxVelocity;

            this.Velocity = vel;
        }

        public Winner Move()
        {
            var pos = this.Position;
            pos.X += this.Velocity.X;
            pos.Y += Velocity.Y;

            if (pos.Y < 0 && this.Velocity.Y < 0)
            {
                pos.Y = this.Height / 2;
                this.ReverseVelocity(y: true);
            }
            else if (pos.Y + this.Height > this.outsideBox.Height && this.Velocity.Y > 0)
            {
                pos.Y = this.outsideBox.Height - this.Height / 2;
                this.ReverseVelocity(y: true);
            }

            this.Position = pos; // update current position of ball

            // x > 800 before it draws on canvas.

            if (pos.X < 0)
                return Winner.LeftSide;
            else if (pos.X + this.Width > this.outsideBox.Width)
                return Winner.RightSide;

            return Winner.Nobody;
        }

        public void ReverseVelocity(bool x = false, bool y = false)
        {
            var vel = this.Velocity;

            if (x)
                vel.X = -vel.X;
            if (y)
                vel.Y = -vel.Y;

            this.Velocity = vel;
        }

        public void ResetVelocity()
        {
            this.Velocity = new Velocity()
            {
                X = this._random.Next(0, 2) == 0 ? this._random.Next(2, MaxVelocity) : -this._random.Next(2, MaxVelocity),
                Y = this._random.Next(0, 2) == 0 ? -this._random.Next(2, MaxVelocity) : this._random.Next(2, MaxVelocity),
            };
        }

        public void UpdateGameFieldSize(Rectangle rectangle)
        {
            this.outsideBox = rectangle;
            this.StartPosition = new Point(
                rectangle.Width / 2 - this.Width / 2,
                rectangle.Height / 2 - this.Height / 2
            );
        }

        public void ResetPosition()
        {
            this.Position = this.StartPosition;
        }
    }
}
