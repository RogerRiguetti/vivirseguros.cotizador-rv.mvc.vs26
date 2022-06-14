using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Estudio.Repository.Core.Domain
{
   public class beCurvaTasas
    {
        public string COD_MONEDA { get; set; }
        public int COD_TIPPREAJUSTE { get; set; }
        public int NUM_MES { get; set; }
        public double MTO_VALOR { get; set; }
        public string FEC_INIVIG { get; set; }
        public string FEC_TERVIG { get; set; }

        //Variables para obtener el Gasto Sepelio del mes.
        public double MTO_CUOMOR { get; set; }
        public string FEC_INICUOMOR { get; set; }
        public string FEC_TERCUOMOR { get; set; }

    }
}
