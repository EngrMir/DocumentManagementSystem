using Ninject;
using Ninject.Modules;
using SILDMS.DataAccess.Departments;
using SILDMS.DataAccess.Menu;
using SILDMS.DataAccess.Users;
using SILDMS.DataAccessInterface.Departments;
using SILDMS.DataAccessInterface.Menu;
using SILDMS.DataAccessInterface.Users;
using SILDMS.Service;
using SILDMS.Service.Departments;
using SILDMS.Service.Menu;
using SILDMS.Service.Users;
using SILDMS.Utillity.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SILDMS.DataAccess;
using SILDMS.DataAccess.DashboardDataService;
using SILDMS.DataAccess.DataLevelPermission;
using SILDMS.DataAccess.DefaultValueSetup;
using SILDMS.DataAccess.DocDestroy;
using SILDMS.DataAccess.DocumentCategory;
using SILDMS.DataAccess.DocumentType;
using SILDMS.DataAccess.MultiDocScan;
using SILDMS.DataAccess.Owner;
using SILDMS.DataAccess.OwnerProperIdentity;
using SILDMS.DataAccess.Roles;
using SILDMS.DataAccessInterface.OwnerProperIdentity;
using SILDMS.DataAccessInterface.Roles;
using SILDMS.Service.Roles;
using SILDMS.Service.OwnerProperIdentity;
using SILDMS.Service.OwnerLevel;
using SILDMS.DataAccessInterface.OwnerLevel;
using SILDMS.DataAccess.OwnerLevel;
using SILDMS.DataAccessInterface.DocumentCategory;
using SILDMS.DataAccessInterface.DocumentType;
using SILDMS.DataAccessInterface.Owner;
using SILDMS.Service.DocumentCategory;
using SILDMS.Service.DocumentType;
using SILDMS.Service.Owner;
using SILDMS.Service.DocProperty;
using SILDMS.DataAccessInterface.OwnerProperty;
using SILDMS.DataAccess.OwnerProperty;
using SILDMS.DataAccess.RoleSetup;
using SILDMS.DataAccessInterface.MultiDocScan;
using SILDMS.DataAccessInterface.RoleSetup;
using SILDMS.Service.MultiDocScan;
using SILDMS.Service.RoleSetup;
using SILDMS.Service.NavMenuOptSetup;
using SILDMS.DataAccessInterface.NavMenuOptSetup;
using SILDMS.DataAccess.NavMenuOptSetup;
using SILDMS.DataAccess.OriginalDocSearching;
using SILDMS.DataAccess.OwnerLevelPermission;
using SILDMS.DataAccess.RoleMenuPermission;
using SILDMS.DataAccess.Server;
using SILDMS.DataAccess.UserAccessLog;
using SILDMS.DataAccess.UserLevel;
using SILDMS.DataAccessInterface.DataLevelPermission;
using SILDMS.DataAccessInterface.OriginalDocSearching;
using SILDMS.DataAccessInterface.OwnerLevelPermission;
using SILDMS.DataAccessInterface.RoleMenuPermission;
using SILDMS.DataAccessInterface.Server;
using SILDMS.DataAccessInterface.UserAccessLog;
using SILDMS.DataAccessInterface.UserLevel;
using SILDMS.Service.DataLevelPermission;
using SILDMS.Service.OriginalDocSearching;
using SILDMS.Service.OwnerLevelPermission;
using SILDMS.Service.RoleMenuPermission;
using SILDMS.Service.Server;
using SILDMS.Service.UserAccessLog;
using SILDMS.Service.UserLevel;
using SILDMS.Service.VersionDocSearching;
using SILDMS.DataAccess.VersionDocSearching;
using SILDMS.DataAccess.VersioningOfOriginalDoc;
using SILDMS.DataAccess.VersioningVersionedDoc;
using SILDMS.DataAccessInterface;
using SILDMS.DataAccessInterface.DashboardDataService;
using SILDMS.DataAccessInterface.VersionDocSearching;
using SILDMS.DataAccessInterface.VersioningOfOriginalDoc;
using SILDMS.DataAccessInterface.VersioningVersionedDoc;
using SILDMS.Service.Dashboard;
using SILDMS.Service.DocDestroyPolicy;
using SILDMS.Service.VersioningOfOriginalDoc;
using SILDMS.Service.VersioningVersionedDoc;
using SILDMS.Service.DocDistribution;
using SILDMS.DataAccessInterface.DocDistribution;
using SILDMS.DataAccess.DocDistribution;
using SILDMS.DataAccessInterface.DefaultValueSetup;
using SILDMS.DataAccessInterface.DocDestroy;
using SILDMS.Service.DefaultValueSetup;
using SILDMS.Service.DocDestroy;

