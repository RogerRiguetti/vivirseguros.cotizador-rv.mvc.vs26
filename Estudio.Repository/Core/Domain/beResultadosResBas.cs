using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Estudio.Repository.Core.Domain
{
    public class beResultadosResBas
    {
        public string numPol { get; set; }
        public int numBen { get; set; }
        public string estCal { get; set; }
        public double mtoResBas { get; set; } //Reserva Total
        public double mtoResBasRet { get; set; } //Reserva Total

    }
}
