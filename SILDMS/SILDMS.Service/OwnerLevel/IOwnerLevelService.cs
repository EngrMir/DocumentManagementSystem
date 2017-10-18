using SILDMS.Model.DocScanningModule;
using SILDMS.Utillity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SILDMS.Service.OwnerLevel
{
    public interface IOwnerLevelService
    {
        ValidationResult GetOwnerLevel(string OwnerLevelId, string action, out List<DSM_OwnerLevel> ownerLevelList);
        ValidationResult AddOwnerLevel(DSM_OwnerLevel ownerLevel, string action, out string status);
       
    }
}
