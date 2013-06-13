using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Exodus.Network.Orders;

namespace Exodus.Network.ServerSide
{
    public static class TwoPStatistics
    {
        public static int P1InternetID = -1;
        public static int P2InternetID = -1;
        private List<string> armyValue = new List<string> { "", "" };
        private List<string> resourcesValue = new List<string> { "", "" };
        private List<int> killsCount = new List<int> { 0, 0 };
        private List<int> lostsCount = new List<int> { 0, 0 };
        public static string TimeElapsed = DateTime.Now.Subtract(Data.Network.GameStartTime).ToString();

        public static void AddStatistic(Statistics PlayerStats)
        {
            if (P1InternetID == -1)
                P1InternetID = PlayerStats.Id;
            else if (P2InternetID == -1)
                P2InternetID = PlayerStats.Id;
        }
        public static void Reset()
        {
            P1InternetID = -1;
            P2InternetID = -1;
        }
    }
}