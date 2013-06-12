using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Exodus.PlayGame.Tasks
{
    [Serializable]
    class Attack : Task
    {
        Item Enemy;
        public Attack(Item parent, Item enemy) : base(parent, "Attack", "Attack the ennemy")
        {
            if (parent != null)
                this.Parent = parent;
            this.Enemy = enemy;
        }

        public override void Initialize()
        {
            if (Parent is Unit && ((Unit)this.Parent).AttackSound != null)
                ((Unit)this.Parent).AttackSound.Play();
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            // si l'ennemi n'est plus juste a côté
            if (Math.Abs(Enemy.pos.Value.X - this.Parent.pos.Value.X) + Math.Abs(Enemy.pos.Value.Y - this.Parent.pos.Value.Y) > 2)
            {
                this.Parent.AddTask(new PlayGame.Tasks.Move(this.Parent, Enemy.pos.Value), false, true);
            }
            else
            {
                this.Parent.currentAttackDelay -= (int)gameTime.ElapsedGameTime.TotalMilliseconds;
                if (this.Parent.currentAttackDelay < 0)
                {
                    this.Parent.currentAttackDelay = this.Parent.AttackDelayMax;
                    this.Enemy.currentLife -= this.Parent.AttackStrength;
                    if (this.Enemy.currentLife < 0)
                    {
                        this.Finished = true;
                        if (Parent is Unit && ((Unit)this.Parent).AttackSound != null)
                            ((Unit)this.Parent).AttackSound.Stop();
                    }
                }
            }
        }
    }
}
