using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using SILDMS.Model.DocScanningModule;
using SILDMS.Service.Owner;
using SILDMS.Service.OwnerLevel;
using SILDMS.Utillity;
using SILDMS.Utillity.Localization;
using System.Web.SessionState;
using SILDMS.Web.UI.Areas.SecurityModule;
using SILDMS.Web.UI.Areas.SecurityModule.Models;

namespace SILDMS.Web.UI.Areas.DocScanningModule.Controllers
{
    public class OwnerController : Controller
    {

        #region Fields

        readonly IOwnerService _ownerService;
        private readonly IOwnerLevelService _ownerLevelService;
        private readonly ILocalizationService _localizationService;
        private ValidationResult _respStatus = new ValidationResult();
        private string _outStatus = string.Empty;
        private string _action = string.Empty;
        private readonly string UserID = string.Empty;
        #endregion

        #region Constructor

        public OwnerController(IOwnerService ownerService, ILocalizationService localizationService, IOwnerLevelService ownerLevelService)
        {
            _ownerService = ownerService;
            _localizationService = localizationService;
            _ownerLevelService = ownerLevelService;
            UserID = SILAuthorization.GetUserID();
        }

        #endregion

        // GET: DocScanningModule/Owner
        [SILAuthorize]
        public ActionResult Index()
        {
            return View();
        }

        #region Owner Manipulation

        // Add New Owner
        [Authorize]
        [HttpPost]
        [SILLogAttribute]
        public async Task<dynamic> AddOwner(DSM_Owner objOwner)
        {
            if (ModelState.IsValid)
            {
                _action = "add";
                objOwner.SetBy = UserID;
                objOwner.ModifiedBy = objOwner.SetBy;
                _respStatus = await Task.Run(() => _ownerService.ManipulateOwner(objOwner, _action, out _outStatus));
                // Error handling.   
                return Json(new { Message = _respStatus.Message, _respStatus }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                _respStatus = new ValidationResult("E404", _localizationService.GetResource("E404"));
            }
            return Json(new { Message = _respStatus.Message, _respStatus }, JsonRequestBehavior.AllowGet);
        }

        // Edit Owner
        [Authorize]
        [HttpPost]
        [SILLogAttribute]
        public async Task<dynamic> EditOwner(DSM_Owner obOwner)
        {
            if (ModelState.IsValid)
            {
                _action = "edit";
                obOwner.SetBy = UserID;
                obOwner.ModifiedBy = obOwner.SetBy;
                _respStatus = await Task.Run(() => _ownerService.ManipulateOwner(obOwner, _action, out _outStatus));
                return Json(new { Message = _respStatus.Message, _respStatus }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                _respStatus = new ValidationResult("E404", _localizationService.GetResource("E404"));
            }
            return Json(new { Message = _respStatus.Message, _respStatus }, JsonRequestBehavior.AllowGet);
        }

        // Delete Owner
        [Authorize]
        [HttpPost]
        [SILLogAttribute]
        public async Task<dynamic> DeleteOwnerLevel(DSM_Owner objOwner)
        {
            _action = "delete";
            _respStatus = await Task.Run(() => _ownerService.ManipulateOwner(objOwner, _action, out _outStatus));
            return Json(new { Message = _respStatus.Message, _respStatus }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Retrive Data for View

        // Get Owner(s)
        [Authorize]
        [HttpGet]
        public async Task<dynamic> GetOwners(string ownerId)
        {
            var owner = new List<DSM_Owner>();
            await Task.Run(() => _ownerService.GetAllOwners(ownerId, UserID, out owner));
            var result = owner.Select(x => new
            {
                x.OwnerID,
                x.OwnerLevelID,
                x.UDOwnerCode,
                x.LevelName,
                x.OwnerName,
                x.OwnerShortName,
                x.ParentOwner,
                x.ParentName,
                x.Status
            }).OrderByDescending(ob => ob.OwnerID);
            return Json(new {result, Msg = ""}, JsonRequestBehavior.AllowGet);
        }

        // Get Owner Level(s)
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

        // Get Parents
        [Authorize]
        [HttpGet]
        public async Task<dynamic> GetParent(string id)
        {
            var OwnerLevels= new List<DSM_OwnerLevel>();
            var Owners = new List<DSM_Owner>();
            await Task.Run(() => _ownerLevelService.GetOwnerLevel("", UserID, out OwnerLevels));
            await Task.Run(() => _ownerService.GetAllOwners("", UserID, out Owners));
            var OwnerLevelsAccessLevel = (from ol in OwnerLevels
                where ol.OwnerLevelID == id
                select ol.LevelAccess).FirstOrDefault();

            var result = (from ol in OwnerLevels
                where Convert.ToInt32(ol.LevelAccess) < Convert.ToInt32(OwnerLevelsAccessLevel)

                join ow in Owners on ol.OwnerLevelID equals ow.OwnerLevelID into owners

                from ow in owners.DefaultIfEmpty()
                where ow.Status == 1
                select new
                {
                    ParentOwner = ow.OwnerID,
                    ParentName = ow.OwnerName
                }).ToList();

            //var result = parents.Where(ob => (ob.OwnerLevelID == id && ob.Status == 1)).Select(x => new
            //{
            //    ParentOwner = x.OwnerID,
            //    ParentName = x.OwnerName
            //});
            return Json(new {result, Msg = ""}, JsonRequestBehavior.AllowGet);
        }

        #endregion

    }
}