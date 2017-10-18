


using MvcSiteMapProvider.Globalization;
using SILDMS.Model.DocScanningModule;
using SILDMS.Model.SecurityModule;
using SILDMS.Service.MultiDocScan;
using SILDMS.Utillity;
using SILDMS.Web.UI.Areas.SecurityModule.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using SILDMS.Utillity;
using SILDMS.Utillity.Localization;
using SILDMS.Service.OwnerProperIdentity;
using SILDMS.Service.DocDistribution;
using SILDMS.Service.OriginalDocSearching;
using SILDMS.Service.VersionDocSearching;
using SILDMS.Web.UI.Areas.SecurityModule;

namespace SILDMS.Web.UI.Areas.DocScanningModule.Controllers
{
    public class DocDistributionController : Controller
    {
        private SILDMS.Utillity.ValidationResult _respStatus = new SILDMS.Utillity.ValidationResult();
        private readonly SILDMS.Utillity.Localization.ILocalizationService _localizationService;
        private readonly IDocDistributionService _docDistributionService;
        private readonly IOwnerProperIdentityService _docPropertyIdentityService;
        private readonly IVersionDocSearchingService _versionDocScanService;
        private readonly IOriginalDocSearchingService _originalDocSearchingService;
        private readonly string _userId;
        private string outStatus = string.Empty;
        public DocDistributionController(IOriginalDocSearchingService originalDocSearchingService,IVersionDocSearchingService versionDocScanService, IDocDistributionService docDistributionService, IOwnerProperIdentityService docPropertyIdentityService, SILDMS.Utillity.Localization.ILocalizationService localizationService)
        {
            _originalDocSearchingService = originalDocSearchingService;
            _versionDocScanService = versionDocScanService;
            _docDistributionService = docDistributionService;
            _docPropertyIdentityService = docPropertyIdentityService;
            _localizationService = localizationService;
            _userId = SILAuthorization.GetUserID();
        }

        [HttpPost]
        [Authorize]
        public string UploadHandler()
        {
            HttpPostedFile httpPostedFileBase = System.Web.HttpContext.Current.Request.Files[0];
            if (httpPostedFileBase != null)
            {
                string[] file = httpPostedFileBase.FileName.Split('.');
                if (file.Length > 0)
                {
                    if ((file[file.Length - 1].ToString()) == "xlsx" || (file[file.Length - 1].ToString() == "xls"))
                    {
                        DataTable dt;
                        ExcelFileReader xlReader = new ExcelFileReader();
                        dt = xlReader.GetExcelDataTable(HttpContext.Request.Files[0]);
                        TempData["ExcelData"] = dt;

                        return httpPostedFileBase.FileName;
                    }

                }
            }


            else
            {
                return "1";
            }
            return "0";
        }

        //[HttpPost]
        //public string UploadHandler()
        //{
        //    // Checking no of files injected in Request object  
        //    if (Request.Files.Count > 0)
        //    {
        //        try
        //        {
        //            //  Get all files from Request object  
        //            HttpFileCollectionBase files = Request.Files;
        //            for (int i = 0; i < files.Count; i++)
        //            {
        //                //string path = AppDomain.CurrentDomain.BaseDirectory + "Uploads/";  
        //                //string filename = Path.GetFileName(Request.Files[i].FileName);  

        //                HttpPostedFileBase file = files[i];
        //                string fname;

        //                string[] filePart = file.FileName.Split('.');

        //                if ((filePart[filePart.Length - 1].ToString()) == "xlsx" || (filePart[filePart.Length - 1].ToString() == "xls"))
        //                {
        //                    DataTable dt;
        //                    ExcelFileReader xlReader = new ExcelFileReader();
        //                    dt = xlReader.GetExcelDataTable(HttpContext.Request.Files[0]);
        //                    TempData["ExcelData"] = dt;

        //                    return file.FileName;
        //                }

                            
                        

        //                // Get the complete folder path and store the file inside it.  
        //                //fname = Path.Combine(Server.MapPath("~/Uploads/"), fname);
        //                //file.SaveAs(fname);
        //            }
        //            // Returns message that successfully uploaded  
        //            return "File Uploaded Successfully!";
        //        }
        //        catch (Exception ex)
        //        {
        //            return "Error occurred. Error details: " + ex.Message;
        //        }
        //    }
        //    else
        //    {
        //        return "No files selected.";
        //    }
        //}  




