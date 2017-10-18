using SILDMS.Model.SecurityModule;
using SILDMS.Utillity;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.SessionState;
using SILDMS.Service.RoleMenuPermission;
using SILDMS.Service.RoleSetup;
using SILDMS.Service.Users;
using SILDMS.Utillity.Localization;
using SILDMS.Web.UI.Areas.SecurityModule.Models;

namespace SILDMS.Web.UI.Areas.SecurityModule.Controllers
{

    public class RoleMenuPermissionController : Controller
    {
        readonly IRoleSetupService _roleSetupService;
        private readonly IRoleMenuPermissionService _roleMenuPermissionService;
        private readonly IUserService _userService;

        private readonly ILocalizationService _localizationService;
        private ValidationResult respStatus = new ValidationResult();
        private string outStatus = string.Empty;        
        private readonly string UserID = string.Empty;

        public RoleMenuPermissionController(IRoleSetupService roleService,
            ILocalizationService localizationService,IRoleMenuPermissionService roleMenuPermissionService,
            IUserService userService)
        {
            _roleSetupService = roleService;
            
            _localizationService = localizationService;
            _roleMenuPermissionService = roleMenuPermissionService;
            UserID = SILAuthorization.GetUserID();
           _userService = userService;
        }
        [SILAuthorize]
        public ActionResult Index()
        {
            return View();
        }
        [Authorize]
        public async Task<dynamic> GetRole(string _OwnerID)
        {
            List<SEC_Role> rolesList = null;

            await Task.Run(() => _roleSetupService.GetRole(UserID, "",
                out rolesList));

            var result = (from r in rolesList
                          where r.OwnerID == _OwnerID && r.Status == 1

                          select new SEC_Role
                          {
                              RoleID = r.RoleID,
                              RoleTitle = r.RoleTitle,
                          }).ToList();

            return Json(new { Msg = "", result }, JsonRequestBehavior.AllowGet);
        }
        [Authorize]
        public async Task<dynamic> GetMenuTreeData( string _OwnerID)
        {
            List<SEC_User> users = null;
                      await Task.Run(() => _userService.GetAllUser(UserID, "", out users));

            var roleInfo = (from r in users
                where r.UserID == UserID
                select new
                {
                    RoleID = r.RoleID,
                    RoleTitle = r.RoleTitle
                }).FirstOrDefault();

            if (roleInfo.RoleTitle == "Super Admin")
            {
                List<Model.SecurityModule.Parent> parent = new List<Model.SecurityModule.Parent>();
                List<Child> child = new List<Child>();
                List<SEC_NavMenuOptSetup> resultNav = null;
                await Task.Run(() => _roleMenuPermissionService.GetOwnerPermittedMenu
                    (UserID, "", out resultNav));

                var root = (from t in resultNav where t.ParentMenuID == "0" &
                                t.Status == 1 select t).ToList();
                foreach (var item in root)
                {
                    Model.SecurityModule.Parent p = new Model.SecurityModule.Parent();
                    p.key = item.MenuID;
                    p.title = item.MenuTitle;
                    child = GetChild(resultNav, item.MenuID);
                    if (child.Count > 0)
                    {
                        p.children = child;
                    }
                    parent.Add(p);
                }

                return Json(parent, JsonRequestBehavior.AllowGet);
            }
            else
            {
                List<SEC_RoleMenuPermission> roleMenuPermissionList = null;
                await Task.Run(() => _roleMenuPermissionService.
                    GetRoleMenuPermission(UserID, "", out roleMenuPermissionList));

                List<SEC_NavMenuOptSetup> resultNav = null;
                await Task.Run(() => _roleMenuPermissionService.GetOwnerPermittedMenu
                    (UserID, "", out resultNav));


                var PermittedMenus = (from pm in roleMenuPermissionList
                    where pm.RoleID == roleInfo.RoleID & pm.Status == 1
                    join m in resultNav on pm.MenuID equals m.MenuID into permittedmenus
                    from r in permittedmenus.DefaultIfEmpty()
                    select new SEC_NavMenuOptSetup
                    {
                        MenuID = pm.MenuID,
                        MenuTitle = pm.MenuTitle,
                        MenuUrl = r.MenuUrl,
                        ParentMenuID = r.ParentMenuID,
                        MenuIcon = r.MenuIcon,
                        MenuOrder = r.MenuOrder,
                        SetBy = pm.SetBy,
                        SetOn = pm.SetOn,
                        Status = pm.Status
                    }).ToList();


                List<Model.SecurityModule.Parent> parent = new List<Model.SecurityModule.Parent>();
                List<Child> child = new List<Child>();


                var root = (from t in PermittedMenus where t.ParentMenuID == "0" &
                                t.Status == 1 select t).ToList();
                foreach (var item in root)
                {
                    Model.SecurityModule.Parent p = new Model.SecurityModule.Parent();
                    p.key = item.MenuID;
                    p.title = item.MenuTitle;
                    child = GetChild(PermittedMenus, item.MenuID);
                    if (child.Count > 0)
                    {
                        p.children = child;
                    }
                    parent.Add(p);
                }

                return Json(parent, JsonRequestBehavior.AllowGet);
            }
            
        }
        [Authorize]
        public List<Child> GetChild(List<SEC_NavMenuOptSetup> lstMenuSetup, string parentId)
        {
            List<Child> lstChild = new List<Child>();
            var hasChild = (from c in lstMenuSetup where c.ParentMenuID == parentId &
                                c.Status == 1 select c).ToList();
            if (hasChild.Count > 0)
            {
                foreach (var item in hasChild)
                {
                    Child obChild = new Child();
                    obChild.key = item.MenuID;
                    obChild.title = item.MenuTitle;
                    obChild.children = GetChild(lstMenuSetup, item.MenuID);
                    lstChild.Add(obChild);
                }
            }
            return lstChild;
        }
        [Authorize]
        [SILLogAttribute]
        public async Task<dynamic> SetRoleMenuPermission(SEC_RoleMenuPermission objMenuDetails)
        {

            if (ModelState.IsValid)
            {

                objMenuDetails.SetBy = UserID;
                
                respStatus = await Task.Run(() => _roleMenuPermissionService.
                    SetRoleMenuPermission(objMenuDetails, out outStatus));
                 
                return Json(new { Message = respStatus.Message, respStatus }, 
                    JsonRequestBehavior.AllowGet);
            }
            else
            {
                //var errors = ModelState.Values.SelectMany(v => v.Errors);
                respStatus = new ValidationResult("E404", _localizationService.GetResource("E404"));
            }
            return Json(new { Message = respStatus.Message, respStatus }, JsonRequestBehavior.AllowGet);
        }
        [Authorize]
        public async Task<dynamic> GetRoleMenuPermission(string _OwnerID, string _RoleID)
        {
            List<SEC_RoleMenuPermission> roleMenuPermissionList = null;

            await Task.Run(() => _roleMenuPermissionService.GetRoleMenuPermission(UserID, "", out roleMenuPermissionList));

            var result = (from r in roleMenuPermissionList
                select new SEC_RoleMenuPermission
                {
                    RoleMenuPermissionID = r.RoleMenuPermissionID,
                    RoleID = r.RoleID,
                    MenuID = r.MenuID
                }).ToList();
                          

            return Json(new { Msg = "", result }, JsonRequestBehavior.AllowGet);
        }
        [Authorize]
        public async Task<dynamic> GetRolePermittedMenu(string _OwnerID, string _RoleID)
        {
            List<SEC_RoleMenuPermission> roleMenuPermissionList = null;
            await Task.Run(() => _roleMenuPermissionService.GetRoleMenuPermission(UserID, "", out roleMenuPermissionList));

            var result = (from r in roleMenuPermissionList
                where r.OwnerID == _OwnerID && r.RoleID == _RoleID & r.Status == 1 && r.UserLevel == "1"
                select r.MenuID).ToList();

            return Json(result, JsonRequestBehavior.AllowGet);
        }

	}

}