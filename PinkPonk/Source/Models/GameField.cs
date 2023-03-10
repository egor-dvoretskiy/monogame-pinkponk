using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using PinkPonk.Source.Abstract;
using PinkPonk.Source.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PinkPonk.Source.Models
{
    public class GameField
    {
        public const int PaddleOffset = 32;

        private readonly Ball _ball;
        private readonly PaddleSet _paddleSet;
        private readonly GraphicsDevice _graphicsDevice;
        private readonly Texture2D _textureDash;
        private readonly SpriteFont _scoreFont;

        private readonly Vector2 _scoreOffset = new Vector2(64, 16);

        private Rectangle box;
        private int previousY;

        public GameField(ContentManager contentManager, GraphicsDevice graphicsDevice, Rectangle outsideBox)
        {
            this.box = outsideBox;

            this._graphicsDevice = graphicsDevice;
            this._ball = new Ball(contentManager, outsideBox);
            this._paddleSet = new PaddleSet()
            {
                PaddleLeft = new Paddle(contentManager.Load<Texture2D>("Models/Paddle-left"), Enums.Player.Left, box),
                PaddleRight = new Paddle(contentManager.Load<Texture2D>("Models/Paddle-right"), Enums.Player.Right, box),
            };
            this._scoreFont = contentManager.Load<SpriteFont>("Fonts/ButtonFont");

            this._textureDash = new Texture2D(graphicsDevice, 1, 1);
            this._textureDash.SetData(new Color[] { Color.White });

            this.Score = new Scores();
        }

        public Scores Score { get; private set; }

        public int Width
        {
            get => this.box.Width;
        }

        public int Height
        {
            get => this.box.Height;
        }

        public void DrawStart(GameTime gameTime, SpriteBatch spriteBatch, Rectangle vector)
        {
            for (int i = 0; i < 31; i++)
            {
                spriteBatch.Draw(
                    this._textureDash, 
                    new Rectangle(
                        vector.X,
                        i * vector.Y / 31,
                        2,
                        vector.Height / 62
                    ), 
                    Color.White
                );
            }

            this._ball.Draw(gameTime, spriteBatch,
                new Rectangle(
                    vector.X - this._ball.Width / 2,
                    vector.Y / 2 - this._ball.Height / 2,
                    this._ball.Width,
                    this._ball.Height
                )
            );

            this._paddleSet.PaddleLeft.Draw(gameTime, spriteBatch,
                new Rectangle(
                    PaddleOffset,
                    vector.Y / 2 - this._paddleSet.PaddleLeft.Height / 2,
                    this._paddleSet.PaddleLeft.Width,
                    this._paddleSet.PaddleLeft.Height
                )
            );
            this._paddleSet.PaddleRight.Draw(gameTime, spriteBatch,
                new Rectangle(
                    vector.X * 2 - PaddleOffset - this._paddleSet.PaddleRight.Width,
                    vector.Y / 2 - this._paddleSet.PaddleRight.Height / 2,
                    this._paddleSet.PaddleRight.Width,
                    this._paddleSet.PaddleRight.Height
                )
            );

            this.DrawScore(spriteBatch);
        }

        public void DrawMove(GameTime gameTime, SpriteBatch spriteBatch)
        {
            for (int i = 0; i < 31; i++)
            {
                spriteBatch.Draw(
                    this._textureDash,
                    new Rectangle(
                        this.box.Width / 2,
                        i * this.box.Height / 31,
                        2,
                        this.box.Height / 62
                    ),
                    Color.White
                );
            }

            this._ball.Draw(
                gameTime, 
                spriteBatch,
                new Rectangle(
                    this._ball.Position.X - this._ball.Width / 2,
                    this._ball.Position.Y - this._ball.Height / 2,
                    this._ball.Width,
                    this._ball.Height
                )
            );

            this._paddleSet.PaddleLeft.Draw(gameTime, spriteBatch,
                new Rectangle(
                    PaddleOffset,
                    this._paddleSet.PaddleLeft.Position.Y,
                    this._paddleSet.PaddleLeft.Width,
                    this._paddleSet.PaddleLeft.Height
                )
            );
            this._paddleSet.PaddleRight.Draw(gameTime, spriteBatch,
                new Rectangle(
                    this.box.Width - PaddleOffset - this._paddleSet.PaddleRight.Width,
                    this._paddleSet.PaddleRight.Position.Y,
                    this._paddleSet.PaddleRight.Width,
                    this._paddleSet.PaddleRight.Height
                )
            );

            this.DrawScore(spriteBatch);
        }

        public void Move()
        {
            var winner = this._ball.Move();

            if (this.Score.SetScore(winner))
            {
                this._ball.ResetPosition();
                this._ball.ResetVelocity();
            }

            var mouseState = Mouse.GetState();
            var deltaY = mouseState.Y - this.previousY;
            this.previousY = mouseState.Y;

            //this._paddleSet.PaddleLeft.AIMove(this._ball);
            this._paddleSet.PaddleLeft.PlayerMove(deltaY);
            this._paddleSet.PaddleRight.AIMove(this._ball);

            var hit1 = this._paddleSet.PaddleLeft.CollisionCheck(this._ball);
            var hit2 = this._paddleSet.PaddleRight.CollisionCheck(this._ball);

            if (hit1 || hit2)
            {
                _ = 3;
            }
        }

        public void UpdatePrepare(Rectangle rectangle)
        {
            this._ball.UpdateGameFieldSize(rectangle);
            this._ball.ResetPosition();

            this._paddleSet.PaddleLeft.UpdateGameFieldSize(rectangle, PaddleOffset);
            this._paddleSet.PaddleLeft.ResetPosition();

            this._paddleSet.PaddleRight.UpdateGameFieldSize(rectangle, rectangle.Width - PaddleOffset);
            this._paddleSet.PaddleRight.ResetPosition();
        }

        public void UpdateGameFieldSize(Rectangle rectangle)
        {
            this.box = rectangle;

            this._ball.UpdateGameFieldSize(rectangle);
        }

        public void DrawEnd(SpriteBatch spriteBatch)
        {
            string winString = $"{this.Score.Winner} wins!";

            spriteBatch.DrawString(
                this._scoreFont,
                winString,
                new Vector2(
                    this.Width / 2 - this._scoreFont.MeasureString(winString).X / 2,
                    this.Height / 2 - this._scoreFont.MeasureString(winString).Y / 2
                ),
                Color.White
            );
        }

        private void DrawScore(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(
                this._scoreFont,
                this.Score.LeftScore.ToString(),
                new Vector2(
                    this.Width / 2 - this._scoreOffset.X - this._scoreFont.MeasureString(this.Score.LeftScore.ToString()).X,
                    this._scoreOffset.Y
                ),
                Color.White
            );

            spriteBatch.DrawString(
                this._scoreFont,
                this.Score.RightScore.ToString(),
                new Vector2(
                    this.Width / 2 + this._scoreOffset.X,
                    this._scoreOffset.Y
                ),
                Color.White
            );
        }
    }
}
