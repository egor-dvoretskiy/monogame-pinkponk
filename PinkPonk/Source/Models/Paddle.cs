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
    public class Paddle : Component
    {
        public static int AiPaddleSpeed = 6;

        private readonly Texture2D _texture;
        private readonly Player _player;

        private Rectangle outsideBox;

        public Paddle(Texture2D texture, Player player, Rectangle box)
        {
            this._texture = texture;
            this._player = player;
            this.outsideBox = box;
        }

        public Point Position { get; set; }

        public Point StartPosition { get; private set; }

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

        public void UpdateGameFieldSize(Rectangle rectangle, int paddleXPosition)
        {
            this.outsideBox = rectangle;
            this.StartPosition = new Point(
                paddleXPosition,
                rectangle.Height / 2 - this.Height / 2
            );
        }

        public void Move(Ball ball)
        {

        }

        public void ResetPosition()
        {
            this.Position = this.StartPosition;
        }

        public void AIMove(Ball ball)
        {
            var delta = ball.Position.Y + ball.Height / 2 - (this.Position.Y + this.Height / 2);
            var pos = this.Position;

            if (Math.Abs(delta) > AiPaddleSpeed)
                delta = Math.Sign(delta) * AiPaddleSpeed;

            pos.Y += delta;

            this.FixBounds(pos);
        }

        public void PlayerMove(int diff)
        {
            var pos = this.Position;
            pos.Y += diff;
            this.FixBounds(pos);
        }

        private void FixBounds(Point pos)
        {
            if (pos.Y < this.Height / 2)
                pos.Y = this.Height / 2;
            if (pos.Y + this.Height > this.outsideBox.Height - this.Height / 2)
                pos.Y = this.outsideBox.Height - 3 * this.Height / 2;

            this.Position = pos;
        }

        /*private bool IsBallAbleToBeHit(Ball ball)
        {
            bool directionCheck;
            bool distanceCheck;
            if (this._player == Player.Left)
            {
                directionCheck = ball.Velocity.X > 0;
                distanceCheck = ball.Position.X + ball.Width > this.Box.X;
                return directionCheck & distanceCheck;
            }

            directionCheck = ball.Velocity.X < 0;
            distanceCheck = ball.Box.X < this.Box.X + this.Box.Width;
            return directionCheck & distanceCheck;
        }

        private bool PaddleCheck(int x, int y, Ball ball)
        {
            return x <= this.Box.X + this.Box.Width &&
                x + ball.Box.Width >= this.Box.X &&
                y <= this.Box.Y + this.Box.Height &&
                y + ball.Box.Height >= this.Box.Y;
        }*/
    }
}
