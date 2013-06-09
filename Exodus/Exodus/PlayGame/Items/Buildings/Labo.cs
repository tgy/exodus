using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Exodus.PlayGame.Items.Buildings
{
    [Serializable]
    class Labo : Building
    {
        public Labo(int IdPlayer)
        {
            Name = "La Bo et Le Bete";
            maxLife = 700;
            maxShield = 300;
            Width = 2;
            this.IdPlayer = IdPlayer;
            base.Initialize(40, 10, 5, 6);
            this.ItemsProductibles = new List<Type>
            {
            };
            this.TasksOnMenu = new List<MenuTask>
            {
                MenuTask.Research
            };
        }
    }
}
