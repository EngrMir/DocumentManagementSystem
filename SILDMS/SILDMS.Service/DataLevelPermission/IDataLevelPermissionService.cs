using System.Collections.Generic;
using SILDMS.Model.SecurityModule;
using SILDMS.Utillity;

namespace SILDMS.Service.DataLevelPermission
{
    public interface IDataLevelPermissionService
    {
        ValidationResult GetAllUserDataAccess(string _UserID,
            out List<SEC_UserDataAccess> secUserDataAccesses);

        ValidationResult SetDataLevelPermission(SEC_UserOwnerAccess model, string action,
            out string status);

    }
}
