using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace SILDMS.Utillity
{
    public class ExcelFileReader
    {
        public DataTable GetExcelDataTable(HttpPostedFileBase postedFile)
        {
            var strGuid = Convert.ToString(Guid.NewGuid());
            var strTempPath = Path.GetTempPath();
            var filename = Path.Combine(strTempPath, strGuid + Path.GetFileName(postedFile.FileName));

            postedFile.SaveAs(filename);

            var connectionString = "";

            var d = postedFile.FileName.Split('.');
            var fileExtension = "." + d[d.Length - 1].ToString();
            if (d.Length > 0)
            {
                if (fileExtension == ".xls")
                {
                    connectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + filename + ";Extended Properties=\"Excel 8.0;HDR=Yes;IMEX=1\"";
                }
                //connection String for xlsx file format.
                else if (fileExtension == ".xlsx")
                {
                    connectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + filename + ";Extended Properties=\"Excel 12.0;HDR=Yes;IMEX=1\"";
                }
                else
                {
                    return null;
                }
            }

            var query = String.Format("SELECT * from [{0}]", GetSchemaTable(connectionString));
            var dataAdapter = new OleDbDataAdapter(query, connectionString);
            var dataSet = new DataSet();
            dataAdapter.Fill(dataSet);
            return dataSet.Tables[0];

        }

        public string GetSchemaTable(string connectionString)
        {
            using (var connection = new
                       OleDbConnection(connectionString))
            {
                connection.Open();
                var schemaTable = connection.GetOleDbSchemaTable(
                    OleDbSchemaGuid.Tables,
                    new object[] { null, null, null, "TABLE" });
                if (schemaTable != null) return schemaTable.Rows[0].ItemArray[2].ToString();

            }
            return "Sheet1$";
        }
    }
}
