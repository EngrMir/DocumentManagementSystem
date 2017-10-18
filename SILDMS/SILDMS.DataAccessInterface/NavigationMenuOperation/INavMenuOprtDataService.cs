using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SILDMS.Model.DocScanningModule;
using SILDMS.Model.SecurityModule;

namespace SILDMS.DataAccessInterface.NavigationMenuOperation
{
    public interface INavMenuOprtDataService
    {
        List<SEC_MenuOperation> GetMenuOperation(string id, string action, out string errorNumber);
        string ManipulateMenuOperation(SEC_MenuOperation menuOpteration, string action, out string errorNumber);
    }
}
