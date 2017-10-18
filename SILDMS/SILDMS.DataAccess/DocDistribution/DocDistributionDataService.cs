using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using SILDMS.DataAccessInterface.DocDistribution;
using SILDMS.Model.DocScanningModule;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SILDMS.DataAccess.DocDistribution
{
    public class DocDistributionDataService : IDocDistributionDataService
    {
        private readonly string spStatusParam = "@p_Status";
        public string AddDocumentInfo(DocumentsInfo _modelDocumentsInfo, string _selectedPropID, List<DocMetaValue> _docMetaValues, string _action, out string _errorNumber)
        {
            DataTable docMetaDataTable = new DataTable();
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

          
            _errorNumber = String.Empty;
            DatabaseProviderFactory factory = new DatabaseProviderFactory();
            SqlDatabase db = factory.CreateDefault() as SqlDatabase;
            using (DbCommand dbCommandWrapper = db.GetStoredProcCommand("SetDocumentDistributionInfo"))
            {

                db.AddInParameter(dbCommandWrapper, "@Doc_MetaType", SqlDbType.Structured, docMetaDataTable);
                db.AddInParameter(dbCommandWrapper, "@DocumentID", SqlDbType.NVarChar, _docMetaValues[0].DocumentID);
                //db.AddInParameter(dbCommandWrapper, "@OwnerLevelID", SqlDbType.NVarChar, _modelDocumentsInfo.OwnerLevel.OwnerLevelID);
                db.AddInParameter(dbCommandWrapper, "@OwnerID", SqlDbType.NVarChar, _modelDocumentsInfo.Owner.OwnerID);
                db.AddInParameter(dbCommandWrapper, "@DocCategoryID ", SqlDbType.NVarChar, _modelDocumentsInfo.DocCategory.DocCategoryID);
                db.AddInParameter(dbCommandWrapper, "@DocTypeID", SqlDbType.NVarChar, _modelDocumentsInfo.DocType.DocTypeID);
                db.AddInParameter(dbCommandWrapper, "@DocPropertyID", SqlDbType.NVarChar, _selectedPropID);
                db.AddInParameter(dbCommandWrapper, "@Remarks", SqlDbType.NVarChar, _docMetaValues[0].Remarks == null ? "" : _docMetaValues[0].Remarks.Trim());
                db.AddInParameter(dbCommandWrapper, "@SetBy ", SqlDbType.NVarChar, _modelDocumentsInfo.SetBy);
                db.AddInParameter(dbCommandWrapper, "@ModifiedBy", SqlDbType.NVarChar, _modelDocumentsInfo.ModifiedBy);
                db.AddInParameter(dbCommandWrapper, "@Status", SqlDbType.Int, 1);
                db.AddInParameter(dbCommandWrapper, "@DidtributionOf", SqlDbType.NVarChar, "");
                //db.AddInParameter(dbCommandWrapper, "@Doc_PropertyType", SqlDbType.Structured, docPropertyIDDataTable);
                db.AddOutParameter(dbCommandWrapper, spStatusParam, SqlDbType.VarChar, 10);

                DataSet ds = db.ExecuteDataSet(dbCommandWrapper);

                if (!db.GetParameterValue(dbCommandWrapper, spStatusParam).IsNullOrZero())
                {
                    _errorNumber = db.GetParameterValue(dbCommandWrapper, spStatusParam).PrefixErrorCode();
                  
                }

            }

            return _errorNumber;
        }



        public string AddDocumentInfoForVersion(DocumentsInfo _modelDocumentsInfo, string _selectedPropID, List<DocMetaValue> _docMetaValues, string action, out string _errorNumber)
        {
            DataTable docMetaDataTable = new DataTable();
            docMetaDataTable.Columns.Add("DocPropertyID");
            docMetaDataTable.Columns.Add("MetaValue");
            docMetaDataTable.Columns.Add("Remarks");
            docMetaDataTable.Columns.Add("DocPropIdentifyID");
            docMetaDataTable.Columns.Add("DocMetaID");

            foreach (var item in _docMetaValues)
            {
                DataRow objDataRow = docMetaDataTable.NewRow();

                objDataRow[0] = item.DocPropertyID;
                objDataRow[1] = item.MetaValue;
                objDataRow[2] = item.Remarks;
                objDataRow[3] = item.DocPropIdentifyID;
                objDataRow[4] = item.DocMetaID;
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


            _errorNumber = String.Empty;
            DatabaseProviderFactory factory = new DatabaseProviderFactory();
            SqlDatabase db = factory.CreateDefault() as SqlDatabase;
            using (DbCommand dbCommandWrapper = db.GetStoredProcCommand("SetDocumentDistributionForVersion"))
            {

                db.AddInParameter(dbCommandWrapper, "@Doc_MetaTypeForVersion", SqlDbType.Structured, docMetaDataTable);
                db.AddInParameter(dbCommandWrapper, "@DocumentID", SqlDbType.NVarChar, _docMetaValues[0].DocumentID);
                db.AddInParameter(dbCommandWrapper, "@DocVersionID", SqlDbType.NVarChar, _docMetaValues[0].DocVersionID);
                
                //db.AddInParameter(dbCommandWrapper, "@OwnerLevelID", SqlDbType.NVarChar, _modelDocumentsInfo.OwnerLevel.OwnerLevelID);
                db.AddInParameter(dbCommandWrapper, "@OwnerID", SqlDbType.NVarChar, _modelDocumentsInfo.Owner.OwnerID);
                db.AddInParameter(dbCommandWrapper, "@DocCategoryID ", SqlDbType.NVarChar, _modelDocumentsInfo.DocCategory.DocCategoryID);
                db.AddInParameter(dbCommandWrapper, "@DocTypeID", SqlDbType.NVarChar, _modelDocumentsInfo.DocType.DocTypeID);
                db.AddInParameter(dbCommandWrapper, "@DocPropertyID", SqlDbType.NVarChar, _selectedPropID);
                db.AddInParameter(dbCommandWrapper, "@Remarks", SqlDbType.NVarChar, _docMetaValues[0].Remarks == null ? "" : _docMetaValues[0].Remarks.Trim());
                db.AddInParameter(dbCommandWrapper, "@SetBy ", SqlDbType.NVarChar, _modelDocumentsInfo.SetBy);
                db.AddInParameter(dbCommandWrapper, "@ModifiedBy", SqlDbType.NVarChar, _modelDocumentsInfo.ModifiedBy);
                db.AddInParameter(dbCommandWrapper, "@Status", SqlDbType.Int, 1);
                db.AddInParameter(dbCommandWrapper, "@DidtributionOf", SqlDbType.NVarChar, _modelDocumentsInfo.DidtributionOf);
                //db.AddInParameter(dbCommandWrapper, "@Doc_PropertyType", SqlDbType.Structured, docPropertyIDDataTable);
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
