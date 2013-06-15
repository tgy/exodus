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
            maxLife = 700;
            maxShield = 0;
            Speed = 300;
            AttackStrength = 5;
            AttackDelayMax = 10;
            currentAttackDelay = AttackDelayMax;
            this.IdPlayer = IdPlayer;
            Initialize(20, 24, -47, -5);
            SightRange = 4;
            Range = 1.4f;
        }
    }
}
