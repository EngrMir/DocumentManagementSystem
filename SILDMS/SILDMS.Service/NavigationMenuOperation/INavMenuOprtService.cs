using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SILDMS.Model.SecurityModule;
using SILDMS.Utillity;

namespace SILDMS.Service.NavigationMenuOperation
{
    public interface INavMenuOprtService
    {
        ValidationResult GetMenuOperation(string id, string action, out List<SEC_MenuOperation> menuOperationList);
        ValidationResult ManipulateMenuOperation(SEC_MenuOperation menuOperation, string action, out string status);
    }
}
