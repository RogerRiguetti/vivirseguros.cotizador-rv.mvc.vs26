using System;
using System.Collections.Generic;

namespace Estudio.Controllers.Request
{
    public class CalculoExtraOficialRequest
    {
        public int IdCotizacionJubilare { get; set; }
        public string Aplicacion { get; set; }
        public string TipoCambio { get; set; }
        public int GastoSepelio { get; set; }
        public int MontoCIC { get; set; }
        public string Asesor { get; set; }
        public Asegurado Asegurado { get; set; }
        public List<Beneficiario> Beneficiario { get; set; }
        public List<Modalidad> Modalidad { get; set; }
    }

    public class Asegurado
    {
        public int IdBeneficiarioJubilare { get; set; }
        public string CUSPP { get; set; }
        public string TipoInvalidez { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public string Genero { get; set; }
        public int PorcentajeBeneficiario { get; set; }
        public string Departamento { get; set; }
        public string Distrito { get; set; }
        public string Provincia { get; set; }
        public string TipoAFP { get; set; }
        public string TipoPension { get; set; }
        public string NombreDocumento { get; set; }
        public string NumeroDocumento { get; set; }
        public string Nombres { get; set; }
        public string ApellidoMaterno { get; set; }
        public string ApellidoPaterno { get; set; }
        public DateTime FechaDevengue { get; set; }
    }

    public class Beneficiario
    {
        public int IdBeneficiarioJubilare { get; set; }
        public string Nombres { get; set; }
        public string ApellidoPaterno { get; set; }
        public string ApellidoMaterno { get; set; }
        public string Parentesco { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public string NombreDocumento { get; set; }
        public string NumeroDocumento { get; set; }
        public string Genero { get; set; }
        public string TipoInvalidez { get; set; }
    }

    public class Modalidad
    {
        public int IdModalidadJubilare { get; set; }
        public string Moneda { get; set; }
        public string TipoRenta { get; set; }
        public string TipoModalidad { get; set; }
        public decimal PeriodoDiferido { get; set; }
        public decimal PeriodoGarantizado { get; set; }
        public string Gratificacion { get; set; }
        public decimal PrimerTramo { get; set; }
        public decimal SegundoTramo { get; set; }
        public decimal TasaRentaAFP { get; set; }
        public decimal RentaTemp { get; set; }
    }
}