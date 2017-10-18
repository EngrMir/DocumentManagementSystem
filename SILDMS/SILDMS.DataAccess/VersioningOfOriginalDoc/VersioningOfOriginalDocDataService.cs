using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using SILDMS.DataAccessInterface.VersioningOfOriginalDoc;
using SILDMS.Model.DocScanningModule;

namespace SILDMS.DataAccess.VersioningOfOriginalDoc
{
    public class VersioningOfOriginalDocDataService:IVersioningOfOriginalDocDataService
    {
        private readonly string spStatusParam = "@p_Status";
        public DSM_DocPropIdentify AddVersionDocumentInfo(DocumentsInfo _modelDocumentsInfo,
           string _action, out string _errorNumber)
        {
            DataTable docMetaDataTable = new DataTable();
            docMetaDataTable.Columns.Add("DocPropertyID");
            docMetaDataTable.Columns.Add("MetaValue");
            docMetaDataTable.Columns.Add("Remarks");
            docMetaDataTable.Columns.Add("DocPropIdentifyID");
            docMetaDataTable.Columns.Add("DocMetaID");


            foreach (var item in _modelDocumentsInfo.DocMetaValues)
            {
                if (item.VersionMetaValue != null)
                {
                    DataRow objDataRow = docMetaDataTable.NewRow();

                    objDataRow[0] = _modelDocumentsInfo.DocPropertyID;
                    objDataRow[1] = item.VersionMetaValue;
                    objDataRow[2] = item.Remarks;
                    objDataRow[3] = item.DocPropIdentifyID;
                    objDataRow[4] = item.DocMetaID;
                    docMetaDataTable.Rows.Add(objDataRow);
                }
                
            }

            DataTable docPropertyIDDataTable = new DataTable();
            docPropertyIDDataTable.Columns.Add("DocPropertyID");

            
            DataRow objDataRow2 = docPropertyIDDataTable.NewRow();
            objDataRow2[0] = _modelDocumentsInfo.DocPropertyID;

            docPropertyIDDataTable.Rows.Add(objDataRow2);
            

            DSM_DocPropIdentify docPropIdentifyList = new DSM_DocPropIdentify();
            _errorNumber = String.Empty;
            DatabaseProviderFactory factory = new DatabaseProviderFactory();
            SqlDatabase db = factory.CreateDefault() as SqlDatabase;
            using (DbCommand dbCommandWrapper = db.GetStoredProcCommand("SetVersionDocumentsInfo"))
            {
                db.AddInParameter(dbCommandWrapper, "@OwnerLevelID", SqlDbType.NVarChar, _modelDocumentsInfo.OwnerLevelID);
                db.AddInParameter(dbCommandWrapper, "@OwnerID", SqlDbType.NVarChar, _modelDocumentsInfo.OwnerID);
                db.AddInParameter(dbCommandWrapper, "@DocCategoryID ", SqlDbType.NVarChar, _modelDocumentsInfo.DocCategoryID);
                db.AddInParameter(dbCommandWrapper, "@DocTypeID", SqlDbType.NVarChar, _modelDocumentsInfo.DocTypeID);
                db.AddInParameter(dbCommandWrapper, "@DocumentID", SqlDbType.NVarChar, _modelDocumentsInfo.DocumentID);
                db.AddInParameter(dbCommandWrapper, "@VersionID", SqlDbType.NVarChar, _modelDocumentsInfo.DocVersionID);

                db.AddInParameter(dbCommandWrapper, "@FileOriginalName", SqlDbType.NVarChar, "");
                db.AddInParameter(dbCommandWrapper, "@FileCodeName", SqlDbType.NVarChar, "");
                db.AddInParameter(dbCommandWrapper, "@FileExtension", SqlDbType.NVarChar, "");
                db.AddInParameter(dbCommandWrapper, "@UploaderIP", SqlDbType.NVarChar, _modelDocumentsInfo.UploaderIP);

                db.AddInParameter(dbCommandWrapper, "@SetBy ", SqlDbType.NVarChar, _modelDocumentsInfo.SetBy);
                db.AddInParameter(dbCommandWrapper, "@ModifiedBy", SqlDbType.NVarChar, _modelDocumentsInfo.ModifiedBy);
                db.AddInParameter(dbCommandWrapper, "@Status", SqlDbType.Int, 1);
                db.AddInParameter(dbCommandWrapper, "@Doc_MetaTypeVersion", SqlDbType.Structured, docMetaDataTable);
                db.AddInParameter(dbCommandWrapper, "@Doc_PropertyType", SqlDbType.Structured, docPropertyIDDataTable);
                db.AddOutParameter(dbCommandWrapper, spStatusParam, SqlDbType.VarChar, 10);
                
                
                DataSet ds = db.ExecuteDataSet(dbCommandWrapper);
                if (!db.GetParameterValue(dbCommandWrapper, spStatusParam).IsNullOrZero())
                {
                    _errorNumber = db.GetParameterValue(dbCommandWrapper, spStatusParam).PrefixErrorCode();
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        DataTable dt1 = ds.Tables[0];
                        docPropIdentifyList = dt1.AsEnumerable().Select(reader => new DSM_DocPropIdentify
                        {
                            //DocMetaIDVersion = reader.GetString("DocMetaIDVersion"),
                            //DocMetaID = reader.GetString("DocMetaID"),
                            //DocumentID = reader.GetString("DocumentID"),
                            //DocPropIdentifyID = reader.GetString("DocPropIdentifyID"),
                            //DocPropertyID = reader.GetString("DocPropertyID"),
                            //DocCategoryID = reader.GetString("DocCategoryID"),
                            //DocTypeID = reader.GetString("DocTypeID"),
                            //OwnerID = reader.GetString("OwnerID"),
                            //IdentificationCode = reader.GetString("IdentificationCode"),
                            //DocPropertyName = reader.GetString("DocPropertyName"),
                            //AttributeGroup = reader.GetString("AttributeGroup"),
                            //IdentificationAttribute = reader.GetString("IdentificationAttribute"),
                            //MetaValue = reader.GetString("MetaValue"),
                            //Remarks = reader.GetString("Remarks"),
                            FileServerUrl = reader.GetString("FileServerUrl"),
                            ServerIP = reader.GetString("ServerIP"),
                            ServerPort = reader.GetString("ServerPort"),
                            FtpUserName = reader.GetString("FtpUserName"),
                            FtpPassword = reader.GetString("FtpPassword"),
                            DocVersionID = reader.GetString("DocVersionID"),
                            VersionNo = reader.GetString("VersionNo")
                        }).FirstOrDefault();
                    }
                }

            }

