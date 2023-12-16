using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SocketProto;
namespace SocketServer.Controller
{
    class BaseController
    {
        protected RequestCode requestCode = RequestCode.RequestNone;
        //private static T instance;
        //public static T Instance
        //{
        //    get
        //    {
        //        if (instance == null)
        //        {
        //            instance = new T();
        //        }
        //        return instance;
        //    }
        //}
        public BaseController()
        {
            requestCode = RequestCode.RequestNone;
        }
        public RequestCode GetRequestCode
        {
            get
            {
                return requestCode;
            }
        }
    }
}
