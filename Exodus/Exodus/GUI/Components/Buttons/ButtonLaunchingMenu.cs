using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Exodus.GameStates;

namespace Exodus.GUI.Components.Buttons
{
    public class ButtonLaunchingMenu : Button
    {
        public delegate void OnClick(MenuState m, int i);
        public OnClick DoClick = null;
        public MenuState SubMenu;
    }
}
