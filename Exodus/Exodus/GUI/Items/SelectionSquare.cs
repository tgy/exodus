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

namespace Exodus.GUI.Items
{
    class SelectionSquare : Item
    {
        Texture2D _insideTexture,
                  _borderTexture;
        float _layerDepth;
        Vector2 p1, p2;
        bool b1, b2;
        public bool Defined, Drawn;
        public bool Active;
        int minSize;
        public SelectionSquare(string insideTexture, string borderTexture)
        {
            _insideTexture = Textures.Menu[insideTexture];
            _borderTexture = Textures.Menu[borderTexture];
            _layerDepth = 43 * Data.GameDisplaying.Epsilon;
            b1 = false;
            b2 = false;
            p1 = Vector2.Zero;
            p2 = Vector2.Zero;
            Active = true;
            minSize = 5;
            Defined = false;
            Drawn = false;
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            if (Active && b1 && b2)
            {
                Defined = true;
                if (p1.X < p2.X)
                {
                    Area.X = (int)p1.X - PlayGame.Camera.x;
                    Area.Width = (int)(p2.X - p1.X);
                }
                else
                {
                    Area.X = (int)p2.X - PlayGame.Camera.x;
                    Area.Width = (int)(p1.X - p2.X);
                }
                if (p1.Y < p2.Y)
                {
                    Area.Y = (int)p1.Y - PlayGame.Camera.y;
                    Area.Height = (int)(p2.Y - p1.Y);
                }
                else
                {
                    Area.Y = (int)p2.Y - PlayGame.Camera.y;
                    Area.Height = (int)(p1.Y - p2.Y);
                }
                if (Area.Width + Area.Height < minSize)
                    Drawn = false;
                else
                {
                    Drawn = true;
                    // Dessin du fond
                    spriteBatch.Draw(_insideTexture, Area, null, Color.White, 0, new Vector2(0, 0),
                                     SpriteEffects.None, _layerDepth);
                    // Dessin des borders
                    spriteBatch.Draw(_borderTexture,
                                     new Rectangle(Area.X, Area.Y, 1, Area.Height), null,
                                     Color.White, 0, new Vector2(0, 0), SpriteEffects.None, _layerDepth);
                    spriteBatch.Draw(_borderTexture,
                                     new Rectangle(Area.X, Area.Y, Area.Width, 1), null,
                                     Color.White, 0, new Vector2(0, 0), SpriteEffects.None, _layerDepth);
                    spriteBatch.Draw(_borderTexture,
                                     new Rectangle(Area.X + Area.Width, Area.Y, 1,
                                                   Area.Height), null, Color.White, 0, new Vector2(0, 0),
                                     SpriteEffects.None, _layerDepth);
                    spriteBatch.Draw(_borderTexture,
                                     new Rectangle(Area.X, Area.Y + Area.Height,
                                                   Area.Width + 1, 1), null, Color.White, 0, new Vector2(0, 0),
                                     SpriteEffects.None, _layerDepth);
                }

            }
            else
                Defined = false;
        }
        public override void Update(GameTime gameTime)
        {
            Focused = false;
            if (Active)
            {
                if (Inputs.MouseState.LeftButton == ButtonState.Pressed)
                {
                    if (!b1)
                    {
                        p1.X = Inputs.MouseState.X + PlayGame.Camera.x;
                        p1.Y = Inputs.MouseState.Y + PlayGame.Camera.y;
                        b1 = true;
                    }
                    else
                    {
                        p2.X = Inputs.MouseState.X + PlayGame.Camera.x;
                        p2.Y = Inputs.MouseState.Y + PlayGame.Camera.y;
                        b2 = true;
                    }
                }
                else
                {
                    b1 = false;
                    b2 = false;
                }
            }
        }
        public bool Contains(int x, int y)
        {
            return Active && b1 && b2 && Area.Contains(x, y);
        }
    }
}
