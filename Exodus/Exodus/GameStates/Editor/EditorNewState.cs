using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Exodus.GUI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.Threading;

namespace Exodus.GameStates
{
    public class EditorNewState : GameState
    {
        private Application game;
        private GameState previous;

        private GUI.Items.ScrollingSelection selectionBox;

        public EditorNewState(Application g, GameState previous)
        {
            this.previous = previous;
            this.game = g;
        }
        // two first int : size of the Texture2D[,] which stores the multiple tilesets of the same map
        List<Tuple<int, int, int, int, string, string, string>> _listMaps = new List<Tuple<int, int, int, int, string, string, string>>
        {
            new Tuple<int, int, int,int,string,string,string>(4, 4, 50,50,"Desolate","09/06/13","tileSet"),
            new Tuple<int, int, int,int,string,string,string>(4, 4, 40,40,"Pixel World","09/06/13","tileSet"),
        };

        public override void Initialize()
        {

        }

        public override void LoadContent()
        {
            Texture2D t = Textures.Menu["ScrollingSelection"]; GUI.Items.Passive fenster = new GUI.Items.Passive(t, (Data.Window.WindowWidth - t.Width) / 2, (Data.Window.WindowHeight - t.Height) / 2, 5 * float.Epsilon);
            Items.Add(fenster);
            selectionBox = new GUI.Items.ScrollingSelection((Data.Window.WindowWidth - t.Width) / 2, (Data.Window.WindowHeight - t.Height) / 2, new Tuple<string, string, string>("TAILLE", "NOM", "DATE"), new List<int> { 10, 164, 590 });
            Items.Add(selectionBox);
            List<Tuple<string, string, string>> l = new List<Tuple<string, string, string>>();
            foreach (Tuple<int, int, int, int, string, string, string> tu in _listMaps)
                l.Add(new Tuple<string, string, string>(tu.Item3 + "x" + tu.Item4, tu.Item5, tu.Item6));
            selectionBox.Reset(l);
            GUI.Items.MenuHorizontal menu = new GUI.Items.MenuHorizontal(Data.Window.ScreenCenter.X + 170, Data.Window.ScreenCenter.Y + 112, 0);
            GUI.Components.Buttons.MenuButtons.OrangeMenuButton b = new GUI.Components.Buttons.MenuButtons.OrangeMenuButton("LOAD THE MAP");
            b.DoClick = New;
            menu.Create(new List<GUI.Component> { b });
            Items.Add(menu);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (Data.Window.GameFocus)
            {
                if (!Items.Exists(delegate(GUI.Item i) { return i.Focused; }))
                {
                    if (Inputs.LeftClick())
                        game.Pop();
                }
                else
                {
                    if (Inputs.KeyPress(Keys.Enter))
                        New();
                }
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            previous.Draw(spriteBatch);
            base.Draw(spriteBatch);
        }

        private void New(MenuState m, int i)
        {
            New();
        }

        private void New()
        {
            if (selectionBox.SelectedItem >= 0)
                (new Thread(ThreadingFunction)).Start();
        }

        private void ThreadingFunction()
        {
            GameState g = new GameStates.LoadingState(game, this, "Creation en cours...");
            g.Initialize();
            g.LoadContent();
            game.Push(g);
            try
            {
                Tuple<int, int, int, int, string, string, string> m = _listMaps[selectionBox.SelectedItem];
                PlayGame.Tile.tileSetTextures = new Texture2D[m.Item1, m.Item2];
                for (int i = 0; i < m.Item1; i++)
                    for (int j = 0; j < m.Item2; j++)
                        PlayGame.Tile.tileSetTextures[i, j] = Textures.Game[m.Item5 + "-" + i + "-" + j];
                PlayGame.Map.Load(_listMaps[selectionBox.SelectedItem].Item1, _listMaps[selectionBox.SelectedItem].Item2);
            }
            catch
            {
            }
            game.Pop();
            game.Pop();
        }

    }
}