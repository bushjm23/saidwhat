using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace SaidWhat.Common.MySql
{
    public class ConnectionBuilderFactory
    {
        public static ConnectionBuilder CreateConfigurationBuilder(string name)
        {            
            var connectionString = ConfigurationManager.ConnectionStrings[name];
            return new ConnectionBuilder(name, connectionString.ConnectionString);                            
        }
    }
}
