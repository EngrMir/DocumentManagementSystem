using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SILDMS.Model.DocScanningModule
{
    public class DSM_DocProperty
    {

        public string DocPropertyID { get; set; }
        [Required]
        public string DocCategoryID { get; set; }
        [Required]
        public string OwnerLevelID { get; set; }
        [Required]
        public string OwnerID { get; set; }
        [Required]
        public string DocTypeID { get; set; }
        public string DocPropertySL { get; set; }
        public string UDDocPropertyCode { get; set; }
        [Required]
        public string DocPropertyName { get; set; }

        public string DocClassification { get; set; }
        public string PreservationPolicy { get; set; }
        public string PhysicalLocation { get; set; }
        public string Remarks { get; set; }
        public int? SerialNo { get; set; }
        
        public string SetOn { get; set; }
        public string SetBy { get; set; }
        public string ModifiedOn { get; set; }
        public string ModifiedBy { get; set; }
        [Required]
        public int? Status { get; set; }

    }
}
