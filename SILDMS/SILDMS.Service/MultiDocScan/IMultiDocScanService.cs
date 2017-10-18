using System.Collections.Generic;
using SILDMS.Model.DocScanningModule;
using SILDMS.Utillity;

namespace SILDMS.Service.MultiDocScan
{
    public interface IMultiDocScanService
    {
        ValidationResult AddDocumentInfo(DocumentsInfo _modelDocumentsInfo,
            string _selectedPropID, List<DocMetaValue> _docMetaValues, string action,
            out List<DSM_DocPropIdentify> docPropIdentifyList);

        ValidationResult GetAllDocumentsInfo(string _UserID, string _DocumentID, 
            out List<DSM_Documents> dsmDocuments);

        ValidationResult GetAllDocumentsMetaInfo(string _UserID, string _DocMetaID,
            out List<DSM_DocumentsMeta> dsmDocumentsMetas);

        ValidationResult DeleteDocumentInfo(string _DocumentIDs);

    }
}
