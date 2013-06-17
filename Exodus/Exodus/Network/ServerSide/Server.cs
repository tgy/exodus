using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Net.Sockets;
using System.Net;
using System.IO;
using System.Threading;
using Exodus.Network.Orders;
using System.Net.NetworkInformation;

namespace Exodus.Network.ServerSide
{
    public static class Server
    {
        public static Game TheGame;
        private static TcpListener server;
        public static bool IsRunning;
        public static bool GameRunned;
        private static Thread Client_reading;
        private static Thread Observer_reading;
        private static Thread SyncObservers = null;
        private static int PrimaryKey = 0;
        private static bool GameHasChanged = false;
        private static TwoPStatistics TPStats;
        public static GUI.Items.PlayerInfosLaunching player2;
        #region Start
        public static void Start()
        {
            server = new TcpListener(IPAddress.Any, Data.Network.Port);
            try
            {
                server.Start();
            }
            catch
            {
                Data.Network.Server = "Server not started, another program is\nalready using the port " + Data.Network.Port;
                return;
            }
            TheGame = new Game(LocalIP(), "     Hephaistos-42");
            Data.Network.GameStartTime = DateTime.Now;
            IsRunning = true;
            GameRunned = false;
            //SyncClient
            Thread OnlineSynchronyzation = new Thread(SyncClient.ConnectAsServer);
            OnlineSynchronyzation.Name = "OnlineSynchronyzation";
            OnlineSynchronyzation.Start();
            //
            SyncObservers = new Thread(SyncObserversAuto);
            SyncObservers.Name = "Sync Observers";
            SyncObservers.Start();
            SClient Accepted;
            Thread BroadcastSignal = new Thread(Broadcast);
            BroadcastSignal.Name = "BroadcastSignal";
            BroadcastSignal.Start();
            while (IsRunning)
            {
                TcpClient client;
                if (server.Pending())
                {
                    client = server.AcceptTcpClient();
                    //client is now connected!
                    Accepted = new SClient(client);
                    int index = IsSClientAlreadyConnected(Accepted);
                    if (index != -1)
                    {
                        Data.Network.ConnectedClients[index].Stop();
                        Data.Network.ConnectedClients.Remove(Accepted);
                        TheGame.NbPlayers--;
                        GameHasChanged = true;
                        //throw new Exception("Client " + Accepted.IP + " was already connected! (two clients started with the same IP?)");
                    }

                    Data.Network.ConnectedClients.Add(Accepted);
                    if (Data.Network.ConnectedClients.Count <= Data.Network.MaxPlayersInCurrentGame)
                    {
                        TheGame.NbPlayers++;
                        GameHasChanged = true;
                        Client_reading = new Thread(new ParameterizedThreadStart(ReceivePlayer));
                        Client_reading.Name = "Receiver (" + Accepted.IP + ") (Player)";
                        Client_reading.Start(Accepted);
                        Accepted.Id = Data.Network.ConnectedClients.Count;
                        SendToClient(Accepted, new GetId(Accepted.Id));
                        SendToClient(Accepted, Data.PlayerInfos.InternetID);
                    }
                    else
                    {
                        TheGame.NbObservers++;
                        GameHasChanged = true;
                        Observer_reading = new Thread(new ParameterizedThreadStart(ReceiveObserver));
                        Observer_reading.Name = "Receiver (" + Accepted.IP + ") (Observer)";
                        Observer_reading.Start(Accepted);
                        SendToClient(Accepted, new GetId(0));
                        SendToClient(Accepted, Data.PlayerInfos.InternetID);
                    }
                }
                else
                    Thread.Sleep(100);
            }
        }
        public static void RunGame()
        {
            GameRunned = true;
            TPStats = new TwoPStatistics();
            Thread ReSyncAuto = new Thread(ReSyncTimer);
            ReSyncAuto.Name = "ReSyncAuto";
            ReSyncAuto.Start();
            Thread CheckWinner = new Thread(CheckWin);
            CheckWinner.Name = "CheckWin";
            CheckWinner.Start();
        }
        #endregion

