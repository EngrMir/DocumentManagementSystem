using System.Collections.Generic;
using SILDMS.Model.SecurityModule;

namespace SILDMS.DataAccessInterface.OwnerLevelPermission
{
    public interface IOwnerLevelPermissionDataService
    {
        List<SEC_UserOwnerAccess> GetAllUserOwnerAccess(string _UserID,  out string _errorNumber);

        List<UserWisePermittedOwner> GetUserWisePermittedOwnerList(string _UserID, string _SessionUserID, string _OwnerLevelID, out string _errorNumber);

        string SetOwnerLevelPermission(SEC_UserOwnerAccess model, out string errorNumber);
    }
    
    
}
