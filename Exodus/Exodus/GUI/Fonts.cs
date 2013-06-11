using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Exodus.GUI
{
    public static class Fonts
    {
        public static SpriteFont Eurostile10,
                                 Eurostile10Bold,
                                 Eurostile12,
                                 Eurostile12Bold,
                                 Eurostile16Bold,
                                 Eurostile18,
                                 Eurostile18Bold,
                                 Eurostile24,
                                 Eurostile60,
                                 Arial9;

        public static void Initialize(ContentManager content)
        {
            Eurostile10 = content.Load<SpriteFont>("Fonts/Eurostile/10");
            Eurostile10Bold = content.Load<SpriteFont>("Fonts/Eurostile/10Bold");
            Eurostile12 = content.Load<SpriteFont>("Fonts/Eurostile/12");
            Eurostile12Bold = content.Load<SpriteFont>("Fonts/Eurostile/12Bold");
            Eurostile16Bold = content.Load<SpriteFont>("Fonts/Eurostile/16Bold");
            Eurostile18 = content.Load<SpriteFont>("Fonts/Eurostile/18");
            Eurostile18Bold = content.Load<SpriteFont>("Fonts/Eurostile/18Bold");
            Eurostile24 = content.Load<SpriteFont>("Fonts/Eurostile/24");
            Eurostile60 = content.Load<SpriteFont>("Fonts/Eurostile/60");
            Arial9 = content.Load<SpriteFont>("Fonts/Arial9");
        }
    }
}
