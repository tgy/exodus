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
        private Vector2 backgroundPosition;
        private int offsetX = 129;
        private int offsetY = 14;
        DateTime start;
        Label hour;
        public Minimap(int x, int y, float depth)
        {
            Depth = depth;

            minitile = Textures.GameUI["MiniTile"];
            borders = Textures.GameUI["Minimap"];
            background = Textures.Game["minimap-background"];

            Area = new Rectangle(x, y, borders.Width, borders.Height);
            backgroundPosition = new Vector2(Area.X + offsetX, Area.Y + offsetY);
            start = DateTime.Now;
            hour = new Label(GUI.Fonts.Eurostile12, "", x + 54, y + 139);
            Components.Add(hour);
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            hour.Txt = DateTime.Now.Subtract(start).ToString().Substring(0, 8);
            /*spriteBatch.Draw(minitile, new Rectangle(Area.X + offsetX, Area.Y + offsetY, 143, 143), null,
                             new Color(6, 36, 49), 0f, Vector2.Zero, SpriteEffects.None,
                             this.Depth + 2*Data.GameDisplaying.Epsilon);*/
            spriteBatch.Draw(background, backgroundPosition, null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None,
                             Depth + 5*Data.GameDisplaying.Epsilon);
            spriteBatch.Draw(borders, Area, null, Color.White, 0f, Vector2.Zero, SpriteEffects.None, this.Depth);
            for (int i = 0; i < PlayGame.Map.Width; i++)
            {
                for (int j = 0; j < PlayGame.Map.Height; j++)
                {
                    if (PlayGame.Map.MapCells[i, j].ListItems.Count > 0)
                    {
                        if (PlayGame.Map.MapCells[i, j].ListItems.Count != 0)
                        {
                            Color c = Color.Gray;
                            PlayGame.Item item = PlayGame.Map.MapCells[i, j].ListItems[0];
                            if (!(item is PlayGame.Obstacle) || item is PlayGame.Building)
                            {
                                if (!PlayGame.Map.ListSelectedItems.Exists(n => n == item.PrimaryId))
                                    switch (item.IdPlayer)
                                    {
                                        case 0:
                                            c = Color.Red;
                                            break;
                                        case 1:
                                            c = Color.Yellow;
                                            break;
                                        case 2:
                                            c = Color.Blue;
                                            break;
                                    }
                                else
                                    c = Color.White;
                                Vector2 pos = ScreenToMiniMap(PlayGame.Map.MapToScreen(i, j));
                                if (i + j >= PlayGame.Map.Width/2 && i + j <= PlayGame.Map.Width + PlayGame.Map.Height/2 &&
                                    Math.Abs(i - j) <= PlayGame.Map.Width/2)
                                    spriteBatch.Draw(minitile, new Rectangle((int) pos.X, (int) pos.Y, 2, 2), null, c,
                                                     0f, new Vector2(0, 0), SpriteEffects.None,
                                                     this.Depth + Data.GameDisplaying.Epsilon);
                            }
                        }
                    }
                } // end for
            } // end for
            Vector2 pos1 = ScreenToMiniMap(new Vector2(PlayGame.Camera.x, PlayGame.Camera.y)),
                    pos2 =
                        ScreenToMiniMap(new Vector2(PlayGame.Camera.x + Data.Window.WindowWidth,
                                                    PlayGame.Camera.y + Data.Window.WindowHeight));
            spriteBatch.Draw(minitile, new Rectangle((int) pos1.X, (int) pos1.Y, (int) (pos2.X - pos1.X + 1), 1),
                             Color.Green);
            spriteBatch.Draw(minitile, new Rectangle((int) pos1.X, (int) pos2.Y, (int) (pos2.X - pos1.X + 1), 1),
                             Color.Green);
            spriteBatch.Draw(minitile, new Rectangle((int) pos1.X, (int) pos1.Y, 1, (int) (pos2.Y - pos1.Y + 1)),
                             Color.Green);
            spriteBatch.Draw(minitile, new Rectangle((int) pos2.X, (int) pos1.Y, 1, (int) (pos2.Y - pos1.Y + 1)),
                             Color.Green);
            base.Draw(spriteBatch);
        }

        public Vector2 ScreenToMiniMap(Vector2 pos)
        {
            pos.X -= PlayGame.Camera.minX;
            pos.Y -= PlayGame.Camera.minY;
            pos.X /= PlayGame.Tile.tileWidth/2;
            pos.Y /= PlayGame.Tile.tileHeight/2;
            pos.X *= 2;
            pos.Y *= 2;
            pos.X = pos.X*143/(PlayGame.Map.Width + PlayGame.Map.Height - 1);
            pos.Y = pos.Y*143/(PlayGame.Map.Width + PlayGame.Map.Height - 1);
            pos.X += Area.X + offsetX;
            pos.Y += Area.Y + offsetY;
            return pos;
        }
    }
}
