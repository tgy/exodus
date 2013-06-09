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

namespace Exodus.ParticleEngine
{
    class ParticleEngine
    {
        protected List<Particle> listParticles = new List<Particle>();
        protected List<Texture2D> listTexture2D = new List<Texture2D>();
        public ParticleEngine(int x, int y, int textureID)
        {
        }
        public void Update()
        {
            List<Particle> lp;
            for (int i = 0; i < listParticles.Count; i++)
            {
                // On met à jour la particule
                listParticles[i].Update();
                // Si on a des enfants crées, on les rajoute.
                lp = listParticles[i].Enfante();
                if (lp.Any())
                    listParticles.AddRange(lp);
                // Si la particule meurt, on la retire.
                if (listParticles[i].Mourir())
                {
                    // On ajoute les éventuels réincarnés.
                    lp = listParticles[i].Reincarnation();
                    listParticles.RemoveAt(i);
                    if (lp.Any())
                        listParticles.InsertRange(i, lp);
                }
            }
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (Particle p in listParticles)
                p.Draw(spriteBatch);
        }
        public void Reset()
        {
            listParticles = new List<Particle> { listParticles[0] };
        }
    }
}