        #region IO
        private static void ReceivePlayer(object Sclient)
        {
            SClient client = (SClient)Sclient;
            BinaryReader Receiver = new BinaryReader(client.Client.GetStream());
            int lenght = 0;
            byte[] data = null;
            while (IsRunning)
            {
                if (client.Connected && client.Client.Available != 0)
                {
                    try
                    {
                        lenght = Receiver.ReadByte() * 256 + Receiver.ReadByte();
                    }
                    catch
                    {
                        //throw new Exception("Couldn't read message lenght. Client disconnected?");
                    }

                    try
                    {
                        data = Receiver.ReadBytes(lenght + 1);
                    }
                    catch
                    {
                        //throw new Exception("The lenght of the message was not correct! (Server stopped during the writing?)");
                    }
                    if (data != null)
                        ProcessPlayerObject(client, data);
                }
                else
                    Thread.Sleep(5);
            }
            Receiver.Close();
        }
        private static void ReceiveObserver(object Sclient)
        {
            SClient client = (SClient)Sclient;
            BinaryReader Receiver = new BinaryReader(client.Client.GetStream());
            int lenght;
            byte[] data;
            while (IsRunning)
            {
                if (client.Connected && client.Client.Available != 0)
                {
                    try
                    {
                        lenght = Receiver.ReadByte() * 256 + Receiver.ReadByte();
                    }
                    catch
                    {
                        throw new Exception("Couldn't read message lenght. Client disconnected?");
                    }

                    try
                    {
                        data = Receiver.ReadBytes(lenght + 1);
                    }
                    catch
                    {
                        throw new Exception("The lenght of the message was not correct! (Server stopped during the writing?)");
                    }
                    ProcessObserverObject(client, data);
                }
                else
                    Thread.Sleep(5);
            }
            Receiver.Close();
        }
        private static void SendToAll(object ToSend)
        {
            byte[] data = SerialyzeObject(ToSend);
            for (int i = 0; i < Data.Network.ConnectedClients.Count; i++)
                SendToClient(Data.Network.ConnectedClients[i], data);
        }
        private static void SendToAll(byte[] ToSend)
        {
            for (int i = 0; i < Data.Network.ConnectedClients.Count; i++)
                SendToClient(Data.Network.ConnectedClients[i], ToSend);
        }
        private static void SendToClient(SClient client, object ToSend)
        {
            SendToClient(client, SerialyzeObject(ToSend));
        }
        private static void SendToClient(SClient client, byte[] data)
        {
            try
            {
                new BinaryWriter(client.Client.GetStream()).Write(data);
            }
            catch
            {
                SClientCrash(client);
            }
        }
        private static void Broadcast()
        {
            UdpClient BroadCaster = new UdpClient();
            BroadCaster.EnableBroadcast = true;
            byte[] temp = new byte[0]; //= Serialize.Serializer.ObjectToByteArray(TheGame);
            byte[] message = new byte[0];
            NetworkInterface[] NetInterfaces = NetworkInterface.GetAllNetworkInterfaces();
            while (!GameHasChanged)
                Thread.Sleep(10);
            while (IsRunning)
            {
                if (GameHasChanged)
                {
                    temp = Serialize.Serializer.ObjectToByteArray(TheGame);
                    GameHasChanged = false;
                    //message = new byte[temp.Length + 2];
                    //message[0] = (byte)(temp.Length / 256);
                    //message[1] = (byte)(temp.Length % 256);
                    //temp.CopyTo(message, 2);
                    SyncClient.SendGame(temp);
                }

                foreach (NetworkInterface adapter in NetInterfaces)
                {
                    UnicastIPAddressInformationCollection UnicastIPInfos = adapter.GetIPProperties().UnicastAddresses;
                    foreach (UnicastIPAddressInformation unicastIPAddressInformation in UnicastIPInfos)
                    {
                        if (unicastIPAddressInformation.Address.AddressFamily == AddressFamily.InterNetwork && unicastIPAddressInformation.IPv4Mask != null)
                        {
                            BroadCaster.Send(temp/*message*/, temp.Length, IPAddress.Broadcast.ToString(), 4269);
                        }
                    }
                }
                Thread.Sleep(100);
            }
            BroadCaster.Close();
        }
        #endregion

