using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SILDMS.Model.SecurityModule
{
    public partial class AspNetRole
    {
        public AspNetRole()
        {
            this.AspNetUserRoles = new HashSet<AspNetUserRole>();
            this.Sec_RoleMenuMap = new HashSet<Sec_RoleMenuMap>();
        }

        public string Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<AspNetUserRole> AspNetUserRoles { get; set; }
        public virtual ICollection<Sec_RoleMenuMap> Sec_RoleMenuMap { get; set; }
    }
}
