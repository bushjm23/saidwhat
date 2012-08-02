using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SaidWhat.Engine.Site.Controllers
{
    public class BaseController : Controller
    {
        public bool IsJsonRequest
        {
            get 
            {
                return
                    HttpContext.Request.ContentType == "application/json"
                    || HttpContext.Request.QueryString["contenttype"] == "application/json";
            
            }
        }
    }
}
