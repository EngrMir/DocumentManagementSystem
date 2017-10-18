using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using SILDMS.Model.SecurityModule;
using SILDMS.Service.Menu;
using SILDMS.Service.Users;
using SILDMS.Web.UI.Areas.SecurityModule.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace SILDMS.Web.UI.Areas.SecurityModule.Controllers
{
    [RoutePrefix("api/ApiAccount")]
    public class ApiAccountController : ApiController
    {
        
        private AuthRepository _repo = null;
        readonly IMenuService _menuService;
        public ApiAccountController(IMenuService repository)
        {
            _repo = new AuthRepository();
            this._menuService = repository;
        }

        //[HttpGet]
        ////  [CustomAuthorize]
        //public async Task<dynamic> GetMenu(string menuId)
        //{
        //    List<Sec_NavigationMenu> obMenu = null;
        //    await Task.Run(() => _menuService.GetMenu("","", "", out obMenu));
        //    var result = obMenu.Select(x => new
        //    {
        //        MenuId = x.MenuId,
        //        MenuTitle = x.MenuTitle,
        //        MenuUrl = x.MenuUrl,
        //        MenuParentId = x.MenuParentId,
        //        MenuParentTitle = (x.MenuParentId > 0 ? ((from t in obMenu where t.MenuId == x.MenuParentId select t.MenuTitle).FirstOrDefault()) : "Root"),
        //        MenuIcon = x.MenuIcon,
        //        MenuOrder = x.MenuOrder,
        //        MenuStatus = x.MenuStatus
        //    }).OrderByDescending(ob => ob.MenuId);

        //    return result;
        //}


        //private IAuthenticationManager Authentication
        //{
        //    get { return Request.GetOwinContext().Authentication; }
        //}

        ////[HttpPost]
        ////public bool SetAuthorization(string user)
        ////{
        ////    GetUserAccessPermission_Result obPermission = null;
        ////    var data = _userService.GetUserAccessPermission(user, out obPermission); //_repo.FindRefreshTokenIdByUser(user, "sildms");
        ////    if (data != null)
        ////    {
        ////        return false;
        ////    }
        ////    var claims = new List<Claim>() 
        ////        { 
        ////            new Claim(ClaimTypes.NameIdentifier, obPermission.UserId), 
        ////            new Claim(ClaimTypes.Name, obPermission.UserName), 
        ////            new Claim(ClaimTypes.Role, obPermission.RoleId), 
        ////            new Claim("RoleName", obPermission.Role)            
        ////        };
        ////    var identity = new ClaimsIdentity(claims, DefaultAuthenticationTypes.ApplicationCookie);
        ////    var ctx = Request.GetOwinContext();
        ////    var authenticationManager = ctx.Authentication;
        ////    authenticationManager.SignIn(identity);
        ////    return true;
        ////}

        //// POST api/Account/Register

        [HttpPost]
        [AllowAnonymous]
        [Route("Register")]
        public async Task<IHttpActionResult> Register(UserModel userModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            IdentityResult result = await _repo.RegisterUser(userModel);
            IHttpActionResult errorResult = GetErrorResult(result);
            if (errorResult != null)
            {
                return errorResult;
            }
            return Ok();
        }

        [HttpGet]
        [ActionName("Logout")]
        public HttpResponseMessage Logout()
        {
            var authentication = HttpContext.Current.GetOwinContext().Authentication;
            authentication.SignOut(DefaultAuthenticationTypes.ExternalBearer);
            return new HttpResponseMessage(HttpStatusCode.OK);
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _repo.Dispose();
            }
            base.Dispose(disposing);
        }

        private IHttpActionResult GetErrorResult(IdentityResult result)
        {
            if (result == null)
            {
                return InternalServerError();
            }

            if (!result.Succeeded)
            {
                if (result.Errors != null)
                {
                    foreach (string error in result.Errors)
                    {
                        ModelState.AddModelError("", error);
                    }
                }

                if (ModelState.IsValid)
                {
                    // No ModelState errors are available to send, so just return an empty BadRequest.
                    return BadRequest();
                }

                return BadRequest(ModelState);
            }

            return null;
        }
   
    }
}
