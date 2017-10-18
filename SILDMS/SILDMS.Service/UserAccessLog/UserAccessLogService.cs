using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SILDMS.DataAccessInterface.UserAccessLog;
using SILDMS.Model.SecurityModule;
using SILDMS.Utillity;
using SILDMS.Utillity.Localization;

namespace SILDMS.Service.UserAccessLog
{
    public class UserAccessLogService : IUserAccessLogService
    {
        #region Fields

        private readonly IUserAccessLogDataService _accessLogDataService;
        private readonly ILocalizationService _localizationService;
        private string _errorNumber = string.Empty;

        #endregion

        #region Constructor

        public UserAccessLogService(IUserAccessLogDataService accessLogDataService, ILocalizationService localizationService)
        {
            _accessLogDataService = accessLogDataService;
            _localizationService = localizationService;
        }

        #endregion

        #region Functions

        public ValidationResult GetUserAccessLog(string userID, DateTime? dateFrom, DateTime? dateTo, out List<SEC_UserLog> userLogs)
        {
            userLogs = _accessLogDataService.GetUserLog(userID, dateFrom, dateTo, out _errorNumber);
            return _errorNumber.Length > 0
                ? new ValidationResult(_errorNumber, _localizationService.GetResource(_errorNumber))
                : ValidationResult.Success;
        }

        public string ManipulateUserAccessLog(SEC_UserLog userLogin)
        {
          return  _accessLogDataService.ManipulateUserAccessLog(userLogin);
          
        }

        #endregion
    }
}
