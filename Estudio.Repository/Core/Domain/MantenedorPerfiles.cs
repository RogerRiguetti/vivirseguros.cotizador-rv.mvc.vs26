using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Estudio.Repository.Core.Domain
{
    public class MantenedorPerfiles
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public string Active { get; set; }
        public string DetailDescription { get; set; }
        public int IdSistema { get; set; }
        public string NombreSistema { get; set; }
        public string ClaveSistema { get; set; }
        public int UserId { get; set; }
        public string Name { get; set; }
        public int IdprofilePermission { get; set; }
        public string NamePantalla { get; set; }
        public string NodoPadre { get; set; }
        

    }
}
