using System.Collections.Generic;
using SILDMS.Model.DocScanningModule;
using SILDMS.Utillity;

namespace SILDMS.Service.OriginalDocSearching
{
    public interface IOriginalDocSearchingService
    {
        ValidationResult UpdateDocMetaInfo(DocumentsInfo _modelDocumentsInfo, string _UserID, out string outStatus);

        ValidationResult GetOriginalDocBySearchParam(string _OwnerID, string _DocCategoryID,
            string _DocTypeID, string _DocPropertyID, string _SearchBy, string _UserID, out List<DocSearch> userList);
    }
}
