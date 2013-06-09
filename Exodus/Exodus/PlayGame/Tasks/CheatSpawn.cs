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
        public CheatSpawn(Item parent, int timer, Item child, Point pos)
            : base(parent)
        {
            this.pos = pos;
            this.child = child;
        }
        public override void Initialize()
        {
            child.SetPos(pos.X, pos.Y, true);
            Map.AddItem(child);
            Finished = true;
        }
        public override void Update(GameTime gameTime)
        {
            Finished = true;
        }
    }
}
