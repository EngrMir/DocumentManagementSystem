using SILDMS.Model.DocScanningModule;
using SILDMS.Utillity;
using System.Collections.Generic;


namespace SILDMS.Service.VersionDocSearching
{
    public interface IVersionDocSearchingService  
    {
        ValidationResult GetVersionDocBySearchParam(string _OwnerID, string _DocCategoryID,
            string _DocTypeID, string _DocPropertyID, string _SearchBy, string _UserID, out List<DocSearch> userList);

        ValidationResult UpdateDocMetaInfo(DocumentsInfo _modelDocumentsInfo, string _UserID,
            out string outStatus);
    }
}
