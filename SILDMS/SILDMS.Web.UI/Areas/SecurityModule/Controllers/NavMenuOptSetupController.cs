
using SILDMS.Model.SecurityModule;
using SILDMS.Service.NavMenuOptSetup;
using SILDMS.Utillity;
using SILDMS.Utillity.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace SILDMS.Web.UI.Areas.SecurityModule.Controllers
{
    public class NavMenuOptSetupController : Controller
    {
        #region Fields
                private readonly INavMenuOptSetupService _menuSetupService;
        private readonly ILocalizationService _localizationService;
        private ValidationResult respStatus = new ValidationResult();
        private string outStatus = string.Empty;
        private string action = "";

        #endregion

        //#region Constructor

        public NavMenuOptSetupController(INavMenuOptSetupService menuSetupService, ILocalizationService localizationService)
        {
            _localizationService = localizationService;     
            _menuSetupService = menuSetupService;
                
        }



        //#endregion


        [HttpGet]
        //  [CustomAuthorize]
        public async Task<dynamic> LoadMenuSetupData(string ownerID)
        {
            List<Parent> parent = new List<Parent>();
            List<Child> child = new List<Child>();
            List<SEC_NavMenuOptSetup> resultNav=null;
            var data = await Task.Run(() => _menuSetupService.GetNavMenuOptSetup(ownerID, out resultNav));

            var root = (from t in resultNav where t.ParentMenuID == "0" select t).ToList();
            foreach (var item in root)
            {
                Model.SecurityModule.Parent p = new Model.SecurityModule.Parent();
                p.key = item.MenuID;
                p.title = item.MenuTitle;
                p.isFolder = true;
                child = GetChild(resultNav, item.MenuID);
              //  p.select = Convert.ToBoolean(item.check == "" ? "false" : item.check);      
                
                if (child.Count > 0)
                {
                    p.children = child;
                }
                parent.Add(p);
            }

            return Json( parent , JsonRequestBehavior.AllowGet);
        }

        public List<Child> GetChild(List<SEC_NavMenuOptSetup> lstMenuSetup, string parentId)
        {
            List<Child> lstChild = new List<Child>();
            var hasChild = (from c in lstMenuSetup where c.ParentMenuID == parentId select c).ToList();
            if (hasChild.Count > 0) 
            { 
                foreach (var item in hasChild)
                {
                    Child obChild = new Child();
                    obChild.key = item.MenuID;
                    obChild.title = item.MenuTitle;
                    obChild.select = Convert.ToBoolean(item.check==""?"false":item.check);                   
                    obChild.children = GetChild(lstMenuSetup,item.MenuID);
                    lstChild.Add(obChild);
                }
            }
            return lstChild;
        }
        //
        // GET: /SecurityModule/MenuOperationSetup/
        public ActionResult Index()
        {

            return View();
        }

        [HttpPost]
        [SILLogAttribute]
        public async Task<dynamic> SetMenuOperations(SEC_MenuDetails objMenuDetails)
        {
   
            if (ModelState.IsValid)
            {
                action = "add";
                //objMenuDetails.SetBy = Session["UserId"].ToString();           
                respStatus = await Task.Run(() => _menuSetupService.SetMenuOperations(objMenuDetails, action, out outStatus));
                // Error handling.   
                return Json(new { Message = respStatus.Message, respStatus }, JsonRequestBehavior.AllowGet);
            }
            else 
            {
                respStatus = new ValidationResult("E404", _localizationService.GetResource("E404"));
            }
            return Json(new { Message = respStatus.Message, respStatus}, JsonRequestBehavior.AllowGet);
        }

	}
}