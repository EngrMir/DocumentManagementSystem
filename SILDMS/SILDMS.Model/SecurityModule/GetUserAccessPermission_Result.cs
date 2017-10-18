using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SILDMS.Model.SecurityModule
{
    public class GetUserAccessPermission_Result
    {
        public string OwnerLevelID { get; set; }
        public string LevelName { get; set; }
        public string OwnerID { get; set; }
        public string OwnerName { get; set; }
        public string RoleID { get; set; }
        public string RoleTitle { get; set; }
        public string UserID { get; set; }
        public string UserName { get; set; }
        public string DefaultServer { get; set; }
        public virtual ICollection<SEC_Menu> AccessMenu { get; set; }       

    }
}
