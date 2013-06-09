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
    public class MenuVertical : Item
    {
        public int Step;

        public MenuVertical(int x, int y, int step)
        {
            Area = new Rectangle(x, y, Area.Width, Area.Height);
            Step = step;
        }

        public MenuVertical(List<Component> components)
        {
            Components = components;

            SelectedComponent = Components.IndexOf(Components.Find(x => x.Focused));
        }

        public void Create(List<Component> components)
        {
            components[0].Area = new Rectangle(Area.X, Area.Y, components[0].Area.Width, components[0].Area.Height);
            components[0].SetPosition();
            Components.Add(components[0]);
            for (int i = 1; i < components.Count; i++)
            {
                components[i].Area = new Rectangle(Area.X, Step + components[i - 1].Area.Height + components[i - 1].Area.Y,
                                                   components[i].Area.Width, components[i].Area.Height);

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
