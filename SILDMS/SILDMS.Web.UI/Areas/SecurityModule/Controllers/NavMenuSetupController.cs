using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SILDMS.Web.UI.Areas.SecurityModule.Controllers
{
    public class NavMenuSetupController : Controller
    {
        // GET: SecurityModule/NavMenuSetup
        [SILAuthorize]
        public ActionResult Index()
        {
            return View();
        }
    }
}