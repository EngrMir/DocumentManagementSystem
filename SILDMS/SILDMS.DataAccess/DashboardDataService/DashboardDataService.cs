using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using SILDMS.DataAccessInterface.DashboardDataService;
using SILDMS.Model.SecurityModule;

namespace SILDMS.DataAccess.DashboardDataService
{
    public class DashboardDataService : IDashboardDataService
    {
        public Dashboard GetDashbordElements(string userId, DateTime? dateFrom, DateTime? dateTo)
        {
            var returnObj = new Dashboard {UserLogs = new List<SEC_UserLog>()};

            var factory = new DatabaseProviderFactory();
            var db = factory.CreateDefault() as SqlDatabase;

            using (var dbCommandWrapper = db.GetStoredProcCommand("GetInfoDashBoard"))
            {
                
                db.AddInParameter(dbCommandWrapper, "@UserID", SqlDbType.VarChar, userId);
                db.AddInParameter(dbCommandWrapper, "@DateFrom", SqlDbType.DateTime, dateFrom);
                db.AddInParameter(dbCommandWrapper, "@DateTo", SqlDbType.DateTime, dateTo);
                var ds = db.ExecuteDataSet(dbCommandWrapper);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    returnObj.TotalDocuments = ds.Tables[0].Rows[0].GetInt32("TotalDocuments");


                }
                if (ds.Tables[1].Rows.Count > 0)
                {
                    returnObj.VersionOfVersioned = ds.Tables[1].Rows[0].GetInt32("VersionOfVersioned"); 
                }
                if (ds.Tables[2].Rows.Count > 0)
                {
                    returnObj.OriginalDocuments = ds.Tables[2].Rows[0].GetInt32("OriginalDocuments");
                }
                if (ds.Tables[3].Rows.Count > 0)
                {

                    returnObj.VersionedDocuments = ds.Tables[3].Rows[0].GetInt32("VersionedDocuments");
                  
                }
                if (ds.Tables[4].Rows.Count > 0)
                {
                    returnObj.UserLogs = ds.Tables[4].AsEnumerable().Select(ob => new SEC_UserLog
                    {
                        LogID = ob.GetString("LogID"),
                        UserID = ob.GetString("UserID"),
                        UsedIP = ob.GetString("UsedIP"),
                        ActionUrl = ob.GetString("ActionUrl"),
                        UserAction = ob.GetString("UserAction"),
                        ActionEventTime = ob.GetString("ActionEventTime"),
                        GenericID = ob.GetString("GenericID"),
                        Status = ob.GetInt32("Status")
                    }).OrderByDescending(ob=>ob.LogID).ToList();
                }
                
            }

            return returnObj;
        }
    }
}
