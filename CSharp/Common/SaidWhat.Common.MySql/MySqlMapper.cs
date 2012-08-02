using System;
using System.Collections.Generic;
using System.Data;
using MySql.Data.MySqlClient;
using SaidWhat.Common.Infrastructure;

namespace SaidWhat.Common.MySql
{
    /// <summary>
    /// Base class for MySql data access
    /// </summary>
    public abstract class MySqlMapper : IMapper
    {
        ////////////////////////////////////////////////////////////
        /// CONSTRUCTORS ///////////////////////////////////////////
        ////////////////////////////////////////////////////////////
        #region Constructors

        /// <summary>
        /// Creates a MySqlMapper using the default conneciton string from the application settings
        /// </summary>
        public MySqlMapper()            
        {
            this.ConnectionPool = new ConnectionPool();
        }

        #endregion

        /////////////////////////////////////////////////////////////
        /// PROPERTIES //////////////////////////////////////////////
        /////////////////////////////////////////////////////////////
        #region Properties

        /// <summary>
        /// Gets or sets the ConnectionManagers
        /// </summary>
        public ConnectionPool ConnectionPool { get; set; }

        #endregion Properties


        /////////////////////////////////////////////////////////////
        /// METHODS /////////////////////////////////////////////////
        /////////////////////////////////////////////////////////////
        #region Methods

        /// <summary>
        /// Provides generic execute scalar wrapper that returns the scalar object value
        /// </summary>
        /// <param name="cmdText">The sql text or stored proc name</param>
        /// <param name="commandType">The type of the command</param>
        /// <param name="parameters">The parameters that need to be added</param>        
        /// <returns>Returns an object of the scalar result</returns>
        public virtual object ExecuteScalar(string cmdText, string connectionName, CommandType commandType, MySqlParameter[] parameters)
        {
            using (MySqlConnection conn = ConnectionPool.GetConnectionBuilder(connectionName).CreateConnection())
            using (MySqlCommand command = new MySqlCommand(cmdText, conn))
            {
                if (parameters != null)
                    command.Parameters.AddRange(parameters);
                command.CommandType = commandType;
                command.Connection.Open();
                return command.ExecuteScalar();
            }
        }

        /// <summary>
        /// Provides a generic ExecuteNonQuery wrapper
        /// </summary>
        /// <param name="cmdText">The sql text or stored proc name</param>
        /// <param name="commandType">The type of the command</param>
        /// <param name="parameters">The parameters that need to be added</param>        
        /// <returns>Returns the number of rows affected</returns>
        public virtual int ExecuteNonQuery(string cmdText, string connectionName, CommandType commandType, MySqlParameter[] parameters)
        {
            ulong lastInsertId;
            return ExecuteNonQuery(cmdText, connectionName, commandType, parameters, out lastInsertId);
        }

        /// <summary>
        /// Provides a generic ExecuteNonQuery wrapper that returns the LastInsertId
        /// </summary>
        /// <param name="cmdText">The sql text or stored proc name</param>
        /// <param name="commandType">The type of the command</param>
        /// <param name="parameters">The parameters that need to be added</param>        
        /// <param name="lastInsertId">Out parameter for the last inserted identifier</param>
        /// <returns>Returns the number of rows affected</returns>
        public virtual int ExecuteNonQuery(string cmdText, string connectionName, CommandType commandType, MySqlParameter[] parameters, out ulong lastInsertId)
        {
            using (MySqlConnection conn = ConnectionPool.GetConnectionBuilder(connectionName).CreateConnection())
            using (MySqlCommand command = new MySqlCommand(cmdText, conn))
            {
                if(parameters != null)
                    command.Parameters.AddRange(parameters);
                command.CommandType = commandType;
                command.Connection.Open();
                int result = command.ExecuteNonQuery();
                lastInsertId = (ulong)command.LastInsertedId;
                return result;
            }
        }        

