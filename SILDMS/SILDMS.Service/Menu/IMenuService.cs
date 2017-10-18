using SILDMS.Model.SecurityModule;
using SILDMS.Utillity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.HtmlControls;

namespace SILDMS.Service.Menu
{
    public interface IMenuService
    {
        ValidationResult GetMenu(string ownerID,string menuID, string action, out List<SEC_Menu> menuList);
        ValidationResult AddMenu(SEC_Menu menu, string action, out string status);
        ValidationResult EditMenu(SEC_Menu menu, string action, out string status);
        ValidationResult DeleteMenu(SEC_Menu menu, string action, out string status);

        StringBuilder GenerateMenu(List<SEC_Menu> lstMenu);
    }
}
