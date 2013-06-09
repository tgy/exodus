using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Exodus.Network.Orders.Tasks
{
    [Serializable]
    class Move : ItemTask
    {
        public int x;
        public int y;
        public Move(int parentPos, bool overrideTask, int x, int y)
            : base(parentPos, overrideTask)
        {
            this.overrideTask = overrideTask;
            this.x = x;
            this.y = y;
        }
    }
}
