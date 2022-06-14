using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Estudio.Repository.Core.Domain;
using Estudio.Repository.Core.Domain.Views;

namespace Estudio.Repository.Core.Repositories
{
    public interface IgzPermissionRepository : IRepository<gzPermission>
    {
        List<PermissionView> Search(string description, string active);
        List<PermissionView> Search(string description);
        List<PermissionView> GetPermissions(int pageId);
    }
}
