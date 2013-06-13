using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Exodus.GUI.Components;
using Exodus.GUI.Components.Buttons.GameButtons;
using Exodus.PlayGame;
using Exodus.PlayGame.Items.Units;
using Exodus.PlayGame.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Exodus.GUI.Items
{
    internal class BuildingProduction : Item
    {
        private ProgressBar _progressBar;
        private JustTexture _justTexture;
        private Label _name;
        private Big _big;
        public Type BuildingType
        {
            get
            {
                return buildingType;
            }
            set
            {
                _justTexture.Texture = Textures.BigGameItems[buildingType];
                buildingType = value;
            }

        }
        Type buildingType;

        public BuildingProduction(int x, int y, Type buildingType)
        {
            IsVisible = false;
            Area = new Rectangle(x, y, Textures.GameUI["unitProduction"].Width, Textures.GameUI["unitProduction"].Height);
            Components = new List<Component>();
            Point pos = new Point(x + 97, y + 53);
            for (int i = 0; i < 5; i++)
            {
                Components.Add(new Mini(DoNothing, typeof(int), null, null, pos.X, pos.Y));
                pos.X += 4 + Textures.GameUI["smallItem"].Width;
            }
            _justTexture = new JustTexture(Textures.BigGameItems[buildingType], x + 3, y + 4,
                                           40 * Data.GameDisplaying.Epsilon);
            Components.Add(_justTexture);
            _progressBar = new ProgressBar(Textures.GameUI["creationProgressBar"],
                                           Textures.GameUI["creationProgressBarGradient"], x + 88, y + 25, 201,
                                           25 * Data.GameDisplaying.Epsilon);
            Components.Add(_progressBar);

            _big = new Big(x + 2, y + 3, 10 * Data.GameDisplaying.Epsilon, null);
            Components.Add(_big);
            _name = new Label(Fonts.Arial9, "", x + 74, y + 2);
            _name.Color = new Color(22, 127, 176);
            Components.Add(_name);
        }
        public override void Update(GameTime gameTime)
        {
            if (!IsVisible)
                return;
            PlayGame.Item c = Map.ListItems.Find(u => u.PrimaryId == Map.ListSelectedItems[0]);
            _progressBar.Progression = c.TasksList.Count > 0
                ? (c.TasksList[0] is ProductItem ? ((ProductItem)c.TasksList[0]).Progress : ((ChangeResource)c.TasksList[0]).Progress)
                                           : 0;
            _big.Item = c;
            base.Update(gameTime);
            for (int i = 0; i < 5; i++)
            {
                ((Mini)Components[i]).Texture = null;
                ((Mini)Components[i]).TextureHover = null;
            }
            for (int i = 0; i < c.TasksList.Count(s => s is ProductItem || s is ChangeResource); i++)
            {
                Type name = c.TasksList[i] is ProductItem ?
                    ((ProductItem)c.TasksList[i]).GetTypeOfProduct() :
                    ((ChangeResource)c.TasksList[i]).GetType();
                ((Mini)Components[i]).Texture = Textures.MiniGameItems[name];
                ((Mini)Components[i]).TextureHover = Textures.MiniGameItems[name];
            }
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Textures.GameUI["unitProduction"], Area, null,
                             Color.White, 0f, Vector2.Zero, SpriteEffects.None, 30 * Data.GameDisplaying.Epsilon);
            base.Draw(spriteBatch);
        }
        public void Set(Type BuildingType)
        {
            if (buildingType != BuildingType)
            {
                this.buildingType = BuildingType;
                _justTexture.Texture = Textures.BigGameItems[buildingType];
            }
            _name.Txt = Map.ListItems.Find(u => u.PrimaryId == Map.ListSelectedItems[0]).Name + "(" + Map.ListSelectedItems[0] + ")";
        }
        void DoNothing(Type t) { }
    }
}


