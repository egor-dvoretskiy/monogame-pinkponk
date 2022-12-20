using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PinkPonk.Source.Abstract
{
    public abstract class Component
    {
        public abstract int Width { get; }

        public abstract int Height { get; }

        public abstract void Draw(GameTime gameTime, SpriteBatch spriteBatch, Rectangle vector);

        public abstract void Update(GameTime gameTime);
    }
}
