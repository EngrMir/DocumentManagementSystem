using System.Collections.Generic;
using SILDMS.DataAccessInterface.MultiDocScan;
using SILDMS.Model.DocScanningModule;
using SILDMS.Utillity;
using SILDMS.Utillity.Localization;

namespace SILDMS.Service.MultiDocScan
{
    public class MultiDocScanService: IMultiDocScanService
    {
        private readonly IMultiDocScanDataService _multiDocScanDataService;
        private readonly ILocalizationService _localizationService;
        private string _errorNumber = string.Empty;
       

        public MultiDocScanService(IMultiDocScanDataService multiDocScanDataService,
            ILocalizationService localizationService)
        {
            _multiDocScanDataService = multiDocScanDataService;
            _localizationService = localizationService;
        }
        public ValidationResult AddDocumentInfo(DocumentsInfo _modelDocumentsInfo,
            string _selectedPropID, List<DocMetaValue> _docMetaValues, string action,
            out List<DSM_DocPropIdentify> docPropIdentifyList)
        {
            docPropIdentifyList = _multiDocScanDataService.AddDocumentInfo
                (_modelDocumentsInfo, _selectedPropID, _docMetaValues, action, out _errorNumber);

            if (_errorNumber.Length > 0)
            {
                return new ValidationResult(_errorNumber, 
                    _localizationService.GetResource(_errorNumber));
            }

            return ValidationResult.Success;
        }

        public ValidationResult GetAllDocumentsInfo(string _UserID, string _DocumentID, 
            out List<DSM_Documents> dsmDocuments)
        {
            dsmDocuments = _multiDocScanDataService.
                GetAllDocumentsInfo(_UserID, _DocumentID, out _errorNumber);

            if (_errorNumber.Length > 0)
            {
                return new ValidationResult(_errorNumber,
                    _localizationService.GetResource(_errorNumber));
            }

            return ValidationResult.Success;
        }

        public ValidationResult GetAllDocumentsMetaInfo(string _UserID, string _DocMetaID, 
            out List<DSM_DocumentsMeta> dsmDocumentsMetas)
        {
            dsmDocumentsMetas = _multiDocScanDataService.
                GetAllDocumentsMetaInfo(_UserID, _DocMetaID, out _errorNumber);

            if (_errorNumber.Length > 0)
            {
                return new ValidationResult(_errorNumber, 
                    _localizationService.GetResource(_errorNumber));
            }

            return ValidationResult.Success;
        }

        public ValidationResult DeleteDocumentInfo(string _DocumentIDs)
        {
            _multiDocScanDataService.DeleteDocumentInfo(_DocumentIDs, out _errorNumber);

            if (_errorNumber == "S201")
            {
                return ValidationResult.Success;
            }
            else
            {
                return ValidationResult.Success;
            }

        }
    }
}