            return docPropIdentifyList;
        }


        public string DeleteVersionDocumentInfo(string _DocumentIDs, out string _errorNumber)
        {
            DataTable docPropertyIDDataTable = new DataTable();
            docPropertyIDDataTable.Columns.Add("DocPropertyID");

            string[] docPropIDs = _DocumentIDs.Split(',');
            foreach (var item in docPropIDs)
            {
                DataRow objDataRow = docPropertyIDDataTable.NewRow();
                objDataRow[0] = item;

                docPropertyIDDataTable.Rows.Add(objDataRow);
            }

            _errorNumber = String.Empty;
            DatabaseProviderFactory factory = new DatabaseProviderFactory();
            SqlDatabase db = factory.CreateDefault() as SqlDatabase;

            using (DbCommand dbCommandWrapper = db.GetStoredProcCommand("DeleteVersionDocumentInfo"))
            {
                db.AddInParameter(dbCommandWrapper, "@Doc_PropertyType", SqlDbType.Structured, docPropertyIDDataTable);
                db.AddOutParameter(dbCommandWrapper, spStatusParam, SqlDbType.VarChar, 10);

                DataSet ds = db.ExecuteDataSet(dbCommandWrapper);

                if (!db.GetParameterValue(dbCommandWrapper, spStatusParam).IsNullOrZero())
                {
                    _errorNumber = db.GetParameterValue(dbCommandWrapper, spStatusParam).PrefixErrorCode();
                }

            }

            return _errorNumber;
        }
    }
}
