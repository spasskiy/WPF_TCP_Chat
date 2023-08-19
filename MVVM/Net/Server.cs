using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;

namespace Chat19Aug.MVVM.Net
{
    internal class Server
    {
        TcpClient client;
        string _IP = "127.0.0.1";
        int port = 31338;
        public Server() 
        {
            client = new TcpClient();
        }
        public void ConnectToServer(string username)
        {
            if(!client.Connected)
            {
                client.Connect(_IP, port);
            }
        }
    }
}
