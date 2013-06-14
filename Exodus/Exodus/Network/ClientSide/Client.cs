using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.IO;
using Exodus.GUI;
using Exodus.Network.Orders;
using Microsoft.Xna.Framework;
using Exodus.Network.ServerSide;
using Exodus.PlayGame;

namespace Exodus.Network.ClientSide
{
    static class Client
    {
        public static List<Game> ServerList = new List<Game>();

        private static string IP;
        public static bool IsRefreshing { get; private set; }
        private static bool IsRunning;
        private static BinaryWriter sender;
        private static TcpClient client;
        private static UdpClient BroadcastListener;
        private static IPEndPoint EndPoint;
        public static GUI.Items.Chat chat;
        public static int UnitsTrained = 0;
        public static int UnitsLost = 0;
        public static Orders.Tasks.ReSync ReSyncOrder = null;
        public static bool MustReSync = false;
        public static GUI.Items.PlayerInfosLaunching player1;

        #region Start
        public static void Start(object ip)
        {
            if (ip is string)
                Start((string)ip);
            else
                throw new Exception("Unknown Type " + ip.GetType());
        }
        private static void Start(string ip)
        {
            IsRunning = false;
            try
            {
                client = new TcpClient(ip, Data.Network.Port);
            }
            catch
            {
                Data.Network.ServerIP = "Could not connect to " + IP + ":" + Data.Network.Port;
                Reconnect();
            }
            IP = ip;
            sender = new BinaryWriter(client.GetStream());
            Data.Network.ServerIP = "Connected to " + IP + ":" + Data.Network.Port;
            IsRunning = true;
            Receive();
            SendObject(Data.PlayerInfos.InternetID);
        }
        public static void RunGame()
        {
            Thread Stats = new Thread(UpdateStatistics);
            Stats.Name = "Stats Update";
            Stats.Start();
            SendObject(new PlayerName(Data.PlayerInfos.Name));
        }
        #endregion

        #region GameListing
        public static void RefreshLANServerList()
        {
            if (!IsRefreshing)
            {
                Thread Refreshing = new Thread(StartRefreshing);
                Refreshing.Start();
                Refreshing.Name = "LAN Games Refreshing";
            }
        }
        public static void RefreshInternetServerList()
        {
            //SyncClient
            Thread OnlineSynchronyzation = new Thread(SyncClient.ConnectAsClient);
            OnlineSynchronyzation.Name = "OnlineSynchronyzation";
            OnlineSynchronyzation.Start();
            //Thread.Sleep(1000);
        }
        private static void StartRefreshing()
        {
            IsRefreshing = true;
            ServerList.Clear();
            byte[] GamePlusTwo;
            try
            {
                BroadcastListener = new UdpClient(4269);
            }
            catch
            {
                throw new Exception("Another Socket is alreading using the port 4269!");
            }
            EndPoint = new IPEndPoint(IPAddress.Any, 4269);
            for (int i = 0; i < 10; i++)
            {
                GamePlusTwo = BroadcastListener.Receive(ref EndPoint);
                Game NewGame = (Game)Serialize.Serializer.ByteArrayToObject(ShortenArray(GamePlusTwo, 2));
                if (!IsGameAlreadyInList(NewGame))
                    ServerList.Add(NewGame);
            }
            BroadcastListener.Close();
            IsRefreshing = false;
        }
        private static bool IsGameAlreadyInList(Game ThisGame)
        {
            foreach (Game game in ServerList)
            {
                if (game.CreationTime == ThisGame.CreationTime)
                    return true;
            }
            return false;
        }
        #endregion

        #region Errors
        private static void Reconnect()
        {
            Data.Network.ServerIP += ", trying to reconnect to it.";
            Thread.Sleep(500);
            Start(IP);
        }
        private static void DisconnectedError()
        {
            IsRunning = false;
            Data.Network.ServerIP = "Lost connection with " + IP + ":" + Data.Network.Port;
            Reconnect();
        }
        #endregion

