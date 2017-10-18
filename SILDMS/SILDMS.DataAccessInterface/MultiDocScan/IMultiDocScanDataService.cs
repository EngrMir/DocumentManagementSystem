using System.Collections.Generic;
using SILDMS.Model.DocScanningModule;

namespace SILDMS.DataAccessInterface.MultiDocScan
{
    public interface IMultiDocScanDataService
    {
        List<DSM_DocPropIdentify> AddDocumentInfo(DocumentsInfo _modelDocumentsInfo,
            string _selectedPropID, List<DocMetaValue> _docMetaValues,
            string _action, out string _errorNumber);

        List<DSM_Documents> GetAllDocumentsInfo(string _UserID, string _DocumentID, 
            out string errorNumber);
        List<DSM_DocumentsMeta> GetAllDocumentsMetaInfo(string _UserID, string _DocMetaID,
            out string errorNumber);

        string DeleteDocumentInfo(string _DocumentIDs, out string _errorNumber);
    }
}
