using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SILDMS.Model.SecurityModule
{
    public class SEC_NavMenuOptSetup
    {
        public string MenuDetailID { get; set; }
        public string MenuID { get; set; }
        public string MenuOperationID { get; set; }
        public string VisibleInRoleMenu { get; set; }
        public int? OwnerID { get; set; }
        public string UserLevel { get; set; }
        public string SetOn { get; set; }
        public int? Status { get; set; }
        public string MenuTitle { get; set; }
        public string MenuUrl { get; set; }
        public string ParentMenuID { get; set; }
        public string MenuIcon { get; set; }
        public int MenuOrder { get; set; }
        public string SetBy { get; set; }
        public string check { get; set; }
    }

    public class Parent {
        public string key { get; set; }
        public string title { get; set; }
        public string check { get; set; }
        public bool? select { get; set; }
        public bool?  isFolder { get; set; }
        public virtual ICollection<Child> children { get; set; }
    }
    public class Child
    {
        public string key { get; set; }
        public string title { get; set; }
        public string check { get; set; }
        public bool? select { get; set; }
        //public bool? expand { get; set; }
        public virtual ICollection<Child> children { get; set; }
    }

    public class SelectedMenu {
        public string MenuID { get; set; }
    }
}