        #region IO
        public static void SendObject(object obj)
        {
            if (obj is Orders.Tasks.ProductItem)
                UnitsTrained++;
            else if (obj is Orders.Tasks.Die)
                UnitsLost++;
            if (IsRunning)
            {
                byte[] ObjectTable = Serialize.Serializer.ObjectToByteArray(obj);
                byte[] tWithLength = new byte[3 + ObjectTable.Length];
                tWithLength[0] = (byte)(ObjectTable.Length / 256);
                tWithLength[1] = (byte)(ObjectTable.Length % 256);
                //Todo: 3rd byte: types
                //Todo: Server sends disconnecting message
                // 1: Le serveur désérialisera
                if (obj is string ||
                    obj is DisconnectionMessage ||
                    obj is PlayerName ||
                    obj is Orders.Tasks.ProductItem ||
                    obj is int ||
                    obj is Statistics)
                    tWithLength[2] = 1;
                // Sinon le serveur ne désérialisera pas
                else //Le serveur fera suivre a tous les clients sans deserialiser
                    tWithLength[2] = 0;
                ObjectTable.CopyTo(tWithLength, 3);
                //try
                //{
                try
                {
                    sender.Write(tWithLength);
                }
                catch
                {
                    Data.Network.ServerIP = "Host crashed, game is finished.";
                    chat.InsertMsg("Server crashed!");
                    IsRunning = false;
                }
            }
            else
                chat.InsertMsg("You are not connected!");
            //}
            //catch
            //{
            //    chat.InsertMsg("You are not connected!");
            //    DisconnectedError();
            //}
        }
        private static void Receive()
        {
            BinaryReader NetReader = new BinaryReader(client.GetStream());
            int lenght = 0;
            byte[] data = new byte[0];
            while (IsRunning)
            {
                if (/*client.Connected && */client.Available != 0)
                {
                    lenght = 0;
                    try
                    {
                        lenght = NetReader.ReadByte() * 256 + NetReader.ReadByte();
                        NetReader.ReadByte();
                        data = NetReader.ReadBytes(lenght);
                    }
                    catch
                    {
                        DisconnectedError();
                    }
                    ProcessObject(data);
                }
                else
                    Thread.Sleep(5);
            }
            NetReader.Close();
        }
        #endregion

