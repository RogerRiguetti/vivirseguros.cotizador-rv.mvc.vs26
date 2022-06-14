using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Estudio.Repository.Core.Domain
{
    public class gzPage
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public string Active { get; set; }
        public int ModuleId { get; set; }
        public virtual gzModule Module { get; set; }
        public virtual List<PagePermission> PagePermissions { get; set; }
    }
}
