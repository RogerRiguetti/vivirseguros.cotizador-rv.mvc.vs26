using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Estudio.Repository.Core.Domain
{
    public class Reservas
    {
        public decimal PrcCastigo { get; set; }
        public decimal TopeMax { get; set; }
        public string Num_Poliza { get; set; }
        public int Num_Endoso { get; set; }
        public string Cod_Tippension { get; set; }
        public string Cod_Estado { get; set; }
        public string Cod_Tipren { get; set; }
        public string Cod_Modalidad { get; set; }
        public string Fec_Vigencia { get; set; }
        public decimal Mto_Prima { get; set; }
        public decimal Mto_Pension { get; set; }
        public int Num_Mesdif { get; set; }
        public int Num_Mesgar { get; set; }
        public decimal Prc_tasace { get; set; }
        public decimal Prc_tasvta { get; set; }
        public int Num_Cargas { get; set; }
        public string FecDev { get; set; }
        public string Cod_Cuspp { get; set; }
        public string Cod_Moneda { get; set; }
        public string Ind_Cob { get; set; }
        public string Cod_Cobercon { get; set; }
        public decimal Mto_Facpenella { get; set; }
        public decimal Prc_Facpenella { get; set; }
        public string Cod_Dercre { get; set; }
        public string Cod_Dergra { get; set; }
        public string Cod_Tipreajuste { get; set; }
        public decimal Mto_Valreajustetri { get; set; }
        public decimal Mto_Valreajustemen { get; set; }
        public int Num_Mesesc { get; set; }
        public decimal Prc_Rentaesc { get; set; }
        public decimal mto_pensionact { get; set; }
        public string FecPagPri { get; set; }
        public string FecCot { get; set; }
        public string CobCia { get; set; }
        public decimal MtoGtoSep { get; set; }
        public int QuiebraPorc { get; set; }
        public int QuiebraTopeMax { get; set; }
        public decimal IpcDevengue { get; set; }
        public decimal IpcCotizacion { get; set; }
        public int Cod_cliente { get; set; }
        public bool SwFecDev { get; set; }
        public int Num_Orden { get; set; }
        public string Cod_Sexo { get; set; }
        public string Cod_Par { get; set; }
        public string Cod_SitInv { get; set; }
        public string Cod_Grufam { get; set; }
        public string Cod_Derpen { get; set; }
        public string Cod_Motreqpen { get; set; }
        public string Fec_Nacben { get; set; }
        public string Fec_Falben { get; set; }
        public string Fec_Nachm { get; set; }
        public string Fec_Invben { get; set; }
        public string Cod_Cauinv { get; set; }
        public decimal Mto_Pensiongar { get; set; }
        public decimal Prc_Pension { get; set; }
        public decimal Prc_Pensionleg { get; set; }
        public decimal Prc_Pensiongar { get; set; }
        public string Cod_ESTPERIODO { get; set; }
        public double Prc_TasBaseDef { get; set; }
        public int NumeroRegistros { get; set; }
        public int Num_Caso_Especial { get; set; }

        #region VARIABLES PARA ARCHIVO CSV
        //Variables para creación de archivo CSV.
        public string Tipo_Cartera { get; set; }
        public string Pol_NumPol { get; set; }
        public string Pol_CUSPP { get; set; }
        public string Pol_CodTipPen { get; set; }
        public string Pol_CodMod { get; set; }
        public string Pol_FecVig { get; set; }
        public string Pol_FecDev { get; set; }
        public int Pol_MesesDif { get; set; }
        public int Pol_MesesGar { get; set; }
        public int Pol_MesesEsc { get; set; }
        public decimal Pol_PrcRentaEsc { get; set; }
        public string Pol_CodGratif { get; set; }
        public string Ben_CodSitInv { get; set; }
        public string Ben_FecNac { get; set; }
        public string Ben_CodSexo { get; set; }
        public string Ben_CodPar { get; set; }
        public decimal Ben_PrcPension { get; set; }
        public string CodigoTope18 { get; set; }
        public string Num_Estudiante { get; set; }
        public string Ben_FecFal { get; set; }
        public decimal Remuneracion_Ini { get; set; }
        public decimal Remuneracion_Ajus { get; set; }
        public decimal Pension_Ajus { get; set; }
        public decimal Pol_MtoPrima { get; set; }
        public decimal Pol_TasaVta { get; set; }
        public decimal Pol_TasaLR { get; set; }
        public decimal Tasa_VtaProm { get; set; }
        public decimal Tasa_Equiv { get; set; }
        public decimal Reserva_Ben { get; set; }
        public decimal Reserva_GastoSep { get; set; }
        public decimal Reserva_TotPol { get; set; }
        public string Pol_Prestacion { get; set; }
        public string Cod_Estudiante { get; set; }

        #endregion

        public string FecActual { get; set; }//Variable para validar Fecha de Tasa Promedio de Mercado
        public double TipCambio { get; set; } //Variable para guardar el tipo de cambio en caso de encontrarlo
        public string Gls_Elemento { get; set; } //Variable para homologar parentesco en Carga de Excel
        public int LastPol { get; set; } //Variable para obtener la última póliza de la tabla de Tasas de Reservas.


        #region VARIABLES PARA RPT RESUMEN_RESERVAS
        public double Jubilacion_SolIndex { get; set; }
        public double Jubilacion_SolReaj { get; set; }
        public double Jubilacion_Dolar { get; set; }
        public double Invalidez_SolIndex { get; set; }
        public double Invalidez_SolReaj { get; set; }
        public double Invalidez_Dolar { get; set; }
        public double Sobrevivencia_SolIndex { get; set; }
        public double Sobrevivencia_SolReaj { get; set; }
        public double Sobrevivencia_Dolar { get; set; }

        public double Jubilacion_Tot { get; set; }
        public double Invalidez_Tot { get; set; }
        public double Sobrevivencia_Tot { get; set; }

        public double SolIndex_Tot { get; set; }
        public double SolReaj_Tot { get; set; }
        public double Dolar_Tot { get; set; }

        public string Pension { get; set; }
        public string Moneda { get; set; }
        public double Mtos_TotalesRes { get; set; }



        /*2019*/
        public double Jubilacion_SolIndex19 { get; set; }
        public double Jubilacion_SolReaj19 { get; set; }
        public double Jubilacion_Dolar19 { get; set; }
        public double Invalidez_SolIndex19 { get; set; }
        public double Invalidez_SolReaj19 { get; set; }
        public double Invalidez_Dolar19 { get; set; }
        public double Sobrevivencia_SolIndex19 { get; set; }
        public double Sobrevivencia_SolReaj19 { get; set; }
        public double Sobrevivencia_Dolar19 { get; set; }

        /*Totales*/
        public string Renta { get; set; }
        #endregion

        #region VARIABLES PARA RPT DE FLUJOS DE CARTERA
        public int FC_Mes { get; set; }
        public double FC_SIPensiones { get; set; }
        public double FC_SISepelio { get; set; }
        public double FC_SAPensiones { get; set; }
        public double FC_SASepelio { get; set; }
        public double FC_DPensiones { get; set; }
        public double FC_DSepelio { get; set; }
        public double FC_SITotales { get; set; }
        public double FC_SATotales { get; set; }
        public double FC_DTotales { get; set; }


        public string FC_NumPol { get; set; }
        public double FC_GastoSep { get; set; }
        public double FC_Titular { get; set; }
        public double FC_Conyugue { get; set; }
        public double FC_Padre { get; set; }
        public double FC_Madre { get; set; }
        public double FC_Hijo1 { get; set; }
        public double FC_Hijo2 { get; set; }
        public double FC_Hijo3 { get; set; }
        public double FC_Hijo4 { get; set; }
        public double FC_Hijo5 { get; set; }
        public double FC_Hijo6 { get; set; }
        public double FC_Hijo7 { get; set; }
        public double FC_Hijo8 { get; set; }
        public double FC_Hijo9 { get; set; }
        public double FC_Hijo10 { get; set; }
        public double FC_MtoTotal { get; set; }
        #endregion

        public string Mensaje { get; set; }
        
    }
}
