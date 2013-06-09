using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Exodus.GUI.Components.Buttons.MenuButtons
{
    public class OrangeMenuButton : MenuButton
    {
        public OrangeMenuButton(string text)
        {
            Text = text;
            ButtonTextures.Add(Textures.Menu["OrangeMenuButton"]);
            ButtonTextures.Add(Textures.Menu["OrangeMenuButtonHover"]);
            ButtonTextures.Add(Textures.Menu["OrangeMenuButtonClick"]);
            Area = new Rectangle(Area.X, Area.Y, ButtonTextures[0].Width, ButtonTextures[0].Height);

            Font = Fonts.Eurostile12;
            Color = Color.White;
        }
    }
}
