using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Exodus.GUI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;

namespace Exodus
{
    public abstract class GameState
    {
        public List<Item> Items = new List<Item>();
        public SoundEffectInstance Music;

        public virtual void Initialize()
        {
        }

        public virtual void LoadContent() { }

        public virtual void Update(GameTime gameTime)
        {
            bool b = false;
            for (int i = Items.Count - 1; i >= 0; i--)
            {
                if (!Items[i].IsVisible) continue;
                if (Items[i].Area.Contains(Inputs.MouseState.X, Inputs.MouseState.Y) && !b)
                {
                    Items[i].Focused = true;
                    b = true;
                }

                else
                    Items[i].Focused = false;

                Items[i].Update(gameTime);
            }
        }
        public virtual void UnLoad()
        {

        }
        public virtual void Draw(SpriteBatch spriteBatch)
        {
            foreach (var item in Items.Where(item => item.IsVisible))
                item.Draw(spriteBatch);
        }
    }
}
