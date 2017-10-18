using SILDMS.DataAccessInterface.NavMenuOptSetup;
using SILDMS.Model.SecurityModule;
using SILDMS.Utillity;
using SILDMS.Utillity.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SILDMS.Service.NavMenuOptSetup
{
    public class NavMenuOptSetupService : INavMenuOptSetupService
    {
           #region Fields

        private readonly INavMenuOptSetupDataService navSetupService;
        private readonly ILocalizationService localizationService;
        private string errorNumber = "";
        #endregion

        #region Constractor
        public NavMenuOptSetupService(ILocalizationService localizationService, INavMenuOptSetupDataService navSetupService)
        {
            this.localizationService = localizationService;
            this.navSetupService = navSetupService;
        }

        #endregion

        public object GetNavMenuOptSetup(string ownerID,out List<SEC_NavMenuOptSetup> setupService)
        {
            setupService = navSetupService.GetNavMenuOptSetup(ownerID, out errorNumber);
           
                                 // Owners is a collection }).ToList();
            if (errorNumber.Length > 0)
            {
                return new ValidationResult(errorNumber, localizationService.GetResource(errorNumber));
            }
            return setupService;
        }



        public ValidationResult SetMenuOperations(SEC_MenuDetails menuDetails, string action, out string status)
        {
            navSetupService.SetMenuOperations(menuDetails, action, out status);
            if (status.Length > 0)
            {
                return new ValidationResult(status, localizationService.GetResource(status));
            }
            return ValidationResult.Success;
        }
    }
}
