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
    public abstract class Particle
    {
        protected Random r = new Random();
        protected Vector2 pos = new Vector2(0, 0);
        protected Vector2 center = new Vector2(0, 0);
        protected int textureID = 0;
        protected int fecondite = 0,
                      bornDelay = 0,
                      nextChild = 0,
                      randomRangeBorn = 0;
        protected int dureeVieBase = -1,
                      dureeVie = -1;
        protected float direction = 0,
                        limitationDirection = 0,
                        dirX = 0,
                        dirY = 0;
        protected double speed = 0;
        protected float rotSpeed = 0,
                        currentRot = 0;
        protected float alphaBase = 1,
                        alphaLimitation = 0,
                        currentAlpha = 0.5f,
                        alphaStep = 0.1f,
                        alphaBut;
        int timer = 0;
        protected float layerDepth;
        protected List<Texture2D> listParticleTextures;

        public void Draw(SpriteBatch spriteBatch)
        {
            if (listParticleTextures[textureID] != null)
                spriteBatch.Draw(listParticleTextures[textureID],
                                 pos,
                                 null,
                                 Color.White * (dureeVie > 9 ? currentAlpha : (dureeVie < 0 ? (dureeVieBase - dureeVie < 10 && dureeVie > 0 ? (float)currentAlpha * (dureeVieBase - dureeVie) / 10 : currentAlpha) : (float)currentAlpha * dureeVie / 10)),
                                 currentRot,
                                 center,
                                 1f,
                                 SpriteEffects.None,
                                 layerDepth
            );
        }
        public void Update()
        {
            Deplacer();
            UpdateAlpha();
            if (fecondite != 0)
            {
                nextChild--;
            }
            if (dureeVie > 0)
            {
                dureeVie--;
            }
        }
        public bool Mourir()
        {
            return (dureeVie == 0);
        }
        protected void NextChild()
        {
            nextChild = bornDelay + r.Next(randomRangeBorn * 2) - randomRangeBorn;
        }
        protected abstract void Deplacer();
        public abstract List<Particle> Enfante();
        public abstract List<Particle> Reincarnation();
        public void UpdateAlpha()
        {
            if (alphaLimitation > 0 && timer == 0)
            {
                if (alphaBut == currentAlpha)
                {
                    if (alphaBase > alphaBut)
                    {
                        alphaBut = (float)(alphaBase + alphaLimitation * r.NextDouble());
                    }
                    else
                    {
                        alphaBut = (float)(alphaBase - alphaLimitation * r.NextDouble());
                    }
                }
                else if (alphaBut > currentAlpha)
                {
                    currentAlpha += alphaStep;
                }
                else
                {
                    currentAlpha -= alphaStep;
                }
                timer = 0;

            }
            else
                timer--;
        }
    }
}
