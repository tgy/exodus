using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Exodus.Network.Orders
{
    [Serializable]
    class ItemTask
    {
        public long parentPrimaryKey;
        public bool overrideTask;
        public ItemTask(long parentPrimaryKey, bool overrideTask)
        {
            this.parentPrimaryKey = parentPrimaryKey;
            this.overrideTask = overrideTask;
        }
    }
}
