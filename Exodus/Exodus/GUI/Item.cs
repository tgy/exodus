using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Exodus.GUI
{
    public abstract class Item
    {
        public bool Focused = false;
        public bool IsVisible = true;
        public int SelectedComponent;
        public List<Component> Components = new List<Component>();

        public Rectangle Area;
        public float Depth;
        public Padding Padding;

        public virtual void LoadContent() { }

        public virtual void Initialize()
        {
        }

        public virtual void Update(GameTime gameTime)
        {
            foreach (var c in Components)
            {
                if (c.ClickFocus)
                {
                    if (Inputs.LeftClick())
                        c.Focused = c.Area.Contains(Inputs.MouseState.X, Inputs.MouseState.Y);
                }
                else
                    c.Focused = c.Area.Contains(Inputs.MouseState.X, Inputs.MouseState.Y);

                c.Update(gameTime);
            }
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < Components.Count; i++)
                Components[i].Draw(spriteBatch);
        }
    }
}
