using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Exodus.PlayGame.Items.Obstacles
{
    [Serializable]
    class Iron : Obstacle
    {
        public Iron()
        {
            Name = "Iron";
            maxLife = 100;
            maxShield = 666;
            Width = 2;
            base.Initialize(Int32.MaxValue, 1, 0, 0);
            this.maxResource = new Resource(0, 5000, 0, 0, 0);
            this.currentResource = new Resource(0, 5000, 0, 0, 0);
        }

        protected override void UpdateAnim()
        {
            anim = (int)(this.currentResource.Iron > 0 ? Animation.Anim : Animation.Stand);
        }
    }
}
