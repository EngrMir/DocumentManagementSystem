using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SILDMS.Model.DocScanningModule
{
    public class DSM_DefaultValue
    {
        public string DefaultValueID { get; set; }
	    public string DefaultValueGroupID { get; set; }
	    public string ConfigureColumnID { get; set; }
	    public string DefaultValue { get; set; }
	    public string UpdateAllowed { get; set; }
	    public int? UserLevel { get; set; }
	    public string Remarks { get; set; }
	    public string SetOn { get; set; }
	    public string SetBy { get; set; }
	    public string ModifiedOn { get; set; }
	    public string ModifiedBy { get; set; }
        public string Status { get; set; }
    }
}
