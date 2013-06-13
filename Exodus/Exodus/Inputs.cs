using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Exodus
{
    static class Inputs
    {
        static public MouseState MouseState;
        static public MouseState PreMouseState;
        static public KeyboardState KeyboardState;
        static public KeyboardState PreKeyboardState;
        static int lastDoubleRightClick;
        static int lastDoubleLeftClick;
        static bool changeL, changeR;
        static public void Initialise()
        {
            MouseState = Mouse.GetState();
            KeyboardState = Keyboard.GetState();
            PreMouseState = MouseState;
            PreKeyboardState = KeyboardState;
            lastDoubleLeftClick = 2 * Data.Config.TimerDoubleClick;
            lastDoubleRightClick = lastDoubleLeftClick;
            changeL = false;
            changeR = false;
        }
        static public void Update(GameTime gameTime)
        {
            PreMouseState = MouseState;
            PreKeyboardState = KeyboardState;
            MouseState = Mouse.GetState();
            KeyboardState = Keyboard.GetState();
            lastDoubleLeftClick += (int)gameTime.ElapsedGameTime.TotalMilliseconds;
            lastDoubleRightClick += (int)gameTime.ElapsedGameTime.TotalMilliseconds;
            if (changeL)
                lastDoubleLeftClick = 0;
            if (changeR)
                lastDoubleRightClick = 0;
            changeL = LeftClick();
            changeR = RightClick();
        }
        static public bool LeftClick()
        {
            return Data.Window.GameFocus && MouseState.LeftButton == ButtonState.Released && PreMouseState.LeftButton == ButtonState.Pressed;
        }
        static public bool DoubleLeftClick()
        {
            return (LeftClick() && lastDoubleLeftClick > 0 && lastDoubleLeftClick <= Data.Config.TimerDoubleClick);
        }
        static public bool RightClick()
        {
            return Data.Window.GameFocus && MouseState.RightButton == ButtonState.Released && PreMouseState.RightButton == ButtonState.Pressed;
        }
        static public bool DoubleRightClick()
        {
            return (RightClick() && lastDoubleRightClick <= Data.Config.TimerDoubleClick);
        }
        static public bool KeyPress(Keys k)
        {
            return PreKeyboardState.IsKeyDown(k) && KeyboardState.IsKeyUp(k);
        }
    }
}
