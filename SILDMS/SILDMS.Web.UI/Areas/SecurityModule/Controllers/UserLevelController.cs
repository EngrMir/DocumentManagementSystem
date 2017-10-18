using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using SILDMS.Model.SecurityModule;
using SILDMS.Service.UserLevel;
using SILDMS.Utillity;
using SILDMS.Utillity.Localization;
using SILDMS.Web.UI.Areas.SecurityModule.Models;

namespace SILDMS.Web.UI.Areas.SecurityModule.Controllers
{
    public class UserLevelController : Controller
    {
        #region Fields

        private readonly IUserLevelService _userLevelService;
        private readonly ILocalizationService _localizationService;
        private ValidationResult _respStatus = new ValidationResult();
        private string _outStatus = string.Empty;
        private string _action = string.Empty;
        private readonly string UserID = string.Empty;
        #endregion

        #region Constructor

        public UserLevelController(IUserLevelService userLevelService, ILocalizationService localizationService)
        {
            _userLevelService = userLevelService;
            _localizationService = localizationService;
            UserID = SILAuthorization.GetUserID();
        }

        #endregion

        #region Retrive Data for View
        // GET: SecurityModule/UserLevel
          [SILAuthorize]  
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<dynamic> GetUserLevel(int? userLevel, string levelType)
        {
            var userLevels = new List<SEC_UserLevel>();
            await Task.Run(() => _userLevelService.GetUserLevels(userLevel, _action, levelType, out userLevels));
            var result = userLevels.Select(ob => new
            {
                ob.ID,
                ob.UserLevel,
                ob.UserLevelName,
                ob.UserLevelSL,
                ob.UserLevelType,
                ob.Status
            }).OrderBy(ob => ob.UserLevel);
            return Json(new { result, Msg = "" }, JsonRequestBehavior.AllowGet);
        } 

        #endregion

        #region Manipulate UserLevel Data

        // Add New User Level
        [HttpPost]
        [SILLogAttribute]
        public async Task<dynamic> AddUserLevel(SEC_UserLevel objUserLevel)
        {
            if (ModelState.IsValid)
            {
                _action = "add";
                objUserLevel.SetBy = "16012500001";//Session["UserId"].ToString();
                objUserLevel.ModifiedBy = objUserLevel.SetBy;
                _respStatus = await Task.Run(() => _userLevelService.ManipulateUserLevel(objUserLevel, _action, out _outStatus));
                // Error handling.   
                return Json(new { Message = _respStatus.Message, _respStatus }, JsonRequestBehavior.AllowGet);
            }
            _respStatus = new ValidationResult("E404", _localizationService.GetResource("E404"));
            return Json(new { Message = _respStatus.Message, _respStatus }, JsonRequestBehavior.AllowGet);
        }

        // Edit User Level
        [Authorize]
        [HttpPost]
        [SILLogAttribute]
        public async Task<dynamic> EditUserLevel(SEC_UserLevel objUserLevel)
        {
            if (ModelState.IsValid)
            {
                _action = "edit";
                objUserLevel.SetBy = "16012500001";//Session["UserId"].ToString();
                objUserLevel.ModifiedBy = objUserLevel.SetBy;
                _respStatus = await Task.Run(() => _userLevelService.ManipulateUserLevel(objUserLevel, _action, out _outStatus));
                return Json(new { Message = _respStatus.Message, _respStatus }, JsonRequestBehavior.AllowGet);
            }
            _respStatus = new ValidationResult("E404", _localizationService.GetResource("E404"));
            return Json(new { Message = _respStatus.Message, _respStatus }, JsonRequestBehavior.AllowGet);
        }

        #endregion


    }
}