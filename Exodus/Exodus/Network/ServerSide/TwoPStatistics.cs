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
        public int P1InternetID = -1;
        public int P2InternetID = -1;
        public int P1ArmyValue = -1;
        public int P2ArmyValue = -1;
        public string P1ResourcesValue = "";
        public string P2ResourcesValue = "";
        public int P1TrainedCount = -1;
        public int P2TrainedCount = -1;
        public int P1LostCount = -1;
        public int P2LostCount = -1;
        public string TimeElapsed = "";
        public bool P1Sended = false;
        public bool P2Sended = false;

        public void AddStatistic(Statistics PlayerStats)
        {
            if (P1InternetID == -1)
                P1InternetID = PlayerStats.InternetId;
            else if (P2InternetID == -1 && P1InternetID != PlayerStats.InternetId)
                P2InternetID = PlayerStats.InternetId;
            if (P1InternetID == PlayerStats.InternetId)
            {
                P1ArmyValue = PlayerStats.ArmyValue;
                P1ResourcesValue = PlayerStats.Electricity + "-" + PlayerStats.Iron + "-" + PlayerStats.Hydrogen + "-" + PlayerStats.Steel + "-" + PlayerStats.Graphene;
                P1TrainedCount = PlayerStats.UnitsTrained;
                P1LostCount = PlayerStats.UnitsLost;
                P1Sended = true;
            }
            else if (P2InternetID == PlayerStats.InternetId)
            {
                P2ArmyValue = PlayerStats.ArmyValue;
                P2ResourcesValue = PlayerStats.Electricity + "-" + PlayerStats.Iron + "-" + PlayerStats.Hydrogen + "-" + PlayerStats.Steel + "-" + PlayerStats.Graphene;
                P2TrainedCount = PlayerStats.UnitsTrained;
                P2LostCount = PlayerStats.UnitsLost;
                P2Sended = true;
            }
            else throw new Exception("Something went wrong...");
            //To send
            if (P1Sended && P2Sended)
            {
                TimeElapsed = DateTime.Now.Subtract(Data.Network.GameStartTime).ToString().Substring(0, 8);
                byte[] SStats = Serialize.Serializer.ObjectToByteArray(this);
                byte[] SStatsPlusThree = new byte[SStats.Length + 3];
                SStats.CopyTo(SStatsPlusThree, 3);
                SStatsPlusThree[0] = (byte)((SStats.Length + 1) / 256);
                SStatsPlusThree[1] = (byte)((SStats.Length + 1) % 256);
                SStatsPlusThree[2] = 2;
                SyncClient.SendDataToGameManagerAsServer(SStatsPlusThree);
                Reset();
            }
        }
        public void Reset()
        {
            //P1InternetID = -1;
            //P2InternetID = -1;
            P1Sended = false;
            P2Sended = false;
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