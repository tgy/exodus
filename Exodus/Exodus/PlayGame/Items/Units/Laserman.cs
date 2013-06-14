using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Exodus.PlayGame.Items.Units
{
    [Serializable]
    class Laserman : Unit
    {
        public Laserman(int IdPlayer)
        {
            Name = "Laserman";
            maxLife = 4242;
            maxShield = 4242;
            Speed = 520;
            AttackStrength = 69;
            AttackDelayMax = 10;
            currentAttackDelay = AttackDelayMax;
            this.IdPlayer = IdPlayer;
            Initialize(20, 24, -47, -5);
        }
    }
}
