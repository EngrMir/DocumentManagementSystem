using System.Collections.Generic;
using SILDMS.Model.SecurityModule;

namespace SILDMS.DataAccessInterface.RoleMenuPermission
{
    public interface IRoleMenuPermissionDataService
    {
        List<SEC_RoleMenuPermission> GetRoleMenuPermission(string _UserID, string _RoleID,
            out string errorNumber);
        List<SEC_NavMenuOptSetup> GetOwnerPermittedMenu(string _UserID, string ownerID,
            out string errorNumber);

        string SetRoleMenuPermission(SEC_RoleMenuPermission model, out string errorNumber);
    }
}
