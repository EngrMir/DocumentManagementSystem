using System.Collections.Generic;
using SILDMS.DataAccessInterface.VersioningOfOriginalDoc;
using SILDMS.Model.DocScanningModule;
using SILDMS.Utillity;
using SILDMS.Utillity.Localization;

namespace SILDMS.Service.VersioningOfOriginalDoc
{
    public class VersioningOfOriginalDocService:IVersioningOfOriginalDocService
    {
        private readonly IVersioningOfOriginalDocDataService _versioningOfOriginalDocDataService;
        private readonly ILocalizationService _localizationService;
        private string _errorNumber = string.Empty;

        public VersioningOfOriginalDocService(IVersioningOfOriginalDocDataService 
            versioningOfOriginalDocDataService, ILocalizationService localizationService)
        {
            _versioningOfOriginalDocDataService = versioningOfOriginalDocDataService;
            _localizationService = localizationService;
        }

        public ValidationResult AddVersionDocumentInfo(DocumentsInfo _modelDocumentsInfo,
            string action, out DSM_DocPropIdentify docPropIdentifyList)
        {
            docPropIdentifyList = _versioningOfOriginalDocDataService.AddVersionDocumentInfo
                (_modelDocumentsInfo, action, out _errorNumber);

            if (_errorNumber.Length > 0)
            {
                return new ValidationResult(_errorNumber, _localizationService.GetResource(_errorNumber));
            }

            return ValidationResult.Success;
        }

        public ValidationResult DeleteVersionDocumentInfo(string _DocumentIDs)
        {
            _versioningOfOriginalDocDataService.DeleteVersionDocumentInfo(_DocumentIDs, out _errorNumber);

            if (_errorNumber == "S201")
            {
                return ValidationResult.Success;
            }
            else
            {
                return ValidationResult.Success;
            }
        }
    }
}
