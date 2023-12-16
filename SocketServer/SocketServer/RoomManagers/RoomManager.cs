using MySql.Data.MySqlClient;
using SocketProto;
using SocketServer.DAO;
using SocketServer.Servers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocketServer.RoomManagers
{
    class RoomManager
    {
        private static RoomManager instance;
        private Dictionary<string,Room> rooms=new Dictionary<string, Room>();
        public static RoomManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new RoomManager();
                }
                return instance;
            }
        }
        private RoomManager() { }
        public void DeleteRoom(string name)
        {
            rooms.Remove(name);
        }
        public bool CreateRoom(Client client,ref MainPack pack)
        {
            Console.WriteLine(pack.Roompack[0].Roomname);
            if (rooms.TryGetValue(pack.Roompack[0].Roomname,out Room room1))
            {
                return false;
            }
            Room room = new Room(ref pack,client);
            rooms.Add(room.Name, room);
            return true;
        }
        public MainPack FindRoom(MainPack pack)
        {
            foreach(var room in rooms)
            {
                Console.WriteLine(room.Key);
                pack.Roompack.Add(room.Value.GetRoomInfo());
            }
            return pack;
        }
        public MainPack JoinRoom(MainPack pack,Client client)
        {
            string name = pack.Roompack[0].Roomname;
            Console.WriteLine(name + "  " + rooms);
            if (rooms.TryGetValue(name,out Room room))
            {
                if (room.CanJoin(client,ref pack))
                {
                    pack.Returncode = ReturnCode.Succeed;
                }
                else
                {
                    pack.Returncode = ReturnCode.Fail;
                }
            }
            else
            {
                pack.Returncode = ReturnCode.NotFound;
            }
            return pack;
        }
        public MainPack LeaveRoom(MainPack pack, Client client)
        {
            string name = pack.Roompack[0].Roomname;
            Console.WriteLine(name + "  " + rooms.ElementAt(0).Key);
            if (rooms.TryGetValue(name, out Room room))
            {
                
                if (room.LeaveRoom(client))
                {
                    pack.Returncode = ReturnCode.Succeed;
                }
                else
                {
                    pack.Returncode = ReturnCode.Fail;
                }
            }
            else
            {
                pack.Returncode = ReturnCode.NotFound;
            }
            return pack;
        }
        public MainPack SendMessage(MainPack pack,Client clinet)
        {
            string roomName = pack.Roompack[0].Roomname;
            if (rooms.TryGetValue(roomName, out Room room))
            {
                pack.Returncode = ReturnCode.Succeed;
                room.SendMessage(pack, clinet);
            }
            else
            {
                pack.Returncode = ReturnCode.NotFound;
            }
            pack.Str = "我:" + pack.Str+"\n";
            Console.WriteLine(pack.Str+" "+pack.Returncode);
            return pack;
        }
        public void Ready(MainPack pack, Client clinet)
        {
            string roomName = pack.Roompack[0].Roomname;
            if (rooms.TryGetValue(roomName, out Room room))
            {
                pack.Returncode = ReturnCode.Succeed;
                room.Reday(pack);
            }
            else
            {
                pack.Returncode = ReturnCode.NotFound;
            }
            //return pack;
        }
    }
}
