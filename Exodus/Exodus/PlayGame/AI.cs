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
        public static Items.Buildings.Habitation baseAI;
        public static Random rand = new Random();
        public static int id;
        
        public static void Load(int idAI)
        {
            id = idAI;
        }

        public static void Update(TimeSpan time)
        {
            if (task == 0 && time >= new TimeSpan(0, 0, 0, 2))
            {
                baseAI = (Items.Buildings.Habitation)First<Items.Buildings.Habitation>(true);
                if (baseAI != null)
                {
                    Items.Units.Worker w;
                    for (int i = 0; i < 5; i++)
                    {
                        w = new Items.Units.Worker(id);
                        baseAI.AddTask(new Tasks.ProductItem(baseAI, Data.GameInfos.timeCreatingItem[w.GetType()], w, baseAI.pos.Value, true, true, false), false, false);
                    }
                }
                task++;
            }
            else if (baseAI != null)
            {
                if (task == 1 && time >= new TimeSpan(0, 0, 0, 10))
                {
                    Items.Units.Worker w = (Items.Units.Worker)Map.ListItems.FirstOrDefault(x => x is Items.Units.Worker && x.IdPlayer == id);
                    if (w != null)
                    {
                        Items.Buildings.HydrogenExtractor h = new Items.Buildings.HydrogenExtractor(id);
                        Items.Obstacles.Gas g = (Items.Obstacles.Gas)Closest<Items.Obstacles.Gas>(false);
                        if (g != null)
                            w.AddTask(new Tasks.ProductItem(w, Data.GameInfos.timeCreatingItem[h.GetType()], h, g.pos.Value, true, false, true), true, false);
                    }
                    task++;
                }
                else if (task == 2 && time >= new TimeSpan(0, 0, 0, 11))
                {
                    Items.Units.Worker w2 = (Items.Units.Worker)Map.ListItems.FirstOrDefault(x => x is Items.Units.Worker && x.IdPlayer == id && x.TasksList.Count == 0);
                    if (w2 != null)
                        w2.AddTask(new Tasks.ProductItem(w2, Data.GameInfos.timeCreatingItem[typeof(Items.Buildings.Habitation)], new Items.Buildings.Habitation(id), new Point(86, rand.Next(120, 150)), true, true, true), true, false);
                    task++;
                }
                else if (task == 3 && time >= new TimeSpan(0, 0, 0, 12))
                {
                    Items.Units.Worker w3 = (Items.Units.Worker)Map.ListItems.FirstOrDefault(x => x is Items.Units.Worker && x.IdPlayer == id && x.TasksList.Count == 0);
                    Items.Obstacles.Iron i = (Items.Obstacles.Iron)Closest<Items.Obstacles.Iron>(false);
                    if (w3 != null && i != null)
                    {
                        w3.AddTask(new Tasks.HarvestIron(w3, i), true, false);
                    }
                    task++;
                }
                else if ((task - 4) % 3 == 0 && time >= new TimeSpan(0, 0, 0, 15) && (time.Seconds - 15) % 15 == 0)
                {
                    for (int i = 0; i < rand.Next(10); i++)
                    {
                        ProduceRandomUnit();
                    }
                    task++;
                }
                else if ((task - 5) % 3 == 0 && time >= new TimeSpan(0, 0, 0, 20) && (time.Seconds - 20) % 15 == 0)
                {
                    GetNUnitAndAttackRandomItem(rand.Next(10));
                    task++;
                }
                else if ((task - 6) % 3 == 0 && time >= new TimeSpan(0, 0, 0, 25) && (time.Seconds - 25) % 15 == 0)
                {
                    GetUselessUnitToBase();
                    task++;
                }
            }
        }

        private static Item First<T>(bool activeItem)
        {
            return (activeItem ? Map.ListItems : Map.ListPassiveItems).FirstOrDefault(x => x is T && x.IdPlayer == id);
        }

        private static Item First<T>(bool activeItem, List<Item> different)
        {
            return (activeItem ? Map.ListItems : Map.ListPassiveItems).FirstOrDefault(x => x is T && x.IdPlayer == id && !different.Contains(x));
        }

        private static Item Closest<T>(bool activeItem)
        {
            Item min = null;
            foreach (Item t in (activeItem ? Map.ListItems : Map.ListPassiveItems))
            {
                if (t is T)
                {
                    if (min == null)
                        min = t;
                    else if (AStar.Heuristic(t.pos.Value, baseAI.pos.Value) < AStar.Heuristic(min.pos.Value, baseAI.pos.Value))
                        min = t;
                }
            }
            return min;
        }

        private static void ProduceRandomUnit()
        {
            List<Item> l = new List<Item>();
            foreach (Item i in Map.ListItems)
            {
                if (i is Items.Buildings.Habitation && i.IdPlayer == id)
                {
                    switch (rand.Next(4))
                    {
                        case 0:
                            i.AddTask(new Tasks.ProductItem(i, Data.GameInfos.timeCreatingItem[typeof(Items.Units.Gunner)], new Items.Units.Gunner(id), i.pos.Value, true, true, false), false, false);
                            break;
                        case 1:
                            i.AddTask(new Tasks.ProductItem(i, Data.GameInfos.timeCreatingItem[typeof(Items.Units.Laserman)], new Items.Units.Laserman(id), i.pos.Value, true, true, false), false, false);
                            break;
                        case 2:
                            i.AddTask(new Tasks.ProductItem(i, Data.GameInfos.timeCreatingItem[typeof(Items.Units.Spider)], new Items.Units.Spider(id), i.pos.Value, true, true, false), false, false);
                            break;
                        case 3:
                            i.AddTask(new Tasks.ProductItem(i, Data.GameInfos.timeCreatingItem[typeof(Items.Units.Worker)], new Items.Units.Worker(id), i.pos.Value, true, true, false), false, false);
                            break;
                    }
                }
            }
        }

        private static void GetNUnitAndAttackRandomItem(int n)
        {
            List<Item> enemyItems = new List<Item>();
            foreach (Item i in Map.ListItems)
            {
                if (i.IdPlayer != id)
                {
                    enemyItems.Add(i);
                }
            }
            Item item = enemyItems[rand.Next(enemyItems.Count)];
            int count = 0;
            foreach (Item i in Map.ListItems)
            {
                if (i is Unit && !(i is Items.Units.Worker) && i.IdPlayer == id && i.TasksList.Count == 0)
                {
                    i.AddTask(new Tasks.Attack(i, item, 0), false, false);
                    count++;
                }
                if (count == n)
                    break;
            }
        }

        private static void GetUselessUnitToBase()
        {
            foreach (Item i in Map.ListItems)
            {
                if (i is Unit && i.IdPlayer == id && i.TasksList.Count == 0)
                    i.AddTask(new Tasks.Move(i, baseAI.pos.Value), true, false);
            }
        }
    }
}
