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
        public static bool IsAuthenticated;
        private static BinaryWriter sender;
        private static BinaryReader NetReader;
        private static TcpClient Client;

        public static void ConnectAsServer()
        {
            InitializeConnection();
            SendIdMessage(true);
            Data.Network.Server = "Server (synchronized): Connected clients:";
        }

        public static void ConnectAsClient()
        {
            InitializeConnection();
            NetReader = new BinaryReader(Client.GetStream());
            SendIdMessage(false);
            Receive();
        }

        public static void ConnectToAuthenticate()
        {
            InitializeConnection();
            NetReader = new BinaryReader(Client.GetStream());
            SendIdMessage(false);
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
            if (IsAuthenticated)
            {
                Thread Pinger = new Thread(Ping);
                Pinger.Name = "SyncClientPing";
                Pinger.Start();
            }
        }

        public static void UserIsValid(string UserName, string Password)
        {
            ConnectToAuthenticate();
            Player.ConnectionState = 2;
            //FIXME: La commande pour rechercher l'utilisateur;
            SendDBCMDToGameManager("SELECT * FROM `user` WHERE `name`=\"" + UserName + "\" AND `password`=SHA1(\"" + Password + "\")");
            for (byte b = 0; b < 50; b++)
            {
                Thread.Sleep(100);
                if (Player.ConnectionState == 1)
                {
                    IsAuthenticated = true;
                    return;
                }
            }
        }

        private static void SendIdMessage(bool IsServer)
        {
            //if (!IsRunning)
            //    Thread.Sleep(100);
            byte ID;
            if (IsAuthenticated)
            {
                if (IsServer)
                    ID = 0;
                else
                    ID = 1;
            }
            else ID = 2;
            for (byte i = 0; i < 10; i++)
                if (!IsRunning)
                    Thread.Sleep(10);
            sender.Write(ID);
        }
        public static void SendDBCMDToGameManager(string command)
        {
            byte[] Serialized = Serialize.Serializer.ObjectToByteArray(command);
            byte[] cmd = new byte[Serialized.Length + 3];
            Serialized.CopyTo(cmd, 3);
            cmd[0] = (byte)(cmd.Length / 256);
            cmd[1] = (byte)(cmd.Length % 256);
            cmd[2] = 2;

            SendDataToGameManager(cmd);
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
            InitializeConnection();
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
                    Player.ConnectionState = data[1];
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
                //try
                //{
                sender.Write(ping);
                //}
                //catch
                //{
                //    Connect();
                //    Thread.Sleep(500);
                //}
                Thread.Sleep(100);
            }
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
    }
}