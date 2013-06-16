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
            else if (t == typeof(Spider))
                return new Spider(IdPlayer);
            else if (t == typeof(Laserman))
                return new Laserman(IdPlayer);
            return null;
        }
        public static Building LoadBuilding(Type t, int IdPlayer)
        {
            if (t == typeof(Habitation))
                return new Habitation(IdPlayer);
            if (t == typeof(University))
                return new University(IdPlayer);
            if (t == typeof(Laboratory))
                return new Laboratory(IdPlayer);
            if (t == typeof(HydrogenExtractor))
                return new HydrogenExtractor(IdPlayer);
            return null;
        }
        public static Obstacle LoadObstacle(Type t)
        {
            if (t == typeof(Creeper))
                return new Creeper();
            if (t == typeof(Nothing1x1))
                return new Nothing1x1();
            if (t == typeof(Nothing2x2))
                return new Nothing2x2();
            if (t == typeof(Iron))
                return new Iron();
            if (t == typeof(Gas))
                return new Gas();
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
