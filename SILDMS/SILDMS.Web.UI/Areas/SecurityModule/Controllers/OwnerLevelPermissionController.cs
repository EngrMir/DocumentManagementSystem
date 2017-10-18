using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using System.Web.Mvc;
using System.Web.SessionState;

using SILDMS.Model.DocScanningModule;
using SILDMS.Model.SecurityModule;
using SILDMS.Service.Owner;
using SILDMS.Service.OwnerLevelPermission;
using SILDMS.Service.Users;
using SILDMS.Utillity;
using SILDMS.Utillity.Localization;
using SILDMS.Web.UI.Areas.SecurityModule.Models;

namespace SILDMS.Web.UI.Areas.SecurityModule.Controllers
{
    public class OwnerLevelPermissionController : Controller
    {
        private readonly IOwnerLevelPermissionService _OwnerLevelPermissionService ;
        private readonly IUserService _userService;
        private readonly IOwnerService _ownerService;
        private readonly ILocalizationService _localizationService;
        private ValidationResult respStatus = new ValidationResult();
        private string outStatus = string.Empty;
 
        private readonly string UserID = string.Empty;
        public OwnerLevelPermissionController(IOwnerLevelPermissionService ownerLevelPermissionService,
            IUserService userService, IOwnerService ownerService, ILocalizationService localizationService)
        {
            this._OwnerLevelPermissionService = ownerLevelPermissionService;
            this._userService = userService;
            this._ownerService = ownerService;
            this._localizationService = localizationService;
            UserID = SILAuthorization.GetUserID();
        }

        [SILAuthorize]
        public ActionResult Index()
        {
            return View();
        }

        [Authorize]
        public async Task<dynamic> GetEmployeeListForOwner(string _OwnerID)
        {
            List<SEC_User> userList = null;
            await Task.Run(() => _userService.GetAllUser("", _OwnerID, out userList));

            var result = (from r in userList
                where r.OwnerID == _OwnerID
                select new
                {
                    UserID = r.UserID,
                    EmployeeID = r.EmployeeID,
                    UserFullName = r.UserFullName,
                    UserDesignation = r.UserDesignation,
                    JobLocation = r.JobLocation,
                    UserNo = r.UserNo,
                    UserName = r.UserName,
                    UserRole = r.RoleTitle,
                    PermissionLevel = r.PermissionLevel,
                    RoleTitle = r.RoleTitle,
                    SupervisorLevel = r.SupervisorLevel,
                    UserLevel = r.UserLevelID,
                    EnableOwnerSecurity = r.SecurityStatus == "Enabled" ? "1" : "0"
                }).ToList();

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        [SILLogAttribute]
        public async Task<dynamic> SetOwnerLevelPermission(SEC_UserOwnerAccess model)
        {
            if (ModelState.IsValid)
            {
                respStatus = await Task.Run(() => _OwnerLevelPermissionService.
                    SetOwnerLevelPermission(model, out outStatus));

                List<UserWisePermittedOwner> userWisePermittedOwners = null;


                await Task.Run(() => _OwnerLevelPermissionService.GetUserWisePermittedOwnerList(
                    model.UserID, UserID, model.OwnerLevelAccessID, out userWisePermittedOwners));

                return Json(new { Message = respStatus.Message, respStatus, userWisePermittedOwners }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors);
                respStatus = new ValidationResult("E404", _localizationService.GetResource("E404"));
            }
            return Json(new { Message = respStatus.Message, respStatus }, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        public async Task<dynamic> GetOwnersForOwnerLevelPermission(string _UserID,
            string _OwnerLevelID)
        {
            
            List<UserWisePermittedOwner> userWisePermittedOwners = null;


            await Task.Run(() => _OwnerLevelPermissionService.GetUserWisePermittedOwnerList(_UserID, UserID, _OwnerLevelID, out userWisePermittedOwners));



            return Json( userWisePermittedOwners , JsonRequestBehavior.AllowGet);
        }
	}


    
}