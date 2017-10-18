using SILDMS.Model.SecurityModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SILDMS.Model.SecurityModule
{
    public partial class Sec_Company
    {
        public Sec_Company()
        {
            this.Sec_Department = new HashSet<Sec_Department>();
        }
        public int CompanyId { get; set; }
        public string CompanyName { get; set; }
        public string CompanyDescription { get; set; }
        public string CreatedBy { get; set; }
        public System.DateTime CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public System.DateTime ModifiedOn { get; set; }
        public int CompanyStatus { get; set; }
        public virtual ICollection<Sec_Department> Sec_Department { get; set; }
    }
}
