using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace itrying
{
    class SqlConnection
    {

        public static MySqlConnection
                //делаем подключение
                 GetDBConnection(string host, int port, string database, string username, string password)
        {
            String connString = "Server=" + host + ";Database=" + database
                + ";port=" + port + ";User Id=" + username + ";password=" + password + ";Allow User Variables=True";

            MySqlConnection conn = new MySqlConnection(connString);

            return conn;
        }

    }
}