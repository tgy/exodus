using System;
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
                    if (!IsPlaceAvailable(pos) && !(this.child is Items.Buildings.HydrogenExtractor))
                        Finished = true;
                    else
                    {
                        if (this.child is Items.Buildings.HydrogenExtractor)
                        {
                            Items.Obstacles.Gas gas = (Items.Obstacles.Gas)Map.MapCells[this.pos.X, this.pos.Y].ListItems.FirstOrDefault(x => x is Items.Obstacles.Gas);
                            if (gas != null)
                            {
                                this.child.currentResource = gas.currentResource;
                            }
                        }
                        else
                        {
                            // On réserve l'espace !
                            for (int i = pos.X, mi = i + tempItem.Width; i < mi; i++)
                                for (int j = pos.Y, mj = j + tempItem.Width; j < mj; j++)
                                    Map.MapCells[i, j].ListItems.Add(new PlayGame.Items.Obstacles.Nothing1x1());
                        }
                        tempItem = this.Parent;
                        Move m = new Move(
                                this.Parent,
                                AStar.closestFreePoint(IsPlaceAvailable, this.pos, this.pos));
                        m.Initialize();
                        if (m.path == null)
                        {
                            Finished = true;
                            Network.ClientSide.Client.chat.InsertMsg("Product Canceled: Impossible to move near to the building location");
                            PlayGame.Map.PlayerResources += Data.GameInfos.CostsItems[child.GetType()];
                        }
                        else
                            tempItem.AddTask(m, false, true);
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
                        p = AStar.closestFreePoint(IsPlaceAvailable, pos, pos);
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
        public override void BruteFinish()
        {
            if (!Finished)
            {
                if (Initialized)
                {
                    if (moveToBuildingPoint)
                    {
                        for (int i = pos.X, mi = i + child.Width; i < mi; i++)
                            for (int j = pos.Y, mj = j + child.Width; j < mj; j++)
                                Map.MapCells[i, j].ListItems.RemoveAll(item => item is PlayGame.Items.Obstacles.Nothing1x1);
                    }
                }
                Map.PlayerResources += Data.GameInfos.CostsItems[child.GetType()];
            }
        }
        bool IsPlaceAvailable(Point p)
        {
            if (p.X < 0 || p.Y < 0 || p.X + tempItem.Width >= Map.Width || p.Y + tempItem.Width >= Map.Height)
                return false;
            for (int i = p.X, mi = i + tempItem.Width; i < mi; i++)
                for (int j = p.Y, mj = j + tempItem.Width; j < mj; j++)
                    if (Map.MapCells[i,j].ListItems.Count > 0)
                        return false;
            return true;
        }
        public Type GetTypeOfProduct()
        {
            return child.GetType();
        }
        /*void MakeObstacleCHildPos(bool obstacle)
        {
            for (int i = pos.X, mi = i + child.Width; i < mi; i++)
                for (int j = pos.Y, mj = j + child.Width; j < mj; j++)
                    Map.ObstacleMap[i, j] = obstacle;
        }*/
    }
}
