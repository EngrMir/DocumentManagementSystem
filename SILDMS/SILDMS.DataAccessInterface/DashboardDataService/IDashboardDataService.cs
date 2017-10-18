using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SILDMS.Model.SecurityModule;

namespace SILDMS.DataAccessInterface.DashboardDataService
{
    public interface IDashboardDataService
    {
        Dashboard GetDashbordElements(string userId, DateTime? dateFrom, DateTime? dateTo);
    }
}
