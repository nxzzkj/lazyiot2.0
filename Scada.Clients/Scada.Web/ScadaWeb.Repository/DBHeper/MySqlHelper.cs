
using System;
using System.Data.SQLite;


namespace ScadaWeb.Repository
{
    public class MySqlHelper
    {
         
        public static string datafile = Environment.GetEnvironmentVariable("LAZY_PATH", EnvironmentVariableTarget.Machine) + "IOProject\\IOCenterServer.station";
        public static string connectionString = "";
        public static System.Data.IDbConnection GetConnection()
        {

      
            var connection = new SQLiteConnection(connectionString);
            connection.Open();
            return connection;
        }
    }
}
