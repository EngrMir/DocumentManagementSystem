using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SILDMS.DataAccessInterface.NavigationMenuOperation;
using SILDMS.Model.SecurityModule;
using SILDMS.Utillity;
using SILDMS.Utillity.Localization;

namespace SILDMS.Service.NavigationMenuOperation
{
    public class NavMenuOprtService : INavMenuOprtService
    {

        #region Fields

        private readonly INavMenuOprtDataService _menuOprtDataService;
        private readonly ILocalizationService _localizationService;
        private string _errorNumber = string.Empty;

        #endregion

        #region Constructor

        public NavMenuOprtService(INavMenuOprtDataService menuOprtDataService, ILocalizationService localizationService)
        {
            _menuOprtDataService = menuOprtDataService;
            _localizationService = localizationService;
        }

        #endregion

        #region Functions
        public ValidationResult GetMenuOperation(string id, string action, out List<SEC_MenuOperation> menuOperationList)
        {
            menuOperationList = _menuOprtDataService.GetMenuOperation(id, action, out _errorNumber);
            return _errorNumber.Length > 0
                ? new ValidationResult(_errorNumber, _localizationService.GetResource(_errorNumber))
                : ValidationResult.Success;
        }

        public ValidationResult ManipulateMenuOperation(SEC_MenuOperation menuOperation, string action, out string status)
        {
            _menuOprtDataService.ManipulateMenuOperation(menuOperation, action, out status);
            return status.Length > 0
                ? new ValidationResult(status, _localizationService.GetResource(status))
                : ValidationResult.Success;
        }
        #endregion
    }
}
