using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ScadaWeb.Web.Areas.Scada.Models
{
    public class ConnectionBaseModel
    {
        public long Id { set; get; }
    }
    public   class SqlServerModel: ConnectionBaseModel
    {
        public string Database
        {
            set; get;
        }
        public string Server
        {
            set; get;
        } = "127.0.0.1";
        public string UserId
        {
            set; get;
        } = "sa";
        public string Password
        {
            set; get;
        }
        public string DBTitle { set; get; }

        public string GetConnectionString()
        {

            return "server=" + Server + ";database=" + Database + ";integrated security=false;uid=" + UserId + ";pwd=" + Password;
        }
        public void SetConnectionString(string conn)
        {
            string[] arr = conn.Split(new char[1] { ';' }, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < arr.Length; i++)
            {
                if (arr[i].Trim() != "")
                {
                    string[] keys = arr[i].Split('=');
                    if (keys[0].Trim().ToLower() == "server" && keys.Length == 2)
                    {
                        Server = keys[1].Trim();
                    }
                    else if (keys[0].Trim().ToLower() == "database" && keys.Length == 2)
                    {
                        Database = keys[1].Trim();
                    }
                    else if (keys[0].Trim().ToLower() == "uid" && keys.Length == 2)
                    {
                        UserId = keys[1].Trim();
                    }
                    else if (keys[0].Trim().ToLower() == "pwd" && keys.Length == 2)
                    {
                        Password = keys[1].Trim();
                    }
                }
            }

        }
    }
    public   class OracleModel: ConnectionBaseModel
    {

        public string DataSource
        {
            set; get;
        } = "ORCL";
        public string UserId
        {
            set; get;
        } = "sa";
        public string Password
        {
            set; get;
        }
        public string DBTitle { set; get; }

        public string GetConnectionString()
        {

            return "Data Source=" + DataSource + ";User Id=" + UserId + ";Password=" + Password;
        }
        public void SetConnectionString(string conn)
        {
            string[] arr = conn.Split(new char[1] { ';' }, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < arr.Length; i++)
            {
                if (arr[i].Trim() != "")
                {
                    string[] keys = arr[i].Split('=');
                    if (keys[0].Trim().ToLower() == "data source" && keys.Length == 2)
                    {
                        DataSource = keys[1].Trim();
                    }
                    else if (keys[0].Trim().ToLower() == "user id" && keys.Length == 2)
                    {
                        UserId = keys[1].Trim();
                    }
                    else if (keys[0].Trim().ToLower() == "uid" && keys.Length == 2)
                    {
                        UserId = keys[1].Trim();
                    }
                    else if (keys[0].Trim().ToLower() == "password" && keys.Length == 2)
                    {
                        Password = keys[1].Trim();
                    }
                }
            }

        }
    }
    public   class MySqlModel: ConnectionBaseModel
    {

        public string Server
        {
            set; get;
        } = "127.0.0.1";
        public string Port
        {
            set; get;
        } = "3306";
        public string Database
        {
            set; get;
        } = "ScadaWeb";
        public string UserId
        {
            set; get;
        } = "root";
        public string Password
        {
            set; get;
        } = "123456";
        public string DBTitle { set; get; } = "我的链接";

        public string GetConnectionString()
        {

            return "server=" + Server + ";port=" + Port + ";uid=" + UserId + ";pwd=" + Password + ";database=" + Database + ";charset=utf8;default command timeout=300;";
        }
        public void SetConnectionString(string conn)
        {
            string[] arr = conn.Split(new char[1] { ';' }, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < arr.Length; i++)
            {
                if (arr[i].Trim() != "")
                {
                    string[] keys = arr[i].Split('=');
                    if (keys[0].Trim().ToLower() == "server" && keys.Length == 2)
                    {
                        Server = keys[1].Trim();
                    }
                    else if (keys[0].Trim().ToLower() == "port" && keys.Length == 2)
                    {
                        Port = keys[1].Trim();
                    }
                    else if (keys[0].Trim().ToLower() == "database" && keys.Length == 2)
                    {
                        Database = keys[1].Trim();
                    }
                    else if (keys[0].Trim().ToLower() == "uid" && keys.Length == 2)
                    {
                        UserId = keys[1].Trim();
                    }
                    else if (keys[0].Trim().ToLower() == "pwd" && keys.Length == 2)
                    {
                        Password = keys[1].Trim();
                    }
                }
            }

        }
    }
    public   class SybaseModel: ConnectionBaseModel
    {

        public string Server
        {
            set; get;
        } = "127.0.0.1";

        public string Database
        {
            set; get;
        } = "ScadaWeb";
        public string UserId
        {
            set; get;
        } = "root";
        public string Password
        {
            set; get;
        } = "123456";
        public string DBTitle { set; get; } = "我的链接";

        public string GetConnectionString()
        {
            string str = "Data Source=" + Server + ";" +
           "Initial Catalog=" + Database + ";" +
           "User id=" + UserId + ";" +
           "Password=" + Password + ";";
            return str;
        }
        public void SetConnectionString(string conn)
        {
            string[] arr = conn.Split(new char[1] { ';' }, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < arr.Length; i++)
            {
                if (arr[i].Trim() != "")
                {
                    string[] keys = arr[i].Split('=');
                    if (keys[0].Trim().ToLower() == "data source" && keys.Length == 2)
                    {
                        Server = keys[1].Trim();
                    }
                    else if (keys[0].Trim().ToLower() == "initial catalog" && keys.Length == 2)
                    {
                        Database = keys[1].Trim();
                    }
                    else if (keys[0].Trim().ToLower() == "user id" && keys.Length == 2)
                    {
                        UserId = keys[1].Trim();
                    }
                    else if (keys[0].Trim().ToLower() == "password" && keys.Length == 2)
                    {
                        Password = keys[1].Trim();
                    }
                }
            }

        }
    }
}