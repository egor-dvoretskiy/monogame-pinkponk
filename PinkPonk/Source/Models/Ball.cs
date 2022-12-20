using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using PinkPonk.Source.Abstract;
using PinkPonk.Source.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PinkPonk.Source.Models
{
    public class Ball : Component
    {
        public const int MaxVelocity = 64;

        private readonly Texture2D _texture;
        private readonly Random _random;

        private Rectangle outsideBox;

        public Ball(ContentManager contentManager, Rectangle box)
        {
            this._texture = contentManager.Load<Texture2D>("Models/Ball");
            this.outsideBox = box;
            this._random = new Random();
            this.InitializeVelocity();
        }

        public Point Position { get; private set; }

        public Velocity Velocity { get; set; }

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
            this.Position = new Point(vector.X, vector.Y);

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

        public void UpdateGameFieldSize(Rectangle rectangle)
        {
            this.outsideBox = rectangle;
        }

        public Winner Move()
        {
            var pos = this.Position;
            pos.X += this.Velocity.X;
            pos.Y += this.Velocity.Y;

            if (pos.X < 0)
                return Winner.LeftSide;
            else if (pos.X + this.Width > this.outsideBox.Width)
                return Winner.RightSide;

            if (pos.Y < 0)
            {
                pos.Y = -pos.Y;
                this.ReverseVelocity(y: true);
            } 
            else if (pos.Y + this.Height > this.outsideBox.Height)
            {
                pos.Y = this.outsideBox.Height - (pos.Y + this.Height - this.outsideBox.Height);
                this.ReverseVelocity(y: true);
            }

            this.Position = pos; // update current position of ball

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

        private void InitializeVelocity()
        {
            this.Velocity = new Velocity()
            {
                X = this._random.Next(0, 2) == 0 ? this._random.Next(2, 7) : -this._random.Next(2, 7),
                Y = this._random.Next() > int.MaxValue / 2 ? this._random.Next(2, 7) : -this._random.Next(2, 7),
            };
        }
    }
}
