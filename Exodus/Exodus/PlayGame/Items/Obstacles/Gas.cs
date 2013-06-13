using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Exodus.PlayGame.Items.Obstacles
{
    [Serializable]
    class Gas : Obstacle
    {
        public Gas()
        {
            Name = "Gas";
            maxLife = 666;
            maxShield = 666;
            Width = 2;
            base.Initialize(Int32.MaxValue, 1, 25, 24);
        }
    }
}
