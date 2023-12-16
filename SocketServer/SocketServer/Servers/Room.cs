using SocketProto;
using SocketServer.RoomManagers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocketServer.Servers
{
    internal class Room
    {
        private string roomName;
        private int nowPlayer, maxPlayer;
        private int state;
        private List<Client> players;
        private Dictionary<String, int> playerState = new Dictionary<string, int>();
        public string Name
        {
            get
            {
                return roomName;
            }
        }
        public Room(ref MainPack pack, Client client)
        {
            players = new List<Client>();
            roomName = pack.Roompack[0].Roomname;
            nowPlayer = 1;
            maxPlayer = pack.Roompack[0].Maxplayer;
            players.Add(client);
            playerState.Add(client.name, 0);
            pack =AddPlayerInfos(pack);
        }
        public RoomPack GetRoomInfo()
        {
            RoomPack pack = new RoomPack();
            pack.Roomname = roomName;
            pack.Nowplayer = nowPlayer;
            pack.Maxplayer = maxPlayer;
            pack.State = state;
            return pack;
        }
        public bool CanJoin(Client client,ref MainPack pack)
        {
            Console.WriteLine("CanJoin?");
            if (nowPlayer < maxPlayer&&!players.Contains(client))
            {
                nowPlayer++;
                players.Add(client);
                if (nowPlayer >= maxPlayer)
                {
                    state = 1;
                }
                playerState.Add(client.name, 0);
                pack = AddPlayerInfos(pack);
                MainPack p = CreateRefreshPack();
                BoardCast(client,p);
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool LeaveRoom(Client client)
        {
            Console.WriteLine("LeaveRoom?");
            if (players.Contains(client))
            {
                if (players[0] == client)
                {
                    RoomManager.Instance.DeleteRoom(roomName);
                    MainPack p = CreateLeavePack();
                    BoardCast(client, p);
                    return true;
                }
                players.Remove(client);
                playerState.Remove(client.name);
                nowPlayer--;
                MainPack pack = CreateRefreshPack();
                BoardCast(client,pack);
                return true;
            }
            return false;
        }
        public void SendMessage(MainPack pack, Client client)
        {
            MainPack p = new MainPack(pack);
            p.Str = client.name +":"+ p.Str + "\n";
            BoardCast(client, p);
        }
        public void Reday(MainPack pack)
        {
            Console.WriteLine("ready" + pack.Roompack[0].Playerinfos[0].Playername);
            MainPack p = new MainPack(pack);
            playerState[p.Roompack[0].Playerinfos[0].Playername] = 1-playerState[p.Roompack[0].Playerinfos[0].Playername];
            p= CreateRefreshPack();
            BoardCast(null,p);
        }
        private MainPack CreateLeavePack()
        {
            MainPack pack = new MainPack();
            pack.Actioncode = ActionCode.LeaveRoom;
            pack.Returncode = ReturnCode.Succeed;
            return pack;
        }
        private MainPack CreateRefreshPack()
        {
            MainPack pack = new MainPack();
            pack.Roompack.Add(new RoomPack());
            pack = AddPlayerInfos(pack);
            pack.Actioncode = ActionCode.RefreshPlayer;
            pack.Returncode = ReturnCode.Succeed;
            return pack;
        }
        private void BoardCast(Client c,MainPack pack)
        {
            foreach (Client client in players)
            {
                if (client == c) continue;
                client.SendMessage(pack);
            }
        }
        private MainPack AddPlayerInfos(MainPack pack)
        {
            foreach (Client client in players)
            {
                PlayerInfo playerInfo = new PlayerInfo();
                playerInfo.Playername = client.name;
                playerInfo.Playerstate = playerState[client.name];
                pack.Roompack[0].Playerinfos.Add(playerInfo);
            }
            return pack;
        }
    }
}
