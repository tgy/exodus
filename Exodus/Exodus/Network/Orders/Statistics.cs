using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Exodus.Network.Orders
{
    [Serializable]
    class Statistics
    {
        public int InternetId,
                   Electricity,
                   Hydrogen,
                   Iron,
                   Graphene,
                   Steel,
                   UnitsTrained,
                   UnitsLost,
                   ArmyValue;

        public Statistics(int InternetId, PlayGame.Resource resources, int ArmyValue)
        {
            this.InternetId = InternetId;
            this.Electricity = (int)resources.Electricity;
            this.Graphene = (int)resources.Graphene;
            this.Hydrogen = (int)resources.Hydrogen;
            this.Iron = (int)resources.Iron;
            this.Steel = (int)resources.Steel;
            UnitsLost = ClientSide.Client.UnitsLost;
            UnitsTrained = ClientSide.Client.UnitsTrained;
            this.ArmyValue = ArmyValue;
        }
    }
}
