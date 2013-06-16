using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Exodus.PlayGame.Items.Units;

namespace Exodus.PlayGame.Tasks.ChangeResources
{
    [Serializable]
    class HESToG : ChangeResource
    {
        public HESToG(Item parent)
            : base(parent, 20000, new Resource(450, 0, 0, 200, 1000), new Resource(0, 0, 350, 0, 0))
        {
        }
    }
}
