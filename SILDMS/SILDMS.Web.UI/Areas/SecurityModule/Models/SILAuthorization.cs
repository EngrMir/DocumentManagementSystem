using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.SessionState;

namespace SILDMS.Web.UI.Areas.SecurityModule.Models
{
    [SessionState(SessionStateBehavior.ReadOnly)]
    public static class SILAuthorization
    {
        public static string GetUserID() {
            try { return Convert.ToString(HttpContext.Current.Session["UserID"]); }
            catch (Exception) { return ""; }
        }

        public static string GetOwnerLevelID()
        {
            if (HttpContext.Current.Session["OwnerLevelID"] != null)
            {
                return Convert.ToString(HttpContext.Current.Session["OwnerLevelID"]);
            }
            return "";
        }

        public static string GetOwnerID()
        {
            if (HttpContext.Current.Session["OwnerID"] != null)
            {
                return Convert.ToString(HttpContext.Current.Session["OwnerID"]);
            }
            return "";
        }
    }
}