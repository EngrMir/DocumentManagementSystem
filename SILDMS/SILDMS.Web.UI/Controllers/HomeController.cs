using SILDMS.Utillity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SILDMS.Web.UI.Areas.SecurityModule;
using System.Web.UI;

namespace SILDMS.Web.UI.Controllers
{
    public class HomeController : Controller
    {
        [SILAuthorize]
        [OutputCache(Duration = 1000, VaryByParam = "none", Location = OutputCacheLocation.Client, NoStore = true)]
        public ActionResult DocumentScanning()
        {
            return View();
        }

        [SILAuthorize]
        [OutputCache(Duration = 1000, VaryByParam = "none", Location = OutputCacheLocation.Client, NoStore = true)]
        public ActionResult OriginalDocSearching()
        {     
            return View();
        }

        [SILAuthorize]
        [OutputCache(Duration = 1000, VaryByParam = "none", Location = OutputCacheLocation.Client, NoStore = true)]
        public ActionResult VersioningOriginalDoc()
        {          
            return View();
        }

        [SILAuthorize]
        [OutputCache(Duration = 1000, VaryByParam = "none", Location = OutputCacheLocation.Client, NoStore = true)]
        public ActionResult VersionDocSearching()
        {
            return View();
        }

        [SILAuthorize]
        [OutputCache(Duration = 1000, VaryByParam = "none", Location = OutputCacheLocation.Client, NoStore = true)]
        public ActionResult VersioningVersionedDoc()
        {
            return View();
        }

        [OutputCache(Duration = 1000, VaryByParam = "none", Location = OutputCacheLocation.Server)]
        public ActionResult NotFound() 
        {          
            return View();
        }

        [OutputCache(Duration = 1000, VaryByParam = "none", Location = OutputCacheLocation.Server)]
        public ActionResult BadRequest()
        {
            return View();
        }
         [SILAuthorize]
        public ActionResult DocumentDistribution()
        {
            return View();
        }

         public ActionResult DocumentSharing()
         {
             return View();
         }
        
    }

}