using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SILDMS.Model.SecurityModule;

namespace SILDMS.DataAccessInterface.UserAccessLog
{
    public interface IUserAccessLogDataService
    {
        List<SEC_UserLog> GetUserLog(string userID, DateTime? dateFrom, DateTime? dateTo, out string errorNumber);
        string ManipulateUserAccessLog(SEC_UserLog userLogin);
    }
}
