using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Exodus.GUI.Items
{
    class Container : Item
    {
        public Container()
        {
            Area = new Microsoft.Xna.Framework.Rectangle(0, 0, 0, 0);
        }
        public Container(List<Component> l)
        {
            Components = l;
        }
    }
}
