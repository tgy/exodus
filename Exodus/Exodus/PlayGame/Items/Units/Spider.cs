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
            maxLife = 4657;
            maxShield = 69;
            Speed = 100;
            AttackStrength = 69;
            AttackDelayMax = 10;
            currentAttackDelay = AttackDelayMax;
            this.IdPlayer = IdPlayer;
            Initialize(40, 16, 0, 0);
        }
    }
}
