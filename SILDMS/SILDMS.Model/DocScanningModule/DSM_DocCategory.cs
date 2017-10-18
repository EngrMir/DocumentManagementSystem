using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SILDMS.Model.DocScanningModule
{
    public partial class DSM_DocCategory
    {
        public DSM_DocCategory()
        {
            
        }

        public string DocCategoryID { get; set; }
        public string OwnerID { get; set; }
        public string DocCategorySL { get; set; }
        public string UDDocCategoryCode { get; set; }
        public string DocCategoryName { get; set; }
        public string SetOn { get; set; }
        public string SetBy { get; set; }
        public string ModifiedOn { get; set; }
        public string ModifiedBy { get; set; }
        public int Status { get; set; }
        public int UserLevel { get; set; }

        #region Fields for View

        public string OwnerName { get; set; }

        #endregion

    }
}
