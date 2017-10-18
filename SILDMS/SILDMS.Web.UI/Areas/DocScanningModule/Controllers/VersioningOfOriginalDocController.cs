using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using SILDMS.Model.DocScanningModule;
using SILDMS.Service.OwnerProperIdentity;
using SILDMS.Service.VersioningOfOriginalDoc;
using SILDMS.Utillity;
using SILDMS.Utillity.Localization;
using SILDMS.Web.UI.Areas.SecurityModule.Models;
using SILDMS.Web.UI.Areas.SecurityModule;

namespace SILDMS.Web.UI.Areas.DocScanningModule.Controllers
{
    public class VersioningOfOriginalDocController : Controller
    {
        
        private readonly IVersioningOfOriginalDocService _versioningOfOriginalDocService;
        readonly IOwnerProperIdentityService _DocPropIdentityService;

        
        private readonly ILocalizationService _localizationService;
        private ValidationResult respStatus = new ValidationResult();
        private readonly string UserID = string.Empty;
        private string action = "";

        public VersioningOfOriginalDocController(ILocalizationService localizationService,
            IVersioningOfOriginalDocService versioningOfOriginalDocService,
            IOwnerProperIdentityService docPropIdentityService)
        {
            UserID = SILAuthorization.GetUserID();
            _localizationService = localizationService;
            _versioningOfOriginalDocService = versioningOfOriginalDocService;
            _DocPropIdentityService = docPropIdentityService;
        }



        [HttpPost]
        [Authorize]
        [SILLogAttribute]
        public async Task<dynamic> AddVersionDocumentInfo(DocumentsInfo _modelDocumentsInfo)
        {
            DSM_DocPropIdentify objDocPropIdentifies = new DSM_DocPropIdentify();

            if (ModelState.IsValid)
            {
                action = "add";
                _modelDocumentsInfo.SetBy = UserID;
                _modelDocumentsInfo.ModifiedBy = _modelDocumentsInfo.SetBy;
                _modelDocumentsInfo.UploaderIP = GetIPAddress.LocalIPAddress();
                respStatus.Message = "Success";
                respStatus = await Task.Run(() => _versioningOfOriginalDocService.AddVersionDocumentInfo(
                    _modelDocumentsInfo, action, out objDocPropIdentifies));

                    try
                    {
                        FolderGenerator.MakeFTPDir(objDocPropIdentifies.ServerIP,
                            objDocPropIdentifies.ServerPort,
                            objDocPropIdentifies.FileServerUrl,
                            objDocPropIdentifies.FtpUserName,
                            objDocPropIdentifies.FtpPassword);
                    }
                    catch (Exception e)
                    {


                    }


                return Json(new
                {
                    Message = respStatus.Message,
                    result = objDocPropIdentifies,
                    //DistinctID = DistinctDocIDs
                }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                //var errors = ModelState.Values.SelectMany(v => v.Errors);
                respStatus = new ValidationResult("E404", _localizationService.GetResource("E404"));
                return Json(new { Message = respStatus.Message, respStatus }, JsonRequestBehavior.AllowGet);
            }


        }

        [Authorize]
        public async Task<dynamic> GetDocPropIdentityForSpecificDocType(string _OwnerID, string _DocCategoryID,
             string _DocTypeID, string _DocPropertyID)
        {
            List<DSM_DocPropIdentify> objDocPropIdentifies = null;

            await Task.Run(() => _DocPropIdentityService.GetDocPropIdentify(UserID, "", out objDocPropIdentifies));

            var result = (from dp in objDocPropIdentifies
                          where dp.OwnerID == _OwnerID & dp.DocCategoryID == _DocCategoryID &
                                dp.DocTypeID == _DocTypeID & dp.DocPropertyID == _DocPropertyID
                                & dp.Status == 1

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

        public async Task<dynamic> DeleteVersionDocumentInfo(string _DocumentIDs)
        {
            respStatus = await Task.Run(() => _versioningOfOriginalDocService.DeleteVersionDocumentInfo(_DocumentIDs));
            return Json(null, JsonRequestBehavior.AllowGet);
        }

	}
}