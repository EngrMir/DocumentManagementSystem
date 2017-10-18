using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SILDMS.DataAccessInterface;
using SILDMS.DataAccessInterface.DocDestroy;
using SILDMS.Model.DocScanningModule;
using SILDMS.Utillity;
using SILDMS.Utillity.Localization;

namespace SILDMS.Service.DocDestroy
{
    public class DocDestroyService : IDocDestroyService
    {
        private readonly IDocDestroyDataService _docDestroyDataService;
        private readonly ILocalizationService _localizationService;
        private string _errorNumber = string.Empty;

        public DocDestroyService(IDocDestroyDataService docDestroyDataService,
            ILocalizationService localizationService)
        {
            _docDestroyDataService = docDestroyDataService;
            _localizationService = localizationService;
        }

        public ValidationResult GetDestroyDetailsBySearchParam(string _DestroyID, string _UserID, string _OwnerID, string _DocCategoryID, string _DocTypeID,
            string _DocPropertyID, string _DocPropIdentityID, out List<DSM_DestroyPolicy> destroyPolicies)
        {
            destroyPolicies = _docDestroyDataService.GetDestroyDetailsBySearchParam
                (_DestroyID, _UserID, _OwnerID, _DocCategoryID, _DocTypeID, _DocPropertyID,
                    _DocPropIdentityID, out _errorNumber);

            if (_errorNumber.Length > 0)
            {
                return new ValidationResult(_errorNumber, _localizationService.GetResource(_errorNumber));
            }

            return ValidationResult.Success;
        }

        public ValidationResult SetDocDestroy(DSM_DestroyPolicy model, string action,
            out string status)
        {
            _docDestroyDataService.SetDocDestroy(model, action, out status);
            if (status.Length > 0)
            {
                return new ValidationResult(status, _localizationService.GetResource(status));
            }
            return ValidationResult.Success;
        }
    }
}
