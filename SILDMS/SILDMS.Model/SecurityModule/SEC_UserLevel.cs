using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SILDMS.Model.SecurityModule
{
    public class SEC_UserLevel
    {
        #region Constructor

        public SEC_UserLevel() { }

        #endregion

        #region Fields

        public string ID { get; set; }
        public int UserLevel { get; set; }
        public string UserLevelName { get; set; }
        public string UserLevelSL { get; set; }
        public string UserLevelType { get; set; }
        public string SetOn { get; set; }
        public string SetBy { get; set; }
        public string ModifiedOn { get; set; }
        public string ModifiedBy { get; set; }
        public int? Status { get; set; }

        #endregion
    }
}
