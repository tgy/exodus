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
    public abstract class Item
    {
        public enum Direction
        {
            Right,
            TopRight,
            Top,
            TopLeft,
            Left,
            BottomLeft,
            Bottom,
            BottomRight,
        }
        #region Variables

        #region GameInfos
        public Point? pos { get; set; }
        public int currentShield,
                   currentLife;
        public int maxShield { get; protected set; }
        public int maxLife { get; protected set; }
        public int Range { get; protected set; }
        public int SightRange { get; protected set; }
        public int AttackDelayMax { get; protected set; }
        public int currentAttackDelay;
        public int AttackStrength;
        public int Speed { get; protected set; }
        public int Width { get; protected set; }
        [NonSerialized]public GUI.Components.BigLife bigLife;
        [NonSerialized]protected Texture2D _texture;
        [NonSerialized]protected Texture2D _selectionCircle;
        [NonSerialized]public SoundEffectInstance AttackSound;
        [NonSerialized]public SoundEffectInstance DieSound;
        [NonSerialized]public SoundEffectInstance SelectionSound;
        public Resource resourcesGeneration { get; protected set; }
        [OnDeserializedAttribute]
        protected void OnDeserialisation(StreamingContext context)
        {
            this._texture = Textures.GameItems[GetType().ToString() + IdPlayer];
            this.bigLife = new GUI.Components.BigLife(this.screenPos.X, this.screenPos.Y, this.layerDepth);
            this.AttackSound = Audio.Attack[GetType()];
            this.DieSound = Audio.Die[GetType()];
            this.SelectionSound = Audio.Selection[GetType()];
            //this.Focused = Map.ListSelectedItems.Contains(this.PrimaryId);
        }
        public string Name { get; protected set; }
        #endregion

        #region ConfigInfos
        //[NonSerialized]public bool Focused = false;
        // Représente la position de la texture à l'écran
        protected Rectangle screenPos;
        // Représente la position du cercle de sélection si l'unité est dessinée
        Vector2 selectionPosition;
        // Direction actuelle de l'item
        public Direction dir;
        // Position du calque parmi les autres
        public float layerDepth { get; protected set; }
        // Direction animation gerée automatiquement
        protected int nbAnims = 0;
        protected int anim = 0;
        // Rectangle source de la planche de sprites
        protected Rectangle spriteAnimRect;
        // Timer d'animation
        int currentAnimDelay = 0,
            baseAnimDelay = Int32.MaxValue;
        // Marges à gauche et en bas pour positionner plus précisémment l'item
        int marginX = 0,
            marginY = 0;
        public float Alpha = 1f;
        public Color Color = Color.White;
        #endregion

        // File d'actions que l'unité doit effectuer
        protected int MaxNumbTasks = Int32.MaxValue;
        public List<Task> TasksList { get; protected set; }

        public List<MenuTask> TasksOnMenu { get; protected set; }
        public List<Type> ItemsProductibles { get; protected set; }
        public int IdPlayer { get; protected set; }
        public int PrimaryId;
        public Resource currentResource;
        public Resource maxResource;
        #endregion

        #region Fonctions

        #region Initialisation
        /// <summary>
        /// Fonction de base de chargement de l'unité
        /// </summary>
        protected virtual void Initialize(int baseAnimDelay, int AnimNbFrames, int marginX, int marginY, string selectionCircle)
        {
            _selectionCircle = Textures.Game[selectionCircle];
            this.Initialize(baseAnimDelay, AnimNbFrames, marginX, marginY);
        }
        protected virtual void Initialize(int baseAnimDelay, int AnimNbFrames, int marginX, int marginY)
        {
            this.pos = null;
            this.baseAnimDelay = baseAnimDelay;
            this.currentLife = maxLife;
            this.currentShield = maxShield;
            this.marginX = marginX;
            this.marginY = marginY;
            this.TasksList = new List<Task>();
            this._texture = Textures.GameItems[GetType().ToString()+IdPlayer];
            this.spriteAnimRect = new Rectangle(0, 0, _texture.Width / AnimNbFrames, _texture.Height / this.nbAnims);
            this.screenPos = new Rectangle(0, 0, spriteAnimRect.Width, spriteAnimRect.Height);
            this.selectionPosition = new Vector2(0, 0);
            this.dir = (Direction)5;
            this.TasksOnMenu = new List<MenuTask> { MenuTask.Die };
            this.ItemsProductibles = new List<Type>();
            this.bigLife = new GUI.Components.BigLife(this.screenPos.X, this.screenPos.Y, this.layerDepth);
            this.bigLife.Value = 100;
            this.AttackSound = Audio.Attack[GetType()];
            this.DieSound = Audio.Die[GetType()];
            this.SelectionSound = Audio.Selection[GetType()];
            this.resourcesGeneration = new Resource(0,0,0,0,0);
            this.maxResource = new Resource(0, 0, 0, 0, 0);
            this.currentResource = new Resource(0, 0, 0, 0, 0);
            this.SightRange = 0;
        }
        /// <summary>
        /// Initialise la position de l'unité
        /// </summary>
        /// <param name="x">X (map)</param>
        /// <param name="y">Y (map)</param>
        public virtual void SetPos(int x, int y, bool secure)
        {
            if (pos != null && secure)
            {
                // On met toutes les cases ex-occupée par notre item à libre
                for (int i = (int)pos.Value.X, mi = Math.Min(i + Width, Map.Width); i < mi; i++)
                    for (int j = (int)pos.Value.Y, mj = Math.Min(j + Width, Map.Height); j < mj; j++)
                    {
                        //Map.ObstacleMap[i, j] = false;
                        Map.MapCells[i, j].ListItems.Remove(this);
                    }

            }
            pos = new Point(x, y);
            if (secure)
            {
                // On met toutes les cases occupées par notre item à occupées
                for (int i = x, mi = Math.Min(i + Width, Map.Width); i < mi; i++)
                    for (int j = y, mj = Math.Min(j + Width, Map.Height); j < mj; j++)
                    {
                        //Map.ObstacleMap[i, j] = true;
                        Map.MapCells[i, j].ListItems.Clear();
                        Map.MapCells[i, j].ListItems.Add(this);
                    }
            }
        }
        /// <summary>
        /// Initialise la position de notre screenPosition, ou la met à jour
        /// </summary>
        protected virtual void UpdateScreenPosition()
        {
            if (pos != null)
            {
                UpdateScreenPosition(Map.MapToScreen((int)pos.Value.X, (int)pos.Value.Y));
            }
        }
        protected void UpdateScreenPosition(Vector2 p)
        {
            this.layerDepth = Map.GetLayerDepth(p);
            screenPos.X = (int)(p.X + marginX - Camera.x);
            screenPos.Y = (int)(p.Y - screenPos.Height + (Width + 1) * Tile.tileHeight / 2 - marginY - Camera.y);
            selectionPosition.X = p.X + (Width * Tile.tileWidth - _selectionCircle.Width) / 2 - Camera.x;
            selectionPosition.Y = p.Y + (Tile.tileHeight - _selectionCircle.Height) / 2 - Camera.y;
        }
        protected abstract void UpdateAnim();
        #endregion

        public virtual void Update(GameTime gameTime)
        {
            #region MàJ Tâches
            if (TasksList.Count > 0)
            {
                if (TasksList[0] != null)
                {
                    TasksList[0].Update(gameTime);
                    if (TasksList[0].Finished)
                    {
                        TasksList.RemoveAt(0);
                        if (TasksList.Count > 0)
                            TasksList[0].Initialize();
                    }
                }
                else
                    TasksList.RemoveAt(0);
            }
            #endregion
            #region MàJ de l'anim
            // On met à jour la catégorie d'animation !
            UpdateAnim();
            currentAnimDelay -= (int)gameTime.ElapsedGameTime.TotalMilliseconds;
            // S'il est temps de changer d'image afin d'éviter de lasser le joueur à toujours voir la même image n.n
            if (currentAnimDelay < 0)
            {
                currentAnimDelay = baseAnimDelay;
                spriteAnimRect.X = (spriteAnimRect.X + spriteAnimRect.Width) % _texture.Width;
            }
            // On prend la série de sprites à l'angle adapté à celui voulu
            spriteAnimRect.Y = (spriteAnimRect.Height * (int)anim) % _texture.Height;
            #endregion
            #region Killed
            if (this.currentLife <= 0)
            {
                if (Data.Network.SinglePlayer)
                    this.AddTask(new PlayGame.Tasks.Die(this), true, false);
                else
                    Network.ClientSide.Client.SendObject(new Network.Orders.Tasks.Die(this.PrimaryId, false));
            }
            #endregion
            this.bigLife.Depth = this.layerDepth;
            this.bigLife.Value = 100 * currentLife / maxLife;
            this.bigLife.Area.X = this.screenPos.X + this.screenPos.Width / 2 - this.bigLife.Area.Width / 2;
            this.bigLife.Area.Y = this.screenPos.Y;
            this.bigLife.Update(gameTime);
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            UpdateScreenPosition();
            if (pos != null)
            {
                spriteBatch.Draw(_texture,
                                 screenPos,
                                 spriteAnimRect,
                                 Color * Alpha,
                                 0f,
                                 Vector2.Zero,
                                 SpriteEffects.None,
                                 layerDepth
                );
                if (Map.ListSelectedItems.Exists(n => n == PrimaryId))
                {
                    spriteBatch.Draw(_selectionCircle,
                                     selectionPosition,
                                     null,
                                     Color,
                                     0f,
                                     Vector2.Zero,
                                     1f,
                                     SpriteEffects.None,
                                     layerDepth + Data.GameDisplaying.Epsilon);
                }
            }
            if (this.Intersect(Inputs.MouseState.X, Inputs.MouseState.Y) || Map.ListSelectedItems.Contains(PrimaryId))
                this.bigLife.Draw(spriteBatch);
        }

        public bool Intersect(int x, int y)
        {
            // Si on est hors de la texture, on oublie.
            if (!screenPos.Contains(x, y))
                return false;
            // Sinon on vérifie que la couleur est bien transparente.
            uint[] color = new uint[1];
            _texture.GetData(0, new Rectangle(spriteAnimRect.X + x - screenPos.X, spriteAnimRect.Y + y - screenPos.Y, 1, 1), color, 0, 1);
            return (color[0] > 0x00FFFFFF);
        }
        public bool Intersect(Rectangle r)
        {
            if (!screenPos.Intersects(r))
                return false;
            // Pour l'instant, on ne fait pas la suite.
            // return true;
            Rectangle intersect = new Rectangle(0, 0, 0, 0);
            intersect.X = Math.Max(r.X, screenPos.X);
            intersect.Y = Math.Max(r.Y, screenPos.Y);
            intersect.Width = Math.Min(r.X + r.Width, screenPos.X + screenPos.Width) - intersect.X;
            intersect.Height = Math.Min(r.Y + r.Height, screenPos.Y + screenPos.Height) - intersect.Y;
            if (intersect.Width != 0 && intersect.Height != 0)
            {
                intersect.X -= screenPos.X - spriteAnimRect.X;
                intersect.Y -= screenPos.Y - spriteAnimRect.Y;
                uint[] color = new uint[intersect.Width * intersect.Height];
                _texture.GetData(0, intersect, color, 0, color.Length);
                foreach (uint c in color)
                    if (c > 0x00FFFFFF)
                        return true;
            }
            return false;
        }
        public bool IsActive()
        {
            return (TasksList.Count > 0);
        }
        public void AddTask(Task t, bool overrideTasks, bool First)
        {
            if (TasksList.Count < MaxNumbTasks)
            {
                if (!First)
                {
                    // Si on doit arrêter toutes les tasks
                    if (overrideTasks)
                    {
                        /*if (TasksList.Count >= 1)
                        {
                            Task temp = TasksList[0];
                            temp.MustStop = true;
                            TasksList.Clear();
                            TasksList.Add(temp);
                        }*/
                        for (int i = 0; i < TasksList.Count; )
                        {
                            if (i == 0)
                            {
                                TasksList[0].MustStop = true;
                                i++;
                            }
                            else
                            {
                                TasksList[i].BruteFinish();
                                TasksList.RemoveAt(i);
                            }

                        }
                    }
                    if (TasksList.Count == 0)
                        t.Initialize();
                    if (!t.Finished)
                        TasksList.Add(t);
                }
                else
                {
                    t.Initialize();
                    if (!t.Finished)
                    {
                        if (TasksList.Count > 0)
                            TasksList.Insert(0, t);
                        else if (TasksList.Count == 0)
                            TasksList.Add(t);
                    }
                    else if (TasksList.Count > 0)
                    {
                        TasksList[0].MustStop = true;
                    }
                }
            }
        }
        public void DelTask(int Position)
        {
            if (Position < TasksList.Count)
            {
                if (Position == 0)
                    TasksList[0].MustStop = true;
                else
                {
                    TasksList[Position].BruteFinish();
                    TasksList.RemoveAt(Position);
                }
            }
        }
        #endregion
    }
}