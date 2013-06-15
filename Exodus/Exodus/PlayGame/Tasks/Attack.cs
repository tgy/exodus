using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Exodus.PlayGame.Tasks
{
    [Serializable]
    class Attack : Task
    {
        public Item Enemy;
        int delay;
        public Attack(Item parent, Item enemy, int delay) : base(parent, "Attack", "Attack the ennemy")
        {
            if (parent != null)
                this.Parent = parent;
            this.Enemy = enemy;
            this.delay = delay;
        }

        public override void Initialize()
        {
            //if (Parent is Unit && ((Unit)this.Parent).AttackSound != null)
            //    ((Unit)this.Parent).AttackSound.Play();
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            delay -= (int)gameTime.ElapsedGameTime.TotalMilliseconds;
            if (delay > 0)
                return;
            if (MustStop)
                this.Finished = true;
            if (!this.Finished)
            {
                // si l'ennemi n'est plus juste a côté
                if (AStar.Heuristic(this.Parent.pos.Value, this.Enemy.pos.Value) > 14)
                {
                    this.Parent.AddTask(new PlayGame.Tasks.Move(this.Parent, Enemy.pos.Value), false, true);
                }
                else
                {
                    if (this.Parent.AttackSound != null)
                        this.Parent.AttackSound.Play();
                    this.Parent.currentAttackDelay -= (int)gameTime.ElapsedGameTime.TotalMilliseconds;
                    if (this.Parent.currentAttackDelay < 0)
                    {
                        this.Parent.currentAttackDelay = this.Parent.AttackDelayMax;
                        this.Enemy.currentLife -= this.Parent.AttackStrength;
                        if (!(this.Enemy.TasksList.Count > 0 && (this.Enemy.TasksList[0] is Attack || this.Enemy.TasksList[0] is Move)))
                            this.Enemy.AddTask(new Attack(this.Enemy, this.Parent, 1000), true, false);
                        if (this.Enemy.currentLife < 0)
                        {
                            this.Finished = true;
                            if (this.Parent.AttackSound != null)
                                this.Parent.AttackSound.Stop();
                        }
                    }
                }
            }
        }
    }
}