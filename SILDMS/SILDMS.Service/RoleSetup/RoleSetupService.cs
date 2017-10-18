using System.Collections.Generic;
using SILDMS.DataAccessInterface.RoleSetup;
using SILDMS.Model.SecurityModule;
using SILDMS.Utillity;
using SILDMS.Utillity.Localization;

namespace SILDMS.Service.RoleSetup
{
    public class RoleSetupService: IRoleSetupService
    {
        private readonly IRoleSetupDataService _roleSetupDataService;
        private readonly ILocalizationService _localizationService;
        private string _errorNumber = string.Empty;


        public RoleSetupService(IRoleSetupDataService roleSetupDataService,
            ILocalizationService localizationService)
        {
            _roleSetupDataService = roleSetupDataService;
            _localizationService = localizationService;
        }


        public ValidationResult GetRole(string _UserID, string RoleID,
            out List<SEC_Role> rolesList)
        {
            rolesList = _roleSetupDataService.GetRole
                (_UserID, RoleID, out _errorNumber);

            if (_errorNumber.Length > 0)
            {
                return new ValidationResult(_errorNumber, _localizationService.GetResource(_errorNumber));
            }

            return ValidationResult.Success;
        }

        public ValidationResult AddRole(SEC_Role modeRole, string action, out string status)
        {
            _roleSetupDataService.AddRole(modeRole, action, out status);
            if (status.Length > 0)
            {
                return new ValidationResult(status, _localizationService.GetResource(status));
            }
            return ValidationResult.Success;
        }

        public ValidationResult EditRole(SEC_Role modeRole, string action, out string status)
        {
            _roleSetupDataService.AddRole(modeRole, action, out status);
            if (status.Length > 0)
            {
                return new ValidationResult(status, _localizationService.GetResource(status));
            }
            return ValidationResult.Success;
        }
    }
}
