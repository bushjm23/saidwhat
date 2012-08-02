using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SaidWhat.Engine.Models;
using SaidWhat.Engine.Site.Models;

namespace SaidWhat.Engine.Site.Controllers
{
    public class PostsController : Controller
    {        
        public ActionResult Index(uint page = 1, uint pageSize = 25)
        {
            uint total;
            var posts = Post.Find(page, pageSize, out total);

            var model = new PostIndexModel()
            {
                Page = page,
                PageSize = pageSize,
                TotalResults = total,
                Posts = posts
            };
            return View("index", model);
        }

        public ActionResult New()
        {
            var model = new Post();
            return View("new", model);
        }

        [HttpPost]
        public ActionResult Create(Post post)
        {
            if (!ModelState.IsValid)
            {
                return View("new", post);
            }             
            post.Save();
            return RedirectToAction("index");            
        }
    }
}
