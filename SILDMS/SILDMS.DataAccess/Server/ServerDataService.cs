using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using SILDMS.DataAccessInterface.Server;
using SILDMS.Model.SecurityModule;
using SILDMS.Utillity;

namespace SILDMS.DataAccess.Server
{
    public class ServerDataService : IServerDataService
    {
        #region Fields

        private readonly string spErrorParam = "@p_Error";
        private readonly string spStatusParam = "@p_Status";

        #endregion

        #region Functions

        public List<SEC_Server> GetServers(string id, string action, out string errorNumber)
        {
            errorNumber = string.Empty;
            var ownersList = new List<SEC_Server>();
            var factory = new DatabaseProviderFactory();
            var db = factory.CreateDefault() as SqlDatabase;
            using (var dbCommandWrapper = db.GetStoredProcCommand("GetServerInfo"))
            {
                db.AddOutParameter(dbCommandWrapper, spErrorParam, DbType.Int32, 10);
                db.AddInParameter(dbCommandWrapper, "@ServerID", SqlDbType.VarChar, id);
                //db.AddInParameter(dbCommandWrapper, "@Action", SqlDbType.VarChar, action);
                

                var ds = db.ExecuteDataSet(dbCommandWrapper);

                if (!db.GetParameterValue(dbCommandWrapper, "@p_Error").IsNullOrZero())
                {
                    // Get the error number, if error occurred.
                    errorNumber = db.GetParameterValue(dbCommandWrapper, "@p_Error").PrefixErrorCode();
                }
                else
                {
                    if (ds.Tables[0].Rows.Count <= 0) return ownersList;
                    var dt1 = ds.Tables[0];
                    ownersList = dt1.AsEnumerable().Select(reader => new SEC_Server
                    {
                        ServerID = reader.GetString("ServerID"),
                        ServerIP = reader.GetString("ServerIP"),
                        LastReplacedIP = reader.GetString("LastReplacedIP"),
                        ServerName = reader.GetString("ServerName"),
                        ServerFor = reader.GetString("ServerFor"),
                        ServerType = reader.GetString("ServerType"),
                        ServerLocation = reader.GetString("ServerLocation"),
                        PurchaseDate = string.Format("{0:dd/mm/yyyy}", Convert.ToString(reader.GetDateTime("PurchaseDate").ToShortDateString(), CultureInfo.InvariantCulture)),
                        WarrantyPeriod = reader.GetInt32("WarrantyPeriod"),
                        ServerProcessor = reader.GetString("ServerProcessor"),
                        ServerRAM = reader.GetString("ServerRAM"),
                        ServerHDD = reader.GetString("ServerHDD"),
                        OwnerID = reader.GetString("OwnerID"),
                        OwnerName = reader.GetString("OwnerName"),
                        OwnerLevelID = reader.GetString("OwnerLevelID"),
                        LevelName = reader.GetString("LevelName"),
                        SetOn = string.Format("{0:dd/mm/yyyy}", Convert.ToString(reader.GetDateTime("SetOn"), CultureInfo.InvariantCulture)),
                        SetBy = reader.GetString("SetBy"),
                        ModifiedOn = string.Format("{0:dd/mm/yyyy}", reader.GetDateTime("ModifiedOn").ToString(CultureInfo.InvariantCulture)),
                        ModifiedBy = reader.GetString("ModifiedBy"),
                        Status = reader.GetInt16("Status"),
                        UserLevel = reader.GetInt32("UserLevel"),
                        FtpPort = reader.GetString("FtpPort"),
                        FtpUserName = reader.GetString("FtpUserName"),
                        FtpPassword = reader.GetString("FtpPassword"),
                    }).ToList();
                }
            }
            return ownersList;
        }

