using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using Chat19Aug.MVVM.Net.IO;

namespace Chat19Aug.MVVM.Net
{
    internal class Server
    {
        TcpClient client;        
        public PacketReader packetReader;
        public event Action connectedEvent;
        public event Action msgReceivedEvent;
        public event Action userDisconectEvent;
        public Server() 
        {
            client = new TcpClient();
        }
        public void ConnectToServer(string username, string IP, int port)
        {
            if(!client.Connected)
            {
                client.Connect(IP, port);
                packetReader = new PacketReader(client.GetStream());

                if(!string.IsNullOrEmpty(username))
                {
                    var connectPacket = new PacketBuilder();
                    connectPacket.WriteOpCode(0);
                    connectPacket.WriteString(username);
                    client.Client.Send(connectPacket.GetPacketBytes());
                }
                ReadPackets();

                
            }
        }

        private void ReadPackets()
        {
            Task.Run(() =>
            {
                while(true)
                {
                    var opcode = packetReader.ReadByte();
                    switch (opcode)
                    {
                        case 1: 
                            connectedEvent?.Invoke();
                            break;
                        case 5:
                            msgReceivedEvent?.Invoke();
                            break;
                        case 10:
                            userDisconectEvent?.Invoke();
                            break;
                        default:
                            Console.WriteLine("Default in ReadPackets!");
                            break;
                    }
                }
            });
        }

        public void SendMessageToServer(string message)
        {
            var massagePacket = new PacketBuilder();
            massagePacket.WriteOpCode(5);
            massagePacket.WriteString(message);
            client.Client.Send(massagePacket.GetPacketBytes());
        }
    }
}
