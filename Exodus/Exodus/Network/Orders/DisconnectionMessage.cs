using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Exodus.Network.Orders
{
    [Serializable]
    class DisconnectionMessage
    {
        public string reason;
        public DisconnectionMessage(string reason)
        {
            this.reason = reason;
        }
    }
}
