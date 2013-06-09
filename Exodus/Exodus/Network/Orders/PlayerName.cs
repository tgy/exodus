using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Exodus.Network.Orders
{
    [Serializable]
    class PlayerName
    {
        public string name;
        public PlayerName(string name)
        {
            this.name = name;
        }
    }
}
