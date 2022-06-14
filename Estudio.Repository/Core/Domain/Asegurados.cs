using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Estudio.Repository.Core.Domain
{
    public class Asegurados
    {
        public string Documento { get; set; }
        public string CUSPP { get; set; }
        public string Nombres { get; set; }
        public string ApellidoPaterno { get; set; }
        public string ApellidoMaterno { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public string Cic { get; set; }
        public DateTime FechaDevengue { get; set; }
        public int IdSexo { get; set; }
        public int IdTipoDocumento { get; set; }
        public int IdDepartamento { get; set; }
        public int IdProvincia { get; set; }
        public int IdDistrito { get; set; }
        public int IdAfp { get; set; }
        public int IdPension { get; set; }
    }
}
