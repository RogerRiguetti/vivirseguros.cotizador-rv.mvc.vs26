using Estudio.Repository.Core.Domain;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Estudio.Repository.Persistence.Repositories
{
    public class CalcularAsignacionIntermediarioRepository
    {
        public string cod_Par = "";
        public int Num_Cor = 0;
        public double MtoFacPenElla = 0.0;
        public double PrcFacPenElla = 0.0;
        public int vgNumeroTotalTablas = 0;
        public string vgTipoPeriodoAnual = "A";

        
        public static bool ValidaIPC(DateTime fecha)
        {
            try
            {
                bool respuesta = true;
                var parameters = new List<SqlParameter>();
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pClave", SqlDbType.VarChar, "VALIDAIPC", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@vFecIni", SqlDbType.VarChar, fecha.ToString("yyyyMMdd"), ParameterDirection.Input));
                if (VCEDBContext<DataTable>.CallStoreProcedureDt(StoredProcedures.CO_CatalogosCalcularAsignacionIntermediario, parameters).Rows.Count == 0)
                {
                    respuesta = false;
                }
                return respuesta;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }


        public List<SolicitudesCotizacion> obtenerDatos(int numArch)
        {
            List<SolicitudesCotizacion> _SolicitudCotizacion = new List<SolicitudesCotizacion>();
            try
            {
                var parameters = new List<SqlParameter>();
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pClave", SqlDbType.VarChar, "OBTENERDATOS", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@vNumArchivo", SqlDbType.Int, numArch, ParameterDirection.Input));
                _SolicitudCotizacion = VCEDBContext<SolicitudesCotizacion>.CallStoreProcedure(StoredProcedures.CO_CatalogosCalcularAsignacionIntermediario, parameters, x => new SolicitudesCotizacion
                {
                    strNumCot = x.GetString(0),
                    intNumOpe = Convert.ToInt32(x.GetDecimal(1)),
                    strCussp = x.GetString(2),
                    strMtoCIC = x.GetDecimal(3).ToString(),
                    strTipoDoc = x.GetString(4),
                    strNumDoc = x.GetString(5),
                    strNom = x.GetString(6),
                    strLugCita = x.GetString(7).Split('.')[0],
                    numAgente = x.GetInt32(8)
                }).ToList();

                return _SolicitudCotizacion;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Carga de Archivo de Solicitudes");
                throw;
            }
        }


        public string datosCabeza(int numArch)
        {
            SolicitudesCotizacion _SolicitudCotizacion = new SolicitudesCotizacion();
            try
            {
                string nombre = "";
                var parameters = new List<SqlParameter>();
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pClave", SqlDbType.VarChar, "DATOSCABEZA", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@vNumArchivo", SqlDbType.Int, numArch, ParameterDirection.Input));

                _SolicitudCotizacion = VCEDBContext<SolicitudesCotizacion>.CallStoreProcedure(StoredProcedures.CO_CatalogosCalcularAsignacionIntermediario, parameters, x => new SolicitudesCotizacion
                {
                    strNom = x.GetString(0)
                }).FirstOrDefault();
                if (_SolicitudCotizacion != null)
                {
                    nombre = _SolicitudCotizacion.strNom;
                }
                return nombre;

            }
            catch (Exception)
            {
                return "";
                throw;
            }
        }

        public List<SolicitudesCotizacion> asesores()
        {
            List<SolicitudesCotizacion> _SolicitudCotizacion = new List<SolicitudesCotizacion>();
            try
            {
                var parameters = new List<SqlParameter>();
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pClave", SqlDbType.VarChar, "ASESORES", ParameterDirection.Input));
                _SolicitudCotizacion = VCEDBContext<SolicitudesCotizacion>.CallStoreProcedure(StoredProcedures.CO_CatalogosCalcularAsignacionIntermediario, parameters, x => new SolicitudesCotizacion
                {
                    idAsesor = x.GetInt32(0),
                    nomAsesor = x.GetString(1)
                }).ToList();

                return _SolicitudCotizacion;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Carga de Archivo de Solicitudes");
                throw;
            }
        }

        public static bool ValidaTM(DateTime fecha)
        {
            try
            {
                bool respuesta = true;
                var parameters = new List<SqlParameter>();
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pClave", SqlDbType.VarChar, "VALIDATM", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@vFecIniYear", SqlDbType.VarChar, fecha.Year, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@vFecIniMes", SqlDbType.VarChar, fecha.Month, ParameterDirection.Input));
                if (VCEDBContext<DataTable>.CallStoreProcedureDt(StoredProcedures.CO_CatalogosCalcularAsignacionIntermediario, parameters).Rows.Count == 0)
                {
                    respuesta = false;
                }
                return respuesta;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }


        public static string fgBuscarIndCalcularModSoles()
        {
            try
            {
                string respuesta = "N";
                var parameters = new List<SqlParameter>();
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pClave", SqlDbType.VarChar, "VALIDASOLES", ParameterDirection.Input));

                foreach (DataRow row in VCEDBContext<DataTable>.CallStoreProcedureDt(StoredProcedures.CO_CatalogosCalcularAsignacionIntermediario, parameters).Rows)
                {
                    respuesta = row[0].ToString();
                }
                return respuesta;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }


        public static List<string> BuscarValReajuste(DateTime fechaActual, string moneda)
        {
            try
            {
                List<string> respuesta = new List<string>(); ;
                var parameters = new List<SqlParameter>();
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pClave", SqlDbType.VarChar, "VALIDAREAJUSTE", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@vFecIni", SqlDbType.VarChar, fechaActual.ToString("yyyyMMdd"), ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@vMoneda", SqlDbType.VarChar, moneda, ParameterDirection.Input));

                foreach (DataRow row in VCEDBContext<DataTable>.CallStoreProcedureDt(StoredProcedures.CO_CatalogosCalcularAsignacionIntermediario, parameters).Rows)
                {
                    respuesta.Add(row[0].ToString());
                    respuesta.Add(row[1].ToString());
                    respuesta.Add(row[2].ToString());
                }
                return respuesta;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }


        public List<SolicitudesCotizacion> Busca_Num_Cot(int num_Archivo)
        {
            try
            {//'BUSCA LOS NROS DE COTIZACION SIN RECHAZO
                var parameters = new List<SqlParameter>();
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pClave", SqlDbType.VarChar, "NUMCOTIZACION", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@vFecIni", SqlDbType.VarChar, num_Archivo, ParameterDirection.Input));
                return VCEDBContext<SolicitudesCotizacion>.CallStoreProcedure(StoredProcedures.CO_CatalogosLimiteCotizacionInicial, parameters, x => new SolicitudesCotizacion
                {
                    intNumOpe = x.GetInt32(0),
                    codRechazo = x.GetInt32(1)
                }).ToList();


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }


        public static bool ValidaDeptoEstandar(string v, DateTime fecha)
        {
            try
            {
                bool respuesta = true;
                var parameters = new List<SqlParameter>();
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pClave", SqlDbType.VarChar, "VALIDADEPEST", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@vFecIni", SqlDbType.VarChar, fecha.ToString("yyyyMMdd"), ParameterDirection.Input));
                if (VCEDBContext<DataTable>.CallStoreProcedureDt(StoredProcedures.CO_CatalogosCalcularAsignacionIntermediario, parameters).Rows.Count == 0)
                {
                    respuesta = false;
                }
                return respuesta;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }


        /// <summary>
        /// José Hernández Alvarado
        /// 24-09-2018
        /// Devuelve lista con valores de tabla de mortalidad.
        /// </summary>
        /// <param name="iPeriodo">ID del periodo</param>
        /// <returns>Lista con valores para arreglo de tabla de mortalidad.</returns>
        public List<AsignacionIntermediario> Tbl_Mortalidad(string iPeriodo)
        {
            try
            {
                AsignacionIntermediario valores = new AsignacionIntermediario();

                var parameters = new List<SqlParameter>();
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pClave", SqlDbType.VarChar, "NUMCORRELATIVO", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pPeriodo", SqlDbType.VarChar, iPeriodo, ParameterDirection.Input));
                valores = VCEDBContext<AsignacionIntermediario>.CallStoreProcedure(StoredProcedures.CO_CatalogosCalcularAsignacionIntermediario, parameters, x => new AsignacionIntermediario
                {
                    TblNumMort = x.GetInt32(0)
                }).FirstOrDefault();

                vgNumeroTotalTablas = valores.TblNumMort;

                List<AsignacionIntermediario> valoresTbl = new List<AsignacionIntermediario>();
                if (vgNumeroTotalTablas != 0)
                {
                    //pepe
                    var parameters2 = new List<SqlParameter>();
                    parameters2.Add(VCEDBContext<RowAffected>.AddParams("@pClave", SqlDbType.VarChar, "TBLMORTALIDAD2", ParameterDirection.Input));
                    parameters2.Add(VCEDBContext<RowAffected>.AddParams("@pPeriodo", SqlDbType.VarChar, iPeriodo, ParameterDirection.Input));
                    valoresTbl = VCEDBContext<AsignacionIntermediario>.CallStoreProcedure(StoredProcedures.CO_CatalogosCalcularAsignacionIntermediario, parameters2, x => new AsignacionIntermediario
                    {
                        Num_Correlativo = x.GetInt32(0),
                        Tipo_Tabla = x.GetString(1),
                        Sexo = x.GetString(2),
                        Fecha_Ini = x.GetString(3),
                        Fecha_Fin = x.GetString(4),
                        Nombre = x.GetString(5),
                        Tipo_Generar = x.GetString(6),
                        Tipo_Periodo = x.GetString(7),
                        Ini_Tab = x.GetInt32(8),
                        Fin_Tab = x.GetInt32(9),
                        Tasa = x.GetDecimal(10),
                        Estado = x.GetString(11),
                        Oficial = x.GetString(12),
                        Tipo_Movimiento = x.GetString(13),
                        Year_Base = x.GetInt32(14)
                    }).ToList();
                }
                return valoresTbl;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }


        /// <summary>
        /// José Hernández Alvarado.
        /// 25-09-2018
        /// Devuelve lista con registros para tabla gastos.
        /// </summary>
        /// <param name="fechaCal">Fecha actual del sistema.</param>
        /// <param name="tipoCotizar">Valor de Tipo de Cotización.</param>
        /// <param name="usuario">Usuario logeado en el sistema.</param>
        /// <returns>Lista con registros de la base de datos.</returns>
        public List<AsignacionIntermediario> Tbl_Gastos(DateTime fechaCal, string tipoCotizar, string usuario)
        {
            try
            {
                List<AsignacionIntermediario> TBL_TMP1 = new List<AsignacionIntermediario>();
                string strFecha = fechaCal.ToString("yyyyMMdd");

                //Query para primer consulta.
                #region quer1 
                string query = "SELECT G.COD_MONEDA,G.COD_TIPREAJUSTE,TV.COD_TIPPENSION,T.COD_REGION";

                if (tipoCotizar == "C") { query = query + ",RP.MTO_INIRANGO,RP.MTO_TERRANGO"; }
                query = query + ",G.MTO_GASADM,G.MTO_GASEMI,G.PRC_ENDEUDA,G.PRC_GASCTRSUP,I.MTO_IMP,P.PRC_MAXIMO,TV.PRC_MAXIMO AS TASAV,TV.PRC_MINIMO AS TASAVMIN,T.PRC_MINIMO " +
                                "FROM PT_TVAL_GASTO G, MA_TVAL_IMPUESTO I, PT_TVAL_MINMAXPER_INI P, PT_TVAL_MINMAXTAS_INI TV, PT_TVAL_MINMAXTIR_INI T";

                if (tipoCotizar == "C") { query = query + ", PT_TMAE_GENRANPRI RP"; }
                query = query + " WHERE G.COD_MONEDA=P.COD_MONEDA AND P.COD_MONEDA = TV.COD_MONEDA AND TV.COD_MONEDA = T.COD_MONEDA AND G.COD_TIPREAJUSTE = P.COD_TIPREAJUSTE AND P.COD_TIPREAJUSTE = TV.COD_TIPREAJUSTE " +
                                "AND TV.COD_TIPREAJUSTE = T.COD_TIPREAJUSTE AND G.FEC_INIVIG <= '" + strFecha + "' AND G.FEC_TERVIG >= '" + strFecha + "'  AND I.FEC_INIIMP <= '" + strFecha + "' AND I.FEC_TERIMP >= '" + strFecha + "' AND P.FEC_INIMINMAX <= '" + strFecha + "' AND P.FEC_TERMINMAX >= '" + strFecha + "'";

                if (tipoCotizar == "M") { query = query + " AND TV.COD_TIPPENSION = '' "; }
                query = query + " AND TV.FEC_INIMINMAX <= '" + strFecha + "' AND TV.FEC_TERMINMAX >= '" + strFecha + "' AND T.FEC_INIMINMAX <= '" + strFecha + "' AND T.FEC_TERMINMAX >= '" + strFecha + "' AND T.COD_REGION = P.COD_REGION AND T.COD_REGION = TV.COD_REGION";

                if (tipoCotizar == "C") { query = query + " AND TV.COD_TIPPENSION = RP.COD_TIPPENSION"; }
                query = query + " ORDER BY G.COD_MONEDA,G.COD_TIPREAJUSTE,TV.COD_TIPPENSION,T.COD_REGION";

                if (tipoCotizar == "C") { query = query + ",RP.MTO_INIRANGO"; }

                #endregion 

                TBL_TMP1 = SRVDBContext<AsignacionIntermediario>.CallSelectStatement(query, x => new AsignacionIntermediario
                {
                    Cod_Moneda = x.GetString(0),
                    Reajuste = x.GetString(1),
                    Tipo_Pension = x.GetString(2),
                    Cod_Depto = x.GetString(3),
                    Mto_IniRango = x.GetDecimal(4),
                    Mto_TerRango = x.GetDecimal(5),
                    Mto_GasAdm = x.GetDecimal(6),
                    Mto_GasEmi = x.GetDecimal(7),
                    Mto_Endeuda = x.GetDecimal(8),
                    Mto_GasCtrSup = x.GetDecimal(9),
                    Mto_Imp = x.GetDecimal(10),
                    Prc_Maximo = x.GetDecimal(11),
                    TasaV = x.GetDecimal(12),
                    TasaV_Min = x.GetDecimal(13),
                    Prc_MinimoTir = x.GetDecimal(14)
                }).ToList();


                string query2 = "";

                for (int i = 0; i < TBL_TMP1.Count; i++)
                {
                    List<AsignacionIntermediario> Gastos_Tmp = new List<AsignacionIntermediario>();
                    query2 = "SELECT COD_MONEDA FROM PT_TTMP_GASTO WHERE COD_TIPCOT = 'C' AND COD_USUARIO = '" + usuario + "' AND COD_MONEDA = '" + TBL_TMP1[i].Cod_Moneda + "' " +
                        "AND COD_TIPPENSION = '" + TBL_TMP1[i].Tipo_Pension + "' AND MTO_PRIMIN = " + TBL_TMP1[i].Mto_IniRango + " AND COD_REGION = '" + TBL_TMP1[i].Cod_Depto + "' AND COD_TIPREAJUSTE = '" + TBL_TMP1[i].Reajuste + "'";

                    Gastos_Tmp = SRVDBContext<AsignacionIntermediario>.CallSelectStatement(query2, x => new AsignacionIntermediario
                    {
                        gastos_val = x.GetString(0)
                    }).ToList();


                    var parameters = new List<SqlParameter>();
                    parameters.Add(VCEDBContext<RowAffected>.AddParams("@vMoneda", SqlDbType.VarChar, TBL_TMP1[i].Cod_Moneda, ParameterDirection.Input));
                    parameters.Add(VCEDBContext<RowAffected>.AddParams("@pReajuste", SqlDbType.VarChar, TBL_TMP1[i].Reajuste, ParameterDirection.Input));
                    parameters.Add(VCEDBContext<RowAffected>.AddParams("@pTipoCot", SqlDbType.VarChar, "C", ParameterDirection.Input));
                    parameters.Add(VCEDBContext<RowAffected>.AddParams("@pUsuario", SqlDbType.VarChar, usuario, ParameterDirection.Input));
                    parameters.Add(VCEDBContext<RowAffected>.AddParams("@pTipoPension", SqlDbType.VarChar, TBL_TMP1[i].Tipo_Pension, ParameterDirection.Input));
                    parameters.Add(VCEDBContext<RowAffected>.AddParams("@pMtoPriMin", SqlDbType.Decimal, TBL_TMP1[i].Mto_IniRango, ParameterDirection.Input));
                    parameters.Add(VCEDBContext<RowAffected>.AddParams("@pMtoPriMax", SqlDbType.Decimal, TBL_TMP1[i].Mto_TerRango, ParameterDirection.Input));
                    parameters.Add(VCEDBContext<RowAffected>.AddParams("@pPrcGasCtr", SqlDbType.Decimal, TBL_TMP1[i].Mto_GasCtrSup, ParameterDirection.Input));
                    parameters.Add(VCEDBContext<RowAffected>.AddParams("@pGasAdm", SqlDbType.Decimal, TBL_TMP1[i].Mto_GasAdm, ParameterDirection.Input));
                    parameters.Add(VCEDBContext<RowAffected>.AddParams("@pPrcEndeuda", SqlDbType.Decimal, TBL_TMP1[i].Mto_Endeuda, ParameterDirection.Input));
                    parameters.Add(VCEDBContext<RowAffected>.AddParams("@pMtoImp", SqlDbType.Decimal, TBL_TMP1[i].Mto_Imp, ParameterDirection.Input));
                    parameters.Add(VCEDBContext<RowAffected>.AddParams("@pMtoGasEmi", SqlDbType.Decimal, TBL_TMP1[i].Mto_GasEmi, ParameterDirection.Input));
                    parameters.Add(VCEDBContext<RowAffected>.AddParams("@pTasaTir", SqlDbType.Decimal, TBL_TMP1[i].Prc_MinimoTir, ParameterDirection.Input));
                    parameters.Add(VCEDBContext<RowAffected>.AddParams("@pTasaMax", SqlDbType.Decimal, TBL_TMP1[i].TasaV, ParameterDirection.Input));
                    parameters.Add(VCEDBContext<RowAffected>.AddParams("@pTasaMin", SqlDbType.Decimal, TBL_TMP1[i].TasaV_Min, ParameterDirection.Input));
                    parameters.Add(VCEDBContext<RowAffected>.AddParams("@pPrcPerMax", SqlDbType.Decimal, TBL_TMP1[i].Prc_Maximo, ParameterDirection.Input));
                    parameters.Add(VCEDBContext<RowAffected>.AddParams("@pCodRegion", SqlDbType.VarChar, TBL_TMP1[i].Cod_Depto, ParameterDirection.Input));
                    if (Gastos_Tmp.Count == 0)
                    {
                        parameters.Add(VCEDBContext<RowAffected>.AddParams("@pClave", SqlDbType.VarChar, "INSERTGASTOS", ParameterDirection.Input));
                        VCEDBContext<DataTable>.CallStoreProcedureDt(StoredProcedures.CO_CatalogosCalcularAsignacionIntermediario, parameters);
                    }
                    else
                    {
                        parameters.Add(VCEDBContext<RowAffected>.AddParams("@pClave", SqlDbType.VarChar, "UPDATEGASTOS", ParameterDirection.Input));
                        VCEDBContext<DataTable>.CallStoreProcedureDt(StoredProcedures.CO_CatalogosCalcularAsignacionIntermediario, parameters);
                    }
                }

                var parameters2 = new List<SqlParameter>();
                parameters2.Add(VCEDBContext<RowAffected>.AddParams("@pClave", SqlDbType.VarChar, "BUSCATBLGASTOS", ParameterDirection.Input));
                parameters2.Add(VCEDBContext<RowAffected>.AddParams("@pUsuario", SqlDbType.VarChar, usuario, ParameterDirection.Input));
                return VCEDBContext<AsignacionIntermediario>.CallStoreProcedure(StoredProcedures.CO_CatalogosCalcularAsignacionIntermediario, parameters2, x => new AsignacionIntermediario
                {
                    Cod_Moneda = x.GetString(1),
                    Reajuste = x.GetString(2),
                    Tipo_Pension = x.GetString(5),
                    Cod_Depto = x.GetString(0),
                    Mto_IniRango = x.GetDecimal(6),
                    Mto_TerRango = x.GetDecimal(7),
                    Mto_GasCtrSup = x.GetDecimal(8),
                    Mto_GasAdm = x.GetDecimal(9),
                    Mto_Endeuda = x.GetDecimal(10),
                    Mto_Imp = x.GetDecimal(11),
                    Mto_GasEmi = x.GetDecimal(12),
                    Prc_MinimoTir = x.GetDecimal(14),
                    TasaV = x.GetDecimal(15),
                    TasaV_Min = x.GetDecimal(16),
                    Prc_Maximo = x.GetDecimal(17)
                }).ToList();


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public void insertRutina(List<List<beResultados>> ResultadoCot, string usuario)
        {
            DateTime fecha = DateTime.Now;
            for (int i = 0; i < ResultadoCot.Count; i++)
            {
                foreach (var item in ResultadoCot[i]) { 
                    try
                    {
                        string query2 = "update PT_TMAE_detcotizacion set (mto_ajusteipc=" + item.MTO_AJUSTEIPC + ", mto_ctaindafp=" + item.MTO_CTAINDAFP +
                                         ", mto_pension=" + item.MTO_PENSION + ", mto_priunidif=" + item.MTO_PRIUNIDIF + ", mto_rentatmpafp=" + item.MTO_RENTATMPAFP +
                                         ", mto_resmat=" + item.MTO_RESMAT + ", prc_percon=" + item.PRC_PERCON + ",prc_tasatce=" + item.PRC_TASATCE + ",cod_rechazo=" + item.Cod_Rechazo +
                                         ",FEC_CALCULO='" + fecha.ToString("yyyyMMdd") + ",COD_USUARIOMODI='" + usuario + "',FEC_MODI='" + fecha.ToString("yyyyMMdd") + "',HOR_MODI='" + fecha.ToString("hhmmss") +
                                         ", prc_tasatir=" + item.PRC_TASATIR + ", prc_tasavta=" + item.PRC_TASAVTA + 
                                         " where num_correlativo=" + item.NUM_CORRELATIVO + " and num_operacion="+item.NUM_COTESTUDIO;


                        SRVDBContext<beResultados>.CallSelectStatement(query2, x => new beResultados
                        {

                        }).FirstOrDefault();
                    }
                    catch (Exception ex)
                    {

                    }
            }
            }
        }
        public List<AsignacionIntermediario> LLenar_dt_TA(string strFecCal)
        {
            List<AsignacionIntermediario> dtableTA = new List<AsignacionIntermediario>();
            try
            {
                string query = "SELECT COD_MONEDA, COD_TIPREAJUSTE, ISNULL(PRC_TASA,0.0) AS PRC_TASA " +
                                "FROM MA_TVAL_TASAANCLAJE " +
                                "WHERE FEC_INIVIG<='" + strFecCal + "' AND FEC_TERVIG>='" + strFecCal + "'";

                dtableTA = SRVDBContext<AsignacionIntermediario>.CallSelectStatement(query, x => new AsignacionIntermediario
                {
                    cod_moneda = x.GetString(0),
                    cod_tipreajuste = x.GetString(1),
                    prc_ta = x.GetDecimal(2).ToString()
                }).ToList();

                return dtableTA;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ha ocurrido un error al llenar los Parámetros de Tasa de Anclaje.");
                return dtableTA;
                throw;
            }
        }


        public AsignacionIntermediario flDatosDetalle(string strNumCot)
        {
            try
            {
                AsignacionIntermediario datosAI = new AsignacionIntermediario();
                string query = "SELECT DISTINCT C.FEC_DEV,C.COD_TIPPENSION,C.IND_COB, C.COD_AFP" +
                               " FROM PT_TMAE_DETCOTIZACION D, PT_TMAE_COTIZACION C WHERE" +
                               " C.NUM_COT ='" + strNumCot + "' AND C.NUM_COT=D.NUM_COT";

                datosAI = SRVDBContext<AsignacionIntermediario>.CallSelectStatement(query, x => new AsignacionIntermediario
                {
                    strFecDev = x.GetString(0),
                    strTipPen = x.GetString(1),
                    strIndCob = x.GetString(2),
                    strAfp = x.GetString(3)
                }).FirstOrDefault();
                return datosAI;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }


        public AsignacionIntermediario flBusDatBen(string strNumCot)
        {
            try
            {
                AsignacionIntermediario datosAI = new AsignacionIntermediario();
                string query = "SELECT COD_SEXO,FEC_NACBEN" +
                               " FROM PT_TMAE_COTBEN WHERE" +
                               " NUM_COT ='" + strNumCot + "' AND COD_PAR='99'";

                datosAI = SRVDBContext<AsignacionIntermediario>.CallSelectStatement(query, x => new AsignacionIntermediario
                {
                    strSexo = x.GetString(0),
                    strFecNac = x.GetString(1)
                }).FirstOrDefault();
                return datosAI;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }


        public AsignacionIntermediario flCalculaDatos(string strFecNac, string strSexo, string strNumCot, string strAfp, string strFecDev)
        {
            AsignacionIntermediario datosAI = new AsignacionIntermediario();
            string datFec;
            DateTime fecha = DateTime.Now;
            int anioActual = int.Parse(fecha.ToString("yyyy"));
            datFec = strFecNac.Substring(0, 4);
            int vgEdadActual = anioActual - int.Parse(datFec);

            if (strSexo == "F")
            {
                datosAI.vlEdad = 60 - vgEdadActual;
            }
            else
            {
                if (strSexo == "M")
                {
                    datosAI.vlEdad = 65 - vgEdadActual;
                } else
                {
                    return datosAI;
                }
            }

            if (datosAI.vlEdad > 0)
            {
                datosAI.vlAnnoJub = anioActual + datosAI.vlEdad;
            }
            else
            {
                datosAI.vlAnnoJub = anioActual;
            }
            datosAI.vlNumBenef = cuentasBen(strNumCot);
            datosAI.vgPalabra = strAfp;
            datosAI.vgRentabilidadAFP = valor_Rentabilidad(datosAI.vgPalabra, "AF", strFecDev); //BUSCA RENTABILIDAD DE LA AFP
            datosAI.douCuoMor = valor_Cuota_Mortuoria(fecha.ToString("yyyyMMdd"), "NS").ToString("0.00");//Agregar valor de cuota mortuoria en soles

            guardarDatos(datosAI.vlAnnoJub, datosAI.vlNumBenef, strNumCot);
            guardarDatosDetalle(datosAI.vgRentabilidadAFP, double.Parse(datosAI.douCuoMor), strNumCot);

            return datosAI;
        }


        public int cuentasBen(string strNumCot)
        {
            try
            {
                AsignacionIntermediario _AsignacionDeIntermediarioa = new AsignacionIntermediario();
                string query = "select count(num_orden)as num from pt_tmae_cotben " +
                               "where NUM_COT='" + strNumCot + "'";

                _AsignacionDeIntermediarioa = SRVDBContext<AsignacionIntermediario>.CallSelectStatement(query, x => new AsignacionIntermediario
                {
                    cuentaBen = x.GetInt32(0)
                }).FirstOrDefault();

                return _AsignacionDeIntermediarioa.cuentaBen;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }


        public double valor_Rentabilidad(string strAfp, string tabla, string ifecha)
        {
            try
            {
                AsignacionIntermediario _AsignacionDeIntermediarioa = new AsignacionIntermediario();
                string query = "select MTO_ELEMENTO from MA_TPAR_TABCODVIG " +
                               "where COD_TABLA='" + tabla + "' AND COD_ELEMENTO='" + strAfp + "' and FEC_INIVIG <= '" + ifecha + "' and FEC_TERVIG >= '" + ifecha + "'";

                _AsignacionDeIntermediarioa = SRVDBContext<AsignacionIntermediario>.CallSelectStatement(query, x => new AsignacionIntermediario
                {
                    douMonto = Convert.ToDouble(x.GetDecimal(0))
                }).FirstOrDefault();

                return _AsignacionDeIntermediarioa.douMonto;
            }
            catch (Exception ex)
            {
                Console.WriteLine("El porcentaje de Rentabilidad AFP no se encuentra registrado");
                throw;
            }
        }


        public double valor_Cuota_Mortuoria(string strfecha, string moneda)
        {
            try
            {
                AsignacionIntermediario _AsignacionDeIntermediarioa = new AsignacionIntermediario();
                string query = "select MTO_CUOMOR from MA_TVAL_CUOMOR " +
                               "where COD_MONEDA='" + moneda + "' and FEC_INICUOMOR <= '" + strfecha + "' and FEC_TERCUOMOR >= '" + strfecha + "'";

                _AsignacionDeIntermediarioa = SRVDBContext<AsignacionIntermediario>.CallSelectStatement(query, x => new AsignacionIntermediario
                {
                    douMonto = Convert.ToDouble(x.GetDecimal(0))
                }).FirstOrDefault();

                return _AsignacionDeIntermediarioa.douMonto;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }


        public void guardarDatos(int vlAnnoJub, int vlNumBenef, string numCot)
        {
            try
            {
                AsignacionIntermediario _AsignacionDeIntermediarioa = new AsignacionIntermediario();
                string query = "update pt_tmae_cotizacion set " +
                               "num_annojub='" + vlAnnoJub + "', num_cargas = '" + vlNumBenef + "' where NUM_COT = '" + numCot + "'";

                _AsignacionDeIntermediarioa = SRVDBContext<AsignacionIntermediario>.CallSelectStatement(query, x => new AsignacionIntermediario
                { }).FirstOrDefault();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }


        public void guardarDatosDetalle(double rentabilidadAfp, double CuoMor, string numCot)
        {
            try
            {
                AsignacionIntermediario _AsignacionDeIntermediarioa = new AsignacionIntermediario();
                string query = "update pt_tmae_detcotizacion set " +
                               "prc_rentaafp='" + Convert.ToDecimal(rentabilidadAfp) + "', prc_rentaafpori = '" + Convert.ToDecimal(rentabilidadAfp) + "', mto_cuomor = '" + CuoMor +
                               "' where num_cot = '" + numCot + "'";

                _AsignacionDeIntermediarioa = SRVDBContext<AsignacionIntermediario>.CallSelectStatement(query, x => new AsignacionIntermediario
                { }).FirstOrDefault();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }


        public List<AsignacionIntermediario> Tbl_TM()
        {
            try
            {
                List<AsignacionIntermediario> TBL_TMP1 = new List<AsignacionIntermediario>();
                string vlMes = DateTime.Now.Month.ToString();
                string vlAno = DateTime.Now.Year.ToString();

                var parameters = new List<SqlParameter>();
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pClave", SqlDbType.VarChar, "TM", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pValMes", SqlDbType.VarChar, "PRC_MES" + vlMes, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pValAno", SqlDbType.VarChar, vlAno.ToString(), ParameterDirection.Input));
                return VCEDBContext<AsignacionIntermediario>.CallStoreProcedure(StoredProcedures.CO_CatalogosCalcularAsignacionIntermediario, parameters, x => new AsignacionIntermediario
                {
                    Cod_Moneda = x.GetString(0),
                    Reajuste = x.GetString(1),
                    PRC_MES = x.GetDecimal(2)
                }).ToList();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }


        public bool calculaFPE(string strNumCot)
        {
            try
            {
                AsignacionIntermediario datosAI = new AsignacionIntermediario();
                string query = "SELECT D.NUM_CORRELATIVO,D.COD_COBERCON,B.COD_PAR, B.COD_SEXO,B.COD_SITINV" +
                               " FROM PT_TMAE_DETCOTIZACION D, PT_TMAE_COTBEN B" +
                               " WHERE D.NUM_COT ='" + strNumCot + "' AND D.COD_COBERCON <> '0' AND D.NUM_COT=B.NUM_COT" +
                               " AND (B.COD_PAR='11' OR B.COD_PAR='10' OR B.COD_PAR='20' OR B.COD_PAR='21')";

                datosAI = SRVDBContext<AsignacionIntermediario>.CallSelectStatement(query, x => new AsignacionIntermediario
                {
                    Num_Cor = x.GetInt32(0),
                    strIndCob = x.GetString(1),
                    cod_Par = x.GetString(2),
                    Sexo = x.GetString(3),
                    cod_sitinv = x.GetString(4)
                }).FirstOrDefault();
                if (datosAI != null)
                {
                    Num_Cor = datosAI.Num_Cor;
                    cod_Par = datosAI.cod_Par;
                    DateTime fecha = DateTime.Now;
                    PrcFacPenElla = valor_PrcFacPenElla(datosAI.strIndCob);
                    MtoFacPenElla = datosAI.PrcFacPenElla / valor_MtoFacPenElla(datosAI.cod_Par, datosAI.Sexo, datosAI.cod_sitinv, fecha.ToString("yyyyMMdd"));
                }
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }


        public double valor_PrcFacPenElla(string iCod)
        {
            try
            {
                AsignacionIntermediario datosAI = new AsignacionIntermediario();
                datosAI.bandCalculaFPE = false;
                string query = "SELECT MTO_COBERCON FROM MA_TPAR_COBERCON WHERE COD_COBERCON ='" + iCod + "'";

                datosAI = SRVDBContext<AsignacionIntermediario>.CallSelectStatement(query, x => new AsignacionIntermediario
                {
                    douMonto = Convert.ToDouble(x.GetDecimal(0))
                }).FirstOrDefault();

                return datosAI.douMonto;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }


        public double valor_MtoFacPenElla(string par, string sexo, string sitiv, string fecha)
        {
            try
            {
                AsignacionIntermediario datosAI = new AsignacionIntermediario();
                datosAI.bandCalculaFPE = false;
                string query = "SELECT PRC_PENSION FROM MA_TVAL_PORPAR WHERE COD_PAR ='" + par + "'" +
                               " AND COD_SITINV='" + sitiv + "' AND COD_SEXO='" + sexo + "' AND FEC_INIVIGPOR <= '" + fecha + "' AND FEC_TERVIGPOR >= '" + fecha + "'";

                datosAI = SRVDBContext<AsignacionIntermediario>.CallSelectStatement(query, x => new AsignacionIntermediario
                {
                    douMonto = Convert.ToDouble(x.GetDecimal(0))
                }).FirstOrDefault();

                return datosAI.douMonto;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }


        public List<AsignacionIntermediario> llenar_Dt_Ben_Calculo(string strNumCot, string cod_Tippension, string vgTipoPensionSob)
        {
            try
            {
                List<AsignacionIntermediario> datosAI = new List<AsignacionIntermediario>();
                string query = "SELECT NUM_ORDEN,COD_PAR,FEC_NACBEN,COD_GRUFAM,COD_SEXO,COD_SITINV,ISNULL(COD_DERPEN,'99') AS COD_DERPEN," +
                                " ISNULL(PRC_PENSION, '0') AS PRC_PENSION, ISNULL(PRC_PENSIONLEG,'0') AS PRC_PENSIONLEG, ISNULL(FEC_NACHM,'') AS FEC_NACHM," +
                                " ISNULL(COD_DERCRE,'N') AS COD_DERCRE, ISNULL(GLS_NOMBEN,'') AS GLS_NOMBEN" +
                                " FROM PT_TMAE_COTBEN WHERE NUM_COT = '" + strNumCot + "' ";
                if (cod_Tippension == vgTipoPensionSob)
                {
                    query = query + "AND COD_PAR<>'99'";
                }
                query = query + "ORDER BY NUM_ORDEN ASC";

                datosAI = SRVDBContext<AsignacionIntermediario>.CallSelectStatement(query, x => new AsignacionIntermediario
                {
                    numOrdenBen = x.GetInt32(0),
                    parentescoBen = x.GetString(1),
                    fecNacBen = x.GetString(2),
                    grupFamBen = x.GetString(3),
                    sexoBen = x.GetString(4),
                    cod_sitinvBen = x.GetString(5),
                    dPensionBen = x.GetString(6),
                    porsentajeBen = Convert.ToDouble(x.GetDecimal(7)),
                    porcLegBen = Convert.ToDouble(x.GetDecimal(8)),
                    fecNacHM = x.GetString(9),
                    dCrecerBen = x.GetString(10),
                    nombreBen = x.GetString(11)
                }).ToList();

                return datosAI;
            }
            catch (Exception ex)
            {
                Console.WriteLine("No existen Beneficiarios en la BD para este Estudio");
                throw;
            }
        }


        public AsignacionIntermediario fgCargarVariablesMortal(string strFecDev)
        {
            try
            {
                List<AsignacionIntermediario> datosAI = new List<AsignacionIntermediario>();
                AsignacionIntermediario dat = new AsignacionIntermediario();
                string query = "SELECT GLS_NOMBRE,NUM_CORRELATIVO,COD_TIPTABMOR, COD_SEXO,COD_TIPOPER FROM MA_TVAL_MORTAL WHERE FEC_INI<="
                    + strFecDev + " AND FEC_TER>=" + strFecDev + " AND COD_TIPTABMOR<> 'IND'";

                datosAI = SRVDBContext<AsignacionIntermediario>.CallSelectStatement(query, x => new AsignacionIntermediario
                {
                    GLS_NOMBRE = x.GetString(0),
                    NUM_CORRELATIVO = x.GetInt32(1),
                    COD_TIPTABMOR = x.GetString(2),
                    COD_SEXO = x.GetString(3),
                    COD_TIPOPER = x.GetString(4)
                }).ToList();

                if (datosAI != null)
                {
                    for (int i = 0; i < datosAI.Count; i++)
                    {
                        //llenar variables renta vitalicia
                        if (datosAI[i].COD_TIPTABMOR == "RV")
                        {
                            if (datosAI[i].COD_SEXO == "F")
                            {
                                if (datosAI[i].COD_TIPOPER == "M")
                                {
                                    dat.vgMortalVit_F = datosAI[i].NUM_CORRELATIVO;
                                    dat.vgPalabra_MortalVit_F = datosAI[i].GLS_NOMBRE;
                                }
                                else
                                {
                                    dat.vgMortalVit_F_A = datosAI[i].NUM_CORRELATIVO;
                                    dat.vgPalabra_MortalVit_F_A = datosAI[i].GLS_NOMBRE;
                                }
                            }
                            else
                            {
                                if (datosAI[i].COD_TIPOPER == "M")
                                {
                                    dat.vgMortalVit_M = datosAI[i].NUM_CORRELATIVO;
                                    dat.vgPalabra_MortalVit_M = datosAI[i].GLS_NOMBRE;
                                }
                                else
                                {
                                    dat.vgMortalVit_M_A = datosAI[i].NUM_CORRELATIVO;
                                    dat.vgPalabra_MortalVit_M_A = datosAI[i].GLS_NOMBRE;
                                }
                            }
                        }

                        //llenar variables invalidez total
                        if (datosAI[i].COD_TIPTABMOR == "MIT")
                        {
                            if (datosAI[i].COD_SEXO == "F")
                            {
                                if (datosAI[i].COD_TIPOPER == "M")
                                {
                                    dat.vgMortalTot_F = datosAI[i].NUM_CORRELATIVO;
                                    dat.vgPalabra_MortalTot_F = datosAI[i].GLS_NOMBRE;
                                }
                                else
                                {
                                    dat.vgMortalTot_F_A = datosAI[i].NUM_CORRELATIVO;
                                    dat.vgPalabra_MortalTot_F_A = datosAI[i].GLS_NOMBRE;
                                }
                            }
                            else
                            {
                                if (datosAI[i].COD_TIPOPER == "M")
                                {
                                    dat.vgMortalTot_M = datosAI[i].NUM_CORRELATIVO;
                                    dat.vgPalabra_MortalTot_M = datosAI[i].GLS_NOMBRE;
                                }
                                else
                                {
                                    dat.vgMortalTot_M_A = datosAI[i].NUM_CORRELATIVO;
                                    dat.vgPalabra_MortalTot_M_A = datosAI[i].GLS_NOMBRE;
                                }
                            }
                        }

                        //llenar variables invalidez parcial
                        if (datosAI[i].COD_TIPTABMOR == "MIP")
                        {
                            if (datosAI[i].COD_SEXO == "F")
                            {
                                if (datosAI[i].COD_TIPOPER == "M")
                                {
                                    dat.vgMortalPar_F = datosAI[i].NUM_CORRELATIVO;
                                    dat.vgPalabra_MortalPar_F = datosAI[i].GLS_NOMBRE;
                                }
                                else
                                {
                                    dat.vgMortalPar_F_A = datosAI[i].NUM_CORRELATIVO;
                                    dat.vgPalabra_MortalPar_F_A = datosAI[i].GLS_NOMBRE;
                                }
                            }
                            else
                            {
                                if (datosAI[i].COD_TIPOPER == "M")
                                {
                                    dat.vgMortalPar_M = datosAI[i].NUM_CORRELATIVO;
                                    dat.vgPalabra_MortalPar_M = datosAI[i].GLS_NOMBRE;
                                }
                                else
                                {
                                    dat.vgMortalPar_M_A = datosAI[i].NUM_CORRELATIVO;
                                    dat.vgPalabra_MortalPar_M_A = datosAI[i].GLS_NOMBRE;
                                }
                            }
                        }

                        //llenar variables beneficiarios
                        if (datosAI[i].COD_TIPTABMOR == "B")
                        {
                            if (datosAI[i].COD_SEXO == "F")
                            {
                                if (datosAI[i].COD_TIPOPER == "M")
                                {
                                    dat.vgMortalBen_F = datosAI[i].NUM_CORRELATIVO;
                                    dat.vgPalabra_MortalBen_F = datosAI[i].GLS_NOMBRE;
                                }
                                else
                                {
                                    dat.vgMortalBen_F_A = datosAI[i].NUM_CORRELATIVO;
                                    dat.vgPalabra_MortalBen_F_A = datosAI[i].GLS_NOMBRE;
                                }
                            }
                            else
                            {
                                if (datosAI[i].COD_TIPOPER == "M")
                                {
                                    dat.vgMortalBen_M = datosAI[i].NUM_CORRELATIVO;
                                    dat.vgPalabra_MortalBen_M = datosAI[i].GLS_NOMBRE;
                                }
                                else
                                {
                                    dat.vgMortalBen_M_A = datosAI[i].NUM_CORRELATIVO;
                                    dat.vgPalabra_MortalBen_M_A = datosAI[i].GLS_NOMBRE;
                                }
                            }
                        }
                        //Asignar a lista datos los valores en dat


                    }//fin For
                    /*if(datosAI.strSexo == "RV")
                    {

                    }*/
                }

                return dat;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }


        public AsignacionIntermediario fgFinTabAnual(AsignacionIntermediario datos, int vgNumeroTotalTablas, List<AsignacionIntermediario> egTablaMortal)
        {

            AsignacionIntermediario datosAI = new AsignacionIntermediario();
            
            datosAI.FinTab = -1; //valor a retornar en el 2005
            datosAI.vgFinTabVit_F_A = fgFintab_Mortal(datos.vgMortalVit_F_A, vgNumeroTotalTablas, egTablaMortal);
            if (datosAI.vgFinTabVit_F_A == -1)
            {
                Console.WriteLine("No existe la Edad Final de la Tabla de Mortalidad Anual de Rtas. Vitalicias de Mujeres.");
                return datosAI;
            }
            datosAI.vgFinTabTot_F_A = fgFintab_Mortal(datos.vgMortalTot_F_A, vgNumeroTotalTablas, egTablaMortal);
            if (datosAI.vgFinTabTot_F_A == -1)
            {
                Console.WriteLine("No existe la Edad Final de la Tabla de Mortalidad Anual de Inv. Total de Mujeres.");
                return datosAI;
            }
            datosAI.vgFinTabPar_F_A = fgFintab_Mortal(datos.vgMortalPar_F_A, vgNumeroTotalTablas, egTablaMortal);
            if (datosAI.vgFinTabPar_F_A == -1)
            {
                Console.WriteLine("No existe la Edad Final de la Tabla de Mortalidad Anual de Inv. Parcial de Mujeres.");
                return datosAI;
            }
            datosAI.vgFinTabBen_F_A = fgFintab_Mortal(datos.vgMortalBen_F_A, vgNumeroTotalTablas, egTablaMortal);
            if (datosAI.vgFinTabBen_F_A == -1)
            {
                Console.WriteLine("No existe la Edad Final de la Tabla de Mortalidad Anual de Beneficiarios de Mujeres.");
                return datosAI;
            }
            datosAI.vgFinTabVit_M_A = fgFintab_Mortal(datos.vgMortalVit_M_A, vgNumeroTotalTablas, egTablaMortal);
            if (datosAI.vgFinTabVit_M_A == -1)
            {
                Console.WriteLine("No existe la Edad Final de la Tabla de Mortalidad Anual de Rtas. Vitalicias de Hombres.");
                return datosAI;
            }
            datosAI.vgFinTabTot_M_A = fgFintab_Mortal(datos.vgMortalTot_M_A, vgNumeroTotalTablas, egTablaMortal);
            if (datosAI.vgFinTabTot_M_A == -1)
            {
                Console.WriteLine("No existe la Edad Final de la Tabla de Mortalidad Anual de Inv. Total de Hombres.");
                return datosAI;
            }
            datosAI.vgFinTabPar_M_A = fgFintab_Mortal(datos.vgMortalPar_M_A, vgNumeroTotalTablas, egTablaMortal);
            if (datosAI.vgFinTabPar_M_A == -1)
            {
                Console.WriteLine("No existe la Edad Final de la Tabla de Mortalidad Anual de Inv. Parcial de Hombres.");
                return datosAI;
            }
            datosAI.vgFinTabBen_M_A = fgFintab_Mortal(datos.vgMortalBen_M_A, vgNumeroTotalTablas, egTablaMortal);
            if (datosAI.vgFinTabBen_M_A == -1)
            {
                Console.WriteLine("No existe la Edad Final de la Tabla de Mortalidad Anual de Beneficiarios de Hombres.");
                return datosAI;
            }

            int valor;
            valor = amax1(datosAI.vgFinTabVit_F_A, datosAI.vgFinTabPar_F_A);
            valor = amax1(valor, datosAI.vgFinTabPar_F_A);
            valor = amax1(valor, datosAI.vgFinTabBen_F_A);
            valor = amax1(valor, datosAI.vgFinTabVit_M_A);
            valor = amax1(valor, datosAI.vgFinTabTot_M_A);
            valor = amax1(valor, datosAI.vgFinTabPar_M_A);
            valor = amax1(valor, datosAI.vgFinTabBen_M_A);

            datosAI.FinTab = valor;
            return datosAI;
        }


        public int amax1(int arg3, int arg4)
        {
            var amax1 = comp_amax1(arg3, arg4);
            return amax1;
        }


        public int comp_amax1(int arg3, int arg4)
        {
            int xx;
            if (arg3 >= arg4) {
                xx = arg3;
            }
            else {
                xx = arg4;
            }

            return xx;
        }


        public int fgFintab_Mortal(int iCorrelativo, int vgNumeroTotalTablas, List<AsignacionIntermediario> egTablaMortal)
        {
            var fgFintab_Mortal = fgFintab(iCorrelativo, vgNumeroTotalTablas, egTablaMortal);
            return fgFintab_Mortal;
        }


        public int fgFintab(int iCorrelativo, int vgNumeroTotalTablas, List<AsignacionIntermediario> egTablaMortal)
        {
            var fgFintab = -1;
            for (int i=0; i<vgNumeroTotalTablas; i++)
            {
                if (egTablaMortal[i].Num_Correlativo == iCorrelativo)
                {
                    fgFintab = egTablaMortal[i].Fin_Tab;
                    return fgFintab;
                }
            }
            return fgFintab;
        }


        /// <summary>
        /// José Hernández Alvarado.
        /// 27-09-2018
        /// Retorna lista con registros de beneficiarios.
        /// </summary>
        /// <param name="strNumCot">Número de cotización del afiliado.</param>
        /// <returns>Lista con beneficiarios.</returns>
        public List<AsignacionIntermediario> Tbl_Beneficiario(string strNumCot)
        {
            try
            {
                string query = "SELECT NUM_ORDEN, COD_PAR, FEC_NACBEN, COD_GRUFAM, COD_SEXO, COD_SITINV, ISNULL(FEC_SITINV, '') AS FEC_SITINV, COD_CAUINV, COD_DERPEN, PRC_PENSION, PRC_PENSIONLEG, ISNULL(FEC_NACHM, '') AS FEC_NACHM, " +
                                      "COD_DERCRE, ISNULL(FEC_FALBEN, '') AS FEC_FALBEN, COD_TIPOIDEN, ISNULL(NUM_IDEN, '') AS NUM_IDEN, GLS_NOMBEN, ISNULL(GLS_NOMSEGBEN, '') AS GLS_NOMSEGBEN, GLS_PATBEN, ISNULL(GLS_MATBEN, '') AS GLS_MATBEN " +
                                      "FROM PT_TMAE_COTBEN where NUM_COT = '" + strNumCot + "' ORDER BY NUM_ORDEN";

                return SRVDBContext<AsignacionIntermediario>.CallSelectStatement(query, x => new AsignacionIntermediario
                {
                    Num_Orden = x.GetInt32(0),
                    Cod_Parentesco = x.GetString(1),
                    Fec_Nac_Ben = x.GetString(2),
                    Cod_GruFam = x.GetString(3),
                    Cod_Sexo = x.GetString(4),
                    Cod_SitInv = x.GetString(5),
                    Fec_SitInv = x.GetString(6),
                    Cod_CauInv = x.GetString(7),
                    Cod_DerPen = x.GetString(8),
                    Prc_Pension = x.GetDecimal(9),
                    Prc_PensionLeg = x.GetDecimal(10),
                    Fec_Nac_HM = x.GetString(11),
                    Cod_DerCre = x.GetString(12),
                    Fec_FalBen = x.GetString(13),
                    Cod_TipoIden = x.GetInt32(14),
                    Num_Iden = x.GetString(15),
                    Gls_NomBen = x.GetString(16),
                    Gls_NomSegBen = x.GetString(17),
                    Gls_PatBen = x.GetString(18),
                    Gls_MatBen = x.GetString(19),
                }).ToList();

            }
            catch (Exception ex)
            {
                Console.WriteLine("Ha ocurrido un error al crear lista de tabla beneficiarios.");
                throw;
            }
        }


        public List<AsignacionIntermediario> Tbl_Ben_Actualiza(List<AsignacionIntermediario> Tbl_Ben, string strFecIni, string strSexo, string strCobertura, string strPension)
        {
            try
            {
                long L24 = (long)(Limite_Edad("LI", "L24", strFecIni));
                L24 = L24 * 12; //Mensualizar la edad de 24 años.

                long L18 = (long)(Limite_Edad("LI", "L18", strFecIni));
                L18 = L18 * 12; //Mensualizar la edad de 18 años.



                return Tbl_Ben;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ha ocurrido un error al actualizar lista de beneficiarios.");
                throw;
            }
        }


        /// <summary>
        /// José Hernández Alvarado.
        /// 27-09-2018
        /// Devuelve un valor(long) de edad limite.
        /// </summary>
        /// <param name="strTbl">COD_TABLA de BD</param>
        /// <param name="strElemento">COD_ELEMENTO de BD</param>
        /// <param name="strFecha">Cadena de fecha</param>
        /// <returns>Valor de edad limite.</returns>
        public decimal Limite_Edad(string strTbl, string strElemento, string strFecha)
        {
            try
            {
                List<AsignacionIntermediario> edad = new List<AsignacionIntermediario>();
                string query = "SELECT MTO_ELEMENTO FROM MA_TPAR_TABCODVIG WHERE COD_TABLA = '" + strTbl + "' AND COD_ELEMENTO = '" + strElemento + "' AND FEC_INIVIG <= '" + strFecha + "' and FEC_TERVIG >= '" + strFecha + "'";

                edad = SRVDBContext<AsignacionIntermediario>.CallSelectStatement(query, x => new AsignacionIntermediario
                {
                    Limite_Edad = x.GetDecimal(0)
                }).ToList();

                return edad[0].Limite_Edad;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ha ocurrido un error al obtener el límite de edad.");
                throw;
            }
        }

        //Consultas para obtener valores de rutina
        #region CONSULTAS PARA RUTINA

        /// <summary>
        /// José Hernández Alvarado.
        /// 01-10-2018
        /// Devuelve objeto de la clase de Asignación de intermediario (Entity).
        /// </summary>
        /// <param name="strNumCot">Número de cotización.</param>
        /// <returns>Objeto con valores a utilizar en la rutina.</returns>
        public AsignacionIntermediario ConsultarCotizacion(string strNumOp)
        {
            try
            {
                var parameters = new List<SqlParameter>();
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pClave", SqlDbType.VarChar, "CONSULTARCOTIZACION", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pNumOpera", SqlDbType.VarChar, strNumOp, ParameterDirection.Input));

                return VCEDBContext<AsignacionIntermediario>.CallStoreProcedure(StoredProcedures.CO_ConsultasCotizacionesOficiales, parameters, x => new AsignacionIntermediario
                {
                    CUSPP = x.GetString(0),
                    FechaNacimientoStr = x.GetString(1),
                    FechaNacimiento = Convert.ToDateTime(x.GetString(1).Substring(6, 2) + "/" + x.GetString(1).Substring(4, 2) + "/" + x.GetString(1).Substring(0, 4)),
                    FechaDevengueStr = x.GetString(2),
                    FechaDevengue = Convert.ToDateTime(x.GetString(2).Substring(6, 2) + "/" + x.GetString(2).Substring(4, 2) + "/" + x.GetString(2).Substring(0, 4)),
                    Cic = x.GetDecimal(3),
                    FechaEstudioStr = x.GetString(4),
                    FechaEstudio = Convert.ToDateTime(x.GetString(4).Substring(6, 2) + "/" + x.GetString(4).Substring(4, 2) + "/" + x.GetString(4).Substring(0, 4)),
                    CodigoPension = x.GetString(5),
                    ClaveSexo = x.GetString(6),
                    TipoCambio = x.GetDecimal(7),
                    Afp = x.GetString(8),
                    Ind_Cob = x.GetString(9),
                    Fec_DevSolStr = x.GetString(10),
                    Fec_DevSol = Convert.ToDateTime(x.GetString(10).Substring(6, 2) + "/" + x.GetString(10).Substring(4, 2) + "/" + x.GetString(10).Substring(0, 4)),
                    Cod_region = x.GetString(11)
                }).FirstOrDefault();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public List<Beneficiario> ConsultarBeneficiariosModificar(string idCotizacion, string strNumArch)
        {
            try
            {
                var parameters = new List<SqlParameter>();
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pClave", SqlDbType.VarChar, "CONSULTABENEFICIARIOS", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pNumOpera", SqlDbType.VarChar, idCotizacion, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pNumArch", SqlDbType.VarChar, strNumArch, ParameterDirection.Input));
                List<Beneficiario> b = new List<Beneficiario>();
                b = VCEDBContext<Beneficiario>.CallStoreProcedure(StoredProcedures.CO_ConsultasCotizacionesOficiales, parameters, x => new Beneficiario
                {
                    TipoDocumento = x.IsDBNull(0) ? "" : x.GetString(0),
                    Documento = x.GetString(1),
                    FechaNacimientoStr = x.GetString(2),
                    FechaNacimiento = Convert.ToDateTime(x.GetString(2).Substring(6, 2) + "/" + x.GetString(2).Substring(4, 2) + "/" + x.GetString(2).Substring(0, 4)),
                    Sexo = x.GetString(4) == "M" ? "Masculino" : "Femenino",
                    IdBeneficiario = x.GetInt32(3),
                    ClaveSexo = x.GetString(4),
                    FechaFallecimientoStr = x.GetString(5) == "" ? "" : x.GetString(5).Substring(6, 2) + "/" + x.GetString(5).Substring(4, 2) + "/" + x.GetString(5).Substring(0, 4),
                    FechaInvalidezRut = x.IsDBNull(5) ? "" : x.GetString(5),
                    ClaveSituacionInvalidez = x.GetString(6),
                    CodigoParentesco = x.IsDBNull(7) ? "" : x.GetString(7),
                    Parentesco = x.IsDBNull(7) ? "" : x.GetString(7)
                }).ToList();
                return b;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }


        public List<Modalidad> ConsultarModalidadesModificar(string idCotizacion)
        {
            try
            {
                var parameters = new List<SqlParameter>();
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pClave", SqlDbType.VarChar, "CONSULTARMODALIDADESMOD", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pNumOpera", SqlDbType.VarChar, idCotizacion, ParameterDirection.Input));

                return VCEDBContext<Modalidad>.CallStoreProcedure(StoredProcedures.CO_ConsultasCotizacionesOficiales, parameters, x => new Modalidad
                {
                    Moneda = x.GetString(0),
                    AniosGarantizados = x.GetInt32(1),
                    AniosDiferidos = x.GetInt32(2),
                    PorcentajeRentaTemporal = Convert.ToInt32(x.GetDecimal(3)),
                    IdModalidad = x.GetInt32(4),
                    CodigoTiposRenta =x.GetString(5),
                    CodigoModalidad = x.GetString(6),
                    ClaveMoneda = x.GetString(7),
                    CodigoTipoReajuste = Convert.ToInt32(x.GetString(8) == "" ? "3" : x.GetString(8)),
                    ValorComision = x.GetDecimal(9),
                    DerGra = x.GetString(10),
                    PrimerTramo = Convert.ToDecimal(x.GetInt32(11)),
                    SegundoTramo = x.GetDecimal(12),
                    ComisionInicial = x.GetDecimal(13),
                    PrccomS = x.GetDecimal(14),
                    Prcfaclab = x.GetDecimal(15),
                    Prc_Inicial_1 = x.GetDecimal(16),
                    Prc_Inicial = x.GetDecimal(17),
                    Prc_Minimo_1 = x.GetDecimal(18),
                    Prc_Minimo = x.GetDecimal(19),
                    Excepsiscovs = x.GetString(20)
                }).ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public void RegistroRutinaOficiales(string query)
        {
            try
            {
                SRVDBContext<SolicitudesCotizacion>.CallSelectStatement(query, x => new SolicitudesCotizacion
                {
                }).FirstOrDefault();
            }
            catch (Exception ex)
            {

                throw;
            }
        }


        #endregion

        public void updateAsesor(string idAsesor, string numOperacion, double cic)
        {
            try
            {
                var parameters = new List<SqlParameter>();
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pClave", SqlDbType.VarChar, "UPDATEASESOR", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@numIdenCor", SqlDbType.VarChar, idAsesor, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pNumOpera", SqlDbType.VarChar, numOperacion, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pNumCIC", SqlDbType.Decimal, cic, ParameterDirection.Input));
                VCEDBContext<SolicitudesCotizacion>.CallStoreProcedure(StoredProcedures.CO_CatalogosCalcularAsignacionIntermediario, parameters, x => new SolicitudesCotizacion
                {
                }).FirstOrDefault();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public void pensionBen(decimal pension, int numArch, int numOrden, string numOpera, decimal prcleg)
        {
            try
            {
                var parameters = new List<SqlParameter>();
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pClave", SqlDbType.VarChar, "PRCPENSIONBEN", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pPrcPension", SqlDbType.Decimal, pension, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pPrcLeg", SqlDbType.Decimal, prcleg, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@vNumArchivo", SqlDbType.Int, numArch, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pNumOrden", SqlDbType.Int, numOrden, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pNumOpera", SqlDbType.VarChar, numOpera, ParameterDirection.Input));
                VCEDBContext<SolicitudesCotizacion>.CallStoreProcedure(StoredProcedures.CO_CatalogosCalcularAsignacionIntermediario, parameters, x => new SolicitudesCotizacion
                {
                }).FirstOrDefault();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public void PensionValid(decimal pension, int numArch, string numOpera, int numCorr, int ind_sisco)
        {
            try
            {
                var parameters = new List<SqlParameter>();
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pClave", SqlDbType.VarChar, "PENSIONVALID", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pPrcPension", SqlDbType.Decimal, pension, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@vNumArchivo", SqlDbType.Int, numArch, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pNumOpera", SqlDbType.VarChar, numOpera, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pNumCorr", SqlDbType.Int, numCorr, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@IND_SIS", SqlDbType.Int, ind_sisco, ParameterDirection.Input));
                VCEDBContext<SolicitudesCotizacion>.CallStoreProcedure(StoredProcedures.CO_CatalogosCalcularAsignacionIntermediario, parameters, x => new SolicitudesCotizacion
                {
                }).FirstOrDefault();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }


        public void PensionNueSisco(decimal pension, int numArch, string numOpera, int numCorr, /*int ind_sisco, string SNCotiza, */decimal tasavta)
        {
            try
            {
                var parameters = new List<SqlParameter>();
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pClave", SqlDbType.VarChar, "PENSIONNUESISCO", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pPrcPension", SqlDbType.Decimal, pension, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@vNumArchivo", SqlDbType.Int, numArch, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pNumOpera", SqlDbType.VarChar, numOpera, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pNumCorr", SqlDbType.Int, numCorr, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@TasaVta", SqlDbType.VarChar, tasavta, ParameterDirection.Input));
                //parameters.Add(VCEDBContext<RowAffected>.AddParams("@IND_SIS", SqlDbType.VarChar, ind_sisco, ParameterDirection.Input));
                //parameters.Add(VCEDBContext<RowAffected>.AddParams("@INDSNCot", SqlDbType.VarChar, SNCotiza, ParameterDirection.Input));

                VCEDBContext<SolicitudesCotizacion>.CallStoreProcedure(StoredProcedures.CO_CatalogosCalcularAsignacionIntermediario, parameters, x => new SolicitudesCotizacion
                {
                }).FirstOrDefault();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public void ValSNCotza( int numArch, string numOpera, int numCorr,string sncotiza)
        {
            try
            {
                var parameters = new List<SqlParameter>();
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pClave", SqlDbType.VarChar, "VALSNCOTIZA", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@vNumArchivo", SqlDbType.Int, numArch, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pNumOpera", SqlDbType.VarChar, numOpera, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pNumCorr", SqlDbType.Int, numCorr, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@INDSNCot", SqlDbType.VarChar, sncotiza, ParameterDirection.Input));
                VCEDBContext<SolicitudesCotizacion>.CallStoreProcedure(StoredProcedures.CO_CatalogosCalcularAsignacionIntermediario, parameters, x => new SolicitudesCotizacion
                {
                }).FirstOrDefault();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public SolicitudesCotizacion ValInfSis(int numArch, string numOpera)
        {
            try
            { 
                var parameters = new List<SqlParameter>();
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pClave", SqlDbType.VarChar, "VALSNCOTIZASEL", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@vNumArchivo", SqlDbType.Int, numArch, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pNumOpera", SqlDbType.VarChar, numOpera, ParameterDirection.Input));
                return VCEDBContext<SolicitudesCotizacion>.CallStoreProcedure(StoredProcedures.CO_CatalogosCalcularAsignacionIntermediario, parameters, x => new SolicitudesCotizacion
                {
                    Ind_Sis = x.GetInt32(0)
                }).FirstOrDefault();


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        //public void ValMensajeError(int numArch, string numOpera, int numCorr, string codrecha, string mensajeerr)
        //{
        //    try
        //    {
        //        var parameters = new List<SqlParameter>();
        //        parameters.Add(VCEDBContext<RowAffected>.AddParams("@pClave", SqlDbType.VarChar, "UPDATEMJEERRO", ParameterDirection.Input));
        //        parameters.Add(VCEDBContext<RowAffected>.AddParams("@vNumArchivo", SqlDbType.Int, numArch, ParameterDirection.Input));
        //        parameters.Add(VCEDBContext<RowAffected>.AddParams("@pNumOpera", SqlDbType.VarChar, numOpera, ParameterDirection.Input));
        //        parameters.Add(VCEDBContext<RowAffected>.AddParams("@pNumCorr", SqlDbType.Int, numCorr, ParameterDirection.Input));
        //        parameters.Add(VCEDBContext<RowAffected>.AddParams("@pCodRech", SqlDbType.VarChar, codrecha, ParameterDirection.Input));
        //        parameters.Add(VCEDBContext<RowAffected>.AddParams("@pMensaje", SqlDbType.VarChar, mensajeerr, ParameterDirection.Input));
        //        VCEDBContext<SolicitudesCotizacion>.CallStoreProcedure(StoredProcedures.CO_CatalogosCalcularAsignacionIntermediario, parameters, x => new SolicitudesCotizacion
        //        {
        //        }).FirstOrDefault();
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine(ex.Message);
        //        throw;
        //    }
        //}


    }
}
