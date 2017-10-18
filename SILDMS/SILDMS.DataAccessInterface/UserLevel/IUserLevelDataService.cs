using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SILDMS.Model.SecurityModule;

namespace SILDMS.DataAccessInterface.UserLevel
{
    public interface IUserLevelDataService
    {
        List<SEC_UserLevel> GetUserLevel(int? userLevel, string action, string levelType, out string errorNumber);
        string ManipulateUserLevel(SEC_UserLevel userLevel, string action, out string errorNumber);
    }
}
