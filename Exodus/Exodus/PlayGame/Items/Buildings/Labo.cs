using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Exodus.PlayGame.Items.Buildings
{
    [Serializable]
    class Laboratory : Building
    {
        public Laboratory(int IdPlayer)
        {
            Name = "Laboratory";
            maxLife = 40000;
            maxShield = 0;
            Width = 3;
            this.IdPlayer = IdPlayer;
            base.Initialize(40, 21, 7, 8);
            this.TasksOnMenu = new List<MenuTask>
            {
                MenuTask.ChangeResources,
            };
        }
    }
}
