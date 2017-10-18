using SILDMS.Model.SecurityModule;
using SILDMS.Service.Menu;
using SILDMS.Utillity;
using SILDMS.Utillity.Localization;
using SILDMS.Web.UI.Areas.SecurityModule.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace SILDMS.Web.UI.Areas.SecurityModule.Controllers
{
    public class MenuController : Controller
    {
        readonly IMenuService _menuService;
        private readonly ILocalizationService _localizationService;
        private string outStatus = string.Empty;
        private string action = "";
        private ValidationResult respStatus = new ValidationResult();
        private readonly string UserID = string.Empty;
        public MenuController(IMenuService repository, ILocalizationService localizationService)
        {
            this._menuService = repository;
            this._localizationService = localizationService;
            UserID = SILAuthorization.GetUserID();
        }

        [SILAuthorize]
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [Authorize]
        public async Task<dynamic> GetMenu()
        {
            List<SEC_Menu> obMenu = null;
            await Task.Run(() => _menuService.GetMenu("", "", "", out obMenu));
            var result = obMenu.Select(x => new
            {
                MenuID = x.MenuID,
                MenuTitle = x.MenuTitle,
                MenuUrl = x.MenuUrl,
                ParentMenuID = x.ParentMenuID,
                MenuParentTitle = (x.ParentMenuID !="" ? ((from t in obMenu where t.MenuID == x.ParentMenuID select t.MenuTitle).FirstOrDefault()) : "Root"),
                MenuIcon = x.MenuIcon,
                MenuOrder = x.MenuOrder,
                Status = x.Status
            }).OrderByDescending(ob => ob.MenuID);

            return Json(new { Msg = "", result }, JsonRequestBehavior.AllowGet);
        }

         
        [HttpPost]
        [Authorize]
        [SILLogAttribute]
        public async Task<dynamic> SetMenu(SEC_Menu objMenu)
        {
            if (ModelState.IsValid)
            {
                action = "add";
                objMenu.SetBy = UserID; 
                objMenu.ModifiedBy = objMenu.SetBy;
                respStatus = await Task.Run(() => _menuService.AddMenu(objMenu, action, out outStatus));

                //    respStatus = await Task.Run(() => _ownerLevelService.AddOwnerLevel(objOwnerLevel, action, out outStatus));
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
        public async Task<string> EditMenu(SEC_Menu objMenu)
        {
            action = "edit";
            objMenu.SetBy = UserID;
            objMenu.ModifiedBy = objMenu.SetBy;
            var result = await Task.Run(() => _menuService.AddMenu(objMenu, action, out outStatus));
            if (result != ValidationResult.Success)
            {
                return result.Message;
            }
            return "OK";
        }
      
         
        [HttpPost]
        [Authorize]
        [SILLogAttribute]
        public async Task<string> DeleteMenu(SEC_Menu menu)
        {
            action = "delete";
            var result = await Task.Run(() => _menuService.DeleteMenu(menu, action, out outStatus));
            if (result != ValidationResult.Success)
            {
                return result.Message;
            }
            return "";
        }
    }
}