        public string ManipulateServer(SEC_Server server, string action, out string errorNumber)
        {
            errorNumber = string.Empty;
            try
            {
                var factory = new DatabaseProviderFactory();
                var db = factory.CreateDefault() as SqlDatabase;
                using (var dbCommandWrapper = db.GetStoredProcCommand("SetServerInfo"))
                {
                    // Set parameters 
                    db.AddInParameter(dbCommandWrapper, "@ServerID", SqlDbType.NVarChar, server.ServerID);
                    db.AddInParameter(dbCommandWrapper, "@ServerIP", SqlDbType.NVarChar, DataValidation.TrimmedOrDefault(server.ServerIP));
                    db.AddInParameter(dbCommandWrapper, "@LastReplacedIP", SqlDbType.NVarChar, server.LastReplacedIP);
                    db.AddInParameter(dbCommandWrapper, "@ServerName", SqlDbType.NVarChar, DataValidation.TrimmedOrDefault(server.ServerName));
                    db.AddInParameter(dbCommandWrapper, "@ServerFor", SqlDbType.NVarChar, DataValidation.TrimmedOrDefault(server.ServerFor));
                    db.AddInParameter(dbCommandWrapper, "@ServerType", SqlDbType.NVarChar, DataValidation.TrimmedOrDefault(server.ServerType));
                    db.AddInParameter(dbCommandWrapper, "@ServerLocation", SqlDbType.NVarChar, 
                        DataValidation.TrimmedOrDefault(server.ServerLocation));
                    db.AddInParameter(dbCommandWrapper, "@PurchaseDate", SqlDbType.DateTime, 
                        DataValidation.DateTimeConversion(DataValidation.TrimmedOrDefault(server.PurchaseDate)));
                    db.AddInParameter(dbCommandWrapper, "@ServerProcessor", SqlDbType.NVarChar, 
                        DataValidation.TrimmedOrDefault(server.ServerProcessor));
                    db.AddInParameter(dbCommandWrapper, "@ServerRAM", SqlDbType.NVarChar, DataValidation.TrimmedOrDefault(server.ServerRAM));
                    db.AddInParameter(dbCommandWrapper, "@ServerHDD", SqlDbType.NVarChar, DataValidation.TrimmedOrDefault(server.ServerHDD));
                    db.AddInParameter(dbCommandWrapper, "@OwnerID", SqlDbType.NVarChar, DataValidation.TrimmedOrDefault(server.OwnerID));
                    db.AddInParameter(dbCommandWrapper, "@WarrantyPeriod", SqlDbType.Int, server.WarrantyPeriod);
                    db.AddInParameter(dbCommandWrapper, "@UserLevel", SqlDbType.Int, server.UserLevel);
                    db.AddInParameter(dbCommandWrapper, "@SetBy ", SqlDbType.NVarChar, server.SetBy);
                    db.AddInParameter(dbCommandWrapper, "@ModifiedBy", SqlDbType.NVarChar, server.ModifiedBy);
                    db.AddInParameter(dbCommandWrapper, "@FtpPort", SqlDbType.NVarChar, server.FtpPort);
                    db.AddInParameter(dbCommandWrapper, "@FtpUserName", SqlDbType.NVarChar, server.FtpUserName);
                    db.AddInParameter(dbCommandWrapper, "@FtpPassword", SqlDbType.NVarChar, server.FtpPassword);
                    db.AddInParameter(dbCommandWrapper, "@Status", SqlDbType.Int, server.Status);
                    db.AddInParameter(dbCommandWrapper, "@Action", SqlDbType.VarChar, action);
                    db.AddOutParameter(dbCommandWrapper, spStatusParam, SqlDbType.VarChar, 10);
                    // Execute SP.
                    db.ExecuteNonQuery(dbCommandWrapper);
                    // Getting output parameters and setting response details.
                    if (!db.GetParameterValue(dbCommandWrapper, spStatusParam).IsNullOrZero())
                    {
                        // Get the error number, if error occurred.
                        errorNumber = db.GetParameterValue(dbCommandWrapper, spStatusParam).PrefixErrorCode();
                    }
                }
            }
            catch (Exception ex)
            {
                errorNumber = "E404"; // Log ex.Message  Insert Log Table               
            }
            return errorNumber;
        }

        #endregion
    }
}
