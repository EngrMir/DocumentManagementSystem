using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SILDMS.Model.DocScanningModule;

namespace SILDMS.DataAccessInterface.DocumentCategory
{
    public interface IDocCategoryDataService
    {
        List<DSM_DocCategory> GetDocCategory(string id, string action, out string errorNumber);
        string ManipulateDocCategory(DSM_DocCategory category, string action, out string errorNumber);
    }
}
