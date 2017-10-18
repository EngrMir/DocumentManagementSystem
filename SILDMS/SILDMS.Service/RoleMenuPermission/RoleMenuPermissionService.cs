using System.Collections.Generic;
using SILDMS.DataAccessInterface.RoleMenuPermission;
using SILDMS.Model.SecurityModule;
using SILDMS.Utillity;
using SILDMS.Utillity.Localization;

namespace SILDMS.Service.RoleMenuPermission
{
    public class RoleMenuPermissionService: IRoleMenuPermissionService
    {
        private readonly IRoleMenuPermissionDataService roleMenuPermissionDataService;
        private readonly ILocalizationService localizationService;
        private string errorNumber = "";

        public RoleMenuPermissionService(ILocalizationService localizationService, 
            IRoleMenuPermissionDataService roleMenuPermissionDataService)
        {
            this.localizationService = localizationService;
            this.roleMenuPermissionDataService = roleMenuPermissionDataService;
        }


        public ValidationResult GetRoleMenuPermission(string _UserID, string _RoleID,
            out List<SEC_RoleMenuPermission> docPropIdentifyList)
        {
            docPropIdentifyList = roleMenuPermissionDataService.GetRoleMenuPermission
                (_UserID, _RoleID, out errorNumber);

            if (errorNumber.Length > 0)
            {
                return new ValidationResult(errorNumber, localizationService.GetResource(errorNumber));
            }

            return ValidationResult.Success;
        }


        public object GetOwnerPermittedMenu(string _UserID, string ownerID, 
            out List<SEC_NavMenuOptSetup> setupService)
        {
            setupService = roleMenuPermissionDataService.GetOwnerPermittedMenu(_UserID, ownerID,
                out errorNumber);

            if (errorNumber.Length > 0)
            {
                return new ValidationResult(errorNumber, localizationService.GetResource(errorNumber));
            }
            return setupService;
        }

        public ValidationResult SetRoleMenuPermission(SEC_RoleMenuPermission model, out string status)
        {
            roleMenuPermissionDataService.SetRoleMenuPermission(model, out status);
            if (status.Length > 0)
            {
                return new ValidationResult(status, localizationService.GetResource(status));
            }
            return ValidationResult.Success;
        }
    }
}
