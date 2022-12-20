using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using PinkPonk.Source.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PinkPonk.Source.Models
{
    public class GameField : Component
    {
        public const int PaddleOffset = 32;

        private readonly Ball _ball;
        private readonly PaddleSet _paddleSet;
        private readonly GraphicsDevice _graphicsDevice;
        private readonly Texture2D _textureDash;

        private Rectangle box;

        public GameField(ContentManager contentManager, GraphicsDevice graphicsDevice, Rectangle outsideBox)
        {
            this.box = outsideBox;

            this._graphicsDevice = graphicsDevice;
            this._ball = new Ball(contentManager, outsideBox);
            this._paddleSet = new PaddleSet()
            {
                PaddleLeft = new Paddle(contentManager, Enums.Player.Left, box),
                PaddleRight = new Paddle(contentManager, Enums.Player.Right, box),
            };

            this._textureDash = new Texture2D(graphicsDevice, 1, 1);
            this._textureDash.SetData(new Color[] { Color.White });

            this.Score = new Scores();
        }

        public Scores Score { get; private set; }

        public override int Width
        {
            get => this.box.Width;
        }

        public override int Height
        {
            get => this.box.Height;
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch, Rectangle vector)
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
                    vector.X * 2 - PaddleOffset,
                    vector.Y / 2 - this._paddleSet.PaddleLeft.Height / 2,
                    this._paddleSet.PaddleLeft.Width,
                    this._paddleSet.PaddleLeft.Height
                )
            );
        }

        public void DrawMove(GameTime gameTime, SpriteBatch spriteBatch)
        {
            this._ball.Draw(gameTime, spriteBatch,
                   new Rectangle(
                       this._ball.Position.X - this._ball.Width / 2,
                       this._ball.Position.Y / 2 - this._ball.Height / 2,
                       this._ball.Width,
                       this._ball.Height
                   )
               );
            this._paddleSet.PaddleLeft.Draw(gameTime, spriteBatch,
                new Rectangle(
                    PaddleOffset,
                    this._paddleSet.PaddleLeft.Position.Y / 2 - this._paddleSet.PaddleLeft.Height / 2,
                    this._paddleSet.PaddleLeft.Width,
                    this._paddleSet.PaddleLeft.Height
                )
            );
            this._paddleSet.PaddleRight.Draw(gameTime, spriteBatch,
                new Rectangle(
                    this._paddleSet.PaddleRight.Position.X * 2 - PaddleOffset,
                    this._paddleSet.PaddleRight.Position.Y / 2 - this._paddleSet.PaddleRight.Height / 2,
                    this._paddleSet.PaddleRight.Width,
                    this._paddleSet.PaddleRight.Height
                )
            );
        }

        public override void Update(GameTime gameTime)
        {
            throw new NotImplementedException();
        }

        public void UpdateGameFieldSize(Rectangle rectangle)
        {
            this.box = rectangle;
        }

        public void Move()
        {
            var winner = this._ball.Move();
            this.Score.SetScore(winner);

        }
    }
}
