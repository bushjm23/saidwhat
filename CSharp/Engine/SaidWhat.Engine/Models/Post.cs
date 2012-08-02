using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SaidWhat.Common.Infrastructure;
using SaidWhat.Engine.Mappers;

namespace SaidWhat.Engine.Models
{
    public class Post
    {
        //////////////////////////////////////////////////////////////////////
        /// CONSTRUCTORS /////////////////////////////////////////////////////
        //////////////////////////////////////////////////////////////////////
        #region Constructors

        /// <summary>
        /// Default constructor
        /// </summary>
        public Post()
        {
            this.Created = DateTime.UtcNow;
        }

        /// <summary>
        /// Full property constructor
        /// </summary>        
        public Post(int idpost, DateTime created, string message)
        {
            this.IdPost = idpost;
            this.Created = created;
            this.Message = message;
        }

        #endregion

        //////////////////////////////////////////////////////////////////////
        /// PROPERTIES ///////////////////////////////////////////////////////
        //////////////////////////////////////////////////////////////////////
        #region Properties

        /// <summary>
        /// Gets or sets the unique identifier
        /// </summary>
        public int IdPost { get; set; }

        /// <summary>
        /// Gets or sets the creation date
        /// </summary>
        public DateTime Created { get; set; }

        /// <summary>
        /// Gets or sets the post message
        /// </summary>
        public string Message { get; set; }

        #endregion

        //////////////////////////////////////////////////////////////////////
        /// METHODS //////////////////////////////////////////////////////////
        //////////////////////////////////////////////////////////////////////
        #region Methods

        #endregion

        //////////////////////////////////////////////////////////////////////
        /// ACTIVE RECORD ////////////////////////////////////////////////////
        //////////////////////////////////////////////////////////////////////
        #region Active Record

        /// <summary>
        /// Saves the instance
        /// </summary>
        public void Save()
        {
            var id = Container.Instance.Get<IPostMapper>().Save(this);
            if (id > 0)
                this.IdPost = id;
        }

        /// <summary>
        /// Deletes the instance
        /// </summary>
        public void Delete()
        {
            Container.Instance.Get<IPostMapper>().Delete(this.IdPost);
            this.IdPost = 0;
        }

        #endregion

        //////////////////////////////////////////////////////////////////////
        /// ACTIVE RECORD STATICS ////////////////////////////////////////////
        //////////////////////////////////////////////////////////////////////
        #region Active Record Statics

        /// <summary>
        /// Finds the instance by it's unique identifier
        /// </summary>
        /// <param name="idpost">Unique identifier</param>
        /// <returns>Returns the instance or null if not found</returns>
        public static Post FindById(int idpost)
        {
            return Container.Instance.Get<IPostMapper>().FindById(idpost);
        }

        /// <summary>
        /// Returns a list of posts
        /// </summary>
        /// <param name="page">Page</param>
        /// <param name="pageSize">Page size</param>
        /// <param name="total">Total results found</param>
        /// <returns>List of posts</returns>
        public static List<Post> Find(uint page, uint pageSize, out uint total)
        {
            return Container.Instance.Get<IPostMapper>().Find(page, pageSize, out total);
        }

        #endregion
    }
}
