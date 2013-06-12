using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Exodus.GUI.Components;
using Exodus.GUI.Items;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.Threading;
using Exodus.PlayGame;
using Exodus.Network;

namespace Exodus.PlayGame
{
    class BigTileSet
    {
        Texture2D[,] tilesets;
        public BigTileSet(Texture2D[,] tilesets)
        {
            this.tilesets = tilesets;
        }

        public Tuple<Texture2D, Rectangle> GetSourceRectangle(int tileID)
        {
            int tileX = tileID % (tilesets[0, 0].Width * tilesets.GetLength(0) / Tile.tileWidth);
            int tileY = tileID / (tilesets[0, 0].Width * tilesets.GetLength(0) / Tile.tileWidth);
            tileX *= Tile.tileWidth;
            tileY *= Tile.tileHeight;
            int tilesetX = tileX / tilesets[0, 0].Width;
            int tilesetY = tileY / tilesets[0, 0].Height;
            return new Tuple<Texture2D, Rectangle>(
                tilesets[tilesetX, tilesetY],
                new Rectangle(
                    tileX % tilesets[0, 0].Width,
                    tileY % tilesets[0, 0].Height,
                    Tile.tileWidth,
                    Tile.tileHeight
                    )
            );
        }
    }
}
