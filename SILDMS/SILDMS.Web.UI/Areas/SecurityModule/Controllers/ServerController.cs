using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using SILDMS.Model.DocScanningModule;
using SILDMS.Model.SecurityModule;
using SILDMS.Service.Owner;
using SILDMS.Service.OwnerLevel;
using SILDMS.Service.Server;
using SILDMS.Utillity;
using SILDMS.Utillity.Localization;
using SILDMS.Web.UI.Areas.SecurityModule.Models;

namespace SILDMS.Web.UI.Areas.SecurityModule.Controllers
{
    public class ServerController : Controller
    {

        #region Fields

        private readonly IOwnerService _ownerService;
        private readonly IOwnerLevelService _ownerLevelService;
        private readonly IServerService _serverService;
        private readonly ILocalizationService _localizationService;
        private ValidationResult _respStatus;
        private string _outStatus = string.Empty;
        private string _action = string.Empty;
        private readonly string UserID = string.Empty;

        #endregion

        #region Constructor

        public ServerController(IOwnerService ownerService, IOwnerLevelService ownerLevelService, IServerService serverService,
            ILocalizationService localizationService, ValidationResult respStatus)
        {
            _ownerService = ownerService;
            _ownerLevelService = ownerLevelService;
            _serverService = serverService;
            _localizationService = localizationService;
            _respStatus = respStatus;
            UserID = SILAuthorization.GetUserID();
        }

        #endregion

        // GET: SecurityModule/Server
        //[SILAuthorize]
        public ActionResult Index()
        {
            return View();
        }

        #region Retrive Data

        [Authorize]
        [HttpGet]
        public async Task<dynamic> GetOwnerLevel(string id)
        {
            var ownerLevel = new List<DSM_OwnerLevel>();
            await Task.Run(() => _ownerLevelService.GetOwnerLevel(id, UserID, out ownerLevel));
            var result = ownerLevel.Where(ob => ob.Status == 1).Select(x => new
            {
                x.OwnerLevelID,
                x.LevelName
            }).OrderByDescending(ob => ob.LevelName);
            return Json(new { result, Msg = "" }, JsonRequestBehavior.AllowGet);
        }

        // Get Owner(s)
        [Authorize]
        [HttpGet]
        public async Task<dynamic> GetOwners(string id)
        {
            var owner = new List<DSM_Owner>();
            await Task.Run(() => _ownerService.GetAllOwners("", UserID, out owner));
            var result = owner.Where(ob => ob.Status == 1).Select(x => new
            {
                x.OwnerID,
                x.OwnerLevelID,
                x.OwnerName
            }).Where(ob => ob.OwnerLevelID == id).OrderByDescending(ob => ob.OwnerID);
            return Json(new { result, Msg = "" }, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        [HttpGet]
        public async Task<dynamic> GetServers(string id)
        {
            var servers = new List<SEC_Server>();
            await Task.Run(() => _serverService.GetServers("", UserID, out servers));
            var result = servers.Select(x => new
            {
                x.ServerID,
                x.ServerIP,
                x.LastReplacedIP,
                x.ServerName,
                x.ServerFor,
                x.ServerType,
                x.ServerLocation,
                PurchaseDate = string.Format("{0:dd/mm/yyyy}", x.PurchaseDate),
                x.WarrantyPeriod,
                x.ServerProcessor,x
                .ServerRAM,
                x.ServerHDD,
                x.OwnerLevelID,
                x.LevelName,
                x.OwnerID,
                x.OwnerName,
                x.FtpPort,
                x.FtpUserName,
                x.FtpPassword,
                x.Status
            }).OrderByDescending(ob => ob.ServerID);
            return Json(new {result, Msg = ""}, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Manipulate Server Data

        [Authorize]
        [HttpPost]
        [SILLogAttribute]
        public async Task<dynamic> AddServer(SEC_Server objServer)
        {
            if (ModelState.IsValid)
            {
                _action = "add";
                objServer.SetBy = UserID;
                objServer.ModifiedBy = objServer.SetBy;
                _respStatus = await Task.Run(() => _serverService.ManipulateServer(objServer, _action, out _outStatus));
                // Error handling.   
                return Json(new { Message = _respStatus.Message, _respStatus }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                _respStatus = new ValidationResult("E404", _localizationService.GetResource("E404"));
            }
            return Json(new { Message = _respStatus.Message, _respStatus }, JsonRequestBehavior.AllowGet);
        }

        // Edit Server
        [Authorize]
        [HttpPost]
        [SILLogAttribute]
        public async Task<dynamic> EditServer(SEC_Server objServer)
        {
            if (ModelState.IsValid)
            {
                _action = "edit";
                objServer.SetBy = UserID;
                objServer.ModifiedBy = objServer.SetBy;
                _respStatus = await Task.Run(() => _serverService.ManipulateServer(objServer, _action, out _outStatus));
                return Json(new { Message = _respStatus.Message, _respStatus }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                _respStatus = new ValidationResult("E404", _localizationService.GetResource("E404"));
            }
            return Json(new { Message = _respStatus.Message, _respStatus }, JsonRequestBehavior.AllowGet);
        }


        #endregion
    }
}