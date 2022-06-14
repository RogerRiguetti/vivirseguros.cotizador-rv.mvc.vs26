using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Estudio.Repository.Core;
using Estudio.Repository.Core.Domain;
using Estudio.Repository.Core.Repositories;
using Estudio.Repository.Persistence.Repositories;
namespace Estudio.Repository.Persistence
{
public class UnitOfWork : IUnitOfWork
{
private readonly ApplicationContext _context;
public int Complete()
{
return _context.SaveChanges();
}
public void Dispose()
{
_context.Dispose();
}
public UnitOfWork(ApplicationContext context)
{
    _context = context;
            SystemRoutines = new gzSystemRepository(_context);
            ModuleRoutines = new gzModuleRepository(_context);
            PageRoutines = new gzPageRepository(_context);
            //RoleRoutines = new gzRoleRepository(_context);
            PermissionRoutines = new gzPermissionRepository(_context);
            PagePermissionRoutines = new PagePermissionRepository(_context);
            RolePagePermissionRoutines = new RolePagePermissionRepository(_context);
            //UserRoutines = new gzUserRepository(_context);
            //GzRolesRoutines = new gzRole(_context);
            AuthorizationRoutines = new AuthorizationRepository(_context);
}
public IgzSystemRepository SystemRoutines { get; private set; }
public IgzModuleRepository ModuleRoutines { get; private set; }
public IgzPageRepository PageRoutines { get; private set; }
public IgzRoleRepository RoleRoutines { get; private set; }
public IgzPermissionRepository PermissionRoutines { get; private set; }
public IPagePermissionRepository PagePermissionRoutines { get; private set; }
public IRolePagePermissionRepository RolePagePermissionRoutines { get; private set; }
public IgzUserRepository UserRoutines { get; private set; }
public IUserProfileRepository UserProfileRoutines { get; private set; }
public IAuthorizationRepository AuthorizationRoutines { get; private set; }
}
}
