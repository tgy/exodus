using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Exodus.GUI.Components
{
    internal class MiniLife : Component
    {
        public int Value
        {
            get { return 0; }
            set
            {
                _spriteRect.Y = value/26;
            }
        }

        private readonly Texture2D _lifeBar;
        private Rectangle _spriteRect;

        public MiniLife(int x, int y, float depth)
        {
            _lifeBar = Textures.GameUI["smallLifeBar"];
            Area = new Rectangle(x, y, 24, 1);
            _spriteRect = new Rectangle(0, 0, _lifeBar.Width, _lifeBar.Height/4);
            Depth = depth;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_lifeBar, Area, _spriteRect, Color.White, 0f, Vector2.Zero, SpriteEffects.None, Depth);
        }
    }
}
