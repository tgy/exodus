﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Exodus.PlayGame.Items.Units;

namespace Exodus.PlayGame.Tasks
{
    [Serializable]
    class ProductItem : Task
    {
        int timer, baseTimer;
        public float Progress { get; private set; }
        Point pos;
        Item child, tempItem;
        bool closestFreePosition;
        bool canBeDeletedDuringProduction;
        bool moveToBuildingPoint;
        public ProductItem(Item parent, int timer, Item child, Point pos, bool closestFreePosition, bool canBeDeletedDuringProduction, bool moveToBuildingPoint)
            : base(parent)
        {
            this.pos = pos;
            this.child = child;
            this.closestFreePosition = closestFreePosition;
            this.timer = timer;
            this.baseTimer = timer;
            this.canBeDeletedDuringProduction = canBeDeletedDuringProduction;
            this.moveToBuildingPoint = moveToBuildingPoint;
        }
        public override void Initialize()
        {
            if (!Initialized)
            {
                Initialized = true;
                // Si on doit bouger jusqu'au point de création pour construire.
                if (this.moveToBuildingPoint)
                {
                    tempItem = this.child;
                    // Si la place est prise, alors on oublie :/
                    if (!IsPlaceAvailable(pos))
                        Finished = true;
                    else
                    {
                        // On réserve l'espace !
                        MakeObstacleCHildPos(true);
                        tempItem = this.Parent;
                        Move m = new Move(
                                this.Parent,
                                AStar.closestFreePoint(IsPlaceAvailable, this.pos));
                        m.Initialize();
                        if (m.path == null)
                            Finished = true;
                        else
                            tempItem.AddTask(m,false,true);
                    }
                }
            }
        }
        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            if (timer > 0)
            {
                if (MustStop && canBeDeletedDuringProduction)
                    Finished = true;
                timer -= (int)gameTime.ElapsedGameTime.TotalMilliseconds;
                Progress = (1 - (float)timer / (float)baseTimer) * 100;
            }
            else
            {
                tempItem = this.child;
                // Si on cherche à placer à la position pos et uniquement à cette position là
                if (!closestFreePosition)
                {
                    // Si la place est libre, on l'ajoute, sinon... va s'faire voir n.n
                    if (IsPlaceAvailable(pos))
                    {
                        child.SetPos(pos.X, pos.Y, true);
                        Map.AddItem(child);
                    }
                }
                // On veut placer notre item à la position la plus proche de notre point de départ
                else
                {
                    // On récupère la place vide la plus proche
                    Point? p;
                    if (moveToBuildingPoint)
                        p = pos;
                    else
                        p = AStar.closestFreePoint(IsPlaceAvailable, pos);
                    // si cette place existe, alors on ajoute l'item, sinon on envoie voir n.n
                    if (p != null)
                    {
                        child.SetPos(p.Value.X, p.Value.Y, true);
                        Map.AddItem(child);
                    }
                }
                Finished = true;
            }
        }// Regarde si la possibilité de construction à la position (x,y) est valide
        bool IsPlaceAvailable(Point p)
        {
            if (p.X < 0 || p.Y < 0 || p.X + tempItem.Width >= Map.Width || p.Y + tempItem.Width >= Map.Height)
                return false;
            for (int i = p.X, mi = i + tempItem.Width; i < mi; i++)
                for (int j = p.Y, mj = j + tempItem.Width; j < mj; j++)
                    if (Map.ObstacleMap[i, j])
                        return false;
            return true;
        }
        public Type GetTypeOfProduct()
        {
            return child.GetType();
        }
        void MakeObstacleCHildPos(bool obstacle)
        {
            for (int i = pos.X, mi = i + child.Width; i < mi; i++)
                for (int j = pos.Y, mj = j + child.Width; j < mj; j++)
                    Map.ObstacleMap[i, j] = obstacle;
        }
    }
}