using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SILDMS.Model.DocScanningModule;

namespace SILDMS.DataAccessInterface.DocumentType
{
    public interface IDocTypeDataService
    {
        List<DSM_DocType> GetDocType(string id, string action, out string errorNumber);
        string ManipulateDocType(DSM_DocType docType, string action, out string errorNumber);
    }
}
