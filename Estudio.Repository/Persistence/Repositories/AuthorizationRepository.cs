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
public class AuthorizationRepository : Repository<Authorization>, IAuthorizationRepository
{
public AuthorizationRepository(ApplicationContext _context) : base(_context) { }
public ApplicationContext applicationContext { get { return Context as ApplicationContext; } }
//public List<ViewUserPermission> ExecuteProcedure(int parameter)
//{
//List<ViewUserPermission> result = applicationContext.Database
//.SqlQuery<ViewUserPermission>("ProcedureName @parameter", new SqlParameter("parameter", parameter))
//.ToList();
//return result;
//}
}
}
