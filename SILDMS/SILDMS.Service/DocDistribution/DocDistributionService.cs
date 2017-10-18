using SILDMS.Model.DocScanningModule;
using SILDMS.Utillity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SILDMS.Utillity.Localization;
using SILDMS.DataAccessInterface.DocDistribution;

namespace SILDMS.Service.DocDistribution
{
    public class DocDistributionService : IDocDistributionService
    {
        private string _errorNumber = "";
        public DocDistributionService(IDocDistributionDataService docDistDataService,ILocalizationService localizationService)
        {
            _docDistDataService = docDistDataService;
            _localizationService = localizationService;
        }


        public IDocDistributionDataService _docDistDataService { get; set; }

        public ILocalizationService _localizationService { get; set; }



        public ValidationResult AddDocumentInfoForVersion(DocumentsInfo modelDocumentsInfo, string selectedPropId, List<DocMetaValue> lstMetaValues, string action, out string outStatus)
        {
            _docDistDataService.AddDocumentInfoForVersion(modelDocumentsInfo, selectedPropId, lstMetaValues, action, out  outStatus);
            if (outStatus.Length > 0)
            {
                return new ValidationResult(outStatus, _localizationService.GetResource(outStatus));
            }
            return ValidationResult.Success;
        }

        public ValidationResult AddDocumentInfo(DocumentsInfo modelDocumentsInfo, string selectedPropId, List<DocMetaValue> lstMetaValues, string action, out string outStatus)
        {
            _docDistDataService.AddDocumentInfo(modelDocumentsInfo, selectedPropId, lstMetaValues, action, out outStatus);
            if (outStatus.Length > 0)
            {
                return new ValidationResult(outStatus, _localizationService.GetResource(outStatus));
            }
            return ValidationResult.Success;
        }
    }
}
