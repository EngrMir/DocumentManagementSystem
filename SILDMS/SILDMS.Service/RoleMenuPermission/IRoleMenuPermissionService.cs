using System.Collections.Generic;
using SILDMS.Model.SecurityModule;
using SILDMS.Utillity;

namespace SILDMS.Service.RoleMenuPermission
{
    public interface IRoleMenuPermissionService
    {
        object GetOwnerPermittedMenu(string _UserID, string ownerID, 
            out List<SEC_NavMenuOptSetup> setupService);

        ValidationResult GetRoleMenuPermission(string _UserID, string _RoleID, 
            out List<SEC_RoleMenuPermission> docPropIdentifyList);

        ValidationResult SetRoleMenuPermission(SEC_RoleMenuPermission model, out string status);
    }
}
