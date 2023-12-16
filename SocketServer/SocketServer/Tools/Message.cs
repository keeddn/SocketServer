using Google.Protobuf;
using SocketProto;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SocketServer.Servers;

namespace SocketServer.Tools
{
    class Message
    {
        private byte[] bytes = new byte[1024];
        private int endIndex,startIndex;
        public byte[] Byte
        {
            get
            {
                return bytes;
            }
        }
        public int StartIndex
        {
            get
            {
                return startIndex;
            }
        }
        public int EndIndex
        {
            get
            {
                return endIndex;
            }
        }
        public int Remsize
        {
            get
            {
                return bytes.Length - endIndex;
            }
        }
        public void ReadMessage(int len,Action<MainPack> HandleMes)
        {
            Console.WriteLine("ReadMessage");
            endIndex += len;
            if (endIndex <= 4) return;
            while (true)
            {
                int count = BitConverter.ToInt32(bytes, 0);
                
                MainPack Pack = (MainPack)MainPack.Descriptor.Parser.ParseFrom(bytes, 4, count);
                HandleMes(Pack);
                Array.Copy(bytes, 4 + count, bytes, 0, endIndex - count - 4);
                endIndex -= count + 4;
                if (endIndex <= 4) break;
            }
        }
        public static byte[] PackData(MainPack pack)
        {
            byte[] data= pack.ToByteArray();
            byte[] head = BitConverter.GetBytes(data.Length);
            return head.Concat(data).ToArray();
        }
    }
}
