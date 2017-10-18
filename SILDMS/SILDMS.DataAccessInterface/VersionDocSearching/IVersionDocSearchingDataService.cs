using SILDMS.Model.DocScanningModule;
using System.Collections.Generic;


namespace SILDMS.DataAccessInterface.VersionDocSearching
{
    public interface IVersionDocSearchingDataService
    {
        List<DocSearch> GetVersionDocBySearchParam(string _OwnerID, string _DocCategoryID, 
            string _DocTypeID, string _DocPropertyID, string _SearchBy, string _UserID, out string errorNumber);

        string UpdateDocMetaInfo(DocumentsInfo _modelDocumentsInfo, string userId,
            out string outStatus);
    }
}
