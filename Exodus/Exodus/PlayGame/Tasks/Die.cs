using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Exodus.PlayGame.Tasks
{
    [Serializable]
    class Die : Task
    {
        public Die(Item parent)
            : base(parent, "Die", "Delete the selected item")
        {
        }

        public override void Initialize()
        {
            if (Parent != null)
            {
                Map.ListItems.Remove(Parent);
                Map.MapCells[Parent.pos.Value.X, Parent.pos.Value.Y]
                    .ListItems.Remove(Parent);
                Map.ListSelectedItems.Remove(Parent);
                for (int x = Parent.pos.Value.X, mx = x + Parent.Width; x < mx; x++)
                    for (int y = Parent.pos.Value.Y, my = y + Parent.Width; y < my; y++)
                        Map.ObstacleMap[x, y] = false;
                Finished = true;
            }
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
        }
    }
}
