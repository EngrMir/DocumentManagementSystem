using SILDMS.Model.SecurityModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dept = SILDMS.Model.SecurityModule.Sec_Department;

namespace SILDMS.DataAccessInterface.Departments
{
    public interface IDepartmentDataService
    {
        List<Dept> GetAllDepartments(string id, out string errorNumber);
    }
}
