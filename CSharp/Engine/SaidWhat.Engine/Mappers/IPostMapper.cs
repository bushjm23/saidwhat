using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SaidWhat.Engine.Models;

namespace SaidWhat.Engine.Mappers
{
    /// <summary>
    /// Data mapping service for Posts
    /// </summary>
    public interface IPostMapper
    {
        /// <summary>
        /// Saves the instance
        /// </summary>
        /// <param name="instance">Instance to save</param>
        /// <returns>Returns the new id for insertions, updates return 0</returns>
        int Save(Post instance);

        /// <summary>
        /// Deletes the instance
        /// </summary>
        /// <param name="idpost">ID of instance to delete</param>
        void Delete(int idpost);

        /// <summary>
        /// Finds the post by its identifier
        /// </summary>
        /// <param name="idpost">Identifier</param>
        /// <returns>The post instance or null if not found</returns>
        Post FindById(int idpost);

        /// <summary>
        /// Finds a list of posts
        /// </summary>
        /// <param name="page">Page</param>
        /// <param name="pageSize">Page size</param>
        /// <param name="total">Total results found</param>
        /// <returns>List of posts</returns>
        List<Post> Find(uint page, uint pageSize, out uint total);
    }
}
