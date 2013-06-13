using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Exodus.PlayGame.Items.Units;

namespace Exodus.PlayGame.Tasks
{
    [Serializable]
    class ChangeResource : Task
    {
        Resource Cost, Gain;
        public float Progress { get; private set; }
        int timer, baseTimer;
        public ChangeResource(Item parent, int timer, Resource Cost, Resource Gain)
            : base(parent)
        {
            this.Cost = Cost;
            this.Gain = Gain;
            this.timer = timer;
            this.baseTimer = this.timer;
        }
        public override void Initialize()
        {
            if (!Initialized)
            {
                Initialized = true;
                if (!(PlayGame.Map.PlayerResources >= Cost))
                    Finished = true;
                else
                    PlayGame.Map.PlayerResources -= Cost;
            }
        }
        public override void Update(GameTime gameTime)
        {
            if (!Finished)
            {
                if (timer > 0)
                {
                    timer -= (int)gameTime.ElapsedGameTime.TotalMilliseconds;
                    Progress = (1 - (float)timer / (float)baseTimer) * 100;
                }
                else
                {
                    Finished = true;
                    PlayGame.Map.PlayerResources += Gain;
                }
            }
        }
    }
}
