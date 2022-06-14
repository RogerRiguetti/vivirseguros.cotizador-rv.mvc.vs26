using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Estudio.Repository.Core.Domain
{
    public class PagePermission
    {
        public int Id { get; set; }
        public int PageId { get; set; }
        public int PermissionId { get; set; }
        public virtual gzPage gzPage { get; set; }
        public virtual gzPermission gzPermission { get; set; }
        public virtual List<RolePagePermission> RolePagePermissions { get; set; }
    }
}
