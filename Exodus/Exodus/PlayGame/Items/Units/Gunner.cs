using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Exodus.PlayGame.Items.Units
{
    [Serializable]
    class Gunner : Unit
    {
        public Gunner(int IdPlayer)
        {
            Name = "Marchand de Pruneaux";
            maxLife = 5000;
            maxShield = 0;
            Speed = 150;
            AttackStrength = 60;
            AttackDelayMax = 100;
            currentAttackDelay = AttackDelayMax;
            this.IdPlayer = IdPlayer;
            Initialize(40, 24, 9, 15);
            SightRange = 100;
            Range = 60;
        }
    }
}
