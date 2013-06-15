using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Exodus.Network.Orders.Tasks
{
    [Serializable]
    class HarvestIron : ItemTask
    {
        public int ironIndex;
        public HarvestIron(int parentPos, int ironIndex, bool overrideTask)
            : base(parentPos, overrideTask)
        {
            this.overrideTask = overrideTask;
            this.ironIndex = ironIndex;
        }
    }
}