        #region Compute
        private static byte[] SerialyzeObject(object ToSerialyze)
        {
            byte[] objectTable = Serialize.Serializer.ObjectToByteArray(ToSerialyze);
            byte[] tWithLength = new byte[objectTable.Length + 3];
            tWithLength[0] = (byte)(objectTable.Length / 256);
            tWithLength[1] = (byte)(objectTable.Length % 256);
            //Todo: 3rd byte: types
            // Quoi qu'il arrive, le client désérialise ^^
            tWithLength[2] = 1;
            objectTable.CopyTo(tWithLength, 3);
            return tWithLength;
        }
        private static byte[] ShortenArray(byte[] Long, int StartIndex)
        {
            byte[] Short = new byte[Long.Length - StartIndex];
            for (int i = 0; i < Long.Length - StartIndex; i++)
                Short[i] = Long[i + StartIndex];
            return Short;
        }
        private static void ProcessPlayerObject(SClient client, byte[] ObjectTablePlusOne)
        {
            object o;
            switch (ObjectTablePlusOne[0])
            {
                case 1: // Le serveur désérialise
                    try
                    {
                        o = Serialize.Serializer.ByteArrayToObject(ShortenArray(ObjectTablePlusOne, 1));
                    }
                    catch
                    {
                        //SendToAll("Desync detected!");
                        //if (IsRunning)
                        throw new Exception("Error during deserialization!");
                        //else
                        //return;
                    }
                    break;

                default: // sinon on ne désérialise pas
                    byte[] Packet = new byte[ObjectTablePlusOne.Length + 2];
                    Packet[0] = (byte)((ObjectTablePlusOne.Length - 1) / 256);
                    Packet[1] = (byte)((ObjectTablePlusOne.Length - 1) % 256);
                    ObjectTablePlusOne.CopyTo(Packet, 2);
                    SendToAll(Packet);
                    return;
            }
            if (o is string)
                SendToAll("[" + DateTime.Now.ToShortTimeString() + "] <" + client.Name + "> " + (string)o);
            else if (o is LaunchGame)
                SendToAll(o);
            else if (o is DisconnectionMessage)
            {
                client.Stop();
                Data.Network.ConnectedClients.Remove(client);
                SendToAll(client.Name + " Disconnected gracefully: " + ((DisconnectionMessage)o).reason);
                TheGame.NbPlayers--;
                GameHasChanged = true;
            }
            else if (o is PlayerName)
            {
                if (client.Name == "Undefined")
                {
                    client.Name = ((PlayerName)o).name;
                    SendToAll("[" + DateTime.Now.ToShortTimeString() + "] * " + client.Name + " has joined the game.");
                }
                else
                    throw new Exception("Client " + client.Name + " sent his name two times!");
            }
            else if (o is Orders.Tasks.ProductItem)
            {
                Orders.Tasks.ProductItem order = (Orders.Tasks.ProductItem)o;
                order.nextId = PrimaryKey;
                PrimaryKey++;
                SendToAll(order);
            }
            else if (o is Orders.Tasks.CheatSpawn)
            {
                Orders.Tasks.CheatSpawn order = (Orders.Tasks.CheatSpawn)o;
                order.nextId = PrimaryKey;
                PrimaryKey++;
                SendToAll(order);
            }
            else if (o is int)
            {
                client.InternetID = (int)o;
                if (client.InternetID != Data.PlayerInfos.InternetID)
                {
                    client.SendInternetIDToGameManager();
                    string[][] result = SyncClient.SendSQLRequest("SELECT `name`, `avatar` FROM `user` WHERE `id` = " + client.InternetID);
                    if (result != null)
                    {
                        PlayerOpponent.name = result[0][0];
                        PlayerOpponent.avatarURL = result[0][1];
                        PlayerOpponent.rank = Int32.Parse(((string[][])SyncClient.SendSQLRequest("SELECT COUNT(*) FROM `user` WHERE `score` > (SELECT `score` FROM `user` WHERE `id` = " + client.InternetID + ")"))[0][0]) + 1;
                        PlayerOpponent.victories = Int32.Parse(((string[][])SyncClient.SendSQLRequest("SELECT COUNT(*) FROM `game` WHERE `winnerID`=" + client.InternetID))[0][0]);
                        PlayerOpponent.defeats = Int32.Parse(((string[][])SyncClient.SendSQLRequest("SELECT COUNT(*) FROM `game` WHERE `winnerID`!=" + client.InternetID + " AND (`P1ID`=" + client.InternetID + " OR `P2ID`=" + client.InternetID + ")"))[0][0]);
                        player2.Reset(PlayerOpponent.name, PlayerOpponent.avatarURL, PlayerOpponent.rank, PlayerOpponent.victories, PlayerOpponent.defeats, false);
                    }
                }
            }
            else if (o is Statistics)
                TPStats.AddStatistic((Statistics)o);
            else if (o is Task)
                SendToAll(o);
            /*else
                throw new Exception("Dah hell is that object?");*/
        }
        private static void ProcessObserverObject(SClient client, byte[] ObjectTablePlusOne)
        {
            object o;
            if (ObjectTablePlusOne[0] == 1)
            {
                try
                {
                    o = Serialize.Serializer.ByteArrayToObject(ShortenArray(ObjectTablePlusOne, 1));
                }
                catch
                {
                    //SendToAll("Desync detected!");
                    return;
                }
                //Un Observateur doit-il pouvoir envoyer des messages de chat?

                //if (o is string)
                //    SendToAll("[" + DateTime.Now.ToShortTimeString() + "] <" + client.Name + "> " + (string)o);
                /*else */
                if (o is DisconnectionMessage)
                {
                    client.Stop();
                    Data.Network.ConnectedClients.Remove(client);
                    SendToAll(client.Name + " Disconnected gracefully: " + ((DisconnectionMessage)o).reason);
                    TheGame.NbObservers--;
                    GameHasChanged = true;
                }
                else if (o is LaunchGame)
                    SendToAll(o);
                else if (o is PlayerName)
                {
                    if (client.Name == "Undefined")
                    {
                        client.Name = ((PlayerName)o).name;
                        SendToAll("[" + DateTime.Now.ToShortTimeString() + "] * " + client.Name + " has joined the game as an observer!.");
                    }
                    else
                        throw new Exception("Client " + client.Name + " sent his name two times!");
                }
                else if (o is int)
                {
                    client.InternetID = (int)o;
                }
                //else
                //    throw new Exception("This action is forbidden!"); ;
            }
        }
        private static void Resync()
        {
            SendToAll(new Orders.Tasks.ReSync(PlayGame.Map.ListPassiveItems, PlayGame.Map.ListItems));
        }
        private static void ReSyncTimer()
        {
            Thread.Sleep(100);
            while (IsRunning)
            {
                Resync();
                Thread.Sleep(5000);
            }
        }
        private static void SyncObserversAuto()
        {
            while (IsRunning && !GameRunned)
            {
                UpdaterObservers<string> r = new UpdaterObservers<string>();
                for (int i = Data.Network.MaxPlayersInCurrentGame; i < Data.Network.ConnectedClients.Count && r.Count < 3; i++)
                    r.Add(Data.Network.ConnectedClients[i].Name);
                SendToAll(r);
                Thread.Sleep(1000);
            }
        }
        private static void CheckWin()
        {
            while (IsRunning && GameRunned)
            {
                Thread.Sleep(1000);
                if (Data.Network.ConnectedClients.Count >= 2)
                {
                    int idP1, idP2;
                    if (Data.Network.ConnectedClients[0].Id == 1)
                    {
                        idP1 = Data.Network.ConnectedClients[0].InternetID;
                        idP2 = Data.Network.ConnectedClients[1].InternetID;
                    }
                    else
                    {
                        idP1 = Data.Network.ConnectedClients[1].InternetID;
                        idP2 = Data.Network.ConnectedClients[0].InternetID;
                    }
                    bool foundP1 = false, foundP2 = false;
                    for (int i = 0; i < PlayGame.Map.ListItems.Count && !(foundP1 && foundP2); i++)
                    {
                        if (PlayGame.Map.ListItems[i].IdPlayer == 1)
                            foundP1 = true;
                        else if (PlayGame.Map.ListItems[i].IdPlayer == 2)
                            foundP2 = true;
                    }
                    if (!foundP1 || !foundP2)
                    {
                        if (foundP1)
                        {
                            SendToAll(new WhoWon(idP1));
                            ThisClientWins((byte)idP1);
                        }
                        else if (foundP2)
                        {
                            SendToAll(new WhoWon(idP2));
                            ThisClientWins((byte)idP2);
                        }
                        else
                            SendToAll(new WhoWon((new Random().Next(2)) == 1 ? idP2 : idP1));
                        GameRunned = false;
                        //FIXME STOP SERVER
                    }
                }
                else if (Data.Network.ConnectedClients.Count == 1)
                {
                    SendToAll(new WhoWon(Data.Network.ConnectedClients[0].InternetID));
                    GameRunned = false;
                    ThisClientWins((byte)Data.Network.ConnectedClients[0].InternetID);
                    //FIXME STOP SERVER
                }

            }
        }
        #endregion

