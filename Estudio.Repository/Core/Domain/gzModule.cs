using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Estudio.Repository.Core.Domain
{
    public class gzModule
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public string Active { get; set; }
        public int SystemId { get; set; }
        public virtual gzSystem gzSystem { get; set; }
        public virtual List<gzPage> Pages { get; set; }
    }
}
