using SILDMS.Model.SecurityModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SILDMS.DataAccessInterface.Menu
{
    public interface IMenuDataService
    {
        List<SEC_Menu> GetMenu(string ownerID,string menuID, string action, out string errorNumber);
        string AddMenu(SEC_Menu menu, string action, out string errorNumber);
        string EditMenu(SEC_Menu menu, string action, out string errorNumber);
        string DeleteMenu(SEC_Menu menu, string action, out string errorNumber);

    }
}
