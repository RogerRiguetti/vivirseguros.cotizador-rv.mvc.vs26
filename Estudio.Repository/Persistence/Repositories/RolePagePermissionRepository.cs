using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Estudio.Repository.Core.Domain;
using Estudio.Repository.Core.Repositories;
using Estudio.Repository.Helpers;
using System.Data.SqlClient;
namespace Estudio.Repository.Persistence.Repositories
{
public class RolePagePermissionRepository : Repository<RolePagePermission>, IRolePagePermissionRepository
{
public RolePagePermissionRepository(ApplicationContext _context) : base(_context) { }
public ApplicationContext applicationContext { get { return Context as ApplicationContext; } }
//public List<ViewRolePagePermission> ExecuteProcedure(int parameter)
//{
//List<ViewRolePagePermission> result = applicationContext.Database
//.SqlQuery<ViewRolePagePermission>("ProcedureName @parameter", new SqlParameter("parameter", parameter))
//.ToList();
//return result;
//}
}
}
