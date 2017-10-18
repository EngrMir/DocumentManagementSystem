using SILDMS.Model.DocScanningModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SILDMS.DataAccessInterface.OwnerProperty
{
    public interface IDocPropertyDataService
    {
        List<DSM_DocProperty> GetDocProperty(string DocPropertyId, string action, out string errorNumber);
        string AddDocProperty(DSM_DocProperty ownerLevel, string action, out string errorNumber);
    }
}
