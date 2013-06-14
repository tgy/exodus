using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Exodus.GUI.Components.Buttons.MenuButtons
{
    public class LaunchingOrangeButton: MenuButton
    {
        public LaunchingOrangeButton(string text)
        {
            Text = text;
            ButtonTextures.Add(Textures.Menu["StartButton"]);
            ButtonTextures.Add(Textures.Menu["StartButtonHover"]);
            ButtonTextures.Add(Textures.Menu["StartButtonClick"]);
            Area = new Rectangle(Area.X, Area.Y, ButtonTextures[0].Width, ButtonTextures[0].Height);
            Font = Fonts.Eurostile12;
            Color = Color.White;
        }
    }
}
