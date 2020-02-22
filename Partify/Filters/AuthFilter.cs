using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Partify.Filters
{
    public class AuthFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext.HttpContext.Session["loggedUser"] == null)
            {
                string url = filterContext.HttpContext.Request.Url.PathAndQuery;
                filterContext.Result = new RedirectResult("/Home/Login?url=" + url);
                return;
            }
        }
    }
}