using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Exodus.Network.ServerSide;
using Exodus.Network.ClientSide;


namespace Exodus.Network
{
    public static class NetGame
    {
        public static bool SinglePlayerGame;
        private static Thread ClientThread;
        private static Thread ServerThread;

        public static void Start(string GameType)
        {
            switch (GameType)
            {
                case "C":
                    ClientThread = new Thread(new ParameterizedThreadStart(Client.Start));
                    ClientThread.Start(Data.Network.LastIP);
                    Data.Network.ServerIP = "Connecting to " + Data.Network.LastIP + ":" + Data.Network.Port;
                    ClientThread.Name = "Client";
                    Data.Network.Running_as = "Running as: Client";
                    Data.Network.Client = "Client:";
                    Data.Network.Server = "";
                    SinglePlayerGame = false;
                    break;
                case "SC":
                    ClientThread = new Thread(new ParameterizedThreadStart(Client.Start));
                    ClientThread.Start("127.0.0.1");
                    Data.Network.ServerIP = "Connecting to 127.0.0.1:" + Data.Network.Port;
                    Data.Network.Server = "Server (not synchronized): Connected clients:";
                    ServerThread = new Thread(Server.Start);
                    ServerThread.Start();
                    ServerThread.Name = "Server";
                    ClientThread.Name = "Client";
                    Data.Network.Running_as = "Running as: Server + Client";
                    Data.Network.Client = "Client:";
                    SinglePlayerGame = false;
                    break;
                case "No":
                    Data.Network.Running_as = "Running as: Singleplayer";
                    SinglePlayerGame = true;
                    return;
                default:
                    throw new Exception("OMFGWTFBBQ! The menu buttons may not be binded to the good NetGame functions (invalid arguments?)");
            }
        }

        public static void Stop()
        {
            if (ServerThread != null)
            {
                Server.Stop();
                ServerThread = null;
            }

            if (ClientThread != null)
            {
                Client.Stop();
                ClientThread = null;
            }
            SyncClient.Stop();
            Thread.Sleep(100);
            Client.RefreshLANServerList();
        }
    }
}