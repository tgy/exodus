using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Exodus.GUI.Components.Buttons.MenuButtons
{
    public class BlueMenuButton : MenuButton
    {
        public BlueMenuButton(string text)
        {
            Text = text;
            ButtonTextures.Add(Textures.Menu["BlueMenuButton"]);
            ButtonTextures.Add(Textures.Menu["BlueMenuButtonHover"]);
            ButtonTextures.Add(Textures.Menu["BlueMenuButtonClick"]);
            Area = new Rectangle(Area.X, Area.Y, ButtonTextures[0].Width, ButtonTextures[0].Height);

            Font = Fonts.Eurostile12;
            Color = Color.White;
        }
    }
}
