using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Estudio.Repository.Core.Domain
{
    public class Exceptiones
    {
        public string moneda { get; set; }
        public string modalidad { get; set; }
        public string periodoDiferido { get; set; }
        public double rentaTMP { get; set; }
        public string perGarantizado { get; set; }
        public double rentaEsc { get; set; }
        public double primaUnica { get; set; }
        public double renTmp1T { get; set; }
        public double mtoPensio { get; set; }
        public double tasaVenta { get; set; }
        public double tir { get; set; }
        public string perdida { get; set; }
        public double comision { get; set; }
        public int numOperacion { get; set; }
        public string dni { get; set; }
        public string afp { get; set; }
        public double cic { get; set; }
        public string asegurado { get; set; }
        public string cuspp { get; set; }
        public string sexo { get; set; }
        public string fechaNac { get; set; }
        public string codTipRen { get; set; }
        public double codTipCambio { get; set; }
        public string numCot { get; set; }
        public int numArchivo { get; set; }
        public DateTime fechaNacDate { get; set; }
        public string FechaDevengueStr { get; set; }
        public DateTime FechaDevengue { get; set; }
        public string FechaEstudioStr { get; set; }
        public DateTime FechaEstudio { get; set; }
        public string CodigoPension { get; set; }
        public int numCorrelativo { get; set; }
        public string caso { get; set; }
        public int regTablaEx { get; set; }
        public string codTP { get; set; }
        public double mtoSumPension { get; set; }
        public string Ind_Cob { get; set; }
        public string Fec_DevSolStr { get; set; }
        public DateTime Fec_DevSol { get; set; }
        public string Ind_Estado { get; set; }
        public int Ind_SISCO { get; set; }
        public int anosRT { get; set; }
        public decimal prcRVD { get; set; }
        public string siCotizaNoCotiza { get; set; }
        public string nroCotizacion { get; set; }
        public decimal primaAFPEESS { get; set; }
        public decimal primaEESS { get; set; }
        public decimal pensionRT { get; set; }
        public decimal tasaRT { get; set; }
        public decimal pensionRVD { get; set; }
        public decimal tasaRVD { get; set; }
        public int cod_tipreajuste { get; set; }
        public int mes_esc { get; set; }
        public string Cod_region { get; set; }

        //calculo
        public string MTO_AJUSTEIPC { get; set; }
        public string MTO_CTAINDAFP { get; set; }
        public string MTO_RENTATMPAFP { get; set; }
        public string MTO_RESMAT { get; set; }
        public string PRC_TASATCE { get; set; }
        public string FecCal { get; set; }
        public string MTO_PENANUAL { get; set; }
        public string MTO_PENSIONGAR { get; set; }
        public string MTO_PRIUNISIM { get; set; }
        public string MTO_RMGTOSEP { get; set; }
        public string MTO_RMGTOSEPRV { get; set; }
        public string MTO_VALREAJUSTEMEN { get; set; }
        public string MTO_VALREAJUSTETRI { get; set; }
        public string MTO_VALPREPENTMP { get; set; }
        public string primerTramo { get; set; }
        public string derechoCrecer { get; set; }
        public string gratificacion { get; set; }
        public string motivo { get; set; }
        public int estado { get; set; }

    }
}
