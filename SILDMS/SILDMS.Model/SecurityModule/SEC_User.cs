using SILDMS.Model.DocScanningModule;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SILDMS.Model.SecurityModule
{
    public class SEC_User
    {
        public string UserID { get; set; }
        [Required]
        public string OwnerLevelID { get; set; }
        public string UserPassword { get; set; }
        public string LevelName { get; set; }
        [Required]
        public string OwnerID { get; set; }
        public string OwnerName { get; set; }
        public string EmployeeID { get; set; }
        public string UserFullName { get; set; }
        public string UserDesignation { get; set; }
        public string JobLocation { get; set; }
        public string UserNo { get; set; }
          [Required]
        public string UserName { get; set; }
          [Required]
        public string RoleID { get; set; }
        public string RoleTitle { get; set; }
        public string PermissionLevel { get; set; }
        public string AccessOwnerLevel { get; set; }
        public string AccessDataLevel { get; set; }
        public string DocClassification { get; set; }
        public string SecurityStatus { get; set; }
        public string DateLimit { get; set; }
        public string DefaultServer { get; set; }
        public string IntMailAddress { get; set; }
        public string IntmailStatus { get; set; }
        public string ExtMailAddress { get; set; }
        public string ExtMailStatus { get; set; }
        public string UserPicture { get; set; }
        public string SetOn { get; set; }
        public string Remarks { get; set; }
        public string UserLevelID { get; set; }
        public string UserLevelName { get; set; }
        public string SupervisorLevel { get; set; }
        public string SupervisorLevelName { get; set; }
    
        public string Status { get; set; }
        public string ClassificationLevel { get; set; }
        public string ClassificationLevelName { get; set; }
        public string ModifiedOn { get; set; }
        public string SetBy { get; set; }
        public string ModifiedBy { get; set; }
        public string ContactNo { get; set; }
        public string MessageStatus { get; set; }

        public string AccessDataLevelName { get; set; }
        public string DocClassificationName { get; set; }
        public virtual SEC_Role SEC_Role { get; set; }
        public virtual DSM_Owner DSM_Owner { get; set; }
        public virtual DSM_OwnerLevel DSM_OwnerLevel { get; set; }
    }


}
