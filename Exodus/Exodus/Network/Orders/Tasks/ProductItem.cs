using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Exodus.Network.Orders.Tasks
{
    [Serializable]
    class ProductItem : ItemTask
    {
        public int x;
        public int y;
        public bool closestFreePosition,
                    canBeDeletedDuringProduction,
                    moveToBuildingPoint;
        public Type child;
        public int nextId = 0;
        public ProductItem(long parentPrimaryKey, bool overrideTask, Type child, int x, int y, bool closestFreePosition, bool canBeDeletedDuringProduction, bool moveToBuildingPoint)
            : base(parentPrimaryKey, overrideTask)
        {
            this.overrideTask = overrideTask;
            this.x = x;
            this.y = y;
            this.child = child;
            this.closestFreePosition = closestFreePosition;
            this.canBeDeletedDuringProduction = canBeDeletedDuringProduction;
            this.moveToBuildingPoint = moveToBuildingPoint;
        }
    }
}
