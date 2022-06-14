using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Estudio.Repository.Core.Domain
{
    public class ManCurvaTasasRes
    {
        public DateTime Fec_IniVig { get; set; }
        public int Mes { get; set; }
        public decimal TasaAnualSolesIndex { get; set; }
        public decimal TasaAnualSolesAj { get; set; }
        public decimal TasaAnualDolaresAj { get; set; }
        public int Num_Mes { get; set; }
        public decimal Mto_valor { get; set; }
        public string COD_SCOMP { get; set; }
    }
}
