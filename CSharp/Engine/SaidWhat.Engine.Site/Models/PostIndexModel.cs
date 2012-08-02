using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SaidWhat.Engine.Models;

namespace SaidWhat.Engine.Site.Models
{
    public class PostIndexModel
    {
        public List<Post> Posts { get; set; }

        public uint Page { get; set; }
        public uint PageSize { get; set; }
        public uint TotalResults { get; set; }
    }
}