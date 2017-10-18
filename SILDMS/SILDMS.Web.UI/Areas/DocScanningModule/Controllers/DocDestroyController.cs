using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using SILDMS.Model.DocScanningModule;
using SILDMS.Service.DocDestroy;
using SILDMS.Service.DocDestroyPolicy;
using SILDMS.Service.DocProperty;
using SILDMS.Service.DocumentCategory;
using SILDMS.Service.DocumentType;
using SILDMS.Service.OwnerProperIdentity;
using SILDMS.Utillity;
using SILDMS.Utillity.Localization;
using SILDMS.Web.UI.Areas.SecurityModule;
using SILDMS.Web.UI.Areas.SecurityModule.Models;

namespace SILDMS.Web.UI.Areas.DocScanningModule.Controllers
{
    public class DocDestroyController : Controller
    {
        private ValidationResult respStatus = new ValidationResult();
        private string outStatus = string.Empty;
        private string action = "";

        private readonly IDocDestroyPolicyService _destroyPolicyService;
        private readonly IDocDestroyService _docDestroyService;
        readonly IOwnerProperIdentityService _ownerProperIdentityService;
        readonly IDocCategoryService _docCategoryService;
        readonly IDocTypeService _docTypeService;
        readonly IDocPropertyService _docPropertyService;
        private readonly ILocalizationService _localizationService;
        private readonly string UserID = string.Empty;

        public DocDestroyController(IDocDestroyPolicyService destroyPolicyService,
            IDocDestroyService docDestroyService,
            IOwnerProperIdentityService ownerProperIdentityRepository,
            IDocCategoryService docCategoryService, IDocTypeService docTypeService,
            IDocPropertyService docPropertyService,  
            ILocalizationService localizationService)
        {
            _docDestroyService = docDestroyService;
            _destroyPolicyService = destroyPolicyService;
            _ownerProperIdentityService = ownerProperIdentityRepository;
            _docCategoryService = docCategoryService;
            _docTypeService = docTypeService;
            _docPropertyService = docPropertyService;
            _localizationService = localizationService;
            UserID = SILAuthorization.GetUserID();
        }


        [SILAuthorize]
        public ActionResult Index()
        {
            return View();
        }


