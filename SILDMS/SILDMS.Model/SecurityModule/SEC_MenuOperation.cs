using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SILDMS.Model.SecurityModule
{
    public class SEC_MenuOperation
    {
        #region Constructor

        public SEC_MenuOperation() { }

        #endregion

        #region Fields

        public string MenuOperationID { get; set; }
        public string MenuOperationTitle { get; set; }
        public bool DefaultValue { get; set; }
        public string MenuOperationSL { get; set; }
        public string OwnerID { get; set; }
        public int UserLevel { get; set; }
        public string SetOn { get; set; }
        public string SetBy { get; set; }
        public string ModifiedOn { get; set; }
        public string ModifiedBy { get; set; }
        public int Status { get; set; }

        #endregion
    }
}
