using SILDMS.Utillity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.SessionState;
using SILDMS.Model.DocScanningModule;
using SILDMS.Service.DocumentCategory;
using SILDMS.Service.DocumentType;
using SILDMS.Service.Owner;
using SILDMS.Service.OwnerLevel;
using SILDMS.Service.OwnerProperIdentity;
using SILDMS.Service.DocProperty;
using SILDMS.Utillity.Localization;
using SILDMS.Web.UI.Areas.SecurityModule;
using SILDMS.Web.UI.Areas.SecurityModule.Models;

namespace SILDMS.Web.UI.Areas.DocScanningModule.Controllers
{
    public class OwnerProperIdentityController : Controller
    {
        readonly IOwnerProperIdentityService _ownerProperIdentityService;
        readonly IOwnerLevelService _ownerLevelService;
        readonly IOwnerService _ownerService;
        readonly IDocCategoryService _docCategoryService;
        readonly IDocTypeService _docTypeService;
        readonly IDocPropertyService _docPropertyService;
        private readonly ILocalizationService _localizationService;
        private ValidationResult respStatus = new ValidationResult();
        private string outStatus = string.Empty;
        private string action = "";
        private readonly string UserID = string.Empty;


        public OwnerProperIdentityController(IOwnerProperIdentityService ownerProperIdentityRepository,
            IOwnerLevelService ownerLevelServiceRepository, IOwnerService ownerServiceRepository,
            IDocCategoryService docCategoryService, IDocTypeService docTypeService,
            IDocPropertyService docPropertyService, ILocalizationService localizationService)
        {
            this._ownerProperIdentityService = ownerProperIdentityRepository;
            this._ownerLevelService = ownerLevelServiceRepository;
            this._ownerService = ownerServiceRepository;
            this._docCategoryService = docCategoryService;
            this._docTypeService = docTypeService;
            this._docPropertyService = docPropertyService;
            this._localizationService = localizationService;
            UserID = SILAuthorization.GetUserID();
        }

        [SILAuthorize]
        public ActionResult Index()
        {
            return View();
        }



