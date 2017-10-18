using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SILDMS.Model.SecurityModule
{
    public class SEC_Server
    {
        #region Constructor

        public SEC_Server()
        {
        }

        #endregion

        #region Fields

        public string ServerID { get; set; }
        public string ServerIP { get; set; }
        public string LastReplacedIP { get; set; }
        public string ServerName { get; set; }
        public string ServerFor { get; set; }
        public string ServerType { get; set; }
        public string ServerLocation { get; set; }
        public string PurchaseDate { get; set; }
        public int? WarrantyPeriod { get; set; }
        public string ServerProcessor { get; set; }
        public string ServerRAM { get; set; }
        public string ServerHDD { get; set; }
        public string OwnerID { get; set; }
        public int? UserLevel { get; set; }
        public string SetOn { get; set; }
        public string SetBy { get; set; }
        public string ModifiedOn { get; set; }
        public string ModifiedBy { get; set; }
        public int? Status { get; set; }
        public string FtpPort { get; set; }
        public string FtpUserName { get; set; }
        public string FtpPassword { get; set; }

        #endregion

        #region Fields for View

        public string OwnerName { get; set; }
        public string OwnerLevelID { get; set; }
        public string LevelName { get; set; }

        #endregion
    }
}
