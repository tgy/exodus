using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Exodus.GameStates;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Exodus.GUI.Components
{
    class MultiButton : Button
    {
        public delegate void OnClick(int i);
        public OnClick DoClick = null;
        Texture2D _buttonTexture;
        List<Rectangle> _areas = new List<Rectangle>();
        Rectangle _sourceRectangle;
        public Color Color = Color.White;
        int _maxValue;

        public MultiButton(int x, int y, string buttonTexture, List<int> Areas)
        {
            Value = 0;
            _maxValue = Areas.Count;
            Area.X = x;
            Area.Y = y;
            _buttonTexture = Textures.Menu[buttonTexture];
            Area.Width = _buttonTexture.Width;
            Area.Height = _buttonTexture.Height / _maxValue;
            _sourceRectangle = new Rectangle(0, 0, Area.Width, Area.Height);
            if (Areas.Count == 0)
                _areas.Add(Area);
            else
            {
                _areas.Add(new Rectangle(Area.X, Area.Y, Areas[0], Area.Height));
                for (int i = 1; i < Areas.Count; i++)
                    _areas.Add(new Rectangle(_areas[i - 1].X + _areas[i - 1].Width, Area.Y, Areas[i], Area.Height));
            }
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            base.Update(gameTime);
            if (Focused && Inputs.MouseState.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed)
            {
                for (int i = 0; i < _areas.Count; i++)
                    if (_areas[i].Contains(Inputs.MouseState.X, Inputs.MouseState.Y))
                    {
                        Value = i;
                        break;
                    }
                _sourceRectangle.Y = Value * _sourceRectangle.Height;
                if (DoClick != null)
                    DoClick(Value);
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_buttonTexture,
                             Area,
                             _sourceRectangle,
                             Color.White,
                             0f,
                             Vector2.Zero,
                             SpriteEffects.None,
                             2 * float.Epsilon);
            base.Draw(spriteBatch);
        }
        public override void SetPosition()
        {
            _areas[0] = new Rectangle(Area.X, Area.Y, _areas[0].Width, _areas[0].Height);
            for (int i = 1; i < _areas.Count; i++)
            {
                _areas[i] = new Rectangle(_areas[i - 1].X + _areas[i - 1].Width, Area.Y, _areas[i].Width, _areas[i].Height);
            }
        }
    }
}
