using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using SILDMS.Model.DocScanningModule;
using SILDMS.Service.DocumentCategory;
using SILDMS.Service.Owner;
using SILDMS.Service.OwnerLevel;
using SILDMS.Utillity;
using SILDMS.Utillity.Localization;
using System.Web.SessionState;
using SILDMS.Web.UI.Areas.SecurityModule;
using SILDMS.Web.UI.Areas.SecurityModule.Models;

namespace SILDMS.Web.UI.Areas.DocScanningModule.Controllers
{

    public class DocCategoryController : Controller
    {

        #region Fields
        readonly IOwnerService _ownerService;
        private readonly IOwnerLevelService _ownerLevelService;
        readonly IDocCategoryService _docCategoryService;
        private readonly ILocalizationService _localizationService;
        private ValidationResult _respStatus = new ValidationResult();
        private string _outStatus = string.Empty;
        private string _action = string.Empty;
        private readonly string UserID = string.Empty;

        #endregion

        #region Constructor

        public DocCategoryController(IDocCategoryService docCategoryService, ILocalizationService localizationService, IOwnerService ownerService, IOwnerLevelService ownerLevelService)
        {
            _docCategoryService = docCategoryService;
            _localizationService = localizationService;
            _ownerService = ownerService;
            _ownerLevelService = ownerLevelService;
            UserID = SILAuthorization.GetUserID();
        }

        #endregion

        // GET: DocScanningModule/DocCategory

        [SILAuthorize]
        public ActionResult Index()
        {
            return View();
        }

        #region Document Category Manipulation

        // Add New Document Category
        [Authorize]
        [HttpPost]
        [SILLogAttribute]
        public async Task<dynamic> AddDocCategory(DSM_DocCategory objDocCategory)
        {
            if (ModelState.IsValid)
            {
                _action = "add";
                objDocCategory.SetBy = UserID;
                objDocCategory.ModifiedBy = objDocCategory.SetBy;
                _respStatus = await Task.Run(() => _docCategoryService.ManipulateDocCategory(objDocCategory, _action, out _outStatus));
                // Error handling.   
                return Json(new { Message = _respStatus.Message, _respStatus }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                _respStatus = new ValidationResult("E404", _localizationService.GetResource("E404"));
            }
            return Json(new { Message = _respStatus.Message, _respStatus }, JsonRequestBehavior.AllowGet);
        }

        // Edit Document Category
        [Authorize]
        [HttpPost]
        [SILLogAttribute]
        public async Task<dynamic> EditDocCategory(DSM_DocCategory objDocCategory)
        {
            if (ModelState.IsValid)
            {
                _action = "edit";
                objDocCategory.SetBy = UserID;
                objDocCategory.ModifiedBy = objDocCategory.SetBy;
                _respStatus = await Task.Run(() => _docCategoryService.ManipulateDocCategory(objDocCategory, _action, out _outStatus));
                return Json(new { Message = _respStatus.Message, _respStatus }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                _respStatus = new ValidationResult("E404", _localizationService.GetResource("E404"));
            }
            return Json(new { Message = _respStatus.Message, _respStatus }, JsonRequestBehavior.AllowGet);
        }

        // Delete Document Category
        [Authorize]
        [HttpPost]
        [SILLogAttribute]
        public async Task<dynamic> DeleteDocCategory(DSM_DocCategory objDocCategory)
        {
            _action = "delete";
            _respStatus = await Task.Run(() => _docCategoryService.ManipulateDocCategory(objDocCategory, _action, out _outStatus));
            return Json(new { Message = _respStatus.Message, _respStatus }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Retrive Data for View

        // Get Owner Level(s)
        [Authorize]
        [HttpGet]
        public async Task<dynamic> GetOwnerLevel(string id)
        {
            var ownerLevel = new List<DSM_OwnerLevel>();
            await Task.Run(() => _ownerLevelService.GetOwnerLevel(id, Convert.ToString(Session["UserID"]), out ownerLevel));
            var result = ownerLevel.Where(ob => ob.Status == 1).Select(x => new
            {
                x.OwnerLevelID,
                x.LevelName
            }).OrderByDescending(ob => ob.LevelName);
            return Json(new { result, Msg = "" }, JsonRequestBehavior.AllowGet);
        }

        // Get Owner(s)
        [Authorize]
        [HttpGet]
        public async Task<dynamic> GetOwners(string id)
        {
            var owner = new List<DSM_Owner>();
            await Task.Run(() => _ownerService.GetAllOwners("", Convert.ToString(Session["UserID"]), out owner));
            var result = owner.Where(ob => ob.Status == 1).Select(x => new
            {
                x.OwnerID,
                x.OwnerLevelID,
                x.OwnerName
            }).Where(ob => ob.OwnerLevelID == id).OrderByDescending(ob => ob.OwnerID);
            return Json(new { result, Msg = "" }, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        [HttpGet]
        public async Task<dynamic> GetDocCategoryForOwner(string id)
        {
            var docCategories = new List<DSM_DocCategory>();
            await Task.Run(() => _docCategoryService.GetDocCategories("", Convert.ToString(Session["UserID"]), out docCategories));
            var result = docCategories.Where(ob => ob.OwnerID == id).Select(x => new
            {
                x.DocCategoryID,
                x.OwnerID,
                x.DocCategorySL,
                x.UDDocCategoryCode,
                x.DocCategoryName,
                x.Status
            }).OrderByDescending(ob => ob.DocCategoryID);
            return Json(new { result, Msg = "" }, JsonRequestBehavior.AllowGet);
        }

        #endregion

    }
}