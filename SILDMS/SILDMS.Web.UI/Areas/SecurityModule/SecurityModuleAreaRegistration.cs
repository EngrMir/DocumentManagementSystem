using System.Web.Mvc;

namespace SILDMS.Web.UI.Areas.SecurityModule
{
    public class SecurityModuleAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "SecurityModule";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "SecurityModule_default",
                "SecurityModule/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional },
                  new[] { "SILDMS.Web.UI.Areas.SecurityModule.Controllers" }
            );
        }
    }
}