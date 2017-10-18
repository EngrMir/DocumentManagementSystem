using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.SessionState;
using SILDMS.Model.DocScanningModule;
using SILDMS.Service.DocProperty;
using SILDMS.Service.MultiDocScan;
using SILDMS.Service.OwnerProperIdentity;
using SILDMS.Utillity;
using SILDMS.Utillity.Localization;
using SILDMS.Web.UI.Areas.SecurityModule;
using SILDMS.Web.UI.Areas.SecurityModule.Models;

namespace SILDMS.Web.UI.Areas.DocScanningModule.Controllers
{
   
    public class MultiDocScanController : Controller
    {
        private readonly IMultiDocScanService _multiDocScanService;
        private readonly IOwnerProperIdentityService _ownerProperIdentityService;
        
        
        private readonly IDocPropertyService _docPropertyService;
        private readonly ILocalizationService _localizationService;
        private ValidationResult respStatus = new ValidationResult();
        private readonly string UserID = string.Empty;
        
        private string action = "";


        public MultiDocScanController(IMultiDocScanService multiDocScanService, IOwnerProperIdentityService ownerProperIdentityRepository,
            IDocPropertyService docPropertyService, ILocalizationService localizationService)
        {
            _multiDocScanService = multiDocScanService;
            _ownerProperIdentityService = ownerProperIdentityRepository;
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
        public async Task<dynamic> GetDocPropIdentityForSelectedDocTypes(string _OwnerID, string _DocCategoryID,
             string _DocTypeID, string _SelectedPropID)
        {
            var SelectedPropID = _SelectedPropID.Split(',').Select(x => x).ToList();

            List<DSM_DocPropIdentify> objDocPropIdentifies = null;
            List<DSM_DocProperty> objDocPropertyList = null;
            

            await Task.Run(() => _ownerProperIdentityService.GetDocPropIdentify
                (UserID, "", out objDocPropIdentifies));

            await Task.Run(() => _docPropertyService.GetDocProperty
                ("", UserID, out objDocPropertyList));

            var result = (from dpi in objDocPropIdentifies.AsEnumerable()
                where dpi.OwnerID == _OwnerID & dpi.DocCategoryID == _DocCategoryID &
                      dpi.DocTypeID == _DocTypeID & dpi.Status == 1 &
                      SelectedPropID.Contains(dpi.DocPropertyID)

                join dp in objDocPropertyList on dpi.DocPropertyID equals dp.DocPropertyID

                orderby dp.SerialNo 
                select new DSM_DocPropIdentify
                {
                    DocPropIdentifyID = dpi.DocPropIdentifyID,
                    DocPropertyID = dpi.DocPropertyID,
                    DocPropertyName = dp.DocPropertyName,
                    //DocTypeID = dpi.DocTypeID,
                    //DocCategoryID = dpi.DocCategoryID,
                    //OwnerID = dpi.OwnerID,
                    //IdentificationCode = dpi.IdentificationCode,
                    //IdentificationSL = dpi.IdentificationSL,
                    IdentificationAttribute = dpi.IdentificationAttribute,
                    //AttributeGroup = dpi.AttributeGroup,
                    IsRequired = dpi.IsRequired,
                    IsAuto = dpi.IsAuto,
                    //Remarks = dpi.Remarks,
                    Status = dpi.Status,
                    IsRestricted = dpi.IsRequired
                }).ToList();

            return Json(new { Msg = "", result }, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        public async Task<dynamic> GetDocumentProperty(string _DocCategoryID,
            string _OwnerID, string _DocTypeID)
        {
            List<DSM_DocProperty> objDsmDocProperties = null;
            List<DSM_DocPropIdentify> objDocPropIdentifies = null;

            await Task.Run(() => _docPropertyService.GetDocProperty("", UserID, out objDsmDocProperties));
            await Task.Run(() => _ownerProperIdentityService.GetDocPropIdentify(UserID, "", out objDocPropIdentifies));

            var joinResult = (from dp in objDsmDocProperties
                where dp.OwnerID == _OwnerID & dp.DocCategoryID == _DocCategoryID &
                      dp.DocTypeID == _DocTypeID & dp.Status == 1

                join dpi in objDocPropIdentifies on dp.DocPropertyID equals dpi.DocPropertyID into docPropIdentities
                from dpi in docPropIdentities.DefaultIfEmpty()

                select new
                {
                    DocPropertyID = dp.DocPropertyID,
                    DocPropertyName = dp.DocPropertyName,
                    IsSelected = true
                }).ToList();

            var result = (from s in joinResult
                          group s by new
                          {
                              s.DocPropertyID
                          }
                              into g
                              select new
                              {
                                  DocPropertyID = g.Select(p => p.DocPropertyID).FirstOrDefault(),
                                  DocPropertyName = g.Select(x => x.DocPropertyName).FirstOrDefault(),
                                  IsSelected = true
                              }).ToList();

            
            return Json(new { Msg = "", result }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [Authorize]
        [SILLogAttribute]
        public async Task<dynamic> AddDocumentInfo(DocumentsInfo _modelDocumentsInfo,
            string _selectedPropID, List<DocMetaValue> _docMetaValues)
        {
            List<DSM_DocPropIdentify> objDocPropIdentifies = null;

            if (ModelState.IsValid)
            {
                action = "add";
                _modelDocumentsInfo.SetBy = UserID;
                _modelDocumentsInfo.ModifiedBy = _modelDocumentsInfo.SetBy;
                _modelDocumentsInfo.UploaderIP = GetIPAddress.LocalIPAddress();
                respStatus.Message = "Success";
                respStatus = await Task.Run(() => _multiDocScanService.AddDocumentInfo(_modelDocumentsInfo, _selectedPropID, _docMetaValues, action, out objDocPropIdentifies));


                var DistinctDocIDs1 = (from s in objDocPropIdentifies
                    group s by new
                    {
                        s.DocumentID
                    }
                    into g
                    select new
                    {
                        DocPropID = g.Select(p => p.DocPropertyID).FirstOrDefault(),
                        DocumentID = g.Select(p => p.DocumentID).FirstOrDefault(),
                        FileServerUrl = g.Select(x => x.FileServerUrl).FirstOrDefault()
                    }).ToList();

                List<DSM_DocProperty> proplList = new List<DSM_DocProperty>();
                string[] docPropIDs = _selectedPropID.Split(',');

                foreach (var item in docPropIDs)
                {
                    DSM_DocProperty objDocProperty = new DSM_DocProperty();
                    objDocProperty.DocPropertyID = item;

                    proplList.Add(objDocProperty);
                }


                var DistinctDocIDs = (from p in proplList
                    join d in DistinctDocIDs1 on p.DocPropertyID equals d.DocPropID
                    select new
                    {
                        DocPropID = d.DocPropID,
                        DocumentID = d.DocumentID,
                        FileServerUrl = d.FileServerUrl
                    }).ToList();

                foreach (var item in DistinctDocIDs)
                {
                    try
                    {
                        FolderGenerator.MakeFTPDir(objDocPropIdentifies.FirstOrDefault().ServerIP,
                            objDocPropIdentifies.FirstOrDefault().ServerPort,
                            item.FileServerUrl,
                            objDocPropIdentifies.FirstOrDefault().FtpUserName,
                            objDocPropIdentifies.FirstOrDefault().FtpPassword);
                    }
                    catch (Exception e)
                    {


                    }

                }

                return Json(new
                {
                    Message = respStatus.Message,
                    result = objDocPropIdentifies,
                    DistinctID = DistinctDocIDs
                }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                //var errors = ModelState.Values.SelectMany(v => v.Errors);
                respStatus = new ValidationResult("E404", _localizationService.GetResource("E404"));
                return Json(new { Message = respStatus.Message, respStatus }, JsonRequestBehavior.AllowGet);
            }
            

        }

        public async Task<dynamic> DeleteDocumentInfo(string _DocumentIDs)
        {
            respStatus = await Task.Run(() => _multiDocScanService.DeleteDocumentInfo(_DocumentIDs));
            return Json(null, JsonRequestBehavior.AllowGet);
        }

	}
}