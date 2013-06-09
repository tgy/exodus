using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Exodus.GUI.Components
{
    public class BigLife : Component
    {
        public int Value
        {
            get { return 0; }
            set
            {
                _spriteRect.Y = value/25 * 5;
                Area.Width = (value*64)/100;
            }
        }

        private readonly Texture2D _lifeBar;
        private Rectangle _spriteRect;

        public BigLife(int x, int y, float depth)
        {
            Area = new Rectangle(x, y, 0, 5);
            _lifeBar = Textures.GameUI["bigLifeBar"];
            _spriteRect = new Rectangle(0, 0, _lifeBar.Width, _lifeBar.Height/4);
            Depth = depth;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_lifeBar, Area, _spriteRect, Color.White, 0f, Vector2.Zero, SpriteEffects.None, Depth);
        }
    }
}
