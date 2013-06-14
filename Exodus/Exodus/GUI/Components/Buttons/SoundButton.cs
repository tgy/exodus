using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Exodus.GUI.Components.Buttons
{
    class SoundButton : Component
    {
        public delegate void OnClick(int i);
        public OnClick DoClick = null;
        Texture2D _button,
                  _textureProgress;
        Rectangle textureProgressTotArea,
                  textureProgressArea;
        int _firstX = 8, _firstY = 7,
            _lastX = 250, _lastY = 38;
        public float Progress
        {
            get
            {
                return _Progress;
            }
            set
            {
                _Progress = value;
                if (_Progress > 1)
                    _Progress = 1;
                else if (_Progress < 0)
                    _Progress = 0;
                textureProgressArea.Width = (int)(_Progress * textureProgressTotArea.Width);
            }
        }
        // 0 <= _P <= 1
        float _Progress,
              _layerDepth;
        bool clicking = false;
        public SoundButton(int x, int y, Padding p, float layerDepth)
        {
            _button = Textures.Menu["SoundButton"];
            _textureProgress = Textures.Menu["SoundButtonProgress"];
            Area = new Rectangle(x, y, _button.Width, _button.Height);
            this.Padding = p;
            textureProgressTotArea = new Rectangle(x + _firstX, y + _firstY, _lastX - _firstX, _lastY - _firstX);
            textureProgressArea = new Rectangle(textureProgressTotArea.X, textureProgressTotArea.Y, 0, textureProgressTotArea.Height);
            this._layerDepth = layerDepth;
        }
        public override void SetPosition()
        {
            textureProgressTotArea.X = Area.X + _firstX;
            textureProgressTotArea.Y = Area.Y + _firstY;
            textureProgressArea.X = textureProgressTotArea.X;
            textureProgressArea.Y = textureProgressTotArea.Y;
        }
        public override void Update(GameTime gameTime)
        {
            if (Inputs.MouseState.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed && textureProgressTotArea.Contains(Inputs.MouseState.X, Inputs.MouseState.Y))
                clicking = true;
            else if (clicking && Inputs.MouseState.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Released)
                clicking = false;
            if (clicking)
            {
                Progress = ((float)Inputs.MouseState.X - (float)textureProgressTotArea.X) / (float)textureProgressTotArea.Width;
                if (DoClick != null)
                    DoClick((int)(Progress * 100));
            }
            base.Update(gameTime);
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_button, Area, null, Color.White, 0f, Vector2.Zero, SpriteEffects.None, _layerDepth);
            spriteBatch.Draw(_textureProgress, textureProgressArea, null, Color.White, 0f, Vector2.Zero, SpriteEffects.None, _layerDepth + Data.GameDisplaying.Epsilon);
        }
    }
}
