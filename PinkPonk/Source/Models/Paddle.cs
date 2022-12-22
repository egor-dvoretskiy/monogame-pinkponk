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

        public bool CollisionCheck(Ball ball)
        {
            if (!this.IsBallAbleToBeHit(ball))
                return false;

            (float delta, bool wayPastPaddle) = this.FindDeltaInBallMovement(ball);

            if (wayPastPaddle)
                return false;

            float deltaTime = delta / ball.Velocity.X;
            int collY = (int)(ball.Position.Y - ball.Velocity.Y * deltaTime);
            int collX = (int)(ball.Position.X - ball.Velocity.X * deltaTime);

            if (this.PaddleCheck(collX, collY, ball))
            {
                ball.SetPosition(new Point(collX, collY));

                var diffY = (collY + ball.Height / 2) - (this.Position.Y + this.Height / 2);
                diffY /= this.Height / 8;
                diffY -= Math.Sign(diffY);

                ball.IncreaseVelocity(Math.Sign(ball.Velocity.X), diffY);
                ball.ReverseVelocity(true);

                return true;
            }
            return false;
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

        private (float, bool) FindDeltaInBallMovement(Ball ball)
        {
            float delta;
            bool wayPastPaddle;
            if (this._player == Player.Right)
            {
                delta = ball.Position.X + ball.Width - this.Position.X;
                wayPastPaddle = delta > ball.Velocity.X + ball.Width;
                return (delta, wayPastPaddle);
            }

            delta = ball.Position.X - (this.Position.X + this.Width);
            wayPastPaddle = delta < ball.Velocity.X;
            return (delta, wayPastPaddle);
        }

        private bool IsBallAbleToBeHit(Ball ball)
        {
            bool directionCheck;
            bool distanceCheck;
            if (this._player == Player.Right)
            {
                directionCheck = ball.Velocity.X > 0;
                distanceCheck = ball.Position.X + ball.Width > this.Position.X;
                return directionCheck && distanceCheck;
            }

            directionCheck = ball.Velocity.X < 0;
            distanceCheck = ball.Position.X < this.Position.X + this.Width;
            return directionCheck && distanceCheck;
        }

        private bool PaddleCheck(int x, int y, Ball ball)
        {
            return x <= this.Position.X + this.Width &&
                x + ball.Width >= this.Position.X &&
                y <= this.Position.Y + this.Height &&
                y + ball.Height >= this.Position.Y;
        }
    }
}
