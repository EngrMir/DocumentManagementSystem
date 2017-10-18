using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SILDMS.Model.SecurityModule
{
    public partial class AspNetUser
    {
        public AspNetUser()
        {
            this.AspNetUserClaims = new HashSet<AspNetUserClaim>();
            this.AspNetUserLogins = new HashSet<AspNetUserLogin>();
            this.AspNetUserRoles = new HashSet<AspNetUserRole>();
            this.Sec_NavigationMenu = new HashSet<Sec_NavigationMenu>();
            this.Sec_NavigationMenu1 = new HashSet<Sec_NavigationMenu>();
            this.Sec_RoleMenuMap = new HashSet<Sec_RoleMenuMap>();
            this.Sec_RoleMenuMap1 = new HashSet<Sec_RoleMenuMap>();
            this.Sec_Company = new HashSet<Sec_Company>();
            this.Sec_Company1 = new HashSet<Sec_Company>();
            this.Sec_Department = new HashSet<Sec_Department>();
            this.Sec_Department1 = new HashSet<Sec_Department>();
        }

        public string Id { get; set; }
        public string Email { get; set; }
        public bool EmailConfirmed { get; set; }
        public string PasswordHash { get; set; }
        public string SecurityStamp { get; set; }
        public string PhoneNumber { get; set; }
        public bool PhoneNumberConfirmed { get; set; }
        public bool TwoFactorEnabled { get; set; }
        public Nullable<System.DateTime> LockoutEndDateUtc { get; set; }
        public bool LockoutEnabled { get; set; }
        public int AccessFailedCount { get; set; }
        public string UserName { get; set; }
        public System.DateTime CreatedOn { get; set; }
        public System.DateTime ModifiedOn { get; set; }
        public int UserStatus { get; set; }

        public virtual ICollection<AspNetUserClaim> AspNetUserClaims { get; set; }
        public virtual ICollection<AspNetUserLogin> AspNetUserLogins { get; set; }
        public virtual ICollection<AspNetUserRole> AspNetUserRoles { get; set; }
        public virtual ICollection<Sec_NavigationMenu> Sec_NavigationMenu { get; set; }
        public virtual ICollection<Sec_NavigationMenu> Sec_NavigationMenu1 { get; set; }
        public virtual ICollection<Sec_RoleMenuMap> Sec_RoleMenuMap { get; set; }
        public virtual ICollection<Sec_RoleMenuMap> Sec_RoleMenuMap1 { get; set; }
        public virtual ICollection<Sec_Company> Sec_Company { get; set; }
        public virtual ICollection<Sec_Company> Sec_Company1 { get; set; }
        public virtual ICollection<Sec_Department> Sec_Department { get; set; }
        public virtual ICollection<Sec_Department> Sec_Department1 { get; set; }
    }
}
