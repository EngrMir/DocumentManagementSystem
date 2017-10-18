using System.Collections.Generic;
using SILDMS.Model.DocScanningModule;

namespace SILDMS.DataAccessInterface
{
    public interface IDocDestroyPolicyDataService
    {
        List<DSM_DestroyPolicy> GetDestroyPolicyBySearchParam(string _DestroyPolicyID, string _UserID, string _OwnerID,
            string _DocCategoryID, string _DocTypeID, string _DocPropertyID, 
            string _DocPropIdentityID, out string _errorNumber);

        string SetDocDestroyPolicy(DSM_DestroyPolicy model, string action, out string errorNumber);
    }
}
