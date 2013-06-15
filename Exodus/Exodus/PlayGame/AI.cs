using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Exodus.PlayGame
{
    static class AI
    {
        public static void Update(DateTime time, int id)
        {
            if (time >= new DateTime(0, 0, 0, 0, 0, 10, 0))
            {
                Items.Buildings.Habitation h = (Items.Buildings.Habitation)Map.ListItems.FirstOrDefault(x => x is Items.Buildings.Habitation);
                if (h != null)
                {
                    Items.Units.Worker w = new Items.Units.Worker(id);
                    h.AddTask(new Tasks.ProductItem(h, Data.GameInfos.timeCreatingItem[w.GetType()], w, h.pos.Value, true, true, false), true, false);
                }
            }
        }
    }
}
