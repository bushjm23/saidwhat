using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SaidWhat.Common.MySql
{    
    public class ConnectionPool
    {
        public ConnectionPool()
        {
            Pool = new Dictionary<string, ConnectionBuilder>();
        }

        Dictionary<string, ConnectionBuilder> Pool { get; set; }

        public ConnectionBuilder GetConnectionBuilder(string name)
        {
            if (!Pool.ContainsKey(name))
            {
                var connectionBuilder = ConnectionBuilderFactory.CreateConfigurationBuilder(name);
                Pool.Add(name, connectionBuilder);
            }            
            
            return Pool[name];
        }
    }
}
