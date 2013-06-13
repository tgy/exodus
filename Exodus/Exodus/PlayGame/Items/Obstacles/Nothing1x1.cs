using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Exodus.PlayGame.Items.Obstacles
{
    [Serializable]
    class Nothing1x1 : Obstacle
    {
        public Nothing1x1()
        {
            Name = "Nothing";
            maxLife = Int32.MaxValue;
            maxShield = Int32.MaxValue;
            Width = 1;
            base.Initialize(Int32.MaxValue, 1, 0, 0);
        }
    }
}
