using SILDMS.Model.SecurityModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Role = SILDMS.Model.SecurityModule.AspNetRole;

namespace SILDMS.DataAccessInterface.Roles
{
    public interface IRoleDataService
    {
        List<Role> GetAllRoles(string id, out string errorNumber);
    }
}
