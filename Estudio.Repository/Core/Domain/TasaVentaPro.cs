using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Estudio.Repository.Core.Domain
{
    public class TasaVentaPro
    {
        public int IdMoneda { get; set; }
        public string Elemento { get; set; }
        public string CodigoMoneda { get; set; }
        public string ClaveMoneda { get; set; }
        public string IdRangoTasa { get; set; }
        public string Mes { get; set; }
        public decimal Prom { get; set; }
    }
}
