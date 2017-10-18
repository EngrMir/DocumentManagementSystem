using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SILDMS.Model.DocScanningModule;
using SILDMS.Utillity;

namespace SILDMS.Service.DocumentCategory
{
    public interface IDocCategoryService
    {
        ValidationResult GetDocCategories(string id, string action, out List<DSM_DocCategory> docCategoriesList);
        ValidationResult ManipulateDocCategory(DSM_DocCategory docCategory, string action, out string status);
    }
}
