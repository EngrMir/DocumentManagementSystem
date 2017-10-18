using System;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;

namespace SILDMS.Model.DocScanningModule
{
    public class DSM_OwnerLevel
    {
        public string OwnerLevelID { get; set; }
        [Required]
        public string LevelName { get; set; }

        public string LevelAccess { get; set; }
        public string LevelSL { get; set; }
        public int? UserLevel { get; set; }
        public string SetOn { get; set; }
        public string SetBy { get; set; }
        public string ModifiedOn { get; set; }
        public string ModifiedBy { get; set; }
        [Required]
        public int? Status { get; set; }
 
    }
}
