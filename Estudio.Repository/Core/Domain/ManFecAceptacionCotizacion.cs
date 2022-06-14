using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Estudio.Repository.Core.Domain
{
    public class ManFecAceptacionCotizacion
    {
        //Variables Archivo Busqueda
        public string NUM_COT { get; set; }
        public int NUM_CORRELATIVO { get; set; }
        public string NUM_OPERACION { get; set; }
        public string COD_CUSPP { get; set; }
        public int NUM_MESDIF { get; set; }
        public string COD_MODALIDAD { get; set; }
        public int NUM_MESGAR { get; set; }
        public string COD_MONEDA { get; set; }
        public double PRC_RENTATMP { get; set; }
        public string COD_COBERCON { get; set; }
        public string COD_DERGRA { get; set; }
        public string COD_DERCRE { get; set; }
        public string FEC_SUSCRIPCION { get; set; }
        public string FEC_ENVIO { get; set; }
        public string COD_ESTCOT { get; set; }
        public double MTO_PRIUNI { get; set; }
        public double MTO_PENSION { get; set; }
        public double PRC_TASAVTA { get; set; }
        public string COD_TIPREN { get; set; }
        public double MTO_PRIUNIDIF { get; set; }
        public double MTO_SUMPENSION { get; set; }
        public string COD_TIPPENSION { get; set; }
        public string FEC_ACEPTA { get; set; }
        public int NUM_MESESC { get; set; }
        public double PRC_RENTAESC { get; set; }
        public string IND_GANA { get; set; }
    }
}
