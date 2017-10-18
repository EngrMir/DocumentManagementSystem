using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SILDMS.DataAccessInterface.UserLevel;
using SILDMS.Model.SecurityModule;
using SILDMS.Utillity;
using SILDMS.Utillity.Localization;

namespace SILDMS.Service.UserLevel
{
    public class UserLevelService : IUserLevelService
    {

        #region Fields

        private readonly IUserLevelDataService _userLevelDataService;
        private readonly ILocalizationService _localizationService;
        private string _errorNumber = string.Empty;

       
        #endregion

        #region Constructor

        public UserLevelService(IUserLevelDataService userLevelDataService, ILocalizationService localizationService)
        {
            _userLevelDataService = userLevelDataService;
            _localizationService = localizationService;
        }

        #endregion

        #region Functions

        public ValidationResult GetUserLevels(int? userLevel, string action, string levelType, out List<SEC_UserLevel> userLevelList)
        {
            userLevelList = _userLevelDataService.GetUserLevel(userLevel, action, levelType, out _errorNumber);
            return _errorNumber.Length > 0
                ? new ValidationResult(_errorNumber, _localizationService.GetResource(_errorNumber))
                : ValidationResult.Success;
        }

        public ValidationResult ManipulateUserLevel(SEC_UserLevel userLevel, string action, out string status)
        {
            _userLevelDataService.ManipulateUserLevel(userLevel, action, out status);
            return status.Length > 0
                ? new ValidationResult(status, _localizationService.GetResource(status))
                : ValidationResult.Success;
        }

        #endregion

    }
}
