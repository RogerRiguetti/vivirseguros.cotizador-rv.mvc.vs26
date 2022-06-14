using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Estudio.Repository.Core.Domain
{
   public class Descargas
    {
        public int Id { get; set; }
        public string NombreArchivo { get; set; }
        public string Path { get; set; }
        public int Estatus { get; set; }
        public int Descargado { get; set; }
        public DateTime Fecha { get; set; }
    }
}
