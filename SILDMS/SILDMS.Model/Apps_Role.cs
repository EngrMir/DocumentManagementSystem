using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SILDMS.Model
{
    public class Apps_Role
    {
        public int RoleID { get; set; }
        public string Name { get; set; }
        public bool? IsActivated { get; set; }
        public int? Status { get; set; }
    }
}
