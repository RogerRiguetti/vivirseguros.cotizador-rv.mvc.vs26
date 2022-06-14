using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Estudio.Repository.Core.Domain
{
    public class beCPK
    {
        public string COD_MONEDA { get; set; }
        public int COD_TIPREAJUSTE { get; set; }
        public int NUM_ANNO { get; set; }
        public double PRC_CPK { get; set; }
        public string FEC_INIVIG { get; set; }
        public string FEC_TERVIG { get; set; }
    }
}
