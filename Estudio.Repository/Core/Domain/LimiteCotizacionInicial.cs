using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Estudio.Repository.Core.Domain
{
    public class LimiteCotizacionInicial
    {
        //valor combos
        public string CodDepartamento { get; set; }
        public string CodMoneda { get; set; }
        public string DescMoneda { get; set; }
        public int CodReajuste { get; set; }
        //valor de las fechas
        public string fechaInicial { get; set; }
        public string FechaFinal { get; set; }
        //items de TIR
        public decimal Minimo_Tir { get; set; }
        public decimal Minimo_TirInmediatas { get; set; }
        public decimal Maximo_Tir { get; set; }
        //items de Perdida
        public decimal Minimo_Per { get; set; }
        public decimal Maximo_Per { get; set; }
        //items de Comision
        public decimal Minimo_Com { get; set; }
        public decimal Maximo_Com { get; set; }

        public decimal ComisionInmediata { get; set; }
        public decimal ComisionDiferida { get; set; }
        //items de lista tasa Venta
        public string CodPension { get; set; }
        public string NombrePension { get; set; }
        public decimal Minimo_TV { get; set; }
        public decimal Maximo_TV { get; set; }

        public List<LimiteCotizacionInicial> RangosTasaVenta { get; set; }
        //excel
        public string lugar { get; set; }
        public string cic { get; set; }
        public string moneda { get; set; }
        public double it { get; set; }
        public double ip { get; set; }
        public double s { get; set; }
        public double ja { get; set; }
        public double jl { get; set; }
        public double tir { get; set; }
        public double prd { get; set; }
        public string codRegion { get; set; }
        public string accion { get; set; }
    }
}
