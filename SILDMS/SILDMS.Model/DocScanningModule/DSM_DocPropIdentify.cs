using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SILDMS.Model.DocScanningModule
{
    public class DSM_DocPropIdentify
    {
        public string DocPropIdentifyID { get; set; }

        public string OwnerID { get; set; }

        public string DocCategoryID { get; set; }

        public string DocTypeID { get; set; }

        public string DocPropertyID { get; set; }
        public string DocPropertyName { get; set; }
       
        public string IdentificationCode { get; set; }
        public string IdentificationSL { get; set; }
        public string AttributeGroup { get; set; }

        [Required]
        public string IdentificationAttribute { get; set; }

        [Required]
        public int? IsRequired { get; set; }
        public int? IsAuto { get; set; }
        public int? IsRestricted { get; set; }
        public string Remarks { get; set; }
        public string SetOn { get; set; }
        public string SetBy { get; set; }
        public string ModifiedOn { get; set; }
        public string ModifiedBy { get; set; }
        [Required]
        public int Status { get; set; }
        public string MetaValue { get; set; }
        public string FileServerUrl { get; set; }
        public string DocumentID { get; set; }
        public string DocMetaID { get; set; }

        public string ServerIP { get; set; }
        public string ServerPort { get; set; }
        public string FtpUserName { get; set; }
        public string FtpPassword { get; set; }
        public string DocVersionID { get; set; }
        public string VersionNo { get; set; }

        public string DocMetaIDVersion { get; set; }
        public ddlDSMOwnerLevel OwnerLevel { get; set; }
        public ddlDSMOwner Owner { get; set; }
        public ddlDSMDocCategory DocCategory { get; set; }
        public ddlDSMDocType DocType { get; set; }
        public ddlDSMDocProperty DocProperty { get; set; }
    }

    public class ddlDSMOwnerLevel
    {
        [Required]
        public string OwnerLevelID { get; set; }
        public string LevelName { get; set; }
    }

    public class ddlDSMOwner
    {
        [Required]
        public string OwnerID { get; set; }
        public string OwnerName { get; set; }
    }

    public class ddlDSMDocCategory
    {
        [Required]
        public string DocCategoryID { get; set; }
        public string DocCategoryName { get; set; }
    }

    public class ddlDSMDocType
    {
        [Required]
        public string DocTypeID { get; set; }
        public string DocTypeName { get; set; }
    }

    public class ddlDSMDocProperty
    {
        [Required]
        public string DocPropertyID { get; set; }
        public string DocPropertyName { get; set; }
        public bool IsSelected { get; set; }
    }

    public class ddlRole
    {
        public string RoleID { get; set; }
        public string RoleTitle { get; set; }
    }

    public class DocMetaValue
    {
        public string DocPropertyID { get; set; }
        public string MetaValue { get; set; }
        public string VersionMetaValue { get; set; }
        public string Remarks { get; set; }
        public string DocPropIdentifyID { get; set; }
        public string DocumentID { get; set; }
        public string DocVersionID { get; set; }
        public string DocMetaID { get; set; }
        public string IdentificationAttribute { get; set; }
    }

    public class DocumentsInfo
    {
        public ddlDSMOwnerLevel OwnerLevel { get; set; }
        public ddlDSMOwner Owner { get; set; }
        public ddlDSMDocCategory DocCategory { get; set; }
        public ddlDSMDocType DocType { get; set; }
        public ddlDSMDocProperty DocProperty { get; set; }
        public string DidtributionOf { get; set; }
        public string Remarks { get; set; }
        public string DocVersionID { get; set; }
        public string SetBy { get; set; }
        public string ModifiedBy { get; set; }


        public string OwnerLevelID { get; set; }
        public string OwnerID { get; set; }
        public string DocPropertyID { get; set; }
        public string DocTypeID { get; set; }
        public string DocCategoryID { get; set; }
        public string DocumentID { get; set; }

        public string UploaderIP { get; set; }


        public IList<DocMetaValue> DocMetaValues { get; set; }
    }
}
