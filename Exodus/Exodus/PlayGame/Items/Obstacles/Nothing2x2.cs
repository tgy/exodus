using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Exodus.PlayGame.Items.Obstacles
{
    [Serializable]
    class Nothing2x2 : Obstacle
    {
        public Nothing2x2()
        {
            Name = "Nothing";
            maxLife = Int32.MaxValue;
            maxShield = Int32.MaxValue;
            Width = 2;
            base.Initialize(Int32.MaxValue, 1, 0, 0);
        }
    }
}