        public async Task<dynamic> GetDestroyDetailsBySearchParam(string _OwnerID, string _DestroyID,
            string _DocCategoryID, string _DocTypeID, string _DocPropertyID, string _DocPropIdentityID)
        {
            List<DSM_DestroyPolicy> destroyPolicies = null;


            await Task.Run(() => _docDestroyService.GetDestroyDetailsBySearchParam(_DestroyID, UserID, _OwnerID,
                _DocCategoryID, _DocTypeID, _DocPropertyID, _DocPropIdentityID, out destroyPolicies));


            var result = destroyPolicies
                .GroupBy(p => p.DestroyPolicyID)
                .Select(g => g.First())
                .ToList();


            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        public async Task<dynamic> GetDocCategoriesForSpecificUser(string _DestroyPolicyID, string _OwnerID, string _DestroyID)
        {
            List<DSM_DocCategory> objDsmDocCategories = null;
            List<DSM_DestroyPolicy> dsmDestroyPolicies = null;
            List<DSM_DestroyPolicy> dsmDestroy = null;

            await Task.Run(() => _docCategoryService.GetDocCategories("", UserID, out objDsmDocCategories));
            await Task.Run(() => _destroyPolicyService.GetDestroyPolicyBySearchParam(null, UserID, _OwnerID,
                null, null, null, null, out dsmDestroyPolicies));

            await Task.Run(() => _docDestroyService.GetDestroyDetailsBySearchParam(_DestroyID, UserID, _OwnerID,
                null, null, null, null, out dsmDestroy));

            if (_DestroyID == "")
            {
                var result = (from dc in dsmDestroyPolicies
                              where dc.PolicyDetailStatus == 1 & dc.DocTypeID == "" & dc.OwnerID == _OwnerID

                              join c in objDsmDocCategories on dc.DocCategoryID equals c.DocCategoryID
                              select new
                              {
                                  DestroyPolicyDtlID = dc.DestroyPolicyDtlID,
                                  DestroyPolicyID = dc.DestroyPolicyID,
                                  DocCategoryID = dc.DocCategoryID,
                                  DocCategoryName = c.DocCategoryName,
                                  IsSelected = false,
                                  TimeValue = (dc.TimeValue),
                                  TimeUnit = (dc.TimeUnit),
                                  ExceptionValue = (dc.ExceptionValue)
                              }).ToList();

                return Json(new { Msg = "", result }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var result = (from dpd in dsmDestroyPolicies
                              where dpd.PolicyDetailStatus == 1 & dpd.DocTypeID == ""

                              join d in dsmDestroy.Where(x => x.DestroyPolicyID == _DestroyPolicyID) on dpd.DocCategoryID equals d.DocCategoryID into Categories
                              from d in Categories.DefaultIfEmpty()

                              join dc in objDsmDocCategories on dpd.DocCategoryID equals dc.DocCategoryID into CategoryDetails
                              from dc in CategoryDetails.DefaultIfEmpty()

                              select new
                              {
                                  DestroyPolicyDtlID = dpd.DestroyPolicyDtlID,
                                  DestroyPolicyID = dpd.DestroyPolicyID,
                                  DocCategoryID = dpd.DocCategoryID,
                                  DocCategoryName = dc.DocCategoryName,
                                  IsSelected = (d != null && d.IsSelected),
                                  TimeValue = (dpd.TimeValue),
                                  TimeUnit = (dpd.TimeUnit),
                                  ExceptionValue = (dpd.ExceptionValue)
                              }).ToList();

                return Json(new { Msg = "", result }, JsonRequestBehavior.AllowGet);
            }

        }

        [Authorize]
        public async Task<dynamic> GetDocumentTypeForSpecificUser(string _DestroyPolicyID,
            string _OwnerID, string _DocCategoryID, string _DestroyID)
        {
            List<DSM_DocType> objDsmDocTypes = null;
            List<DSM_DestroyPolicy> dsmDestroyPolicies = null;
            List<DSM_DestroyPolicy> dsmDestroy = null;

            await Task.Run(() => _docTypeService.GetDocTypes("", UserID, out objDsmDocTypes));
            await Task.Run(() => _destroyPolicyService.GetDestroyPolicyBySearchParam(null, UserID, _OwnerID,
                _DocCategoryID, null, null, null, out dsmDestroyPolicies));

            await Task.Run(() => _docDestroyService.GetDestroyDetailsBySearchParam(_DestroyID, UserID, _OwnerID,
                null, null, null, null, out dsmDestroy));

            if (_DestroyID == "")
            {
                var result = (from dc in dsmDestroyPolicies
                              where dc.PolicyDetailStatus == 1 & dc.DocPropertyID == ""
                              & dc.OwnerID == _OwnerID & dc.DocCategoryID == _DocCategoryID

                              join c in objDsmDocTypes on dc.DocTypeID equals c.DocTypeID
                              select new
                              {
                                  DestroyPolicyDtlID = dc.DestroyPolicyDtlID,
                                  DestroyPolicyID = dc.DestroyPolicyID,
                                  DocTypeID = dc.DocTypeID,
                                  DocTypeName = c.DocTypeName,
                                  IsSelected = false,
                                  TimeValue = (dc.TimeValue),
                                  TimeUnit = (dc.TimeUnit),
                                  ExceptionValue = (dc.ExceptionValue)
                              }).ToList();

                return Json(new { Msg = "", result }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var result = (from dpd in dsmDestroyPolicies
                              where dpd.PolicyDetailStatus == 1 & dpd.DestroyPolicyID == _DestroyPolicyID

                              join d in dsmDestroy.Where(x=>x.DestroyPolicyID == _DestroyPolicyID) on dpd.DocTypeID equals d.DocTypeID into Categories
                              from d in Categories.DefaultIfEmpty()

                              join dc in objDsmDocTypes on dpd.DocTypeID equals dc.DocTypeID into CategoryDetails
                              from dc in CategoryDetails.DefaultIfEmpty()

                              select new
                              {
                                  DestroyPolicyDtlID = dpd.DestroyPolicyDtlID,
                                  DestroyPolicyID = dpd.DestroyPolicyID,
                                  DocTypeID = dpd.DocTypeID,
                                  DocTypeName = dc.DocTypeName,
                                  IsSelected = d != null && d.IsSelected,
                                  TimeValue = (dpd.TimeValue),
                                  TimeUnit = (dpd.TimeUnit),
                                  ExceptionValue = (dpd.ExceptionValue)
                              }).ToList();

                return Json(new { Msg = "", result }, JsonRequestBehavior.AllowGet);
            }
        }


        [Authorize]
        public async Task<dynamic> GetDocumentPropertyForSpecificUser(string _DestroyPolicyID, string _OwnerID, string _DocCategoryID,
            string _DocTypeID, string _DestroyID)
        {
            List<DSM_DocProperty> objDsmDocProperties = null;
            List<DSM_DestroyPolicy> dsmDestroyPolicies = null;

            List<DSM_DestroyPolicy> dsmDestroy = null;

            await Task.Run(() => _docPropertyService.GetDocProperty("", UserID, out objDsmDocProperties));
            await Task.Run(() => _destroyPolicyService.GetDestroyPolicyBySearchParam(null, UserID, _OwnerID,
                _DocCategoryID, _DocTypeID, null, null, out dsmDestroyPolicies));

            await Task.Run(() => _docDestroyService.GetDestroyDetailsBySearchParam(_DestroyID, UserID, _OwnerID,
                null, null, null, null, out dsmDestroy));

            if (_DestroyID == "")
            {
                var result = (from dc in dsmDestroyPolicies
                              where dc.PolicyDetailStatus == 1 & dc.DocPropIdentifyID == ""
                              & dc.OwnerID == _OwnerID & dc.DocCategoryID == _DocCategoryID &
                              dc.DocTypeID == _DocTypeID

                              join c in objDsmDocProperties on dc.DocPropertyID equals c.DocPropertyID
                              select new
                              {
                                  DestroyPolicyDtlID = dc.DestroyPolicyDtlID,
                                  DestroyPolicyID = dc.DestroyPolicyID,
                                  DocPropertyID = dc.DocPropertyID,
                                  DocPropertyName = c.DocPropertyName,
                                  IsSelected = false,
                                  TimeValue = (dc.TimeValue),
                                  TimeUnit = (dc.TimeUnit),
                                  ExceptionValue = (dc.ExceptionValue)
                              }).ToList();

                return Json(new { Msg = "", result }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var result = (from dpd in dsmDestroyPolicies
                              where dpd.PolicyDetailStatus == 1 & dpd.DestroyPolicyID == _DestroyPolicyID

                              join d in dsmDestroy.Where(x => x.DestroyPolicyID == _DestroyPolicyID) on dpd.DocPropertyID equals d.DocPropertyID into Categories
                              from d in Categories.DefaultIfEmpty()

                              join dc in objDsmDocProperties on dpd.DocPropertyID equals dc.DocPropertyID into CategoryDetails
                              from dc in CategoryDetails.DefaultIfEmpty()

                              select new
                              {
                                  DestroyPolicyDtlID = dpd.DestroyPolicyDtlID,
                                  DestroyPolicyID = dpd.DestroyPolicyID,
                                  DocPropertyID = dpd.DocPropertyID,
                                  DocPropertyName = dc.DocPropertyName,
                                  IsSelected = (d != null && d.IsSelected),
                                  TimeValue = (dpd.TimeValue),
                                  TimeUnit = (dpd.TimeUnit),
                                  ExceptionValue = (dpd.ExceptionValue)
                              }).ToList();


                return Json(new { Msg = "", result }, JsonRequestBehavior.AllowGet);
            }
        }

        [Authorize]
        public async Task<dynamic> GetDocPropIdentityForSpecificUser(string _DestroyPolicyID, string _OwnerID, string _DocCategoryID,
            string _DocTypeID, string _DocPropertyID, string _DestroyID)
        {
            List<DSM_DocPropIdentify> objDocPropIdentifies = null;
            List<DSM_DestroyPolicy> dsmDestroyPolicies = null;
            List<DSM_DestroyPolicy> dsmDestroy = null;

            await Task.Run(() => _ownerProperIdentityService.GetDocPropIdentify(UserID, "", out objDocPropIdentifies));
            await Task.Run(() => _destroyPolicyService.GetDestroyPolicyBySearchParam(null, UserID, _OwnerID,
               _DocCategoryID, _DocTypeID, _DocPropertyID, null, out dsmDestroyPolicies));

            await Task.Run(() => _docDestroyService.GetDestroyDetailsBySearchParam(_DestroyID, UserID, _OwnerID,
                null, null, null, null, out dsmDestroy));


            if (_DestroyID == "")
            {
                var result = (from dc in dsmDestroyPolicies
                              where dc.PolicyDetailStatus == 1 & dc.OwnerID == _OwnerID & dc.DocCategoryID == _DocCategoryID &
                              dc.DocTypeID == _DocTypeID & dc.DocPropertyID == _DocPropertyID

                              join c in objDocPropIdentifies on dc.DocPropIdentifyID equals c.DocPropIdentifyID
                              select new
                              {
                                  DestroyPolicyDtlID = dc.DestroyPolicyDtlID,
                                  DestroyPolicyID = dc.DestroyPolicyID,
                                  DocPropIdentifyID = dc.DocPropIdentifyID,
                                  IdentificationAttribute = c.IdentificationAttribute,
                                  IsSelected = false,
                                  TimeValue = (dc.TimeValue),
                                  TimeUnit = (dc.TimeUnit),
                                  ExceptionValue = (dc.ExceptionValue)
                              }).ToList();




                return Json(new { Msg = "", result }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var result = (from dpd in dsmDestroyPolicies
                              where dpd.PolicyDetailStatus == 1

                              join d in dsmDestroy.Where(x => x.DestroyPolicyID == _DestroyPolicyID) on dpd.DocPropIdentifyID equals d.DocPropIdentifyID into Categories
                              from d in Categories.DefaultIfEmpty()

                              join dc in objDocPropIdentifies on dpd.DocPropIdentifyID equals dc.DocPropIdentifyID into CategoryDetails
                              from dc in CategoryDetails.DefaultIfEmpty()

                              select new
                              {
                                  DestroyPolicyDtlID = dpd.DestroyPolicyDtlID,
                                  DestroyPolicyID = dpd.DestroyPolicyID,
                                  DocPropIdentifyID = dpd.DocPropIdentifyID,
                                  IdentificationAttribute = dc.IdentificationAttribute,
                                  IsSelected = (d != null && d.IsSelected),
                                  TimeValue = (dpd.TimeValue),
                                  TimeUnit = (dpd.TimeUnit),
                                  ExceptionValue = (dpd.ExceptionValue)
                              }).ToList();


                return Json(new { Msg = "", result }, JsonRequestBehavior.AllowGet);
            }

        }



        [Authorize]
        [SILLogAttribute]
        public async Task<dynamic> SetDocDestroy(DSM_DestroyPolicy model)
        {
            if (ModelState.IsValid)
            {
                action = "add";
                model.SetBy = UserID;
                model.ModifiedBy = model.SetBy;
                //model.UploaderIP = GetIPAddress.LocalIPAddress();
                respStatus = await Task.Run(() => _docDestroyService.SetDocDestroy(model, action, out outStatus));

                return Json(new { Message = respStatus.Message, respStatus }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors);
                respStatus = new ValidationResult("E404", _localizationService.GetResource("E404"));
            }
            return Json(new { Message = respStatus.Message, respStatus }, JsonRequestBehavior.AllowGet);
        }




	}
}