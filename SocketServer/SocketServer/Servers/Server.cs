using SocketProto;
using SocketServer.Controller;
using SocketServer.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;

using System.Threading.Tasks;

namespace SocketServer.Servers
{
    internal class Server
    {
        Socket socket;
        List<Client> Clients;
        ControllerManager controllerManager;

        public Server(int port)
        {
            controllerManager = new ControllerManager(this);
            Clients = new List<Client>();
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            socket.Bind(new IPEndPoint(IPAddress.Any,port));
            socket.Listen(0);
            StartAccept();

        }
        private void StartAccept()
        {
            Console.WriteLine("StartAccept");
            socket.BeginAccept(AcceptCallBack, null);
        }
        void AcceptCallBack(IAsyncResult async)
        {
            Console.WriteLine("AcceptCallBack");

            Socket client = socket.EndAccept(async);
            Clients.Add(new Client(client,this));
            StartAccept();
        }
        public void HandleMessage(MainPack pack, Client client)
        {
            controllerManager.HandleRequest(pack,client);
        }
    }
}
