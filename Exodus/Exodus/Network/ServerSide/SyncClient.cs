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
        private static BinaryWriter sender;
        private static BinaryReader NetReader;
        private static TcpClient Client;
        static bool _MySQLConnection = false;
        public static void ConnectAsServer()
        {
            WhatIsIt = 0;
            InitializeConnection();
            SendIdMessage();
            Data.Network.Server = "Server (synchronized): Connected clients:";
        }
        private static string[][] SQLAnswer;

        public static void ConnectAsClient()
        {
            WhatIsIt = 1;
            InitializeConnection();
            NetReader = new BinaryReader(Client.GetStream());
            SendIdMessage();
            Receive();
        }

        public static void ConnectToSendRequest()
        {
            WhatIsIt = 2;
            InitializeConnection();
            NetReader = new BinaryReader(Client.GetStream());
            SendIdMessage();
        }

        private static void InitializeConnection()
        {
            if (IsRunning)
            {
                Stop();
                Thread.Sleep(10);
                //throw new Exception("Why was it already started?");
            }
            try
            {
                //Client = new TcpClient(Dns.GetHostAddresses("thefirsthacker.myftp.org")[0].ToString(), 4000);
                Client = new TcpClient("192.168.1.15", 4000);
                //NetReader = new BinaryReader(Client.GetStream());
                //InternetGames = new List<Game>();
            }
            catch
            {
                return;
            }
            sender = new BinaryWriter(Client.GetStream());
            IsRunning = true;
            if (WhatIsIt != 2)
            {
                Thread Pinger = new Thread(Ping);
                Pinger.Name = "SyncClientPing";
                Pinger.Start();
            }
        }
        public static void UserIsValid(string UserName, string Password)
        {
            SQLAnswer = null;
            ConnectToSendRequest();
            //Player.ConnectionState = 2;
            SendDBCMDToGameManager("SELECT * FROM `user` WHERE `name`=\"" + UserName + "\" AND `password`=\""+ SHA1(Password) +"\"");
            Receive();
            for (byte b = 0; b < 10; b++)
            {
                Thread.Sleep(100);
                if (SQLAnswer != null)
                {
                    if (SQLAnswer.Length == 1)
                        Player.ConnectionState = 1;
                    else
                        Player.ConnectionState = 0;
                    return;
                }
            }
        }
        private static void SendIdMessage()
        {
            //if (!IsRunning)
            //    Thread.Sleep(100);
            for (byte i = 0; i < 10; i++)
                if (!IsRunning)
                    Thread.Sleep(10);
            sender.Write(WhatIsIt);
        }
        public static void SendDBCMDToGameManager(string command)
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
                    SendDataToGameManager(cmd);
                    _MySQLConnection = false;
                    break;
                }
            }
        }
        public static void SendGame(byte[] SGame)
        {
            //byte[] SGame = Serialize.Serializer.ObjectToByteArray(Server.TheGame);
            byte[] GamePlusThree = new byte[SGame.Length + 3];
            SGame.CopyTo(GamePlusThree, 3);
            GamePlusThree[0] = (byte)((SGame.Length + 1) / 256);
            GamePlusThree[1] = (byte)((SGame.Length + 1) % 256);
            GamePlusThree[2] = 1;
            for (byte i = 0; i < 10; i++)
                if (!IsRunning)
                    Thread.Sleep(100);
            SendDataToGameManager(GamePlusThree);
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
        private static void SendDataToGameManager(byte[] data)
        {
            //InitializeConnection();
            sender.Write(data);
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
                    ProcessSQLRequest(ShortenArray(data,1));
                    break;

                //case 3:
                //
                //    break;
                default:
                    throw new Exception("What (the hell) was that?");
            }
        }
        private static void Ping()
        {
            byte[] ping = new byte[3];
            ping[0] = 0;
            ping[1] = 1;
            ping[2] = 0;
            Thread.Sleep(100);
            while (IsRunning)
            {
                sender.Write(ping);
                Thread.Sleep(100);
            }
        }
        private static void ProcessSQLRequest(byte[]data)
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
                sender.Close();
            }
            InternetGames.Clear();
        }
        private static string SHA1(string s1)
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
}