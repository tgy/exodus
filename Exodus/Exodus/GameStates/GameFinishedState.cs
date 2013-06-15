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

namespace Exodus.GameStates
{
    public enum endGameState
    {
        won,
        lost
    }
    class GameFinishedState : GameState
    {
        Application game;
        GameState previous;
        public GameFinishedState(Application g, GameState previous, endGameState state)
        {
            game = g;
            this.previous = previous;
            SpriteFont font = GUI.Fonts.Eurostile18;
            Items.Add(new GUI.Items.BackgroundFull(Textures.Menu["loadingBackground"], 2 * float.Epsilon));
            Items.Add(new GUI.Items.Background(Textures.Menu["EndGame" + (state == endGameState.won ? "Won" : "Lost")], float.Epsilon));
        }
        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            if (Inputs.KeyboardState.IsKeyDown(Keys.Enter))
            {
                while (!(game.Peek() is MenuState))
                    game.Pop();
                if (!Data.Network.SinglePlayer)
                {
                    game.Pop();
                    game.Pop();
                }
            }
        }
        public override void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch)
        {
            previous.Draw(spriteBatch);
            base.Draw(spriteBatch);
        }
    }
}