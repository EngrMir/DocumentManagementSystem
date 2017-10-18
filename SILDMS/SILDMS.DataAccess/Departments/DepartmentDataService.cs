using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using SILDMS.DataAccessInterface.Users;
using SILDMS.Model.SecurityModule;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SILDMS.DataAccessInterface.Departments;
using Dept = SILDMS.Model.SecurityModule.Sec_Department;

namespace SILDMS.DataAccess.Departments
{
    public class DepartmentDataService : IDepartmentDataService
    {
        public List<Sec_Department> GetAllDepartments(string id, out string errorNumber)
        {
            errorNumber = string.Empty;
            var deptList = new List<Sec_Department>();
            var factory = new DatabaseProviderFactory();
            var db = factory.CreateDefault() as SqlDatabase;
            var query = new StringBuilder();

            query.Append("SELECT * FROM [dbo].[Sec_Department]");
            if (!string.IsNullOrEmpty(id))
            {
                query.Append("WHERE DepartmentId=" + id);
            }
            if (db != null)
            {
                var ds = db.ExecuteDataSet(CommandType.Text, query.ToString());

                if (ds.Tables[0].Rows.Count > 0)
                {
                    var dt = ds.Tables[0];
                    deptList = dt.AsEnumerable().Select(reader => new Sec_Department
                    {
                        DepartmentId = reader.GetInt32("DepartmentId"),
                        DepartmentShortName = reader.GetString("DepartmentShortName")
                    }).ToList();
                }
                
            }

            return deptList;
        }
    }
}
