using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SILDMS.Model.SecurityModule
{
    public class SEC_UserDataAccess
    {
        public string UserDataAccessID { get; set; }
        public string UserID { get; set; }

        public string OwnerID { get; set; }
        public string UserOwnerAccessID { get; set; }

        public string DocCategoryID { get; set; }
        public string DocTypeID { get; set; }
        public string DocPropertyID { get; set; }

        public string DocPropIdentifyID { get; set; }
        public string MetaValue { get; set; }

        public string DataAccessLevel { get; set; }
        public string DataAccessPower { get; set; }

        public string AccessTimeLimit { get; set; }
        public string Remarks { get; set; }

        public string SetOn { get; set; }
        public string SetBy { get; set; }
        public string ModifiedOn { get; set; }
        public string ModifiedBy { get; set; }
        
        public int Status { get; set; }
    }
}
