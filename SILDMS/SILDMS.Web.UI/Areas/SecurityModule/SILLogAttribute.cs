using SILDMS.Model.SecurityModule;
using SILDMS.Service.UserAccessLog;
using SILDMS.Utillity;
using SILDMS.Web.UI.Areas.SecurityModule.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Ninject.Web.Mvc.FilterBindingSyntax;

namespace SILDMS.Web.UI.Areas.SecurityModule
{
    public class SILLogAttribute : ActionFilterAttribute
    {
        private readonly string UserID = string.Empty;
        protected DateTime start_time;
     
        private IUserAccessLogService userAccessLogService;
        public SILLogAttribute()
            : this(DependencyResolver.Current.GetService(typeof(IUserAccessLogService)) as IUserAccessLogService)
        {
            UserID = SILAuthorization.GetUserID();
            
        }

        public SILLogAttribute(IUserAccessLogService userAccessLogService)
        {
            // TODO: Complete member initialization
            this.userAccessLogService = userAccessLogService;
        }
       

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            start_time = DateTime.Now;
        }

        public override void OnResultExecuted(ResultExecutedContext filterContext)
        {
             SEC_UserLog _userLog = new SEC_UserLog();
            RouteData route_data = filterContext.RouteData;
            _userLog.ActionExecuteTime = (DateTime.Now - start_time).ToString();
            _userLog.ActionUrl = (route_data.DataTokens["area"] as string) + "/" + (route_data.Values["controller"] as string) + "/" + route_data.Values["action"] as string;
            _userLog.UserID = UserID;
            _userLog.UsedIP = GetIPAddress.LocalIPAddress();
            _userLog.UserAction = route_data.Values["action"] as string;
            _userLog.ActionEventTime = DateTime.Now.ToString();
            userAccessLogService.ManipulateUserAccessLog(_userLog);          
        }
    }
}