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
    public class EditorSaveState : GameState
    {
        Application game;
        GameState previous;
        GUI.Items.Passive fenster;
        GUI.Items.ScrollingSelection selectionBox;
        GUI.Items.MenuVertical menu;
        GUI.Components.TextBox textBox;
        public EditorSaveState(Application g, GameState previous)
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
            fenster = new GUI.Items.Passive(t, (Data.Window.WindowWidth - t.Width) / 2, (Data.Window.WindowHeight - t.Height) / 2, 6 * Data.GameDisplaying.Epsilon);
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
            selectionBox.selectionable = false;
            //Fin
            menu = new GUI.Items.MenuVertical(Data.Window.ScreenCenter.X + 170, Data.Window.ScreenCenter.Y + 112, 0);
            GUI.Components.Buttons.MenuButtons.OrangeMenuButton b = new GUI.Components.Buttons.MenuButtons.OrangeMenuButton("SAVE THE MAP");
            b.DoClick = Save;
            menu.Create(new List<GUI.Component> { b });
            Items.Add(menu);
            textBox = new GUI.Components.TextBox(Data.Window.ScreenCenter.X - 210, Data.Window.ScreenCenter.Y + 109, "", "textBoxTexture2", new GUI.Padding(7, 0), 50, Data.GameDisplaying.Epsilon * 5);
            textBox.SetPosition();
            GUI.Items.Container con = new GUI.Items.Container();
            GUI.Components.Label label = new GUI.Components.Label(GUI.Fonts.Eurostile12, "Name: ", Data.Window.ScreenCenter.X - 260, Data.Window.ScreenCenter.Y + 122);
            label.SetColor(156, 221, 255);
            textBox.SetColor(156, 221, 255);
            con.Components.Add(label);
            con.Components.Add(textBox);
            Items.Add(con);
        }
        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            base.Update(gameTime);
            if (Data.Window.GameFocus)
            {
                if (!Items.Exists(i => i.Focused))
                {
                    if (Inputs.MouseState.LeftButton == ButtonState.Pressed)
                        game.Pop();
                }
                else
                {
                    if (Inputs.KeyboardState.IsKeyDown(Keys.Enter))
                        Save();
                }
            }
        }
        public override void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch)
        {
            previous.Draw(spriteBatch);
            base.Draw(spriteBatch);

        }
        void Save(MenuState m, int i)
        {
            Save();
        }
        void Save()
        {
            (new Thread(ThreadFunction)).Start();
        }
        void ThreadFunction()
        {
            GameState g = new GameStates.LoadingState(game, this, "Sauvegarde en cours...");
            g.Initialize();
            g.LoadContent();
            game.Push(g);
            PlayGame.MapFile file = new PlayGame.MapFile();
            string fileName = textBox.Value;
            file.map = new int[PlayGame.Map.Width, PlayGame.Map.Height];
            for (int x = 0; x < PlayGame.Map.Width; x++)
                for (int y = 0; y < PlayGame.Map.Height; y++)
                    file.map[x, y] = PlayGame.Map.MapCells[x, y].TileId;
            file.author = Data.PlayerInfos.Name;
            file.xMax = PlayGame.Camera.maxX;
            file.xMin = PlayGame.Camera.minX;
            file.yMax = PlayGame.Camera.maxY;
            file.yMin = PlayGame.Camera.minY;
            if (previous is MapEditorState)
            {
                file.listItems = new List<PlayGame.Item>();
                file.listPassives = new List<PlayGame.Item>();
                foreach (PlayGame.Item i in PlayGame.Map.ListItems)
                    file.listItems.Add(i);
                foreach (PlayGame.Item i in PlayGame.Map.ListPassiveItems)
                    file.listPassives.Add(i);
            }
            Serialize.Serializer.Serialize(file, Data.Config.PathMaps + fileName + Data.Config.MapsFileExtension);
            game.Pop();
            game.Pop();
        }
    }
}
