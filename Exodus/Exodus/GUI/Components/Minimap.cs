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
        int offsetX = 129;
        int offsetY = 14;
        public Minimap(int x, int y, float depth)
        {
            this.Depth = depth;
            this.minitile = Textures.GameUI["MiniTile"];
            this.borders = Textures.GameUI["Minimap"];
            this.Area = new Rectangle(x, y, borders.Width, borders.Height);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(minitile, new Rectangle(Area.X + offsetX, Area.Y + offsetY, 143, 143), null, new Color(6, 36, 49), 0f, Vector2.Zero, SpriteEffects.None, this.Depth + 2 * Data.GameDisplaying.Epsilon);
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
                            if (!(item is PlayGame.Obstacle))
                            {
                                switch (item.IdPlayer)
                                {
                                    case 0:
                                        c = Color.Blue;
                                        break;
                                    case 1:
                                        c = Color.Red;
                                        break;
                                    case 2:
                                        c = Color.Green;
                                        break;
                                }
                                Vector2 pos = PlayGame.Map.MapToScreen(i, j);
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
                                if (i + j >= PlayGame.Map.Width / 2 && i + j <= PlayGame.Map.Width + PlayGame.Map.Height / 2 && Math.Abs(i - j) <= PlayGame.Map.Width / 2)
                                    spriteBatch.Draw(minitile, new Rectangle((int)pos.X, (int)pos.Y, 2, 2), null, c, 0f, new Vector2(0, 0), SpriteEffects.None, this.Depth + Data.GameDisplaying.Epsilon);
                            }
                        }
                    }
                }
            }
        }
    }
}
