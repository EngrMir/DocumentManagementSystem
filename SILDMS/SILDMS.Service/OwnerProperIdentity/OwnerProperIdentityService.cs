using System.Collections.Generic;
using SILDMS.DataAccessInterface.OwnerProperIdentity;
using SILDMS.Model.DocScanningModule;
using SILDMS.Utillity;
using SILDMS.Utillity.Localization;

namespace SILDMS.Service.OwnerProperIdentity
{
    public class OwnerProperIdentityService: IOwnerProperIdentityService
    {
        private readonly IOwnerProperIdentityDataService _OwnerProperIdentityDataService;
        private readonly ILocalizationService _localizationService;
        private string _errorNumber = string.Empty;

        public OwnerProperIdentityService(IOwnerProperIdentityDataService ownerProperIdentityDataService,
            ILocalizationService localizationService)
        {
            _OwnerProperIdentityDataService = ownerProperIdentityDataService;
            _localizationService = localizationService;
        }

        public ValidationResult GetDocPropIdentify(string _UserID, string docPropIdentifyID,
            out List<DSM_DocPropIdentify> docPropIdentifyList)
        {
            docPropIdentifyList = _OwnerProperIdentityDataService.GetDocPropIdentify
                (_UserID, docPropIdentifyID, out _errorNumber);

            if (_errorNumber.Length > 0)
            {
                return new ValidationResult(_errorNumber, _localizationService.GetResource(_errorNumber));
            }

            return ValidationResult.Success;
        }

        public ValidationResult AddOwnerPropIdentity(DSM_DocPropIdentify modelDsmDocPropIdentify,
            string action, out string status)
        {
            _OwnerProperIdentityDataService.AddOwnerPropIdentity(modelDsmDocPropIdentify, action,
                out status);
            if (status.Length > 0)
            {
                return new ValidationResult(status, _localizationService.GetResource(status));
            }
            return ValidationResult.Success;
        }

        public ValidationResult EditOwnerPropIdentity(DSM_DocPropIdentify modelDsmDocPropIdentify,
            string action, out string status)
        {
            _OwnerProperIdentityDataService.AddOwnerPropIdentity(modelDsmDocPropIdentify, action,
                out status);
            if (status.Length > 0)
            {
                return new ValidationResult(status, _localizationService.GetResource(status));
            }
            return ValidationResult.Success;
        }
    }
}
