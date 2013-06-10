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
        public Minimap(int x, int y, float depth)
        {
            this.Depth = depth;
            this.Area = new Rectangle(x, y, PlayGame.Map.Width * 2, PlayGame.Map.Height * 2);
            this.minitile = Textures.GameUI["MiniTile"];
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < PlayGame.Map.Width; i++)
            {
                for (int j = 0; j < PlayGame.Map.Height; j++)
                {
                    Color c = Color.Gray;
                    if (PlayGame.Map.MapCells[i, j].ListItems.Count != 0)
                    {
                        PlayGame.Item item = PlayGame.Map.MapCells[i, j].ListItems[0];
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
                    }
                    Vector2 pos = PlayGame.Map.MapToScreen(i, j);
                    pos.X /= PlayGame.Tile.tileWidth / 2;
                    pos.Y /= PlayGame.Tile.tileHeight / 2;
                    pos.X *= 2;
                    pos.Y *= 2;
                    pos.X += Area.X;
                    pos.Y += Area.Y;
                    spriteBatch.Draw(minitile, pos, null, c, 0f, new Vector2(0, 0), 1f, SpriteEffects.None, this.Depth);
                }
            }
        }
    }
}
