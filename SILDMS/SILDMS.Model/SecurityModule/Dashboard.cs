using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SILDMS.Model.SecurityModule
{
    public class Dashboard
    {
        #region Constructor
        public Dashboard()
        {

        }
        #endregion

        #region Fields

        public int TotalDocuments { get; set; }
        public int VersionOfVersioned { get; set; }
        public int OriginalDocuments { get; set; }
        public int VersionedDocuments { get; set; }
        public List<SEC_UserLog> UserLogs { get; set; }

        #endregion

    }
}
