using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Estudio.Repository.Core.Domain
{
    public class beRentabilidad
    {
        public string COD_MONEDA { get; set; }
        public int COD_TIPREAJUSTE { get; set; }
        public int NUM_ANNO { get; set; }
        public double PRC_TASAREN { get; set; }
        public int Cod_Scomp { get; set; }
        public string Gls_Descripcion { get; set; }
        public string FEC_INIVIG { get; set; }
        public string FEC_TERVIG { get; set; }
        public string Periodo { get; set; }
        public string IDPeriodo { get; set; }
        public string Usuario { get; set; }

    }
}
