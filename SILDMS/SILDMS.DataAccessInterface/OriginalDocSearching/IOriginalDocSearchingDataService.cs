using System.Collections.Generic;
using SILDMS.Model.DocScanningModule;

namespace SILDMS.DataAccessInterface.OriginalDocSearching
{
    public interface IOriginalDocSearchingDataService
    {
        string UpdateDocMetaInfo(DocumentsInfo _modelDocumentsInfo, string userId, 
            out string outStatus);

        List<DocSearch> GetOriginalDocBySearchParam(string _OwnerID, string _DocCategoryID,
            string _DocTypeID, string _DocPropertyID, string _SearchBy, string _UserID, out string errorNumber);
    }
}
