using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna;
using Microsoft.Xna.Framework;

namespace Exodus.PlayGame
{
    [Serializable]
    public abstract class Building : Obstacle
    {
        protected override void Initialize(int baseAnimDelay, int AnimNbFrames, int marginX, int marginY)
        {
            base.Initialize(baseAnimDelay, AnimNbFrames, marginX, marginY);
            this.MaxNumbTasks = 5;
        }
    }
}