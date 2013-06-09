using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Exodus.GUI.Components.Buttons.GameButtons
{
    class Mini : GameButton
    {
        public Texture2D Texture;
        public Texture2D TextureHover;
        public new delegate void OnClick(Type t);
        public new OnClick DoClick;
        public Type Type;
        public Mini(OnClick doClick, Type parameter, Texture2D texture, Texture2D textureHover, int x, int y)
        {
            Area = new Rectangle(x, y, Textures.GameUI["smallItem"].Width, Textures.GameUI["smallItem"].Height);
            Texture = texture;
            TextureHover = textureHover;
            DoClick = doClick;
            Type = parameter;
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            if (Texture != null)
                spriteBatch.Draw(Area.Contains(Inputs.MouseState.X, Inputs.MouseState.Y) ? TextureHover : Texture,
                                 Area, null,
                                 Color.White, 0f, Vector2.Zero, SpriteEffects.None, 10 * Data.GameDisplaying.Epsilon);
            spriteBatch.Draw(Textures.GameUI["smallItem"], Area, null,
                                 Color.White, 0f, Vector2.Zero, SpriteEffects.None, 5 * Data.GameDisplaying.Epsilon);
        }
        public override void Update(GameTime gameTime)
        {
            if (Focused && Inputs.LeftClick())
                DoClick(Type);
        }
    }
}
