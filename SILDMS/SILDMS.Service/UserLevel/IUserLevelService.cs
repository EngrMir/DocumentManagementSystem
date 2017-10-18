using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SILDMS.Model.SecurityModule;
using SILDMS.Utillity;

namespace SILDMS.Service.UserLevel
{
    public interface IUserLevelService
    {
        ValidationResult GetUserLevels(int? userLevel, string action, string levelType, out List<SEC_UserLevel> userLevelList);
        ValidationResult ManipulateUserLevel(SEC_UserLevel userLevel, string action, out string status);
    }
}
