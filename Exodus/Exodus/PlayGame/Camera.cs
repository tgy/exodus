using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Exodus.PlayGame
{
    static public class Camera
    {
        public static int x { get; private set; }
        public static int y { get; private set; }
        public static int minX { get; private set; }
        public static int maxX { get; private set; }
        public static int minY { get; private set; }
        public static int maxY { get; private set; }
        static int AreaMoving = 10;
        static int SuperAreaMoving = 0;
        static int Step = 20;
        static int SuperStep = 40;
        public static void Initialise(int miniX, int maxiX, int miniY, int maxiY)
        {
            minX = miniX;
            maxX = maxiX;
            minY = miniY;
            maxY = maxiY;
            x = minX;
            y = minY;
        }
        public static void Update(GameTime gameTime)
        {
            GameMouse.MouseDir dir = GameMouse.MouseDir.Normal;
            if ((Inputs.MouseState.X >= 0 && Inputs.MouseState.X <= AreaMoving) || Inputs.KeyboardState.IsKeyDown(Keys.Left))
            {
                if (Inputs.MouseState.X <= SuperAreaMoving)
                    x -= SuperStep;
                else
                    x -= Step;
                if (x < minX)
                    x = minX;
                dir = GameMouse.MouseDir.Left;
            }
            else if ((Inputs.MouseState.X <= Data.Window.WindowWidth && Inputs.MouseState.X >= Data.Window.WindowWidth - AreaMoving) || Inputs.KeyboardState.IsKeyDown(Keys.Right))
            {
                if (Inputs.MouseState.X >= Data.Window.WindowWidth - SuperStep)
                    x += SuperStep;
                else
                    x += Step;
                if (x > maxX)
                    x = maxX;
                dir = GameMouse.MouseDir.Right;
            }
            if ((Inputs.MouseState.Y >= 0 && Inputs.MouseState.Y <= AreaMoving) || Inputs.KeyboardState.IsKeyDown(Keys.Up))
            {
                if (Inputs.MouseState.Y <= SuperStep)
                    y -= SuperStep;
                else
                    y -= Step;
                if (y < minY)
                    y = minY;
                switch (dir)
                {
                    case GameMouse.MouseDir.Left:
                        dir = GameMouse.MouseDir.TopLeft;
                        break;
                    case GameMouse.MouseDir.Right:
                        dir = GameMouse.MouseDir.TopRight;
                        break;
                    default:
                        dir = GameMouse.MouseDir.Top;
                        break;
                }
            }
            else if ((Inputs.MouseState.Y <= Data.Window.WindowHeight && Inputs.MouseState.Y >= Data.Window.WindowHeight - AreaMoving) || Inputs.KeyboardState.IsKeyDown(Keys.Down))
            {
                if (Inputs.MouseState.Y >= Data.Window.WindowHeight - SuperAreaMoving - 1)
                    y += SuperStep;
                else
                    y += Step;
                if (y > maxY)
                    y = maxY;
                switch (dir)
                {
                    case GameMouse.MouseDir.Left:
                        dir = GameMouse.MouseDir.BottomLeft;
                        break;
                    case GameMouse.MouseDir.Right:
                        dir = GameMouse.MouseDir.BottomRight;
                        break;
                    default:
                        dir = GameMouse.MouseDir.Bottom;
                        break;
                }
            }
            GameMouse.Reset(dir);
        }
    }
}
