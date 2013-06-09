using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Exodus.GUI;
using Exodus.Network.ServerSide;

namespace Exodus.GameStates
{
    public class MenuState : GameState
    {
        Application game;
        public MenuState(Application g)
        {
            Items = new List<Item>();
            this.game = g;
        }

        public override void LoadContent()
        {
            this.Music = Audio.MenuMusic;
            this.Music.Play();
        }

        public MenuState(List<Item> items)
        {
            Items = items;
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            if (Inputs.KeyPress(Microsoft.Xna.Framework.Input.Keys.Escape))
            {
                SyncClient.Stop();
                game.Pop();
            }
            base.Update(gameTime);
        }
    }
}