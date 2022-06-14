using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Estudio.Repository.Core.Domain
{
    public class gzSystem
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public string Active { get; set; }
        public virtual List<gzModule> Modules { get; set; }
    }
}
