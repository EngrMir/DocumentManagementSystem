using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SILDMS.Model.SecurityModule;
using SILDMS.Utillity;

namespace SILDMS.Service.NavigationMenuSetup
{
    public interface INavMenuSetupService
    {
        ValidationResult GetMenus(string id, string action, out List<SEC_Menu> menuList);
        ValidationResult ManipulateMenu(SEC_Menu menu, string action, out string status);
    }
}
