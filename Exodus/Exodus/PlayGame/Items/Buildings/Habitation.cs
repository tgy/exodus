using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Exodus.PlayGame.Items.Buildings
{
    [Serializable]
    class Habitation : Building
    {
        public Habitation(int IdPlayer)
        {
            Name = "Photocopieuse";
            maxLife = 30000;
            maxShield = 0;
            Width = 2;
            this.IdPlayer = IdPlayer;
            base.Initialize(40, 10, 5, 6);
            this.resourcesGeneration = new Resource(0, 0, 0, 0, 5);
            this.ItemsProductibles = new List<Type>
            {
                typeof(PlayGame.Items.Units.Worker),
                typeof(PlayGame.Items.Units.Spider),
                typeof(PlayGame.Items.Units.Gunner),
                typeof(PlayGame.Items.Units.Laserman)
            };
            this.TasksOnMenu = new List<MenuTask>
            {
                MenuTask.ProductUnits,
            };
        }
    }
}
