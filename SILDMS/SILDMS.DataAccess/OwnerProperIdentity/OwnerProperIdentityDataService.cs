using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using SILDMS.DataAccessInterface.OwnerProperIdentity;
using SILDMS.Model.DocScanningModule;

namespace SILDMS.DataAccess.OwnerProperIdentity
{
    public class OwnerProperIdentityDataService : IOwnerProperIdentityDataService
    {
        private readonly string spStatusParam = "@p_Status";

        public List<DSM_DocPropIdentify> GetDocPropIdentify(string _UserID, string docPropIdentifyID, out string errorNumber)
        {
            errorNumber = string.Empty;
            List<DSM_DocPropIdentify> docPropIdentifyList = new List<DSM_DocPropIdentify>();
            DatabaseProviderFactory factory = new DatabaseProviderFactory();
            SqlDatabase db = factory.CreateDefault() as SqlDatabase;
            using (DbCommand dbCommandWrapper = db.GetStoredProcCommand("GetDocPropIdentify"))
            {
                // Set parameters 
                db.AddInParameter(dbCommandWrapper, "@UserID", SqlDbType.VarChar, _UserID);
                db.AddInParameter(dbCommandWrapper, "@DocPropIdentifyID", SqlDbType.VarChar, docPropIdentifyID);
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
                        docPropIdentifyList = dt1.AsEnumerable().Select(reader => new DSM_DocPropIdentify
                        {
                            DocPropIdentifyID = reader.GetString("DocPropIdentifyID"),
                            DocPropertyID = reader.GetString("DocPropertyID"),
                            DocCategoryID = reader.GetString("DocCategoryID"),
                            DocTypeID = reader.GetString("DocTypeID"),
                            OwnerID = reader.GetString("OwnerID"),
                            IdentificationCode = reader.GetString("IdentificationCode"),
                            IdentificationSL = reader.GetString("IdentificationSL"),
                            AttributeGroup = reader.GetString("AttributeGroup"),
                            IdentificationAttribute = reader.GetString("IdentificationAttribute"),
                            IsRequired = reader.GetInt16("IsRequired"),
                            IsAuto = reader.GetInt16("IsAuto"),
                            IsRestricted = reader.GetInt16("IsRestriction"),
                            SetOn = reader.GetString("SetOn"),
                            SetBy = reader.GetString("SetBy"),
                            ModifiedOn = reader.GetString("ModifiedOn"),
                            ModifiedBy = reader.GetString("ModifiedBy"),
                            Status = reader.GetInt32("Status"),
                            Remarks = reader.GetString("Remarks")
                        }).ToList();
                    }
                }
            }
            return docPropIdentifyList;
        }

        public string AddOwnerPropIdentity(DSM_DocPropIdentify modelDsmDocPropIdentify, string action, out string errorNumber)
        {
            errorNumber = String.Empty;
            try
            {
                DatabaseProviderFactory factory = new DatabaseProviderFactory();
                SqlDatabase db = factory.CreateDefault() as SqlDatabase;
                using (DbCommand dbCommandWrapper = db.GetStoredProcCommand("SetOwnerPropIdentity"))
                {
                    db.AddInParameter(dbCommandWrapper, "@DocPropIdentifyID", SqlDbType.NVarChar, modelDsmDocPropIdentify.DocPropIdentifyID);
                    db.AddInParameter(dbCommandWrapper, "@OwnerLevelID", SqlDbType.NVarChar, modelDsmDocPropIdentify.OwnerLevel.OwnerLevelID);
                    db.AddInParameter(dbCommandWrapper, "@OwnerID", SqlDbType.NVarChar, modelDsmDocPropIdentify.Owner.OwnerID);
                    db.AddInParameter(dbCommandWrapper, "@DocCategoryID ", SqlDbType.NVarChar, modelDsmDocPropIdentify.DocCategory.DocCategoryID);
                    db.AddInParameter(dbCommandWrapper, "@DocTypeID", SqlDbType.NVarChar, modelDsmDocPropIdentify.DocType.DocTypeID);
                    db.AddInParameter(dbCommandWrapper, "@DocPropertyID", SqlDbType.NVarChar, modelDsmDocPropIdentify.DocProperty.DocPropertyID);

                    db.AddInParameter(dbCommandWrapper, "@IdentificationCode", SqlDbType.NVarChar, modelDsmDocPropIdentify.IdentificationCode);
                    db.AddInParameter(dbCommandWrapper, "@IdentificationSL", SqlDbType.NVarChar, modelDsmDocPropIdentify.IdentificationSL);
                    db.AddInParameter(dbCommandWrapper, "@AttributeGroup ", SqlDbType.NVarChar, modelDsmDocPropIdentify.AttributeGroup);
                    db.AddInParameter(dbCommandWrapper, "@IdentificationAttribute", SqlDbType.NVarChar, modelDsmDocPropIdentify.IdentificationAttribute);

                    db.AddInParameter(dbCommandWrapper, "@IsRequired", SqlDbType.Bit, modelDsmDocPropIdentify.IsRequired);
                    db.AddInParameter(dbCommandWrapper, "@IsAuto", SqlDbType.Bit, modelDsmDocPropIdentify.IsAuto);
                    db.AddInParameter(dbCommandWrapper, "@IsRestriction", SqlDbType.Bit, modelDsmDocPropIdentify.IsRestricted);
                    db.AddInParameter(dbCommandWrapper, "@Remarks", SqlDbType.NVarChar, modelDsmDocPropIdentify.Remarks);

                    db.AddInParameter(dbCommandWrapper, "@SetBy ", SqlDbType.NVarChar, modelDsmDocPropIdentify.SetBy);
                    db.AddInParameter(dbCommandWrapper, "@ModifiedBy", SqlDbType.NVarChar, modelDsmDocPropIdentify.ModifiedBy);
                    db.AddInParameter(dbCommandWrapper, "@Status", SqlDbType.Int, modelDsmDocPropIdentify.Status);
                    db.AddInParameter(dbCommandWrapper, "@Action", SqlDbType.VarChar, action);
                    db.AddOutParameter(dbCommandWrapper, spStatusParam, SqlDbType.VarChar, 10);
                    
                    db.ExecuteNonQuery(dbCommandWrapper);
                    
                    if (!db.GetParameterValue(dbCommandWrapper, spStatusParam).IsNullOrZero())
                    {
                        errorNumber = db.GetParameterValue(dbCommandWrapper, spStatusParam).PrefixErrorCode();
                    }
                }
            }
            catch (Exception ex)
            {
                errorNumber = "E404";             
            }
            return errorNumber;
        }
    }
}
