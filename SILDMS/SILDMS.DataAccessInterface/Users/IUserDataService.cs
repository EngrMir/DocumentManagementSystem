using SILDMS.Model.SecurityModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using User = SILDMS.Model.Apps_User;

namespace SILDMS.DataAccessInterface.Users
{
    public interface IUserDataService
    {
        List<SEC_User> GetAllUser(string id, string ownerID, out string errorNumber);
        string AddUser(SEC_User objUser, string action, out string status);
        bool IsValidUser(string user, string password, string ip, out List<GetUserAccessPermission_Result> accessList);
    }
}
