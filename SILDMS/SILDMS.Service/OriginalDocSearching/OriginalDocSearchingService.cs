using System.Collections.Generic;
using SILDMS.DataAccessInterface.OriginalDocSearching;
using SILDMS.Model.DocScanningModule;
using SILDMS.Utillity;
using SILDMS.Utillity.Localization;

namespace SILDMS.Service.OriginalDocSearching
{
    public class OriginalDocSearchingService: IOriginalDocSearchingService
    {
        private readonly IOriginalDocSearchingDataService _originalDocSearchingDataService;
        private readonly ILocalizationService _localizationService;

        private string errorNumber = "";
        public OriginalDocSearchingService(IOriginalDocSearchingDataService
            originalDocSearchingDataService, ILocalizationService localizationService)
        {
            _originalDocSearchingDataService = originalDocSearchingDataService;
            _localizationService = localizationService;
        }


        public ValidationResult UpdateDocMetaInfo(DocumentsInfo _modelDocumentsInfo,
            string _UserID, out string outStatus)
        {
            _originalDocSearchingDataService.UpdateDocMetaInfo(_modelDocumentsInfo, _UserID,
                out outStatus);
            if (outStatus.Length > 0)
            {
                return new ValidationResult(outStatus, _localizationService.GetResource(outStatus));
            }
            return ValidationResult.Success;
        }

        public ValidationResult GetOriginalDocBySearchParam(string _OwnerID, string _DocCategoryID,
            string _DocTypeID, string _DocPropertyID, string _SearchBy, string _UserID,
            out List<Model.DocScanningModule.DocSearch> docList)
        {
            docList = _originalDocSearchingDataService.GetOriginalDocBySearchParam(_OwnerID, _DocCategoryID, _DocTypeID,
                _DocPropertyID, _SearchBy, _UserID, out errorNumber);
            if (errorNumber.Length > 0)
            {
                return new ValidationResult(errorNumber, _localizationService.GetResource(errorNumber));
            }
            return ValidationResult.Success;
        }
    }
}
