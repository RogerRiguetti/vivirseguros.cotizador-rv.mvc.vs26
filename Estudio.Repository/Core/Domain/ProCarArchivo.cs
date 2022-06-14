using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Estudio.Repository.Core.Domain
{
    public class ProCarArchivo
    {
        #region VARIABLES PARA REPORTES

        //Variable para validación
        public int Num_Reg { get; set; }
        public string Titulo_Rpt { get; set; }
        public string Subtitulo_Rpt { get; set; }

        public string Num_Archivo { get; set; }
        public string Nom_Archivo { get; set; }
        public int Num_Orden { get; set; }
        public int Num_Orden2 { get; set; }

        //Reporte Resumen
        public int Num_RegOksol { get; set; }
        public int Num_RegErrsol { get; set; }
        public int Total_Carga { get; set; }
        public int Num_RegOkmod { get; set; }
        public int Num_RegErrmod { get; set; }
        public int Total_Afi { get; set; }
        public int Num_RegOkcia { get; set; }
        public int Num_RegErrcia { get; set; }
        public int Total_Fondo { get; set; }
        public int Num_RegOkafp { get; set; }
        public int Num_RegErrafp { get; set; }
        public int Total_Ben { get; set; }

        //Reportes
        public string Num_Cotizacion { get; set; }
        public int Num_Corr { get; set; }
        public int Num_Operacion { get; set; }
        public string CUSPP { get; set; }
        public string Tipo_Pension { get; set; }
        public string Tipo_Renta { get; set; }
        public int Meses_DifEsc { get; set; }
        public string Modalidad { get; set; }
        public int Meses_Gar { get; set; }
        public string Cob_Cony { get; set; }
        public string D_Crecer { get; set; }
        public string D_Gratif { get; set; }
        public decimal Renta_Temp { get; set; }
        public string Moneda { get; set; }
        public decimal TIR { get; set; }
        public decimal Renta_Esc { get; set; }
        public decimal Tasa_Venta { get; set; }
        public decimal Mto_Pension { get; set; }
        public decimal Tasa_RT { get; set; }
        public decimal Tasa_RP_RT { get; set; }
        public decimal Mto_PensionRT { get; set; }
        public decimal Prima_Unica { get; set; }
        public decimal Perdida_Contable { get; set; }
        public string Cod_Compañia { get; set; }
        public string Nivel_Base { get; set; }
        public string AFP { get; set; }

        #endregion

        public string CodCia { get; set; }
        public int intNumOpe { get; set; }
        public int intPerGar { get; set; }
        public bool bolExiste { get; set; }
        public bool bolMod { get; set; }
        public bool bolAfp { get; set; }
        public string strTipRen { get; set; }
        public string strMoneda { get; set; }
        public string strDecision { get; set; }
        public string strCussp { get; set; }
        public string strModalidad { get; set; }
        public string strCobCony { get; set; }
        public string strDerCre { get; set; }
        public string strDerGra { get; set; }
        public string strCodCia { get; set; }
        public string strNumCot { get; set; }
        public string douporcentajeRVD { get; set; }
        public string strParCapital { get; set; }
        public int douanosRT { get; set; }
        public string douPriUnicaAFP { get; set; }
        public string douPriUniAFPEESS { get; set; }
        public string strAtiende { get; set; }
        public string strGana { get; set; }
        public string strCotiza { get; set; }
        public string douPrima { get; set; }
        public string douPension { get; set; }
        public string douTasa { get; set; }
        public string douPensionRT { get; set; }
        public string douTasaRT { get; set; }
        public string strCodAfp { get; set; }
        public string strError { get; set; }
        public string strNivBas { get; set; }
        public int numArchivo { get; set; }
        public string strCodCierre { get; set; }
        public string strCodUsuario { get; set; }
        public string strAjuste { get; set; }
        public double prcRVD { get; set; }
        public int numAnosRT { get; set; }
        public string strCodModalidad { get; set; }
        public string strMod2 { get; set; }
        public string codCoberCon { get; set; }
        public int intMesesDif { get; set; }
        public double douPorc { get; set; }
        public double douPrcRtaEsc { get; set; }
        public int intNumEsc { get; set; }
        public int success { get; set; }
        //aceptacion cotizacion
        public double douTasaCia { get; set; }
        public string prcTasaCiaRT { get; set; }
        public int numMesGar { get; set; }
        public double prcRentaTMP { get; set; }
        public int numMesesC { get; set; }
        public string codTipreajuste { get; set; }
        public int strNumCor { get; set; }
        public string fecCierre { get; set; }
        public int numOrden { get; set; }
        public double prcPension { get; set; }
        public string indCalsofDif { get; set; }
        public double prcPensionDIF { get; set; }
        public string etapaAnterior { get; set; }
        //tabla PT_TMAE_COTIZACION  
        public string fecSubscripcion { get; set; }
        public string fecEnvio { get; set; }
        public string FecDev { get; set; }
        public string codIsapre { get; set; }
        public string codTipPension { get; set; }
        public string codVejez { get; set; }
        public string codEstCivil { get; set; }
        public string codCliente { get; set; }
        public int codTipoIden { get; set; }
        public string numIden { get; set; }
        public string direccion { get; set; }
        public int codDireccion { get; set; }
        public string glsFondo { get; set; }
        public string glsCorreo { get; set; }
        public string codViapago { get; set; }
        public string codTipCuenta { get; set; }
        public string codBanco { get; set; }
        public string numCuenta { get; set; }
        public string codSucursal { get; set; }
        public int numAnnoJub { get; set; }
        public int numCargas { get; set; }
        public int codTipoIdenCor { get; set; }
        public string numIdenCor { get; set; }
        public string codBenSocial { get; set; }
        public double mtoMonedaFon { get; set; }
        public double mtoPriuniFon { get; set; }
        public double mtoCataIndFon { get; set; }
        public double mtoBonoFon { get; set; }
        public double mtoPriuni { get; set; }
        public double mtoCtaInd { get; set; }
        public double mtoBono { get; set; }
        public double prcTasaPRT { get; set; }
        public double mtoApoadi { get; set; }
        public string codMonedaFon { get; set; }
        public string indCob { get; set; }
        public string CodTipCot { get; set; }
        public string fecCrea { get; set; }
        public string horaCrea { get; set; }
        public string codUsuarioModi { get; set; }
        public string fecModi { get; set; }
        public string horModi { get; set; }
        public string indEstado { get; set; }
        public string codRegion { get; set; }
        public string fecCalculo { get; set; }
        public double mtoValMoneda { get; set; }
        public double prcCorCom { get; set; }
        public double prcCorComReal { get; set; }
        public double mtoCorCom { get; set; }
        public double prcRentaAFP { get; set; }
        public double prcRentaAfpori { get; set; }
        public double mtoFacPenella { get; set; }
        public double prcFacPanella { get; set; }
        public double mtoCuomor { get; set; }
        public double prcTasaTce { get; set; }
        public double prcTasaVta { get; set; }
        public double prcTasaTir { get; set; }
        public string codTipTir { get; set; }
        public double prcTasaPerGar { get; set; }
        public double mtoCnu { get; set; }
        public double mtoPriuniSim { get; set; }
        public double mtoPriuniDif { get; set; }
        public double mtoPensionGar { get; set; }
        public double mtoCtaIndAfp { get; set; }
        public double mtoRentaTmpAfp { get; set; }
        public double mtoResmat { get; set; }
        public double mtoValPrePenTMP { get; set; }
        public double mtoSumPension { get; set; }
        public double mtoPenAnual { get; set; }
        public double mtoRmPension { get; set; }
        public double mtoRmgtosep { get; set; }
        public double mtoPercon { get; set; }
        public double prcPercon { get; set; }
        public string codEstCot { get; set; }
        public string fecAcepta { get; set; }
        public string codRechazo { get; set; }
        public double mtoRmgTosepRV { get; set; }
        public double mtoAjusteIPC { get; set; }
        public string indCalSobDif { get; set; }
        public double mtoValReajusteTri { get; set; }
        public double mtoValReajusteMen { get; set; }
        //estadisticas
        public int intOKcierresol { get; set; }
        public int intERRcierresol { get; set; }
        public int intOKcierremod { get; set; }
        public int intERRcierremod { get; set; }
        public int intOKcierrecia { get; set; }
        public int intERRcierrecia { get; set; }
        public int intOKcierreafp { get; set; }
        public int intERRcierreafp { get; set; }
        public int intGanadas { get; set; }
        public int intPerdidas { get; set; }
        public int intAFP { get; set; }
        public int intRecotizadas { get; set; }
        public int intDesistidas { get; set; }
        public int intCaducadas { get; set; }
        public string descError { get; set; }
    }
}
