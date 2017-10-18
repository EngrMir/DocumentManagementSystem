using SILDMS.Model.DocScanningModule;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SILDMS.Model.SecurityModule
{
    public class SEC_MenuDetails
    {
        public string MenuDetailID { get; set; }
        [Required]
        public string MenuID { get; set; }
        public string VisibleInRoleMenu { get; set; }
        
        public string OwnerID { get; set; }
        public string UserLevel { get; set; }
        public string SetOn { get; set; }
        public string SetBy { get; set; }
        public string ModifiedOn { get; set; }
        public string ModifiedBy { get; set; }
        public int Status { get; set; }
        [Required]
        public virtual ddlDSMOwner Owner { get; set; }
    }
}
