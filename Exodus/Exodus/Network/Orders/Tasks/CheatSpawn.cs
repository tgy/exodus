using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Exodus.Network.Orders.Tasks
{
    [Serializable]
    class CheatSpawn : ItemTask
    {
        public int x;
        public int y;
        public int IdPlayer;
        public Type child;
        public int nextId = 0;
        public CheatSpawn(long parentPrimaryKey, bool overrideTask, Type child, int IdPlayer, int x, int y)
            : base(parentPrimaryKey, overrideTask)
        {
            this.overrideTask = overrideTask;
            this.IdPlayer = IdPlayer;
            this.x = x;
            this.y = y;
            this.child = child;
        }
    }
}
