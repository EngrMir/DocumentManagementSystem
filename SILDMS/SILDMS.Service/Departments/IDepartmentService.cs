using SILDMS.Model;
using SILDMS.Model.SecurityModule;
using SILDMS.Utillity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dept = SILDMS.Model.SecurityModule.Sec_Department;

namespace SILDMS.Service.Departments
{
    public interface IDepartmentService
    {
        ValidationResult GetAllDepartments(string id, out List<Sec_Department> deparmentList);
    }
}
