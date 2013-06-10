using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Exodus.PlayGame.Items;
using Exodus.PlayGame;

namespace Exodus.Network.Orders.Tasks
{
    [Serializable]
    class ReSync : Task
    {
        public List<Item> listPassives,
                          listItems;
        public ReSync(List<Item> listPassives, List<Item> listItems)
        {
            this.listItems = listItems;
            this.listPassives = listPassives;
        }
    }
}
