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
            MovDownLeft = 0,
            MovDown = 1,
            MovDownRight = 2,
            MovRight = 3,
            MovTopRight = 4,
            MovTop = 5,
            MovTopLeft = 6,
            MovLeft = 7,
            StandDownLeft = 8,
            StandDown = 9,
            StandDownRight = 10,
            StandRight = 11,
            StandTopRight = 12,
            StandTop = 13,
            StandTopLeft = 14,
            StandLeft = 15,
            AttackDownLeft = 16,
            AttackDown = 17,
            AttackDownRight = 18,
            AttackRight = 19,
            AttackTopRight = 20,
            AttackTop = 21,
            AttackTopLeft = 22,
            AttackLeft = 7
        }
        public Point? oldPos { get; set; }
        public float mediumPos { private get; set; }
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
                this.dir = GetDir(this.pos.Value, this.oldPos.Value);
            }
        }
        protected override void UpdateAnim()
        {
            if (TasksList.Count > 0)
            {
                if (TasksList[0] is Tasks.Move)
                {
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
                }
                #endregion
                else if (TasksList[0] is Tasks.Attack)
                {
                    this.dir = GetDir(((Tasks.Attack)TasksList[0]).Enemy.pos.Value, this.pos.Value);
                    #region MàJ anim attack
                    switch (dir)
                    {
                        case Direction.Left:
                            anim = (int)Animation.AttackLeft;
                            break;
                        case Direction.Right:
                            anim = (int)Animation.AttackRight;
                            break;
                        case Direction.Top:
                            anim = (int)Animation.AttackTop;
                            break;
                        case Direction.Bottom:
                            anim = (int)Animation.AttackDown;
                            break;
                        case Direction.BottomLeft:
                            anim = (int)Animation.AttackDownLeft;
                            break;
                        case Direction.BottomRight:
                            anim = (int)Animation.AttackDownRight;
                            break;
                        case Direction.TopLeft:
                            anim = (int)Animation.AttackTopLeft;
                            break;
                        case Direction.TopRight:
                            anim = (int)Animation.AttackTopRight;
                            break;
                        default:
                            throw new Exception("Statut de l'animation inconnu");
                    }
                    #endregion
                }
            }
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
            this.AttackSound = Audio.Attack[GetType()];
            this.DieSound = Audio.Die[GetType()];
            this.SelectionSound = Audio.Selection[GetType()];
            this.Focused = Map.ListSelectedItems.Contains(this.PrimaryId);
        }

        private Direction GetDir(Point pos, Point oldPos)
        {
            Point dir = new Point(pos.X == oldPos.X ? 0 : (pos.X - oldPos.X) / Math.Abs(pos.X - oldPos.X), pos.Y == oldPos.Y ? 0 : (pos.Y - oldPos.Y) / Math.Abs(pos.Y - oldPos.Y));
            switch (dir.X)
            {
                case -1:
                    switch (dir.Y)
                    {
                        case -1:
                            return Direction.Left;
                        case 0:
                            return Direction.BottomLeft;
                        case 1:
                            return Direction.Bottom;
                        default:
                            throw new Exception("Not possible");
                    }
                case 0:
                    switch (dir.Y)
                    {
                        case -1:
                            return Direction.TopLeft;
                        case 0:
                            return this.dir;
                        case 1:
                            return Direction.BottomRight;
                        default:
                            throw new Exception("Not possible");
                    }
                case 1:
                    switch (dir.Y)
                    {
                        case -1:
                            return Direction.Top;
                        case 0:
                            return Direction.TopRight;
                        case 1:
                            return Direction.Right;
                        default:
                            throw new Exception("Not possible");
                    }
                default:
                    throw new Exception("Not possible");
            }
        }
    }
}
