using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SILDMS.Model.SecurityModule
{
    public partial class Sec_RoleMenuMap
    {
        public int UserRoleMapId { get; set; }
        public string RoleId { get; set; }
        public int MenuId { get; set; }
        //public string CreatedBy { get; set; }
        //public System.DateTime CreatedDate { get; set; }
        //public string ModifiedBy { get; set; }
        //public System.DateTime ModifiedDate { get; set; }
        public string AccessPermission { get; set; }
        public int RoleMenuMapStatus { get; set; }
    //    public virtual Sec_NavigationMenu Sec_NavigationMenu { get; set; }
    }
}
