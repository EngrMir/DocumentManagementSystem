using SILDMS.InfraStructure;
using SILDMS.Model.DocScanningModule;
using SILDMS.Service.OwnerLevel;
using SILDMS.Utillity;
using SILDMS.Utillity.Localization;
using SILDMS.Web.UI.Areas.SecurityModule;
using SILDMS.Web.UI.Areas.SecurityModule.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.SessionState;
using System.Web.UI;

namespace SILDMS.Web.UI.Areas.DocScanningModule.Controllers
{

    public class OwnerLevelController : Controller
    {
        readonly IOwnerLevelService _ownerLevelService;
        private readonly ILocalizationService _localizationService;
        private ValidationResult respStatus = new ValidationResult();
        private string outStatus = string.Empty;
        private readonly string UserID = string.Empty;
        private string action = string.Empty;

        public OwnerLevelController(IOwnerLevelService repository, ILocalizationService localizationService)
        {
            this._ownerLevelService = repository;
            this._localizationService = localizationService;
            UserID = SILAuthorization.GetUserID();
        }

        [SILAuthorize]      
        [OutputCache(Duration = 2000, VaryByParam = "none", Location = OutputCacheLocation.Server)]
        public ActionResult Index()
        {          
             return View();
        }
        
        [HttpGet]
        [Authorize]   
        public async Task<dynamic> GetOwnerLevel(string ownerLevelId)
        {
            //UserID = SILAuthorization.GetUserID();
            List<DSM_OwnerLevel> obOwnerLevel = null;
            await Task.Run(() => _ownerLevelService.GetOwnerLevel("", UserID, out obOwnerLevel));
            var result = obOwnerLevel.Select(x => new
            {
                OwnerLevelID = x.OwnerLevelID,
                LevelName = x.LevelName,
                LevelAccess = x.LevelAccess,
                LevelSL = x.LevelSL,
                UserLevel = x.UserLevel,
                SetOn = x.SetOn,
                SetBy = x.SetBy,
                ModifiedOn = x.ModifiedOn,
                MdifiedBy = x.ModifiedBy,
                Status = x.Status.ToString()
            }).OrderByDescending(ob => ob.OwnerLevelID);

            return Json(new { Message = "", result }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]       
        [Authorize]
        [SILLogAttribute]
        public async Task<dynamic> AddOwnerLevel(DSM_OwnerLevel objOwnerLevel)
        {           
            if (ModelState.IsValid)
            {
                action = "add";
                objOwnerLevel.SetBy = UserID;
                objOwnerLevel.ModifiedBy = objOwnerLevel.SetBy;
                respStatus = await Task.Run(() => _ownerLevelService.AddOwnerLevel(objOwnerLevel, action, out outStatus));
                // Error handling.   
                return Json(new { Message = respStatus.Message, respStatus }, JsonRequestBehavior.AllowGet);
            }
            else 
            {
                respStatus = new ValidationResult("E404", _localizationService.GetResource("E404"));
            }
            return Json(new { Message = respStatus.Message, respStatus}, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        
        [Authorize]
        [SILLogAttribute]
        public async Task<dynamic> EditOwnerLevel(DSM_OwnerLevel obOwnerLevel)
        {
            if (ModelState.IsValid)
            {
                action = "edit";
                obOwnerLevel.SetBy = UserID;
                obOwnerLevel.ModifiedBy = obOwnerLevel.SetBy;
                respStatus = await Task.Run(() => _ownerLevelService.AddOwnerLevel(obOwnerLevel, action, out outStatus));
                return Json(new { Message = respStatus.Message, respStatus }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                respStatus = new ValidationResult("E404", _localizationService.GetResource("E404"));
            }
            return Json(new { Message = respStatus.Message, respStatus }, JsonRequestBehavior.AllowGet);
        }

 
        [HttpPost]
        [Authorize]
        [SILLogAttribute]
        public async Task<dynamic> DeleteOwnerLevel(DSM_OwnerLevel menu)
        {
            action = "delete";
            respStatus = await Task.Run(() => _ownerLevelService.AddOwnerLevel(menu, action, out outStatus));
            return Json(new { Message = respStatus.Message, respStatus }, JsonRequestBehavior.AllowGet);
        }
       
	}
}