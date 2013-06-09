using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Exodus.PlayGame.Tasks
{
    [Serializable]
    class Hold : Task
    {
        public Hold(Item parent) : base(parent, "Hold", "Hold the selected unit(s) position(s).")
        {
            if (parent != null)
                this.Parent = base.Parent;
            Finished = false;
        }
        public override void Initialize()
        {
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            if (MustStop)
                Finished = true;
        }
    }
}
