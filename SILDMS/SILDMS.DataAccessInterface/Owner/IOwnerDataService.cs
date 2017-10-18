using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SILDMS.Model.DocScanningModule;

namespace SILDMS.DataAccessInterface.Owner
{
    public interface IOwnerDataService
    {
        List<DSM_Owner> GetOwner(string id, string action, out string errorNumber);
        string ManipulateOwner(DSM_Owner owner, string action, out string errorNumber);
        
    }
}
