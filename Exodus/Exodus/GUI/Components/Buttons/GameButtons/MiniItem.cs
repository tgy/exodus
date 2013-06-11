﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Exodus.PlayGame;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Exodus.GUI.Components.Buttons.GameButtons
{
    internal class MiniItem : Component
    {
        public Texture2D Texture;

        private MiniLife _miniLife;

        private int _id;
        public int Id
        {
            get { return _id; }
            set
            {
                Texture = Textures.MiniGameItems[Map.ListSelectedItems[value].GetType()];
                _id = value;
            }
        }

        public MiniItem(int x, int y, float depth, int id)
        {
            Area = new Rectangle(x, y, Textures.GameUI["smallItem"].Width, Textures.GameUI["smallItem"].Height);
            Depth = depth;
            Id = id;
            _miniLife = new MiniLife(x + 2, y + 24, Depth - 2*Data.GameDisplaying.Epsilon);
        }

        public override void Update(GameTime gameTime)
        {
            _miniLife.Value = 100*Map.ListSelectedItems[Id].currentLife/Map.ListSelectedItems[Id].maxLife;

            if (Focused && Inputs.LeftClick())
            {
                Map.ListSelectedItems = new List<PlayGame.Item> {Map.ListSelectedItems[Id]};
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (Texture != null)
                spriteBatch.Draw(Texture, Area, null, Color.White, 0f, Vector2.Zero, SpriteEffects.None,
                                 Depth);
            spriteBatch.Draw(Textures.GameUI["smallItem"], Area, null,
                             Color.White, 0f, Vector2.Zero, SpriteEffects.None, Depth - Data.GameDisplaying.Epsilon);
            _miniLife.Draw(spriteBatch);
        }
    }
}