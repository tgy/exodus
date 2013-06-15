using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.IO;
using System.Threading;
using System.Net;

namespace Exodus.Network.ServerSide
{
    static class SyncClient
    {
        public static List<Game> InternetGames = new List<Game>();
        private static bool IsRunning;
        //public static bool IsAuthenticated;
        private static byte WhatIsIt;
        private static BinaryWriter ServerSender;
        private static BinaryWriter DBTalker;
        private static BinaryWriter ClientSender;
        private static BinaryReader NetReader;
        private static TcpClient Client;
        static bool _MySQLConnection = false;
        private static string[][] SQLAnswer;

        public static void ConnectAsServer()
        {
            if (ServerSender == null/* || ServerSender.BaseStream.CanTimeout*/)
            {
                WhatIsIt = 0;
                InitializeConnection();
                SendIdMessage();
                Data.Network.Server = "Server (synchronized): Connected clients:";
            }
        }
        public static void ConnectAsClient()
        {
            if (ClientSender == null)
            {
                WhatIsIt = 1;
                InitializeConnection();
                NetReader = new BinaryReader(Client.GetStream());
                SendIdMessage();
                Receive();
            }
        }
        public static void ConnectToSendRequest()
        {
            WhatIsIt = 2;
            InitializeConnection();
            try
            {
                NetReader = new BinaryReader(Client.GetStream());
            }
            catch
            {
                throw new Exception("Could not connect to Exodus Online!");
            }
            SendIdMessage();
        }
        private static void InitializeConnection()
        {
            //if (IsRunning)
            //{
            //    Stop();
            //    Thread.Sleep(10);
            //    //throw new Exception("Why was it already started?");
            //}
            try
            {
                //Client = new TcpClient(Dns.GetHostAddresses("thefirsthacker.myftp.org")[0].ToString(), 4000);
                Client = new TcpClient("192.168.1.14", 4000);
                //Client = new TcpClient("192.168.1.15", 4000);
                //NetReader = new BinaryReader(Client.GetStream());
                //InternetGames = new List<Game>();
            }
            catch
            {
                return;
            }
            Thread Pinger;
            switch (WhatIsIt)
            {
                case 0:
                    ServerSender = new BinaryWriter(Client.GetStream());
                    Pinger = new Thread(new ParameterizedThreadStart(Ping));
                    Pinger.Name = "SyncServerPing";
                    Pinger.Start(ServerSender);
                    break;
                case 1:
                    ClientSender = new BinaryWriter(Client.GetStream());
                    Pinger = new Thread(new ParameterizedThreadStart(Ping));
                    Pinger.Name = "SyncClientPing";
                    Pinger.Start(ClientSender);
                    break;
                case 2:
                    DBTalker = new BinaryWriter(Client.GetStream());
                    break;
                default:
                    throw new Exception("This never happends...");
            }
            IsRunning = true;
        }
        public static int UserIsValid(string UserName, string Password)
        {
            ConnectToSendRequest();
            //Player.ConnectionState = 2;
            SendDBCMDToGameManager("SELECT * FROM `user` WHERE `name`=\"" + UserName + "\" AND `password`=\"" + Data.Security.SHA1(Password) + "\"");
            SQLAnswer = null;
            Receive();
            for (byte b = 0; b < 100; b++)
            {
                if (SQLAnswer != null)
                {
                    if (SQLAnswer.Length == 1)
                    {
                        Player.ConnectionState = 1;
                        Data.PlayerInfos.Name = UserName;
                        return Int32.Parse(SQLAnswer[0][0]);
                    }
                    else
                    {
                        Player.ConnectionState = 0;
                        return -1;
                    }
                }
                else
                    Thread.Sleep(10);
            }
            return -1;
        }
        public static string[][] SendSQLRequest(string request)
        {
            SQLAnswer = null;
            ConnectToSendRequest();
            SendDBCMDToGameManager(request);
            Receive();
            for (byte b = 0; b < 10; b++)
            {
                Thread.Sleep(100);
                if (SQLAnswer != null)
                    return SQLAnswer;
            }
            return null;
        }
        public static void SendSQLOrder(string request)
        {
            ConnectToSendRequest();
            SendDBCMDToGameManager(request);
        }
        private static void SendIdMessage()
        {
            //if (!IsRunning)
            //    Thread.Sleep(100);
            for (byte i = 0; i < 10; i++)
                if (!IsRunning)
                    Thread.Sleep(10);
            switch (WhatIsIt)
            {
                case 0: //Server
                    ServerSender.Write(WhatIsIt);
                    break;
                case 1: //Client
                    ClientSender.Write(WhatIsIt);
                    break;
                case 2:
                    DBTalker.Write(WhatIsIt);
                    break;
                default:
                    throw new Exception("This never happends...");
            }
        }
        private static void SendDBCMDToGameManager(string command)
        {
            while (IsRunning)
            {
                if (_MySQLConnection)
                    Thread.Sleep(10);
                else
                {
                    _MySQLConnection = true;
                    byte[] Serialized = Serialize.Serializer.ObjectToByteArray(command);
                    byte[] cmd = new byte[Serialized.Length + 2];
                    Serialized.CopyTo(cmd, 2);
                    cmd[0] = (byte)(Serialized.Length / 256);
                    cmd[1] = (byte)(Serialized.Length % 256);
                    DBTalker.Write(cmd);
                    _MySQLConnection = false;
                    break;
                }
            }
        }
        public static void SendGame(byte[] SGame)
        {
            ConnectAsServer();
            //byte[] SGame = Serialize.Serializer.ObjectToByteArray(Server.TheGame);
            byte[] GamePlusThree = new byte[SGame.Length + 3];
            SGame.CopyTo(GamePlusThree, 3);
            GamePlusThree[0] = (byte)((SGame.Length + 1) / 256);
            GamePlusThree[1] = (byte)((SGame.Length + 1) % 256);
            GamePlusThree[2] = 1;
            for (byte i = 0; i < 10; i++)
                if (!IsRunning)
                    Thread.Sleep(100);
            ServerSender.Write(GamePlusThree);
        }
        private static void Receive()
        {
            int lenght = 0;
            byte[] data = new byte[0];
            while (IsRunning)
            {
                if (/*client.Connected && */Client.Available != 0)
                {
                    lenght = NetReader.ReadByte() * 256 + NetReader.ReadByte();
                    data = NetReader.ReadBytes(lenght + 1);
                    //object GameList = Serialize.Serializer.ByteArrayToGameList(data);
                    //InternetGames = (List<Game>)GameList;
                    ProcessData(data);
                }
                else
                    Thread.Sleep(100);
            }
        }
        public static void SendDataToGameManagerAsServer(byte[] data)
        {
            ConnectAsServer();
            ServerSender.Write(data);
        }
        private static void ProcessData(byte[] data)
        {
            object o;
            switch (data[0])
            {
                case 0: //Add Game
                    o = Serialize.Serializer.ByteArrayToGame(ShortenArray(data, 1));
                    InternetGames.Add((Game)o);
                    break;

                case 1: //Clear Game
                    InternetGames.Clear();
                    break;

                case 2:
                    IsRunning = false;
                    ProcessSQLRequest(ShortenArray(data, 1));
                    break;

                //case 3:
                //
                //    break;
                default:
                    throw new Exception("What (the hell) was that?");
            }
        }
        private static void Ping(object bw)
        {
            BinaryWriter pinger = (BinaryWriter)bw;
            byte[] ping = new byte[3];
            ping[0] = 0;
            ping[1] = 1;
            ping[2] = 0;
            Thread.Sleep(100);
            while (IsRunning)
            {
                pinger.Write(ping);
                Thread.Sleep(100);
            }
        }
        private static void ProcessSQLRequest(byte[] data)
        {
            SQLAnswer = (string[][])Serialize.Serializer.ByteArrayToObject(data);
        }
        private static byte[] ShortenArray(byte[] Long, int StartIndex)
        {
            byte[] Short = new byte[Long.Length - StartIndex];
            for (int i = 0; i < Long.Length - StartIndex; i++)
                Short[i] = Long[i + StartIndex];
            return Short;
        }
        public static void Stop()
        {
            IsRunning = false;
            if (NetReader != null)
            {
                NetReader.Close();
                NetReader = null;
            }
            DBTalker.Close();
            if (ClientSender != null)
            {
                ClientSender.Close();
                ClientSender = null;
            }
            if (ServerSender != null)
            {
                ServerSender.Close();
                ServerSender = null;
            }
            InternetGames.Clear();
        }
    }
}