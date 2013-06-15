using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Exodus.PlayGame.Tasks
{
    [Serializable]
    class Attack : Task
    {
        public Item Enemy;
        public Func<Point, Point, bool> Arrived;
        int delay;
        public Attack(Item parent, Item enemy, int delay) : base(parent, "Attack", "Attack the ennemy")
        {
            if (parent != null)
                this.Parent = parent;
            this.Enemy = enemy;
            this.Arrived = ((p1, p2) => AStar.Heuristic(p1, p2) <= this.Parent.Range * 10);
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
                if (Enemy == null || Parent == null)
                    Finished = true;
                else
                {
                    // si l'ennemi n'est plus juste a côté
                    if (!Arrived(Parent.pos.Value, Enemy.pos.Value))
                    {
                        this.Parent.AddTask(new PlayGame.Tasks.Move(this.Parent, Enemy.pos.Value, Arrived), false, true);
                    }
                    else
                    {
                        if (this.Parent.AttackSound != null)
                            this.Parent.AttackSound.Play();
                        if (Enemy is Unit && !(this.Enemy.TasksList.Count > 0 && (this.Enemy.TasksList[0] is Attack || this.Enemy.TasksList[0] is Move)))
                            this.Enemy.AddTask(new Attack(this.Enemy, this.Parent, 1000), true, false);
                        this.Parent.currentAttackDelay -= (int)gameTime.ElapsedGameTime.TotalMilliseconds;
                        if (this.Parent.currentAttackDelay < 0)
                        {
                            this.Parent.currentAttackDelay = this.Parent.AttackDelayMax;
                            this.Enemy.currentLife -= this.Parent.AttackStrength;
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
}