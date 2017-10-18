using System;
using SILDMS.Model.DocScanningModule;
using SILDMS.Service.VersionDocSearching;
using SILDMS.Web.UI.Areas.SecurityModule;
using SILDMS.Web.UI.Areas.SecurityModule.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.SessionState;
using Microsoft.Ajax.Utilities;
using SILDMS.Service.OriginalDocSearching;
using SILDMS.Utillity;

namespace SILDMS.Web.UI.Areas.DocScanningModule.Controllers
{
  
    public class VersionDocSearchingController : Controller
    {
        private ValidationResult respStatus = new ValidationResult();

        private readonly IVersionDocSearchingService _versionDocSearchingService;
        private readonly IOriginalDocSearchingService _originalDocSearchingService;
        private readonly string UserID = string.Empty;
        private string outStatus = string.Empty;
        public VersionDocSearchingController(IOriginalDocSearchingService originalDocSearchingService,
            IVersionDocSearchingService versionDocSearchingService)
        {
            _originalDocSearchingService = originalDocSearchingService;
            _versionDocSearchingService = versionDocSearchingService;
            UserID = SILAuthorization.GetUserID();
        }

        [SILAuthorize]
        public ActionResult Index()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        public async Task<dynamic> GetVersionDocBySearchParam(string _OwnerID, string _DocCategoryID, 
            string _DocTypeID, string _DocPropertyID, string _SearchBy)
        {
            

            List<DocSearch> OriginalMeta = null;
            if (!string.IsNullOrEmpty(_OwnerID) && !string.IsNullOrEmpty(_DocCategoryID) &&
                !string.IsNullOrEmpty(_DocTypeID) && !string.IsNullOrEmpty(_DocPropertyID) &&
                !string.IsNullOrEmpty(_SearchBy))
            {
                await Task.Run(() => _originalDocSearchingService.GetOriginalDocBySearchParam
                    (_OwnerID, _DocCategoryID, _DocTypeID,
                        _DocPropertyID, _SearchBy, UserID, out OriginalMeta));
            }

            var OriginalResult = (from r in OriginalMeta
                group r by new
                {
                    r.DocumentID
                    //r.DocDistributionID
                }
                into g

                select new
                {
                    DocumentID = g.Key.DocumentID,
                    MetaValue = String.Join(", ", g.Select(o => o.MetaValue)),
                    DocPropIdentifyID = String.Join(",", g.Select(o => o.DocPropIdentifyID).Distinct()),
                    DocPropIdentifyName = String.Join(",", g.Select(o => o.DocPropIdentifyName).Distinct()),
                    FileServerURL = g.Select(o => o.FileServerURL).FirstOrDefault(),
                    ServerIP = g.Select(o => o.ServerIP).FirstOrDefault(),
                    ServerPort = g.Select(o => o.ServerPort).FirstOrDefault(),
                    FtpUserName = g.Select(o => o.FtpUserName).FirstOrDefault(),
                    FtpPassword = g.Select(o => o.FtpPassword).FirstOrDefault()
                });

            List<DocSearch> VersionMeta = null;
            if (!string.IsNullOrEmpty(_OwnerID) && !string.IsNullOrEmpty(_DocCategoryID) &&
                !string.IsNullOrEmpty(_DocTypeID) && !string.IsNullOrEmpty(_DocPropertyID) &&
                !string.IsNullOrEmpty(_SearchBy))
            {
                await Task.Run(() => _versionDocSearchingService.GetVersionDocBySearchParam(_OwnerID,
                    _DocCategoryID, _DocTypeID, _DocPropertyID, _SearchBy, UserID, out VersionMeta));
            }

            var Versionresult = (from r in VersionMeta
                group r by new
                {
                    r.DocVersionID,
                    r.DocDistributionID
                }

                into g

                select new
                {
                    DocVersionID = g.Key.DocVersionID,
                    DocumentID = g.Select(o => o.DocumentID).FirstOrDefault(),
                    MetaValue = String.Join(", ", g.Select(o => o.MetaValue).Distinct()),
                    OriginalReference = String.Join(", ", g.Select(o => o.OriginalReference).Distinct()),
                    DocPropIdentifyID = String.Join(",", g.Select(o => o.DocPropIdentifyID).Distinct()),
                    DocPropIdentifyName = String.Join(",", g.Select(o => o.DocPropIdentifyName).Distinct()),
                    FileServerURL = g.Select(o => o.FileServerURL).FirstOrDefault(),
                    ServerIP = g.Select(o => o.ServerIP).FirstOrDefault(),
                    ServerPort = g.Select(o => o.ServerPort).FirstOrDefault(),
                    FtpUserName = g.Select(o => o.FtpUserName).FirstOrDefault(),
                    FtpPassword = g.Select(o => o.FtpPassword).FirstOrDefault(),
                    VersionNo = g.Select(o => o.VersionNo).FirstOrDefault()
                });

            var result = (from v in Versionresult
                join o in OriginalResult on v.DocumentID equals o.DocumentID into FinalResult
                from f in FinalResult.DefaultIfEmpty()
                
                select new
                {
                    DocVersionID = v.DocVersionID,
                    DocumentID = v.DocumentID,
                    MetaValue = v.MetaValue,
                    OriginalReference = f.MetaValue,
                    DocPropIdentifyID = v.DocPropIdentifyID,
                    DocPropIdentifyName = v.DocPropIdentifyName,
                    FileServerURL = v.FileServerURL,
                    ServerIP = v.ServerIP,
                    ServerPort = v.ServerPort,
                    FtpUserName = v.FtpUserName,
                    FtpPassword = v.FtpPassword,
                    VersionNo = v.VersionNo
                }).DistinctBy(x=>x.DocVersionID).ToList();


            return Json(new { Message = "", result }, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        [HttpPost]
        [SILLogAttribute]
        public async Task<dynamic> UpdateDocMetaInfo(DocumentsInfo _modelDocumentsInfo)
        {
            //Session["UserID"].ToString();
            respStatus = await Task.Run(() => _versionDocSearchingService.UpdateDocMetaInfo(_modelDocumentsInfo, UserID, out outStatus));
            return Json(new { Message = respStatus.Message, respStatus }, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        public async Task<dynamic> GetDocPropIdentityForSpecificDocType(string _OwnerID,
            string _DocCategoryID, string _DocTypeID, string _DocPropertyID, string _DocVersionID,
            string _SearchBy)
        {
            List<DocSearch> lstDocSearch = null;
            if (!string.IsNullOrEmpty(_OwnerID) && !string.IsNullOrEmpty(_DocCategoryID)
                && !string.IsNullOrEmpty(_DocTypeID) && !string.IsNullOrEmpty(_DocPropertyID)
                && !string.IsNullOrEmpty(_SearchBy))
            {
                await Task.Run(() => _versionDocSearchingService.GetVersionDocBySearchParam
                    (_OwnerID, _DocCategoryID, _DocTypeID, _DocPropertyID, _SearchBy, UserID,
                    out lstDocSearch));
            }
            var result = (from r in lstDocSearch
                          where r.DocVersionID == _DocVersionID
                          select new
                          {
                              DocMetaID = r.DocMetaIDVersion,
                              //DocPropIdentifyID = r.DocPropIdentifyID,
                              DocPropIdentifyName = r.DocPropIdentifyName,
                              MetaValue = r.MetaValue
                          }).ToList();


            return Json(result, JsonRequestBehavior.AllowGet);
        }
	}
}