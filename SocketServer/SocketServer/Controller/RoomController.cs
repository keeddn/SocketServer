using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SocketProto;
using SocketServer.DAO;
using SocketServer.Servers;
using SocketServer.RoomManagers;

namespace SocketServer.Controller
{
    class RoomController : BaseController
    {
        private static RoomController instance;
        public static RoomController Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new RoomController();
                }
                return instance;
            }
        }
        private RoomController()
        {
            requestCode = RequestCode.Room;
        }
        public MainPack CreateRoom(Server server, Client client, MainPack pack)
        {

            if (RoomManager.Instance.CreateRoom(client, ref pack))
            {
                pack.Returncode = ReturnCode.Succeed;
            }
            else
            {
                pack.Returncode = ReturnCode.Fail;
            }
            return pack;
        }
        public MainPack FindRoom(Server server, Client client, MainPack pack)
        {
            pack = RoomManager.Instance.FindRoom(pack);
            if (pack.Roompack.Count != 0)
            {
                pack.Returncode = ReturnCode.Succeed;
            }
            else
            {
                pack.Returncode = ReturnCode.NotFound;
            }
            return pack;
        }
        public MainPack JoinRoom(Server server, Client client, MainPack pack)
        {
            return RoomManager.Instance.JoinRoom(pack, client);
        }
        public MainPack LeaveRoom(Server server, Client client, MainPack pack)
        {
            return RoomManager.Instance.LeaveRoom(pack, client);
        }
        public MainPack SendMessage(Server server, Client client, MainPack pack)
        {
            return RoomManager.Instance.SendMessage(pack, client);
        }
        public void Ready(Server server, Client client, MainPack pack)
        {
            RoomManager.Instance.Ready(pack, client);
        }
    }
}
