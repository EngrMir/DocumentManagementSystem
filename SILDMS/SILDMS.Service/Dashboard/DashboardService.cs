using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SILDMS.DataAccessInterface.DashboardDataService;
using SILDMS.Model.SecurityModule;
using SILDMS.Utillity.Localization;

namespace SILDMS.Service.Dashboard
{
    public class DashboardService : IDashboardService
    {

        #region Fields

        private readonly IDashboardDataService _dashboardDataService;
        private readonly ILocalizationService _localizationService;

        #endregion

        #region Constructor

        public DashboardService(IDashboardDataService dashboardDataService, ILocalizationService localizationService)
        {
            _dashboardDataService = dashboardDataService;
            _localizationService = localizationService;
        }
        
        #endregion

        public Model.SecurityModule.Dashboard GetDashboardInfo(string userId,DateTime? dateFrom, DateTime? dateTo)
        {
            var dashboard = new Model.SecurityModule.Dashboard {UserLogs = new List<SEC_UserLog>()};

            dashboard = _dashboardDataService.GetDashbordElements(userId,dateFrom,dateTo);

            return dashboard;

        }
    }
}
