using System.Collections.Generic;
using SILDMS.Model.SecurityModule;
using SILDMS.Utillity;

namespace SILDMS.Service.RoleSetup
{
    public interface IRoleSetupService
    {
        ValidationResult GetRole(string _UserID, string RoleID, 
            out List<SEC_Role> rolesList);

        ValidationResult AddRole(SEC_Role modelRole, string action, out string status);

        ValidationResult EditRole(SEC_Role modelRole, string action, out string status);
    }


}
