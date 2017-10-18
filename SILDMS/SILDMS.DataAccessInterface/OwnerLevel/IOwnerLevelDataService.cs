using SILDMS.Model.DocScanningModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SILDMS.DataAccessInterface.OwnerLevel
{
    public interface IOwnerLevelDataService
    {
        List<DSM_OwnerLevel> GetOwnerLevel(string OwnerLevelId, string action, out string errorNumber);
        string AddOwnerLevel(DSM_OwnerLevel ownerLevel, string action, out string errorNumber);
      

    }
}
