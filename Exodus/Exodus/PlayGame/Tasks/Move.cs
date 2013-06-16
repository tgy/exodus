using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Exodus.PlayGame.Tasks
{
    [Serializable]
    class Move : Task
    {
        public LinkedList<Point> path { get; protected set; }
        int timer,
            currentBaseTimer,
            baseTimer;
        Point Arrival;
        Unit parent;
        Func<int, int, bool> Arrived = null;
        public Move(Item parent, Point Arrival, Func<int, int, bool> Arrived)
            : this(parent, Arrival)
        {
            this.Arrived = Arrived;
        }
        public Move(Item parent, Point Arrival)
            : base(parent)
        {
            if (!Initialized)
            {
                if (parent.pos == null)
                    Finished = true;
                else
                {
                    this.Arrival = Arrival;
                    this.baseTimer = parent.Speed;
                    if (base.Parent is Unit)
                        this.parent = (Unit)base.Parent;
                    else
                        Finished = true;
                }
                Initialized = true;
            }
        }
        public override void Initialize()
        {
            if (parent == null)
                Finished = true;
            else
            {
                if (AStar.GetFreeNeighbors(this.parent.pos.Value).Count == 0)
                    return;
                if (this.Arrived == null)
                    this.path = PlayGame.AStar.Pathfind(parent.pos.Value, this.Arrival);
                else
                    this.path = PlayGame.AStar.Pathfind(parent.pos.Value, this.Arrival, this.Arrived);
                if (path == null || (path.Count == 1 && path.First.Equals(this.parent.pos)))
                {
                    this.Finished = true;
                    //// si le chemin est null a cause d'autres unites selectionnees, on recalcule le path
                    //List<Point> neighbors = AStar.GetAllNeighbors(parent.pos.Value);
                    //foreach (Point p in neighbors)
                    //{
                    //    if (Map.MapCells[p.X, p.Y].ListItems.Count > 0 && Map.ListSelectedItems.Exists(n => n == Map.MapCells[p.X, p.Y].ListItems[0].PrimaryId))
                    //        this.Finished = false;
                    //}
                }
                else
                {
                    this.parent.oldPos = parent.pos;
                    if (path.Count > 0)
                    {
                        this.parent.SetPos(path.First.Value.X, path.First.Value.Y, true);
                        path.RemoveFirst();
                    }
                    this.parent.mediumPos = 0;
                }
            }
        }
        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            if (!Finished)
            {
                if (path == null)
                {
                    if (this.Arrived != null)
                        path = AStar.Pathfind(parent.pos.Value, this.Arrival, this.Arrived);
                    else
                        path = AStar.Pathfind(parent.pos.Value, this.Arrival);
                }
                else
                {
                    if (timer <= 0)
                    {
                        this.parent.mediumPos = 1;
                        parent.oldPos = parent.pos;
                        if (path.Count > 0 && !MustStop &&
                            // si l'unité est plus loin de l'arrivée que la case libre la plus proche
                            AStar.Heuristic(AStar.closestFreePoint(x => x.X >= 0 && x.X < Map.Width && x.Y >= 0 && x.Y < Map.Height && Map.MapCells[x.X, x.Y].ListItems.Count == 0, this.Arrival, this.parent.pos.Value), this.Arrival) < AStar.Heuristic((Point)this.parent.pos, this.Arrival))
                        {
                            if (Map.MapCells[path.First.Value.X, path.First.Value.Y].ListItems.Count == 0)
                            {
                                parent.SetPos(path.First.Value.X, path.First.Value.Y, true);
                                SetSpeed();
                                path.RemoveFirst();
                            }
                            else
                                this.Initialize();
                        }
                        else
                            Finished = true;
                    }
                    else
                    {
                        timer -= (int)gameTime.ElapsedGameTime.TotalMilliseconds;
                        parent.mediumPos = (float)timer / (float)currentBaseTimer;
                    }
                }
            }
        }
        void SetSpeed()
        {
            int i = Math.Abs(this.parent.pos.Value.X - this.parent.oldPos.Value.X) +
                    Math.Abs(this.parent.pos.Value.Y - this.parent.oldPos.Value.Y);
            if (i == 1)
                currentBaseTimer = baseTimer;
            else
                currentBaseTimer = (int)(1.41421f * baseTimer);
            timer = currentBaseTimer;
        }
    }
}
