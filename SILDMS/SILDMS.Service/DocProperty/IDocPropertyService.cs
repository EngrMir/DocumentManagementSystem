using SILDMS.Model.DocScanningModule;
using SILDMS.Utillity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SILDMS.Service.DocProperty
{
    public interface  IDocPropertyService 
    {
        ValidationResult GetDocProperty(string DocPropertyId, string action, out List<DSM_DocProperty> ownerLevelList);
        ValidationResult AddDocProperty(DSM_DocProperty ownerLevel, string action, out string status);
    }
}
