using SILDMS.Model;
using SILDMS.Model.SecurityModule;
using SILDMS.Utillity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Role = SILDMS.Model.SecurityModule.AspNetRole;

namespace SILDMS.Service.Roles
{
    public partial interface IRoleService
    {
        ValidationResult GetAllRoles(string id, out List<Role> roleList);
    }
}
