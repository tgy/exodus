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
            maxLife = 4242;
            maxShield = 4242;
            Speed = 200;
            AttackStrength = 69;
            AttackDelayMax = 10;
            currentAttackDelay = AttackDelayMax;
            this.IdPlayer = IdPlayer;
            this.AttackSound = Audio.Attack["Gunner"];
            Initialize(40, 24, 9, 15);
        }
    }
}
