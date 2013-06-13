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
using Exodus.PlayGame;

namespace Exodus.GameStates
{
    public class MapEditorState : GameState
    {
        #region Variables
        Application game;
        GUI.Items.ScrollingListButtons listButtons;
        GUI.Components.MultiButton chooseTypeButton, eraserBrusherButton;
        GUI.Items.Passive fenster;
        int _listButtonsXBase,
            _chooseTypeButtonXBase,
            _eraserBrusherButtonXBase,
            _fensterXBase;
        List<Texture2D> _obstacleList = new List<Texture2D>();
        List<Type> _obstacleTypesList = new List<Type>
        {
            typeof(PlayGame.Items.Obstacles.Creeper),
            typeof(PlayGame.Items.Obstacles.Nothing1x1),
            typeof(PlayGame.Items.Obstacles.Nothing2x2)
        };
        List<Texture2D> _gameItemList = new List<Texture2D>();
        List<Type> _gameItemsTypeList = new List<Type>
        {
            typeof(PlayGame.Items.Units.Gunner),
            typeof(PlayGame.Items.Units.Worker),
            typeof(PlayGame.Items.Units.Spider),
            typeof(PlayGame.Items.Buildings.Habitation),
            typeof(PlayGame.Items.Buildings.Labo)
        };
        List<Texture2D> _tilesList = new List<Texture2D>();
        enum itemType
        {
            obstacle,
            item,
            tile
        }
        enum modeType
        {
            brush,
            erase
        }
        itemType _currentItemType;
        Item _currentItem = null;
        modeType _currentMode;
        int _currentItemId;
        bool _noItemSelect = false;
        float _alpha = 0.4f;
        Texture2D _selectedCell;
        int _xClose,
            _xOpen,
            _speed;
        #endregion
        public MapEditorState(Application g)
        {
            game = g;
        }
        public override void Initialize()
        {
            base.Initialize();
        }
        public override void LoadContent()
        {
            Textures.LoadGameItems(game.Content);
            Textures.LoadMiniGameItems(game.Content);
            Textures.LoadGame(game.Content);
            Tile.tileSetTextures = new Texture2D[4, 2];
            Tile.tileSetTextures[0, 0] = Textures.Game["tileSet-0-0"];
            Tile.tileSetTextures[1, 0] = Textures.Game["tileSet-0-1"];
            Tile.tileSetTextures[2, 0] = Textures.Game["tileSet-0-2"];
            Tile.tileSetTextures[3, 0] = Textures.Game["tileSet-0-3"];
            Tile.tileSetTextures[0, 1] = Textures.Game["tileSet-1-0"];
            Tile.tileSetTextures[1, 1] = Textures.Game["tileSet-1-1"];
            Tile.tileSetTextures[2, 1] = Textures.Game["tileSet-1-2"];
            Tile.tileSetTextures[3, 1] = Textures.Game["tileSet-1-3"];
            Tile.tileSetWidth = (Tile.tileSetTextures[0, 0].Width * (Tile.tileSetTextures.GetLength(0) - 1) + Tile.tileSetTextures[Tile.tileSetTextures.GetLength(0) - 1, 0].Width) / Tile.tileWidth;
            
            Map.MouseMap = Textures.Game["mouseMap"];
            #region LoadMap
            PlayGame.Map.Load(200, 200);
            #endregion
            _selectedCell = Textures.Game["selectCase"];
            _xClose = -268;
            _xOpen = -5;
            _speed = 9;
            #region Chargement Interface
            List<GUI.Component> l = new List<GUI.Component>();
            GUI.Components.MenuButton c;
            GUI.Items.MenuHorizontal menuTop = new GUI.Items.MenuHorizontal(5, 5, 10);
            c = new GUI.Components.Buttons.MenuButtons.OrangeMenuButton("Nouveau");
            c.DoClick = New;
            l.Add(c);
            c = new GUI.Components.Buttons.MenuButtons.OrangeMenuButton("Charger");
            c.DoClick = Load;
            l.Add(c);
            c = new GUI.Components.Buttons.MenuButtons.OrangeMenuButton("Sauver");
            c.DoClick = Save;
            l.Add(c);
            c = new GUI.Components.Buttons.MenuButtons.OrangeMenuButton("Quitter");
            c.DoClick = Exit;
            l.Add(c);
            menuTop.Create(l);
            _fensterXBase = 5;
            fenster = new GUI.Items.Passive(Textures.Menu["fenster2"], _fensterXBase + _xClose, 45, float.Epsilon * 3);
            Items.Add(fenster);
            Items.Add(menuTop);
            GUI.Items.MenuHorizontal menuBox = new GUI.Items.MenuHorizontal(18, fenster.Area.Y + 15, 3);
            _chooseTypeButtonXBase = 18;
            _eraserBrusherButtonXBase = 18;
            chooseTypeButton = new GUI.Components.MultiButton(_chooseTypeButtonXBase + _xClose, fenster.Area.Y + 15, "multiButton", new List<int> { 53, 92, 98 });
            chooseTypeButton.DoClick = ChooseType;
            menuBox.Components.Add(chooseTypeButton);
            eraserBrusherButton = new GUI.Components.MultiButton(_eraserBrusherButtonXBase + _xClose, fenster.Area.Y + fenster.Area.Height - 41, "multiButton2", new List<int> { 121, 122 });
            eraserBrusherButton.DoClick = ChooseBrushErase;
            menuBox.Components.Add(eraserBrusherButton);
            _listButtonsXBase = 20;
            listButtons = new GUI.Items.ScrollingListButtons(_listButtonsXBase + _xClose, 140, 234, 453);
            Items.Add(listButtons);
            Items.Add(menuBox);
            #endregion
            #region Chargement textures
            int width = ((Tile.tileSetTextures.GetLength(0) - 1) * Tile.tileSetTextures[0, 0].Width + Tile.tileSetTextures[Tile.tileSetTextures.GetLength(0) - 1, 0].Width) / Tile.tileWidth,
                height = ((Tile.tileSetTextures.GetLength(1) - 1) * Tile.tileSetTextures[0, 0].Height + Tile.tileSetTextures[0, Tile.tileSetTextures.GetLength(1) - 1].Height) / Tile.tileHeight;
            Rectangle sourceRectangle = new Rectangle(0, 0, Tile.tileWidth, Tile.tileHeight);
            Texture2D cropTexture;
            Color[] data;
            foreach (Type t in _obstacleTypesList)
                _obstacleList.Add(Textures.MiniGameItems[t]);
            foreach (Type t in _gameItemsTypeList)
                _gameItemList.Add(Textures.MiniGameItems[t]);
            for (int y = 0; y < height; y++)
                for (int x = 0; x < width; x++)
                {
                    sourceRectangle.X = x * Tile.tileWidth;
                    sourceRectangle.Y = y * Tile.tileHeight;
                    cropTexture = new Texture2D(game.GetGraphicDevice(), sourceRectangle.Width, sourceRectangle.Height);
                    data = new Color[sourceRectangle.Width * sourceRectangle.Height];
                    Tile.GetData(sourceRectangle, data);
                    cropTexture.SetData(data);
                    _tilesList.Add(cropTexture);
                }

            #endregion
            listButtons.SetDisplayedLines(12);
            listButtons.Selectable = true;
            listButtons.Reset(_tilesList);
            _currentItemType = itemType.tile;
            _currentItemId = -1;
            _currentMode = modeType.brush;
        }
        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            if (!fenster.Focused && !listButtons.Focused)
            {
                if (fenster.Area.X > _xClose + _fensterXBase)
                {
                    fenster.Area.X -= _speed;
                    listButtons.SetPosition(listButtons.Area.X - _speed, listButtons.Area.Y);
                    eraserBrusherButton.Area.X -= _speed;
                    chooseTypeButton.Area.X -= _speed;
                    chooseTypeButton.SetPosition();
                    eraserBrusherButton.SetPosition();
                }

            }
            else
            {
                if (fenster.Area.X < _xOpen + _fensterXBase)
                {
                    fenster.Area.X += _speed;
                    listButtons.SetPosition(listButtons.Area.X + _speed, listButtons.Area.Y);
                    eraserBrusherButton.Area.X += _speed;
                    chooseTypeButton.Area.X += _speed;
                    chooseTypeButton.SetPosition();
                    eraserBrusherButton.SetPosition();
                }
            }
            foreach (Item i in PlayGame.Map.ListItems)
                i.Update(gameTime);
            if (_currentItem != null)
                _currentItem.Update(gameTime);
            if (listButtons.Focused)
            {
                if (Inputs.PreMouseState.LeftButton == ButtonState.Pressed && Inputs.MouseState.LeftButton == ButtonState.Released)
                {
                    _currentItemId = listButtons.GetIndexSelected();
                    switch (_currentItemType)
                    {
                        case itemType.obstacle:
                            if (_currentItemId > _obstacleTypesList.Count)
                                _currentItemId = -1;
                            else if (_currentItemId >= 0 && (_currentItem == null || _currentItem.GetType() != _obstacleTypesList[_currentItemId]))
                            {
                                _currentItem = PlayGame.Items.Loader.LoadObstacle(_obstacleTypesList[_currentItemId]);
                                _currentItem.Alpha = _alpha;
                            }
                            break;
                        case itemType.item:
                            if (_currentItemId > _gameItemsTypeList.Count)
                                _currentItemId = -1;
                            else if (_currentItemId >= 0 && (_currentItem == null || _currentItem.GetType() != _gameItemsTypeList[_currentItemId]))
                            {
                                _currentItem = PlayGame.Items.Loader.LoadItem(_gameItemsTypeList[_currentItemId], Data.Network.IdPlayer);
                                _currentItem.Alpha = _alpha;
                            }
                            break;
                        default:
                            break;
                    }
                }
            }
            _noItemSelect = !Items.Exists(delegate(GUI.Item i) { return i.Focused; });
            if (Data.Window.GameFocus && _noItemSelect)
            {
                GameMouse.Update(gameTime);
                Camera.Update(gameTime);
                // Si l'on clique => on fait quelque chose
                if (Inputs.PreMouseState.LeftButton == ButtonState.Pressed)
                {
                    Vector2 mousePos = Map.ScreenToMap((int)Camera.x + Inputs.MouseState.X, (int)Camera.y + Inputs.MouseState.Y);
                    if (mousePos.X >= 0 && mousePos.Y >= 0 && mousePos.X < Map.Width && mousePos.Y < Map.Height)
                    {
                        switch (_currentMode)
                        {
                            case modeType.brush:
                                #region Brush
                                // C'est mieux quand on met un truc sur la map et pas ailleurs
                                if (_currentItemId >= 0)
                                {
                                    switch (_currentItemType)
                                    {
                                        case itemType.tile:
                                            Map.MapCells[(int)mousePos.X, (int)mousePos.Y].TileId = _currentItemId;
                                            break;
                                        default:
                                            if (_currentItem != null)
                                            {
                                                bool allFree = true;
                                                for (int x = (int)mousePos.X, maxX = (int)mousePos.X + _currentItem.Width; x < maxX && allFree; x++)
                                                    for (int y = (int)mousePos.Y, maxY = (int)mousePos.Y + _currentItem.Width; y < maxY && allFree; y++)
                                                        if (x >= Map.Width || y >= Map.Height || Map.ObstacleMap[x, y])
                                                            allFree = false;
                                                if (allFree)
                                                {
                                                    _currentItem.pos = null;
                                                    _currentItem.SetPos((int)mousePos.X, (int)mousePos.Y, true);
                                                    _currentItem.Color = Color.White;
                                                    _currentItem.Alpha = 1f;
                                                    if (_currentItem is Obstacle)
                                                        Map.ListPassiveItems.Add((Obstacle)_currentItem);
                                                    else
                                                        Map.AddItem(_currentItem);
                                                    _currentItem = PlayGame.Items.Loader.LoadItem(_currentItem.GetType(), Data.Network.IdPlayer);
                                                    _currentItem.Alpha = _alpha;
                                                }
                                            }
                                            break;
                                    }
                                }
                                break;
                                #endregion
                            // On a la fonction erase
                            default:
                                #region erase
                                Item currentItem = null;
                                foreach (Item i in Map.ListPassiveItems)
                                {
                                    if (i.Intersect(Inputs.MouseState.X, Inputs.MouseState.Y) && (currentItem == null || currentItem.layerDepth > i.layerDepth))
                                    {
                                        currentItem = i;
                                    }
                                }
                                foreach (Item i in Map.ListItems)
                                {
                                    if (i.Intersect(Inputs.MouseState.X, Inputs.MouseState.Y) && (currentItem == null || currentItem.layerDepth > i.layerDepth))
                                    {
                                        currentItem = i;
                                    }
                                }
                                if (currentItem != null)
                                {
                                    for (int x = currentItem.pos.Value.X, mX = x + currentItem.Width; x < mX; x++)
                                        for (int y = currentItem.pos.Value.Y, mY = y + currentItem.Width; y < mY; y++)
                                            Map.ObstacleMap[x, y] = false;
                                    Map.MapCells[currentItem.pos.Value.X, currentItem.pos.Value.Y].ListItems.Remove(currentItem);
                                }
                                break;
                                #endregion
                        }
                    }
                }
            }
            base.Update(gameTime);
        }
        public override void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch)
        {
            #region Selection de la zone à afficher
            int xMin = 0, xMax = 0, yMin = 0, yMax = 0;
            GetSubArea(ref xMin, ref xMax, ref yMin, ref yMax);
            #endregion
            #region Affichage de l'ensemble des tiles et obstacles
            Vector2 v,
                    mousePos = Map.ScreenToMap(Camera.x + Inputs.MouseState.X, Camera.y + Inputs.MouseState.Y);
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
                                     (Data.GameDisplaying.DisplayObstacle && Map.ObstacleMap[x, y]
                                          ? Data.GameDisplaying.DisplayingColor
                                          : Color.White),
                                     0f, Vector2.Zero, 1f, SpriteEffects.None, 1f
                        );
                    for (int i = 0; i < Map.MapCells[x, y].ListItems.Count; i++)
                        Map.MapCells[x, y].ListItems[i].Draw(spriteBatch);
                    if (_currentMode == modeType.brush && _currentItemId >= 0 && x == mousePos.X && y == mousePos.Y)
                    {
                        if (_currentItemType == itemType.tile)
                        {
                            Tuple<Texture2D, Rectangle> t = Tile.GetSourceRectangle(_currentItemId);
                            spriteBatch.Draw(t.Item1,
                                     v,
                                     t.Item2,
                                     Color.White,
                                     0f, Vector2.Zero, 1f, SpriteEffects.None, 0.9999f
                            );
                        }
                        else if (_currentItem != null)
                        {
                            _currentItem.SetPos(x, y, false);
                            bool allFree = true;
                            for (int dX = x, maxX = dX + _currentItem.Width; dX < maxX; dX++)
                                for (int dY = y, maxY = dY + _currentItem.Width; dY < maxY; dY++)
                                    if (dX >= Map.Width || dY >= Map.Height || Map.ObstacleMap[dX, dY])
                                    {
                                        allFree = false;
                                        break;
                                    }
                            _currentItem.Color = (allFree ? Color.White : Color.Salmon);
                            _currentItem.Draw(spriteBatch);
                        }
                    }
                }

            #endregion
            base.Draw(spriteBatch);
        }
        private void New(MenuState m, int i)
        {
            GameState newState = new EditorNewState(game, this);
            game.Push(newState);
        }
        private void Load(MenuState m, int i)
        {
            GameState load = new EditorLoadState(game, this);
            game.Push(load);
        }
        private void Save(MenuState m, int i)
        {
            GameState save = new EditorSaveState(game, this);
            game.Push(save);
        }
        void Exit(MenuState m, int i)
        {
            game.Pop();
        }
        void ChooseType(int i)
        {
            switch (i)
            {
                case 0:
                    _currentItemType = itemType.tile;
                    listButtons.Reset(_tilesList);
                    break;
                case 1:
                    listButtons.Reset(_obstacleList);
                    _currentItemType = itemType.obstacle;
                    break;
                case 2:
                    listButtons.Reset(_gameItemList);
                    _currentItemType = itemType.item;
                    break;
                default:
                    break;
            }
            _currentItemId = -1;
        }
        void ChooseBrushErase(int i)
        {
            switch (i)
            {
                case 0:
                    _currentMode = modeType.brush;
                    break;
                default:
                    _currentMode = modeType.erase;
                    break;
            }
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