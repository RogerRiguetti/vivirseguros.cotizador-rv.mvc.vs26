using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Estudio.Repository.Core.Repositories;

namespace Estudio.Repository.Core
{
    public interface IUnitOfWork : IDisposable
    {
        int Complete();
        IgzSystemRepository SystemRoutines { get; }
        IgzModuleRepository ModuleRoutines { get; }
        IgzPageRepository PageRoutines { get; }
        IgzRoleRepository RoleRoutines { get; }
        IgzPermissionRepository PermissionRoutines { get; }
        IPagePermissionRepository PagePermissionRoutines { get; }
        IRolePagePermissionRepository RolePagePermissionRoutines { get; }
        IgzUserRepository UserRoutines { get; }
        IUserProfileRepository UserProfileRoutines { get; }
        IAuthorizationRepository AuthorizationRoutines { get; }
    }
}
