using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SILDMS.DataAccessInterface.Departments;
using SILDMS.Model.SecurityModule;
using SILDMS.Utillity;
using SILDMS.Utillity.Localization;

namespace SILDMS.Service.Departments
{
    public class DepartmentService : IDepartmentService
    {
        #region Fields

        private readonly IDepartmentDataService _deptDataService;
        private readonly ILocalizationService _localizationService;
        private string _errorNumber = string.Empty;

        #endregion

        #region Constructor

        public DepartmentService(IDepartmentDataService deptDataService, ILocalizationService localizationService)
        {
            this._deptDataService = deptDataService;
            this._localizationService = localizationService;

        }

        #endregion

        public ValidationResult GetAllDepartments(string id, out List<Sec_Department> deptList)
        {
            deptList = _deptDataService.GetAllDepartments(id, out _errorNumber);
            return _errorNumber.Length > 0 ? new ValidationResult(_errorNumber, _localizationService.GetResource(_errorNumber))
                : ValidationResult.Success;
        }
    }
}
