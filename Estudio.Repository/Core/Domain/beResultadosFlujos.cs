using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Estudio.Repository.Core.Domain
{
    public class beResultadosFlujos
    {
        public string numPol { get; set; }
        public int numOrd { get; set; }
        public long numEdad { get; set; }
        public int numMes { get; set; }
        public double mtoPen { get; set; }
        public double prcFac { get; set; }
        public double GtoSep { get; set; }
        public double fluPen { get; set; }
        public double tasTce { get; set; }

        public string Tip { get; set; } //Variable para definir la tabla (Cartera)

    }
}
