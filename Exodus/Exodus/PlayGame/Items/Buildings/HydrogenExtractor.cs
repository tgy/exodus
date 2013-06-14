using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Exodus.PlayGame.Items.Buildings
{
    class HydrogenExtractor : Building
    {
        public HydrogenExtractor(int IdPlayer)
        {
            Name = "Hydrogen extractor";
            maxLife = 666;
            maxShield = 666;
            Width = 2;
            this.IdPlayer = IdPlayer;
            base.Initialize(Int32.MaxValue, 1, 20, 6);
            this.resourcesGeneration.Electricity = 5;
            this.resourcesGeneration = new Resource(0, 0, 0, 5, 0);
        }
    }
}
