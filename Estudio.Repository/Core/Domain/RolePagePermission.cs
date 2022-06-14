using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Estudio.Repository.Core.Domain
{
    public class RolePagePermission
    {
        public int Id { get; set; }
        public int RoleId { get; set; }
        public int PagePermissionId { get; set; }
        public virtual PagePermission PagePermission { get; set; }
        public virtual gzRole Role { get; set; }
        public virtual List<Authorization> Authorizations { get; set; }
    }
}
