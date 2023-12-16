using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SocketProto;
using SocketServer.DAO;
using SocketServer.Servers;

namespace SocketServer.Controller
{
    class UserController:BaseController
    {
        private static UserController instance;
        public static UserController Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new UserController();
                }
                return instance;
            }
        }
        public UserController()
        {
            requestCode = RequestCode.User;
        }
        public MainPack Register(Server server,Client client,MainPack pack)
        {
            if (UserDataManager.Instance.Register(pack))
            {
                pack.Returncode = ReturnCode.Succeed;
            }
            else
            {
                pack.Returncode = ReturnCode.Fail;
            }
            return pack;
        }
        public MainPack Login(Server server, Client client, MainPack pack)
        {
            if (UserDataManager.Instance.Login(pack,client))
            {
                pack.Returncode = ReturnCode.Succeed;
            }
            else
            {
                pack.Returncode = ReturnCode.Fail;
            }
            return pack;
        }
    }
}
