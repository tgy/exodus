using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Exodus.PlayGame
{
    // Classe MapCell
    [Serializable]
    public class MapCell
    {
        public int TileId;
        public List<Item> ListItems = new List<Item>();
        public MapCell(int tileId)
        {
            TileId = tileId;
        }
    }

    static public class Map
    {
        // Notre map
        public static MapCell[,] MapCells;
        // Notre équivalent en booléen d'obstacles
        public static bool[,] ObstacleMap;
        // Width, height de la map : map[width,height]
        public static int Width, Height;
        // Position absolue à l'écran du point tout en haut à gauche de la map de coordonnées [width - 1,0]
        public static int XBase, YBase;
        // Texture nécessaire au ScreenToMap
        static public Texture2D MouseMap;
        // Nos listes d'unité/obstacles sur la map
        public static List<Item> ListItems = new List<Item>();
        public static void AddItem(Item i)
        {
            if (Data.Network.SinglePlayer)
                i.PrimaryId = ListItems.Count;
            ListItems.Add(i);
        }
        public static List<Item> ListPassiveItems = new List<Item>();
        private static bool _selectionContainsUnit = false;
        public static void AddItemsToSelection(List<Item> l)
        {
            foreach (Item i in l)
            {
                if (_selectionContainsUnit)
                {
                    if (i is Unit && !Map.ListSelectedItems.Contains(i))
                    {
                        Map.ListSelectedItems.Add(i);
                        i.Focused = true;
                    }
                }
                else
                {
                    if (i is Unit)
                    {
                        _selectionContainsUnit = true;
                        for (int j = 0; j < Map.ListSelectedItems.Count; )
                        {
                            if (Map.ListSelectedItems[j] is Unit)
                                j++;
                            else
                            {
                                Map.ListSelectedItems[j].Focused = false;
                                Map.ListSelectedItems.RemoveAt(j);
                            }
                        }
                        Map.ListSelectedItems.Add(i);
                        i.Focused = true;
                    }
                    else if (!Map.ListSelectedItems.Contains(i))
                    {
                        Map.ListSelectedItems.Add(i);
                        i.Focused = true;
                    }
                }
            }
        }
        public static void ClearSelection()
        {
            _selectionContainsUnit = false;
            for (int i = Map.ListSelectedItems.Count - 1; i >= 0; i--)
                Map.ListSelectedItems[i].Focused = false;
            Map.ListSelectedItems.Clear();
        }
        public static List<Item> ListSelectedItems = new List<Item>();
        // Charge une map standard
        public static void Load(int widthMap, int heightMap)
        {
            ListItems = new List<Item>();
            ListPassiveItems = new List<Item>();
            // On Fixe ça à la barbare :3
            int XMax = Int32.MaxValue,
                XMin = 0,
                YMax = Int32.MaxValue,
                YMin = 0;
            MapCell[,] m = new MapCell[widthMap, heightMap];
            Random r = new Random();
            int max = heightMap > widthMap ? heightMap : widthMap;
            int count = 0;
            int x, y;
            for (int i = 0; i < max; i++)
            {
                for (int j = i; j >= 0; j--)
                {
                    x = max - 1 - j;
                    y = i - j;
                    m[x, y] = new MapCell(count);
                    count++;
                }
            }
            for (int i = max - 1; i > 0; i--)
            {
                for (int j = 0; j < i; j++)
                {
                    x = j;
                    y = max - i + j;
                    m[x, y] = new MapCell(count);
                    count++;
                }
            }
            Camera.Initialise(XMin, XMax, YMin, YMax);
            // On fixe la position du  premier point servant à dessiner les maps
            Width = widthMap;
            Height = heightMap;
            MapCells = m;
            XBase = (widthMap - 1) * Tile.tileWidth / 2;
            YBase = 0;
            CreateObstacleMap();
        }
        // Charge une map à partir d'un fichier
        public static void Load(string pathMap)
        {
            ListPassiveItems.Clear();
            ListItems.Clear();
            try
            {
                // Si le directory n'existe pas, on le crée.
                if (!Directory.Exists(Data.Config.PathMaps))
                    Directory.CreateDirectory(Data.Config.PathMaps);
                MapFile file = (MapFile)Serialize.Serializer.DeSerialize(Data.Config.PathMaps + pathMap + Data.Config.MapsFileExtension);
                MapCell[,] m = new MapCell[file.map.GetLength(0), file.map.GetLength(0)];
                for (int x = 0; x < m.GetLength(0); x++)
                    for (int y = 0; y < m.GetLength(1); y++)
                        m[x, y] = new MapCell(file.map[x, y]);
                Camera.Initialise(file.xMin, file.xMax, file.yMin, file.yMax);
                Width = m.GetLength(0);
                Height = m.GetLength(1);
                XBase = (Width - 1) * Tile.tileWidth / 2;
                YBase = Tile.tileHeight - Tile.tileHeight;
                MapCells = m;
                CreateObstacleMap();
                ListItems = file.listItems;
                ListPassiveItems = file.listPassives;
                foreach (Item i in ListPassiveItems)
                    i.SetPos(i.pos.Value.X, i.pos.Value.Y, true);
                foreach (Item i in ListItems)
                    i.SetPos(i.pos.Value.X, i.pos.Value.Y, true);
                // On place notre point base
            }
            // S'il y a une erreur pendant la lecture de fichier source, on génère une map standard
            catch (Exception e)
            {
                Load(50, 50);
            }

        }
        public static void SetCameraMove(int xMin2, int yMin2, int xMax2, int yMax2)
        {
            /*XMin = xMin2; YMin = yMin2; XMax = xMax2; YMax = yMax2;
            Camera.Location = new Vector2(XMin, YMin);
             */
        }
        // Conversion écran/map
        public static Vector2 ScreenToMap(int xMouse, int yMouse)
        {
            int localXMouse = (xMouse - (Width % 2 == 0 ? Tile.tileWidth / 2 : 0)) % MouseMap.Width,
                localYMouse = yMouse % MouseMap.Height;
            while (localXMouse < 0)
                localXMouse += MouseMap.Width;
            while (localYMouse < 0)
                localYMouse += MouseMap.Height;
            xMouse = xMouse - (xMouse + (Width % 2 == 0 ? Tile.tileWidth / 2 : 0)) % (Tile.tileWidth);
            yMouse = yMouse - yMouse % (Tile.tileHeight);
            uint[] color = new uint[1];
            // Magnifique calcul ultraaaaaaa simple trouvé un poil au pif (mais pas complètement lolilol :D)
            int x = (xMouse - XBase) / Tile.tileWidth - (yMouse - YBase) / Tile.tileHeight + Width,
                y = 2 * (yMouse - YBase) / Tile.tileHeight - Width + x - 1;
            /*spriteBatch.Draw(mouseMap, new Vector2(xMouse, yMouse), Color.White);
            spriteBatch.Draw(mouseMap, new Vector2(localXMouse, localYMouse), Color.White);/**/
            MouseMap.GetData(0, new Rectangle(localXMouse, localYMouse, 1, 1), color, 0, 1);
            switch (color[0])
            {
                // Red
                case 0xFF0000FF:
                    y--;
                    break;
                // Blue
                case 0xFFFF0000:
                    y++;
                    break;
                // Green
                case 0xFF00FF00:
                    x--;
                    break;
                // Yellow
                case 0xFF00FFFF:
                    x++;
                    break;
                default:
                    break;
            }
            return new Vector2(x, y);
        }
        public static Vector2 MapToScreen(int xCoord, int yCoord)
        {
            int left = XBase + (xCoord + yCoord - Width + 1) * Tile.tileWidth / 2,
                top = YBase + (Width - xCoord + yCoord + 1) * Tile.tileHeight / 2;
            return new Vector2(left, top);
        }
        // Calcule la map d'obstacle
        static void CreateObstacleMap()
        {
            ObstacleMap = new bool[Width, Height];
            for (int x = 0; x < Width; x++)
                for (int y = 0; y < Height; y++)
                    ObstacleMap[x, y] = false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pos">Absolute screen coordinate</param>
        /// <returns></returns>
        static public float GetLayerDepth(Vector2 pos)
        {

            int height = (Width + Height) * Tile.tileHeight / 2;
            float depth = (1 - pos.Y / height) * 8 / 10 + 0.1f;
            return depth;
        }
    }
}
