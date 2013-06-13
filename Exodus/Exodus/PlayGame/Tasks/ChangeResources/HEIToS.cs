using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Exodus.PlayGame.Items.Units;

namespace Exodus.PlayGame.Tasks.ChangeResources
{
    [Serializable]
    class HEIToS : ChangeResource
    {
        public HEIToS(Item parent)
            : base(parent, 20000, new Resource(0, 100, 0, 10, 50), new Resource(50, 0, 0, 0, 0))
        {
        }
    }
}
