using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Estudio.Repository.Core.Domain
{
    public class gzPermission
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public string Active { get; set; }
        public virtual List<PagePermission> PagePermissions { get; set; }
        public string DetailDescription { get; set; }
        public int IdSistema { get; set; }
        public string NamePantalla { get; set; }
        public string NodoPadre { get; set; }
    }
}
