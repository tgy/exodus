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
        static public Texture2D tileSetTexture;
        static public int tileWidth = 64;
        static public int tileHeight = 32;

        static public Rectangle GetSourceRectangle(int tileID)
        {
            int tileX = tileID % (tileSetTexture.Width / tileWidth);
            int tileY = tileID / (tileSetTexture.Width / tileWidth);

            return new Rectangle(tileX * tileWidth, tileY * tileHeight, tileWidth, tileHeight);
        }
    }
}
