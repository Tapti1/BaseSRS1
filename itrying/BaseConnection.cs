using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace itrying
{
    class BaseConnection
    {
        //отсылаем данные для подключения
        public static MySqlConnection GetDBConnection()
        {
            string host = "127.0.0.1";
            int port = 3306;
            string database = "sys";
            string username = "root";
            string password = "P@ssw0rd";

            return SqlConnection.GetDBConnection(host, port, database, username, password);
        }

    }
}