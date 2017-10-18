using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SILDMS.Model.DocScanningModule;

namespace SILDMS.Model.SecurityModule
{
    public class SEC_RoleMenuPermission
    {
        public string RoleMenuPermissionID { get; set; }
        public string RoleID { get; set; }
        
        public string MenuID { get; set; }
        public string MenuTitle { get; set; }
        public string RoleParentMenuID { get; set; }
        public string OwnerID { get; set; }
        public string UserLevel { get; set; }
        public string SetOn { get; set; }
        public string SetBy { get; set; }
        public string ModifiedOn { get; set; }
        public string ModifiedBy { get; set; }
        public int Status { get; set; }

        public ddlDSMOwner Owner { get; set; }
        public ddlRole Role { get; set; }
    }

    
}
