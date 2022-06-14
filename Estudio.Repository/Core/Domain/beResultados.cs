using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Estudio.Repository.Core.Domain
{
    public class beResultados
    {
        public double MTO_PENANUAL {get; set;}
        public double MTO_RMGTOSEP {get; set;}
        public double MTO_RMPENSION {get; set;}
        public double PRC_TASATCE {get; set;}
        public double PRC_TASAVTA {get; set;}
        public double PRC_TASATIR {get; set;}
        public double MTO_RESMAT {get; set;}
        public double NUM_CORRELATIVO {get; set;}
        public double MTO_PRIMA {get; set;}
        public double MTO_COMISION {get; set;}
        public double MTO_PENSION {get; set;}
        public double MTO_GASTO {get; set;}
        public double MTO_RENTA {get; set;}
        public double MTO_AJUSTE {get; set;}
        public double MTO_MARGEN {get; set;}
        public double MTO_MARGENIMP {get; set;}
        public double MTO_EXCEDENTE {get; set;}
        public double MTO_YIECURVE {get; set;}
        public double MTO_CPK {get; set;}
        public string NUM_COTESTUDIO {get; set;}
        public double NUM_ANNO {get; set;}
        public double PRC_PERCON {get; set;}
        public double MTO_VALREAJUSTETRI {get; set;}
        public double MTO_VALREAJUSTEMEN {get; set;}
        public double MTO_VALREAJUSTEANU {get; set;}
        public double MTO_AJUSTEIPC {get; set;}
        public double MTO_RMGTOSEPRV {get; set;}
        public double MTO_PRIUNISIM {get; set;}
        public double MTO_VALPREPENTMP {get; set;}
        public double MTO_PENSIONGAR {get; set;}
        public double MTO_PRIUNIDIF {get; set;}
        public double MTO_CTAINDAFP {get; set;}
        public double MTO_RENTATMPAFP {get; set;}
        public double PRIMA_UNICA {get; set;}
        public double MTO_SUMPENSION {get; set;}
        public string MARCASOB {get; set;}
        public string Mensaje { get; set; }
        public double PRC_TASATCI { get; set; }
        public double PRC_TASAPRO { get; set; }

        //Variable de código de rechazo
        public string Cod_Rechazo { get; set; }
        public string FecCal { get; set; }
        public string INDFILTROCOTIZA { get; set; }
        public string COD_ESTCOT { get; set; }
        public int IND_SISCO { get; set; }

        public double PRC_COM { get; set; }
    }
}
