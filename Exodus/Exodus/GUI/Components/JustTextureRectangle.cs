using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Exodus.GUI.Components
{
    class JustTextureRectangle : Component
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
        public int Width
        {
            get
            {
                return Area.Width;
            }
            set
            {
                Area.Width = value;
            }
        }
        public int Height
        {
            get
            {
                return Area.Height;
            }
            set
            {
                Area.Height = value;
            }
        }
        private Texture2D _texture;
        public Color c;
        
        public JustTextureRectangle(Texture2D texture, int x, int y, int w, int h, float depth)
        {
            Area = new Rectangle(x, y, w, h);
            _texture = texture;
            Depth = depth;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, Area, null, c, 0f, Vector2.Zero, SpriteEffects.None, Depth);
        }
    }
}
