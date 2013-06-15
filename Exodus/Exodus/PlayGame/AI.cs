using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Exodus.PlayGame
{
    static class AI
    {
        static int task = 0;
        public static void Update(TimeSpan time, int id)
        {
            if (task == 0 && time >= new TimeSpan(0, 0, 0, 10))
            {
                Items.Buildings.Habitation h = (Items.Buildings.Habitation)Map.ListItems.FirstOrDefault(x => x is Items.Buildings.Habitation && x.IdPlayer == id);
                if (h != null)
                {
                    Items.Units.Worker w = new Items.Units.Worker(id);
                    h.AddTask(new Tasks.ProductItem(h, Data.GameInfos.timeCreatingItem[w.GetType()], w, h.pos.Value, true, true, false), true, false);
                }
                task++;
            }
        }
    }
}
