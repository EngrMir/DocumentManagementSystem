using SILDMS.Model.SecurityModule;
using SILDMS.Service.Roles;
using SILDMS.Service.RoleSetup;
using SILDMS.Service.Server;
using SILDMS.Service.Users;
using SILDMS.Utillity;
using SILDMS.Utillity.Localization;
using SILDMS.Web.UI.Areas.SecurityModule.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.SessionState;

namespace SILDMS.Web.UI.Areas.SecurityModule.Controllers
{
  
    public class UserController : Controller
    {
        //
        // GET: /SecurityModule/User/
        readonly IUserService _userService;
        readonly IRoleSetupService _roleService;
        readonly IServerService _serverService;

        private readonly ILocalizationService _localizationService;
        private ValidationResult respStatus = new ValidationResult();
        private string outStatus = string.Empty;
        private string action = "";
        private readonly string UserID = "";
        public UserController(IUserService usrRepository, IRoleSetupService roleService, ILocalizationService localizationService, IServerService serverService)
        {
            this._userService = usrRepository;
            this._roleService = roleService;
            this._localizationService = localizationService;
            this._serverService = serverService;
            UserID = SILAuthorization.GetUserID();
        }
       
        [SILAuthorize]        
        public ActionResult Index()
        {          
            return View();
        }

