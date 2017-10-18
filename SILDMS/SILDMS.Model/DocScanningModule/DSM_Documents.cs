using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SILDMS.Model.DocScanningModule
{
    public class DSM_DocumentsMeta
    {
        public string DocumentID { get; set; }
        public string DocMetaID { get; set; }
        public string FileType { get; set; }
        public string DocPropIdentifyID { get; set; }
        public string MetaValue { get; set; }
        public string Remarks { get; set; }
        public string SetBy { get; set; }
        public string ModifiedBy { get; set; }

    }

    public class DSM_Documents
    {
        public string DocumentID { get; set; }
        public string OwnerID { get; set; }
        public string DocCategoryID { get; set; }
        public string DocTypeID { get; set; }
        public string DocPropertyID { get; set; }
        public string FileServerURL { get; set; }
        public string VersionNo { get; set; }
        public string FileOriginalName { get; set; }
        public string ServerID { get; set; }
    }
}
