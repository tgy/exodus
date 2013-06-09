using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Exodus.Network.Orders.Tasks
{
    [Serializable]
    class Die : ItemTask
    {
        public Die(int parentPos, bool overrideTask) : base(parentPos, overrideTask)
        {
            this.overrideTask = overrideTask;
            this.parentPrimaryKey = parentPos;
        } 
    }
}
