using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Exodus.GUI.Items
{
    class ParticleEngine : Item
    {
        Exodus.ParticleEngine.ParticleEngine engine;
        public ParticleEngine(Exodus.ParticleEngine.ParticleEngine p)
        {
            this.engine = p;
        }
        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            engine.Update();
            base.Update(gameTime);
        }
        public override void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch)
        {
            engine.Draw(spriteBatch);
            base.Draw(spriteBatch);
        }
    }
}
