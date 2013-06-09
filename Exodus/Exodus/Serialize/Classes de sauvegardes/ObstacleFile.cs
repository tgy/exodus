using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Exodus.PlayGame
{
    [Serializable]
    public class ObstacleFile : ItemFile
    {
        public int width, height;
        public int paddingX, paddingY;
    }
}
