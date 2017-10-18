using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SILDMS.DataAccessInterface.Server;
using SILDMS.Model.SecurityModule;
using SILDMS.Utillity;
using SILDMS.Utillity.Localization;

namespace SILDMS.Service.Server
{
    public class ServerService : IServerService
    {
        #region Fields

        private readonly IServerDataService _serverDataService;
        private readonly ILocalizationService _localizationService;
        private string _errorNumber = string.Empty;

        #endregion

        #region Constructor

        public ServerService(IServerDataService serverDataService, ILocalizationService localizationService)
        {
            _serverDataService = serverDataService;
            _localizationService = localizationService;
        }

        #endregion

        #region Functions
        public ValidationResult GetServers(string id, string action, out List<SEC_Server> serverList)
        {
            serverList = _serverDataService.GetServers(id, action, out _errorNumber);
            return _errorNumber.Length > 0 ? new ValidationResult(_errorNumber, _localizationService.GetResource(_errorNumber)) 
                : ValidationResult.Success;
        }

        public ValidationResult ManipulateServer(SEC_Server server, string action, out string status)
        {
            _serverDataService.ManipulateServer(server, action, out status);
            return status.Length > 0 ? new ValidationResult(status, _localizationService.GetResource(status)) : ValidationResult.Success;
        }

        #endregion
    }
}
