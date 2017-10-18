using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using SILDMS.DataAccessInterface.DataLevelPermission;
using SILDMS.Model.SecurityModule;



namespace SILDMS.DataAccess.DataLevelPermission
{
    public class DataLevelPermissionDataService: IDataLevelPermissionDataService
    {
        private readonly string spStatusParam = "@p_Status";
        public List<SEC_UserDataAccess> GetAllUserDataAccess(string _UserID,
            out string _errorNumber)
        {
            _errorNumber = string.Empty;
            List<SEC_UserDataAccess> userDataAccesses = new List<SEC_UserDataAccess>();

            DatabaseProviderFactory factory = new DatabaseProviderFactory();
            SqlDatabase db = factory.CreateDefault() as SqlDatabase;
            using (DbCommand dbCommandWrapper = db.GetStoredProcCommand("GetAllUserDataAccess"))
            {
                db.AddInParameter(dbCommandWrapper, "@UserID", SqlDbType.VarChar, _UserID);
                db.AddOutParameter(dbCommandWrapper, spStatusParam, DbType.String, 10);
                DataSet ds = db.ExecuteDataSet(dbCommandWrapper);

                if (!db.GetParameterValue(dbCommandWrapper, spStatusParam).IsNullOrZero())
                {
                    _errorNumber = db.GetParameterValue(dbCommandWrapper, spStatusParam).PrefixErrorCode();
                }
                else
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        DataTable dt1 = ds.Tables[0];
                        userDataAccesses = dt1.AsEnumerable().Select(reader => new SEC_UserDataAccess
                        {
                            UserDataAccessID = reader.GetString("UserDataAccessID"),
                            UserID = reader.GetString("UserID"),
                            OwnerID = reader.GetString("OwnerID"),
                            UserOwnerAccessID = reader.GetString("UserOwnerAccessID"),
                            DocCategoryID = reader.GetString("DocCategoryID"),
                            DocTypeID = reader.GetString("DocTypeID"),
                            DocPropertyID = reader.GetString("DocPropertyID"),
                            DocPropIdentifyID = reader.GetString("DocPropIdentifyID"),
                            MetaValue = reader.GetString("MetaValue"),
                            DataAccessLevel = reader.GetString("DataAccessLevel"),
                            DataAccessPower = reader.GetString("DataAccessPower"),
                            AccessTimeLimit = reader.GetString("AccessTimeLimit"),
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
            return userDataAccesses;
        }

        public string SetDataLevelPermission(SEC_UserOwnerAccess model, string action, out string errorNumber)
        {
            errorNumber = String.Empty;
            try
            {
                IFormatProvider culture = new System.Globalization.CultureInfo("fr-FR", true);
                DatabaseProviderFactory factory = new DatabaseProviderFactory();
                SqlDatabase db = factory.CreateDefault() as SqlDatabase;

                DataTable docCategoryDataTable = new DataTable();
                docCategoryDataTable.Columns.Add("DocCategoryID");
                docCategoryDataTable.Columns.Add("AccessTimeLimit");
                docCategoryDataTable.Columns.Add("Remarks");

                DataTable docTypeDataTable = new DataTable();
                docTypeDataTable.Columns.Add("DocTypeID");
                docTypeDataTable.Columns.Add("AccessTimeLimit");
                docTypeDataTable.Columns.Add("Remarks");

                DataTable docPropertyDataTable = new DataTable();
                docPropertyDataTable.Columns.Add("DocPropertyID");
                docPropertyDataTable.Columns.Add("AccessTimeLimit");
                docPropertyDataTable.Columns.Add("Remarks");

                DataTable docPropIdentifyDataTable = new DataTable();
                docPropIdentifyDataTable.Columns.Add("DocPropIdentifyID");
                docPropIdentifyDataTable.Columns.Add("AccessTimeLimit");
                docPropIdentifyDataTable.Columns.Add("Remarks");
                docPropIdentifyDataTable.Columns.Add("MetaValue");

                if (model.DocCategoryModel != null)
                {
                    foreach (var item in model.DocCategoryModel)
                    {
                        DataRow objDataRow = docCategoryDataTable.NewRow();

                        objDataRow[0] = item.CategoryID;
                        if (item.CategoryTime != null)
                            objDataRow[1] =Convert.ToDateTime(item.CategoryTime).ToString("yyyy/MM/dd");//DateTime.Parse(, culture, System.Globalization.DateTimeStyles.AssumeLocal);
                        objDataRow[2] = item.CategoryRemarks;

                        docCategoryDataTable.Rows.Add(objDataRow);
                    }
                }
                else if (model.DocTypeModel != null)
                {

                    foreach (var item in model.DocTypeModel)
                    {
                        DataRow objDataRow = docTypeDataTable.NewRow();

                        objDataRow[0] = item.TypeID;
                        if (item.TypeTime != null)
                            objDataRow[1] = DateTime.Parse(item.TypeTime, culture,
                                System.Globalization.DateTimeStyles.AssumeLocal);
                        objDataRow[2] = item.TypeRemarks;

                        docTypeDataTable.Rows.Add(objDataRow);
                    }
                }
                else if (model.DocPropertyModel != null)
                {

                    foreach (var item in model.DocPropertyModel)
                    {
                        DataRow objDataRow = docPropertyDataTable.NewRow();

                        objDataRow[0] = item.PropertyID;
                        if (item.PropertyTime != null)
                            objDataRow[1] = DateTime.Parse(item.PropertyTime, culture,
                                System.Globalization.DateTimeStyles.AssumeLocal);
                        ;
                        objDataRow[2] = item.PropertyRemarks;

                        docPropertyDataTable.Rows.Add(objDataRow);
                    }
                }
                else if (model.DocPropIdentityModel != null)
                {
                    foreach (var item in model.DocPropIdentityModel)
                    {
                        DataRow objDataRow = docPropIdentifyDataTable.NewRow();

                        objDataRow[0] = item.PropIdentityID;
                        if (item.PropIdentityTime != null)
                        objDataRow[1] = DateTime.Parse(item.PropIdentityTime, culture, System.Globalization.DateTimeStyles.AssumeLocal); ;
                        objDataRow[2] = item.PropIdentityRemarks;
                        objDataRow[3] = item.PropIdentityMetaValue;

                        docPropIdentifyDataTable.Rows.Add(objDataRow);
                    }
                }



                if (model.DataLevelAccess == "DocumentCategory")
                {
                    using (DbCommand dbCommandWrapper = db.GetStoredProcCommand("SetDocCategoryToDataLevelPermission"))
                    {
                        db.AddInParameter(dbCommandWrapper, "@UserID", SqlDbType.NVarChar, model.UserID);
                        db.AddInParameter(dbCommandWrapper, "@OwnerID", SqlDbType.NVarChar, model.OwnerID);
                        db.AddInParameter(dbCommandWrapper, "@UserOwnerAccessID", SqlDbType.NVarChar, model.UserOwnerAccessID);
                        db.AddInParameter(dbCommandWrapper, "@DataAccessLevel", SqlDbType.NVarChar, model.DataLevelAccess);
                        db.AddInParameter(dbCommandWrapper, "@Doc_Category", SqlDbType.Structured, docCategoryDataTable);
                        db.AddOutParameter(dbCommandWrapper, spStatusParam, SqlDbType.VarChar, 10);
                        db.ExecuteNonQuery(dbCommandWrapper);

                        if (!db.GetParameterValue(dbCommandWrapper, spStatusParam).IsNullOrZero())
                        {
                            errorNumber = db.GetParameterValue(dbCommandWrapper, spStatusParam).PrefixErrorCode();
                        }
                    }
                }
                else if (model.DataLevelAccess == "DocumentType")
                {
                    using (DbCommand dbCommandWrapper = db.GetStoredProcCommand("SetDocTypeToDataLevelPermission"))
                    {
                        db.AddInParameter(dbCommandWrapper, "@UserID", SqlDbType.NVarChar, model.UserID);
                        db.AddInParameter(dbCommandWrapper, "@OwnerID", SqlDbType.NVarChar, model.OwnerID);
                        db.AddInParameter(dbCommandWrapper, "@UserOwnerAccessID", SqlDbType.NVarChar, model.UserOwnerAccessID);
                        db.AddInParameter(dbCommandWrapper, "@DataAccessLevel", SqlDbType.NVarChar, model.DataLevelAccess);
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
                else if (model.DataLevelAccess == "DocumentProperty")
                {
                    using (DbCommand dbCommandWrapper = db.GetStoredProcCommand("SetDocPropertyToDataLevelPermission"))
                    {
                        db.AddInParameter(dbCommandWrapper, "@UserID", SqlDbType.NVarChar, model.UserID);
                        db.AddInParameter(dbCommandWrapper, "@OwnerID", SqlDbType.NVarChar, model.OwnerID);
                        db.AddInParameter(dbCommandWrapper, "@UserOwnerAccessID", SqlDbType.NVarChar, model.UserOwnerAccessID);
                        db.AddInParameter(dbCommandWrapper, "@DataAccessLevel", SqlDbType.NVarChar, model.DataLevelAccess);
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
                else if (model.DataLevelAccess == "DocumentPropertyValue")
                {
                    using (DbCommand dbCommandWrapper = db.GetStoredProcCommand("SetDocProppIdentifyToDataLevelPermission"))
                    {
                        db.AddInParameter(dbCommandWrapper, "@UserID", SqlDbType.NVarChar, model.UserID);
                        db.AddInParameter(dbCommandWrapper, "@OwnerID", SqlDbType.NVarChar, model.OwnerID);
                        db.AddInParameter(dbCommandWrapper, "@UserOwnerAccessID", SqlDbType.NVarChar, model.UserOwnerAccessID);
                        db.AddInParameter(dbCommandWrapper, "@DataAccessLevel", SqlDbType.NVarChar, model.DataLevelAccess);
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

        //public string SetDataLevelPermission(SEC_UserOwnerAccess model, string action, out string errorNumber)
        //{
        //    errorNumber = String.Empty;
        //    try
        //    {
        //        IFormatProvider culture = new System.Globalization.CultureInfo("fr-FR", true);
        //        DatabaseProviderFactory factory = new DatabaseProviderFactory();
        //        SqlDatabase db = factory.CreateDefault() as SqlDatabase;

        //        if (model.DocCategoryModel != null)
        //        {
        //            DataTable docCategoryDataTable = new DataTable();
        //            docCategoryDataTable.Columns.Add("DocCategoryID");
        //            docCategoryDataTable.Columns.Add("AccessTimeLimit");
        //            docCategoryDataTable.Columns.Add("Remarks");

        //            foreach (var item in model.DocCategoryModel)
        //            {
        //                DataRow objDataRow = docCategoryDataTable.NewRow();

        //                objDataRow[0] = item.CategoryID;
        //                if(item.CategoryTime != null)
        //                objDataRow[1] = DateTime.Parse(item.CategoryTime, culture, System.Globalization.DateTimeStyles.AssumeLocal);
        //                objDataRow[2] = item.CategoryRemarks;

        //                docCategoryDataTable.Rows.Add(objDataRow);
        //            }

        //            using (DbCommand dbCommandWrapper = db.GetStoredProcCommand("SetDocCategoryToDataLevelPermission"))
        //            {
        //                db.AddInParameter(dbCommandWrapper, "@UserID", SqlDbType.NVarChar, model.UserID);
        //                db.AddInParameter(dbCommandWrapper, "@OwnerID", SqlDbType.NVarChar, model.OwnerID);
        //                db.AddInParameter(dbCommandWrapper, "@UserOwnerAccessID", SqlDbType.NVarChar, model.UserOwnerAccessID);
        //                db.AddInParameter(dbCommandWrapper, "@DataAccessLevel", SqlDbType.NVarChar, model.DataLevelAccess);
        //                db.AddInParameter(dbCommandWrapper, "@Doc_Category", SqlDbType.Structured, docCategoryDataTable);
        //                db.AddOutParameter(dbCommandWrapper, spStatusParam, SqlDbType.VarChar, 10);
        //                db.ExecuteNonQuery(dbCommandWrapper);
                        
        //                if (!db.GetParameterValue(dbCommandWrapper, spStatusParam).IsNullOrZero())
        //                {
        //                    errorNumber = db.GetParameterValue(dbCommandWrapper, spStatusParam).PrefixErrorCode();
        //                }
        //            }
        //        }



        //        else if (model.DocTypeModel != null)
        //        {
        //            DataTable docTypeDataTable = new DataTable();
        //            docTypeDataTable.Columns.Add("DocTypeID");
        //            docTypeDataTable.Columns.Add("AccessTimeLimit");
        //            docTypeDataTable.Columns.Add("Remarks");
        //            foreach (var item in model.DocTypeModel)
        //            {
        //                DataRow objDataRow = docTypeDataTable.NewRow();

        //                objDataRow[0] = item.TypeID;
        //                if (item.TypeTime != null)
        //                objDataRow[1] = DateTime.Parse(item.TypeTime, culture, System.Globalization.DateTimeStyles.AssumeLocal); 
        //                objDataRow[2] = item.TypeRemarks;

        //                docTypeDataTable.Rows.Add(objDataRow);
        //            }

        //            using (DbCommand dbCommandWrapper = db.GetStoredProcCommand("SetDocTypeToDataLevelPermission"))
        //            {
        //                db.AddInParameter(dbCommandWrapper, "@UserID", SqlDbType.NVarChar, model.UserID);
        //                db.AddInParameter(dbCommandWrapper, "@OwnerID", SqlDbType.NVarChar, model.OwnerID);
        //                db.AddInParameter(dbCommandWrapper, "@UserOwnerAccessID", SqlDbType.NVarChar, model.UserOwnerAccessID);
        //                db.AddInParameter(dbCommandWrapper, "@DataAccessLevel", SqlDbType.NVarChar, model.DataLevelAccess);
        //                db.AddInParameter(dbCommandWrapper, "@DocCategoryID", SqlDbType.NVarChar, model.DocCategoryID);

        //                db.AddInParameter(dbCommandWrapper, "@Doc_Type", SqlDbType.Structured, docTypeDataTable);
        //                db.AddOutParameter(dbCommandWrapper, spStatusParam, SqlDbType.VarChar, 10);
        //                db.ExecuteNonQuery(dbCommandWrapper);
                        
        //                if (!db.GetParameterValue(dbCommandWrapper, spStatusParam).IsNullOrZero())
        //                {
        //                    errorNumber = db.GetParameterValue(dbCommandWrapper, spStatusParam).PrefixErrorCode();
        //                }
        //            }
        //        }

        //        else if (model.DocPropertyModel != null)
        //        {
        //            DataTable docPropertyDataTable = new DataTable();
        //            docPropertyDataTable.Columns.Add("DocPropertyID");
        //            docPropertyDataTable.Columns.Add("AccessTimeLimit");
        //            docPropertyDataTable.Columns.Add("Remarks");
        //            foreach (var item in model.DocPropertyModel)
        //            {
        //                DataRow objDataRow = docPropertyDataTable.NewRow();

        //                objDataRow[0] = item.PropertyID;
        //                if (item.PropertyTime != null)
        //                objDataRow[1] = DateTime.Parse(item.PropertyTime, culture, System.Globalization.DateTimeStyles.AssumeLocal); ;
        //                objDataRow[2] = item.PropertyRemarks;

        //                docPropertyDataTable.Rows.Add(objDataRow);
        //            }

        //            using (DbCommand dbCommandWrapper = db.GetStoredProcCommand("SetDocPropertyToDataLevelPermission"))
        //            {
        //                db.AddInParameter(dbCommandWrapper, "@UserID", SqlDbType.NVarChar, model.UserID);
        //                db.AddInParameter(dbCommandWrapper, "@OwnerID", SqlDbType.NVarChar, model.OwnerID);
        //                db.AddInParameter(dbCommandWrapper, "@UserOwnerAccessID", SqlDbType.NVarChar, model.UserOwnerAccessID);
        //                db.AddInParameter(dbCommandWrapper, "@DataAccessLevel", SqlDbType.NVarChar, model.DataLevelAccess);
        //                db.AddInParameter(dbCommandWrapper, "@DocCategoryID", SqlDbType.NVarChar, model.DocCategoryID);
        //                db.AddInParameter(dbCommandWrapper, "@DocTypeID", SqlDbType.NVarChar, model.DocTypeID);
        //                db.AddInParameter(dbCommandWrapper, "@Doc_Property", SqlDbType.Structured, docPropertyDataTable);
        //                db.AddOutParameter(dbCommandWrapper, spStatusParam, SqlDbType.VarChar, 10);
                        
        //                db.ExecuteNonQuery(dbCommandWrapper);
                        
        //                if (!db.GetParameterValue(dbCommandWrapper, spStatusParam).IsNullOrZero())
        //                {
        //                    errorNumber = db.GetParameterValue(dbCommandWrapper, spStatusParam).PrefixErrorCode();
        //                }
        //            }
        //        }



        //        else if (model.DocPropIdentityModel != null)
        //        {
        //            DataTable docPropIdentifyDataTable = new DataTable();
        //            docPropIdentifyDataTable.Columns.Add("DocPropIdentifyID");
        //            docPropIdentifyDataTable.Columns.Add("AccessTimeLimit");
        //            docPropIdentifyDataTable.Columns.Add("Remarks");
        //            docPropIdentifyDataTable.Columns.Add("MetaValue");
        //            foreach (var item in model.DocPropIdentityModel)
        //            {
        //                DataRow objDataRow = docPropIdentifyDataTable.NewRow();

        //                objDataRow[0] = item.PropIdentityID;
        //                if (item.PropIdentityTime != null)
        //                objDataRow[1] = DateTime.Parse(item.PropIdentityTime, culture, System.Globalization.DateTimeStyles.AssumeLocal); ;
        //                objDataRow[2] = item.PropIdentityRemarks;
        //                objDataRow[3] = item.PropIdentityMetaValue;

        //                docPropIdentifyDataTable.Rows.Add(objDataRow);
        //            }

        //            using (DbCommand dbCommandWrapper = db.GetStoredProcCommand("SetDocProppIdentifyToDataLevelPermission"))
        //            {
        //                db.AddInParameter(dbCommandWrapper, "@UserID", SqlDbType.NVarChar, model.UserID);
        //                db.AddInParameter(dbCommandWrapper, "@OwnerID", SqlDbType.NVarChar, model.OwnerID);
        //                db.AddInParameter(dbCommandWrapper, "@UserOwnerAccessID", SqlDbType.NVarChar, model.UserOwnerAccessID);
        //                db.AddInParameter(dbCommandWrapper, "@DataAccessLevel", SqlDbType.NVarChar, model.DataLevelAccess);
        //                db.AddInParameter(dbCommandWrapper, "@DocCategoryID", SqlDbType.NVarChar, model.DocCategoryID);
        //                db.AddInParameter(dbCommandWrapper, "@DocTypeID", SqlDbType.NVarChar, model.DocTypeID);
        //                db.AddInParameter(dbCommandWrapper, "@DocPropertyID", SqlDbType.NVarChar, model.DocPropertyID);
                        

        //                db.AddInParameter(dbCommandWrapper, "@Doc_PropIdentity", SqlDbType.Structured, docPropIdentifyDataTable);
        //                db.AddOutParameter(dbCommandWrapper, spStatusParam, SqlDbType.VarChar, 10);
                       
        //                db.ExecuteNonQuery(dbCommandWrapper);
                        
        //                if (!db.GetParameterValue(dbCommandWrapper, spStatusParam).IsNullOrZero())
        //                {
        //                    errorNumber = db.GetParameterValue(dbCommandWrapper, spStatusParam).PrefixErrorCode();
        //                }
        //            }
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        errorNumber = "E404";             
        //    }
        //    return errorNumber;
        //}
    }
    }

    
