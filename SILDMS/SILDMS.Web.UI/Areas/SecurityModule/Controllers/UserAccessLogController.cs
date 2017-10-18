using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Threading.Tasks;
using SILDMS.Model.DocScanningModule;
using SILDMS.Model.SecurityModule;
using SILDMS.Service.Owner;
using SILDMS.Service.OwnerLevel;
using SILDMS.Service.Server;
using SILDMS.Service.UserAccessLog;
using SILDMS.Service.Users;
using SILDMS.Utillity;
using SILDMS.Utillity.Localization;
using SILDMS.Web.UI.Areas.SecurityModule.Models;

namespace SILDMS.Web.UI.Areas.SecurityModule.Controllers
{
    public class UserAccessLogController : Controller
    {
        #region Fields
        private readonly string UserID = string.Empty;
        private readonly IOwnerService _ownerService;
        private readonly IOwnerLevelService _ownerLevelService;
        private readonly IUserService _userService;
        private readonly IUserAccessLogService _accessLogService;
        private readonly ILocalizationService _localizationService;
        private ValidationResult _respStatus;
        private string _outStatus = string.Empty;
        private string _action = string.Empty;

        #endregion

        #region Constructor

        public UserAccessLogController(IOwnerService ownerService, IOwnerLevelService ownerLevelService, IUserAccessLogService accessLogService,
            ILocalizationService localizationService, ValidationResult respStatus, IUserService userService)
        {
            _ownerService = ownerService;
            _ownerLevelService = ownerLevelService;
            _accessLogService = accessLogService;
            _localizationService = localizationService;
            _respStatus = respStatus;
            _userService = userService;
            UserID = SILAuthorization.GetUserID();
        }

        #endregion

        // GET: SecurityModule/UserAccessLog
        public ActionResult Index()
        {
            return View();
        }

        #region Retrive Data

        //[Authorize]
        [HttpGet]
        public async Task<dynamic> GetOwnerLevel(string id)
        {
            var ownerLevel = new List<DSM_OwnerLevel>();
            await Task.Run(() => _ownerLevelService.GetOwnerLevel(id, UserID, out ownerLevel));
            var result = ownerLevel.Where(ob => ob.Status == 1).Select(x => new
            {
                x.OwnerLevelID,
                x.LevelName
            }).OrderByDescending(ob => ob.LevelName);
            return Json(new { result, Msg = "" }, JsonRequestBehavior.AllowGet);
        }

        // Get Owner(s)
        //[Authorize]
        [HttpGet]
        public async Task<dynamic> GetOwners(string id)
        {
            var owner = new List<DSM_Owner>();
            await Task.Run(() => _ownerService.GetAllOwners("", UserID, out owner));
            var result = owner.Where(ob => ob.Status == 1).Select(x => new
            {
                x.OwnerID,
                x.OwnerLevelID,
                x.OwnerName
            }).Where(ob => ob.OwnerLevelID == id).OrderByDescending(ob => ob.OwnerID);
            return Json(new { result, Msg = "" }, JsonRequestBehavior.AllowGet);
        }

        // Get User(s)
        //[Authorize]
        [HttpGet]
        public async Task<dynamic> GetUsers(string id)
        {
            var users = new List<SEC_User>();
            await Task.Run(() => _userService.GetAllUser("", id, out users));
            var result = users.Where(ob => ob.Status == 1.ToString()).Select(x => new
            {
                x.UserID,
                x.UserFullName
            }).OrderByDescending(ob => ob.UserID);
            return Json(new { result, Msg = "" }, JsonRequestBehavior.AllowGet);
        }


        // Get User Log(s)
        //[Authorize]
        [HttpGet]
        public async Task<dynamic> GetUserLog(string id, string dateForm, string dateTo)
        {
            var userLog = new List<SEC_UserLog>();
            await Task.Run(() => _accessLogService.GetUserAccessLog(id, dateForm == "" ? 
                (DateTime?)null : DataValidation.DateTimeConversion(dateForm),
                dateTo == "" ? (DateTime?)null : DataValidation.DateTimeConversion(dateTo), out userLog));
            var result = userLog.Where(ob => ob.Status == 1).Select(x => new
            {
                x.LogID,
                x.UserID,
                x.UserFullName,
                x.UsedIP,
                x.ActionUrl,
                ActionEventTime = string.Format("{0:dd/mm/yyyy}", x.ActionEventTime),
                x.Status
            });
            return Json(new { result, Msg = "" }, JsonRequestBehavior.AllowGet);
        }

        #endregion
    }
}