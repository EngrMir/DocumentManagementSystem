using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SILDMS.Model.SecurityModule
{
    public partial class Sec_Department
    {
        public int DepartmentId { get; set; }
        public string DepartmentShortName { get; set; }
        public string DepartmentLongName { get; set; }
        public int DepartmentParent { get; set; }
        public string DepartmentType { get; set; }
        public int CompanyId { get; set; }
        public string CreatedBy { get; set; }
        public System.DateTime CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public System.DateTime ModifiedOn { get; set; }
        public int DepartmentStatus { get; set; }
        public virtual Sec_Company Sec_Company { get; set; }
    }
}
