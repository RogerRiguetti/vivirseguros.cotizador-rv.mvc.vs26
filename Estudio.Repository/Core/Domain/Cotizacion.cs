using System;

namespace Estudio.Repository.Core.Domain
{
    public class Cotizacion
    {
        public int IdCotizacion { get; set; }
        public string Documento { get; set; }
        public string CUSPP { get; set; }
        public string Nombres { get; set; }
        public string ApellidoPaterno { get; set; }
        public string ApellidoMaterno { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public string FechaNacimientoStr { get; set; }
        public decimal Cic { get; set; }
        public DateTime FechaDevengue { get; set; }
        public string FechaDevengueStr { get; set; }
        public DateTime FechaEstudio { get; set; }
        public string FechaEstudioStr { get; set; }
        public decimal GastoSepelio { get; set; }
        public string TipoCambio { get; set; }
        public DateTime FechaCotizacion { get; set; }
        public string FechaCotizacionStr { get; set; }
        public int IdAsesor { get; set; }
        public string Asesor { get; set; }
        public int IdSexo { get; set; }
        public int IdTipoDocumento { get; set; }
        public string TipoDocumento { get; set; }
        public int IdDepartamento { get; set; }
        public int IdProvincia { get; set; }
        public int IdDistrito { get; set; }
        public int IdAfp { get; set; }
        public int IdPension { get; set; }
        public string Afp { get; set; }
        public string CodigoPension { get; set; }
        public string ClaveSexo { get; set; }
        public string PorAfp { get; set; }
        public int Estado { get; set; }
        public int[] IdCotizacionjubilare { get; set; }
        public int[] IdModalidadjubilare { get; set; }
        public int[] IdBeneficiariojubilare { get; set; }
        public string[] Benf_prc_pension { get; set; }
        public object Tipo_Pension { get; set; }
        public string[] Mod_mon { get; set; }
        public string[] Mod_tre { get; set; }
        public decimal[] Mod_pes { get; set; }
    }
}