using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Exodus.PlayGame.Tasks
{
    class Wait : Task
    {
        public int n;
        public Wait(Item parent, int n) : base(parent, "Wait", "Wait")
        {
            this.n = n;
        }
        public override void Initialize()
        {
        }
        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            this.n -= gameTime.ElapsedGameTime.Milliseconds;
            if (n <= 0)
            {
                this.Finished = true;
            }
        }
    }
}
