using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Exodus.PlayGame.Items.Units;

namespace Exodus.PlayGame.Tasks
{
    [Serializable]
    class CheatSpawn : Task
    {
        PlayGame.Item child;
        Point pos;
        int primaryId = 0;
        public CheatSpawn(Item parent, int timer, Item child, Point pos, int primaryId)
            : this(parent, timer, child, pos)
        {
            this.primaryId = primaryId;
        }
        public CheatSpawn(Item parent, int timer, Item child, Point pos)
            : base(parent)
        {
            this.pos = pos;
            this.child = child;
        }
        public override void Initialize()
        {
            if (child != null)
            {
                child.SetPos(pos.X, pos.Y, true);
                child.PrimaryId = primaryId;
                if (child is Building || child is Unit)
                    Map.AddItem(child);
                else
                    Map.AddPassiveItem(child);
                Finished = true;
            }
            else
                Finished = true;
        }
        public override void Update(GameTime gameTime)
        {
            Finished = true;
        }
    }
}
