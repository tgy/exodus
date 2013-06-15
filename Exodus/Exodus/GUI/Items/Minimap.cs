using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Exodus.GUI.Components
{
    public class Minimap : Item
    {
        private Texture2D minitile;
        private Texture2D borders;
        private Texture2D background;
        private int offsetX = 129;
        private int offsetY = 14;
        public DateTime start;
        Label hour;
        Vector2 pos1, pos2;
        Rectangle areaBackground;
        public Minimap(int x, int y, float depth)
        {
            Depth = depth;

            minitile = Textures.GameUI["MiniTile"];
            borders = Textures.GameUI["Minimap"];
            background = Textures.Game["minimap-background"];

            Area = new Rectangle(x, y, borders.Width, borders.Height);
            areaBackground = new Rectangle(Area.X + offsetX, Area.Y + offsetY, background.Width, background.Height);
            start = DateTime.Now;
            hour = new Label(GUI.Fonts.Eurostile12, "", x + 54, y + 139, 4 * float.Epsilon);
            Components.Add(hour);
        }
        public override void Update(GameTime gameTime)
        {
            if (areaBackground.Contains(Inputs.MouseState.X, Inputs.MouseState.Y) && Inputs.MouseState.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed)
            {
                this.Focused = true;
                pos1 = MiniMapToScreen(new Vector2(Inputs.MouseState.X, Inputs.MouseState.Y));
                pos1.X -= Data.Window.WindowWidth / 2;
                pos1.Y -= Data.Window.WindowHeight / 2;
                PlayGame.Camera.Set((int)pos1.X, (int)pos1.Y);
            }
            else
                this.Focused = false;
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            hour.Txt = DateTime.Now.Subtract(start).ToString().Substring(0, 8);
            /*spriteBatch.Draw(minitile, new Rectangle(Area.X + offsetX, Area.Y + offsetY, 143, 143), null,
                             new Color(6, 36, 49), 0f, Vector2.Zero, SpriteEffects.None,
                             this.Depth + 2*Data.GameDisplaying.Epsilon);*/
            spriteBatch.Draw(background, areaBackground, null, Color.White, 0f, Vector2.Zero, SpriteEffects.None,
                             Depth + 5 * Data.GameDisplaying.Epsilon);
            spriteBatch.Draw(borders, Area, null, Color.White, 0f, Vector2.Zero, SpriteEffects.None, this.Depth);
            List<PlayGame.Item> l = new List<PlayGame.Item>();
            foreach (PlayGame.Item i in PlayGame.Map.ListItems)
                l.Add(i);
            foreach (PlayGame.Item i in PlayGame.Map.ListPassiveItems)
                l.Add(i);
            foreach (PlayGame.Item i in l)
            {
                Color c = Color.Gray;
                if (!(i is PlayGame.Obstacle) || i is PlayGame.Building)
                {
                    if (!PlayGame.Map.ListSelectedItems.Exists(n => n == i.IdPlayer))
                        switch (i.IdPlayer)
                        {
                            case 0:
                                c = Color.Gray;
                                break;
                            case 1:
                                c = Color.Blue;
                                break;
                            case 2:
                                c = Color.Yellow;
                                break;
                        }
                    else
                        c = Color.White;
                    Vector2 pos = ScreenToMiniMap(PlayGame.Map.MapToScreen(i.pos.Value.X, i.pos.Value.Y));
                    if (i.pos.Value.X + i.pos.Value.Y >= PlayGame.Map.Width / 2 && i.pos.Value.X + i.pos.Value.Y <= PlayGame.Map.Width + PlayGame.Map.Height / 2 &&
                        Math.Abs(i.pos.Value.X - i.pos.Value.Y) <= PlayGame.Map.Width / 2)
                        spriteBatch.Draw(minitile, new Rectangle((int)pos.X, (int)pos.Y, 2, 2), null, c,
                                         0f, new Vector2(0, 0), SpriteEffects.None,
                                         this.Depth + Data.GameDisplaying.Epsilon);
                }
            }
            pos1 = ScreenToMiniMap(new Vector2(PlayGame.Camera.x, PlayGame.Camera.y));
            pos2 = ScreenToMiniMap(new Vector2(PlayGame.Camera.x + Data.Window.WindowWidth,
                                               PlayGame.Camera.y + Data.Window.WindowHeight));
            spriteBatch.Draw(minitile, new Rectangle((int)pos1.X, (int)pos1.Y, (int)(pos2.X - pos1.X + 1), 1), null,
                             Color.Green, 0f, Vector2.Zero, SpriteEffects.None, 4 * float.Epsilon);
            spriteBatch.Draw(minitile, new Rectangle((int)pos1.X, (int)pos2.Y, (int)(pos2.X - pos1.X + 1), 1), null,
                             Color.Green, 0f, Vector2.Zero, SpriteEffects.None, 4 * float.Epsilon);
            spriteBatch.Draw(minitile, new Rectangle((int)pos1.X, (int)pos1.Y, 1, (int)(pos2.Y - pos1.Y + 1)), null,
                             Color.Green, 0f, Vector2.Zero, SpriteEffects.None, 4 * float.Epsilon);
            spriteBatch.Draw(minitile, new Rectangle((int)pos2.X, (int)pos1.Y, 1, (int)(pos2.Y - pos1.Y + 1)), null,
                             Color.Green, 0f, Vector2.Zero, SpriteEffects.None, 4 * float.Epsilon);
            base.Draw(spriteBatch);
        }
        int Clamp(int val, int min, int max)
        {
            return val > min ? val < max ? val : max : min;
        }
        public Vector2 ScreenToMiniMap(Vector2 pos)
        {
            pos.X -= PlayGame.Camera.minX;
            pos.Y -= PlayGame.Camera.minY;
            pos.X /= PlayGame.Tile.tileWidth / 2;
            pos.Y /= PlayGame.Tile.tileHeight / 2;
            pos.X *= 2;
            pos.Y *= 2;
            pos.X = pos.X * 143 / (PlayGame.Map.Width + PlayGame.Map.Height - 1);
            pos.Y = pos.Y * 143 / (PlayGame.Map.Width + PlayGame.Map.Height - 1);
            pos.X += Area.X + offsetX;
            pos.Y += Area.Y + offsetY;
            return pos;
        }
        public Vector2 MiniMapToScreen(Vector2 pos)
        {
            pos.X -= Area.X + offsetX;
            pos.Y -= Area.Y + offsetY;
            pos.X = pos.X * (PlayGame.Map.Width + PlayGame.Map.Height - 1) / 143;
            pos.Y = pos.Y * (PlayGame.Map.Width + PlayGame.Map.Height - 1) / 143;
            pos.X /= 2;
            pos.Y /= 2;
            pos.X *= PlayGame.Tile.tileWidth / 2;
            pos.Y *= PlayGame.Tile.tileHeight / 2;
            pos.X += PlayGame.Camera.minX;
            pos.Y += PlayGame.Camera.minY;
            return pos;
        }
    }
}
