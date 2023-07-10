using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Estudio.Repository.Core.Domain
{
    public class AsignacionIntermediario
    {
        //Variables Tabla Gastos
        public string Cod_Moneda { get; set; }
        public string Reajuste { get; set; }
        public string Tipo_Pension { get; set; }
        public string Cod_Depto { get; set; }
        public decimal Mto_IniRango { get; set; }
        public decimal Mto_TerRango { get; set; }
        public decimal Mto_GasAdm { get; set; }
        public decimal Mto_GasEmi { get; set; }
        public decimal Mto_Endeuda { get; set; }
        public decimal Mto_GasCtrSup { get; set; }
        public decimal Mto_Imp { get; set; }
        public decimal Prc_Maximo { get; set; }
        public decimal TasaV { get; set; }
        public decimal TasaV_Min { get; set; }
        public decimal Prc_MinimoTir { get; set; }
        public string gastos_val { get; set; }

        //Fin tab anual
        public int vgFinTabVit_F_A { get; set; }
        public int vgFinTabTot_F_A { get; set; }
        public int vgMortalTot_F_A { get; set; }
        public int vgFinTabPar_F_A { get; set; }
        public int vgMortalPar_F_A { get; set; }
        public int vgFinTabBen_F_A { get; set; }
        public int vgMortalBen_F_A { get; set; }
        public double FinTab { get; set; }
        public int vgNumeroTotalTablas { get; set; }
        public int vgFinTabVit_M_A { get; set; }
        public int vgMortalVit_M_A { get; set; }
        public int vgFinTabTot_M_A { get; set; }
        public int vgMortalTot_M_A { get; set; }
        public int vgFinTabPar_M_A { get; set; }
        public int vgMortalPar_M_A { get; set; }
        public int vgFinTabBen_M_A { get; set; }
        public int vgMortalBen_M_A { get; set; }
        //Variables Tabla Tasa de Mercado
        public decimal PRC_MES { get; set; }
        public int vgMortalVit_F_A { get; set; }
        //For
        public int numOrdenBen { get; set; }
        public string parentescoBen { get; set; }
        public string fecNacBen { get; set; }
        public string grupFamBen { get; set; }
        public string sexoBen { get; set; }
        public string cod_sitinvBen { get; set; }
        public string dPensionBen { get; set; }
        public double porsentajeBen { get; set; }
        public double porcLegBen { get; set; }
        public string fecNacHM { get; set; }
        public string dCrecerBen { get; set; }
        public string nombreBen { get; set; }
        public double MtoFacPenElla { get; set; }
        public string cod_sitinv { get; set; }
        public string cod_Par { get; set; }
        public double PrcFacPenElla { get; set; }
        public int Num_Cor { get; set; }
        public bool bandCalculaFPE { get; set; }
        public string Cod_Tippension { get; set; }
        public string Num_Cot { get; set; }
        public string strNumCot { get; set; }
        public string strFecDev { get; set; }
        public string strTipPen { get; set; }
        public string strIndCob { get; set; }
        public string strAfp { get; set; }
        public string vgFecPro { get; set; }
        public string strFecSol { get; set; }
        public string strSexo { get; set; }
        public string strFecNac { get; set; }
        public int vgEdadActual { get; set; }
        public int vlEdad { get; set; }
        public int vlAnnoJub { get; set; }
        public int vlNumBenef { get; set; }
        public int cuentaBen { get; set; }
        public string vgPalabra { get; set; }
        public string Cod_region { get; set; }
        //Variables Tasa de Anclaje TA
        public string cod_moneda { get; set; }
        public string cod_tipreajuste { get; set; }
        public string prc_ta { get; set; }
        public double vgRentabilidadAFP { get; set; }
        public double douMonto { get; set; }
        public string douCuoMor { get; set; }
        public bool vlCalculoBono { get; set; }

        //Variables Tabla Mortalidad
        public int TblNumMort { get; set; }
        public int Num_Correlativo { get; set; }
        public string Tipo_Tabla { get; set; }
        public string Sexo { get; set; }
        public string Fecha_Ini { get; set; }
        public string Fecha_Fin { get; set; }
        public string Nombre { get; set; }
        public string Tipo_Generar { get; set; }
        public string Tipo_Periodo { get; set; }
        public int Ini_Tab { get; set; }
        public int Fin_Tab { get; set; }
        public decimal Tasa { get; set; }
        public string Oficial { get; set; }
        public string Estado { get; set; }
        public string Tipo_Movimiento { get; set; }
        public int Year_Base { get; set; }


        //Carga Variables Mortalidad
        public string GLS_NOMBRE { get; set; }
        public int NUM_CORRELATIVO { get; set; }
        public string COD_TIPTABMOR { get; set; }
        public string COD_SEXO { get; set; }
        public string COD_TIPOPER { get; set; }
        public int vgMortalVit_F { get; set; }
        public string vgPalabra_MortalVit_F { get; set; }
        public string vgPalabra_MortalVit_F_A { get; set; }
        public int vgMortalVit_M { get; set; }
        public string vgPalabra_MortalVit_M { get; set; }
        public string vgPalabra_MortalVit_M_A { get; set; }
        public int vgMortalTot_F { get; set; }
        public string vgPalabra_MortalTot_F { get; set; }
        public string vgPalabra_MortalTot_F_A { get; set; }
        public int vgMortalTot_M { get; set; }
        public string vgPalabra_MortalTot_M { get; set; }
        public string vgPalabra_MortalTot_M_A { get; set; }
        public int vgMortalPar_F { get; set; }
        public string vgPalabra_MortalPar_F { get; set; }
        public string vgPalabra_MortalPar_F_A { get; set; }
        public int vgMortalPar_M { get; set; }
        public string vgPalabra_MortalPar_M { get; set; }
        public string vgPalabra_MortalPar_M_A { get; set; }
        public int vgMortalBen_F { get; set; }
        public string vgPalabra_MortalBen_F { get; set; }
        public string vgPalabra_MortalBen_F_A { get; set; }
        public int vgMortalBen_M { get; set; }
        public string vgPalabra_MortalBen_M { get; set; }
        public string vgPalabra_MortalBen_M_A { get; set; }

        //fgBuscarMortalidad.

        public string vgBuscarMortalVit_F { get; set; }
        public int h { get; set; }
        public int i { get; set; }
        public int j { get; set; }
        public string vgs_Sexo { get; set; }
        public string vgs_Tipo { get; set; }
        public int vgs_Nro { get; set; }
        public bool vgSW { get; set; }
        public string vgFechaIniMortalVit_F { get; set; }
        public string vgFechaFinMortalVit_F { get; set; }
        public string vgIndicadorTipoMovimiento_F { get; set; }
        public int vgDinamicaAñoBase_F { get; set; }
        public int num_edad { get; set; }
        public double mto_lx { get; set; }
        public int k { get; set; }
        public double vgMto { get; set; }
        public int Cor { get; set; }
        public int iNavig { get; set; }
        public int iNmvig { get; set; }
        public int iNdvig { get; set; }
        public int iab { get; set; }
        public int imb { get; set; }
        public int idb { get; set; }
        public double xq { get; set; }
        public double factor { get; set; }
        public string vgBuscarMortalTot_M { get; set; }
        public string vgFechaIniMortalTot_F { get; set; }



        //Variables Tbl Beneficiarios
        public int Num_Orden { get; set; }
        public string Cod_Parentesco { get; set; }
        public string Fec_Nac_Ben { get; set; }
        public string Cod_GruFam { get; set; }
        public string Cod_Sexo { get; set; }
        public string Cod_SitInv { get; set; }
        public string Fec_SitInv { get; set; }
        public string Cod_CauInv { get; set; }
        public string Cod_DerPen { get; set; }
        public decimal Prc_Pension { get; set; }
        public decimal Prc_PensionLeg { get; set; }
        public string Fec_Nac_HM { get; set; }
        public string Cod_DerCre { get; set; }
        public string Fec_FalBen { get; set; }
        public int Cod_TipoIden { get; set; }
        public string Num_Iden { get; set; }
        public string Gls_NomBen { get; set; }
        public string Gls_NomSegBen { get; set; }
        public string Gls_PatBen { get; set; }
        public string Gls_MatBen { get; set; }


        //Varbiales para ejecución de rutina.
        #region Variables de rutina
        public string CUSPP { get; set; }
        public string Nombres { get; set; }
        public string ApellidoPaterno { get; set; }
        public string ApellidoMaterno { get; set; }
        public string FechaNacimientoStr { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public string FechaDevengueStr { get; set; }
        public DateTime FechaDevengue { get; set; }
        public decimal Cic { get; set; }
        public string LugarCita { get; set; }
        public string FechaEstudioStr { get; set; }
        public DateTime FechaEstudio { get; set; }
        public decimal GastoSepelio { get; set; }
        public int IdTipoDocumento { get; set; }
        public string Documento { get; set; }
        public string CodigoPension { get; set; }
        public string ClaveSexo { get; set; }
        public decimal TipoCambio { get; set; }
        public string Afp { get; set; }
        #endregion


        //Variables para lista de beneficiarios.
        public decimal Limite_Edad { get; set; } //Limite de edad 

        public string Ind_Cob { get; set; }
        public string Fec_DevSolStr { get; set; }
        public DateTime Fec_DevSol { get; set; }
        public int NumArch { get; set; }

        #region Variables para Excel de Solicitudes Calculadas y No Calculadas.
        public int MesesDif { get; set; }
        public int MesesGar { get; set; }
        public int Num_Operacion { get; set; }
        public string Tipo_Renta { get; set; }
        public string Modalidad { get; set; }
        public int CobCony { get; set; }
        public string Cod_DerGra { get; set; }
        public double Prc_RentaEsc { get; set; }
        public double Prc_TasaRPRT { get; set; }
        public double Mto_PensionRT { get; set; }
        public double Prima_Unica { get; set; }
        public double Prc_PerCon { get; set; }
        public double Prc_CorCom { get; set; }
        public string Intermediario { get; set; }
        public string Ind_Mej { get; set; }
        public string Gls_Region { get; set; }
        public double CIC { get; set; }
        public string Cod_Rechazo { get; set; }
        public string Error_Descrip { get; set; }
        public string DepartamentoAsignado { get; set; }
        public string Asesor { get; set; }
        public string Supervisor { get; set; }
        #endregion

    }
}
