using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chat19Aug.MVVM.Net.IO
{
    internal class PacketBuilder
    {
        MemoryStream ms;
        public PacketBuilder()
        {
            ms = new MemoryStream();
        }
        public void WriteOpCode(byte opcode)
        {
            ms.WriteByte(opcode);
        }
        public void WriteString(string msg)
        {
            var msgLength = msg.Length * 2;
            ms.Write(BitConverter.GetBytes(msgLength));
            ms.Write(Encoding.Unicode.GetBytes(msg));
        }
        public byte[] GetPacketBytes()
        {
            return ms.ToArray();
        }
    }
}
