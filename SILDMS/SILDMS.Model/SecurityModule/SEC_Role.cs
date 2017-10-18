using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SILDMS.Model.DocScanningModule;

namespace SILDMS.Model.SecurityModule
{
    public class SEC_Role
    {
        public string RoleID { get; set; }
        public string OwnerID { get; set; }
        public string RoleTitle { get; set; }
        public string RoleType { get; set; }
        public string RoleDescription { get; set; }
        public string UserLevel { get; set; }
        public int Status { get; set; }
        public string SetOn { get; set; }
        public string SetBy { get; set; }
        public string ModifiedOn { get; set; }
        public string ModifiedBy { get; set; }

        public ddlDSMOwner Owner { get; set; }

    }
}
