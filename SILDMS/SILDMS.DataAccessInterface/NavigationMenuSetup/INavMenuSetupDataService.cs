using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SILDMS.Model.SecurityModule;

namespace SILDMS.DataAccessInterface.NavigationMenuSetup
{
    public interface INavMenuSetupDataService
    {
        List<SEC_Menu> GetMenus(string id, string action, out string errorNumber);
        string ManipulateMenu(SEC_Menu menu, string action, out string errorNumber);
    }
}