        #region Compute
        private static void ProcessObject(byte[] ObjectTable)
        {
            object o;
            //try
            //{
            o = Serialize.Serializer.ByteArrayToObject(ObjectTable);
            //}
            //catch
            //{
            //if (IsRunning)
            //{
            //    o = ""; //throw new Exception("Error during deserialization!");
            //}
            //return;
            //}
            if (o is string)
                chat.InsertMsg((string)o);
            else if (o is int)
            {
                int Id = (int)o;
                if (!Server.IsRunning)
                {
                    string[][] result = SyncClient.SendSQLRequest("SELECT `name`, `avatar` FROM `user` WHERE `id` = " + Id);
                    PlayerOpponent.name = result[0][0];
                    PlayerOpponent.avatarURL = result[0][1];
                    PlayerOpponent.rank = Int32.Parse(((string[][])SyncClient.SendSQLRequest("SELECT COUNT(*) FROM `user` WHERE `score` > (SELECT `score` FROM `user` WHERE `id` = " + Id + ")"))[0][0]) + 1;
                    PlayerOpponent.victories = Int32.Parse(((string[][])SyncClient.SendSQLRequest("SELECT COUNT(*) FROM `game` WHERE `winnerID`=" + Id))[0][0]);
                    PlayerOpponent.defeats = Int32.Parse(((string[][])SyncClient.SendSQLRequest("SELECT COUNT(*) FROM `game` WHERE `winnerID`!=" + Id + " AND (`P1ID`=" + Id + " OR `P2ID`=" + Id + ")"))[0][0]);
                    player1.Reset(PlayerOpponent.name, PlayerOpponent.avatarURL, PlayerOpponent.rank, PlayerOpponent.victories, PlayerOpponent.defeats, false);
                }
            }
            else if (o is Network.Orders.Task)
            {
                if (o is Orders.Tasks.ReSync)
                {
                    ReSyncOrder = (Orders.Tasks.ReSync)o;
                    MustReSync = true;
                }
                if (o is Orders.Tasks.CheatSpawn)
                {
                    Orders.Tasks.CheatSpawn Orders = (Orders.Tasks.CheatSpawn)o;
                    new PlayGame.Tasks.CheatSpawn(null, 0, PlayGame.Items.Loader.LoadItem(Orders.child, Orders.IdPlayer), new Point(Orders.x, Orders.y)).Initialize();
                }
                else if (o is Orders.Tasks.Move)
                {
                    Orders.Tasks.Move Orders = (Orders.Tasks.Move)o;
                    PlayGame.Item i = PlayGame.Map.ListItems.FirstOrDefault(s => s.PrimaryId == Orders.parentPrimaryKey);
                    if (i != null)
                        i.AddTask(
                             new PlayGame.Tasks.Move(i, new Point(Orders.x, Orders.y)), Orders.overrideTask, false
                        );
                }
                else if (o is Orders.Tasks.ProductItem)
                {
                    Orders.Tasks.ProductItem Orders = (Orders.Tasks.ProductItem)o;
                    PlayGame.Item i = PlayGame.Map.ListItems.FirstOrDefault(s => s.PrimaryId == Orders.parentPrimaryKey),
                                  it;
                    if (i != null)
                    {
                        it = PlayGame.Items.Loader.LoadItem(Orders.child, i.IdPlayer);
                        if (it != null)
                        {
                            it.PrimaryId = Orders.nextId;
                            i.AddTask(
                                new PlayGame.Tasks.ProductItem(
                                    i,
                                    Data.GameInfos.timeCreatingItem[Orders.child],
                                    it,
                                    new Point(Orders.x, Orders.y),
                                    Orders.closestFreePosition,
                                    Orders.canBeDeletedDuringProduction,
                                    Orders.moveToBuildingPoint

                                ),
                                Orders.overrideTask,
                                false
                            );
                        }
                    }
                }
                else if (o is Orders.Tasks.Hold)
                {
                    Orders.Tasks.Hold Orders = (Orders.Tasks.Hold)o;
                    PlayGame.Item i = PlayGame.Map.ListItems.FirstOrDefault(s => s.PrimaryId == Orders.parentPrimaryKey);
                    if (i != null)
                    {
                        i.AddTask(
                            new PlayGame.Tasks.Hold(
                                i
                            ),
                            true,
                            false
                        );
                    }
                }
                else if (o is Orders.Tasks.Die)
                {
                    Orders.Tasks.Die Orders = (Orders.Tasks.Die)o;
                    PlayGame.Item i = PlayGame.Map.ListItems.FirstOrDefault(s => s.PrimaryId == Orders.parentPrimaryKey);
                    if (i != null)
                        i.AddTask(
                            new PlayGame.Tasks.Die(
                                i
                                ),
                                true,
                                false
                                );
                }
                else if (o is Orders.Tasks.Attack)
                {
                    Orders.Tasks.Attack Orders = (Orders.Tasks.Attack)o;
                    PlayGame.Item i = PlayGame.Map.ListItems.FirstOrDefault(s => s.PrimaryId == Orders.parentPrimaryKey);
                    if (i != null)
                        i.AddTask(
                              new PlayGame.Tasks.Attack(
                                  i,
                                  PlayGame.Map.ListItems[Orders.enemyPos]
                                  ),
                                  false,
                                  false
                                  );
                }
            }
            else if (o is GetId)
                Data.Network.IdPlayer = ((GetId)o).Id;
            else if (o is DisconnectionMessage)
            {
                Data.Network.ServerIP = "Host decided to quit, game is finished.";
                chat.InsertMsg("Server gracefully stopped: " + ((DisconnectionMessage)o).reason);
                IsRunning = false;
            }

            // On gère pas l'objet reçue, exception
            //else
            //throw new Exception("Unknown object Type: " + o.GetType());
        }
        private static byte[] ShortenArray(byte[] Long, int StartIndex)
        {
            byte[] Short = new byte[Long.Length - StartIndex];
            for (int i = 0; i < Long.Length - StartIndex; i++)
                Short[i] = Long[i + StartIndex];
            return Short;
        }
        private static void UpdateStatistics()
        {
            while (IsRunning)
            {
                SendObject(new Statistics(Data.PlayerInfos.InternetID, PlayGame.Map.PlayerResources, GetArmyValue()));
                Thread.Sleep(30000);
            }
        }
        private static int GetArmyValue()
        {
            Resource r = new Resource(0, 0, 0, 0, 0);
            foreach (PlayGame.Item i in PlayGame.Map.ListItems.Where(it => it.IdPlayer == Data.Network.IdPlayer))
            {
                r += Data.GameInfos.CostsItems[i.GetType()];
            }
            return (int)(r.Electricity + 5*r.Graphene + 2*r.Hydrogen + r.Iron + 3*r.Steel);
        }
        #endregion

        public static void Stop()
        {
            //Todo: Send disconecting message
            SendObject(new DisconnectionMessage("Requested by user"));
            IsRunning = false;
            // BroadcastListener.Close();
            sender.Close();
            client.Close();
        }
    }
}