using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Threading;

namespace Exodus.Network.ServerSide
{
    public class SClient
    {
        public TcpClient Client;
        public bool Connected
        {
            get { return _Connected; }
        }
        public string IP;
        public int Id = -1;
        public int InternetID = -1;
        public string Name = "Undefined";
        private bool _Connected;
        //private Thread ConnectionStatus;
        //private bool ShouldStop;

        public SClient(TcpClient client)
        {
            Client = client;
            IP = Client.Client.RemoteEndPoint.ToString();
            _Connected = true;
            //ShouldStop = false;
            //ConnectionStatus = new Thread(UpdateConnectionStatus);
            //ConnectionStatus.Start();
            //ConnectionStatus.Name = "UpdateConnectionStatus(" + IP.ToString() + ")";
        }

        public void SendInternetIDToGameManager()
        {
            byte[] msg = new byte[4];
            msg[0] = 0;
            msg[1] = 2;
            msg[2] = 3;
            msg[3] = (byte)InternetID;
            SyncClient.SendDataToGameManagerAsServer(msg);
        }

        //private void UpdateConnectionStatus()
        //{
        //    while (!_Connected && !ShouldStop)
        //    {
        //        Thread.Sleep(500);
        //    }
        //
        //    byte[] buff = new byte[1];
        //    while (_Connected)
        //    {
        //        try
        //        {
        //            // Detect if client disconnected
        //            if (Client.Client.Connected && !Client.Client.Poll(-1, SelectMode.SelectWrite)/* && Client.Client.Receive(buff, SocketFlags.Peek) == 0*/)
        //            {
        //                // Client disconnected
        //                _Connected = false;
        //            }
        //            Thread.Sleep(50);
        //        }
        //        catch
        //        {
        //            //Client.Client.Connected was false but Client.Client.Poll(...) was executed;
        //        }
        //    }
        //}

        public void Stop()
        {
            //ShouldStop = true;
            _Connected = false;
            Client.Close();
        }
    }
    #region Trash
    //class SClient
    //{
    //    public TcpClient Client;
    //    public bool Connected;
    //    public string IP;
    //    public int Id = -1;
    //    public string Name = "Undefined";
    //    //public bool running;
    //    private Thread ConnectionStatus;
    //
    //    public SClient()
    //    {
    //        Connected = false;
    //        //running = false;
    //        Client = new TcpClient();
    //        ConnectionStatus = new Thread(UpdateConnectionStatus);
    //        ConnectionStatus.Start();
    //    }
    //
    //    public void NameUpdateThread()
    //    {
    //        ConnectionStatus.Name = "UpdateConnectionStatus(" + IP.ToString() + ")";
    //    }
    //
    //    private void UpdateConnectionStatus()
    //    {
    //        while (!Connected)
    //        {
    //            Thread.Sleep(500);
    //        }
    //
    //        //while (running)
    //        byte[] buff = new byte[1];
    //        while (Connected)
    //        {
    //            Thread.Sleep(100);
    //            // Detect if client disconnected
    //            if (Client.Client.Connected && Client.Client.Poll(0, SelectMode.SelectRead) && Client.Client.Receive(buff, SocketFlags.Peek) == 0)
    //            {
    //                // Client disconnected
    //                Connected = false;
    //            }
    //        }
    //    }
    //
    //    public void GetRidOfThatSClient()
    //    {
    //        Connected = false;
    //        //running = false;
    //        Client.Close();
    //    }
    //}
    #endregion
}
