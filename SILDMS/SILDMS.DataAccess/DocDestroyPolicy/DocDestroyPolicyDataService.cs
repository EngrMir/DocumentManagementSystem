
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using SILDMS.DataAccessInterface;
using SILDMS.Model.DocScanningModule;
using SILDMS.Model.SecurityModule;

namespace SILDMS.DataAccess
{
    public class DocDestroyPolicyDataService : IDocDestroyPolicyDataService
    {
        private readonly string spStatusParam = "@p_Status";
        public List<DSM_DestroyPolicy> GetDestroyPolicyBySearchParam(string _DestroyPolicyID, 
            string _UserID, string _OwnerID,
            string _DocCategoryID, string _DocTypeID,
            string _DocPropertyID, string _DocPropIdentityID, out string _errorNumber)
        {
            _errorNumber = string.Empty;
            List<DSM_DestroyPolicy> destroyPolicies = new List<DSM_DestroyPolicy>();

            DatabaseProviderFactory factory = new DatabaseProviderFactory();
            SqlDatabase db = factory.CreateDefault() as SqlDatabase;
            using (DbCommand dbCommandWrapper = db.GetStoredProcCommand("GetDestroyPolicyBySearchParam"))
            {
                // Set parameters 
                db.AddInParameter(dbCommandWrapper, "@DestroyPolicyID", SqlDbType.VarChar, _DestroyPolicyID);
                db.AddInParameter(dbCommandWrapper, "@UserID", SqlDbType.VarChar, _UserID);
                db.AddInParameter(dbCommandWrapper, "@OwnerID", SqlDbType.VarChar, _OwnerID);
                db.AddInParameter(dbCommandWrapper, "@DocCategory", SqlDbType.VarChar, _DocCategoryID);
                db.AddInParameter(dbCommandWrapper, "@DocType", SqlDbType.VarChar, _DocTypeID);
                db.AddInParameter(dbCommandWrapper, "@DocProperty", SqlDbType.VarChar, _DocPropertyID);
                db.AddInParameter(dbCommandWrapper, "@DocPropIdentity", SqlDbType.VarChar, _DocPropIdentityID);
                
                db.AddOutParameter(dbCommandWrapper, spStatusParam, DbType.String, 10);
                // Execute SP.
                DataSet ds = db.ExecuteDataSet(dbCommandWrapper);

                if (!db.GetParameterValue(dbCommandWrapper, spStatusParam).IsNullOrZero())
                {
                    // Get the error number, if error occurred.
                    _errorNumber = db.GetParameterValue(dbCommandWrapper, spStatusParam).PrefixErrorCode();
                }
                else
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        DataTable dt1 = ds.Tables[0];
                        destroyPolicies = dt1.AsEnumerable().Select(reader => new DSM_DestroyPolicy
                        {
                            DestroyPolicyID = reader.GetString("DestroyPolicyID"),
                            DestroyPolicyDtlID = reader.GetString("DestroyPolicyDtlID"),
                            PolicyFor = reader.GetString("PolicyFor"),
                            PolicyApplicableTo = reader.GetString("PolicyApplicableTo"),
                            DocumentNature = reader.GetString("DocumentNature"),
                            PolicyDetailStatus = reader.GetInt32("PolicyDetailStatus"),
                            PolicyStatus = reader.GetInt32("PolicyStatus"),
                            Status = reader.GetInt32("PolicyStatus"),
                            Remarks = reader.GetString("Remarks"),
                            OwnerID = reader.GetString("OwnerID"),
                            DocCategoryID = reader.GetString("DocCategoryID"),
                            DocTypeID = reader.GetString("DocTypeID"),
                            DocPropertyID = reader.GetString("DocPropertyID"),
                            DocPropIdentifyID = reader.GetString("DocPropIdentifyID"),
                            IsSelected = reader.GetBoolean("PolicyDetailStatus"),

                            TimeValue = reader.GetString("TimeValue"),
                            TimeUnit = reader.GetString("TimeUnit"),
                            ExceptionValue = reader.GetString("ExceptionValue")


                        }).ToList();
                    }
                }
            }
            return destroyPolicies;
        }


        public string SetDocDestroyPolicy(DSM_DestroyPolicy model, string action, out string errorNumber)
        {
            errorNumber = String.Empty;
            try
            {
                //IFormatProvider culture = new System.Globalization.CultureInfo("fr-FR", true);
                DatabaseProviderFactory factory = new DatabaseProviderFactory();
                SqlDatabase db = factory.CreateDefault() as SqlDatabase;

                if (model.DocCategoryModel != null)
                {
                    DataTable docCategoryDataTable = new DataTable();
                    docCategoryDataTable.Columns.Add("DestroyPolicyDtlID");
                    docCategoryDataTable.Columns.Add("DocCategoryID");
                    docCategoryDataTable.Columns.Add("TimeValue");
                    docCategoryDataTable.Columns.Add("TimeUnit");
                    docCategoryDataTable.Columns.Add("ExceptionValue");
                    

                    foreach (var item in model.DocCategoryModel)
                    {
                        DataRow objDataRow = docCategoryDataTable.NewRow();
                        
                        objDataRow[0] = item.DestroyPolicyDtlID;
                        objDataRow[1] = item.CategoryID;
                        objDataRow[2] = item.TimeValue;
                        objDataRow[3] = item.TimeUnit;
                        objDataRow[4] = item.ExceptionValue;
                        

                        docCategoryDataTable.Rows.Add(objDataRow);
                    }

                    using (DbCommand dbCommandWrapper = db.GetStoredProcCommand("SetDocCategoryToDocDestroyPolicy"))
                    {
                        db.AddInParameter(dbCommandWrapper, "@DestroyPolicyID", SqlDbType.NVarChar, model.DestroyPolicyID ?? "");
                        db.AddInParameter(dbCommandWrapper, "@PolicyFor", SqlDbType.NVarChar, model.PolicyFor);
                        db.AddInParameter(dbCommandWrapper, "@DocumentNature", SqlDbType.NVarChar, model.DocumentNature);
                        db.AddInParameter(dbCommandWrapper, "@PolicyApplicableTo", SqlDbType.NVarChar, model.PolicyApplicableTo);
                        db.AddInParameter(dbCommandWrapper, "@Status", SqlDbType.NVarChar, model.Status);
                        db.AddInParameter(dbCommandWrapper, "@OwnerID", SqlDbType.NVarChar, model.OwnerID);
                        db.AddInParameter(dbCommandWrapper, "@UserID", SqlDbType.NVarChar, model.SetBy);
                        db.AddInParameter(dbCommandWrapper, "@Doc_Category", SqlDbType.Structured, docCategoryDataTable);

                        db.AddOutParameter(dbCommandWrapper, spStatusParam, SqlDbType.VarChar, 10);
                        db.ExecuteNonQuery(dbCommandWrapper);

                        if (!db.GetParameterValue(dbCommandWrapper, spStatusParam).IsNullOrZero())
                        {
                            errorNumber = db.GetParameterValue(dbCommandWrapper, spStatusParam).PrefixErrorCode();
                        }
                    }
                }



                else if (model.DocTypeModel != null)
                {
                    DataTable docTypeDataTable = new DataTable();
                    docTypeDataTable.Columns.Add("DestroyPolicyDtlID");
                    docTypeDataTable.Columns.Add("DocTypeID");
                    docTypeDataTable.Columns.Add("TimeValue");
                    docTypeDataTable.Columns.Add("TimeUnit");
                    docTypeDataTable.Columns.Add("ExceptionValue");
                    
                    
                    foreach (var item in model.DocTypeModel)
                    {
                        DataRow objDataRow = docTypeDataTable.NewRow();
                        objDataRow[0] = item.DestroyPolicyDtlID;
                        objDataRow[1] = item.TypeID;
                        objDataRow[2] = item.TimeValue;
                        objDataRow[3] = item.TimeUnit;
                        objDataRow[4] = item.ExceptionValue;
                        

                        docTypeDataTable.Rows.Add(objDataRow);
                    }

                    using (DbCommand dbCommandWrapper = db.GetStoredProcCommand("SetDocTypeToDocDestroyPolicy"))
                    {
                        db.AddInParameter(dbCommandWrapper, "@DestroyPolicyID", SqlDbType.NVarChar, model.DestroyPolicyID ?? "");
                        db.AddInParameter(dbCommandWrapper, "@PolicyFor", SqlDbType.NVarChar, model.PolicyFor);
                        db.AddInParameter(dbCommandWrapper, "@DocumentNature", SqlDbType.NVarChar, model.DocumentNature);
                        db.AddInParameter(dbCommandWrapper, "@PolicyApplicableTo", SqlDbType.NVarChar, model.PolicyApplicableTo);
                        db.AddInParameter(dbCommandWrapper, "@Status", SqlDbType.NVarChar, model.Status);
                        db.AddInParameter(dbCommandWrapper, "@OwnerID", SqlDbType.NVarChar, model.OwnerID);
                        db.AddInParameter(dbCommandWrapper, "@UserID", SqlDbType.NVarChar, model.SetBy);
                        db.AddInParameter(dbCommandWrapper, "@DocCategoryID", SqlDbType.NVarChar, model.DocCategoryID);

                        db.AddInParameter(dbCommandWrapper, "@Doc_Type", SqlDbType.Structured, docTypeDataTable);
                        db.AddOutParameter(dbCommandWrapper, spStatusParam, SqlDbType.VarChar, 10);
                        db.ExecuteNonQuery(dbCommandWrapper);

                        if (!db.GetParameterValue(dbCommandWrapper, spStatusParam).IsNullOrZero())
                        {
                            errorNumber = db.GetParameterValue(dbCommandWrapper, spStatusParam).PrefixErrorCode();
                        }
                    }
                }

                else if (model.DocPropertyModel != null)
                {
                    DataTable docPropertyDataTable = new DataTable();
                    docPropertyDataTable.Columns.Add("DestroyPolicyDtlID");
                    docPropertyDataTable.Columns.Add("DocPropertyID");
                    docPropertyDataTable.Columns.Add("TimeValue");
                    docPropertyDataTable.Columns.Add("TimeUnit");
                    docPropertyDataTable.Columns.Add("ExceptionValue");
                    
                    foreach (var item in model.DocPropertyModel)
                    {
                        DataRow objDataRow = docPropertyDataTable.NewRow();
                        objDataRow[0] = item.DestroyPolicyDtlID;
                        objDataRow[1] = item.PropertyID;
                        objDataRow[2] = item.TimeValue;
                        objDataRow[3] = item.TimeUnit;
                        objDataRow[4] = item.ExceptionValue;
                        

                        docPropertyDataTable.Rows.Add(objDataRow);
                    }

                    using (DbCommand dbCommandWrapper = db.GetStoredProcCommand("SetDocPropertyToDocDestroyPolicy"))
                    {
                        db.AddInParameter(dbCommandWrapper, "@DestroyPolicyID", SqlDbType.NVarChar, model.DestroyPolicyID ?? "");
                        db.AddInParameter(dbCommandWrapper, "@PolicyFor", SqlDbType.NVarChar, model.PolicyFor);
                        db.AddInParameter(dbCommandWrapper, "@DocumentNature", SqlDbType.NVarChar, model.DocumentNature);
                        db.AddInParameter(dbCommandWrapper, "@PolicyApplicableTo", SqlDbType.NVarChar, model.PolicyApplicableTo);
                        db.AddInParameter(dbCommandWrapper, "@Status", SqlDbType.NVarChar, model.Status);
                        db.AddInParameter(dbCommandWrapper, "@OwnerID", SqlDbType.NVarChar, model.OwnerID);
                        db.AddInParameter(dbCommandWrapper, "@UserID", SqlDbType.NVarChar, model.SetBy);
                        db.AddInParameter(dbCommandWrapper, "@DocCategoryID", SqlDbType.NVarChar, model.DocCategoryID);
                        db.AddInParameter(dbCommandWrapper, "@DocTypeID", SqlDbType.NVarChar, model.DocTypeID);

                        db.AddInParameter(dbCommandWrapper, "@Doc_Property", SqlDbType.Structured, docPropertyDataTable);
                        db.AddOutParameter(dbCommandWrapper, spStatusParam, SqlDbType.VarChar, 10);

                        db.ExecuteNonQuery(dbCommandWrapper);

                        if (!db.GetParameterValue(dbCommandWrapper, spStatusParam).IsNullOrZero())
                        {
                            errorNumber = db.GetParameterValue(dbCommandWrapper, spStatusParam).PrefixErrorCode();
                        }
                    }
                }



                else if (model.DocPropIdentityModel != null)
                {
                    DataTable docPropIdentifyDataTable = new DataTable();
                    docPropIdentifyDataTable.Columns.Add("DestroyPolicyDtlID");
                    docPropIdentifyDataTable.Columns.Add("DocPropIdentifyID");
                    docPropIdentifyDataTable.Columns.Add("TimeValue");
                    docPropIdentifyDataTable.Columns.Add("TimeUnit");
                    docPropIdentifyDataTable.Columns.Add("ExceptionValue");
                    

                    foreach (var item in model.DocPropIdentityModel)
                    {
                        DataRow objDataRow = docPropIdentifyDataTable.NewRow();
                        objDataRow[0] = item.DestroyPolicyDtlID;
                        objDataRow[1] = item.PropIdentityID;
                        objDataRow[2] = item.TimeValue;
                        objDataRow[3] = item.TimeUnit;
                        objDataRow[4] = item.ExceptionValue;
                        

                        docPropIdentifyDataTable.Rows.Add(objDataRow);
                    }

                    using (DbCommand dbCommandWrapper = db.GetStoredProcCommand("SetDocPropIdentityToDocDestroyPolicy"))
                    {
                        db.AddInParameter(dbCommandWrapper, "@DestroyPolicyID", SqlDbType.NVarChar, model.DestroyPolicyID ?? "");
                        db.AddInParameter(dbCommandWrapper, "@PolicyFor", SqlDbType.NVarChar, model.PolicyFor);
                        db.AddInParameter(dbCommandWrapper, "@DocumentNature", SqlDbType.NVarChar, model.DocumentNature);
                        db.AddInParameter(dbCommandWrapper, "@PolicyApplicableTo", SqlDbType.NVarChar, model.PolicyApplicableTo);
                        db.AddInParameter(dbCommandWrapper, "@Status", SqlDbType.NVarChar, model.Status);
                        db.AddInParameter(dbCommandWrapper, "@OwnerID", SqlDbType.NVarChar, model.OwnerID);
                        db.AddInParameter(dbCommandWrapper, "@UserID", SqlDbType.NVarChar, model.SetBy);
                        db.AddInParameter(dbCommandWrapper, "@DocCategoryID", SqlDbType.NVarChar, model.DocCategoryID);
                        db.AddInParameter(dbCommandWrapper, "@DocTypeID", SqlDbType.NVarChar, model.DocTypeID);
                        db.AddInParameter(dbCommandWrapper, "@DocPropertyID", SqlDbType.NVarChar, model.DocPropertyID);

                        db.AddInParameter(dbCommandWrapper, "@Doc_PropIdentity", SqlDbType.Structured, docPropIdentifyDataTable);
                        db.AddOutParameter(dbCommandWrapper, spStatusParam, SqlDbType.VarChar, 10);

                        db.ExecuteNonQuery(dbCommandWrapper);

                        if (!db.GetParameterValue(dbCommandWrapper, spStatusParam).IsNullOrZero())
                        {
                            errorNumber = db.GetParameterValue(dbCommandWrapper, spStatusParam).PrefixErrorCode();
                        }
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
