using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Exodus.Network
{
    [Serializable]
    public class Game
    {
        public string IP;
        public int NbPlayers;
        public int NbObservers;
        public DateTime CreationTime;
        public string HostName;
        public string Map;
    
        public Game(string LocalIP, string MapName)
        {
            IP = LocalIP;
            Map = MapName;
            NbPlayers = 0;
            NbObservers = 0;
            CreationTime = DateTime.Now;
            HostName = Data.PlayerInfos.Name;
        }
    }
}
