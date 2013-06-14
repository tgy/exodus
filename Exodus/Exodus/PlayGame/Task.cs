using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Exodus.GUI.Components;
using Exodus.GUI.Items;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.Threading;
using Exodus.PlayGame;
using Exodus.Network;

namespace Exodus.PlayGame
{
    [Serializable]
    public abstract class Task
    {
        public Item Parent { get; protected set; }
        public bool Finished { get; protected set; }
        public bool MustStop { protected get; set; }
        public bool Initialized { get; protected set; }
        [NonSerialized]public SoundEffectInstance Sound;

        protected Task(Item parent, string name, string description)
        {
            Parent = parent;
            Finished = false;
            MustStop = false;
            Name = name;
            Description = description;
        }

        protected Task(Item parent) : this(parent, string.Empty, string.Empty)
        {
            this.Parent = parent;
            Finished = false;
            MustStop = false;
        }
        public abstract void Initialize();
        public abstract void Update(GameTime gameTime);
        public virtual void BruteFinish()
        {

        }
        public string Name;
        public string Description;
    }
    public enum MenuTask
    {
        Attack,
        Hold,
        Die,
        Patrol,
        Build,
        ProductUnits,
        Research,
        ChangeResources
    }
}
