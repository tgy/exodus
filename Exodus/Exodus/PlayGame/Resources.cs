using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Exodus.PlayGame
{
    public static class Resources
    {
        public static Resource Resource = new Resource(0, 0, 0, 0, 0);
        public static void Reset()
        {
            Resource.Electricity = 10;
            Resource.Hydrogen = 0;
            Resource.Iron = 500;
            Resource.Steel = 0;
            Resource.Graphene = 0;
        }
        public static void Draw(SpriteBatch spriteBatch)
        {

            spriteBatch.DrawString(GUI.Fonts.Eurostile12, "Resources: [Electricity]" + Resources.Resource.Electricity + " [Hydrogen]" + Resources.Resource.Hydrogen + " [Iron]" + Resources.Resource.Iron + " [Steel]" + Resources.Resource.Steel + " [Graphene]" + Resources.Resource.Graphene, new Vector2(Data.Window.WindowWidth - 500, 0), Color.White);
        }
    }
}
