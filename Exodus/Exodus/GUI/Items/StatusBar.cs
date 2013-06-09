using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Exodus.GUI.Items
{
    public class StatusBar : Item
    {
        public Texture2D Texture;
        public int Speed, Timer, AnimationState;
        public Rectangle DisplayedArea;
        private bool _animated;

        public string Text;
        public Vector2 TextPosition;
        public SpriteFont Font;
        public Color TextColor;

        public StatusBar(int x, int y)
        {
            Texture = Textures.Menu["StatusBar"];
            Area = new Rectangle(x, y, Texture.Width, Texture.Height / 6);
            AnimationState = 0;
            DisplayedArea = new Rectangle(0, 0, Texture.Width, Area.Height);
            Speed = 300;
            Timer = Speed;
            _animated = true;
            Text = "LOADING";
            TextColor = Color.White;
            Font = Fonts.Eurostile12;
            TextPosition = new Vector2((int)(Area.X + (Area.Width - Font.MeasureString(Text).X) / 2),
                                       (int)(Area.Y + (Area.Height - Font.MeasureString(Text).Y) / 2));
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            base.Update(gameTime);
            if (!_animated) return;
            if (Timer <= 0)
            {
                AnimationState = (AnimationState + 1)%6;
                Timer = Speed;
                DisplayedArea.Y = AnimationState*Area.Height + AnimationState;
            }

            else
            {
                Timer -= (int) gameTime.ElapsedGameTime.TotalMilliseconds;
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, Area, DisplayedArea, Color.White, 0f, Vector2.Zero, SpriteEffects.None, 2 * float.Epsilon);
            spriteBatch.DrawString(Font, Text, TextPosition, TextColor, 0f, Vector2.Zero, 1f, SpriteEffects.None, float.Epsilon);
        }

        public void ToggleAnimation()
        {
            if (_animated)
            {
                _animated = false;
                AnimationState = 0;
                Timer = Speed;
            }

            else
                _animated = true;
        }
    }
}
