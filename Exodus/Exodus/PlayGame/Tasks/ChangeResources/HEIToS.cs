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
            : base(parent, 20000, new Resource(0, 400, 0, 200, 400), new Resource(300, 0, 0, 0, 0))
        {
        }
    }
}
