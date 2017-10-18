using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using SILDMS.DataAccessInterface.OwnerProperty;
using SILDMS.Model.DocScanningModule;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SILDMS.DataAccess.OwnerProperty
{
    public class DocPropertyDataService : IDocPropertyDataService
    {
        private readonly string spStatusParam = "@p_Status";
        public List<DSM_DocProperty> GetDocProperty(string DocPropertyId, string userID, out string errorNumber)
        {
            errorNumber = string.Empty;
            List<DSM_DocProperty> docPropertyList = new List<DSM_DocProperty>();
            DatabaseProviderFactory factory = new DatabaseProviderFactory();
            SqlDatabase db = factory.CreateDefault() as SqlDatabase;
            using (DbCommand dbCommandWrapper = db.GetStoredProcCommand("GetDocProperty"))
            {
                // Set parameters 
                db.AddInParameter(dbCommandWrapper, "@UserID", SqlDbType.VarChar, userID);
                db.AddInParameter(dbCommandWrapper, "@DocPropertyID", SqlDbType.VarChar, DocPropertyId);
                db.AddOutParameter(dbCommandWrapper, spStatusParam, DbType.String, 10);
                // Execute SP.
                DataSet ds = db.ExecuteDataSet(dbCommandWrapper);

                if (!db.GetParameterValue(dbCommandWrapper, spStatusParam).IsNullOrZero())
                {
                    // Get the error number, if error occurred.
                    errorNumber = db.GetParameterValue(dbCommandWrapper, spStatusParam).PrefixErrorCode();
                }
                else
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        DataTable dt1 = ds.Tables[0];
                        docPropertyList = dt1.AsEnumerable().Select(reader => new DSM_DocProperty
                        {
                            DocPropertyID = reader.GetString("DocPropertyID"),
                            DocCategoryID = reader.GetString("DocCategoryID"),
                            OwnerLevelID = reader.GetString("OwnerLevelID"),
                            OwnerID = reader.GetString("OwnerID"),
                            DocTypeID = reader.GetString("DocTypeID"),
                            DocPropertySL = reader.GetString("DocPropertySL"),
                            UDDocPropertyCode = reader.GetString("UDDocPropertyCode"),
                            DocPropertyName = reader.GetString("DocPropertyName"),
                            DocClassification = reader.GetString("DocClassification"),
                            PreservationPolicy = reader.GetString("PreservationPolicy"),
                            PhysicalLocation = reader.GetString("PhysicalLocation"),
                            SerialNo = reader.GetInt32("SerialNo"),
                            Remarks = reader.GetString("Remarks"),
                            SetOn = reader.GetString("SetOn"),
                            SetBy = reader.GetString("SetBy"),
                            ModifiedOn = reader.GetString("ModifiedOn"),
                            ModifiedBy = reader.GetString("ModifiedBy"),
                            Status = reader.GetInt32("Status")
                        }).ToList();
                    }
                }
            }
            return docPropertyList;
        }

        public string AddDocProperty( DSM_DocProperty docProperty, string action, out string errorNumber)
        {
            errorNumber = String.Empty;
            try
            {
                DatabaseProviderFactory factory = new DatabaseProviderFactory();
                SqlDatabase db = factory.CreateDefault() as SqlDatabase;
                using (DbCommand dbCommandWrapper = db.GetStoredProcCommand("SetDocProperty"))
                {
                    // Set parameters 
                    db.AddInParameter(dbCommandWrapper, "@DocPropertyID", SqlDbType.NVarChar, docProperty.DocPropertyID);
                    db.AddInParameter(dbCommandWrapper, "@DocCategoryID", SqlDbType.NVarChar, docProperty.DocCategoryID.Trim());
                    db.AddInParameter(dbCommandWrapper, "@OwnerLevelID", SqlDbType.NVarChar, docProperty.OwnerLevelID.Trim());
                    db.AddInParameter(dbCommandWrapper, "@OwnerID", SqlDbType.NVarChar, docProperty.OwnerID.Trim());
                    db.AddInParameter(dbCommandWrapper, "@DocTypeID ", SqlDbType.NVarChar, docProperty.DocTypeID.Trim());
                    db.AddInParameter(dbCommandWrapper, "@DocPropertySL", SqlDbType.NVarChar, docProperty.DocPropertySL !=null?docProperty.DocPropertySL.Trim():"");
                    db.AddInParameter(dbCommandWrapper, "@UDDocPropertyCode", SqlDbType.NVarChar, docProperty.UDDocPropertyCode !=null? docProperty.UDDocPropertyCode.Trim():"");
                    db.AddInParameter(dbCommandWrapper, "@DocPropertyName", SqlDbType.NVarChar, docProperty.DocPropertyName !=null? docProperty.DocPropertyName.Trim():"");
                    db.AddInParameter(dbCommandWrapper, "@DocClassification", SqlDbType.NVarChar, docProperty.DocClassification !=null? docProperty.DocClassification.Trim():"");
                    db.AddInParameter(dbCommandWrapper, "@PreservationPolicy ", SqlDbType.NVarChar, docProperty.PreservationPolicy);
                    db.AddInParameter(dbCommandWrapper, "@PhysicalLocation", SqlDbType.NVarChar, docProperty.PhysicalLocation !=null? docProperty.PhysicalLocation.Trim():"");
                    db.AddInParameter(dbCommandWrapper, "@Remarks", SqlDbType.NVarChar, docProperty.Remarks !=null? docProperty.Remarks.Trim():"");
                    db.AddInParameter(dbCommandWrapper, "@SetBy ", SqlDbType.NVarChar, docProperty.SetBy.Trim());
                    db.AddInParameter(dbCommandWrapper, "@ModifiedBy", SqlDbType.NVarChar, docProperty.ModifiedBy.Trim());
                    db.AddInParameter(dbCommandWrapper, "@SerialNo", SqlDbType.Int, docProperty.SerialNo);
                    db.AddInParameter(dbCommandWrapper, "@Status", SqlDbType.Int, docProperty.Status);
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
    }
}
