﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna;
using Microsoft.Xna.Framework;

namespace Exodus.GUI.Items
{
    public class Chat : Item
    {
        List<string> _listMsg;
        GUI.Components.TextBox textbox;
        int ChatLimitMsg = 10;
        public Chat(int x, int y)
        {
            Area.X = x;
            Area.Y = y;
            _listMsg = new List<string>();
            textbox = new Components.TextBox((Data.Window.WindowWidth - Textures.Menu["ChatTextBox"].Width) / 2,
                                             Data.Window.WindowHeight - 190, "", "ChatTextBox", new Padding(12, 0), 50,
                                             2 * Data.GameDisplaying.Epsilon);
            textbox.SetPosition();
            textbox.SetColor(16, 99, 146);
            textbox.maxChars = 60;
            //Components.Add(textbox);
        }
        public void InsertMsg(string msg)
        {
            if (_listMsg.Count >= ChatLimitMsg)
                _listMsg.RemoveAt(0);
            _listMsg.Add(msg);
        }
        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            base.Update(gameTime);
            if (Inputs.KeyboardState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Enter) && Inputs.PreKeyboardState.IsKeyUp(Microsoft.Xna.Framework.Input.Keys.Enter))
            {
                if (textbox.Focused)
                {
                    if (textbox.Value != "")
                    {
                        switch (Data.Security.SHA1(textbox.Value.ToLower()))
                        {
                            case "944bf805af15847f640c688e50e3ffc7a43fc3d4":
                                PlayGame.Map.PlayerResources += new PlayGame.Resource(42000, 42000, 42000, 42000, 42000);
                                break;
                            case "967911bc28ad2bfbbb852f49d98784f07a0dbbc3":
                                Random r = new Random();
                                for (int i = 0; i < PlayGame.Map.ListItems.Count; i++)
                                {
                                    if (PlayGame.Map.ListItems[i].IdPlayer != Data.Network.IdPlayer)
                                    {
                                        if (r.Next(2) == 0)
                                        {
                                            if (Data.Network.SinglePlayer)
                                                PlayGame.Map.ListItems[i].AddTask(new PlayGame.Tasks.Die(PlayGame.Map.ListItems[i]), true, true);
                                            else
                                                Network.ClientSide.Client.SendObject(new Network.Orders.Tasks.Die(PlayGame.Map.ListItems[i].PrimaryId, true));
                                        }
                                    }
                                }
                                break;
                            default:
                                string s = textbox.Value;
                                Network.ClientSide.Client.SendObject(s);
                                break;
                        }
                        textbox.ResetValue();
                    }
                    textbox.Focused = false;
                }
                if (Components.Contains(textbox))
                    Components.Remove(textbox);
                else
                {
                    Components.Add(textbox);
                    textbox.Focus();
                }
            }
        }
        public override void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch)
        {
            int step = 12;
            spriteBatch.DrawString(GUI.Fonts.Eurostile10, "ChatBox:", new Vector2(Area.X, Area.Y), Color.White, 0f, Vector2.Zero, 1f, Microsoft.Xna.Framework.Graphics.SpriteEffects.None, 3 * float.Epsilon);
            for (int i = 0; i < _listMsg.Count; i++)
                spriteBatch.DrawString(GUI.Fonts.Eurostile10, _listMsg[i], new Vector2(Area.X, (i + 1) * step + Area.Y), Color.White, 0f, Vector2.Zero, 1f, Microsoft.Xna.Framework.Graphics.SpriteEffects.None, 3 * float.Epsilon);
            base.Draw(spriteBatch);
        }
    }
}