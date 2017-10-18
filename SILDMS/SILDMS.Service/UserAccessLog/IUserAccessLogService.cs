using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SILDMS.Model.SecurityModule;
using SILDMS.Utillity;

namespace SILDMS.Service.UserAccessLog
{
    public interface IUserAccessLogService
    {
        ValidationResult GetUserAccessLog(string userID, DateTime? dateFrom, DateTime? dateTo, out List<SEC_UserLog> userLogs);
        string ManipulateUserAccessLog(SEC_UserLog userLogin);
    }
}
