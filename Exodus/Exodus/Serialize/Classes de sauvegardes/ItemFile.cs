using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Exodus.PlayGame
{
    [Serializable]
    public class ItemFile
    {
        public int x, y;
        public Rectangle position;
        public string name;
        public bool selected;
        public int maxLife,
                   currentLife;
        public int maxShield,
                   currentShield;
        public string type;
        public int IdPlayer;
        public int posInList;
    }
}
