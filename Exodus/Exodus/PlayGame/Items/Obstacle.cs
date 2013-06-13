using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Exodus.PlayGame;

namespace Exodus.PlayGame
{
    [Serializable]
    public abstract class Obstacle : Item
    {
        enum Animation
        {
            Stand = 0,
            Anim = 1
        }
        protected virtual new void Initialize(int baseAnimDelay, int AnimNbFrames, int marginX, int marginY)
        {
            this.nbAnims = Enum.GetValues(typeof(Animation)).Length;
            base.Initialize(baseAnimDelay, AnimNbFrames, marginX, marginY, "selectBuilding" + Width);
            this.MaxNumbTasks = 0;
        }
        protected override void UpdateAnim()
        {
            anim = (int)((TasksList.Count > 0) ? Animation.Anim : Animation.Stand);
        }
        [OnDeserializedAttribute]
        protected new void OnDeserialisation(StreamingContext context)
        {
            this._texture = Textures.GameItems[GetType().ToString() + IdPlayer];
            this._selectionCircle = Textures.Game["selectBuilding" + Width];
            this.bigLife = new GUI.Components.BigLife(this.screenPos.X, this.screenPos.Y, this.layerDepth);
            this.AttackSound = Audio.Attack[GetType()];
            this.DieSound = Audio.Die[GetType()];
            this.SelectionSound = Audio.Selection[GetType()];
            this.Focused = Map.ListSelectedItems.Contains(this);
        }
    }
}
