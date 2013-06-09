using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Exodus.Network.Orders.Tasks
{
    [Serializable]
    class Hold : ItemTask
    {
        public Hold(int parentPos, bool overrideTask)
            : base(parentPos, overrideTask)
        {

        }
    }
}
