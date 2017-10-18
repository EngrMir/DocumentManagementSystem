using SILDMS.Model.DocScanningModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SILDMS.DataAccessInterface.DocDistribution
{
    public interface IDocDistributionDataService
    {
        string AddDocumentInfo(DocumentsInfo _modelDocumentsInfo,string _selectedPropID, List<DocMetaValue> _docMetaValues,string _action, out string _errorNumber);

        string AddDocumentInfoForVersion(DocumentsInfo modelDocumentsInfo, string selectedPropId, List<DocMetaValue> lstMetaValues, string action, out string _errorNumber);
    }
}
