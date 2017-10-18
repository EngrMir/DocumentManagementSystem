using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using SILDMS.Model.DocScanningModule;
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
    public class DocDestroyPolicyController : Controller
    {
        private ValidationResult respStatus = new ValidationResult();
        private string outStatus = string.Empty;
        private string action = "";

        private readonly IDocDestroyPolicyService _destroyPolicyService;
        readonly IOwnerProperIdentityService _ownerProperIdentityService;
        readonly IDocCategoryService _docCategoryService;
        readonly IDocTypeService _docTypeService;
        readonly IDocPropertyService _docPropertyService;
        private readonly ILocalizationService _localizationService;
        private readonly string UserID = string.Empty;

        public DocDestroyPolicyController(IDocDestroyPolicyService destroyPolicyService,
            IOwnerProperIdentityService ownerProperIdentityRepository,
            IDocCategoryService docCategoryService, IDocTypeService docTypeService,
            IDocPropertyService docPropertyService,  
            ILocalizationService localizationService)
        {
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




        [Authorize]
        public async Task<dynamic> GetDocCategoriesForSpecificUser(string _OwnerID, string _DestroyPolicyID)
        {
            List<DSM_DocCategory> objDsmDocCategories = null;
            List<DSM_DestroyPolicy> dsmDestroyPolicies = null;
            await Task.Run(() => _docCategoryService.GetDocCategories("", UserID, out objDsmDocCategories));
            await Task.Run(() => _destroyPolicyService.GetDestroyPolicyBySearchParam(_DestroyPolicyID,
                UserID, _OwnerID, null, null, null, null, out dsmDestroyPolicies));

            if (_DestroyPolicyID != "")
            {
                var result = (from dc in objDsmDocCategories
                              where dc.OwnerID == _OwnerID & dc.Status == 1

                              join dpd in dsmDestroyPolicies on new
                              {
                                  CategoryID = dc.DocCategoryID,
                                  //DestroyPolicyID = _DestroyPolicyID
                              }
                              equals  new
                              {
                                  CategoryID = dpd.DocCategoryID,
                                  //DestroyPolicyID = _DestroyPolicyID
                              } into Policies
                              from dpd in Policies.DefaultIfEmpty()
                              //where dpd.DestroyPolicyID == _DestroyPolicyID
                              select new 
                              {
                                  DestroyPolicyDtlID = dpd==null? "": dpd.DestroyPolicyDtlID,
                                  DocCategoryID = dc.DocCategoryID,
                                  DocCategoryName = dc.DocCategoryName,
                                  IsSelected = (dpd != null && dpd.IsSelected),
                                  TimeValue = (dpd == null? "":dpd.TimeValue ),
                                  TimeUnit = (dpd == null ? "" : dpd.TimeUnit),
                                  ExceptionValue = (dpd == null ? "" : dpd.ExceptionValue)

                              }).ToList();

               

                return Json(new { Msg = "", result }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var result = (from dc in objDsmDocCategories
                              where dc.OwnerID == _OwnerID & dc.Status == 1

                              select new
                              {
                                  DocCategoryID = dc.DocCategoryID,
                                  DocCategoryName = dc.DocCategoryName,
                                  IsSelected = false
                              }).ToList();
                return Json(new { Msg = "", result }, JsonRequestBehavior.AllowGet);
            }
            
        }

        [Authorize]
        public async Task<dynamic> GetDocumentTypeForSpecificUser(string _DocCategoryID, string _DestroyPolicyID)
        {
            List<DSM_DocType> objDsmDocTypes = null;
            List<DSM_DestroyPolicy> dsmDestroyPolicies = null;

            await Task.Run(() => _docTypeService.GetDocTypes("", UserID, out objDsmDocTypes));
            await Task.Run(() => _destroyPolicyService.GetDestroyPolicyBySearchParam(_DestroyPolicyID,
                UserID, null, _DocCategoryID, null, null, null, out dsmDestroyPolicies));

            if (_DestroyPolicyID != "")
            {
                var result = (from dc in objDsmDocTypes
                              where dc.DocCategoryID == _DocCategoryID & dc.Status == 1

                              join dpd in dsmDestroyPolicies on new
                              {
                                  TypeID = dc.DocTypeID,
                                  //DestroyPolicyID = _DestroyPolicyID
                              }
                              equals new
                              {
                                  TypeID = dpd.DocTypeID,
                                  //DestroyPolicyID = _DestroyPolicyID
                              } into Policies
                              from dpd in Policies.DefaultIfEmpty()
                              //where dpd.DestroyPolicyID == _DestroyPolicyID
                              select new
                              {
                                  DestroyPolicyDtlID = dpd == null ? "" : dpd.DestroyPolicyDtlID,
                                  DocTypeID = dc.DocTypeID,
                                  DocTypeName = dc.DocTypeName,
                                  IsSelected = (dpd != null && dpd.IsSelected),
                                  TimeValue = (dpd == null ? "" : dpd.TimeValue),
                                  TimeUnit = (dpd == null ? "" : dpd.TimeUnit),
                                  ExceptionValue = (dpd == null ? "" : dpd.ExceptionValue)

                              }).ToList();



                return Json(new { Msg = "", result }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var result = (from dc in objDsmDocTypes
                              where dc.DocCategoryID == _DocCategoryID & dc.Status == 1

                              select new
                              {
                                  DocTypeID = dc.DocTypeID,
                                  DocTypeName = dc.DocTypeName,
                                  IsSelected = false
                              }).ToList();
                return Json(new { Msg = "", result }, JsonRequestBehavior.AllowGet);
            }
        }

        [Authorize]
        public async Task<dynamic> GetDocumentPropertyForSpecificUser(string _DocTypeID, string _DestroyPolicyID)
        {
            List<DSM_DocProperty> objDsmDocProperties = null;
            List<DSM_DestroyPolicy> dsmDestroyPolicies = null;

            await Task.Run(() => _docPropertyService.GetDocProperty("", UserID, out objDsmDocProperties));
            await Task.Run(() => _destroyPolicyService.GetDestroyPolicyBySearchParam(_DestroyPolicyID,
                UserID, null, null, _DocTypeID, null, null, out dsmDestroyPolicies));

            if (_DestroyPolicyID != "")
            {
                var result = (from dc in objDsmDocProperties
                              where dc.DocTypeID == _DocTypeID & dc.Status == 1

                              join dpd in dsmDestroyPolicies on new
                              {
                                  DocPropertyID = dc.DocPropertyID,
                                  //DestroyPolicyID = _DestroyPolicyID
                              }
                              equals new
                              {
                                  DocPropertyID = dpd.DocPropertyID,
                                  //DestroyPolicyID = _DestroyPolicyID
                              } into Policies
                              from dpd in Policies.DefaultIfEmpty()
                              //where dpd.DestroyPolicyID == _DestroyPolicyID
                              select new
                              {
                                  DestroyPolicyDtlID = dpd == null ? "" : dpd.DestroyPolicyDtlID,
                                  DocPropertyID = dc.DocPropertyID,
                                  DocPropertyName = dc.DocPropertyName,
                                  IsSelected = (dpd != null && dpd.IsSelected),
                                  TimeValue = (dpd == null ? "" : dpd.TimeValue),
                                  TimeUnit = (dpd == null ? "" : dpd.TimeUnit),
                                  ExceptionValue = (dpd == null ? "" : dpd.ExceptionValue)

                              }).ToList();



                return Json(new { Msg = "", result }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var result = (from dc in objDsmDocProperties
                              where dc.DocTypeID == _DocTypeID & dc.Status == 1

                              select new
                              {
                                  DocPropertyID = dc.DocPropertyID,
                                  DocPropertyName = dc.DocPropertyName,
                                  IsSelected = false
                              }).ToList();
                return Json(new { Msg = "", result }, JsonRequestBehavior.AllowGet);
            }
        }
        
        [Authorize]
        public async Task<dynamic> GetDocPropIdentityForSpecificUser(string _DocPropertyID, string _DestroyPolicyID)
        {
            List<DSM_DocPropIdentify> objDocPropIdentifies = null;
            List<DSM_DestroyPolicy> dsmDestroyPolicies = null;

            await Task.Run(() => _ownerProperIdentityService.GetDocPropIdentify(UserID, "", out objDocPropIdentifies));
            await Task.Run(() => _destroyPolicyService.GetDestroyPolicyBySearchParam(_DestroyPolicyID,
                UserID, null, null, null, _DocPropertyID, null, out dsmDestroyPolicies));


            if (_DestroyPolicyID != "")
            {
                var result = (from dc in objDocPropIdentifies
                              where dc.DocPropertyID == _DocPropertyID & dc.Status == 1

                              join dpd in dsmDestroyPolicies on new
                              {
                                  DocPropIdentifyID = dc.DocPropIdentifyID,
                                  //DestroyPolicyID = _DestroyPolicyID
                              }
                              equals new
                              {
                                  DocPropIdentifyID = dpd.DocPropIdentifyID,
                                  //DestroyPolicyID = _DestroyPolicyID
                              } into Policies
                              from dpd in Policies.DefaultIfEmpty()
                              //where dpd.DestroyPolicyID == _DestroyPolicyID
                              select new
                              {
                                  DestroyPolicyDtlID = dpd == null ? "" : dpd.DestroyPolicyDtlID,
                                  DocPropIdentifyID = dc.DocPropIdentifyID,
                                  IdentificationAttribute = dc.IdentificationAttribute,
                                  IsSelected = (dpd != null && dpd.IsSelected),
                                  TimeValue = (dpd == null ? "" : dpd.TimeValue),
                                  TimeUnit = (dpd == null ? "" : dpd.TimeUnit),
                                  ExceptionValue = (dpd == null ? "" : dpd.ExceptionValue)

                              }).ToList();



                return Json(new { Msg = "", result }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var result = (from dc in objDocPropIdentifies
                              where dc.DocPropertyID == _DocPropertyID & dc.Status == 1

                              select new
                              {
                                  DocPropIdentifyID = dc.DocPropIdentifyID,
                                  IdentificationAttribute = dc.IdentificationAttribute,
                                  IsSelected = false
                              }).ToList();
                return Json(new { Msg = "", result }, JsonRequestBehavior.AllowGet);
            }

        }

        public async Task<dynamic> GetDestroyPolicyBySearchParam(string _DestroyPolicyID, 
            string _OwnerID, string _DocCategoryID, string _DocTypeID, string _DocPropertyID,
            string _DocPropIdentityID)
        {
            List<DSM_DestroyPolicy> destroyPolicies = null;


            await Task.Run(() => _destroyPolicyService.GetDestroyPolicyBySearchParam(_DestroyPolicyID,
                UserID, _OwnerID, _DocCategoryID, _DocTypeID, _DocPropertyID, _DocPropIdentityID,
                out destroyPolicies));


            var result = destroyPolicies
                .GroupBy(p => p.DestroyPolicyID)
                .Select(g => g.First())
                .ToList();


            return Json(result, JsonRequestBehavior.AllowGet);
        }

        
        [Authorize]
        [SILLogAttribute]
        public async Task<dynamic> SetDocDestroyPolicy(DSM_DestroyPolicy model)
        {
            if (ModelState.IsValid)
            {
                action = "add";
                model.SetBy = UserID;
                model.ModifiedBy = model.SetBy;
                respStatus = await Task.Run(() => _destroyPolicyService.SetDocDestroyPolicy(model, action, out outStatus));

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