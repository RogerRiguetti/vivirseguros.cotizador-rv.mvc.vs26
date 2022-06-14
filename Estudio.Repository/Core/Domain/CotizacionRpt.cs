using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Estudio.Repository.Core.Domain
{
   public class CotizacionRpt
    {
        public int IdCotizacionRpt { get; set; }
        public string NombreCompletoAse { get; set; }
        public string Cuspp { get; set; }
        public string Afp { get; set; }
        public string TasaVenta { get; set; }
        public string Pension { get; set; }
        public string FechaDevengue { get; set; }
        public decimal Cic { get; set; }
        public string TipoCambio { get; set; }
        public string FechaEstudio { get; set; }
        public string NombreCompletoBene { get; set; }
        public string Moneda { get; set; }
        public string Modalidad { get; set; }
        public int AniosDiferidos { get; set; }
        public int PorcentajeRentaTemporal { get; set; }
        public int AniosGarantizados { get; set; }
        public decimal RentaEscalonada { get; set; }
        public string Parentesco { get; set; }
        public string SituacionInvalidez { get; set; }
        public string Sexo { get; set; }
        public string FechaNacimientoBen { get; set; }
        public decimal PorcentajeRentabilidadAfp { get; set; }
        public decimal PrimerTramo { get; set; }
        public decimal SegundoTramo { get; set; }
        public string PrimerTramoStr { get; set; }
        public string SegundoTramoStr { get; set; }
        public int IdModalidad { get; set; }
        public double PRC_TASAVTA { get; set; }
        public double PRC_TASATIR { get; set; }
        public double PRC_PERCON { get; set; }
        public double MTO_PENSION { get; set; }
        public double MTO_RENTATMPAFP { get; set; }
        public int IdBeneficiario { get; set; }
        public string CodigoPension { get; set; }
        public double MontoPension { get; set; }
        public string MontoPensionStr { get; set; }
        public decimal PorcentajeBeneficiario { get; set; }
        public string CodigoTiposRenta { get; set; }
    }
}
