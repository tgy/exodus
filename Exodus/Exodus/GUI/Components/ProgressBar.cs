using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Exodus.GUI.Components
{
    internal class ProgressBar : Component
    {
        public float Progression;

        private Texture2D _background, _gradient;

        /// <summary>
        /// Instanciate a progress bar
        /// </summary>
        /// <param name="background">1px large gradient texture displayed continuously</param>
        /// <param name="gradient">little gradient to give an effet. Set to null to disable it</param>
        /// <param name="x">position X</param>
        /// <param name="y">position Y</param>
        /// <param name="width">progress bar width</param>
        /// <param name="depth">depth</param>
        public ProgressBar(Texture2D background, Texture2D gradient, int x, int y, int width, float depth)
        {
            _background = background;
            _gradient = gradient;
            Area = new Rectangle(x, y, width, background.Height);
            Progression = 0;
            Depth = depth;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (Progression == 0)
                return;
            int width = (int)(Area.Width*Progression)/100;
            spriteBatch.Draw(_background, new Rectangle(Area.X, Area.Y, width, Area.Height), null, Color.White, 0f,
                             Vector2.Zero, SpriteEffects.None, Depth);
            if (_gradient.Width < width)
                spriteBatch.Draw(_gradient,
                                 new Rectangle(Area.X + width - _gradient.Width + 3, Area.Y, _gradient.Width,
                                               _gradient.Height), null, Color.White, 0f, Vector2.Zero,
                                 SpriteEffects.None, Depth - Data.GameDisplaying.Epsilon);
            else
                spriteBatch.Draw(_gradient,
                                 new Rectangle(Area.X, Area.Y, width,
                                               _gradient.Height),
                                 new Rectangle(_gradient.Width - width, 0, width, _gradient.Height),
                                 Color.White, 0f, Vector2.Zero, SpriteEffects.None, Depth - Data.GameDisplaying.Epsilon);
        }
    }
}
