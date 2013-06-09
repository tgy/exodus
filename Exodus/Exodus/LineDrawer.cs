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
    #region Trash
    //static class LineDrawer
    //{
    //    static public Tuple<float, int, int, int> GetInfos(Point p1, Point p2)
    //    {
    //        return GetInfos(p1.X, p1.Y, p2.X, p2.Y);
    //    }
    //    static public Tuple<float, int, int, int> GetInfos(int x, int y, int toX, int toY)
    //    {
    //        int dX = toX - x,
    //            dY = toY - y;
    //        float hypo = (float)Math.Sqrt(dX * dX + dY * dY),
    //            rot = 0;
    //        if (dX == 0)
    //            if (dY > 0)
    //                rot = (float)(Math.PI / 2);
    //            else
    //                rot = (float)(-Math.PI / 2);
    //        else
    //        {
    //            if (dY > 0)
    //                rot = (float)Math.Acos((double)dX / (double)hypo);
    //            else
    //                rot = -(float)Math.Acos((double)dX / (double)hypo);
    //        }
    //        return new Tuple<float, int, int, int>(rot, x, y, (int)hypo);
    //    }
    //    static public void DrawLine(SpriteBatch spriteBatch, Texture2D t, Tuple<float, int, int, int> Infos)
    //    {
    //        DrawLine(spriteBatch, t, Infos.Item1, Infos.Item2, Infos.Item3, Infos.Item4);
    //    }
    //    static public void DrawLine(SpriteBatch spriteBatch, Texture2D t, float rotation, int oX, int oY, int length)
    //    {
    //        spriteBatch.Draw(t,
    //                         new Rectangle(oX, oY, length, 2),
    //                         null,
    //                         Color.White,
    //                         rotation,
    //                         new Vector2(0, 0),
    //                         SpriteEffects.None,
    //                         0f);
    //    }
    //    static public void DirectlyDrawLine(SpriteBatch spriteBatch, Texture2D t, Point p1, Point p2)
    //    {
    //        DrawLine(spriteBatch, t, GetInfos(p1.X, p1.Y, p2.X, p2.Y));
    //    }
    //    static public void DirectlyDrawLine(SpriteBatch spriteBatch, Texture2D t, int x, int y, int toX, int toY)
    //    {
    //        DrawLine(spriteBatch, t, GetInfos(x, y, toX, toY));
    //    }
    //}
    #endregion
}
