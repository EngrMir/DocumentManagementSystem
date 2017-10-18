using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SILDMS.Model.DocScanningModule
{
    public partial class DSM_Owner
    {
        public DSM_Owner()
        {
            
        }

        public string OwnerID { get; set; }
        public string UDOwnerCode { get; set; }
        public string OwnerLevelID { get; set; }
        public string OwnerName { get; set; }
        public string OwnerShortName { get; set; }
        public string ParentOwner { get; set; }
        public string SetOn { get; set; }
        public string SetBy { get; set; }
        public string ModifiedOn { get; set; }
        public string ModifiedBy { get; set; }
        public int Status { get; set; }
        public int UserLevel { get; set; }

        #region For view

        public string LevelName { get; set; }
        public string ParentName { get; set; }

        #endregion
    }
}
