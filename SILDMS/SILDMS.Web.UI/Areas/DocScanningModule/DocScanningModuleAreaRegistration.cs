using System.Web.Mvc;

namespace SILDMS.Web.UI.Areas.DocScanningModule
{
    public class DocScanningModuleAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "DocScanningModule";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "DocScanningModule_default",
                "DocScanningModule/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}