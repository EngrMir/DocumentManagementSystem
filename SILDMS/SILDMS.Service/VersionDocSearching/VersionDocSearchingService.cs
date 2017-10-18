using SILDMS.DataAccessInterface.VersionDocSearching;
using SILDMS.Utillity;
using SILDMS.Utillity.Localization;
using System.Collections.Generic;
using SILDMS.Model.DocScanningModule;


namespace SILDMS.Service.VersionDocSearching
{
    public class VersionDocSearchingService : IVersionDocSearchingService
    {
        #region Fields
        private readonly ILocalizationService _localizationService;
        private readonly IVersionDocSearchingDataService _versionDocSearchingDataService;
        private string errorNumber = "";
        #endregion

        #region Constractor
        public VersionDocSearchingService(
             IVersionDocSearchingDataService versionDocSearchingDataService,
             ILocalizationService localizationService
            )
        {
            this._versionDocSearchingDataService = versionDocSearchingDataService;
            this._localizationService = localizationService;
        }
       
        #endregion

        public ValidationResult GetVersionDocBySearchParam(string _OwnerID, string _DocCategoryID,
            string _DocTypeID, string _DocPropertyID, string _SearchBy, string _UserID,
            out List<Model.DocScanningModule.DocSearch> docList)
        {
            docList = _versionDocSearchingDataService.GetVersionDocBySearchParam(_OwnerID, _DocCategoryID, _DocTypeID,
                _DocPropertyID, _SearchBy, _UserID, out errorNumber);
            if (errorNumber.Length > 0)
            {
                return new ValidationResult(errorNumber, _localizationService.GetResource(errorNumber));
            }
            return ValidationResult.Success;
        }


        public ValidationResult UpdateDocMetaInfo(DocumentsInfo _modelDocumentsInfo,
            string _UserID, out string outStatus)
        {
            _versionDocSearchingDataService.UpdateDocMetaInfo(_modelDocumentsInfo, _UserID,
                out outStatus);
            if (outStatus.Length > 0)
            {
                return new ValidationResult(outStatus, _localizationService.GetResource(outStatus));
            }
            return ValidationResult.Success;
        }
    }
}
