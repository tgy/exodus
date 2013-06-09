using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Exodus.GUI.Items
{
    class Patterned : Item
    {
        private Texture2D _texture;
        public Patterned(Texture2D texture, int x, int y)
        {
            _texture = texture;
            Area = new Rectangle(x, y, Data.Window.WindowWidth, texture.Height);
        }

        public override void Update(GameTime gameTime)
        {
            
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            int currentPosition = 0;
            while (currentPosition < Data.Window.WindowWidth)
            {
                spriteBatch.Draw(_texture, new Rectangle(currentPosition, Area.Y, _texture.Width, _texture.Height), null,
                                 Color.White, 0f, Vector2.Zero, SpriteEffects.None, 42*Data.GameDisplaying.Epsilon);
                currentPosition += _texture.Width;
            }
        }
    }
}
