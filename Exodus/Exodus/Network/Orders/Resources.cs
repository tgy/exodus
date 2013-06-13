using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Exodus.Network.Orders
{
    [Serializable]
    class Resources
    {
        public int Id,
                   Electricity,
                   Hydrogen,
                   Iron,
                   Graphene,
                   Steel;
        public Resources(int Id, PlayGame.Resource resources)
        {
            this.Id = Id;
            this.Electricity = (int)resources.Electricity;
            this.Graphene = (int)resources.Graphene;
            this.Hydrogen = (int)resources.Hydrogen;
            this.Iron = (int)resources.Iron;
            this.Steel = (int)resources.Steel;
        }
    }
}
