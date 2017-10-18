using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SILDMS.Model.DocScanningModule
{
    public class DSM_DefaultValueSetup
    {
        public string DefaultValueSetupID { get; set; }
	    public string ConfigureID { get; set; }
	    public string OwnerID  { get; set; }
	    public string DocCategoryID { get; set; }
	    public string DocTypeID { get; set; }
	    public string DocPropertyID { get; set; }
	    public string DocPropIdentifyID { get; set; }
	    public int? UserLevel { get; set; }
	    public string Remarks { get; set; }
    	public string SetOn { get; set; } 
	    public string SetBy { get; set; }
	    public string ModifiedOn { get; set; }
	    public string ModifiedBy { get; set; }
        public int Status { get; set; }
    }
}
