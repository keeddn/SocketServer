using SocketProto;
using SocketServer.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace SocketServer.Servers
{
    internal class Client
    {
        Socket socket;
        Message message;
        Server server;
        public string name;
        public Client(Socket client, Server server)
        {
            socket = client;
            message = new Message();
            this.server = server;
            StartReceive();
        }
        void StartReceive()
        {
            socket.BeginReceive(message.Byte, message.EndIndex, message.Remsize, SocketFlags.None, ReceiveCallBack, null);
        }
        void ReceiveCallBack(IAsyncResult async)
        {
            try
            {
                if (socket == null || socket.Connected == false) return;
                int len = socket.EndReceive(async);
                Console.WriteLine("receive:" + len);
                if (len == 0)
                {
                    //StartReceive();
                    return;
                }
                message.ReadMessage(len,HandleMessage);
                StartReceive();
            }
            catch
            {
                Console.WriteLine("接受异常");
            }
            
        }
        public void SendMessage(MainPack pack)
        {
            socket.Send(Message.PackData(pack));
        }
        private void HandleMessage(MainPack pack)
        {
            server.HandleMessage(pack, this);
        }
    }
}
