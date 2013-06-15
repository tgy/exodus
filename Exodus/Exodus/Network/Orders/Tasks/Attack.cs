using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Exodus.Network.Orders.Tasks
{
    [Serializable]
    class Attack : ItemTask
    {
        public int enemyPrimaryId;
        public Attack(int parentPos, int enemyPrimaryId, bool overrideTask)
            : base(parentPos, overrideTask)
        {
            this.overrideTask = overrideTask;
            this.enemyPrimaryId = enemyPrimaryId;
        }
    }
}
