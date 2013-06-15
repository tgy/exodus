using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Exodus.GUI.Components;
using Exodus.GUI.Items;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.Threading;
using Exodus.PlayGame;
using Exodus.Network;

namespace Exodus.GameStates
{
    internal class PlayState : GameState
    {
        private GUI.Items.SelectionSquare _selectionSquare;
        Application game;
        Item _currentExample = null;
        TasksDisplayer tasksDisplayer;
        BuildingProduction buildingProduction;
        ResourcesDisplayer resourcesDisplayer;
        private SelectionDisplayer selectionDisplayer;
        public PlayState(Application g)
        {
            this.game = g;
        }
        public override void Initialize()
        {

        }
        public override void LoadContent()
        {
            this.Music = Audio.PlayStateMusic;
            this.Music.Play();
            Texture2D t;
            _selectionSquare = new GUI.Items.SelectionSquare("selectionSquare1", "selectionSquare2");
            Network.ClientSide.Client.chat = new GUI.Items.Chat(5, 5);
            Items.Add(Network.ClientSide.Client.chat);
            Map.MouseMap = Textures.Game["mouseMap"];
            Items.Add(_selectionSquare);

            #region user-interface

            Items.Add(new Patterned(Textures.GameUI["gradient"], 0,
                                    Data.Window.WindowHeight - Textures.GameUI["gradient"].Height));

            Items.Add(new Patterned(Textures.GameUI["topPattern"], 0, 0));

            tasksDisplayer = new TasksDisplayer(306, Data.Window.WindowHeight - Textures.GameUI["actions"].Height);
            Items.Add(tasksDisplayer);

            buildingProduction = new BuildingProduction(7, Data.Window.WindowHeight - Textures.GameUI["unitProduction"].Height - 11, typeof(PlayGame.Items.Buildings.Habitation));
            Items.Add(buildingProduction);

            selectionDisplayer = new SelectionDisplayer(0, Data.Window.WindowHeight - Textures.GameUI["selection"].Height, 25 * Data.GameDisplaying.Epsilon);
            Items.Add(selectionDisplayer);

            Minimap minimap = new Minimap(Data.Window.WindowWidth - 284, Data.Window.WindowHeight - 170, 25 * Data.GameDisplaying.Epsilon);
            Items.Add(minimap);

            t = Textures.GameUI["launchMenuButton"];
            Items.Add(new Container(
                new List<GUI.Component>
                {
                    new GUI.Components.Buttons.GameButtons.StandardButton(Menu,t,t,(Data.Window.WindowWidth - t.Width)/2,0)
                }
            ));
            resourcesDisplayer = new ResourcesDisplayer();
            Items.Add(resourcesDisplayer);
            #endregion

            Tile.tileSetTextures = new Texture2D[4, 2];
            Tile.tileSetTextures[0, 0] = Textures.Game["tileSet-0-0"];
            Tile.tileSetTextures[1, 0] = Textures.Game["tileSet-0-1"];
            Tile.tileSetTextures[2, 0] = Textures.Game["tileSet-0-2"];
            Tile.tileSetTextures[3, 0] = Textures.Game["tileSet-0-3"];
            Tile.tileSetTextures[0, 1] = Textures.Game["tileSet-1-0"];
            Tile.tileSetTextures[1, 1] = Textures.Game["tileSet-1-1"];
            Tile.tileSetTextures[2, 1] = Textures.Game["tileSet-1-2"];
            Tile.tileSetTextures[3, 1] = Textures.Game["tileSet-1-3"];
            Map.Load(200, 200);
            Map.ClearSelection();
            Tile.tileSetWidth = (Tile.tileSetTextures[0, 0].Width * (Tile.tileSetTextures.GetLength(0) - 1) + Tile.tileSetTextures[Tile.tileSetTextures.GetLength(0) - 1, 0].Width) / Tile.tileWidth;
            if (!Data.Network.SinglePlayer)
            {
                if (Network.ServerSide.Server.IsRunning)
                {
                    Network.ClientSide.Client.SendObject(new Network.Orders.Tasks.CheatSpawn(0, true, typeof(PlayGame.Items.Buildings.Habitation), 1, 100, 10));
                    Network.ClientSide.Client.SendObject(new Network.Orders.Tasks.CheatSpawn(0, true, typeof(PlayGame.Items.Buildings.Habitation), 2, 100, 20));
                }
            }
            else
            {
                PlayGame.Items.Obstacles.Iron iron = new PlayGame.Items.Obstacles.Iron();
                iron.SetPos(110, 40, true);
                Map.ListPassiveItems.Add(iron);

                PlayGame.Items.Buildings.Habitation h = new PlayGame.Items.Buildings.Habitation(2);
                
                PlayGame.Items.Units.Worker w = new PlayGame.Items.Units.Worker(2);
                w.SetPos(100, 10, true);
                Map.AddItem(w);
                w.AddTask(new PlayGame.Tasks.ProductItem(w, 0, h, new Point(100, 40), true, true, true), false, false);
                
                
                w = new PlayGame.Items.Units.Worker(1);
                w.SetPos(100, 20, true);
                Map.AddItem(w);
                
                PlayGame.Items.Obstacles.Gas g = new PlayGame.Items.Obstacles.Gas();
                g.SetPos(110, 50, true);
                Map.ListPassiveItems.Add(g);

            }
            Map.EarningPerSec = new Resource(0, 0, 0, 0, 0);
            Map.PlayerResources = new Resource(10000, 10000, 10000, 10000, 10000);
            base.LoadContent();
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            #region Selection de la zone à afficher
            int xMin = 0, xMax = 0, yMin = 0, yMax = 0;
            GetSubArea(ref xMin, ref xMax, ref yMin, ref yMax);
            #endregion
            #region Affichage de l'ensemble des tiles et obstacles
            Vector2 v;
            for (int x = xMin; x < xMax; x++)
                for (int y = yMin; y < yMax; y++)
                {
                    v = Map.MapToScreen(x, y);
                    v.X -= Camera.x;
                    v.Y -= Camera.y;
                    Tuple<Texture2D, Rectangle> tile = Tile.GetSourceRectangle(Map.MapCells[x, y].TileId);
                    spriteBatch.Draw(tile.Item1,
                                     v,
                                     tile.Item2,
                                     (Data.GameDisplaying.DisplayObstacle && Map.MapCells[x, y].ListItems.Count > 0
                                          ? Data.GameDisplaying.DisplayingColor
                                          : Color.White),
                                     0f, Vector2.Zero, 1f, SpriteEffects.None, 1f
                        );
                    for (int i = 0; i < Map.MapCells[x, y].ListItems.Count; i++)
                        Map.MapCells[x, y].ListItems[i].Draw(spriteBatch);
                }

            #endregion
            spriteBatch.DrawString(GUI.Fonts.Eurostile12Bold, Data.Network.Running_as + " (Id:" + Data.Network.IdPlayer + ")",
                                   new Vector2(Data.Window.WindowWidth - 420, 15), Color.Red, 0f, Vector2.Zero, 1f,
                                   SpriteEffects.None, float.Epsilon);
            spriteBatch.DrawString(GUI.Fonts.Eurostile12Bold, Data.Network.Client,
                                   new Vector2(Data.Window.WindowWidth - 370, 34), Color.Red, 0f, Vector2.Zero, 1f,
                                   SpriteEffects.None, float.Epsilon);
            spriteBatch.DrawString(GUI.Fonts.Eurostile12, Data.Network.ServerIP,
                                   new Vector2(Data.Window.WindowWidth - 345, 49), Color.Red, 0f, Vector2.Zero, 1f,
                                   SpriteEffects.None, float.Epsilon);
            spriteBatch.DrawString(GUI.Fonts.Eurostile12Bold, Data.Network.Server,
                                   new Vector2(Data.Window.WindowWidth - 370, 69), Color.Red, 0f, Vector2.Zero, 1f,
                                   SpriteEffects.None, float.Epsilon);
            spriteBatch.DrawString(GUI.Fonts.Eurostile12Bold, Data.Network.Error,
                                   new Vector2(Data.Window.WindowWidth / 2 - 42, Data.Window.WindowHeight / 2), Color.Red,
                                   0f, Vector2.Zero, 1f, SpriteEffects.None, float.Epsilon);
            resourcesDisplayer.Set(PlayGame.Map.PlayerResources);
            int Client_Count = 1;
            while (Client_Count <= Data.Network.ConnectedClients.Count)
            {
                spriteBatch.DrawString(GUI.Fonts.Eurostile12, Data.Network.ConnectedClients[Client_Count - 1].IP.Split(':')[0],
                                   new Vector2(Data.Window.WindowWidth - 245, 15 * Client_Count + 69), Color.Red, 0f, Vector2.Zero, 1f,
                                   SpriteEffects.None, float.Epsilon);
                Client_Count++;
            }
            // Selon notre mode de jeu, on va dessiner des trucs en plus, ou non :)
            switch (Data.GameInfos.currentMode)
            {
                case Data.GameInfos.ModeGame.Attack:
                    GameMouse.Active = false;
                    break;
                case Data.GameInfos.ModeGame.Building:
                    GameMouse.Active = false;
                    if (_currentExample != null)
                    {
                        Vector2 mousPos = Map.ScreenToMap(Inputs.MouseState.X + Camera.x, Inputs.MouseState.Y + Camera.y);
                        _currentExample.SetPos((int)mousPos.X, (int)mousPos.Y, false);
                        _currentExample.Draw(spriteBatch);
                    }
                    break;
                case Data.GameInfos.ModeGame.Patrol:
                    GameMouse.Active = false;
                    break;
                default:
                    GameMouse.Active = true;
                    break;
            }
            _selectionSquare.Active = Data.GameInfos.currentMode == Data.GameInfos.ModeGame.Normal;
            base.Draw(spriteBatch);
        }
        public override void Update(GameTime gameTime)
        {
            Map.EarningPerSec.Reset();
            for (int i = 0; i < Map.ListItems.Count; i++)
            {
                if (Map.ListItems[i].IdPlayer == Data.Network.IdPlayer)
                {
                    if (!(Map.ListItems[i] is PlayGame.Items.Buildings.HydrogenExtractor && Map.ListItems[i].currentResource.Hydrogen <= 0))
                    {
                        Map.EarningPerSec += Map.ListItems[i].resourcesGeneration;
                        Map.ListItems[i].currentResource -= (gameTime.ElapsedGameTime.TotalMilliseconds / 1000) * Map.ListItems[i].resourcesGeneration;
                    }
                }
                Map.ListItems[i].Update(gameTime);
            }
            Map.PlayerResources += (gameTime.ElapsedGameTime.TotalMilliseconds / 1000) * Map.EarningPerSec;
            for (int i = 0; i < Map.ListPassiveItems.Count; i++)
                Map.ListPassiveItems[i].Update(gameTime);
            if (Data.GameInfos.currentMode == Data.GameInfos.ModeGame.Building
                && (_currentExample == null || _currentExample.GetType() != Data.GameInfos.type))
            {
                _currentExample = PlayGame.Items.Loader.LoadBuilding(Data.GameInfos.type, Data.Network.IdPlayer);
                _currentExample.Alpha = 0.6f;
                if (_currentExample == null)
                    Data.GameInfos.currentMode = Data.GameInfos.ModeGame.Normal;
            }
            if (Data.Window.GameFocus)
            {
                Camera.Update(gameTime);
                if (!Items.Exists(s => s.Focused))
                {
                    Vector2 mousePos;
                    // Suivant l'état de jeu, on va faire telle ou telle action en fonction des interations du joueur
                    switch (Data.GameInfos.currentMode)
                    {
                        // Si on attend un clic de la part du joueur pour qu'il assigne une attaque à un item :)
                        case Data.GameInfos.ModeGame.Attack:
                            #region Attack
                            if (Inputs.KeyPress(Keys.Escape))
                                Data.GameInfos.currentMode = Data.GameInfos.ModeGame.Normal;
                            else
                            {
                            }
                            break;
                            #endregion
                        // Si on attend un clic de la part du joueur pour qu'il assigne un endroit où poser un batiment à un item :)
                        case Data.GameInfos.ModeGame.Building:
                            #region Building
                            if (Inputs.KeyPress(Keys.Escape))
                                Data.GameInfos.currentMode = Data.GameInfos.ModeGame.Normal;
                            else
                            {
                                mousePos = Map.ScreenToMap(Inputs.MouseState.X + Camera.x, Inputs.MouseState.Y + Camera.y);
                                _currentExample.Update(gameTime);
                                if (Inputs.LeftClick())
                                {
                                    if (Inputs.KeyboardState.IsKeyUp(Keys.LeftShift) && Inputs.KeyboardState.IsKeyUp(Keys.RightShift))
                                        Data.GameInfos.currentMode = Data.GameInfos.ModeGame.Normal;
                                    Item newItem = PlayGame.Items.Loader.LoadBuilding(Data.GameInfos.type, Data.Network.IdPlayer);

                                    if (newItem != null && Data.GameInfos.item != null)
                                    {
                                        // on ne fait rien si le batiment est un extracteur d'hydrogene et qu'on essaye de le poser autre part que sur un puit de gaz
                                        if (!(newItem is PlayGame.Items.Buildings.HydrogenExtractor && Map.MapCells[(int)mousePos.X, (int)mousePos.Y].ListItems.FirstOrDefault(x => x is PlayGame.Items.Obstacles.Gas) == null))
                                        {
                                            if (!(Map.PlayerResources >= Data.GameInfos.CostsItems[newItem.GetType()]))
                                            {
                                                Network.ClientSide.Client.chat.InsertMsg("Not enough resources: you are missing" + (PlayGame.Map.PlayerResources - Data.GameInfos.CostsItems[newItem.GetType()]).ToString());
                                            }
                                            else
                                            {
                                                Map.PlayerResources -= Data.GameInfos.CostsItems[newItem.GetType()];
                                                if (Data.Network.SinglePlayer)
                                                {
                                                    Data.GameInfos.item.AddTask(
                                                        new PlayGame.Tasks.ProductItem(
                                                            Data.GameInfos.item,
                                                            Data.GameInfos.timeCreatingItem[newItem.GetType()],
                                                            newItem,
                                                            new Point((int)mousePos.X, (int)mousePos.Y),
                                                            true,
                                                            false,
                                                            true
                                                        ),
                                                        !Inputs.KeyboardState.IsKeyDown(Keys.LeftShift) && !Inputs.KeyboardState.IsKeyDown(Keys.RightShift),
                                                        false
                                                    );
                                                }
                                                else
                                                {
                                                    Network.ClientSide.Client.SendObject(
                                                        new Network.Orders.Tasks.ProductItem(
                                                            Data.GameInfos.item.PrimaryId,
                                                            false,
                                                            _currentExample.GetType(),
                                                            (int)mousePos.X,
                                                            (int)mousePos.Y,
                                                            true,
                                                            false,
                                                            true
                                                        )
                                                    );
                                                    //throw new Exception("GO IMPLEMENTER CA EN MULTIPLAYER, SPECE DE GLANDU");
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            break;
                            #endregion
                        // Si on attend un clic de la part du joueur pour qu'il assigne un endroit pour le patrouillage des items :)
                        case Data.GameInfos.ModeGame.Patrol:
                            #region Patrol
                            if (Inputs.KeyPress(Keys.Escape))
                                Data.GameInfos.currentMode = Data.GameInfos.ModeGame.Normal;
                            else
                            {
                            }
                            break;
                            #endregion
                        // case ModeGame.Normal
                        default:
                            #region touche suppr
                            // touche suppr => ajouter la tache Die
                            if (Inputs.KeyboardState.IsKeyDown(Keys.Delete))
                            {
                                TasksDisplayer.Die(default(Type));
                            }
                            #endregion
                            #region Selection des items
                            // Si on a double cliqué
                            if (Inputs.DoubleLeftClick())
                            {
                                #region Double Click
                                // On va checker les unités dans notre zone
                                int xMin = 0, yMin = 0, xMax = 0, yMax = 0;
                                // On définit la zone à checker :>
                                GetSubArea(ref xMin, ref xMax, ref yMin, ref yMax);

                                // Si on n'a pas les touches maj enfoncées, on vide :)
                                #region Vidage

                                if (Inputs.KeyboardState.IsKeyUp(Keys.LeftShift) &&
                                    Inputs.KeyboardState.IsKeyUp(Keys.RightShift))
                                    Map.ClearSelection();

                                #endregion

                                // On va regarder si un item est focus n.n
                                Item current = null;
                                foreach (Item i in Map.ListItems)
                                    if (i.IdPlayer == Data.Network.IdPlayer && i.Intersect(Inputs.MouseState.X, Inputs.MouseState.Y) &&
                                        (current == null || current.layerDepth > i.layerDepth))
                                        current = i;
                                // Si on a un item focused :)
                                if (current != null)
                                {
                                    Type currentType = current.GetType();
                                    Rectangle window = new Rectangle(0, 0, Data.Window.WindowWidth, Data.Window.WindowHeight);
                                    List<Item> toAdd = new List<Item>();
                                    for (int x = xMin; x < xMax; x++)
                                        for (int y = yMin; y < yMax; y++)
                                            for (int i = 0; i < Map.MapCells[x, y].ListItems.Count; i++)
                                                if (Map.MapCells[x, y].ListItems[i].IdPlayer == Data.Network.IdPlayer
                                                    && Map.MapCells[x, y].ListItems[i].GetType() == currentType
                                                    && Map.MapCells[x, y].ListItems[i].Intersect(window))
                                                    toAdd.Add(Map.MapCells[x, y].ListItems[i]);
                                    Map.AddItemsToSelection(toAdd);
                                }
                                #endregion
                            }
                            // Si on effectue un rectangle de sélection, on va chercher à sélectionner le(s) item(s) voulu(s) !
                            else if (_selectionSquare.Defined)
                            {
                                // Si on n'a pas les touches maj enfoncées, on vide :)
                                #region Vidage

                                if (Inputs.KeyboardState.IsKeyUp(Keys.LeftShift) &&
                                    Inputs.KeyboardState.IsKeyUp(Keys.RightShift))
                                    Map.ClearSelection();

                                #endregion
                                // S'il est dessiné = si on effectue un seul clic ou non
                                #region Multisélection

                                if (_selectionSquare.Drawn)
                                {
                                    List<Item> toAdd = new List<Item>();
                                    // Si on sélectionne des Units, il faut que ce ne soit que des units qui soient connectées
                                    foreach (Item i in Map.ListItems)
                                        if (i.IdPlayer == Data.Network.IdPlayer
                                            && i.Intersect(_selectionSquare.Area))
                                            toAdd.Add(i);
                                    Map.AddItemsToSelection(toAdd);
                                }
                                #endregion
                                // Un seul clic, on va chercher à sélectionner un seul item
                                #region Monosélection
                                else
                                {
                                    Item current = null;
                                    foreach (Item i in Map.ListItems)
                                        if ((current == null || current.layerDepth > i.layerDepth)
                                            && i.IdPlayer == Data.Network.IdPlayer
                                            && i.Intersect(Inputs.MouseState.X, Inputs.MouseState.Y))
                                            current = i;
                                    if (current != null)
                                    {
                                        Map.AddItemsToSelection(new List<Item> { current });
                                        if (current.SelectionSound != null)
                                            current.SelectionSound.Play();
                                    }
                                }

                                #endregion
                                // On a changé les items, on reset le tout :>
                                if (tasksDisplayer.IsVisible)
                                    tasksDisplayer.Reset();
                            }
                            #endregion
                            #region Ordres de mouvement

                            if (Inputs.RightClick())
                            {
                                Vector2 mouseMap = Map.ScreenToMap(Inputs.MouseState.X + Camera.x,
                                                                   Inputs.MouseState.Y + Camera.y);
                                Point p = new Point((int)mouseMap.X, (int)mouseMap.Y);
                                bool b = Inputs.KeyboardState.IsKeyUp(Keys.LeftShift) && Inputs.KeyboardState.IsKeyUp(Keys.RightShift);
                                int i;
                                Item c;
                                if (Data.Network.SinglePlayer)
                                {
                                    for (i = 0; i < Map.ListSelectedItems.Count; i++)
                                    {
                                        c = Map.ListItems.Find(u => u.PrimaryId == Map.ListSelectedItems[i]);
                                        if (c is Unit)
                                            c.AddTask(new PlayGame.Tasks.Move(c, p), b, false);
                                    }
                                }
                                else
                                {
                                    for (i = 0; i < Map.ListSelectedItems.Count; i++)
                                        if (Map.ListItems.Find(u => u.PrimaryId == Map.ListSelectedItems[i]) is Unit)
                                            Network.ClientSide.Client.SendObject(
                                                new Network.Orders.Tasks.Move(Map.ListSelectedItems[i], b, p.X, p.Y)
                                            );
                                }
                                #region Attack
                                TasksDisplayer.Attack(default(Type));
                                #endregion
                                #region Harvest
                                TasksDisplayer.Harvest(default(Type));
                                #endregion
                            }
                            #endregion
                            break;
                    }
                }
            }
            #region user-interface
            Item item = Map.ListSelectedItems.Count > 0 ? Map.ListItems.Find(u => u.PrimaryId == Map.ListSelectedItems[0]) : null;

            tasksDisplayer.IsVisible = Map.ListSelectedItems.Count > 0 && (item is Unit || item is Building);

            buildingProduction.IsVisible = Map.ListSelectedItems.Count == 1 && item is Building;
            if (buildingProduction.IsVisible)
                buildingProduction.Set(item.GetType());

            selectionDisplayer.IsVisible = Map.ListSelectedItems.Count > 0 &&
                                           !(item is Building && Map.ListSelectedItems.Count == 1);

            #endregion
            if (Network.ClientSide.Client.MustReSync)
            {
                Network.Orders.Tasks.ReSync Orders = Network.ClientSide.Client.ReSyncOrder;
                Network.ClientSide.Client.MustReSync = false;


                PlayGame.Map.ListItems = Orders.listItems;
                PlayGame.Map.ListPassiveItems = Orders.listPassives;
                for (int x = 0; x < PlayGame.Map.Width; x++)
                    for (int y = 0; y < PlayGame.Map.Height; y++)
                        PlayGame.Map.MapCells[x, y].ListItems.Clear();
                for (int i = 0; i < Orders.listItems.Count; i++)
                    if (Orders.listItems[i].pos != null)
                        for (int y = Orders.listItems[i].pos.Value.Y, mY = y + Orders.listItems[i].Width; y < mY; y++)
                            for (int x = Orders.listItems[i].pos.Value.X, mX = x + Orders.listItems[i].Width; x < mX; x++)
                                PlayGame.Map.MapCells[x, y].ListItems.Add(Orders.listItems[i]);
                for (int i = 0; i < Orders.listPassives.Count; i++)
                    if (Orders.listPassives[i].pos != null)
                        for (int y = Orders.listItems[i].pos.Value.Y, mY = y + Orders.listItems[i].Width; y < mY; y++)
                            for (int x = Orders.listItems[i].pos.Value.X, mX = x + Orders.listItems[i].Width; x < mX; x++)
                                PlayGame.Map.MapCells[x, y].ListItems.Add(Orders.listPassives[i]);
            }
            base.Update(gameTime);
        }
        public override void UnLoad()
        {
            NetGame.Stop();
            base.UnLoad();
        }
        void Menu()
        {
            game.Pop();
        }
        void GetSubArea(ref int xMin, ref int xMax, ref int yMin, ref int yMax)
        {
            Vector2 v = Map.ScreenToMap(Camera.x, Camera.y);
            int nTilesHeight = Data.Window.WindowHeight / Tile.tileHeight,
                nTilesWidth = Data.Window.WindowWidth / Tile.tileWidth;
            xMin = Math.Max((int)v.X - nTilesHeight - Data.GameDisplaying.PaddingMap, 0);
            xMax = Math.Min((int)v.X + nTilesWidth + Data.GameDisplaying.PaddingMap, Map.Width);
            yMin = Math.Max((int)v.Y - Data.GameDisplaying.PaddingMap, 0);
            yMax = Math.Min(yMin + nTilesHeight + nTilesWidth + Data.GameDisplaying.PaddingMap, Map.Height);
        }
    }

}
