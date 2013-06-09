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
using System.Threading;
namespace Exodus.GameStates
{
    public class EditorLoadState : GameState
    {
        Application game;
        GameState previous;
        GUI.Items.ScrollingSelection selectionBox;
        bool _loading = false;
        public EditorLoadState(Application g, GameState previous)
        {
            this.previous = previous;
            this.game = g;
        }
        public override void Initialize()
        {
        }
        public override void LoadContent()
        {
            Texture2D t = Textures.Menu["ScrollingSelection"];
            GUI.Items.Passive fenster = new GUI.Items.Passive(t, (Data.Window.WindowWidth - t.Width) / 2, (Data.Window.WindowHeight - t.Height) / 2, 5 * float.Epsilon);
            Items.Add(fenster);
            selectionBox = new GUI.Items.ScrollingSelection((Data.Window.WindowWidth - t.Width) / 2, (Data.Window.WindowHeight - t.Height) / 2, new Tuple<string, string, string>("TAILLE", "NOM", "DATE"), new List<int> { 10, 164, 590 });
            Items.Add(selectionBox);
            //On Cherche les maps existantes
            List<string> listMaps = System.IO.Directory.EnumerateFileSystemEntries(Data.Config.PathMaps).ToList();
            List<Tuple<string, string, string>> l = new List<Tuple<string, string, string>>();
            for (int i = 0, c = listMaps.Count; i < c; i++)
            {
                l.Add(new Tuple<string, string, string>("", listMaps[i].Substring(Data.Config.PathMaps.Length, listMaps[i].Length - Data.Config.PathMaps.Length - Data.Config.MapsFileExtension.Length), ""));
            }
            selectionBox.Reset(l);
            //Fin
            GUI.Items.MenuHorizontal menu = new GUI.Items.MenuHorizontal(Data.Window.ScreenCenter.X + 170, Data.Window.ScreenCenter.Y + 112, 0);
            GUI.Components.Buttons.MenuButtons.OrangeMenuButton b = new GUI.Components.Buttons.MenuButtons.OrangeMenuButton("LOAD THE MAP");
            b.DoClick = Load;
            menu.Create(new List<GUI.Component> { b });
            Items.Add(menu);
        }
        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
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
                        Load();
                }
            }
        }
        public override void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch)
        {
            previous.Draw(spriteBatch);
            base.Draw(spriteBatch);
        }
        protected void Load(MenuState m, int i)
        {
            Load();
        }
        protected void Load()
        {
            if (!_loading)
                new Thread(ThreadingFunction).Start();
        }
        protected void ThreadingFunction()
        {
            _loading = true;
            DateTime d = DateTime.Now;
            GameState g = new GameStates.LoadingState(game, this, "Chargement en cours...");
            g.Initialize();
            g.LoadContent();
            game.Push(g);
            PlayGame.Map.Load(selectionBox.Entries[selectionBox.SelectedItem].Item2);
            double ms = d.Subtract(DateTime.Now).TotalMilliseconds;
            game.Pop();
            game.Pop();
            _loading = false;
        }
    }
}
