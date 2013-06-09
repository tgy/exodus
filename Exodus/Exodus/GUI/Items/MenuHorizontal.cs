using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Exodus.GUI.Components.Buttons;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Exodus.GUI.Components;

namespace Exodus.GUI.Items
{
    public class MenuHorizontal : Item
    {
        public int Step;

        public MenuHorizontal(int x, int y, int step)
        {
            Area = new Rectangle(x, y, Area.Width, Area.Height);
            Step = step;
        }
        public void Create(List<Component> components)
        {
            components[0].Area = new Rectangle(Area.X, Area.Y, components[0].Area.Width, components[0].Area.Height);
            components[0].SetPosition();
            Area.Height = components[0].Area.Height;
            Area.Width = components[0].Area.Width;
            Components.Add(components[0]);
            for (int i = 1; i < components.Count; i++)
            {
                components[i].Area.X = components[i - 1].Area.X + components[i - 1].Area.Width + Step;
                if (Area.Height < components[i].Area.Height)
                    Area.Height = components[i].Area.Height;
                Area.Width += Step + components[i].Area.Width;
                components[i].Area.Y = Area.Y;

                components[i].SetPosition();

                Components.Add(components[i]);
            }
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }
    }
}
