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

namespace Exodus.ParticleEngine.Particles
{
    class Stars : Particle
    {
        public Stars(float x, float y, List<Texture2D> l, int textureID, int fecondite, int dureeVie, float direction, float limitationDirection, int speed, float rotSpeed)
        {
            this.listParticleTextures = l;
            this.textureID = textureID;
            if (l[textureID] != null)
                center = new Vector2(l[textureID].Width / 2, l[textureID].Height / 2);
            else
                center = new Vector2(0, 0);
            this.pos = new Vector2(x, y);
            this.fecondite = fecondite;
            this.dureeVieBase = dureeVie;
            this.dureeVie = dureeVie;
            this.limitationDirection = limitationDirection;
            direction = (float)(2 * limitationDirection * r.NextDouble() - limitationDirection + direction);
            this.direction = direction;
            this.speed = (speed * (5 + r.Next(10))) / 10;
            this.dirX = (float)(Math.Cos(direction) * this.speed);
            this.dirY = (float)(Math.Sin(direction) * this.speed);
            this.rotSpeed = rotSpeed;
            if (this.fecondite != 0)
            {
                bornDelay = 0;
                randomRangeBorn = 0;
            }
            NextChild();
            alphaBase = 0.7f;
            alphaLimitation = 0.3f;
            alphaStep = 0.01f;
            alphaBut = alphaBase;
            currentAlpha = alphaBut;
            layerDepth = 0.999999f;
        }
        public override List<Particle> Enfante()
        {
            if (fecondite != 0 && nextChild <= 0)
            {
                NextChild();
                fecondite--;
                return new List<Particle> { new Stars(pos.X, pos.Y, listParticleTextures, 2 + r.Next(4), 0, 400, direction, limitationDirection, 4, 0) };
            }
            return new List<Particle>();
        }
        protected override void Deplacer()
        {
            pos.X += dirX;
            pos.Y -= dirY;
            currentRot = (float)((currentRot + rotSpeed) % (2 * Math.PI));
        }
        public override List<Particle> Reincarnation()
        {

            //return new List<Particle> { new MyParticle2(pos.X, pos.Y, listParticleTextures, 3, 0, 10, 0, limitationDirection, 0, (float)r.NextDouble() / 5) };
            return new List<Particle>();
        }
    }
}
