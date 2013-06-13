using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Exodus.Network.Orders;

namespace Exodus.Network.ServerSide
{
    public static class TwoPStatistics
    {
        private static int P1InternetID = -1;
        private static int P2InternetID = -1;
        private static int P1ArmyValue = -1;
        private static int P2ArmyValue = -1;
        private static string P1ResourcesValue = "";
        private static string P2ResourcesValue = "";
        private static int P1TrainedCount = -1;
        private static int P2TrainedCount = -1;
        private static int P1LostCount = -1;
        private static int P2LostCount = -1;
        private static string TimeElapsed = DateTime.Now.Subtract(Data.Network.GameStartTime).ToString();

        public static void AddStatistic(Statistics PlayerStats)
        {
            if (P1InternetID == -1)
                P1InternetID = PlayerStats.Id;
            else if (P2InternetID == -1)
                P2InternetID = PlayerStats.Id;
            if (P1InternetID == PlayerStats.Id)
            {
                P1ArmyValue = PlayerStats.ArmyValue;
                P1          = PlayerStats;
                P1          = PlayerStats;
                P1          = PlayerStats;
            }
            else if (P2InternetID == PlayerStats.Id)
            {
                P2 = PlayerStats;
                P2 = PlayerStats;
                P2 = PlayerStats;
                P2 = PlayerStats;
            }
            else throw new Exception("Something went wrong...");
        }
        public static void Reset()
        {
            P1InternetID = -1;
            P2InternetID = -1;
            P1armyValue = -1;
            P2armyValue = -1;
        }
    }
}