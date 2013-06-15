using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Exodus.PlayGame.Tasks
{
    [Serializable]
    class HarvestIron : Task
    {
        public Item iron;
        public HarvestIron(Item parent, Item iron)
            : base(parent, "Attack", "Attack the ennemy")
        {
            if (parent != null)
                this.Parent = parent;
            this.iron = iron;
        }

        public override void Initialize()
        {
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            if (MustStop)
                this.Finished = true;
            if (AStar.Heuristic(this.Parent.pos.Value, this.iron.pos.Value) > 14)
                this.Parent.AddTask(new PlayGame.Tasks.Move(this.Parent, iron.pos.Value), false, true);
            else
            {
                if (this.iron.currentResource.Iron > 0)
                {
                    this.iron.currentResource -= (gameTime.ElapsedGameTime.TotalMilliseconds / 1000) * new Resource(0, 5, 0, 0, 0);
                    this.iron.currentLife = (int)(this.iron.currentResource.Iron / this.iron.maxResource.Iron * 100);
                    Map.PlayerResources += (gameTime.ElapsedGameTime.TotalMilliseconds / 1000) * new Resource(0, 5, 0, 0, 0);
                }
                else
                {
                    this.iron.AddTask(new Die(this.iron), true, false);
                    this.Finished = true;
                }
            }
        }
    }
}