using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Estudio.Repository.Core.Domain
{
    public class beDatosTasasPar
    {
        public string CodMon { get; set; }
        public string TipPen { get; set; }
        public string CodReg { get; set; }
        public double PriMin { get; set; }
        public double PriMax { get; set; }
        public double MtoGad { get; set; }
        public double PrcDeu { get; set; }
        public double MtoImp { get; set; }
        public double MtoGem { get; set; }
        public double PrcTir { get; set; }
        public double PrcTas { get; set; }
        public double PrcPer { get; set; }
        public int TipRea { get; set; }
        public double ComMax { get; set; }
        public double ComMin { get; set; }
        public double TasaU { get; set; }
    }
}
