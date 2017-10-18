using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SILDMS.Model.DocScanningModule
{
    public partial class DSM_DocType
    {
        #region Constructor
        public DSM_DocType()
        {
            
        }
        #endregion

        #region Fields

        public string DocTypeID { get; set; }
        public string OwnerID { get; set; }
        public string DocCategoryID { get; set; }
        public string DocTypeSL { get; set; }
        public string UDDocTypeCode { get; set; }
        public string DocTypeName { get; set; }
        public string DocPreservationPolicy { get; set; }
        public string DocPhysicalLocation { get; set; }
        public string SetOn { get; set; }
        public string SetBy { get; set; }
        public string ModifiedOn { get; set; }
        public string ModifiedBy { get; set; }
        public int Status { get; set; }
        public int? DocClassification { get; set; }
        public int? ClassificationLevel { get; set; }

        #endregion
    }
}
