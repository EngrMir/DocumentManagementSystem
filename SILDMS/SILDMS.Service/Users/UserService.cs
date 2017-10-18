using SILDMS.DataAccessInterface.Users;
using SILDMS.Model.SecurityModule;
using SILDMS.Utillity;
using SILDMS.Utillity.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using User = SILDMS.Model.Apps_User;

namespace SILDMS.Service.Users
{
    public partial class UserService : IUserService
    {
        #region Fields
        private readonly ILocalizationService localizationService;
        private readonly IUserDataService userDataService;
        private string errorNumber = "";
        #endregion

        #region Constractor
        public UserService(
             IUserDataService repository,
             ILocalizationService localizationService
            )
        {
            this.userDataService = repository;
            this.localizationService = localizationService;
        }
       
        #endregion

        #region Methods
        public ValidationResult GetAllUser(string id,string ownerID, out List<SEC_User> userList)
        {
            userList = userDataService.GetAllUser(id, ownerID, out errorNumber);
            if (errorNumber.Length > 0)
            {
                return new ValidationResult(errorNumber, localizationService.GetResource(errorNumber));
            }
            return ValidationResult.Success;
        }
 
        #endregion



        //public ValidationResult AddUser(SEC_User objUser, string action, out string status)
        //{
        //    userDataService.AddUser(objUser, action, out status);
        //    if (status.Length > 0)
        //    {
        //        return new ValidationResult(status, localizationService.GetResource(status));
        //    }
        //    return ValidationResult.Success;
        //}

        ValidationResult IUserService.AddUser(SEC_User objUser, string action, out string status)
        {
            userDataService.AddUser(objUser, action, out status);
            if (status.Length > 0)
            {
                return new ValidationResult(status, localizationService.GetResource(status));
            }
            return ValidationResult.Success;
        }


        public bool IsValidUser(string user, string password,string ip, out List<GetUserAccessPermission_Result> accessList)
        {
           return userDataService.IsValidUser(user,  password, ip, out accessList);
        }
    }
}
