using SILDMS.DataAccessInterface.Menu;
using SILDMS.Model.SecurityModule;
using SILDMS.Utillity;
using SILDMS.Utillity.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SILDMS.Service.Menu
{
    public class MenuService : IMenuService
    {
        #region Fields

        private readonly IMenuDataService menuDataService;
        private readonly ILocalizationService localizationService;
        private string errorNumber = "";
        #endregion

        #region Constractor
        public MenuService(
             IMenuDataService repository,
             ILocalizationService localizationService
            )
        {
            this.menuDataService = repository;
            this.localizationService = localizationService;
        }

        #endregion


        public ValidationResult GetMenu(string ownerID, string menuID, string action, out List<SEC_Menu> menuList)
        {
            menuList = menuDataService.GetMenu(ownerID, menuID, action, out errorNumber);
            if (errorNumber.Length > 0)
            {
                return new ValidationResult(errorNumber, localizationService.GetResource(errorNumber));
            }
            return ValidationResult.Success;
        }

        public ValidationResult AddMenu(SEC_Menu menu, string action, out string status)
        {
            menuDataService.AddMenu(menu, action, out status);
            if (status.Length > 0)
            {
                return new ValidationResult(status, localizationService.GetResource(status));
            }
            return ValidationResult.Success;
        }

        public ValidationResult EditMenu(SEC_Menu menu, string action, out string status)
        {
            throw new NotImplementedException();
        }
        
        public ValidationResult DeleteMenu(SEC_Menu menu, string action, out string status)
        {
            throw new NotImplementedException();
        }

        public StringBuilder GenerateMenu(List<SEC_Menu> lstMenu)
        {
            int i = 0;
            lstMenu = lstMenu.Where(ob => ob.PermissionClass == "").ToList();
            StringBuilder sb = new StringBuilder();
            List<Child> child = new List<Child>();
            var root = (from t in lstMenu where t.ParentMenuID == "0" select t).ToList();
            sb.Append("<ul class='sidebar-menu'><li class='header'>MAIN NAVIGATION</li>");
            foreach (var item in root)
            {
                sb.Append("<li class='treeview active'><a href='" + item.MenuUrl + "'><i class='" + item.MenuIcon + "'></i> <span>" + item.MenuTitle + "</span><i class='fa fa-angle-left pull-right'></i></a>");
                GetChild(lstMenu, item.MenuID, sb,i);
                sb.Append("</li>");
            }
            sb.Append("</ul>");
            return sb;
        }
        public StringBuilder GetChild(List<SEC_Menu> lstMenuSetup, string parentId, StringBuilder sb, int i)
        {            
            var hasChild = (from c in lstMenuSetup where c.ParentMenuID == parentId select c).ToList();           
            if (hasChild.Count > 0)
            {// menu-open
                if (i == 0)
                { sb.Append("<ul class='treeview-menu  menu-open' style='display: block;'>"); i++; }
                else
                { sb.Append("<ul class='treeview-menu  menu-open' style='display: none;'>"); }
                foreach (var item in hasChild)
                {
                   
                    var hasChild2 = (from c in lstMenuSetup where c.ParentMenuID == item.MenuID select c).ToList();
                    if (hasChild2.Count > 0)
                    {
                        sb.Append("<li><a href='" + item.MenuUrl + "'><i class='" + item.MenuIcon + "'></i> " + item.MenuTitle + "<i class='fa fa-angle-left pull-right'></i></a>");
                        GetChild(lstMenuSetup, item.MenuID, sb,i);
                    }
                    else {
                        sb.Append(" <li><a href='" + item.MenuUrl + "'><i class='" + item.MenuIcon + "'></i> " + item.MenuTitle + "</a>");
                    }
                    sb.Append("</li>");
                }
             
                sb.Append("</ul>");
            }
           
            return sb;
        }
    }
}
