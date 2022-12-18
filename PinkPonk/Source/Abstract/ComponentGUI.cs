using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PinkPonk.Source.Abstract
{
    public abstract class ComponentGUI
    {
        public abstract float Width { get; }

        public abstract float Height { get; }

        public abstract void Draw(GameTime gameTime, SpriteBatch spriteBatch, Vector2 vector);

        public abstract void Update(GameTime gameTime);
    }
}
