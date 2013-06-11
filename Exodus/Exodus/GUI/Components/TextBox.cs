using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Exodus.GUI.Components
{
    public class TextBox : Component
    {
        private short _periodEvolveRepeateChar;
        private short _periodEvolveCursor;
        private short _periodEvolveCursorVisibility;

        private short _cursorIndex;

        public string Value;
        public string Label;
        private string _displayedValue;

        private bool _cursorVisible;

        private short _displayedIndexBegin, _displayedIndexEnd;

        private Vector2 _textPosition, _labelPosition;

        public Texture2D _texture { get; private set; }
        public Texture2D _textureFocused { get; private set; }
        public Texture2D _cursorTexture { get; private set; }
        private SpriteFont _font;
        private readonly int _charDisplayed;
        public Color color { get; private set; }
        public bool Hidden = false;
        private Color _unfocusedColor;
        public bool ValueIsNumber = false;

        public TextBox(int x, int y, string label, string kind, Padding padding, int charDisplayed, float depth)
        {
            Depth = depth;
            HoverFocus = false;
            ClickFocus = true;
            Value = "";
            Label = label;
            _displayedIndexBegin = 0;
            _displayedIndexEnd = 0;
            _displayedValue = "";

            _periodEvolveCursor = 10;
            _periodEvolveRepeateChar = 10;
            _periodEvolveCursorVisibility = 0;

            _cursorVisible = true;
            _cursorIndex = 0;
            _texture = Textures.Menu[kind];
            _textureFocused = Textures.Menu[kind + "Hover"];
            _cursorTexture = Textures.Menu[kind + "Cursor"];
            Padding = padding;
            Area = new Rectangle(x + Padding.Left, y + Padding.Top, _texture.Width, _texture.Height);
            _font = Fonts.Eurostile12;
            _charDisplayed = charDisplayed;
            maxChars = _charDisplayed;
            SetColor(255, 255, 255);
        }
        public override void SetPosition()
        {
            _textPosition = new Vector2(Area.X + Area.Width/2,
                                        Area.Y + Padding.Top);

            _labelPosition = new Vector2(Area.X + (Area.Width - (int) _font.MeasureString(Label).X)/2,
                                         Area.Y + Padding.Top);
            base.SetPosition();
        }
        public void SetPosition(int x, int y)
        {
            Area = new Rectangle(x + Padding.Left, y + Padding.Top, _texture.Width, _texture.Height);

            _textPosition = new Vector2(Area.X + Area.Width/2,
                                        Area.Y + Padding.Top);

            _labelPosition = new Vector2(Area.X + (Area.Width - (int) _font.MeasureString(Label).X)/2,
                                         Area.Y + Padding.Top);
        }
        private static char _keyToChar(Keys k)
        {
            switch (k)
            {
                case Keys.OemPeriod:
                case Keys.Decimal:
                    return '.';
                case Keys.OemComma:
                    return ',';
                case Keys.Space:
                    return ' ';

                    #region test-0-9

                case Keys.D0:
                case Keys.NumPad0:
                    return '0';
                case Keys.D1:
                case Keys.NumPad1:
                    return '1';
                case Keys.D2:
                case Keys.NumPad2:
                    return '2';
                case Keys.D3:
                case Keys.NumPad3:
                    return '3';
                case Keys.D4:
                case Keys.NumPad4:
                    return '4';
                case Keys.D5:
                case Keys.NumPad5:
                    return '5';
                case Keys.D6:
                case Keys.NumPad6:
                    return '6';
                case Keys.D7:
                case Keys.NumPad7:
                    return '7';
                case Keys.D8:
                case Keys.NumPad8:
                    return '8';
                case Keys.D9:
                case Keys.NumPad9:
                    return '9';

                    #endregion

                    #region test a-z

                case Keys.A:
                    return 'a';
                case Keys.B:
                    return 'b';
                case Keys.C:
                    return 'c';
                case Keys.D:
                    return 'd';
                case Keys.E:
                    return 'e';
                case Keys.F:
                    return 'f';
                case Keys.G:
                    return 'g';
                case Keys.H:
                    return 'h';
                case Keys.I:
                    return 'i';
                case Keys.J:
                    return 'j';
                case Keys.K:
                    return 'k';
                case Keys.L:
                    return 'l';
                case Keys.M:
                    return 'm';
                case Keys.N:
                    return 'n';
                case Keys.O:
                    return 'o';
                case Keys.P:
                    return 'p';
                case Keys.Q:
                    return 'q';
                case Keys.R:
                    return 'r';
                case Keys.S:
                    return 's';
                case Keys.T:
                    return 't';
                case Keys.U:
                    return 'u';
                case Keys.V:
                    return 'v';
                case Keys.W:
                    return 'w';
                case Keys.X:
                    return 'x';
                case Keys.Y:
                    return 'y';
                case Keys.Z:
                    return 'z';

                    #endregion

                default:
                    return '$';
            }
        }
        private static char _keyToInt(Keys k)
        {
            switch (k)
            {
                case Keys.OemComma:
                    return ',';

                    #region test-0-9

                case Keys.D0:
                case Keys.NumPad0:
                    return '0';
                case Keys.D1:
                case Keys.NumPad1:
                    return '1';
                case Keys.D2:
                case Keys.NumPad2:
                    return '2';
                case Keys.D3:
                case Keys.NumPad3:
                    return '3';
                case Keys.D4:
                case Keys.NumPad4:
                    return '4';
                case Keys.D5:
                case Keys.NumPad5:
                    return '5';
                case Keys.D6:
                case Keys.NumPad6:
                    return '6';
                case Keys.D7:
                case Keys.NumPad7:
                    return '7';
                case Keys.D8:
                case Keys.NumPad8:
                    return '8';
                case Keys.D9:
                case Keys.NumPad9:
                    return '9';

                    #endregion

                default:
                    return '$';
            }
        }
        public int maxChars;
        public override void Update(GameTime gameTime)
        {
            if (!Focused)
                return;
            _periodEvolveCursorVisibility++;
            if (Inputs.PreKeyboardState.GetPressedKeys().Length > 0)
                _periodEvolveCursorVisibility = 41;
            _cursorVisible = _periodEvolveCursorVisibility > 40;
            if (_periodEvolveCursorVisibility == 65)
                _periodEvolveCursorVisibility = 0;

            foreach (var pressedKey in Inputs.PreKeyboardState.GetPressedKeys())
            {
                var inputChar = ValueIsNumber ? _keyToInt(pressedKey) : _keyToChar(pressedKey);

                if (inputChar > 96 && inputChar < 123 &&
                    (Inputs.PreKeyboardState.GetPressedKeys().Contains(Keys.LeftShift) ||
                     Inputs.PreKeyboardState.GetPressedKeys().Contains(Keys.RightShift)))
                    inputChar = (char) (inputChar - 32);

                if (inputChar != '$' &&
                    ((Inputs.KeyboardState.IsKeyUp(pressedKey) ||
                      _periodEvolveRepeateChar == Data.Config.textBoxPeriodRepetition) &&
                     Inputs.PreKeyboardState.IsKeyDown(pressedKey)))
                {
                    if (Value.Length < maxChars)
                    {
                        if (_displayedIndexBegin == 0 && _displayedIndexEnd == Value.Length &&
                            Value.Length < _charDisplayed)
                        {
                            Value = Value.Insert(_cursorIndex, inputChar.ToString());

                            if (Value.Length <= _charDisplayed)
                                _cursorIndex++;

                            _displayedIndexEnd++;
                        }

                        else if (_cursorIndex == _charDisplayed)
                        {
                            Value = Value.Insert(_displayedIndexBegin + _cursorIndex, inputChar.ToString());

                            _displayedIndexBegin++;
                            _displayedIndexEnd++;
                        }

                        else
                        {
                            Value = Value.Insert(_displayedIndexBegin + _cursorIndex, inputChar.ToString());

                            _cursorIndex++;
                        }

                        _periodEvolveRepeateChar = 0;
                    }
                }

                else
                {
                    if ((Inputs.KeyboardState.IsKeyUp(Keys.Right) ||
                         _periodEvolveCursor == Data.Config.textBoxPeriodRepetitionCursor) &&
                        Inputs.PreKeyboardState.IsKeyDown(Keys.Right))
                    {
                        if (_cursorIndex < _displayedIndexEnd - _displayedIndexBegin)
                            _cursorIndex++;

                        else if (_displayedIndexEnd < Value.Length - 1)
                        {
                            _displayedIndexBegin++;
                            _displayedIndexEnd++;
                        }

                        _periodEvolveCursor = 0;
                    }

                    if ((Inputs.KeyboardState.IsKeyUp(Keys.Left) ||
                         _periodEvolveCursor == Data.Config.textBoxPeriodRepetitionCursor) &&
                        Inputs.PreKeyboardState.IsKeyDown(Keys.Left))
                    {
                        if (_cursorIndex > 0)
                            _cursorIndex--;

                        else if (_displayedIndexBegin > 0)
                        {
                            _displayedIndexBegin--;
                            _displayedIndexEnd--;
                        }

                        _periodEvolveCursor = 0;
                    }

                    if ((Inputs.KeyboardState.IsKeyUp(Keys.Back) ||
                         _periodEvolveRepeateChar == Data.Config.textBoxPeriodRepetition) &&
                        Inputs.PreKeyboardState.IsKeyDown(Keys.Back) && Value.Length > 0)
                    {
                        if (_displayedIndexBegin == 0 && _displayedIndexEnd == Value.Length &&
                            Value.Length <= _charDisplayed && _cursorIndex > 0)
                        {
                            Value = Value.Remove(_cursorIndex - 1, 1);

                            _cursorIndex--;

                            _displayedIndexEnd--;
                        }

                        else if (_cursorIndex != 0)
                        {
                            Value = Value.Remove(_displayedIndexBegin + _cursorIndex - 1, 1);

                            _displayedIndexEnd--;
                            _displayedIndexBegin--;
                        }

                        _periodEvolveRepeateChar = 0;
                    }
                }

                if (Inputs.KeyboardState.IsKeyDown(Keys.Left) || Inputs.KeyboardState.IsKeyDown(Keys.Right))
                    _periodEvolveCursor++;

                if (Inputs.PreKeyboardState.IsKeyDown(pressedKey))
                    _periodEvolveRepeateChar++;
            }

            _displayedValue = Value.Substring(_displayedIndexBegin, _displayedIndexEnd - _displayedIndexBegin);

            _textPosition.X = Area.X + (Area.Width - (int) _font.MeasureString(_displayedValue).X)/2;
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Focused ? _textureFocused : _texture, Area, null,
                             Color.White, 0f, Vector2.Zero, SpriteEffects.None, Depth);

            spriteBatch.DrawString(_font, Hidden ? HideString(_displayedValue) : _displayedValue, _textPosition, Focused ? color : _unfocusedColor, 0f,
                                   Vector2.Zero, 1f, SpriteEffects.None, Depth - Data.GameDisplaying.Epsilon);

            if (_cursorVisible && Focused)
                spriteBatch.Draw(_cursorTexture,
                                 new Vector2(
                                     _textPosition.X +
                                     _cursorIndex*
                                     (_cursorIndex == 0
                                          ? 0
                                          : (_font.MeasureString(Hidden ? HideString(_displayedValue) : _displayedValue).X /
                                             _displayedValue.Length)), _textPosition.Y), null, Color.White, 0f,
                                 Vector2.Zero,
                                 1f,
                                 SpriteEffects.None,
                                 Depth - Data.GameDisplaying.Epsilon);
            spriteBatch.DrawString(_font, new StringBuilder(Label), _labelPosition, new Color(138, 0, 255), 0f,
                                   Vector2.Zero, 1f, SpriteEffects.None, Depth - Data.GameDisplaying.Epsilon);
        }
        string HideString(string s)
        {
            string result = "";
            for (int i = 0; i < s.Length; i++)
                result += '*';
            return result;
        }
        public void ResetValue()
        {
            _displayedIndexBegin = 0;
            _displayedIndexEnd = 0;
            _cursorIndex = 0;
            Value = "";
        }
        public void SetColor(int R, int G, int B)
        {
            color = new Color(R, G, B);
            _unfocusedColor = new Color(4*R/5, 4*G/5, 4*B/5);
        }
    }
}