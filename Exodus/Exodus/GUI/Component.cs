using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Exodus.GUI
{
    public abstract class Component
    {
        public bool Focused = false;

        public bool HoverFocus = true, ClickFocus = false;

        public Rectangle Area;
        public float Depth;
        public Padding Padding;

        public virtual void Initialize() { }
        public virtual void LoadContent() { }
        public virtual void Update(GameTime gameTime)
        {

        }
        public virtual void Draw(SpriteBatch spriteBatch) { }
        public virtual void SetPosition() { }
    }
}
