using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Exodus.GUI.Components.Buttons;
using Exodus.GameStates;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Exodus.GUI.Components
{
    public class MenuButton : ButtonLaunchingMenu
    {
        public List<Texture2D> ButtonTextures = new List<Texture2D>();

        public SpriteFont Font;

        public string Text;

        public Vector2 TextPosition;

        public Color Color;

        public MenuButton()
        {
            Value = 0;
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            if (Focused && Inputs.LeftClick() && DoClick != null)
                DoClick(SubMenu, 0);
            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(ButtonTextures[Focused ? 1 : 0], Area, null, Color.White, 0f,
                             Vector2.Zero,
                             SpriteEffects.None,
                             2*float.Epsilon);
            spriteBatch.DrawString(Font, Text, TextPosition, Color, 0f, Vector2.Zero, 1f, SpriteEffects.None, float.Epsilon);
            base.Draw(spriteBatch);
        }

        public override void SetPosition()
        {
            TextPosition = new Vector2((int) (Area.X + (Area.Width - Font.MeasureString(Text).X)/2),
                                       1 + (int) (Area.Y + (Area.Height - Font.MeasureString(Text).Y)/2));
        }
    }
}
