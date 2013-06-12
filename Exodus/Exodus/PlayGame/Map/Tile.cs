using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Exodus.PlayGame
{
    public static class Tile
    {
        static public Texture2D[,] tileSetTextures;
        static public int tileWidth = 64;
        static public int tileHeight = 32;
        static public int tileSetWidth; // en nb de tiles
        
        static public Tuple<Texture2D,Rectangle> GetSourceRectangle(int tileID)
        {
            int tileX = tileID % tileSetWidth;
            int tileY = tileID / tileSetWidth;
            Texture2D texture = tileSetTextures[tileX / (tileSetTextures[0, 0].Width / Tile.tileWidth), tileY / (tileSetTextures[0, 0].Height / Tile.tileHeight)];
            Rectangle rectangle = new Rectangle(((tileX * Tile.tileWidth) % tileSetTextures[0, 0].Width), ((tileY * Tile.tileHeight) % tileSetTextures[0, 0].Height), Tile.tileWidth, Tile.tileHeight);
            return new Tuple<Texture2D, Rectangle>(texture, rectangle);
        }

        static public Texture2D GetTexture(int x, int y)
        {
            return tileSetTextures[x / tileSetTextures[0, 0].Width, y / tileSetTextures[0, 0].Height];
        }
    }
}