namespace SILDMS.InfraStructure
{
    public class ServiceRegistration
    {
        internal void Load(IKernel kernel)
        {
            kernel.Bind<ILocalizationService>().To<LocalizationService>();

            kernel.Bind<IUserService>().To<UserService>();
            kernel.Bind<IUserDataService>().To<UserDataService>();

            kernel.Bind<IMenuService>().To<MenuService>();
            kernel.Bind<IMenuDataService>().To<MenuDataService>();

            kernel.Bind<IDepartmentService>().To<DepartmentService>();
            kernel.Bind<IDepartmentDataService>().To<DepartmentDataService>();

            kernel.Bind<IRoleService>().To<RoleService>();
            kernel.Bind<IRoleDataService>().To<RoleDataService>();

            kernel.Bind<IOwnerProperIdentityService>().To<OwnerProperIdentityService>();
            kernel.Bind<IOwnerProperIdentityDataService>().To<OwnerProperIdentityDataService>();

            kernel.Bind<IOwnerLevelService>().To<OwnerLevelService>();
            kernel.Bind<IOwnerLevelDataService>().To<OwnerLevelDataService>();

            kernel.Bind<IOwnerService>().To<OwnerService>();
            kernel.Bind<IOwnerDataService>().To<OwnerDataService>();

            kernel.Bind<IDocCategoryService>().To<DocCategoryService>();
            kernel.Bind<IDocCategoryDataService>().To<DocCategoryDataService>();

            kernel.Bind<IDocTypeService>().To<DocTypeService>();
            kernel.Bind<IDocTypeDataService>().To<DocTypeDataService>();

            kernel.Bind<IDocPropertyService>().To<DocPropertyService>();
            kernel.Bind<IDocPropertyDataService>().To<DocPropertyDataService>();

            kernel.Bind<IMultiDocScanService>().To<MultiDocScanService>();
            kernel.Bind<IMultiDocScanDataService>().To<MultiDocScanDataService>();

            kernel.Bind<IRoleSetupService>().To<RoleSetupService>();
            kernel.Bind<IRoleSetupDataService>().To<RoleSetupDataService>();

            kernel.Bind<INavMenuOptSetupService>().To<NavMenuOptSetupService>();
            kernel.Bind<INavMenuOptSetupDataService>().To<NavMenuOptSetupDataService>();

            kernel.Bind<IRoleMenuPermissionService>().To<RoleMenuPermissionService>();
            kernel.Bind<IRoleMenuPermissionDataService>().To<RoleMenuPermissionDataService>();

            kernel.Bind<IOwnerLevelPermissionService>().To<OwnerLevelPermissionService>();
            kernel.Bind<IOwnerLevelPermissionDataService>().To<OwnerLevelPermissionDataService>();

            kernel.Bind<IDataLevelPermissionService>().To<DataLevelPermissionService>();
            kernel.Bind<IDataLevelPermissionDataService>().To<DataLevelPermissionDataService>();

            kernel.Bind<IUserLevelService>().To<UserLevelService>();
            kernel.Bind<IUserLevelDataService>().To<UserLevelDataService>();

            kernel.Bind<IUserAccessLogService>().To<UserAccessLogService>();
            kernel.Bind<IUserAccessLogDataService>().To<UserAccessLogDataService>();

            kernel.Bind<IServerService>().To<ServerService>();
            kernel.Bind<IServerDataService>().To<ServerDataService>();

            kernel.Bind<IOriginalDocSearchingService>().To<OriginalDocSearchingService>();
            kernel.Bind<IOriginalDocSearchingDataService>().To<OriginalDocSearchingDataService>();

            kernel.Bind<IVersionDocSearchingDataService>().To<VersionDocSearchingDataService>();
            kernel.Bind<IVersionDocSearchingService>().To<VersionDocSearchingService>();

            kernel.Bind<IVersioningOfOriginalDocService>().To<VersioningOfOriginalDocService>();
            kernel.Bind<IVersioningOfOriginalDocDataService>().To<VersioningOfOriginalDocDataService>();

            kernel.Bind<IVersioningVersionedDocDataService>().To<VersioningVersionedDocDataService>();
            kernel.Bind<IVersioningVersionedDocService>().To<VersioningVersionedDocService>();

            kernel.Bind<IDashboardDataService>().To<DashboardDataService>();
            kernel.Bind<IDashboardService>().To<DashboardService>();

            kernel.Bind<IDocDestroyPolicyService>().To<DocDestroyPolicyService>();
            kernel.Bind<IDocDestroyPolicyDataService>().To<DocDestroyPolicyDataService>();

            kernel.Bind<IDocDestroyService>().To<DocDestroyService>();
            kernel.Bind<IDocDestroyDataService>().To<DocDestroyDataService>();

            kernel.Bind<IDocDistributionService>().To<DocDistributionService>();
            kernel.Bind<IDocDistributionDataService>().To<DocDistributionDataService>();

            kernel.Bind<IDefaultValueSetupDataService>().To<DefaultValueSetupDataService>();
            kernel.Bind<IDefalutValueSetupService>().To<DefaultValueSetupService>();

        }
    }
   

}
