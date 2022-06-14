using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Estudio.Repository.Core.Domain
{
    public class UserProfile
    {
        public int UserId { get; set; }
        public string Name { get; set; }        
        public int Active { get; set; }
        public virtual gzUser User { get; set; }
        public string Clave { get; set; }
        public long IndexRol { get; set; }
    }
}
