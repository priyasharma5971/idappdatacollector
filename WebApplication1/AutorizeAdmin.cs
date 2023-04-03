using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Filters
{
    public class AutorizeUser : System.Web.Mvc.ActionFilterAttribute, System.Web.Mvc.IActionFilter
    {
        public string usertype;
        public override void OnActionExecuting(System.Web.Mvc.ActionExecutingContext filterContext)
        {
            var q = filterContext.RouteData.Values;
            if (q.Values.ToList()[0].ToString() == "Customers")
            {
                if (HttpContext.Current.Session["customerid"]==null)
                {
                    filterContext.Result = new System.Web.Mvc.RedirectToRouteResult(new System.Web.Routing.RouteValueDictionary
                {
                    {"Controller", "Account"},
                    {"Action", "Login"}
                });
                }
                else
                {

                }
            }
            else if((q.Values.ToList()[0].ToString() == "Users") || (q.Values.ToList()[0].ToString() == "Home" && q.Values.ToList()[1].ToString() == "Index"))
            {
                if (HttpContext.Current.Session["userid"] == null)
                {
                    filterContext.Result = new System.Web.Mvc.RedirectToRouteResult(new System.Web.Routing.RouteValueDictionary
                {
                    {"Controller", "Account"},
                    {"Action", "Login"}
                });
                }
                else
                {

                }
            }

            else
            {
                if (HttpContext.Current.Session["schoolid"] == null)
                {
                    filterContext.Result = new System.Web.Mvc.RedirectToRouteResult(new System.Web.Routing.RouteValueDictionary
                {
                    {"Controller", "Account"},
                    {"Action", "Login"}
                });
                }
                else
                {

                }
            }
            base.OnActionExecuting(filterContext);
        }
    }
}