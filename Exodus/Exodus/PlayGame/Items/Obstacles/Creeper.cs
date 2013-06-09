using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Exodus.PlayGame.Items.Obstacles
{
    [Serializable]
    class Creeper : Obstacle
    {
        public Creeper()
        {
            Name = "Creeper";
            maxLife = 666;
            maxShield = 666;
            Width = 2;
            base.Initialize(Int32.MaxValue, 1, 25, 20);
        }
    }
}
