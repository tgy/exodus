using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Exodus.PlayGame.Items.Units
{
    [Serializable]
    class Spider : Unit
    {
        public Spider(int IdPlayer)
        {
            Name = "Spider Pig";
            maxLife = 150;
            maxShield = 0;
            Speed = 100;
            AttackStrength = 5;
            AttackDelayMax = 100;
            currentAttackDelay = AttackDelayMax;
            this.IdPlayer = IdPlayer;
            Initialize(40, 16, 0, 0);
        }
    }
}
