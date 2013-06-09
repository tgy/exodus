using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Exodus.PlayGame;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Exodus.GUI.Components.Buttons.GameButtons
{
    internal class Big : GameButton
    {
        public BigLife BigLife;
        private Label _currentLife;
        private readonly Label _maxLife;
        public PlayGame.Item Item;

        public Big(int x, int y, float depth, PlayGame.Item item)
        {
            Area = new Rectangle(x, y, Textures.GameUI["bigItem"].Width, Textures.GameUI["bigItem"].Height);
            Item = item;
            Depth = depth;
            BigLife = new BigLife(x + 2, y + 63, Depth - 6*Data.GameDisplaying.Epsilon);
            _maxLife = new Label(Fonts.Arial9, "", x + 32, y + 70) {Color = new Color(22, 127, 176)};
            _currentLife = new Label(Fonts.Arial9, "", 0, y + 70);
        }

        public override void Update(GameTime gameTime)
        {
            BigLife.Value = 100*Item.currentLife/Item.maxLife;

            _maxLife.Txt = " / " + Map.ListSelectedItems[0].maxLife.ToString();
            _currentLife.Txt = Map.ListSelectedItems[0].currentLife.ToString();

            _currentLife.Pos.X = (int)(_maxLife.Pos.X - Fonts.Arial9.MeasureString(_currentLife.Txt).X);

            int percentage = 100 * Map.ListSelectedItems[0].currentLife / Map.ListSelectedItems[0].maxLife;
            Color color;
            if (percentage >= 75)
                color = new Color(91, 224, 43);
            else if (percentage >= 50)
                color = new Color(255, 221, 61);
            else if (percentage >= 25)
                color = new Color(230, 130, 9);
            else
                color = new Color(218, 33, 13);

            _currentLife.Color = color;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Textures.BigGameItems[Item.GetType()],
                             new Rectangle(Area.X + 1, Area.Y + 1, Textures.BigGameItems[Item.GetType()].Width,
                                           Textures.BigGameItems[Item.GetType()].Height), null, Color.White, 0f,
                             Vector2.Zero,
                             SpriteEffects.None, Depth);
            spriteBatch.Draw(Textures.GameUI["bigItem"], Area, null, Color.White, 0f, Vector2.Zero, SpriteEffects.None,
                             Depth - Data.GameDisplaying.Epsilon);
            BigLife.Draw(spriteBatch);
            _currentLife.Draw(spriteBatch);
            _maxLife.Draw(spriteBatch);
        }
    }
}
