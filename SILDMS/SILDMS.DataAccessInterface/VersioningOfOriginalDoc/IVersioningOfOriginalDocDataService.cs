using System.Collections.Generic;
using SILDMS.Model.DocScanningModule;

namespace SILDMS.DataAccessInterface.VersioningOfOriginalDoc
{
    public interface IVersioningOfOriginalDocDataService
    {
        DSM_DocPropIdentify AddVersionDocumentInfo(DocumentsInfo _modelDocumentsInfo,
            string _action, out string _errorNumber);

        string DeleteVersionDocumentInfo(string _DocumentIDs, out string _errorNumber);
    }
}
