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

namespace Exodus
{
    static public class GameMouse
    {
        public enum MouseDir
        {
            Normal,
            TopLeft,
            Top,
            TopRight,
            Right,
            BottomRight,
            Bottom,
            BottomLeft,
            Left
        }

        static Texture2D _texture;
        public static bool Active = true;
        static Dictionary<MouseDir, Texture2D> listTextures = new Dictionary<MouseDir,Texture2D>();
        public static void Initialize(ContentManager content)
        {
            listTextures[MouseDir.Normal] = content.Load<Texture2D>("Mouses/souris");
            listTextures[MouseDir.Bottom] = content.Load<Texture2D>("Mouses/sourisDown");
            listTextures[MouseDir.BottomLeft] = content.Load<Texture2D>("Mouses/sourisDownLeft");
            listTextures[MouseDir.Left] = content.Load<Texture2D>("Mouses/sourisLeft");
            listTextures[MouseDir.TopLeft] = content.Load<Texture2D>("Mouses/sourisUpLeft");
            listTextures[MouseDir.Top] = content.Load<Texture2D>("Mouses/sourisUp");
            listTextures[MouseDir.TopRight] = content.Load<Texture2D>("Mouses/sourisUpRight");
            listTextures[MouseDir.Right] = content.Load<Texture2D>("Mouses/sourisRight");
            listTextures[MouseDir.BottomRight] = content.Load<Texture2D>("Mouses/sourisDownRight");

            _texture = listTextures[MouseDir.Normal];
        }
        public static void Update(GameTime gameTime)
        {
            Reset(MouseDir.Normal);
        }
        public static void Reset(MouseDir dir)
        {
            _texture = listTextures[dir];
        }
        public static void Draw(SpriteBatch spriteBatch)
        {
            if (Active)
                spriteBatch.Draw(_texture,
                                 new Vector2(Inputs.MouseState.X, Inputs.MouseState.Y),
                                 null,
                                 Color.White,
                                 0f,
                                 Vector2.Zero,
                                 1f,
                                 SpriteEffects.None,
                                 0f);
        }
    }
}
