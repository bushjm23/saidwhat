using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using MySql.Data.MySqlClient;
using SaidWhat.Common.MySql;
using SaidWhat.Engine.Models;
using SaidWhat.Engine.Mappers;
using System.Data;

namespace SaidWhat.Engine.MySql
{
    public class PostMapper : MySqlMapper, IPostMapper
    {
        /////////////////////////////////////////////////////////////////////
        /// CONSTRUCTORS ////////////////////////////////////////////////////
        /////////////////////////////////////////////////////////////////////
        #region Constructors

        /// <summary>
        /// Base constructor
        /// </summary>
        public PostMapper()
        { }

        #endregion Constructors

        /////////////////////////////////////////////////////////////////////
        /// PARSERS /////////////////////////////////////////////////////////
        /////////////////////////////////////////////////////////////////////
        #region Parsers

        public static Post Parse(MySqlDataReader reader)
        {
            Post result = null;
            if (reader.HasRows)
            {
                result = new Post(
                    reader.GetInt32("idpost"),
                    reader.GetDateTime("created"),
                    reader.GetString("message"));
            }
            return result;
        }

        #endregion

        /////////////////////////////////////////////////////////////////////
        /// MAPPERS /////////////////////////////////////////////////////////
        /////////////////////////////////////////////////////////////////////        
        #region Mappers

        public int Save(Post instance)
        {
            string cmdText = @"
                insert into posts (idpost, created, message)
                values (@idpost, @created, @message)
                on duplicate key update
                    message=values(message)";

            ulong id;
            ExecuteNonQuery(
                cmdText,
                "SaidWhat.Primary",
                CommandType.Text,
                new MySqlParameter[] {
                    new MySqlParameter("@idpost", instance.IdPost),
                    new MySqlParameter("@created", instance.Created),
                    new MySqlParameter("@message", instance.Message)
                },
                out id);
            return (int)id;
                
        }

        public void Delete(int idpost)
        {
            string cmdText = @"
                delete from posts
                where idpost = @idpost";
            ExecuteNonQuery(
                cmdText,
                "SaidWhat.Primary",
                CommandType.Text,
                new MySqlParameter[] {
                    new MySqlParameter("@idpost", idpost)
                });
        }

        public Post FindById(int idpost)
        {
            string cmdText = @"
                select * 
                from posts
                where idpost = @idpost;";
            return ExecuteReader(
                cmdText,
                "SaidWhat.Primary",
                CommandType.Text,
                new MySqlParameter[] {
                    new MySqlParameter("@idpost", idpost)
                },
                Parse);
        }

        public List<Post> Find(uint page, uint pageSize, out uint total)
        {
            string cmdText = @"
                select *
                from posts
                order by created desc
                limit @start, @results";
            return ExecuteReaderList(
                cmdText,
                "SaidWhat.Primary",
                CommandType.Text,
                new MySqlParameter[] {
                    new MySqlParameter("@start", (page - 1) * pageSize),
                    new MySqlParameter("@results", pageSize)
                },
                Parse,
                out total);
        }

        #endregion
    }
}
