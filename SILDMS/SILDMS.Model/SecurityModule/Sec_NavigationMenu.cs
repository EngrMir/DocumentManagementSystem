using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SILDMS.Model.SecurityModule
{
    public class Sec_NavigationMenu
    {
        public string MenuID { get; set; }
        public string MenuTitle { get; set; }
        public string MenuUrl { get; set; }
        public string ParentMenuID { get; set; }
        public string MenuIcon { get; set; }

        public int? MenuOrder { get; set; }
        public int? TotalUserAllowed { get; set; }
        public int? ConcurrentUser { get; set; }
        public string OwnerID { get; set; }
        public int? UserLevel { get; set; }
        public string SetOn { get; set; }
        public string SetBy { get; set; }
        public string ModifiedOn { get; set; }
        public string ModifiedBy { get; set; }
        public int? Status { get; set; }

    }
}
