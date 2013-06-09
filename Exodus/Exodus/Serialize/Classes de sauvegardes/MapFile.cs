using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Exodus.PlayGame
{
    [Serializable]
    public class MapFile
    {
        public int[,] map;
        public int xMin = 0,
                   xMax = Int32.MaxValue,
                   yMin = 0,
                   yMax = Int32.MaxValue;
        public string author = "unknown",
                      time = "00:00 AM";
        public List<UnitFile> listUnit = new List<UnitFile>();
        public List<BuildingFile> listBuildings = new List<BuildingFile>();
        public List<ObstacleFile> listObstacle = new List<ObstacleFile>();
    }
}
