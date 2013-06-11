using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Exodus.GUI.Items
{
    public class BackgroundFull : Item
    {
        private readonly Texture2D _texture;
        float LayerDepth = 1f; 
        public BackgroundFull(Texture2D texture, float LayerDepth)
            : this(texture)
        {
            this.LayerDepth = LayerDepth;
        }
        public BackgroundFull(Texture2D texture)
        {
            _texture = texture;
            Focused = false;
            Area = new Rectangle(0, 0,
                                 Data.Window.WindowWidth, Data.Window.WindowHeight);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, Area, null, Color.White, 0f, Vector2.Zero, SpriteEffects.None, LayerDepth);
        }
    }
}