        [Authorize]
        [HttpPost]
        public async Task<dynamic> GetVersionDocBySearchParam(string _OwnerID, string _DocCategoryID,string _DocTypeID, string _DocPropertyID, string _SearchBy)
        {
            List<DocSearch> lstDocSearch = null;
            if (!string.IsNullOrEmpty(_OwnerID) && !string.IsNullOrEmpty(_DocCategoryID) && !string.IsNullOrEmpty(_DocTypeID) && !string.IsNullOrEmpty(_DocPropertyID) && !string.IsNullOrEmpty(_SearchBy))
            {
                await Task.Run(() => _versionDocScanService.GetVersionDocBySearchParam(_OwnerID,_DocCategoryID, _DocTypeID, _DocPropertyID, _SearchBy, _userId, out lstDocSearch));
            }
            var result = (from r in lstDocSearch
                          select new
                          {
                              DocMetaID = r.DocMetaID,
                              DocumentID = r.DocumentID,
                              DocVersionID = r.DocVersionID,
                              DocPropIdentifyID = r.DocPropIdentifyID,
                              DocPropIdentifyName = r.DocPropIdentifyName,
                              MetaValue = r.MetaValue,
                              VersionMetaValue = "",
                              DocPropertyID = r.DocPropertyID,
                              DocPropertyName = r.DocPropertyName,
                              FileServerURL = r.FileServerURL,
                              ServerIP = r.ServerIP,
                              ServerPort = r.ServerPort,
                              FtpUserName = r.FtpUserName,
                              FtpPassword = r.FtpPassword
                          }).ToList();
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        
        [Authorize]
        [HttpPost]
        public async Task<dynamic> GetDocPropIdentityForSelectedDocTypes(string _OwnerID, string _DocCategoryID,string _DocTypeID, string _DocPropertyID, string _SearchBy)
        {

            List<DocSearch> lstDocSearch = null;
            if (!string.IsNullOrEmpty(_OwnerID) && !string.IsNullOrEmpty(_DocCategoryID) && !string.IsNullOrEmpty(_DocTypeID) && !string.IsNullOrEmpty(_DocPropertyID) && !string.IsNullOrEmpty(_SearchBy))
            {
                await Task.Run(() => _originalDocSearchingService.GetOriginalDocBySearchParam(_OwnerID, _DocCategoryID, _DocTypeID, _DocPropertyID, _SearchBy, _userId, out lstDocSearch));
            }

            var result = (from r in lstDocSearch
                          select new
                          {
                              DocMetaID = r.DocMetaID,
                              DocumentID = r.DocumentID,
                              DocPropIdentifyID = r.DocPropIdentifyID,
                              DocPropIdentifyName = r.DocPropIdentifyName,
                              MetaValue = r.MetaValue,
                              VersionMetaValue = "",
                              DocPropertyID = r.DocPropertyID,
                              DocPropertyName = r.DocPropertyName,
                              FileServerURL = r.FileServerURL,
                              ServerIP = r.ServerIP,
                              ServerPort = r.ServerPort,
                              FtpUserName = r.FtpUserName,
                              FtpPassword = r.FtpPassword
                          }).ToList();

                return Json(result, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        [Authorize]
        [SILLogAttribute]
        public dynamic AddDocumentInfo(DocumentsInfo modelDocumentsInfo, string selectedPropId, DocMetaValue docMetaValues)
        {
            List<DocMetaValue> lstMetaValues = new List<DocMetaValue>();
            List<DSM_DocPropIdentify> objDocPropIdentifies = new List<DSM_DocPropIdentify>();
            List<string> existColumnName = new List<string>();
            var action = "";
            if (ModelState.IsValid)
            {
                action = "add";
                modelDocumentsInfo.SetBy = _userId;
                modelDocumentsInfo.ModifiedBy = modelDocumentsInfo.SetBy;
                
                DataTable dt = (DataTable)TempData["ExcelData"];
                if (dt != null)
                {
                    _docPropertyIdentityService.GetDocPropIdentify(_userId, "", out objDocPropIdentifies);
                    var documentList = objDocPropIdentifies.Where(ob => ob.DocPropertyID == modelDocumentsInfo.DocProperty.DocPropertyID).Select(ob => ob.IdentificationAttribute).ToList();
                    var arrayNames = (from DataColumn x in dt.Columns select x.ColumnName).ToArray();

                    foreach (string item in arrayNames)
                    {
                        if (documentList.Contains(item))
                        {
                            existColumnName.Add(item);
                        }
                    }


                    if (existColumnName.Count > 0)
                    {
                        // Database and Import File match column
                        foreach (var columnName in existColumnName)
                        {
                            foreach (DataRow row in dt.Rows)
                            {
                                DocMetaValue ob = new DocMetaValue();
                                ob.DocumentID = docMetaValues.DocumentID;
                              
                                ob.MetaValue = row[columnName].ToString();
                                ob.DocPropIdentifyID = (
                                  from t in objDocPropIdentifies
                                  where (t.DocCategoryID == modelDocumentsInfo.DocCategory.DocCategoryID) &&
                                        (t.DocTypeID == modelDocumentsInfo.DocType.DocTypeID) &&
                                        (t.IdentificationAttribute == columnName)
                                  select t.DocPropIdentifyID).FirstOrDefault();// docMetaValues.DocPropIdentifyID;
                                ob.DocPropertyID = selectedPropId;
                                ob.DocMetaID = docMetaValues.DocMetaID;
                                ob.DocumentID = docMetaValues.DocumentID;
                                ob.DocVersionID = docMetaValues.DocVersionID;
                                lstMetaValues.Add(ob);
                            }
                        }


                        if (lstMetaValues.Count > 0)
                        {
                            if (modelDocumentsInfo.DidtributionOf.Equals("Original"))
                            {
                                _respStatus = _docDistributionService.AddDocumentInfo(modelDocumentsInfo, selectedPropId, lstMetaValues, action, out  outStatus);
                            }
                            else
                            {
                                _respStatus = _docDistributionService.AddDocumentInfoForVersion(modelDocumentsInfo, selectedPropId, lstMetaValues, action, out outStatus);
                            }
                            _respStatus = new SILDMS.Utillity.ValidationResult(outStatus, _localizationService.GetResource(outStatus));
                        }
                    }
                    else
                    {
                        _respStatus = new SILDMS.Utillity.ValidationResult("E411", _localizationService.GetResource("E411"));
                    }
                    
                }
            }
            else
            {
                //var errors = ModelState.Values.SelectMany(v => v.Errors);
                _respStatus = new SILDMS.Utillity.ValidationResult("E404", _localizationService.GetResource("E404"));
              //  return Json(new { Message = _respStatus.Message, _respStatus }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { Message = _respStatus.Message, _respStatus }, JsonRequestBehavior.AllowGet);
        }
    }
}