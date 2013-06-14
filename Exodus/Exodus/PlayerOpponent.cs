using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace Exodus
{
    public static class PlayerOpponent
    {
        public static byte ConnectionState = 2;
        public static Texture2D avatar = null;
        public static string avatarURL = "";
        public static int rank = -1,
                          victories = -1,
                          defeats = -1;
    }
}
