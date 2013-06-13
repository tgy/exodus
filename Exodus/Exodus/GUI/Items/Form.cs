using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Exodus.GUI.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Exodus.GUI.Items
{
    internal class Form : Item
    {
        public int SubmitterId;
        private int _vspace;

        public Form(int x, int y, Padding padding, int vspace, float depth)
        {
            Area = new Rectangle(x, y, 0, 0);
            _vspace = vspace;
            Padding = padding;
            Depth = depth;
        }

        public override void Initialize()
        {
            Rectangle totalArea = Components.Find(x => x is JustTexture).Area;
            Area = new Rectangle(Area.X, Area.Y, totalArea.Width, totalArea.Height);
            Point aux = new Point(Area.X + Padding.Left, Area.Y + Padding.Top);
            Components[1].Area = new Rectangle(aux.X + Components[1].Padding.Left, aux.Y + Components[1].Padding.Top,
                                               Components[1].Area.Width, Components[1].Area.Height);
            Components[1].SetPosition();
            for (int i = 2; i < Components.Count; i++)
            {
                Component c = Components[i], pc = Components[i - 1];
                c.Area = new Rectangle(aux.X + c.Padding.Left, pc.Area.Y + c.Padding.Top + pc.Area.Height + _vspace, c.Area.Width, c.Area.Height);
                c.SetPosition();
            }

            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            if (Inputs.KeyPress(Keys.Enter))
            {
                MenuButton submitter = ((MenuButton) Components[SubmitterId]);
                submitter.DoClick(submitter.SubMenu, 0);
            }

            if (Inputs.KeyPress(Keys.Tab))
                TabNext();

            base.Update(gameTime);
        }

        public void TabNext()
        {
            if (Components.Count(x => x is TextBox) < 2)
                return;
            int index = Components.FindIndex(x => x.Focused);
            if (index < 0)
                index = 0;
            bool b = false;
            for (int i = index + 1; i < Components.Count; i++)
                if (Components[i] is TextBox)
                {
                    Components[index].Focused = false;
                    Components[i].Focused = true;
                    b = true;
                    break;
                }
            if (!b)
                for (int i = 0; i < index; i++)
                    if (Components[i] is TextBox)
                    {
                        Components[index].Focused = false;
                        Components[i].Focused = true;
                        break;
                    }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }
    }
}
