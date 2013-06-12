using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;

namespace Exodus.PlayGame
{
    [Serializable]
    public abstract class Unit : Item
    {
        protected enum Animation
        {
            MovTop = 5,
            MovTopRight = 4,
            MovRight = 3,
            MovDownRight = 2,
            MovDown = 1,
            MovDownLeft = 0,
            MovLeft = 7,
            MovTopLeft = 6,
            StandDownLeft = 8,
            StandDown = 9,
            StandDownRight = 10,
            StandRight = 11,
            StandTopRight = 12,
            StandTop = 13,
            StandTopLeft = 14,
            StandLeft = 15,
        }
        public Point? oldPos { get; set; }
        public float mediumPos { private get; set; }
        public SoundEffectInstance AttackSound;
        public SoundEffectInstance DieSound;
        protected new void Initialize(int baseAnimDelay, int AnimNbFrames, int marginX, int marginY)
        {
            this.oldPos = null;
            this.Width = 1;
            this.nbAnims = Enum.GetValues(typeof(Animation)).Length;
            this.TasksOnMenu = new List<MenuTask> { MenuTask.Attack, MenuTask.Hold, MenuTask.Patrol, MenuTask.Die };
            base.Initialize(baseAnimDelay, AnimNbFrames, marginX, marginY, "selectUnit");
            this.TasksOnMenu.Add(MenuTask.Hold);
            this.TasksOnMenu.Add(MenuTask.Patrol);
            this.TasksOnMenu.Add(MenuTask.Attack);
        }
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }
        protected override void UpdateScreenPosition()
        {
            if (oldPos == null)
                base.UpdateScreenPosition();
            else if (pos != null)
            {
                Vector2 p = Map.MapToScreen((int)pos.Value.X, (int)pos.Value.Y),
                        op = Map.MapToScreen((int)oldPos.Value.X, (int)oldPos.Value.Y);
                p.X = (1 - mediumPos) * p.X + mediumPos * op.X;
                p.Y = (1 - mediumPos) * p.Y + mediumPos * op.Y;
                UpdateScreenPosition(p);

                // changing direction depending on the next position
                switch (this.pos.Value.X - this.oldPos.Value.X)
                {
                    case -1:
                        switch (this.pos.Value.Y - this.oldPos.Value.Y)
                        {
                            case -1:
                                this.dir = Direction.Left;
                                break;
                            case 0:
                                this.dir = Direction.BottomLeft;
                                break;
                            case 1:
                                this.dir = Direction.Bottom;
                                break;
                            default:
                                throw new Exception("L'unite essaye d'acceder a une case non voisine");
                        }
                        break;
                    case 0:
                        switch (this.pos.Value.Y - this.oldPos.Value.Y)
                        {
                            case -1:
                                this.dir = Direction.TopLeft;
                                break;
                            case 0:
                                break;
                            case 1:
                                this.dir = Direction.BottomRight;
                                break;
                            default:
                                throw new Exception("L'unite essaye d'acceder a une case non voisine");
                        }
                        break;
                    case 1:
                        switch (this.pos.Value.Y - this.oldPos.Value.Y)
                        {
                            case -1:
                                this.dir = Direction.Top;
                                break;
                            case 0:
                                this.dir = Direction.TopRight;
                                break;
                            case 1:
                                this.dir = Direction.Right;
                                break;
                            default:
                                throw new Exception("L'unite essaye d'acceder a une case non voisine");
                        }
                        break;
                    default:
                        throw new Exception("L'unite essaye d'acceder a une case non voisine");
                }
            }
        }
        protected override void UpdateAnim()
        {
            if (TasksList.Count > 0)
                #region MàJ anim en mouvement
                switch (dir)
                {
                    case Direction.Left:
                        anim = (int)Animation.MovLeft;
                        break;
                    case Direction.Right:
                        anim = (int)Animation.MovRight;
                        break;
                    case Direction.Top:
                        anim = (int)Animation.MovTop;
                        break;
                    case Direction.Bottom:
                        anim = (int)Animation.MovDown;
                        break;
                    case Direction.BottomLeft:
                        anim = (int)Animation.MovDownLeft;
                        break;
                    case Direction.BottomRight:
                        anim = (int)Animation.MovDownRight;
                        break;
                    case Direction.TopLeft:
                        anim = (int)Animation.MovTopLeft;
                        break;
                    case Direction.TopRight:
                        anim = (int)Animation.MovTopRight;
                        break;
                    default:
                        throw new Exception("Statut de l'animation inconnu");

                }
                #endregion
            else
                #region Màj anim immobile
                switch (dir)
                {
                    case Direction.Left:
                        anim = (int)Animation.StandLeft;
                        break;
                    case Direction.Right:
                        anim = (int)Animation.StandRight;
                        break;
                    case Direction.Top:
                        anim = (int)Animation.StandTop;
                        break;
                    case Direction.Bottom:
                        anim = (int)Animation.StandDown;
                        break;
                    case Direction.BottomLeft:
                        anim = (int)Animation.StandDownLeft;
                        break;
                    case Direction.BottomRight:
                        anim = (int)Animation.StandDownRight;
                        break;
                    case Direction.TopLeft:
                        anim = (int)Animation.StandTopLeft;
                        break;
                    case Direction.TopRight:
                        anim = (int)Animation.StandTopRight;
                        break;
                    default:
                        throw new Exception("Statut de l'animation inconnu");

                }
                #endregion
        }
        [OnDeserializedAttribute]
        protected new void OnDeserialisation(StreamingContext context)
        {
            this._texture = Textures.GameItems[GetType().ToString() + IdPlayer];
            this._selectionCircle = Textures.Game["selectUnit"];
            this.bigLife = new GUI.Components.BigLife(this.screenPos.X, this.screenPos.Y, this.layerDepth);
        }
    }
}
