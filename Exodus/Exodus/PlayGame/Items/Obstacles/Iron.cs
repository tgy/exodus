using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Exodus.PlayGame.Items.Obstacles
{
    [Serializable]
    class Iron : Obstacle
    {
        public int Value = 5000;
        public Iron()
        {
            Name = "Iron";
            maxLife = 666;
            maxShield = 666;
            Width = 2;
            base.Initialize(Int32.MaxValue, 1, 0, 0);
        }

        protected override void UpdateAnim()
        {
            anim = (int)(Value > 0 ? Animation.Anim : Animation.Stand);
        }
    }
}