        /// <summary>
        /// Provides generic data reader execution that returns a single object
        /// </summary>
        /// <param name="cmdText">The sql text or stored proc name</param>
        /// <param name="commandType">The type of the command</param>
        /// <param name="parameters">The parameters that need to be added</param>
        /// <param name="parser">The parsing method that should execute for each row</param>
        /// <returns></returns>
        public virtual T ExecuteReader<T>(string cmdText, string connectionName, CommandType commandType, MySqlParameter[] parameters, Func<MySqlDataReader, T> parser)
        {
            T result;
            using (MySqlConnection conn = ConnectionPool.GetConnectionBuilder(connectionName).CreateConnection())
            using (MySqlCommand command = new MySqlCommand(cmdText, conn))
            {
                if (parameters != null)
                    command.Parameters.AddRange(parameters);
                command.CommandType = commandType;
                command.Connection.Open();
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    reader.Read();
                    result = parser(reader);
                }
            }
            return result;
        }

        /// <summary>
        /// Provides generic data reader execution that returns a List of objects
        /// </summary>
        /// <param name="cmdText">The sql text or stored proc name</param>
        /// <param name="commandType">The type of the command</param>
        /// <param name="parameters">The parameters that need to be added</param>
        /// <param name="parser">The parsing method that should execute for each row</param>
        /// <returns></returns>
        public virtual List<T> ExecuteReaderList<T>(string cmdText, string connectionName, CommandType commandType, MySqlParameter[] parameters, Func<MySqlDataReader, T> parser)
        {
            List<T> results = new List<T>();
            using (MySqlConnection conn = ConnectionPool.GetConnectionBuilder(connectionName).CreateConnection())
            using (MySqlCommand command = new MySqlCommand(cmdText, conn))
            {
                if (parameters != null)
                    command.Parameters.AddRange(parameters);
                command.CommandType = commandType;
                command.Connection.Open();
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        T result = parser(reader);
                        if (result != null)
                            results.Add(result);
                    }
                }
            }
            return results;
        }

        /// <summary>
        /// Provides generic data reader execution that returns a List of objects
        /// </summary>
        /// <param name="cmdText">The sql text or stored proc name</param>
        /// <param name="commandType">The type of the command</param>
        /// <param name="parameters">The parameters that need to be added</param>
        /// <param name="parser">The parsing method that should execute for each row</param>
        /// <param name="rowcount">Outputs the row count found through the FOUND_ROWS MySql function</param>
        /// <returns></returns>
        public virtual List<T> ExecuteReaderList<T>(string cmdText, string connectionName, CommandType commandType, MySqlParameter[] parameters, Func<MySqlDataReader, T> parser, out uint rowcount)
        {
            List<T> results = new List<T>();
            using (MySqlConnection conn = ConnectionPool.GetConnectionBuilder(connectionName).CreateConnection())
            using (MySqlCommand command = new MySqlCommand(cmdText, conn))
            {
                if (parameters != null)
                    command.Parameters.AddRange(parameters);
                command.CommandType = commandType;
                command.Connection.Open();
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        T result = parser(reader);
                        if (result != null)
                            results.Add(result);
                    }
                }
                command.CommandType = CommandType.Text;
                command.CommandText = "SELECT FOUND_ROWS() `rowcount`;";
                rowcount = Convert.ToUInt32(command.ExecuteScalar());
            }
            return results;
        }

        /// <summary>
        /// Provides generic data reader execution that returns an enumerable collection for large record sets
        /// </summary>
        /// <param name="cmdText">The sql text or stored proc name</param>
        /// <param name="commandType">The type of the command</param>
        /// <param name="parameters">The parameters that need to be added</param>
        /// <param name="parser">The parsing method that should execute for each row</param>
        /// <returns></returns>
        public virtual IEnumerable<T> ExecuteReaderEnumerator<T>(string cmdText, string connectionName, CommandType commandType, MySqlParameter[] parameters, Func<MySqlDataReader, T> parser)
        {
            using (MySqlConnection conn = ConnectionPool.GetConnectionBuilder(connectionName).CreateConnection())
            using (MySqlCommand command = new MySqlCommand(cmdText, conn))
            {
                if (parameters != null)
                    command.Parameters.AddRange(parameters);
                command.CommandType = commandType;
                command.Connection.Open();
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        T result = parser(reader);
                        if (result != null)
                            yield return result;
                    }
                }
            }
        }        

        #endregion Methods
    }   
}