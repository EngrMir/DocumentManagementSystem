using System.Collections.Generic;
using SILDMS.Model.DocScanningModule;
using SILDMS.Utillity;

namespace SILDMS.Service.OwnerProperIdentity
{
    public interface IOwnerProperIdentityService
    {
        ValidationResult GetDocPropIdentify(string _UserID, string docPropIdentifyID,
            out List<DSM_DocPropIdentify> docPropIdentifyList);

        ValidationResult AddOwnerPropIdentity(DSM_DocPropIdentify modelDsmDocPropIdentify,
            string action, out string status);

        ValidationResult EditOwnerPropIdentity(DSM_DocPropIdentify modelDsmDocPropIdentify,
            string action, out string status);
    }
}
