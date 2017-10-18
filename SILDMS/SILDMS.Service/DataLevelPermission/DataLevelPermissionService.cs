using System.Collections.Generic;
using SILDMS.DataAccessInterface.DataLevelPermission;
using SILDMS.Model.SecurityModule;
using SILDMS.Utillity;
using SILDMS.Utillity.Localization;

namespace SILDMS.Service.DataLevelPermission
{
    public class DataLevelPermissionService: IDataLevelPermissionService
    {
        private readonly IDataLevelPermissionDataService _dataLevelPermissionDataService;
        private readonly ILocalizationService _localizationService;
        private string _errorNumber = string.Empty;


        public DataLevelPermissionService(IDataLevelPermissionDataService ownerLevelPermissionDataService,
            ILocalizationService localizationService)
        {
            _dataLevelPermissionDataService = ownerLevelPermissionDataService;
            _localizationService = localizationService;
        }
        public ValidationResult GetAllUserDataAccess(string _UserID,
            out List<SEC_UserDataAccess> secUserOwnerAccesses)
        {
            secUserOwnerAccesses = _dataLevelPermissionDataService.GetAllUserDataAccess
                (_UserID, out _errorNumber);

            if (_errorNumber.Length > 0)
            {
                return new ValidationResult(_errorNumber, _localizationService.GetResource(_errorNumber));
            }

            return ValidationResult.Success;
        }

        public ValidationResult SetDataLevelPermission(SEC_UserOwnerAccess model, string action,
            out string status)
        {
            _dataLevelPermissionDataService.SetDataLevelPermission(model, action, out status);
            if (status.Length > 0)
            {
                return new ValidationResult(status, _localizationService.GetResource(status));
            }
            return ValidationResult.Success;
        }
    }
}
