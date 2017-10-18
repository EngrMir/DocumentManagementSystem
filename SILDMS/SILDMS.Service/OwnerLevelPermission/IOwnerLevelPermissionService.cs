using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SILDMS.Model.SecurityModule;
using SILDMS.Utillity;

namespace SILDMS.Service.OwnerLevelPermission
{
    public interface IOwnerLevelPermissionService
    {
        ValidationResult GetAllUserOwnerAccess(string _UserID,
            out List<SEC_UserOwnerAccess> secUserOwnerAccesses);

        ValidationResult GetUserWisePermittedOwnerList(string _UserID, string _SessionUserID, string _OwnerLevelID,
            out List<UserWisePermittedOwner> secUserOwnerAccesses);

        ValidationResult SetOwnerLevelPermission(SEC_UserOwnerAccess model, out string status);
    }
}
