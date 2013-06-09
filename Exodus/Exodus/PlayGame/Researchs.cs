using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Exodus.PlayGame
{
    public static class Researchs
    {
        public class Research
        {
            public int Level
            {
                get
                {
                    return level;
                }
                set
                {
                    level = value >= 0 ? MaxLevel < value ? MaxLevel : value : 0;
                    NextCost = PricePercentage(level) * basePrice;
                    Efficience = (float)Researchs.Efficience(level);
                    NextTime = (int)(baseTime * PercentageTimeToSearch(level));
                }
            }
            int level = 0;
            public Resource NextCost { get; private set; }
            public int NextTime { get; private set; }
            public float Efficience { get; private set; }
            Resource basePrice;
            int baseTime;
            public int MaxLevel { get; private set; }
            public string Name { get; private set; }
            public string Dscr { get; private set; }
            public Research(string name, string dscr, Resource basePrice, int maxLevel, int baseTime)
            {
                this.basePrice = basePrice;
                this.MaxLevel = maxLevel;
                this.Name = name;
                this.Dscr = dscr;
                this.baseTime = baseTime;
                this.NextTime = baseTime;
                this.Level = 0;
            }
        }
        public static Research Attack = new Research("", "", new Resource(100, 0, 0, 100, 50), 15, 1000);
        public static Research Defense = new Research("", "", new Resource(50, 25, 0, 50, 40), 15, 1000);
        public static Research Range = new Research("", "", new Resource(0, 100, 75, 100, 0), 15, 5000);
        public static Research Shield = new Research("", "", new Resource(0, 50, 125, 50, 42), 15, 3000);
        public static Research ShieldRegeneration = new Research("", "", new Resource(100, 100, 100, 0, 100), 15, 6500);
        static float Efficience(int level)
        {
            int result = 0;
            while (level > 0)
            {
                result += level;
                level--;
            }
            return 1 + .1f * result;
        }
        static float PricePercentage(int level)
        {
            float result = 1;
            while (level > 1)
            {
                result *= 1.35f + .02f * level;
                level--;
            }
            return result;
        }
        static float PercentageTimeToSearch(int level)
        {
            float result = 0;
            while (level > 0)
            {
                result += (level + 1);
                level--;
            }
            return 1f + .13f * result;
        }
    }
}
