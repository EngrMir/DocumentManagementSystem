using SILDMS.DataAccessInterface.OwnerLevel;
using SILDMS.Model.DocScanningModule;
using SILDMS.Utillity;
using SILDMS.Utillity.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SILDMS.Service.OwnerLevel
{
    public class OwnerLevelService : IOwnerLevelService
    {
         #region Fields

        private readonly IOwnerLevelDataService ownerLevelDataService;
        private readonly ILocalizationService localizationService;
        private string errorNumber = "";
        #endregion

        #region Constractor
        public OwnerLevelService(
             IOwnerLevelDataService repository,
             ILocalizationService localizationService
            )
        {
            this.ownerLevelDataService = repository;
            this.localizationService = localizationService;
        }

        #endregion
        public ValidationResult GetOwnerLevel(string OwnerLevelId, string action, out List<DSM_OwnerLevel> ownerLevelList)
        {
            ownerLevelList = ownerLevelDataService.GetOwnerLevel(OwnerLevelId, action, out errorNumber);
            if (errorNumber.Length > 0)
            {
                return new ValidationResult(errorNumber, localizationService.GetResource(errorNumber));
            }
            return ValidationResult.Success;
        }

        public ValidationResult AddOwnerLevel(DSM_OwnerLevel ownerLevel, string action, out string status)
        {
            ownerLevelDataService.AddOwnerLevel(ownerLevel, action, out status);
            if (status.Length > 0)
            {
                return new ValidationResult(status, localizationService.GetResource(status));
            }
            return ValidationResult.Success;
        }

    }
}
