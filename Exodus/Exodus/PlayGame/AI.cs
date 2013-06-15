using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Exodus.PlayGame
{
    static class AI
    {
        public static int task = 0;
        public static void Update(TimeSpan time, int id)
        {
            if (task == 0 && time >= new TimeSpan(0, 0, 0, 5))
            {
                Items.Buildings.Habitation h = (Items.Buildings.Habitation)Map.ListItems.FirstOrDefault(x => x is Items.Buildings.Habitation && x.IdPlayer == id);
                if (h != null)
                {
                    Items.Units.Worker w;
                    for (int i = 0; i < 5; i++)
                    {
                        w = new Items.Units.Worker(id);
                        h.AddTask(new Tasks.ProductItem(h, Data.GameInfos.timeCreatingItem[w.GetType()], w, h.pos.Value, true, true, false), false, false);
                    }
                }
                task++;
            }
            if (task == 1 && time >= new TimeSpan(0, 0, 0, 15))
            {
                Items.Units.Worker w = (Items.Units.Worker)Map.ListItems.FirstOrDefault(x => x is Items.Units.Worker && x.IdPlayer == id);
                if (w != null)
                {
                    Items.Buildings.HydrogenExtractor h = new Items.Buildings.HydrogenExtractor(id);
                    Items.Obstacles.Gas g = (Items.Obstacles.Gas)Map.ListItems.FirstOrDefault(x => x is Items.Obstacles.Gas);
                    if (g != null)
                        w.AddTask(new Tasks.ProductItem(w, Data.GameInfos.timeCreatingItem[h.GetType()], h, g.pos.Value, false, false, true), true, false);
                }
                Items.Units.Worker w2 = (Items.Units.Worker)Map.ListItems.FirstOrDefault(x => x is Items.Units.Worker && x.IdPlayer == id && x != w);
                if (w2 != null)
                    w2.AddTask(new Tasks.ProductItem(w2, Data.GameInfos.timeCreatingItem[typeof(Items.Buildings.Habitation)], new Items.Buildings.Habitation(id), new Point(86, 135), true, true, true), true, false);
                Items.Units.Worker w3 = (Items.Units.Worker)Map.ListItems.FirstOrDefault(x => x is Items.Units.Worker && x.IdPlayer == id && x != w && x != w2);
                Items.Obstacles.Iron i = (Items.Obstacles.Iron)Map.ListItems.FirstOrDefault(x => x is Items.Obstacles.Iron);
                if (w3 != null && i != null)
                {
                    i.AddTask(new Tasks.HarvestIron(w3, i), true, false);
                }
            }
        }
    }
}
