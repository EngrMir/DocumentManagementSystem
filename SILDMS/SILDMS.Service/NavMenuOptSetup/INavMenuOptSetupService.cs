using SILDMS.Model.SecurityModule;
using SILDMS.Utillity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SILDMS.Service.NavMenuOptSetup
{
    public interface INavMenuOptSetupService
    {
        object GetNavMenuOptSetup(string ownerID, out List<SEC_NavMenuOptSetup> setupService);

        ValidationResult SetMenuOperations(SEC_MenuDetails menuDetails, string action, out string status);
    }
}