        [HttpGet]
        [Authorize]
        public async Task<dynamic> GetOwnerLevel(string _OwnerLevelID)
        {
            List<DSM_OwnerLevel> objDsmOwnerLevels = null;          
            await Task.Run(() => _ownerLevelService.GetOwnerLevel("", UserID, out objDsmOwnerLevels));

            var result = (from ol in objDsmOwnerLevels
                          where ol.Status == 1
                          orderby ol.LevelAccess
                          select new DSM_OwnerLevel
                          {
                              OwnerLevelID = ol.OwnerLevelID,
                              LevelName = ol.LevelName
                          }).ToList();

            return Json(new { Msg = "", result }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [Authorize]
        public async Task<dynamic> GetOwnerForSpecificOwnerLevel(string _OwnerLevelID)
        {
            List<DSM_Owner> objDsmOwner = null;

            await Task.Run(() => _ownerService.GetAllOwners("", UserID, out objDsmOwner));

            var result = (from ow in objDsmOwner
                          where ow.Status == 1 & ow.OwnerLevelID == _OwnerLevelID
                          orderby ow.OwnerName
                          select new DSM_Owner
                          {
                              OwnerID = ow.OwnerID,
                              OwnerName = ow.OwnerName
                          }).ToList();

            return Json(new { Msg = "", result }, JsonRequestBehavior.AllowGet);
        }
        [Authorize]
        public async Task<dynamic> GetDocumentCategoriesForSpecificOwner(string _OwnerID)
        {
            List<DSM_DocCategory> objDsmDocCategories = null;

            await Task.Run(() => _docCategoryService.GetDocCategories("", UserID, out objDsmDocCategories));

            var result = (from ow in objDsmDocCategories
                          where ow.Status == 1 & ow.OwnerID == _OwnerID
                          orderby ow.DocCategoryName
                          select new DSM_DocCategory
                          {
                              DocCategoryID = ow.DocCategoryID,
                              DocCategoryName = ow.DocCategoryName
                          }).ToList();

            return Json(new { Msg = "", result }, JsonRequestBehavior.AllowGet);
        }
        [Authorize]
        public async Task<dynamic> GetDocumentTypeForSpecificDocCategory(string _DocCategoryID, string _OwnerID)
        {
            List<DSM_DocType> objDsmDocTypes = null;

            await Task.Run(() => _docTypeService.GetDocTypes("", UserID, out objDsmDocTypes));

            var result = (from ow in objDsmDocTypes
                          where ow.Status == 1 & ow.DocCategoryID == _DocCategoryID & ow.OwnerID == _OwnerID
                          orderby ow.DocTypeName
                          select new DSM_DocType
                          {
                              DocTypeID = ow.DocTypeID,
                              DocTypeName = ow.DocTypeName
                          }).ToList();

            return Json(new { Msg = "", result }, JsonRequestBehavior.AllowGet);
        }
        [Authorize]
        public async Task<dynamic> GetDocumentPropertyForSpecificDocCategory(string _DocCategoryID,
            string _OwnerID, string _DocTypeID)
        {
            List<DSM_DocProperty> objDsmDocProperties = null;

            await Task.Run(() => _docPropertyService.GetDocProperty("", UserID, out objDsmDocProperties));

            var result = (from dp in objDsmDocProperties
                          where dp.OwnerID == _OwnerID & dp.DocCategoryID == _DocCategoryID &
                                dp.DocTypeID == _DocTypeID & dp.Status == 1

                          select new ddlDSMDocProperty
                          {
                              DocPropertyID = dp.DocPropertyID,
                              DocPropertyName = dp.DocPropertyName,
                              IsSelected = true
                          }).ToList();

            return Json(new { Msg = "", result }, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        public async Task<dynamic> GetDocPropIdentityForSpecificDocType(string _OwnerID, string _DocCategoryID,
             string _DocTypeID, string _DocPropertyID)
        {
            List<DSM_DocPropIdentify> objDocPropIdentifies = null;

            await Task.Run(() => _ownerProperIdentityService.GetDocPropIdentify(UserID, "", out objDocPropIdentifies));

            var result = (from dp in objDocPropIdentifies
                          where dp.OwnerID == _OwnerID & dp.DocCategoryID == _DocCategoryID &
                                dp.DocTypeID == _DocTypeID & dp.DocPropertyID == _DocPropertyID
                                
                            orderby dp.IdentificationSL
                          select new DSM_DocPropIdentify
                          {
                              DocPropIdentifyID = dp.DocPropIdentifyID,
                              DocPropertyID = dp.DocPropertyID,
                              DocTypeID = dp.DocTypeID,
                              DocCategoryID = dp.DocCategoryID,
                              OwnerID = dp.OwnerID,
                              IdentificationCode = dp.IdentificationCode,
                              IdentificationSL = dp.IdentificationSL,
                              IdentificationAttribute = dp.IdentificationAttribute,
                              AttributeGroup = dp.AttributeGroup,
                              IsRequired = dp.IsRequired,
                              IsAuto = dp.IsAuto,
                              IsRestricted = dp.IsRestricted,
                              Remarks = dp.Remarks,
                              Status = dp.Status
                          }).ToList();

            return Json(new { Msg = "", result }, JsonRequestBehavior.AllowGet);
        }



        [HttpPost]
        [Authorize]
        [SILLogAttribute]
        public async Task<dynamic> AddOwnerPropIdentity(DSM_DocPropIdentify modelDsmDocPropIdentify)
        {
            if (ModelState.IsValid)
            {
                action = "add";
                modelDsmDocPropIdentify.SetBy = UserID;
                modelDsmDocPropIdentify.ModifiedBy = modelDsmDocPropIdentify.SetBy;
                respStatus = await Task.Run(() => _ownerProperIdentityService.AddOwnerPropIdentity(modelDsmDocPropIdentify, action, out outStatus));
                return Json(new { Message = respStatus.Message, respStatus }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                //var errors = ModelState.Values.SelectMany(v => v.Errors);
                respStatus = new ValidationResult("E404", _localizationService.GetResource("E404"));
            }
            return Json(new { Message = respStatus.Message, respStatus }, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        [HttpPost]
        [SILLogAttribute]
        public async Task<dynamic> EditOwnerPropIdentity(DSM_DocPropIdentify modelDsmDocPropIdentify)
        {
            if (ModelState.IsValid)
            {
                action = "edit";
                modelDsmDocPropIdentify.SetBy = UserID;
                modelDsmDocPropIdentify.ModifiedBy = modelDsmDocPropIdentify.SetBy;
                respStatus = await Task.Run(() => _ownerProperIdentityService.EditOwnerPropIdentity(modelDsmDocPropIdentify, action, out outStatus));
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