using SILDMS.Model.SecurityModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;

namespace SILDMS.Web.UI.Areas.SecurityModule
{
    public class SILAuthorize : System.Web.Mvc.AuthorizeAttribute  
    {
            protected override bool AuthorizeCore(HttpContextBase httpContext)
            {
                if (httpContext == null)
                {
                    throw new ArgumentNullException("httpContext");
                }
                IPrincipal user = httpContext.User;
                var identity = user.Identity as ClaimsIdentity;
                var claims = identity.Claims;
                if (HttpContext.Current.Session != null)
                {
                    List<SEC_Menu> _menu = (List<SEC_Menu>)HttpContext.Current.Session["SEC_Menu"];
                    if (_menu != null)
                    {
                        string requestUrl = HttpContext.Current.Request.Url.AbsolutePath;
                        foreach (var item in _menu)
                        {
                            if (item.MenuUrl.Equals(requestUrl))
                            {                              
                                return true;
                            }                           
                        }
                    } 
                }
                return false;
            }  
           protected override void HandleUnauthorizedRequest(AuthorizationContext ctx)  
           {
                ctx.Result = new HttpUnauthorizedResult();
                ctx.HttpContext.Response.StatusCode = 403;                  
           }  
    }
}