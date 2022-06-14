using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Estudio.Repository.Core.Domain
{
    public class ParametroGasto
    {
        public string FechaInicial { get; set; }

        public string FechaTermino { get; set; }

        public decimal Mto_GastosAdmin { get; set; }

        public decimal Mto_GastosEmi { get; set; }

        public decimal PRC_GastosCtrlSuper { get; set; }

        public decimal PRC_Endeudamiento { get; set; }

        public decimal Cto_Capital { get; set; }

        public decimal PRC_TasaMercado { get; set; }

        public decimal PRC_ImpuestoRenta { get; set; }

        public int YearTM { get; set; }

        public string Moneda { get; set; }

        public decimal PrccomS1 { get; set; }
        public decimal Prcfaclab1 { get; set; }
        public decimal PrccomS2 { get; set; }
        public decimal Prcfaclab2 { get; set; }
        public int Cod_tipren { get; set; }
    }
}
