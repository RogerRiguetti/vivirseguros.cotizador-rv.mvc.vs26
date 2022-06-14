using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Estudio.Repository.Core.Domain
{
    public class ReporteCasosGanadosRV
    {
        public string num_poliza { get; set; }
        public string cuspp { get; set; }
        public string fechaAdjudicacion { get; set; }
        public string fechaTransferencia { get; set; }
        public string fechaCotizacion { get; set; }
        public decimal comisionAsesorCotizado { get; set; }
        public decimal comisionSupervisorCotizado { get; set; }
        public string prestacion { get; set; }
        public string modalidad { get; set; }
        public string moneda { get; set; }
        public string añosDiferidos { get; set; }
        public string añosGarantizados { get; set; }
        public string primerTramo { get; set; }
        public decimal segundoTramo { get; set; }
        public string gratificacion { get; set; }
        public string cobertura { get; set; }
        public decimal cicCotizacion { get; set; }
        public decimal primaCotizacion { get; set; }
        public decimal rentaCotizacion { get; set; }
        public decimal tasaAFP { get; set; }
        public decimal tcCotizacion { get; set; }
        public decimal tv { get; set; }
        public decimal tir { get; set; }
        public decimal perdida { get; set; }
        public decimal tce { get; set; }
        public decimal tlr { get; set; }
        public decimal tasaMercado { get; set; }
        public decimal duracion { get; set; }
        public decimal spread { get; set; }
        public string puesto { get; set; }
        public string mejora { get; set; }
        public string nombreAfiliado { get; set; }
        public string nombreAsesor { get; set; }
        public string nombreSupervisor { get; set; }
        public string Departamento { get; set; }
        public decimal cicRealSoles { get; set; }
        public decimal primaCSV { get; set; }
        public decimal rentaRealMoneda { get; set; }
        public decimal rentaReferencia { get; set; }
        public decimal rentaReferenciaActualizada { get; set; }
        public decimal ComisionAsesorReal { get; set; }
        public decimal ComisionSupervisorReal { get; set; }
        public string parrilla { get; set; }
    }
}
