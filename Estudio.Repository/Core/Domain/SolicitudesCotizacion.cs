using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Estudio.Repository.Core.Domain
{
    public class SolicitudesCotizacion
    {
        public string vgFecPro { get; set; }
        //ASESORES
        public int idAsesor { get; set; }
        public string nomAsesor { get; set; }
        public int numAgente { get; set; }
        //
        public string strCodRechazo { get; set; }
        public int TblNumMort { get; set; }
        public int codRechazo { get; set; }
        public string usuario { get; set; }
        public string strError { get; set; }
        public string strError1 { get; set; }
        public int existeNumOpe { get; set; }
        public int existeNumArch { get; set; }
        public int success { get; set; }
        public string strSuccess { get; set; }
        //Numero de archivo
        public int numArchivo { get; set; }
        //TRASPASO COTIZACIONES
        public int idAsignado { get; set; }
        public string strCodVejez { get; set; }
        public string strTipPen { get; set; }
        public string strMon { get; set; }
        public int intTipIde { get; set; }
        public string strNumCot { get; set; }
        public string strOperacion { get; set; }
        public int intAnno { get; set; }
        public string strCor { get; set; }
        public int iNumOrden { get; set; }
        public string strPar { get; set; }
        public int intCor { get; set; }
        public string strTipRen { get; set; }
        public string strMod { get; set; }
        public int intNumGar { get; set; }
        public string strMonMod { get; set; }
        public string strReajuste { get; set; }
        public string ioCodMoneda { get; set; }
        public string ioCodReajuste { get; set; }
        public string strCodSCOMP { get; set; }
        public double douPrcRVD { get; set; }
        public int intNumDif { get; set; }
        public int intNumEsc { get; set; }
        public double douPrcRtaEsc { get; set; }
        public int strValReajusteTri { get; set; }
        public int strValReajusteMen { get; set; }
        public string Cod_Etapa { get; set; }
        public string Num_Correlativo { get; set; }
        public string strFecSeg { get; set; }
        public string Fec_Ini { get; set; }
        public string Hor_Ini { get; set; }
        public string strEtapa { get; set; }
        public string Cod_EtapaAnt { get; set; }
        public bool bolSw { get; set; }
        public string strFecha { get; set; }
        //ESTCARGASOL
        public int intOKcarga { get; set; }
        public int intOKfondo { get; set; }
        public int intOKproducto { get; set; }
        public int intOKafiliado { get; set; }
        public int intOKbeneficiario { get; set; }
        public int intERRcarga { get; set; }
        public int intERRfondo { get; set; }
        public int intERRproducto { get; set; }
        public int intERRafiliado { get; set; }
        public int intERRbeneficiario { get; set; }
        public int intERRRP { get; set; }
        public int intOKRP { get; set; }
        public int regTotal { get; set; }
        public int regErr { get; set; }
        public int regOk { get; set; }
        //VARIABLES SOLICITUDES RECIBIDAS 
        public int intNumOpe { get; set; }
        public string strAfp { get; set; }
        public string strCussp { get; set; }
        public string strTipBen { get; set; }
        public string strCamMod { get; set; }
        public string strPenPre { get; set; }
        public string strTasaRPRT { get; set; }
        public DateTime datFecDev { get; set; }
        public string strFecDev { get; set; }
        public DateTime datFecSus { get; set; }
        public string strFecSus { get; set; }
        public DateTime datFecDevSol { get; set; }
        public string strFecDevSol { get; set; }
        public DateTime datFecEnv { get; set; }
        public string strFecEnv { get; set; }
        public DateTime datFecCie { get; set; }
        public string strFecCie { get; set; }
        public string strTipCam { get; set; }
        public DateTime datFecCita { get; set; }
        public string strFecCita { get; set; }
        public string strHorCita { get; set; }
        public string strLugCita { get; set; }
        public string strDepto { get; set; }
        //VARIABLES AFILIADO 
        public string strTipoDoc { get; set; }
        public string strNumDoc { get; set; }
        public string strApPat { get; set; }
        public string strApMat { get; set; }
        public string strNom { get; set; }
        public string strNomSec { get; set; }
        public string strSexo { get; set; }
        public DateTime datFecNac { get; set; }
        public string strFecNac { get; set; }
        public string strGraInv { get; set; }
        public string strSitInv { get; set; }
        public string strEstSob { get; set; }
        //VARIABLES FONDO 
        public string strCodMon { get; set; }
        public string strCapPen { get; set; }
        public string strMtoCIC { get; set; }
        public string strMtoCuo { get; set; }
        public string strMtoSal { get; set; }
        public string strBonAct { get; set; }
        public string strCodCob { get; set; }
        public string strTipCamAA { get; set; }
        public string strCodCiaCob { get; set; }
        public string strApoAdi { get; set; }
        //VARIABLES BENEFICIARIO
        public string strPatBen { get; set; }
        public string strMatBen { get; set; }
        public string strNomBen { get; set; }
        public string strNomSecBen { get; set; }
        public string strParBen { get; set; }
        public string strSitInvBen { get; set; }
        public string datFecNacBen { get; set; }
        public string strFecNacBen { get; set; }
        public string strSexoBen { get; set; }
        //VARIABLES PRODUCTO
        public string strCodMod { get; set; }
        public long contador { get; set; }
        public string strMonPro { get; set; }
        public string strannosRT { get; set; }
        public string strPrcRVD { get; set; }
        public string strPerGar { get; set; }
        public string strCobCon { get; set; }
        //I - KVR 09/08/07
        public string strGratif { get; set; }
        public string strDerCre { get; set; }
        //F - KVR 09/08/07 

        //I - MC 14/09/09
        public string strPartCapital { get; set; }
        public string strNumMensualidad { get; set; }
        public string strTipoFondo { get; set; }

        //Reporte 
        public string nomArchivo { get; set; }
        public string descError { get; set; }
        public int Ind_Sis { get; set; }
        public int SISok { get; set; }
        public double pensionSis { get; set; }
        public double tasaSis { get; set; }

        //RRR - 08/09/2025
        public int SISVSok { get; set; }
        public int SISVSvs { get; set; }
        public double MtoCIc { get; set; }

        //REPORTE DE RESUMEN
        #region Reporte de Resumen
        public int NUM_ARCH_RESUMEN { get; set; }
        public int NUM_REGOKCAR { get; set; }
        public int NUM_REGERRCAR { get; set; }
        public int TotalCarga { get; set; }
        public int NUM_REGOKAFI { get; set; }
        public int NUM_REGERRAFI { get; set; }
        public int TotalAfi { get; set; }
        public int NUM_REGOKFON { get; set; }
        public int NUM_REGERRFON { get; set; }
        public int TotalFondo { get; set; }
        public int NUM_REGOKBEN { get; set; }
        public int NUM_REGERRBEN { get; set; }
        public int TotalBen { get; set; }
        public int NUM_REGOKPROD { get; set; }
        public int NUM_REGERRPROD { get; set; }
        public int TotalProd { get; set; }
        public int NUM_REGOKRP { get; set; }
        public int NUM_REGERRRP { get; set; }
        public int TotalRP { get; set; }
        #endregion
    }
}
