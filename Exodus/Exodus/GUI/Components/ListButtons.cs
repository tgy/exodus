using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Exodus.GUI.Components
{
    public class ListButtons : Component
    {
        public List<Texture2D> listTextures { get; private set; }
        public int widthTexture { get; private set; }
        public int heightTexture { get; private set; }
        public int step;
        public int height { get; private set; }
        public int width { get; private set; }
        public int displayedLinesElements = 0;
        public int indexBegin = 0;
        public int selectedButton;
        public ListButtons(int posX, int posY, int width, int height, int pas)
        {
            widthTexture = -1;
            heightTexture = -1;
            step = pas;
            Area = new Rectangle(posX, posY, width, height);
            listTextures = new List<Texture2D>();
            height = 0;
        }
        public void Reset(List<Texture2D> l)
        {
            listTextures = new List<Texture2D>();
            width = 0;
            height = 0;
            indexBegin = 0;
            selectedButton = -1;
            if (l.Count > 0)
            {
                widthTexture = l[0].Width;
                heightTexture = l[0].Height;
                listTextures.Add(l[0]);
                for (int i = 1; i < l.Count; i++)
                    if (l[i].Width == widthTexture && l[i].Height == heightTexture)
                        listTextures.Add(l[i]);
                int x = 0;
                int count = 0;
                height = 1;
                for (int i = 0; i < listTextures.Count; i++)
                {
                    if (x + widthTexture > Area.Width)
                    {
                        width = count;
                        x = 0;
                        height++;
                    }
                    else if (width == 0)
                        count++;
                    x += step + widthTexture;
                }
            }
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            Vector2 v = new Vector2(Area.X, Area.Y);
            if (displayedLinesElements > 0)
            {
                int lines = 0;
                for (int i = width * indexBegin; i < listTextures.Count; i++)
                {
                    if (v.X + widthTexture > Area.X + Area.Width)
                    {
                        v.Y += heightTexture + step;
                        v.X = Area.X;
                        lines++;
                        if (lines >= displayedLinesElements)
                            break;
                    }
                    spriteBatch.Draw(listTextures[i], v, null, (i == selectedButton ? Color.Red : Color.White), 0f, Vector2.Zero, 1f, SpriteEffects.None, float.Epsilon * 2);
                    v.X += step + widthTexture;
                }
            }
        }
    }
}
