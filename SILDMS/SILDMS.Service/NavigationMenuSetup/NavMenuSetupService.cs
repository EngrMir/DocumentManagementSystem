using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SILDMS.DataAccessInterface.NavigationMenuSetup;
using SILDMS.Model.SecurityModule;
using SILDMS.Utillity;
using SILDMS.Utillity.Localization;

namespace SILDMS.Service.NavigationMenuSetup
{
    public class NavMenuSetupService : INavMenuSetupService
    {
        #region Fields

        private readonly INavMenuSetupDataService _menuSetupDataService;
        private readonly ILocalizationService _localizationService;
        private string _errorNumber = string.Empty;

        #endregion

        #region Constructor

        public NavMenuSetupService(INavMenuSetupDataService menuSetupDataService, ILocalizationService localizationService)
        {
            _menuSetupDataService = menuSetupDataService;
            _localizationService = localizationService;
        }

        #endregion

        #region Functions

        public ValidationResult GetMenus(string id, string action, out List<SEC_Menu> menuList)
        {
            menuList = _menuSetupDataService.GetMenus(id, action, out _errorNumber);
            return _errorNumber.Length > 0
                ? new ValidationResult(_errorNumber, _localizationService.GetResource(_errorNumber))
                : ValidationResult.Success;
        }

        public ValidationResult ManipulateMenu(SEC_Menu menu, string action, out string status)
        {
            _menuSetupDataService.ManipulateMenu(menu, action, out status);
            return status.Length > 0
                ? new ValidationResult(status, _localizationService.GetResource(status))
                : ValidationResult.Success;
        }

        #endregion
    }
}
