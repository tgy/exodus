using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Exodus.GUI.Components
{
    public class Label : Component
    {
        public string Txt;
        readonly SpriteFont _font;
        public Vector2 Pos;
        public Color Color;
        public Label(SpriteFont font, string txt, int x, int y)
        {
            Txt = txt;
            _font = font;
            Area = new Rectangle(0, 0, 0, 0);
            Pos = new Vector2(x, y);
            Color = Color.White;
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(_font, Txt, Pos, Color, 0f, Vector2.Zero, 1f, SpriteEffects.None, float.Epsilon);
        }
        public void SetColor(int R, int G, int B)
        {
            Color = new Color(R, G, B);
        }
        public override void SetPosition()
        {
            Pos = new Vector2(Area.X, Area.Y);
            base.SetPosition();
        }
    }
}
