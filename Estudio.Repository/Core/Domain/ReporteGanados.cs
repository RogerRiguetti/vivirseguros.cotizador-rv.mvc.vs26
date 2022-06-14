using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Estudio.Repository.Core.Domain
{
    public class ReporteGanados
    {
        public int NumeroCotizado { get; set; }
        public double Pension { get; set; }
        public double TasaVenta { get; set; }
        public double DiferenciaPension { get; set; }
        public double DiferenciaTasaVenta { get; set; }
        public string CompañiaSegurosVitalicios { get; set; }
        public int NumeroCasosGanados { get; set; }
        public int NumeroCasosTotales { get; set; }
        public string color { get; set; }
        public double ParticipacionMercado { get; set; }
        public int NumeroRegistro { get; set; }
        public string Compania { get; set; }
    }
}
