using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using SocketProto;
using SocketServer.Servers;

namespace SocketServer.Controller
{
    class ControllerManager
    {
        //private static ControllerManager instance;
        //public static ControllerManager Instance
        //{
        //    get
        //    {
        //        if (instance == null)
        //        {
        //            instance = new ControllerManager();
        //        }
        //    }
        //}
        private Dictionary<RequestCode, BaseController> controllerDict;
        private Server server;
        public ControllerManager(Server server)
        {
            this.server = server;
            controllerDict = new Dictionary<RequestCode, BaseController>();
            controllerDict.Add(UserController.Instance.GetRequestCode, UserController.Instance);
            controllerDict.Add(RoomController.Instance.GetRequestCode, RoomController.Instance);
        }
        public void HandleRequest(MainPack pack,Client client)
        {
            if(controllerDict.TryGetValue(pack.Requestcode,out BaseController baseController))
            {
                string mname = pack.Actioncode.ToString();
                Console.WriteLine(mname);
                MethodInfo method = baseController.GetType().GetMethod(mname);
                if (method == null)
                {
                    Console.WriteLine("未找到方法");
                    return;
                }
                object[] obj = new object[] {server,client,pack};
                object ret = method.Invoke(baseController, obj);
                if (ret != null)
                {
                    client.SendMessage(ret as MainPack);
                }
            }
            else
            {
                Console.WriteLine("没有找到对应的controller处理");
            }
        }
    }
}
