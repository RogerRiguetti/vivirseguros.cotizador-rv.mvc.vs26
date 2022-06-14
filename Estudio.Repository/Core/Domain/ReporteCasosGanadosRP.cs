using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Estudio.Repository.Core.Domain
{
    public class ReporteCasosGanadosRP
    {
        public string num_poliza { get; set; }
        public string fechaAbono { get; set; }
        public decimal comisionAsesorCotizado { get; set; }
        public decimal comisionSupervisorCotizado { get; set; }
        public string moneda { get; set; }
        public string añosPlazo { get; set; }
        public string añosDiferido { get; set; }
        public decimal primaMoneda { get; set; }
        public decimal rentaMoneda { get; set; }
        public decimal tv { get; set; }
        public decimal tir { get; set; }
        public decimal perdida { get; set; }
        public decimal tasaInversion { get; set; }
        public decimal duracion { get; set; }
        public decimal spread { get; set; }
        public string mejora { get; set; }
        public string nombreAsegurado { get; set; }
        public string nombreAsesor { get; set; }
        public string nombreSupervisor { get; set; }
        public string departamento { get; set; }
        public string comisionAsesorReal { get; set; }
        public string comisionSupervisorReal { get; set; }
        public string parrilla { get; set; }
    }
}
