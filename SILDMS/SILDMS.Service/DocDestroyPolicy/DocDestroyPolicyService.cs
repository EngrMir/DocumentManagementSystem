
using System.Collections.Generic;
using SILDMS.DataAccessInterface;
using SILDMS.Model.DocScanningModule;
using SILDMS.Utillity;
using SILDMS.Utillity.Localization;

namespace SILDMS.Service.DocDestroyPolicy
{
    public class DocDestroyPolicyService : IDocDestroyPolicyService
    {
        private readonly IDocDestroyPolicyDataService _docDestroyPolicyDataService;
        private readonly ILocalizationService _localizationService;
        private string _errorNumber = string.Empty;


        public DocDestroyPolicyService(IDocDestroyPolicyDataService docDestroyPolicyDataService,
            ILocalizationService localizationService)
        {
            _docDestroyPolicyDataService = docDestroyPolicyDataService;
            _localizationService = localizationService;
        }



        public ValidationResult GetDestroyPolicyBySearchParam(string _DestroyPolicyID, string _UserID, string _OwnerID, string _DocCategoryID, string _DocTypeID,
            string _DocPropertyID, string _DocPropIdentityID, out List<DSM_DestroyPolicy> destroyPolicies)
        {
            destroyPolicies = _docDestroyPolicyDataService.GetDestroyPolicyBySearchParam
                (_DestroyPolicyID, _UserID, _OwnerID, _DocCategoryID, _DocTypeID, _DocPropertyID,
                    _DocPropIdentityID, out _errorNumber);

            if (_errorNumber.Length > 0)
            {
                return new ValidationResult(_errorNumber, _localizationService.GetResource(_errorNumber));
            }

            return ValidationResult.Success;
        }

        public ValidationResult SetDocDestroyPolicy(DSM_DestroyPolicy model, string action,
            out string status)
        {
            _docDestroyPolicyDataService.SetDocDestroyPolicy(model, action, out status);
            if (status.Length > 0)
            {
                return new ValidationResult(status, _localizationService.GetResource(status));
            }
            return ValidationResult.Success;
        }
    }
}
