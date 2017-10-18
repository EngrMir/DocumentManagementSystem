using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace SILDMS.Web.UI
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");         
            routes.MapRoute(
                "Default",
                "{controller}/{action}/{id}",
                new { controller = "Account", action = "Login", id = UrlParameter.Optional },
                new[] { "SILDMS.Web.UI.Controllers" }
            ).DataTokens = new RouteValueDictionary(new { area = "SecurityModule" }); ;
        }

        
    }
}
