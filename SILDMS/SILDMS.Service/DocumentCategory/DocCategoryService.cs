using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SILDMS.DataAccessInterface.DocumentCategory;
using SILDMS.Model.DocScanningModule;
using SILDMS.Utillity;
using SILDMS.Utillity.Localization;

namespace SILDMS.Service.DocumentCategory
{
    public class DocCategoryService : IDocCategoryService
    {

        #region Fields

        private readonly IDocCategoryDataService _categoryDataService;
        private readonly ILocalizationService _localizationService;
        private string _errorNumber = string.Empty;

        #endregion

        #region Constructor

        public DocCategoryService(IDocCategoryDataService categoryDataService, ILocalizationService localizationService)
        {
            _categoryDataService = categoryDataService;
            _localizationService = localizationService;
        }

        #endregion

        public ValidationResult GetDocCategories(string id, string action, out List<DSM_DocCategory> docCategoriesList)
        {
            docCategoriesList = _categoryDataService.GetDocCategory(id, action, out _errorNumber);
            return _errorNumber.Length > 0 ? new ValidationResult(_errorNumber, _localizationService.GetResource(_errorNumber)) 
                : ValidationResult.Success;
        }

        public ValidationResult ManipulateDocCategory(DSM_DocCategory docCategory, string action, out string status)
        {
            _categoryDataService.ManipulateDocCategory(docCategory, action, out status);
            return status.Length > 0 ? new ValidationResult(status, _localizationService.GetResource(status)) : ValidationResult.Success;
        }
    }
}
