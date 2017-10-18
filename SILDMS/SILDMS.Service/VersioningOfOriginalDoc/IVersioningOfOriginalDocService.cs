using System.Collections.Generic;
using SILDMS.Model.DocScanningModule;
using SILDMS.Utillity;

namespace SILDMS.Service.VersioningOfOriginalDoc
{
    public interface IVersioningOfOriginalDocService
    {
        ValidationResult AddVersionDocumentInfo(DocumentsInfo _modelDocumentsInfo,
            string action, out DSM_DocPropIdentify docPropIdentifyList);

        ValidationResult DeleteVersionDocumentInfo(string _DocumentIDs);
    }
}
