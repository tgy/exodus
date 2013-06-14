using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Exodus.GUI.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Net;
using System.IO;
namespace Exodus.GUI.Items
{
    public class PlayerInfosLaunching : Item
    {
        Label Player;
        Label Rank,
              gPlayed, gPostPlayed,
              gWin, gPostWin,
              gLost, gPostLost;
        Label Percentage;
        JustTextureRectangle BackPercentage,
                             FrontPercentage;
        JustTexture avatar = null,
                             avatarFrame;
        Texture2D _avatarFrame,
                  _avatar;
        int FrontPercentageWidth = 0;
        float currentFrontPercentage = 0,
              layerDepth;
        int _step1, _step2;
        public PlayerInfosLaunching(int x, int y, float layerDepth, bool mirror)
        {
            if (mirror)
            {
                _step2 = 0;
                _step1 = 150;
            }
            else
            {
                _step1 = 0;
                _step2 = 150;
            }
            Focused = false;
            Area = new Rectangle(x, y, 0, 0);
            _avatarFrame = Textures.Menu["avatarFrame2"];
            avatarFrame = new JustTexture(_avatarFrame, x + _step1, y + 20, layerDepth + Data.GameDisplaying.Epsilon);
            Components.Add(avatarFrame);
            Player = new Label(Fonts.Eurostile16Bold, Data.PlayerInfos.Name, x + 6 + _step1, y - 5);
            Player.SetColor(255, 255, 255);
            Components.Add(Player);
            Label l = new Label(Fonts.Eurostile16Bold, "RANK", x + _step2 - 3, y + 20);
            l.SetColor(156, 221, 255);
            Components.Add(l);
            Rank = new Label(Fonts.Eurostile60, "", x + 57 + _step2, y - 31);
            Rank.SetColor(255, 255, 255);
            Components.Add(Rank);
            Components.Add(new JustTexture(Textures.Menu["bar"], x + _step2 - 3, y + 43, layerDepth));
            gPlayed = new Label(Fonts.Eurostile12, "", x + 6 + _step2, y + 57);
            gWin = new Label(Fonts.Eurostile12, "", x + 6 + _step2, y + 73);
            gLost = new Label(Fonts.Eurostile12, "", x + 6 + _step2, y + 89);
            Components.Add(gPlayed);
            Components.Add(gWin);
            Components.Add(gLost);
            gPostPlayed = new Label(Fonts.Eurostile12, " games played", x + _step2 + 6, y + 57);
            gPostPlayed.SetColor(156, 221, 255);
            gPostWin = new Label(Fonts.Eurostile12, " win", x + _step2 + 6, y + 73);
            gPostWin.SetColor(156, 221, 255);
            gPostLost = new Label(Fonts.Eurostile12, " lost", x + _step2 + 6, y + 89);
            gPostLost.SetColor(156, 221, 255);
            Components.Add(gPostWin);
            Components.Add(gPostPlayed);
            Components.Add(gPostLost);
            Percentage = new Label(Fonts.Eurostile24, "42%", x + _step2 + 10, y + 122);
            Components.Add(Percentage);
            BackPercentage = new JustTextureRectangle(Textures.GameUI["MiniTile"], x + _step2 + 6, y + 112, 128, 5, layerDepth + Data.GameDisplaying.Epsilon);
            BackPercentage.c = new Color(57, 162, 17);
            FrontPercentage = new JustTextureRectangle(Textures.GameUI["MiniTile"], x + _step2 + 6, y + 112, 0, 5, layerDepth);
            FrontPercentage.c = new Color(119, 255, 67);
            Components.Add(FrontPercentage);
            Components.Add(BackPercentage);
            this.layerDepth = layerDepth;
        }
        public void Reset(string avatarURL, int rank, int victories, int defeats, bool IsOur)
        {
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
            WebClient Client = new WebClient();
            try
            {
                if (!IsOur)
                {
                    Client.DownloadFile(Data.PlayerInfos.beginAvatar + avatarURL, "Content/" + avatarURL);
                    Stream s = new FileStream("Content/" + avatarURL, FileMode.Open);
                    _avatar = Texture2D.FromStream(Data.GameDisplaying.GraphicsDevice, s);
                }
                else
                    _avatar = Exodus.Player.avatar;
                if (avatar != null)
                    Components.Remove(avatar);
                avatar = new JustTexture(_avatar, avatarFrame.Area.X + (_avatarFrame.Width - _avatar.Width) / 2 - 1, avatarFrame.Area.Y + (_avatarFrame.Height - _avatar.Height) / 2 - 1, layerDepth);
                Components.Add(avatar);
            }
            catch
            {
                Components.Remove(avatar);
            }
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
            Player.Txt = Data.PlayerInfos.Name.ToUpper();
            base.Update(gameTime);
        }
    }
}
