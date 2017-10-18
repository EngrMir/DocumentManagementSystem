using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SILDMS.DataAccessInterface.Owner;
using SILDMS.Model.DocScanningModule;
using SILDMS.Utillity;
using SILDMS.Utillity.Localization;

namespace SILDMS.Service.Owner
{
    public class OwnerService : IOwnerService
    {
        #region Fields

        private readonly IOwnerDataService _ownerDataService;
        private readonly ILocalizationService _localizationService;
        private string _errorNumber = string.Empty;

        #endregion

        #region Constructor

        public OwnerService(IOwnerDataService ownerDataService, ILocalizationService localizationService)
        {
            _ownerDataService = ownerDataService;
            _localizationService = localizationService;
        }

        #endregion

        public ValidationResult GetAllOwners(string id, string action, out List<Model.DocScanningModule.DSM_Owner> ownersList)
        {
            ownersList = _ownerDataService.GetOwner(id, action, out _errorNumber);
            if (_errorNumber.Length > 0)
            {
                return new ValidationResult(_errorNumber, _localizationService.GetResource(_errorNumber));
            }
            return ValidationResult.Success;
        }

        public ValidationResult ManipulateOwner(DSM_Owner owner, string action, out string status)
        {
            _ownerDataService.ManipulateOwner(owner, action, out status);
            if (status.Length > 0)
            {
                return new ValidationResult(status, _localizationService.GetResource(status));
            }
            return ValidationResult.Success;
        }

   
    }
}
