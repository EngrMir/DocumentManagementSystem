using SILDMS.Model.SecurityModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SILDMS.DataAccessInterface.NavMenuOptSetup
{
    public interface INavMenuOptSetupDataService
    {
         List<SEC_NavMenuOptSetup> GetNavMenuOptSetup( string ownerID, out string errorNumber);

         string SetMenuOperations(SEC_MenuDetails menuDetails, string action, out string errorNumber);
    }
}