        private static void SClientCrash(SClient client)
        {
            client.Stop();
            Data.Network.ConnectedClients.Remove(client);
            SendToAll("[" + DateTime.Now.ToShortTimeString() + "] * " + client.Name + " has left the game. (Crash)");
            TheGame.NbPlayers--;
            GameHasChanged = true;
        }
        private static void ThisClientWins(byte id)
        {
            byte[] WinPacket = new byte[4];
            WinPacket[0] = 0;
            WinPacket[1] = 2;
            WinPacket[2] = 4;
            WinPacket[3] = id;
            SyncClient.SendDataToGameManagerAsServer(WinPacket);
        }
        private static int IsSClientAlreadyConnected(SClient client)
        {
            for (int i = 0; i < Data.Network.ConnectedClients.Count; i++)
            {
                if (Data.Network.ConnectedClients[i].IP.ToString().Split(':')[0] == client.IP.ToString().Split(':')[0])
                    return i;
            }
            return -1;
        }
        private static string LocalIP()
        {
            IPHostEntry Host = Dns.GetHostEntry(Dns.GetHostName());
            string LocalIP = "";
            foreach (IPAddress ip in Host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    LocalIP = ip.ToString();
                }
            }
            if (LocalIP == "")
                throw new Exception("Couldn't get local IP address!");
            return LocalIP;
        }
        private static void KillAllSClients()
        {
            for (int i = 0; i < Data.Network.ConnectedClients.Count; i++)
                Data.Network.ConnectedClients[i].Stop();
            Data.Network.ConnectedClients.Clear();
        }
        public static void Stop()
        {
            //Todo: Send disconnecting message
            SendToAll(new DisconnectionMessage("Requested by host"));
            IsRunning = false;
            PrimaryKey = 0;
            KillAllSClients();
            if (TPStats != null)
                TPStats.Reset();
            server.Stop();
            SyncClient.Stop();
        }
    }
}