       [Authorize]
        public ActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        [Authorize]
        [SILLogAttribute]
        public async Task<dynamic> ChangePassword(string password,string oldPassword)
        {
            SEC_User objUser = new SEC_User();         
            action = "changePassword";
            objUser.SetBy = UserID;
            objUser.UserID = UserID;
            objUser.ModifiedBy = UserID;
            objUser.UserName = StringEncription.Encrypt(oldPassword.Trim(), true); 
            objUser.UserPassword = StringEncription.Encrypt( password.Trim(), true);
            respStatus = await Task.Run(() => _userService.AddUser(objUser, action, out outStatus));            
            return Json(new { Message = respStatus.Message, respStatus }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [Authorize]        
        public async Task<dynamic> GetUser(string userId, string ownerID)
        {
            List<SEC_User> obUser = null;
           await Task.Run(() => _userService.GetAllUser(userId, ownerID, out obUser));
           var result = obUser.Select(x => new
            {
                UserID = x.UserID,
                UserPassword = StringEncription.Decrypt( x.UserPassword,true),
                OwnerLevelID = x.OwnerLevelID,
                LevelName = x.LevelName,
                OwnerID = x.OwnerID,
                OwnerName = x.OwnerName,
                RoleTitle = x.RoleTitle,
                RoleID = x.RoleID,
                EmployeeID = x.EmployeeID,
                UserFullName = x.UserFullName,
                UserDesignation = x.UserDesignation,
                JobLocation = x.JobLocation,
                UserNo = x.UserNo,
                UserName = x.UserName,
                PermissionLevel = x.PermissionLevel,
                AccessOwnerLevel = x.AccessOwnerLevel,
                AccessDataLevel = x.AccessDataLevel,
                DocClassification = x.DocClassification,
                SecurityStatus = x.SecurityStatus,
                DateLimit =string.Format(x.DateLimit,"dd/MM/yyyy"),
                DefaultServer = x.DefaultServer,
                IntMailAddress = x.IntMailAddress,
                IntmailStatus = x.IntmailStatus,
                ExtMailAddress = x.ExtMailAddress,
                ExtMailStatus = x.ExtMailStatus,
                UserPicture = x.UserPicture,
                UserLevelID = x.UserLevelID,
                Remarks = x.Remarks,
                ClassificationLevel = x.ClassificationLevel,
                SetOn = x.SetOn,
                SupervisorLevel = x.SupervisorLevel,
                MessageStatus = x.MessageStatus,
                ContactNo = x.ContactNo,              
                SupervisorLevelName = x.SupervisorLevelName,
                AccessDataLevelName = x.AccessDataLevelName,
                UserLevelName = x.UserLevelName,
                ClassificationLevelName = x.ClassificationLevelName,
                DocClassificationName = x.DocClassificationName,

                Status = x.Status
            });

           return Json(new { Msg = "", result }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [Authorize]
        [SILLogAttribute]
        public async Task<dynamic> AddUser(SEC_User objUser)
        {
            if (ModelState.IsValid)
            {
                action = "add";
                objUser.SetOn = UserID;
                objUser.ModifiedBy = objUser.SetBy;
                objUser.UserPassword = StringEncription.Encrypt(objUser.UserPassword, true);   
                respStatus = await Task.Run(() => _userService.AddUser(objUser, action, out outStatus));
                // Error handling.   
                return Json(new { Message = respStatus.Message, respStatus }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                respStatus = new ValidationResult("E404", _localizationService.GetResource("E404"));
            }
            return Json(new { Message = respStatus.Message, respStatus }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [Authorize]
        [SILLogAttribute]
        public async Task<dynamic> UpdateUser(SEC_User objUser)
        {
            if (ModelState.IsValid)
            {
                action = "edit";
                objUser.SetBy = UserID;
                objUser.ModifiedBy = objUser.SetBy;
                objUser.UserPassword = StringEncription.Encrypt(objUser.UserPassword,true);   
                respStatus = await Task.Run(() => _userService.AddUser(objUser, action, out outStatus));
                // Error handling.   
                return Json(new { Message = respStatus.Message, respStatus }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                respStatus = new ValidationResult("E404", _localizationService.GetResource("E404"));
            }
            return Json(new { Message = respStatus.Message, respStatus }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [Authorize]
        public async Task<dynamic> GetServers(string _OwnerID)
        {
            List<SEC_Server> obServer = null;
            var data =await Task.Run(()=>_serverService.GetServers("","", out obServer));
            var result = from t in obServer where t.OwnerID == _OwnerID
                    select new 
                    {
                        ServerName = t.ServerName.ToUpper(),
                        //ServerIP = "\\\\" + t.ServerIP + t.ServerLocation+"\\\\"
                        ServerID=t.ServerID
                    };
            return Json(new { Message = respStatus.Message, result }, JsonRequestBehavior.AllowGet);
        }
         [HttpPost]
        [Authorize]
        public async Task<dynamic> GetRole(string _OwnerID)
        {
            List<SEC_Role> rolesList = null;
            await Task.Run(() => _roleService.GetRole(SILAuthorization.GetUserID(), "", out rolesList));
            var result = (from r in rolesList
                          where r.OwnerID == _OwnerID && r.Status==1
                          select new SEC_Role
                          {
                              RoleID = r.RoleID,
                              RoleTitle = r.RoleTitle,                           
                          }).ToList();

            return Json(new { Msg = "", result }, JsonRequestBehavior.AllowGet);
        }

        //public FileResult Download()
        //{
        //    byte[] fileBytes = System.IO.File.ReadAllBytes(@"C:\test\temp.xlsx");
        //    string fileName = "temp.xlsx";
        //    return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
        //}
        //#region Actions

        ///// <summary>
        ///// Uploads the file.
        ///// </summary>
        ///// <returns></returns>
        //[HttpPost]
        //public virtual ActionResult UploadFile()
        //{
        //    HttpPostedFileBase myFile = Request.Files["MyFile"];
        //    bool isUploaded = false;
        //    string message = "File upload failed";

        //    if (myFile != null && myFile.ContentLength != 0)
        //    {
        //        string pathForSaving = Server.MapPath("~/Uploads");
        //        if (this.CreateFolderIfNeeded(pathForSaving))
        //        {
        //            try
        //            {
        //                myFile.SaveAs(Path.Combine(pathForSaving, myFile.FileName));
        //                isUploaded = true;
        //                message = "File uploaded successfully!";
        //            }
        //            catch (Exception ex)
        //            {
        //                message = string.Format("File upload failed: {0}", ex.Message);
        //            }
        //        }
        //    }
        //    return Json(new { isUploaded = isUploaded, message = message }, "text/html");
        //}

        //#endregion

        //#region Private Methods

        ///// <summary>
        ///// Creates the folder if needed.
        ///// </summary>
        ///// <param name="path">The path.</param>
        ///// <returns></returns>
        //private bool CreateFolderIfNeeded(string path)
        //{
        //    bool result = true;
        //    if (!Directory.Exists(path))
        //    {
        //        try
        //        {
        //            Directory.CreateDirectory(path);
        //        }
        //        catch (Exception)
        //        {
        //            /*TODO: You must process this exception.*/
        //            result = false;
        //        }
        //    }
        //    return result;
        //}

        //#endregion

	}
}