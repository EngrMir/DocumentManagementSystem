using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SILDMS.Model.DocScanningModule;
using SILDMS.Utillity;

namespace SILDMS.Service.DocDestroy
{
    public interface IDocDestroyService
    {
        ValidationResult GetDestroyDetailsBySearchParam(string _DestroyID, string _UserID, string _OwnerID,
            string _DocCategoryID, string _DocTypeID, string _DocPropertyID,
            string _DocPropIdentityID, out List<DSM_DestroyPolicy> destroyPolicies);

        ValidationResult SetDocDestroy(DSM_DestroyPolicy model, string action,
            out string status);
    }
}
