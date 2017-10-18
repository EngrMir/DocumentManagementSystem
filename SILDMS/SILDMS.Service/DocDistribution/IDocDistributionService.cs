using SILDMS.Model.DocScanningModule;
using SILDMS.Utillity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SILDMS.Service.DocDistribution
{
    public interface IDocDistributionService
    {
        //ValidationResult AddDocumentInfo(DocumentsInfo _modelDocumentsInfo,string _selectedPropID, List<DocMetaValue> _docMetaValues, string action,out List<DSM_DocPropIdentify> docPropIdentifyList);

        //ValidationResult AddDocumentInfoForVersion(DocumentsInfo modelDocumentsInfo, string selectedPropId, List<DocMetaValue> lstMetaValues, string action, out List<DSM_DocPropIdentify> objDocPropIdentifies);

        ValidationResult AddDocumentInfoForVersion(DocumentsInfo modelDocumentsInfo, string selectedPropId, List<DocMetaValue> lstMetaValues, string action, out string outStatus);

        ValidationResult AddDocumentInfo(DocumentsInfo modelDocumentsInfo, string selectedPropId, List<DocMetaValue> lstMetaValues, string action, out string outStatus);
    }
}
