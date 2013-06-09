using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Exodus.GUI.Components.Buttons.GameButtons
{
    class StandardButton : GameButton
    {
        Texture2D Texture;
        Texture2D TextureHover;
        public StandardButton(OnClick DoClick, Texture2D texture, Texture2D textureHover, int x, int y)
        {
            Area = new Rectangle(x, y, texture.Width, texture.Height);
            Texture = texture;
            TextureHover = textureHover;
            this.DoClick = DoClick;
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            if (Texture != null)
                spriteBatch.Draw(Area.Contains(Inputs.MouseState.X, Inputs.MouseState.Y) ? TextureHover : Texture,
                                 Area, null,
                                 Color.White, 0f, Vector2.Zero, SpriteEffects.None, 10 * Data.GameDisplaying.Epsilon);
        }
    }
}
