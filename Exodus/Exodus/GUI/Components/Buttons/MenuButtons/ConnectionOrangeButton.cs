using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Exodus.GUI.Components.Buttons.MenuButtons
{
    public class ConnectionOrangeButton: MenuButton
    {
        public ConnectionOrangeButton(string text)
        {
            Text = text;
            ButtonTextures.Add(Textures.Menu["ConnectionOrangeButton"]);
            ButtonTextures.Add(Textures.Menu["ConnectionOrangeButtonHover"]);
            ButtonTextures.Add(Textures.Menu["ConnectionOrangeButtonHover"]);
            Area = new Rectangle(Area.X, Area.Y, ButtonTextures[0].Width, ButtonTextures[0].Height);

            Font = Fonts.Eurostile12;
            Color = Color.White;
        }
    }
}
