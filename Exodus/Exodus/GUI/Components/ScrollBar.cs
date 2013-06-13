using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Exodus.GUI.Components
{
    public class ScrollBar : Component
    {
        private Rectangle _areaSmallThing;
        public double Value;
        private bool _isClicking;
        private readonly Texture2D _texture;
        public ScrollBar(Rectangle parentArea, int totalElements, int displayedElements)
        {
            Area = new Rectangle(parentArea.X + parentArea.Width, parentArea.Y, 8, parentArea.Height);

            
            _areaSmallThing = new Rectangle(Area.X, Area.Y, Area.Width, totalElements > 0 ? Math.Min(Area.Height * displayedElements / totalElements, Area.Height) : 0);

            Value = 0;
            _texture = Textures.Menu["SmallThing"];
        }
        public void Reset(int totalElements, int displayedElements)
        {
            _areaSmallThing.Height = totalElements > 0 && totalElements > displayedElements ? Math.Min(Area.Height * displayedElements / totalElements, Area.Height) : 0;
        }
        public override void Update(GameTime gameTime)
        {

            if (Focused && Inputs.MouseState.LeftButton == ButtonState.Pressed)
                _isClicking = true;
            if (_isClicking && Inputs.MouseState.LeftButton == ButtonState.Released)
                _isClicking = false;
            if (_isClicking)
            {
                Focused = true;
                _areaSmallThing.Y = Inputs.MouseState.Y >= Area.Y ? Inputs.MouseState.Y <= Area.Y + Area.Height - _areaSmallThing.Height ? Inputs.MouseState.Y : Area.Y + Area.Height - _areaSmallThing.Height : Area.Y;
                Value = (double)(_areaSmallThing.Y - Area.Y) / (double)Area.Height;
            }
            else
            {
                _areaSmallThing.Y = (int)(Area.Y + Value * Area.Height);
            }
        }
        public override void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, _areaSmallThing, null, Color.White, 0f, Vector2.Zero, SpriteEffects.None, float.Epsilon);
        }
        public void SetPosition(Rectangle parentArea)
        {
            Area.X = parentArea.X + parentArea.Width;
            int delta = Area.Y - parentArea.Y;
            Area.Y = parentArea.Y;
            _areaSmallThing.X = Area.X;
            _areaSmallThing.Y -= delta;
        }
    }
}