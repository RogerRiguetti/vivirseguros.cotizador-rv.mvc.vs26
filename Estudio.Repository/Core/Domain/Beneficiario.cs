using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Estudio.Repository.Core.Domain
{
    public class Beneficiario
    {
  
        public int IdBeneficiario { get; set; }
        public string Nombres { get; set; }
        public string Apellidos { get; set; }
        public string Documento { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public string FechaNacimientoStr { get; set; }
        public int IdSexo { get; set; }
        public string Sexo { get; set; }
        public int IdParentesco { get; set; }
        public string Parentesco { get; set; }
        public int IdTipoDocumento { get; set; }
        public string TipoDocumento { get; set; }
        public int IdSituacionInvalidez { get; set; }
        public string SituacionInvalidez { get; set; }
        public int IdCotizacion { get; set; }
        public string ClaveSexo { get; set; }
        public DateTime FechaInvalidez { get; set; }
        public string FechaInvalidezStr { get; set; }
        public string FechaInvalidezRut { get; set; }
        public DateTime FechaFallecimiento { get; set; }
        public string FechaFallecimientoStr { get; set; }
        public string ClaveSituacionInvalidez { get; set; }
        public string CodigoParentesco { get; set; }
        public string CodigoElemento { get; set; }
        public string PorcentajeBen { get; set; }
        public double PorcentajeBenDbl { get; set; }
        public string Cuspp { get; set; }
        public string FecDev { get; set; }
    }
}
