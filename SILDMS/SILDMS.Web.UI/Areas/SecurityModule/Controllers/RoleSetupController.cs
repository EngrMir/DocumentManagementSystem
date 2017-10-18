using SILDMS.Model.SecurityModule;
using SILDMS.Utillity;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.SessionState;
using SILDMS.Service.RoleSetup;
using SILDMS.Utillity.Localization;
using SILDMS.Web.UI.Areas.SecurityModule.Models;

namespace SILDMS.Web.UI.Areas.SecurityModule.Controllers
{
    public class RoleSetupController : Controller
    {
        readonly IRoleSetupService _roleSetupService;
        
        private readonly ILocalizationService _localizationService;
        private ValidationResult respStatus = new ValidationResult();
        private string outStatus = string.Empty;
        private string action = "";
        private readonly string UserID = string.Empty;

        public RoleSetupController(IRoleSetupService roleService,
            ILocalizationService localizationService )
        {
            this._roleSetupService = roleService;
            UserID = SILAuthorization.GetUserID();
            this._localizationService = localizationService;
        }
        [SILAuthorize]    
        public ActionResult Index()
        {
            return View();
        }

    
        [Authorize]
        public async Task<dynamic> GetRole(string _OwnerID)
        {
            List<SEC_Role> rolesList = null;

            await Task.Run(() => _roleSetupService.GetRole(UserID, "", out rolesList));

            var result = (from r in rolesList
                          where r.OwnerID == _OwnerID 

                          select new SEC_Role
                          {
                              RoleID = r.RoleID,
                              RoleTitle = r.RoleTitle,
                              RoleDescription = r.RoleDescription,
                              RoleType = r.RoleType,
                              OwnerID = r.OwnerID,
                              UserLevel = r.UserLevel,
                              Status = r.Status
                          }).ToList();

            return Json(new { Msg = "", result }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [Authorize]
        [SILLogAttribute]
        public async Task<dynamic> AddRole(SEC_Role modelRole)
        {
            if (ModelState.IsValid)
            {
                action = "add";
                respStatus = await Task.Run(() => _roleSetupService.AddRole(modelRole, action, out outStatus));
                return Json(new { Message = respStatus.Message, respStatus }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors);
                respStatus = new ValidationResult("E404", _localizationService.GetResource("E404"));
            }
            return Json(new { Message = respStatus.Message, respStatus }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [Authorize]
        [SILLogAttribute]
        public async Task<dynamic> EditRole(SEC_Role modelRole)
        {
            if (ModelState.IsValid)
            {
                action = "edit";
                respStatus = await Task.Run(() => _roleSetupService.EditRole(modelRole, action, out outStatus));
                return Json(new { Message = respStatus.Message, respStatus }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                respStatus = new ValidationResult("E404", _localizationService.GetResource("E404"));
            }
            return Json(new { Message = respStatus.Message, respStatus }, JsonRequestBehavior.AllowGet);
        }
	}
}