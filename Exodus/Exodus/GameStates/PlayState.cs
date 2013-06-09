﻿using System;
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
        List<Item> _listExamples = new List<Item>();
        Item _currentExample = null;
        TasksDisplayer tasksDisplayer;
        BuildingProduction buildingProduction;
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

            t = Textures.GameUI["launchMenuButton"];
            Items.Add(new Container(
                new List<GUI.Component>
                {
                    new GUI.Components.Buttons.GameButtons.StandardButton(Menu,t,t,(Data.Window.WindowWidth - t.Width)/2,0)
                }
            ));
            #endregion

            Tile.tileSetTexture = Textures.Game["tileSet"];
            Map.Load(50, 50);
            _listExamples.Add(new PlayGame.Items.Buildings.Habitation(Data.Network.IdPlayer));
            _listExamples.Add(new PlayGame.Items.Buildings.Labo(Data.Network.IdPlayer));
            PlayGame.Items.Units.Worker w = new PlayGame.Items.Units.Worker(Data.Network.IdPlayer);
            w.SetPos(25, 25, true);
            Map.AddItem(w);
            foreach (Item it in _listExamples)
                it.Alpha = 0.6f;
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
                    spriteBatch.Draw(Tile.tileSetTexture,
                                     v,
                                     Tile.GetSourceRectangle(Map.MapCells[x, y].TileId),
                                     (Data.GameDisplaying.DisplayObstacle && Map.ObstacleMap[x, y]
                                          ? Color.Lime
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
                        Vector2 mousPos = Map.ScreenToMap(Inputs.MouseState.X+Camera.x,Inputs.MouseState.Y+Camera.y);
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
            for (int i = 0; i < Map.ListItems.Count; i++)
                Map.ListItems[i].Update(gameTime);
            if (Data.Window.GameFocus)
            {
                Camera.Update(gameTime);
                if (!Items.Exists(s => s.Focused))
                {
                    Vector2 mousePos;
                    // Suivant l'étate de jeu, on va faire telle ou telle action en fonction des interations du joueur
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
                                if (_currentExample == null)
                                {
                                    _currentExample = _listExamples.Find(s => s.GetType() == Data.GameInfos.type);
                                    if (_currentExample == null)
                                    {
                                        Data.GameInfos.currentMode = Data.GameInfos.ModeGame.Normal;
                                        break;
                                    }
                                }
                                mousePos = Map.ScreenToMap(Inputs.MouseState.X + Camera.x, Inputs.MouseState.Y + Camera.y);
                                _currentExample.Update(gameTime);
                                if (Inputs.LeftClick())
                                {
                                    if (Inputs.KeyboardState.IsKeyUp(Keys.LeftShift) && Inputs.KeyboardState.IsKeyUp(Keys.RightShift))
                                        Data.GameInfos.currentMode = Data.GameInfos.ModeGame.Normal;
                                    Item newItem = PlayGame.Items.Loader.LoadBuilding(Data.GameInfos.type, Data.Network.IdPlayer);

                                    if (newItem != null && Data.GameInfos.item != null)
                                    {
                                        if (Data.Network.SinglePlayer)
                                        {
                                            Data.GameInfos.item.AddTask(
                                                new PlayGame.Tasks.ProductItem(
                                                    Data.GameInfos.item,
                                                    Data.GameInfos.timeCreatingItem[newItem.GetType().ToString()],
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
                                        Map.AddItemsToSelection(new List<Item> { current });
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
                                if (Data.Network.SinglePlayer)
                                {
                                    for (i = 0; i < Map.ListSelectedItems.Count; i++)
                                        if (Map.ListSelectedItems[i] is Unit)
                                            Map.ListSelectedItems[i].AddTask(new PlayGame.Tasks.Move(Map.ListSelectedItems[i], p), b, false);
                                }
                                else
                                {
                                    for (i = 0; i < Map.ListSelectedItems.Count; i++)
                                        if (Map.ListSelectedItems[i] is Unit)
                                            Network.ClientSide.Client.SendObject(
                                                new Network.Orders.Tasks.Move(Map.ListSelectedItems[i].PrimaryId, b, p.X, p.Y)
                                            );
                                }
                                #region Attack
                                TasksDisplayer.Attack(default(Type));
                                #endregion
                            }
                            #endregion
                            break;
                    }
                }
            }
            #region user-interface

            tasksDisplayer.IsVisible = Map.ListSelectedItems.Count > 0 && (Map.ListSelectedItems[0] is Unit || Map.ListSelectedItems[0] is Building);

            buildingProduction.IsVisible = Map.ListSelectedItems.Count == 1 && Map.ListSelectedItems[0] is Building;
            if (buildingProduction.IsVisible)
                buildingProduction.Set(Map.ListSelectedItems[0].GetType());

            selectionDisplayer.IsVisible = Map.ListSelectedItems.Count > 0 &&
                                           !(Map.ListSelectedItems[0] is Building && Map.ListSelectedItems.Count == 1);

            #endregion
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
