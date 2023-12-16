using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SocketServer.Servers;

namespace SocketServer
{
    class MainSocket
    {
        static void Main(string[] args)
        {
            Server server = new Server(6666);
            Console.ReadKey();
        }
    }
}
