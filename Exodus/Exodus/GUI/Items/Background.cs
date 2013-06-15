using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Exodus.GUI.Items
{
    public class Background : Item
    {
        private readonly Texture2D _texture;
        private float _depth = 1f;

        public Background(Texture2D texture, float depth)
            : this(texture)
        {
            _depth = depth;
        }
        public Background(Texture2D texture)
        {
            _texture = texture;
            Focused = false;
            Area = new Rectangle((Data.Window.WindowWidth - texture.Width) / 2, (Data.Window.WindowHeight - texture.Height) / 2,
                                 texture.Width, texture.Height);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, Area, null, Color.White, 0f, Vector2.Zero, SpriteEffects.None, _depth);
        }
    }
}
