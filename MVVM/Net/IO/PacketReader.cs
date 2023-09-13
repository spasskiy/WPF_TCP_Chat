using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Chat19Aug.MVVM.Net.IO
{
    internal class PacketReader : BinaryReader
    {
        private NetworkStream ns;
        public PacketReader(NetworkStream ns) : base(ns)
        {
            this.ns = ns;
        }
        public string ReadMessage()
        {
            byte[] buffer;
            var length = ReadInt32();
            buffer = new byte[length];
            ns.Read(buffer, 0, length);
            var msg = Encoding.Unicode.GetString(buffer);
            return msg;
        }
    }
}
