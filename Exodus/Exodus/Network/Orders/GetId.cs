using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Exodus.Network.Orders
{
    [Serializable]
    class GetId
    {
        public int Id;
        public GetId(int Id)
        {
            this.Id = Id;
        }
    }
}
