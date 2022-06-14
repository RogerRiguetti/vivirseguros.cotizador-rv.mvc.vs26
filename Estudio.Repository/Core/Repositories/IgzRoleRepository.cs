using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Estudio.Repository.Core.Domain;
using Estudio.Repository.Core.Domain.Views;

namespace Estudio.Repository.Core.Repositories
{
    public interface IgzRoleRepository : IRepository<gzRole>
    {
        List<RoleView> Search(string account, string active);
        List<RoleView> Search(string account);
        List<AuthorizationView> GetPermissionsByRole(int roleId);
    }
}
