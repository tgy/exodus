using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Exodus.PlayGame.Items.Units;

namespace Exodus.PlayGame.Tasks.ChangeResources
{
    [Serializable]
    class HToE : ChangeResource
    {
        public HToE(Item parent)
            : base(parent, 7500, new Resource(0, 0, 0, 100, 0), new Resource(0, 0, 0, 0, 1000))
        {
        }
    }
}
