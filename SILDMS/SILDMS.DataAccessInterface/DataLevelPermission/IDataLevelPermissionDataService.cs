using System.Collections.Generic;
using SILDMS.Model.SecurityModule;

namespace SILDMS.DataAccessInterface.DataLevelPermission
{

    public interface IDataLevelPermissionDataService
    {
        List<SEC_UserDataAccess> GetAllUserDataAccess(string _UserID, out string _errorNumber);
        string SetDataLevelPermission(SEC_UserOwnerAccess model, string action, out string errorNumber);
    }
}
