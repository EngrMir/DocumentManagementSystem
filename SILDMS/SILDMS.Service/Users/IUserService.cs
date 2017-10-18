using SILDMS.Model;
using SILDMS.Model.SecurityModule;
using SILDMS.Utillity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using User = SILDMS.Model.Apps_User;

namespace SILDMS.Service.Users
{
    public partial interface IUserService
    {
        ValidationResult GetAllUser(string id,string ownerID,out List<SEC_User> userList);
        ValidationResult AddUser(SEC_User objUser, string action, out string outStatus);
        bool IsValidUser(string user, string password, string ip, out List<GetUserAccessPermission_Result> accessList);
    }
}
