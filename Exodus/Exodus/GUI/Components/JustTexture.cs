using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Exodus.GUI.Components
{
    class JustTexture : Component
    {
        public Texture2D Texture
        {
            private get
            {
                return _texture;
            }
            set
            {
                _texture = value;
                Area.Width = _texture.Width;
                Area.Height = _texture.Height;
            }
        }
        private Texture2D _texture;
        
        public JustTexture(Texture2D texture, int x, int y, float depth)
        {
            Area = new Rectangle(x , y, texture.Width, texture.Height);
            _texture = texture;
            Depth = depth;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, Area, null, Color.White, 0f, Vector2.Zero, SpriteEffects.None, Depth);
        }
    }
}
