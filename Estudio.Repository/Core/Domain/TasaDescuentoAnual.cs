using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Estudio.Repository.Core.Domain
{
    public class TasaDescuentoAnual
    {
        public int Tramo { get; set; }
        public decimal AK { get; set; }
        public decimal BK { get; set; }
        public decimal CK { get; set; }
        public string CPK { get; set; }
        public string CodigoMoneda { get; set; }
        public string TipoReajuste { get; set; }
        public string TipoMoneda { get; set; }
        public string FechaInicioStr { get; set; }
        public string FechaTerminoStr { get; set; }
        public string Periodo { get; set; }
        public string Fecha { get; set; }
        public string Hora { get; set; }
        public string Usuario { get; set; }
    }
}
