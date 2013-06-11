using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Exodus.GUI.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Exodus.GUI.Items
{
    public class PlayerInfos : Item
    {
        Label Player;
        Label Rank,
              gPlayed, gPostPlayed,
              gWin, gPostWin,
              gLost, gPostLost;
        Label Percentage;
        JustTextureRectangle BackPercentage,
                             FrontPercentage;
        int FrontPercentageWidth = 0;
        float currentFrontPercentage = 0;
        public PlayerInfos(int x, int y, float layerDepth)
        {
            Focused = false;
            Area = new Rectangle(x, y, 0, 0);

            JustTexture avatarFrame = new JustTexture(Textures.Menu["avatarFrame"], x, y + 20, layerDepth + Data.GameDisplaying.Epsilon);
            Components.Add(avatarFrame);
            Player = new Label(Fonts.Eurostile16Bold, Data.PlayerInfos.Name, x + 6, y -5);
            Player.SetColor(0, 0, 0);
            Components.Add(Player);
            Label l = new Label(Fonts.Eurostile16Bold, "RANK", x + 147, y + 20);
            l.SetColor(156, 221, 255);
            Components.Add(l);
            Rank = new Label(Fonts.Eurostile60, "", x + 207, y - 31);
            Rank.SetColor(255, 255, 255);
            Components.Add(Rank);
            Components.Add(new JustTexture(Textures.Menu["bar"], x + 147, y + 43, layerDepth));
            gPlayed = new Label(Fonts.Eurostile12, "1892", x + 156, y + 57);
            gWin = new Label(Fonts.Eurostile12, "1102", x + 156, y + 73);
            gLost = new Label(Fonts.Eurostile12, "790", x + 156, y + 89);
            Components.Add(gPlayed);
            Components.Add(gWin);
            Components.Add(gLost);
            gPostPlayed = new Label(Fonts.Eurostile12, " games played", x + 156, y + 57);
            gPostPlayed.SetColor(156, 221, 255);
            gPostWin = new Label(Fonts.Eurostile12, " win", x + 156, y + 73);
            gPostWin.SetColor(156, 221, 255);
            gPostLost = new Label(Fonts.Eurostile12, " lost", x + 156, y + 89);
            gPostLost.SetColor(156, 221, 255);
            Components.Add(gPostWin);
            Components.Add(gPostPlayed);
            Components.Add(gPostLost);
            Percentage = new Label(Fonts.Eurostile24, "58%", x + 160, y + 122);
            Components.Add(Percentage);
            BackPercentage = new JustTextureRectangle(Textures.GameUI["MiniTile"], x + 156, y + 112, 128, 5, layerDepth + Data.GameDisplaying.Epsilon);
            BackPercentage.c = new Color(57, 162, 17);
            FrontPercentage = new JustTextureRectangle(Textures.GameUI["MiniTile"], x + 156, y + 112, 0, 5, layerDepth);
            FrontPercentage.c = new Color(119, 255, 67);
            Components.Add(FrontPercentage);
            Components.Add(BackPercentage);
        }
        public void Reset(string pseudo, int rank, int victories, int defeats)
        {
            Player.Txt = pseudo.ToUpper();
            Rank.Txt = rank.ToString();
            gPlayed.Txt = (victories + defeats).ToString();
            gWin.Txt = victories.ToString();
            gLost.Txt = defeats.ToString();
            gPostPlayed.Pos = new Vector2(gPlayed.Pos.X + Fonts.Eurostile12.MeasureString(gPlayed.Txt).X, gPlayed.Pos.Y);
            gPostWin.Pos = new Vector2(gWin.Pos.X + Fonts.Eurostile12.MeasureString(gWin.Txt).X, gWin.Pos.Y);
            gPostLost.Pos = new Vector2(gLost.Pos.X + Fonts.Eurostile12.MeasureString(gLost.Txt).X, gLost.Pos.Y);
            float percentage = victories > 0 ? 100 * (float)victories / ((float)victories + (float)defeats) : defeats > 0 ? 0 : 50;
            FrontPercentage.Width = 0;
            currentFrontPercentage = 0;
            Percentage.Txt = (int)percentage + "%";
            FrontPercentageWidth = (int)(percentage * BackPercentage.Width / 100);
        }
        public override void Update(GameTime gameTime)
        {
            if (FrontPercentage.Width > FrontPercentageWidth)
            {
                currentFrontPercentage -= Math.Max(1, Math.Min(BackPercentage.Width / 10, (FrontPercentage.Width - FrontPercentageWidth) * 10 / BackPercentage.Width));
                FrontPercentage.Width = (int)currentFrontPercentage;
                Percentage.Txt = (int)(100 * currentFrontPercentage / BackPercentage.Width) + "%";
            }
            else if (FrontPercentage.Width < FrontPercentageWidth)
            {
                currentFrontPercentage += Math.Max(1, Math.Min(BackPercentage.Width / 10, (FrontPercentageWidth - FrontPercentage.Width) * 10 / BackPercentage.Width));
                FrontPercentage.Width = (int)currentFrontPercentage;
                Percentage.Txt = (int)(100 * currentFrontPercentage / BackPercentage.Width) + "%";
            }
            base.Update(gameTime);
        }
    }
}
