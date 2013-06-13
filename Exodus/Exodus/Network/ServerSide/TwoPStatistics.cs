using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Exodus.Network.Orders;

namespace Exodus.Network.ServerSide
{
    [Serializable]
    class TwoPStatistics
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
        private static string TimeElapsed = "";

        public void AddStatistic(Statistics PlayerStats)
        {
            if (P1InternetID == -1)
                P1InternetID = PlayerStats.Id;
            else if (P2InternetID == -1 && P1InternetID != PlayerStats.Id)
                P2InternetID = PlayerStats.Id;
            if (P1InternetID == PlayerStats.Id)
            {
                P1ArmyValue = PlayerStats.ArmyValue;
                P1ResourcesValue = PlayerStats.Electricity + "-" + PlayerStats.Iron + "-" + PlayerStats.Hydrogen + "-" + PlayerStats.Steel + "-" + PlayerStats.Graphene + ";";
                P1TrainedCount = PlayerStats.UnitsTrained;
                P1LostCount = PlayerStats.UnitsLost;
                TimeElapsed = DateTime.Now.Subtract(Data.Network.GameStartTime).ToString();
                if (P2InternetID != -1)
                {
                    byte[] SStats = Serialize.Serializer.ObjectToByteArray(this);
                    byte[] SStatsPlusThree = new byte[SStats.Length + 3];
                    SStatsPlusThree[0] = (byte)((SStats.Length + 1) / 256);
                    SStatsPlusThree[1] = (byte)((SStats.Length + 1) % 256);
                    SStatsPlusThree[2] = 2;
                    SyncClient.SendDataToGameManager(SStatsPlusThree);
                }
            }
            else if (P2InternetID == PlayerStats.Id)
            {
                P2ArmyValue = PlayerStats.ArmyValue;
                P2ResourcesValue = PlayerStats.Electricity + "-" + PlayerStats.Iron + "-" + PlayerStats.Hydrogen + "-" + PlayerStats.Steel + "-" + PlayerStats.Graphene + ";";
                P2TrainedCount = PlayerStats.UnitsTrained;
                P2LostCount = PlayerStats.UnitsLost;
            }
            else throw new Exception("Something went wrong...");
        }
        public void Reset()
        {
            P1InternetID = -1;
            P2InternetID = -1;
            P1ArmyValue = -1;
            P2ArmyValue = -1;
            P1ResourcesValue = "";
            P2ResourcesValue = "";
            P1TrainedCount = -1;
            P2TrainedCount = -1;
            P1LostCount = -1;
            P2LostCount = -1;
            TimeElapsed = "";
        }
    }
}