using SILDMS.DataAccessInterface;
using SILDMS.Model.SecurityModule;
using SILDMS.Utillity;
using SILDMS.Utillity.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SILDMS.DataAccessInterface.Roles;
using Role = SILDMS.Model.SecurityModule.AspNetRole;

namespace SILDMS.Service.Roles
{
    public class RoleService : IRoleService
    {
        #region Fields

        private readonly IRoleDataService _roleDataService;
        private readonly ILocalizationService _localizationService;
        private string _errorNumber = string.Empty;

        #endregion

        #region Constructor

        public RoleService(IRoleDataService role, ILocalizationService local)
        {
            _roleDataService = role;
            _localizationService = local;
        }

        #endregion

        public ValidationResult GetAllRoles(string id, out List<Role> roleList)
        {
            roleList = _roleDataService.GetAllRoles(id, out _errorNumber);
            return _errorNumber.Length > 0
                ? new ValidationResult(_errorNumber, _localizationService.GetResource(_errorNumber))
                : ValidationResult.Success;
        }
    }
}
