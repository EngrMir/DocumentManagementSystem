using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SILDMS.Model.DocScanningModule;
using SILDMS.Utillity;

namespace SILDMS.Service.DocumentType
{
    public interface IDocTypeService
    {
        ValidationResult GetDocTypes(string id, string action, out List<DSM_DocType> docTypeList);
        ValidationResult ManipulateDocType(DSM_DocType docType, string action, out string status);
    }
}
