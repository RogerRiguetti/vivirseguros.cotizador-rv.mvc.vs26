using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using Estudio.Repository.Core.Domain;
using Estudio.Process;
using Estudio.Repository.Persistence.Repositories;
using log4net;
using System.Reflection;
using log4net.Config;
using System.Runtime;

namespace Estudio.Process.Muestra
{
    public class PaginaFlujos
    {
        RutinaReservasRepository _rutinaReservasRepository = new RutinaReservasRepository();
        List<beResultadosFlujosTot> ResultadoCot = new List<beResultadosFlujosTot>();
        ReservasRepository _ReservasRepository = new ReservasRepository();
        public DataTable ModelFluTot = new DataTable();
        public DataTable ModelFluBen = new DataTable();

        private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        [STAThread]
        public async Task<string> RutinaReservas(string FecCalculo, List<beMortalVar> LisTabPar,
               List<beMortalidadDet> LisTabMDetPar, List<beMortalidadDin> LisTabDinPar,
               List<beMortalidadDinDet> LisTabMDDetPar, List<beTasasPromedio> ListaTasPromPar,
               List<beCurvaTasas> ListaCurvaTasasPar, List<beDatosPol> LisTabPolPar,
               List<beDatosBen> LisTabBenPar, double TipCam)
        {
            XmlConfigurator.Configure();
            //bool ResultadosRutina = true;
            List<beResultadosFlujosTot> ResultadoFlujos = new List<beResultadosFlujosTot>();
            List<beResultadosFlujos> ResultadoFlujosBen = new List<beResultadosFlujos>();

            RutinaFlujos RActuarial = new RutinaFlujos();
            RutinaFlujosNoSbs RAactuarialNoSbs = new RutinaFlujosNoSbs();

            #region Parametros de la funcion

            //string Tip = "";
            //string Pol = "1";
            string FecCal = FecCalculo; //"20190131";
            double TicCam = TipCam; //3.373; //TipCam
            string querysDelete = "", res = "";

            #endregion

            #region Carga Matriz de Tablas de Mortalidad

            int MortalVit_F, MortalTot_F, MortalPar_F, MortalBen_F, MortalVit_M, MortalTot_M, MortalPar_M, MortalBen_M;

            RutinaMortalidad RMortal = new RutinaMortalidad();
            List<beMortalidad> ListaMor = new List<beMortalidad>();
            List<beMortalVar> LisTab = new List<beMortalVar>();
            List<beMortalidadDet> LisTabMDet = new List<beMortalidadDet>();
            try
            {
                LisTab = (from td in LisTabPar where Convert.ToInt32(td.FEC_INI) <= Convert.ToInt32(FecCal) && Convert.ToInt32(FecCal) <= Convert.ToInt32(td.FEC_FIN) && td.COD_TIPTABMOR != "IND" && td.COD_TIPOPER == "M" select td).ToList();

                MortalVit_F = LisTab.Where(x => x.COD_TIPTABMOR == "RV" && x.COD_SEXO == "F").Select(x => x.NUM_CORRELATIVO).SingleOrDefault();
                MortalVit_M = LisTab.Where(x => x.COD_TIPTABMOR == "RV" && x.COD_SEXO == "M").Select(x => x.NUM_CORRELATIVO).SingleOrDefault();
                MortalTot_F = LisTab.Where(x => x.COD_TIPTABMOR == "MIT" && x.COD_SEXO == "F").Select(x => x.NUM_CORRELATIVO).SingleOrDefault();
                MortalTot_M = LisTab.Where(x => x.COD_TIPTABMOR == "MIT" && x.COD_SEXO == "M").Select(x => x.NUM_CORRELATIVO).SingleOrDefault();
                MortalPar_F = LisTab.Where(x => x.COD_TIPTABMOR == "MIP" && x.COD_SEXO == "F").Select(x => x.NUM_CORRELATIVO).SingleOrDefault();
                MortalPar_M = LisTab.Where(x => x.COD_TIPTABMOR == "MIP" && x.COD_SEXO == "M").Select(x => x.NUM_CORRELATIVO).SingleOrDefault();
                MortalBen_F = LisTab.Where(x => x.COD_TIPTABMOR == "B" && x.COD_SEXO == "F").Select(x => x.NUM_CORRELATIVO).SingleOrDefault();
                MortalBen_M = LisTab.Where(x => x.COD_TIPTABMOR == "B" && x.COD_SEXO == "M").Select(x => x.NUM_CORRELATIVO).SingleOrDefault();

                //OBTIENE EL DETALLE DE LAS TABLAS DE MORTALIDAD
                LisTabMDet = LisTabMDetPar;

                //llamo al listado de Tablas de Mortalidad
                ListaMor = RMortal.TablaMortalidad(LisTabMDet, MortalVit_F, MortalTot_F, MortalPar_F, MortalBen_F, MortalVit_M, MortalTot_M, MortalPar_M, MortalBen_M);

            }
            catch (Exception ex)
            {
                Console.WriteLine("{0}", ex.Message);
                return "Error en Carga Tabla Mortalidad.";
            }


            #endregion

            #region Carga Tabla Mortaliadad Dinamicas

            //string Query = "";
            int Mortal_M_S, Mortal_F_S, Mortal_M_I, Mortal_F_I;

            RutinaMortalidadDin RMortalDin = new RutinaMortalidadDin();
            List<beMortalidadDin> LisTabDin = new List<beMortalidadDin>();
            List<beMortalidadDinVal> ListaMorDin = new List<beMortalidadDinVal>();
            List<beMortalidadDinDet> LisTabMDDet = new List<beMortalidadDinDet>();
            try
            {
                LisTabDin = (from td in LisTabDinPar where Convert.ToInt32(td.FEC_INIVIG) <= Convert.ToInt32(FecCal) && Convert.ToInt32(FecCal) <= Convert.ToInt32(td.FEC_FINVIG) select td).ToList();

                Mortal_M_S = LisTabDin.Where(x => x.COD_INVALIDEZ == "S" && x.COD_SEXO == "M").Select(x => x.NUM_CORRELATIVO).SingleOrDefault();
                Mortal_F_S = LisTabDin.Where(x => x.COD_INVALIDEZ == "S" && x.COD_SEXO == "F").Select(x => x.NUM_CORRELATIVO).SingleOrDefault();
                Mortal_M_I = LisTabDin.Where(x => x.COD_INVALIDEZ == "I" && x.COD_SEXO == "M").Select(x => x.NUM_CORRELATIVO).SingleOrDefault();
                Mortal_F_I = LisTabDin.Where(x => x.COD_INVALIDEZ == "I" && x.COD_SEXO == "F").Select(x => x.NUM_CORRELATIVO).SingleOrDefault();
                //OBTIENE EL DETALLE DE LAS TABLAS DE MORTALIDAD

                LisTabMDDet = LisTabMDDetPar;

                ListaMorDin = RMortalDin.TablaMortalidad(LisTabMDDet, Mortal_M_S, Mortal_F_S, Mortal_M_I, Mortal_F_I);
            }
            catch (Exception ex)
            {
                Console.WriteLine("{0}", ex.Message);
                return "Error en Carga Tabla Mortalidad Dinámicas.";
            }

            #endregion

            #region Carga Poliza

            /*string TipPen="";
            TipPen = LisTabPolPar[0].TipPen;*/

            #endregion

            #region Carga Beneficiarios

            //Se hace en la parte lógica y se reciben como parte de los parámetros.

            #endregion

            #region Carga Tasa Mercado

            //string Mes = FecCal.Substring(4, 2).Replace("0", "").Trim();

            //Query = "SELECT COD_MONEDA, convert(numeric,COD_TIPREAJUSTE) COD_TIPREAJUSTE, PRC_MES" + Mes + " AS PRC_MES FROM MA_TVAL_TASATM WHERE NUM_ANNO=" + FecCal.Substring(0, 4);
            //cmd.CommandText = Query;
            //cmd.CommandType = CommandType.Text;
            //cmd.Connection = conexion;
            ////Console.WriteLine("{0}", "iNICIA");
            //conexion.Open();

            //reader = cmd.ExecuteReader();
            //List<beTasaMercado> ListaTM = new List<beTasaMercado>();

            //if (reader != null)
            //{
            //    while (reader.Read())
            //    {
            //        beTasaMercado tablaTM = new beTasaMercado();
            //        tablaTM.CodMon = reader.GetString(0);
            //        tablaTM.TipRea = (Int32)reader.GetDecimal(1);
            //        tablaTM.PrcVal = (double)reader.GetDecimal(2);
            //        ListaTM.Add(tablaTM);
            //        //Console.WriteLine("{0}", reader.GetString(0));
            //    }
            //    reader.Close();
            //}
            //conexion.Close();
            #endregion

            #region Carga Tasa Anclaje

            //Query = "SELECT COD_MONEDA, convert(numeric,COD_TIPREAJUSTE) COD_TIPREAJUSTE, PRC_TASA FROM ma_tval_tasaanclaje WHERE " + FecCal + " BETWEEN fec_inivig AND fec_tervig ";
            //cmd.CommandText = Query;
            //cmd.CommandType = CommandType.Text;
            //cmd.Connection = conexion;
            ////Console.WriteLine("{0}", "iNICIA");
            //conexion.Open();

            //reader = cmd.ExecuteReader();
            //List<beTasaAnclaje> ListaTA = new List<beTasaAnclaje>();

            //if (reader != null)
            //{
            //    while (reader.Read())
            //    {
            //        beTasaAnclaje tablaTA = new beTasaAnclaje();
            //        tablaTA.CodMon = reader.GetString(0);
            //        tablaTA.TipRea = (Int32)reader.GetDecimal(1);
            //        tablaTA.PrcVal = (double)reader.GetDecimal(2);
            //        ListaTA.Add(tablaTA);
            //        //Console.WriteLine("{0}", reader.GetString(0));
            //    }
            //    reader.Close();
            //}
            //conexion.Close();
            #endregion

            #region Carga Factor VAC
            List<beTasaFacVac> ListaVac = new List<beTasaFacVac>();
            //se debe crear la tabla de Factores Vacpara indexados

            ListaVac = _rutinaReservasRepository.ConsultaFactorVac();
            #endregion

            #region Carga CPK's

            //Query = "SELECT COD_MONEDA, convert(numeric,COD_TIPREAJUSTE) COD_TIPREAJUSTE,PRC_CPK,NUM_ANNO FROM PT_TVAL_CALCE WHERE FEC_INIVIG<=" + FecCal + " AND FEC_TERVIG>=" + FecCal + " ORDER BY NUM_ANNO";
            //cmd.CommandText = Query;
            //cmd.CommandType = CommandType.Text;
            //cmd.Connection = conexion;
            ////Console.WriteLine("{0}", "iNICIA");
            //conexion.Open();

            //reader = cmd.ExecuteReader();
            //List<beCPK> ListaCPK = new List<beCPK>();

            //if (reader != null)
            //{
            //    while (reader.Read())
            //    {
            //        beCPK tablaCPK = new beCPK();
            //        tablaCPK.COD_MONEDA = reader.GetString(0);
            //        tablaCPK.COD_TIPREAJUSTE = (Int32)reader.GetDecimal(1);
            //        tablaCPK.PRC_CPK = (double)reader.GetDecimal(2);
            //        tablaCPK.NUM_ANNO = reader.GetInt32(3);
            //        ListaCPK.Add(tablaCPK);
            //        //Console.WriteLine("{0}", reader.GetString(0));
            //    }
            //    reader.Close();
            //}
            //conexion.Close();
            #endregion

            #region Carga Rentabilidad
            ////Query = "SELECT COD_MONEDA, convert(numeric,COD_TIPREAJUSTE) COD_TIPREAJUSTE,PRC_TASAREN,NUM_ANNO FROM PT_TVAL_RENTABILIDAD WHERE FEC_INIVIG<=" + FecCal + " AND FEC_TERVIG>=" + FecCal + " ORDER BY NUM_ANNO";
            //Query = "SELECT COD_MONEDA,CONVERT(INTEGER,COD_TIPREAJUSTE) COD_TIPREAJUSTE,PRC_TASAREN,NUM_ANNO FROM PT_TVAL_RENTABILIDAD WHERE '" + FecCal + "' BETWEEN FEC_INIVIG AND FEC_TERVIG ORDER BY NUM_ANNO";
            //cmd.CommandText = Query;
            //cmd.CommandType = CommandType.Text;
            //cmd.Connection = conexion;
            ////Console.WriteLine("{0}", "iNICIA");
            //conexion.Open();

            //reader = cmd.ExecuteReader();
            //List<beRentabilidad> ListaRen = new List<beRentabilidad>();

            //if (reader != null)
            //{
            //    while (reader.Read())
            //    {
            //        beRentabilidad tablaRen = new beRentabilidad();
            //        tablaRen.COD_MONEDA = reader.GetString(0);
            //        tablaRen.COD_TIPREAJUSTE = reader.GetInt32(1);
            //        tablaRen.PRC_TASAREN = (double)reader.GetDecimal(2);
            //        tablaRen.NUM_ANNO = reader.GetInt32(3);
            //        ListaRen.Add(tablaRen);
            //        //Console.WriteLine("{0}", reader.GetString(0));
            //    }
            //    reader.Close();
            //}
            //conexion.Close();
            #endregion

            #region Tasas Promedio
            /*
            List<beTasasPromedio> ListaTasProm = new List<beTasasPromedio>();

            ListaTasProm = (from tasprom in ListaTasPromPar where tasprom.COD_TIPPENSION == TipPen select tasprom).ToList();
            */
            #endregion

            #region Curva Tasas
            List<beCurvaTasas> ListaCurvaTasas = new List<beCurvaTasas>();

            ListaCurvaTasas = (from cutas in ListaCurvaTasasPar where Convert.ToInt32(cutas.FEC_INIVIG) <= Convert.ToInt32(FecCal) && Convert.ToInt32(FecCal) <= Convert.ToInt32(cutas.FEC_TERVIG) select cutas).ToList();
            #endregion

            #region Datos de conexión
            string conexion = _ReservasRepository.cadena_conexion();
            #endregion

            #region Empieza Procesamiento de flujos 
            ////LLAMA LISTADO PAA OBTENER LOS DATOS DE COTIZACION
            try
            {

                querysDelete += "DELETE PR_TTMP_FLUPOL1 \n" +
                                "DELETE PR_TTMP_FLUPOL2 \n" +
                                "DELETE PR_TTMP_FLUBEN1 \n" +
                                "DELETE PR_TTMP_FLUBEN2 \n";

                _ReservasRepository.EjecutaScripts_CalculoFlujos(querysDelete);
                _log.Info("Se realizo la eliminacion de tablas temporales " + querysDelete);
                var resNueva = RActuarial.RutinaActFlujos(LisTabPolPar, LisTabBenPar, ListaMorDin, ListaVac, ListaTasPromPar, ListaCurvaTasas, FecCal, TicCam, conexion);
                await Task.WhenAll(resNueva);

                GC.Collect();
                Console.WriteLine("Memory used after full collection:   {0:N0}",
                                  GC.GetTotalMemory(true));
                GCSettings.LargeObjectHeapCompactionMode = GCLargeObjectHeapCompactionMode.CompactOnce;
                GC.Collect(2, GCCollectionMode.Forced, true, true);
                
                var resAnt = await RAactuarialNoSbs.RutinaActFlujosNoSbs(LisTabPolPar, LisTabBenPar, ListaMor, ListaVac, FecCal, TicCam, conexion);
                

                return resAnt;

            }
            catch (Exception ex)
            {
                Console.WriteLine("{0}", "Error en la Rutina - " + ex.Message);
                _log.Info("Error en Clase Pagina Flujos: " + ex.Message);
                return "Error en la Rutina";
            }
            #endregion

        }
    }
}
