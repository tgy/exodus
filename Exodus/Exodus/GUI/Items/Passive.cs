using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Exodus.GUI.Items
{
    public class Passive : Item
    {
        private readonly Texture2D _texture;
        float layerDepth;
        public Passive(Texture2D texture, int x, int y, float layerDepth)
        {
            _texture = texture;
            Focused = false;
            Area = new Rectangle(x, y, texture.Width, texture.Height);
            this.layerDepth = layerDepth;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, Area, null, Color.White, 0f, Vector2.Zero, SpriteEffects.None, layerDepth);
            base.Draw(spriteBatch);
        }
    }
}
