using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Estudio.Repository.Core.Domain
{
    public class TasaAnclaje
    {
        public string FechaInicial { get; set; }
        public string FechaTermino { get; set; }
        public Decimal Tasa { get; set; }
        public string valorMoneda { get; set; }
    }
}