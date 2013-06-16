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
            maxLife = 40000;
            maxShield = 0;
            Speed = 400;
            AttackStrength = 900;
            AttackDelayMax = 500;
            currentAttackDelay = AttackDelayMax;
            this.IdPlayer = IdPlayer;
            Initialize(20, 24, -47, -5);
            SightRange = 100;
            Range = 14;
        }
    }
}
