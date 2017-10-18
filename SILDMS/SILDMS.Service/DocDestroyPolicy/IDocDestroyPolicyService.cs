

using System.Collections.Generic;
using SILDMS.Model.DocScanningModule;
using SILDMS.Utillity;

namespace SILDMS.Service.DocDestroyPolicy
{
    public interface IDocDestroyPolicyService
    {
        ValidationResult GetDestroyPolicyBySearchParam(string _DestroyPolicyID, string _UserID, string _OwnerID,
            string _DocCategoryID, string _DocTypeID, string _DocPropertyID, 
            string _DocPropIdentityID, out List<DSM_DestroyPolicy> destroyPolicies);

        ValidationResult SetDocDestroyPolicy(DSM_DestroyPolicy model, string action,
            out string status);
    }
}
