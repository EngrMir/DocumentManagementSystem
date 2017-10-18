using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SILDMS.Utillity
{
    public class ExcelReader
    {
        public DataTable GetExcelDataTable()
        {
            var postedFile = Request.Files[0];

            string strGuid = Convert.ToString(Guid.NewGuid());
            string strTempPath = Path.GetTempPath();
            string filename = Path.Combine(strTempPath, strGuid + Path.GetFileName(postedFile.FileName));

            postedFile.SaveAs(filename);

            string connectionString = "";

            string[] d = postedFile.FileName.Split('.');
            string fileExtension = "." + d[d.Length - 1].ToString();
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

            string query = String.Format("SELECT * from [{0}]", GetSchemaTable(connectionString));
            OleDbDataAdapter dataAdapter = new OleDbDataAdapter(query, connectionString);
            DataSet dataSet = new DataSet();
            dataAdapter.Fill(dataSet);
            return dataSet.Tables[0];

        }
    }
}
