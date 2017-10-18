using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using SILDMS.DataAccessInterface.Roles;
using SILDMS.Model.SecurityModule;

namespace SILDMS.DataAccess.Roles
{
    public class RoleDataService : IRoleDataService
    {
        public List<AspNetRole> GetAllRoles(string id, out string errorNumber)
        {
            errorNumber = string.Empty;
            var roleList = new List<AspNetRole>();
            var factory = new DatabaseProviderFactory();
            var db = factory.CreateDefault() as SqlDatabase;
            var query = new StringBuilder();

            query.Append("SELECT * FROM [dbo].[AspNetRoles]");
            if (string.IsNullOrEmpty(id))
            {
                query.Append("WHERE Id=" + id);
            }
            else
            {
                query.Append("ORDER BY Id");    
            }
            if (db !=null)
            {
                var ds = db.ExecuteDataSet(CommandType.Text, query.ToString());

                if (ds.Tables[0].Rows.Count > 0)
                {
                    var dt = ds.Tables[0];
                    roleList = dt.AsEnumerable().Select(reader => new AspNetRole
                    {
                        Id = reader.GetString("Id"),
                        Name = reader.GetString("Name")
                    }).ToList();
                }
            }

            return roleList;
        }
    }
}
