using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Estudio.Repository.Core.Domain
{
    public class beResultadosResBasBen
    {
        public string numPol { get; set; }
        public int numOrd { get; set; }
        public long edad { get; set; }
        public double mtoResBas { get; set; } //Reserva Total
        public double mtoResSep { get; set; } // reserva de gastos de Sep
        public double mtoResBasGar { get; set; } // reserva garantiazada * prc de pension del benef
        public double mtoResBasLeg { get; set; } // reserva vitalicia * prc de pension del benef
    }
}
