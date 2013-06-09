using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Exodus.GameStates;

namespace Exodus.GUI.Components.Buttons
{
    abstract class GameButton : Button
    {
        public delegate void OnClick();
        public OnClick DoClick = null;
        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            if (Focused && Inputs.LeftClick() && DoClick != null)
                DoClick();
            base.Update(gameTime);
        }
    }
}
