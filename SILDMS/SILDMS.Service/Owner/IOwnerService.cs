using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SILDMS.Model.DocScanningModule;
using SILDMS.Utillity;

namespace SILDMS.Service.Owner
{
    public interface IOwnerService
    {
        ValidationResult GetAllOwners(string id, string action, out List<DSM_Owner> ownersList);
        ValidationResult ManipulateOwner(DSM_Owner owner, string action, out string status);
        
    }
}
