using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Exodus.ParticleEngine.Engine
{
    class StarsEngine : ParticleEngine
    {
        public StarsEngine(int x, int y, int textureID)
            : base(x, y, textureID)
        {
            this.listTexture2D = new List<Texture2D>
            {
                    null,
                    Textures.Particles["star1"],
                    Textures.Particles["star2"],
                    Textures.Particles["star3"],
                    Textures.Particles["star4"],
                    Textures.Particles["star5"],
            };
            this.listParticles.Add(new Particles.Stars(x, y, this.listTexture2D, 0, -1, -1, 0, (float)Math.PI, 0, 0));
            this.listParticles.Add(new Particles.Stars(x, y, this.listTexture2D, 0, -1, -1, 1, (float)Math.PI, 0, 0));
            this.listParticles.Add(new Particles.Stars(x, y, this.listTexture2D, 0, -1, -1, 2, (float)Math.PI, 0, 0));
            //this.listParticles.Add(new Particles.Stars(x, y, this.listTexture2D, 0, -1, -1, 3, (float)Math.PI, 0, 0));
        }
    }
}