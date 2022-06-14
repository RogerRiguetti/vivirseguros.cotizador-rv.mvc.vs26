using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Estudio.Repository.Core.Domain
{
    public class Authorization
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int RolePagePermissionId { get; set; }
        public virtual gzUser User { get; set; }
        public virtual RolePagePermission RolePagePermission { get; set; }
    }
}
