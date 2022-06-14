using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Estudio.Repository.Core.Domain
{
    public class beTasaFacVac
    {
        public DateTime FEC_INICUOMOR { get; set; }
        public DateTime FEC_TERCUOMOR { get; set; }
        public double MTO_FACTOR { get; set; }
        public DateTime FEC_IPC { get; set; }
        public double MTO_IPC { get; set; }
        public double PRC_IPC { get; set; }
            
    }
}
