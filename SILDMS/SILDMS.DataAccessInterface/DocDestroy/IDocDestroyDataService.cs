using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SILDMS.Model.DocScanningModule;

namespace SILDMS.DataAccessInterface.DocDestroy
{
    public interface IDocDestroyDataService
    {
        List<DSM_DestroyPolicy> GetDestroyDetailsBySearchParam(string _DestroyID, 
           string _UserID, string _OwnerID, string _DocCategoryID, string _DocTypeID,
           string _DocPropertyID, string _DocPropIdentityID, out string _errorNumber);

        string SetDocDestroy(DSM_DestroyPolicy model, string action, out string errorNumber);
    }
}
