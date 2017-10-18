using SILDMS.DataAccessInterface.OwnerProperty;
using SILDMS.Model.DocScanningModule;
using SILDMS.Utillity;
using SILDMS.Utillity.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SILDMS.Service.DocProperty
{
    public class DocPropertyService :IDocPropertyService
    {
         #region Fields

        private readonly IDocPropertyDataService docPropertyDataService;
        private readonly ILocalizationService localizationService;
        private string errorNumber = "";
        #endregion

        #region Constractor
        public DocPropertyService(
             IDocPropertyDataService repository,
             ILocalizationService localizationService
            )
        {
            this.docPropertyDataService = repository;
            this.localizationService = localizationService;
        }

        #endregion
        public ValidationResult GetDocProperty(string DocPropertyId, string action, out List<DSM_DocProperty> docPropertyList)
        {
            docPropertyList = docPropertyDataService.GetDocProperty(DocPropertyId, action, out errorNumber);
            if (errorNumber.Length > 0)
            {
                return new ValidationResult(errorNumber, localizationService.GetResource(errorNumber));
            }
            return ValidationResult.Success;
        }

        public ValidationResult AddDocProperty(DSM_DocProperty docProperty, string action, out string status)
        {  
            docPropertyDataService.AddDocProperty(docProperty, action, out status);
            if (status.Length > 0)
            {
                return new ValidationResult(status, localizationService.GetResource(status));
            }
            return ValidationResult.Success;
        }
    }
}
