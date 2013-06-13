using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Exodus.GUI.Components;
using Exodus.GUI.Components.Buttons.GameButtons;
using Microsoft.Xna.Framework;
using Exodus.PlayGame;
using Microsoft.Xna.Framework.Graphics;

namespace Exodus.GUI.Items
{
    internal class SelectionDisplayer : Item
    {
        public SelectionDisplayer(int x, int y, float depth)
        {
            IsVisible = false;
            Depth = depth;
            Area = new Rectangle(x, y, Textures.GameUI["selection"].Width, Textures.GameUI["selection"].Height);
            Components = new List<Component>();
        }

        public override void Update(GameTime gameTime)
        {
            Components = new List<Component>();

            if (Map.ListSelectedItems.Count < 5)
                for (int i = 0; i < Map.ListSelectedItems.Count; i++)
                    Components.Add(new Big(Area.X + 10 + i * (Textures.GameUI["bigItem"].Width + 1), Area.Y + 10,
                                           Depth - (Data.GameDisplaying.Epsilon * 2), Map.ListItems.Find(u => u.PrimaryId == Map.ListSelectedItems[i])));

            else
                for (int i = 0; i < Map.ListSelectedItems.Count; i++)
                    Components.Add(new MiniItem(Area.X + 9 + (Textures.GameUI["smallItem"].Width + 3)*(i%9),
                                                Area.Y + 10 + (Textures.GameUI["smallItem"].Height + 3)*(i/9),
                                                Depth - 6*Data.GameDisplaying.Epsilon, i));

            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Textures.GameUI["selection"], Area, null, Color.White, 0f, Vector2.Zero, SpriteEffects.None,
                             Depth);
            base.Draw(spriteBatch);
        }
    }
}
