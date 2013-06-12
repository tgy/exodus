using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using Exodus.Network.ClientSide;
using Exodus.Network.ServerSide;
using Exodus.PlayGame;
using Exodus.PlayGame.Items.Units;
using Exodus.PlayGame.Items.Buildings;

namespace Exodus
{
    public static class Data
    {
        public static class GameInfos
        {
            public static Dictionary<Type, int> timeCreatingItem = new Dictionary<Type, int>();
            public static Dictionary<Type, Resource> CostsItems = new Dictionary<Type, Resource>();
            public enum ModeGame
            {
                Normal,
                Attack,
                Patrol,
                Building
            }
            public static ModeGame currentMode = ModeGame.Normal;
            public static Type type;
            public static PlayGame.Item item = null;
        }
        public static class Network
        {
            //Specifies which type of game to start: "DS" for Dedicated Server, "C" for Client only,
            //"SC" for StarCraft (or Server & Client), or "No" for a singleplayer game.
            //public static string GameType = "SC";
            public static string LastIP = "127.0.0.1";
            public static bool Broadcast = true;
            public static int Port = 1337;
            public static int IdPlayer = 1;
            public static string Running_as = "Nothing started";
            public static string ServerIP = "";
            public static string Server = "";
            public static string Client = "";
            public static string OnlineSync = "";
            public static string Error = "";
            public static bool SinglePlayer = true;
            public static int MaxPlayersInCurrentGame = 5;
            public static List<SClient> ConnectedClients = new List<SClient>();
        }
        public static class Window
        {
            static public bool GameFocus;
            static public int WindowWidth;
            static public int WindowHeight;
            public static Point ScreenCenter;
        }
        public static class GameDisplaying
        {
            public static GraphicsDevice GraphicsDevice;
            public const float Epsilon = 0.000001f;
            public static bool DisplayObstacle = false;
            public static int GraphicQuality = 0;
            public static int PaddingMap = 7;
        }
        public static class PlayerInfos
        {
            public static string Name = "";
            public const string beginAvatar = "http://192.168.1.33/exodus/web/avatar/";
        }
        public static class Config
        {
            public static string Login = "";
            public static string Pwd = "";
            public static int LevelSound = 0;
            public static string PathConfig = "../../../Datas/config.txt";
            public static string PathMaps = "../../../Maps/";
            public static string currentMap = "";
            public static string MapsFileExtension = ".exd";
            public static int MapMaxLenghtName = 42;
            public static int TimerDoubleClick = 200;
            public static int SpaceBetweenMenuItems = 30;
            //public static int SpaceFromBorderMovingCamera = 30;
            public static int DecalageSimpleClick = 5;
            public static int textBoxPeriodRepetition = 1000;
            public static int textBoxPeriodRepetitionCursor = 10;
        }
        public static class Security
        {
            public static string SHA1(string s1)
            {
                System.Security.Cryptography.SHA1CryptoServiceProvider x = new System.Security.Cryptography.SHA1CryptoServiceProvider();
                byte[] bs = System.Text.Encoding.ASCII.GetBytes(s1);
                bs = x.ComputeHash(bs);
                System.Text.StringBuilder s = new System.Text.StringBuilder();
                foreach (byte b in bs)
                {
                    s.Append(b.ToString("x2").ToLower());
                }
                string password = s.ToString();
                return password;
            }
        }
        public static void Load()
        {
            GameInfos.timeCreatingItem.Add(typeof(Gunner), 5000);
            GameInfos.timeCreatingItem.Add(typeof(Worker), 1000);
            GameInfos.timeCreatingItem.Add(typeof(Habitation), 1500);
            GameInfos.timeCreatingItem.Add(typeof(Spider), 5000);
            GameInfos.timeCreatingItem.Add(typeof(Labo), 5000);
            GameInfos.CostsItems.Add(typeof(Gunner), new Resource(100, 50, 0, 0, 20));
            GameInfos.CostsItems.Add(typeof(Worker), new Resource(0, 150, 0, 0, 2));
            GameInfos.CostsItems.Add(typeof(Habitation), new Resource(0, 150, 0, 0, 10));
            GameInfos.CostsItems.Add(typeof(Spider), new Resource(150, 0, 10, 0, 35));
            GameInfos.CostsItems.Add(typeof(Labo), new Resource(350, 500, 100, 200, 100));
            LoadPlayerConfig();
        }
        static void LoadPlayerConfig()
        {
            try
            {
                string[] configs = System.IO.File.ReadAllLines(Config.PathConfig);
                foreach (
                    string[] array in
                        configs.Select(s => s.Replace(" ", ""))
                               .Select(config => config.Split('='))
                               .Where(array => array.Length == 2))
                {
                    switch (array[0])
                    {
                        case "DisplayObstacles":
                            bool.TryParse(array[1], out GameDisplaying.DisplayObstacle);
                            break;
                        case "LevelSound":
                            Int32.TryParse(array[1], out Config.LevelSound);
                            break;
                        case "GraphicQuality":
                            Int32.TryParse(array[1], out GameDisplaying.GraphicQuality);
                            break;
                        case "PaddingMap":
                            Int32.TryParse(array[1], out GameDisplaying.PaddingMap);
                            break;
                        case "LastIP":
                            Network.LastIP = array[1];
                            break;
                        case "Port":
                            Int32.TryParse(array[1], out Network.Port);
                            break;
                        case "Pwd":
                            Config.Pwd = SymetricCrypt(array[1]);
                            break;
                        case "Login":
                            Config.Login = SymetricCrypt(array[1]);
                            break;
                    }
                }
            }
            catch
            {

            }
        }
        public static void SavePlayerConfig()
        {
            try
            {
                System.IO.File.WriteAllText(Config.PathConfig,
                    "DisplayObstacles="+GameDisplaying.DisplayObstacle+"\n"+
                    "LevelSound="+Config.LevelSound+"\n"+
                    "GraphicQuality="+GameDisplaying.GraphicQuality+"\n"+
                    "PaddingMap="+GameDisplaying.PaddingMap+"\n"+
                    "LastIP="+Network.LastIP+"\n"+
                    "Port="+Network.Port+"\n"+
                    "Login="+SymetricCrypt(Config.Login)+"\n"+
                    "Pwd="+SymetricCrypt(Config.Pwd));
            }
            catch
            {
            }
        }
        static string SymetricCrypt(string s)
        {
            string result = "";
            short key = 17651;
            for (int i = 0; i < s.Length; i++)
                result += Convert.ToChar(s[i] ^ key);
            return result;
        }
    }
}
