using System.Collections.Generic;
using SILDMS.Model.SecurityModule;

namespace SILDMS.DataAccessInterface.RoleSetup
{
    public interface IRoleSetupDataService
    {
        List<SEC_Role> GetRole(string _UserID, string _roleID, out string _errorNumber);

        string AddRole(SEC_Role modelRole, string action, out string errorNumber);
    }
}
