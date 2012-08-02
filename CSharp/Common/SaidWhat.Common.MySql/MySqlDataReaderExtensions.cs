using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;

namespace Armslist.Common.MySql
{
    public static class MySqlDataReaderExtensions
    {
        /// <summary>
        /// Reads a value for a null column by performing a null check and returning the specified default value
        /// if it is null.  Otherwise, returns the read value from the getvalue function
        /// </summary>
        /// <typeparam name="T">Resulting type</typeparam>        
        /// /// <param name="columnName">The name of the column</param>
        /// <param name="getFunction">Gets the column's value from the reader</param>        
        /// <param name="defaultValue">The default value</param>
        /// <returns>The read value or the default value if column was null</returns>
        public static T GetNullColumn<T>(this MySqlDataReader reader, string columnName, Func<string, T> getFunction, T defaultValue)
        {
            var ordinal = reader.GetOrdinal(columnName);
            if (reader.IsDBNull(ordinal))
                return defaultValue;
            else
                return getFunction(columnName);
        }
    }
}
