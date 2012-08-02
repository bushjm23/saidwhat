using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;

namespace SaidWhat.Common.MySql
{
    public class ConnectionBuilder
    {
        public ConnectionBuilder(string name, string connectionString)
        {
            this.ConnectionName = name;
            this.ConnectionString = connectionString;
        }

        public string ConnectionName { get; set; }
        public string ConnectionString { get; set; }

        public MySqlConnection CreateConnection()
        {
            return new MySqlConnection(ConnectionString);
        }
    }
}
