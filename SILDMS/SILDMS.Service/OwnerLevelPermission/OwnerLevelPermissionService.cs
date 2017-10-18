using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SILDMS.DataAccessInterface.OwnerLevelPermission;
using SILDMS.DataAccessInterface.RoleSetup;
using SILDMS.Model.SecurityModule;
using SILDMS.Utillity;
using SILDMS.Utillity.Localization;

namespace SILDMS.Service.OwnerLevelPermission
{
    public class OwnerLevelPermissionService: IOwnerLevelPermissionService
    {
        private readonly IOwnerLevelPermissionDataService _ownerLevelPermissionDataService;
        private readonly ILocalizationService _localizationService;
        private string _errorNumber = string.Empty;


        public OwnerLevelPermissionService(IOwnerLevelPermissionDataService ownerLevelPermissionDataService,
            ILocalizationService localizationService)
        {
            _ownerLevelPermissionDataService = ownerLevelPermissionDataService;
            _localizationService = localizationService;
        }

        public ValidationResult GetAllUserOwnerAccess(string _UserID,
            out List<SEC_UserOwnerAccess> secUserOwnerAccesses)
        {
            secUserOwnerAccesses = _ownerLevelPermissionDataService.GetAllUserOwnerAccess
                (_UserID, out _errorNumber);

            if (_errorNumber.Length > 0)
            {
                return new ValidationResult(_errorNumber, _localizationService.GetResource(_errorNumber));
            }

            return ValidationResult.Success;
        }

        public ValidationResult GetUserWisePermittedOwnerList(string _UserID, string _SessionUserID, string _OwnerLevelID,
            out List<UserWisePermittedOwner> secUserOwnerAccesses)
        {
            secUserOwnerAccesses = _ownerLevelPermissionDataService.GetUserWisePermittedOwnerList
                (_UserID, _SessionUserID, _OwnerLevelID, out _errorNumber);

            if (_errorNumber.Length > 0)
            {
                return new ValidationResult(_errorNumber, _localizationService.GetResource(_errorNumber));
            }

            return ValidationResult.Success;
        }

        public ValidationResult SetOwnerLevelPermission(SEC_UserOwnerAccess model, out string status)
        {
            _ownerLevelPermissionDataService.SetOwnerLevelPermission(model, out status);
            if (status.Length > 0)
            {
                return new ValidationResult(status, _localizationService.GetResource(status));
            }
            return ValidationResult.Success;
        }
    }
}
