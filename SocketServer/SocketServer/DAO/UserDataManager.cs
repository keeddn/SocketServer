using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using SocketProto;
using SocketServer.Servers;

namespace SocketServer.DAO
{
    class UserDataManager
    {
        private static UserDataManager instance;
        private MySqlConnection mysqlCon;
        private string connstr = "database=socketgame;data source=localhost;user=root;password=TellmeWHY?;pooling=false;charset=utf8;port=3306";
        private UserDataManager()
        {
            try
            {
                mysqlCon = new MySqlConnection(connstr);
                mysqlCon.Open();
            }
            catch (Exception e)
            {
                Console.WriteLine("连接数据库失败: " + e.Message);
            }

        }
        public static UserDataManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new UserDataManager();
                }
                return instance;
            }
        }
        public bool Register(MainPack pack)
        {
            string name = pack.Loginpack.Name;
            string password = pack.Loginpack.Password;
            Console.WriteLine(name + "  " + password);
            try
            {
                string sql2 = "insert into `socketgame`.`userdata` (`name`,`password`) values('" + name + "','" + password + "')";
                Console.WriteLine(sql2);
                MySqlCommand comd2 = new MySqlCommand(sql2, mysqlCon);
                comd2.ExecuteNonQuery();
                Console.WriteLine("注册成功");
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine("注册失败");

                Console.WriteLine(e.Message);
                return false;
            }
        }
        public bool Login(MainPack pack,Client client)
        {
            string name = pack.Loginpack.Name;
            string password = pack.Loginpack.Password;
            Console.WriteLine(name + "  " + password);
            try
            {
                string sql = "select * from `socketgame`.`userdata` where name=" + name + " and password=" + password + ";";
                Console.WriteLine(sql);
                MySqlCommand comd = new MySqlCommand(sql, mysqlCon);
                MySqlDataReader reader = comd.ExecuteReader();
                if (reader.HasRows)
                {
                    Console.WriteLine("登录成功");
                    client.name= name;
                    Console.WriteLine(client.name);
                    reader.Close();
                    return true;
                }
                else
                {
                    Console.WriteLine("登录失败");
                    reader.Close();
                    return false;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("登录失败");
                Console.WriteLine(e.Message);
                return false;
            }
        }
    }
}
