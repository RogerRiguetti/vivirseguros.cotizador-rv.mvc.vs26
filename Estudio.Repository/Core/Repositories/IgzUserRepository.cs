using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Estudio.Repository.Core.Domain;
using Estudio.Repository.Core.Domain.Views;

namespace Estudio.Repository.Core.Repositories
{
    public interface IgzUserRepository : IRepository<gzUser>
    {
        List<UserView> Search(string account, string active);
        List<UserView> Search(string account);
        List<AuthorizationView> GetPermissions(int userId);
        List<RoleView> GetRolesByUser(int roleId);
    }
}
