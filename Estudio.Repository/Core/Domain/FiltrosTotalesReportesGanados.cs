using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Estudio.Repository.Core.Domain
{
    public class FiltrosTotalesReportesGanados
    {
        // Filtros
        public string Departamento { get; set; }
        public string DecisionAfiliado { get; set; }
        public double Desde { get; set; }
        public double Hasta { get; set; }
        public string FechaDesde { get; set; }
        public string FechaHasta { get; set; }
        public string Moneda { get; set; }
        public string[] Prestacion { get; set; }
        public string[] Modalidad { get; set; }
        public string Cotiza { get; set; }
        public string Gana { get; set; }
        public string PrestacionStr { get; set; }
        public string ModalidadStr { get; set; }


        // Totales
        public double TotalNumeroCotizado { get; set; }
        public double TotalPension { get; set; }
        public double TotalTasaVenta { get; set; }
    }
}
