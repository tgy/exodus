using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Exodus.PlayGame.Items.Units;
using Exodus.PlayGame.Items.Buildings;
using Exodus.PlayGame.Items.Obstacles;

namespace Exodus.PlayGame.Items
{
    static public class Loader
    {
        public static Unit LoadUnit(Type t, int IdPlayer)
        {
            if (t == typeof(Gunner))
                return new Gunner(IdPlayer);
            else if (t == typeof(Worker))
                return new Worker(IdPlayer);
            return null;
        }
        public static Building LoadBuilding(Type t, int IdPlayer)
        {
            if (t == typeof(Habitation))
                return new Habitation(IdPlayer);
            if (t == typeof(Labo))
                return new Labo(IdPlayer);
            return null;
        }
        public static Obstacle LoadObstacle(Type t)
        {
            if (t == typeof(Creeper))
                return new Creeper();
            return null;
        }
        public static Item LoadItem(Type t, int IdPlayer)
        {
            Item i = LoadUnit(t, IdPlayer);
            if (i != null)
                return i;
            i = LoadBuilding(t, IdPlayer);
            if (i != null)
                return i;
            return LoadObstacle(t);

        }
    }
}
