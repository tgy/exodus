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

namespace Exodus.GameStates
{
    class LoadingState : GameState
    {
        Application game;
        GameState previous;
        public LoadingState(Application g, GameState previous, string msg)
        {
            game = g;
            this.previous = previous;
            SpriteFont font = GUI.Fonts.Eurostile18;
            Texture2D t = Textures.Menu["loadingBar"];
            Items.Add(new GUI.Items.BackgroundFull(Textures.Menu["loadingBackground"], 3 * float.Epsilon));
            Items.Add(new GUI.Items.Passive(t, (Data.Window.WindowWidth - t.Width) / 2, (Data.Window.WindowHeight - t.Height) / 2, 2 * float.Epsilon));
            Items.Add(new GUI.Items.Container(
                new List<GUI.Component>
                    {
                        new GUI.Components.Label(font, msg, (int)(Data.Window.ScreenCenter.X - font.MeasureString(msg).X / 2), (int)(Data.Window.ScreenCenter.Y - font.MeasureString(msg).Y / 2))
                    }
            ));
        }
        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
        }
        public override void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch)
        {
            previous.Draw(spriteBatch);
            base.Draw(spriteBatch);
        }
    }
}