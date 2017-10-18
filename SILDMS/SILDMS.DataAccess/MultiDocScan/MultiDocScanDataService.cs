using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using SILDMS.DataAccessInterface.MultiDocScan;
using SILDMS.Model.DocScanningModule;

namespace SILDMS.DataAccess.MultiDocScan
{
    public class MultiDocScanDataService : IMultiDocScanDataService
    {
        private readonly string spStatusParam = "@p_Status";
        
        public List<DSM_DocPropIdentify> AddDocumentInfo(DocumentsInfo _modelDocumentsInfo,
            string _selectedPropID, List<DocMetaValue> _docMetaValues,
            string _action, out string _errorNumber)
        {
            DataTable docMetaDataTable= new DataTable();
            docMetaDataTable.Columns.Add("DocPropertyID");
            docMetaDataTable.Columns.Add("MetaValue");
            docMetaDataTable.Columns.Add("Remarks");
            docMetaDataTable.Columns.Add("DocPropIdentifyID");

            foreach (var item in _docMetaValues)
            {
                DataRow objDataRow = docMetaDataTable.NewRow();

                objDataRow[0] = item.DocPropertyID;
                objDataRow[1] = item.MetaValue;
                objDataRow[2] = item.Remarks;
                objDataRow[3] = item.DocPropIdentifyID;
                docMetaDataTable.Rows.Add(objDataRow);
            }

            DataTable docPropertyIDDataTable = new DataTable();
            docPropertyIDDataTable.Columns.Add("DocPropertyID");

            string[] docPropIDs = _selectedPropID.Split(',');
            foreach (var item in docPropIDs)
            {
                DataRow objDataRow = docPropertyIDDataTable.NewRow();
                objDataRow[0] = item;

                docPropertyIDDataTable.Rows.Add(objDataRow);
            }

            List<DSM_DocPropIdentify> docPropIdentifyList = new List<DSM_DocPropIdentify>();
            _errorNumber = String.Empty;
            DatabaseProviderFactory factory = new DatabaseProviderFactory();
            SqlDatabase db = factory.CreateDefault() as SqlDatabase;
            using (DbCommand dbCommandWrapper = db.GetStoredProcCommand("SetDocumentsInfo"))
            {
                db.AddInParameter(dbCommandWrapper, "@OwnerLevelID", SqlDbType.NVarChar, _modelDocumentsInfo.OwnerLevel.OwnerLevelID);
                db.AddInParameter(dbCommandWrapper, "@OwnerID", SqlDbType.NVarChar, _modelDocumentsInfo.Owner.OwnerID);
                db.AddInParameter(dbCommandWrapper, "@DocCategoryID ", SqlDbType.NVarChar, _modelDocumentsInfo.DocCategory.DocCategoryID);
                db.AddInParameter(dbCommandWrapper, "@DocTypeID", SqlDbType.NVarChar, _modelDocumentsInfo.DocType.DocTypeID);
                db.AddInParameter(dbCommandWrapper, "@FileOriginalName", SqlDbType.NVarChar, "");
                db.AddInParameter(dbCommandWrapper, "@FileCodeName", SqlDbType.NVarChar, "");
                db.AddInParameter(dbCommandWrapper, "@FileExtension", SqlDbType.NVarChar, "");
                db.AddInParameter(dbCommandWrapper, "@UploaderIP", SqlDbType.NVarChar, _modelDocumentsInfo.UploaderIP);
                db.AddInParameter(dbCommandWrapper, "@SetBy ", SqlDbType.NVarChar, _modelDocumentsInfo.SetBy);
                db.AddInParameter(dbCommandWrapper, "@ModifiedBy", SqlDbType.NVarChar, _modelDocumentsInfo.ModifiedBy);
                db.AddInParameter(dbCommandWrapper, "@Status", SqlDbType.Int, 1);
                db.AddInParameter(dbCommandWrapper, "@Doc_MetaType", SqlDbType.Structured, docMetaDataTable);
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
                            DocMetaID = reader.GetString("DocMetaID"),
                            DocumentID = reader.GetString("DocumentID"),
                            DocPropIdentifyID = reader.GetString("DocPropIdentifyID"),
                            DocPropertyID = reader.GetString("DocPropertyID"),
                            DocCategoryID = reader.GetString("DocCategoryID"),
                            DocTypeID = reader.GetString("DocTypeID"),
                            OwnerID = reader.GetString("OwnerID"),
                            IdentificationCode = reader.GetString("IdentificationCode"),
                            DocPropertyName = reader.GetString("DocPropertyName"),
                            AttributeGroup = reader.GetString("AttributeGroup"),
                            IdentificationAttribute = reader.GetString("IdentificationAttribute"),
                            MetaValue = reader.GetString("MetaValue"),
                            Remarks = reader.GetString("Remarks"),
                            FileServerUrl = reader.GetString("FileServerUrl"),
                            ServerIP = reader.GetString("ServerIP"),
                            ServerPort = reader.GetString("ServerPort"),
                            FtpUserName = reader.GetString("FtpUserName"),
                            FtpPassword = reader.GetString("FtpPassword")
                        }).ToList();
                    }
                }
                
            }

            return docPropIdentifyList;
        }

        public List<DSM_Documents> GetAllDocumentsInfo(string _UserID, string _DocumentID, out string errorNumber)
        {
            errorNumber = string.Empty;
            List<DSM_Documents> dsmDocumentses = new List<DSM_Documents>();

            DatabaseProviderFactory factory = new DatabaseProviderFactory();
            SqlDatabase db = factory.CreateDefault() as SqlDatabase;
            using (DbCommand dbCommandWrapper = db.GetStoredProcCommand("GetDocumentsInfo"))
            {
                db.AddInParameter(dbCommandWrapper, "@UserID", SqlDbType.VarChar, _UserID);
                db.AddInParameter(dbCommandWrapper, "@DocumentID", SqlDbType.VarChar, _DocumentID);
                db.AddOutParameter(dbCommandWrapper, spStatusParam, DbType.String, 10);
                DataSet ds = db.ExecuteDataSet(dbCommandWrapper);

                if (!db.GetParameterValue(dbCommandWrapper, spStatusParam).IsNullOrZero())
                {
                    errorNumber = db.GetParameterValue(dbCommandWrapper, spStatusParam).PrefixErrorCode();
                }
                else
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        DataTable dt1 = ds.Tables[0];
                        dsmDocumentses = dt1.AsEnumerable().Select(reader => new DSM_Documents
                        {
                            DocumentID = reader.GetString("DocumentID"),
                            DocPropertyID = reader.GetString("DocPropertyID"),
                            DocCategoryID = reader.GetString("DocCategoryID"),
                            DocTypeID = reader.GetString("DocTypeID"),
                            OwnerID = reader.GetString("OwnerID"),
                            FileServerURL = reader.GetString("FileServerURL"),
                            ServerID = reader.GetString("ServerID")
                        }).ToList();
                    }
                }
            }
            return dsmDocumentses;
        }

        public List<DSM_DocumentsMeta> GetAllDocumentsMetaInfo(string _UserID, string _DocMetaID, out string errorNumber)
        {
            errorNumber = string.Empty;
            List<DSM_DocumentsMeta> dsmDocumentsMetas = new List<DSM_DocumentsMeta>();

            DatabaseProviderFactory factory = new DatabaseProviderFactory();
            SqlDatabase db = factory.CreateDefault() as SqlDatabase;
            using (DbCommand dbCommandWrapper = db.GetStoredProcCommand("GetAllDocumentsMetaInfo"))
            {
                db.AddInParameter(dbCommandWrapper, "@UserID", SqlDbType.VarChar, _UserID);
                db.AddInParameter(dbCommandWrapper, "@DocMetaID", SqlDbType.VarChar, _DocMetaID);
                db.AddOutParameter(dbCommandWrapper, spStatusParam, DbType.String, 10);
                DataSet ds = db.ExecuteDataSet(dbCommandWrapper);

                if (!db.GetParameterValue(dbCommandWrapper, spStatusParam).IsNullOrZero())
                {
                    errorNumber = db.GetParameterValue(dbCommandWrapper, spStatusParam).PrefixErrorCode();
                }
                else
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        DataTable dt1 = ds.Tables[0];
                        dsmDocumentsMetas = dt1.AsEnumerable().Select(reader => new DSM_DocumentsMeta
                        {
                            DocumentID = reader.GetString("DocumentID"),
                            DocMetaID = reader.GetString("DocMetaID"),
                            DocPropIdentifyID = reader.GetString("DocPropIdentifyID"),
                            MetaValue = reader.GetString("MetaValue"),
                            FileType = reader.GetString("FileType"),
                            Remarks = reader.GetString("Remarks")
                        }).ToList();
                    }
                }
            }
            return dsmDocumentsMetas;
        }


        public string DeleteDocumentInfo(string _DocumentIDs, out string _errorNumber)
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

            using (DbCommand dbCommandWrapper = db.GetStoredProcCommand("DeleteDocumentInfo"))
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
