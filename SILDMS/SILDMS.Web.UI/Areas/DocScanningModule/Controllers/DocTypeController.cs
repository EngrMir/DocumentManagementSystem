using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using SILDMS.Model.DocScanningModule;
using SILDMS.Service.DocumentCategory;
using SILDMS.Service.DocumentType;
using SILDMS.Service.Owner;
using SILDMS.Service.OwnerLevel;
using SILDMS.Utillity;
using SILDMS.Utillity.Localization;
using System.Web.SessionState;
using SILDMS.Model.SecurityModule;
using SILDMS.Service.UserLevel;
using SILDMS.Web.UI.Areas.SecurityModule;
using SILDMS.Web.UI.Areas.SecurityModule.Models;

namespace SILDMS.Web.UI.Areas.DocScanningModule.Controllers
{

    public class DocTypeController : Controller
    {

        #region Fields

        readonly IOwnerService _ownerService;
        private readonly IOwnerLevelService _ownerLevelService;
        private readonly IDocCategoryService _docCategoryService;
        private readonly IDocTypeService _docTypeService;
        private readonly IUserLevelService _userLevelService;
        private readonly ILocalizationService _localizationService;
        private ValidationResult _respStatus = new ValidationResult();
        private string _outStatus = string.Empty;
        private string _action = string.Empty;
        private readonly string UserID = string.Empty;
        #endregion

        #region Constructor

        public DocTypeController(IDocTypeService docTypeService, ILocalizationService localizationService, 
            IOwnerService ownerService, IOwnerLevelService ownerLevelService, IDocCategoryService docCategoryService, IUserLevelService userLevelService)
        {
            _docTypeService = docTypeService;
            _localizationService = localizationService;
            _ownerService = ownerService;
            _ownerLevelService = ownerLevelService;
            _docCategoryService = docCategoryService;
            _userLevelService = userLevelService;
            UserID = SILAuthorization.GetUserID();
        }

        #endregion

        // GET: DocScanningModule/DocType
        [SILAuthorize]
        public ActionResult Index()
        {
            return View();
        }

        #region Document Type Manipulation

        // Add New Document Type
        [Authorize]
        [HttpPost]
        [SILLogAttribute]
        public async Task<dynamic> AddDocType(DSM_DocType objDocType)
        {
            if (ModelState.IsValid)
            {
                _action = "add";
                objDocType.SetBy = UserID;
                objDocType.ModifiedBy = objDocType.SetBy;
                _respStatus = await Task.Run(() => _docTypeService.ManipulateDocType(objDocType, _action, out _outStatus));
                // Error handling.   
                return Json(new { Message = _respStatus.Message, _respStatus }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                _respStatus = new ValidationResult("E404", _localizationService.GetResource("E404"));
            }
            return Json(new { Message = _respStatus.Message, _respStatus }, JsonRequestBehavior.AllowGet);
        }

        // Edit Document Type
        [Authorize]
        [HttpPost]
        [SILLogAttribute]
        public async Task<dynamic> EditDocType(DSM_DocType objDocType)
        {
            if (ModelState.IsValid)
            {
                _action = "edit";
                objDocType.SetBy = UserID;
                objDocType.ModifiedBy = objDocType.SetBy;
                _respStatus = await Task.Run(() => _docTypeService.ManipulateDocType(objDocType, _action, out _outStatus));
                return Json(new { Message = _respStatus.Message, _respStatus }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                _respStatus = new ValidationResult("E404", _localizationService.GetResource("E404"));
            }
            return Json(new { Message = _respStatus.Message, _respStatus }, JsonRequestBehavior.AllowGet);
        }

        // Delete Document Type
        [Authorize]
        [HttpPost]
        [SILLogAttribute]
        public async Task<dynamic> DeleteDocType(DSM_DocType objDocType)
        {
            _action = "delete";
            _respStatus = await Task.Run(() => _docTypeService.ManipulateDocType(objDocType, _action, out _outStatus));
            return Json(new { Message = _respStatus.Message, _respStatus }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Get data for view

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
        [HttpGet]
        public async Task<dynamic> GetOwners(string id)
        {
            var owner = new List<DSM_Owner>();
            await Task.Run(() => _ownerService.GetAllOwners("", UserID, out owner));
            var result = owner.Where(ob => ob.OwnerLevelID == id && ob.Status == 1).Select(x => new
            {
                x.OwnerID,
                x.OwnerName
            }).OrderByDescending(ob => ob.OwnerID);
            return Json(new { result, Msg = "" }, JsonRequestBehavior.AllowGet);
        }

        public async Task<dynamic> GetDocCategoryForOwner(string id)
        {
            var docCategories = new List<DSM_DocCategory>();
            await Task.Run(() => _docCategoryService.GetDocCategories("", UserID, out docCategories));
            var result = docCategories.Where(ob => ob.OwnerID == id && ob.Status == 1).Select(x => new
            {
                x.DocCategoryID,
                x.DocCategoryName
            }).OrderByDescending(ob => ob.DocCategoryID);
            return Json(new { result, Msg = "" }, JsonRequestBehavior.AllowGet);
        }

        public async Task<dynamic> GetDocTypeForCategory(string id)
        {
            var docTypes = new List<DSM_DocType>();
            await Task.Run(() => _docTypeService.GetDocTypes("", UserID, out docTypes));
            var result = docTypes.Where(ob => ob.DocCategoryID == id).Select(x => new
            {
                x.DocTypeID,
                x.DocTypeSL,
                x.UDDocTypeCode,
                x.DocTypeName,
                x.DocPreservationPolicy,
                x.DocPhysicalLocation,
                x.DocCategoryID,
                x.OwnerID,
                x.Status,
                x.DocClassification,
                x.ClassificationLevel
                
            }).OrderByDescending(ob => ob.DocCategoryID);
            return Json(new { result, Msg = "" }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public async Task<dynamic> GetDocClassification(int? userLevel, string levelType)
        {
            var userLevels = new List<SEC_UserLevel>();
            await Task.Run(() => _userLevelService.GetUserLevels(userLevel, _action, levelType, out userLevels));
            var result = userLevels.Select(ob => new
            {
                ob.ID,
                DocClassificationName = ob.UserLevelName,
             
            }).OrderBy(ob => ob.ID);
            return Json(new { result, Msg = "" }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public async Task<dynamic> GetClassificationLevel(int? userLevel, string levelType)
        {
            var userLevels = new List<SEC_UserLevel>();
            await Task.Run(() => _userLevelService.GetUserLevels(userLevel, _action, levelType, out userLevels));
            var result = userLevels.Select(ob => new
            {
                ob.ID,
                ClassificationLevelName = ob.UserLevelName,

            }).OrderBy(ob => ob.ID);
            return Json(new { result, Msg = "" }, JsonRequestBehavior.AllowGet);
        }
 

        #endregion

    }
}