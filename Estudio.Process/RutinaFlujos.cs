using Estudio.Repository.Core.Domain;
using Estudio.Repository.Persistence.Repositories;
using log4net;
using log4net.Config;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Runtime;
using System.Threading.Tasks;
using Microsoft.VisualBasic;
using System.Text; 
using Newtonsoft.Json;
using System.Web;
using System.IO;

namespace Estudio.Process
{
    public class RutinaFlujos
    {
        #region Clase de Rutina Nueva
        //public string msj { get; set; }
        public decimal Exp = (decimal)1 / 12;
        public int anoBasTM = 2017;
        public int Fintab = 1332;
        private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        ReservasRepository _ReservasRepository = new ReservasRepository();

        #region Variables para proceso de tareas.
        string Tip = "";
        int numPol = 1;
        string TipPen = "";
        string queryTas = "";

        List<Task> tareasFlujos = new List<Task>();
        List<beDatosBen> LisTabBenPar = new List<beDatosBen>();
        List<beDatosPol> LisTabPolPar = new List<beDatosPol>();
        List<beTasasPromedio> ListaTasProm = new List<beTasasPromedio>();

        //DataTables para las inserciones.
        public DataTable ModelFluBen1 = new DataTable();
        public DataTable ModelFluTot1 = new DataTable();
        public DataTable ModelFluBen2 = new DataTable();
        public DataTable ModelFluTot2 = new DataTable();

        // Lista de DataTable para las inserciones
        public List<DataTable> ListModelFluBen1 = new List<DataTable>();
        public List<DataTable> ListModelFluTot1 = new List<DataTable>();
        public List<DataTable> ListModelFluBen2 = new List<DataTable>();
        public List<DataTable> ListModelFluTot2 = new List<DataTable>();

        //Querys para eliminar meses 0 en las tablas
        //string eliminaCeros = "DELETE FROM PR_TTMP_FLUPOL1 WHERE MTO_FLUTOT_METAN IS NULL AND MTO_FLUTOT = 0 AND NUM_MESFLU = 0  \n " + 
        //                      "DELETE FROM PR_TTMP_FLUPOL2 WHERE MTO_FLUTOT_METAN IS NULL AND MTO_FLUTOT = 0 AND NUM_MESFLU = 0  \n" + 
        //                      "DELETE FROM PR_TTMP_FLUBEN1 WHERE MTO_FLUTOT_METAN IS NULL AND MTO_FLUTOT = 0 AND NUM_MESFLU = 0  \n" + 
        //                      "DELETE FROM PR_TTMP_FLUBEN2 WHERE MTO_FLUTOT_METAN IS NULL AND MTO_FLUTOT = 0 AND NUM_MESFLU = 0 ";
        #endregion

        public async Task RutinaActFlujos(List<beDatosPol> ModelPol, List<beDatosBen> Modelben, List<beMortalidadDinVal> ModelMor, List<beTasaFacVac> ModelFacVac,
                                          List<beTasasPromedio> ModelTasPro, List<beCurvaTasas> ModelTasCurva, string FecCal, double ValTc, string conexion)
        {
            try
            {
                XmlConfigurator.Configure();

                #region Creación de DataTables para inserción.
                ModelFluTot1.Columns.Add("NUM_POLIZA", typeof(string));
                ModelFluTot1.Columns.Add("NUM_MESFLU", typeof(int));
                ModelFluTot1.Columns.Add("MTO_PENSION", typeof(decimal));
                ModelFluTot1.Columns.Add("MTO_FLUPEN", typeof(decimal));
                ModelFluTot1.Columns.Add("MTO_FLUSEP", typeof(decimal));
                ModelFluTot1.Columns.Add("MTO_FLUTOT", typeof(decimal));
                ModelFluTot1.Columns.Add("PRC_TASTCE", typeof(decimal));
                ModelFluTot1.Columns.Add("PRC_TLR", typeof(decimal));
                //ModelFluTot1.Columns.Add("MTO_PENSION_METAN", typeof(decimal));
                //ModelFluTot1.Columns.Add("MTO_FLUPEN_METAN", typeof(decimal));
                //ModelFluTot1.Columns.Add("MTO_FLUSEP_METAN", typeof(decimal));
                //ModelFluTot1.Columns.Add("MTO_FLUTOT_METAN", typeof(decimal));
                //ModelFluTot1.Columns.Add("PRC_TASTCE_METAN", typeof(decimal));
                ModelFluTot1.Columns.Add("FEC_FLU", typeof(string));

                ModelFluBen1.Columns.Add("NUM_POLIZA", typeof(string));
                ModelFluBen1.Columns.Add("NUM_ORDEN", typeof(int));
                ModelFluBen1.Columns.Add("NUM_EDAD", typeof(int));
                ModelFluBen1.Columns.Add("NUM_MESFLU", typeof(int));
                ModelFluBen1.Columns.Add("MTO_PENSION", typeof(decimal));
                ModelFluBen1.Columns.Add("MTO_FLUPEN", typeof(decimal));
                ModelFluBen1.Columns.Add("MTO_FLUSEP", typeof(decimal));
                ModelFluBen1.Columns.Add("MTO_FLUTOT", typeof(decimal));
                ModelFluBen1.Columns.Add("MTO_TPX", typeof(decimal));
                ModelFluBen1.Columns.Add("MTO_QXT", typeof(decimal));
                //ModelFluBen1.Columns.Add("MTO_PENSION_METAN", typeof(decimal));
                //ModelFluBen1.Columns.Add("MTO_FLUPEN_METAN", typeof(decimal));
                //ModelFluBen1.Columns.Add("MTO_FLUSEP_METAN", typeof(decimal));
                //ModelFluBen1.Columns.Add("MTO_FLUTOT_METAN", typeof(decimal));
                //ModelFluBen1.Columns.Add("MTO_TPX_METAN", typeof(decimal));
                //ModelFluBen1.Columns.Add("MTO_QXT_METAN", typeof(decimal));
                ModelFluBen1.Columns.Add("FEC_FLU", typeof(string));

                ModelFluTot2.Columns.Add("NUM_POLIZA", typeof(string));
                ModelFluTot2.Columns.Add("NUM_MESFLU", typeof(int));
                ModelFluTot2.Columns.Add("MTO_PENSION", typeof(decimal));
                ModelFluTot2.Columns.Add("MTO_FLUPEN", typeof(decimal));
                ModelFluTot2.Columns.Add("MTO_FLUSEP", typeof(decimal));
                ModelFluTot2.Columns.Add("MTO_FLUTOT", typeof(decimal));
                ModelFluTot2.Columns.Add("PRC_TASTCE", typeof(decimal));
                ModelFluTot2.Columns.Add("PRC_TLR", typeof(decimal));
                //ModelFluTot2.Columns.Add("MTO_PENSION_METAN", typeof(decimal));
                //ModelFluTot2.Columns.Add("MTO_FLUPEN_METAN", typeof(decimal));
                //ModelFluTot2.Columns.Add("MTO_FLUSEP_METAN", typeof(decimal));
                //ModelFluTot2.Columns.Add("MTO_FLUTOT_METAN", typeof(decimal));
                //ModelFluTot2.Columns.Add("PRC_TASTCE_METAN", typeof(decimal));
                ModelFluTot2.Columns.Add("FEC_FLU", typeof(string));

                ModelFluBen2.Columns.Add("NUM_POLIZA", typeof(string));
                ModelFluBen2.Columns.Add("NUM_ORDEN", typeof(int));
                ModelFluBen2.Columns.Add("NUM_EDAD", typeof(int));
                ModelFluBen2.Columns.Add("NUM_MESFLU", typeof(int));
                ModelFluBen2.Columns.Add("MTO_PENSION", typeof(decimal));
                ModelFluBen2.Columns.Add("MTO_FLUPEN", typeof(decimal));
                ModelFluBen2.Columns.Add("MTO_FLUSEP", typeof(decimal));
                ModelFluBen2.Columns.Add("MTO_FLUTOT", typeof(decimal));
                ModelFluBen2.Columns.Add("MTO_TPX", typeof(decimal));
                ModelFluBen2.Columns.Add("MTO_QXT", typeof(decimal));
                //ModelFluBen2.Columns.Add("MTO_PENSION_METAN", typeof(decimal));
                //ModelFluBen2.Columns.Add("MTO_FLUPEN_METAN", typeof(decimal));
                //ModelFluBen2.Columns.Add("MTO_FLUSEP_METAN", typeof(decimal));
                //ModelFluBen2.Columns.Add("MTO_FLUTOT_METAN", typeof(decimal));
                //ModelFluBen2.Columns.Add("MTO_TPX_METAN", typeof(decimal));
                //ModelFluBen2.Columns.Add("MTO_QXT_METAN", typeof(decimal));
                ModelFluBen2.Columns.Add("FEC_FLU", typeof(string));
                #endregion

                #region Tareas
                //Ciclo que recorre lista de pólizas llamando método que contiene la rutina y crea las tareas.
                
                for (int i = 0; i < ModelPol.Count; i++)
                {
                    Console.WriteLine("Memory used before collection:       {0:N0}",
                    GC.GetTotalMemory(false));
                    try
                    {
                        LisTabPolPar = (from pol in ModelPol where Convert.ToInt32(pol.NumPol) == Convert.ToInt32(ModelPol[i].NumPol) select pol).ToList();
                        if (LisTabPolPar.Count == 1)
                        {
                            LisTabBenPar = (from ben in Modelben where LisTabPolPar[0].NumPol == ben.NumPol select ben).ToList();

                            TipPen = "";
                            TipPen = LisTabPolPar[0].TipPen;

                            #region Tasas Promedio
                            ListaTasProm = (from tasprom in ModelTasPro where tasprom.COD_TIPPENSION == TipPen select tasprom).ToList();
                            #endregion

                            switch (LisTabPolPar[0].Tip)
                            {
                                case "1":
                                    Tip = "1";
                                    break;

                                case "2":
                                    Tip = "2";
                                    break;
                            }
                        }

                        if ((LisTabPolPar.Count != 0) && (LisTabBenPar.Count != 0))
                        {
                            var task = ResFlujoTodo(LisTabPolPar, LisTabBenPar, ModelMor, ModelFacVac, ListaTasProm, ModelTasCurva, FecCal, ValTc, Tip, LisTabPolPar[0].IndStock, LisTabPolPar[0].FecDev);
                            tareasFlujos.Add(task);
                            numPol++;
                        }
                    }
                    catch (Exception ex)
                    {
                        _log.Info("ERROR EN PÓLIZA No. " + LisTabPolPar[0].NumPol + " AL CREAR LA TAREA.");
                        _log.Info("ERROR: " + ex.Message);
                    }

                }
                GC.Collect();
                Console.WriteLine("Memory used after full collection:   {0:N0}",
                                    GC.GetTotalMemory(true));
                GCSettings.LargeObjectHeapCompactionMode = GCLargeObjectHeapCompactionMode.CompactOnce;
                GC.Collect(2, GCCollectionMode.Forced, true, true);

                #endregion

                while (tareasFlujos.Count > 0)
                {
                    Task firstFinishedTask = await Task.WhenAny(tareasFlujos);
                    tareasFlujos.Remove(firstFinishedTask);
                }

                GC.Collect();
                Console.WriteLine("Memory used after full collection:   {0:N0}",
                                  GC.GetTotalMemory(true));
                GCSettings.LargeObjectHeapCompactionMode = GCLargeObjectHeapCompactionMode.CompactOnce;
                GC.Collect(2, GCCollectionMode.Forced, true, true);
                //await Task.WhenAll(tareasFlujos);
                

                try { if (queryTas != "") { _ReservasRepository.EjecutaScripts_CalculoFlujos(queryTas); } }
                catch (Exception ex)
                {
                    _log.Info("ERROR AL ACTUALIZAR TASAS DE RESERVA: " + ex.Message);
                }
                
                try
                {
                    //Inserción de los registros añadidos a los DataTable
                    #region Llenado de DataTables para insertar en BD.
                    foreach (var dt in ListModelFluTot1)
                    {
                        foreach (DataRow item in dt.Rows)
                        {
                            DataRow row = ModelFluTot1.NewRow();

                            row["NUM_POLIZA"] = item[0];
                            row["NUM_MESFLU"] = item[1];
                            row["MTO_PENSION"] = item[2];
                            row["MTO_FLUPEN"] = item[3];
                            row["MTO_FLUSEP"] = item[4];
                            row["MTO_FLUTOT"] = item[5];
                            row["PRC_TASTCE"] = item[6];
                            row["PRC_TLR"] = item[7];
                            row["FEC_FLU"] = item[8];

                            ModelFluTot1.Rows.Add(row);
                        }
                    }

                    foreach (var dt in ListModelFluTot2)
                    {
                        foreach (DataRow item in dt.Rows)
                        {
                            DataRow row = ModelFluTot2.NewRow();

                            row["NUM_POLIZA"] = item[0];
                            row["NUM_MESFLU"] = item[1];
                            row["MTO_PENSION"] = item[2];
                            row["MTO_FLUPEN"] = item[3];
                            row["MTO_FLUSEP"] = item[4];
                            row["MTO_FLUTOT"] = item[5];
                            row["PRC_TASTCE"] = item[6];
                            row["PRC_TLR"] = item[7];
                            row["FEC_FLU"] = item[8];

                            ModelFluTot2.Rows.Add(row);
                        }
                    }

                    foreach (var dt in ListModelFluBen1)
                    {
                        foreach (DataRow item in dt.Rows)
                        {
                            DataRow row = ModelFluBen1.NewRow();

                            row["NUM_POLIZA"] = item[0];
                            row["NUM_ORDEN"] = item[1];
                            row["NUM_EDAD"] = item[2];
                            row["NUM_MESFLU"] = item[3];
                            row["MTO_PENSION"] = item[4];
                            row["MTO_FLUPEN"] = item[5];
                            row["MTO_FLUSEP"] = item[6];
                            row["MTO_FLUTOT"] = item[7];
                            row["MTO_TPX"] = item[8];
                            row["MTO_QXT"] = item[9];
                            row["FEC_FLU"] = item[10];

                            ModelFluBen1.Rows.Add(row);
                        }
                    }

                    foreach (var dt in ListModelFluBen2)
                    {
                        foreach (DataRow item in dt.Rows)
                        {
                            DataRow row = ModelFluBen2.NewRow();

                            row["NUM_POLIZA"] = item[0];
                            row["NUM_ORDEN"] = item[1];
                            row["NUM_EDAD"] = item[2];
                            row["NUM_MESFLU"] = item[3];
                            row["MTO_PENSION"] = item[4];
                            row["MTO_FLUPEN"] = item[5];
                            row["MTO_FLUSEP"] = item[6];
                            row["MTO_FLUTOT"] = item[7];
                            row["MTO_TPX"] = item[8];
                            row["MTO_QXT"] = item[9];
                            row["FEC_FLU"] = item[10];

                            ModelFluBen2.Rows.Add(row);
                        }
                    }
                    #endregion
                }
                catch (Exception ex)
                {
                    _log.Info("ERROR AL LLENAR DATATABLES PARA INSERCIÓN DE RESULTADOS DE RUTINA NUEVA: " + ex.Message);
                }

                try
                {
                    DateTime fecha = DateTime.ParseExact(FecCal, "yyyyMMdd", CultureInfo.InvariantCulture);
                    string strFecha = fecha.AddDays(-1).ToString("yyyyMM");
                    string path = System.Web.Hosting.HostingEnvironment.MapPath("~/BD_Reservas/" + strFecha);
                    _log.Info("Elimina los archivos temporales");
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }

                    if (ModelFluTot1.Rows.Count != 0) { DataTableToJSONWithStringBuilder(ModelFluTot1, "FlujoPolizaSol", path); }
                    if (ModelFluTot2.Rows.Count != 0) { DataTableToJSONWithStringBuilder(ModelFluTot2, "FlujoPolizaDol", path); }
                    if (ModelFluBen1.Rows.Count != 0) { DataTableToJSONWithStringBuilder(ModelFluBen1, "FlujoBenefiSol", path); }
                    if (ModelFluBen2.Rows.Count != 0) { DataTableToJSONWithStringBuilder(ModelFluBen2, "FlujoBenefiDol", path); }


                } 
                catch (Exception ex)
                {
                    _log.Info("ERROR AL INSERTAR RESULTADOS DE FLUJOS DE RUTINA NUEVA: " + ex.Message);
                }

                //_ReservasRepository.Ejecuta_Query_Conn(eliminaCeros, conexion);
                

            }
            catch (Exception ex)
            {
                Console.WriteLine("{0}", "Error en la Rutina Nueva - " + ex.Message);
                _log.Info("Error en Clase Rutina Flujos: " + ex.Message);
            }

        }
        #endregion

        async Task ResFlujoTodo(List<beDatosPol> ModelPol, List<beDatosBen> Modelben, List<beMortalidadDinVal> ModelMor, List<beTasaFacVac> ModelFacVac, List<beTasasPromedio> ModelTasPro,
                                List<beCurvaTasas> ModelTasCurva, string FecCal, double ValTc, string TipTbl, string Stock, string FechaDev)
        {
            #region Rutina
            await Task.Factory.StartNew(() =>
            {
                List<beResultadosFlujos> ListaResultados = new List<beResultadosFlujos>();
                List<beResultadosFlujosTot> ListaResultadosTot = new List<beResultadosFlujosTot>();
                string msj = "";
                DateTime vlFechaDev = new DateTime();
                vlFechaDev = DateTime.ParseExact(FechaDev.Substring(0, 6) + "01", "yyyyMMdd", CultureInfo.InvariantCulture);

                #region DT Locales
                DataTable ModelFluBen1Local = new DataTable();
                DataTable ModelFluTot1Local = new DataTable();
                DataTable ModelFluBen2Local = new DataTable();
                DataTable ModelFluTot2Local = new DataTable();

                ModelFluTot1Local.Columns.Add("NUM_POLIZA", typeof(string));
                ModelFluTot1Local.Columns.Add("NUM_MESFLU", typeof(int));
                ModelFluTot1Local.Columns.Add("MTO_PENSION", typeof(decimal));
                ModelFluTot1Local.Columns.Add("MTO_FLUPEN", typeof(decimal));
                ModelFluTot1Local.Columns.Add("MTO_FLUSEP", typeof(decimal));
                ModelFluTot1Local.Columns.Add("MTO_FLUTOT", typeof(decimal));
                ModelFluTot1Local.Columns.Add("PRC_TASTCE", typeof(decimal));
                ModelFluTot1Local.Columns.Add("PRC_TLR", typeof(decimal));
                //ModelFluTot1Local.Columns.Add("MTO_PENSION_METAN", typeof(decimal));
                //ModelFluTot1Local.Columns.Add("MTO_FLUPEN_METAN", typeof(decimal));
                //ModelFluTot1Local.Columns.Add("MTO_FLUSEP_METAN", typeof(decimal));
                //ModelFluTot1Local.Columns.Add("MTO_FLUTOT_METAN", typeof(decimal));
                //ModelFluTot1Local.Columns.Add("PRC_TASTCE_METAN", typeof(decimal));
                ModelFluTot1Local.Columns.Add("FEC_FLU", typeof(string));

                ModelFluBen1Local.Columns.Add("NUM_POLIZA", typeof(string));
                ModelFluBen1Local.Columns.Add("NUM_ORDEN", typeof(int));
                ModelFluBen1Local.Columns.Add("NUM_EDAD", typeof(int));
                ModelFluBen1Local.Columns.Add("NUM_MESFLU", typeof(int));
                ModelFluBen1Local.Columns.Add("MTO_PENSION", typeof(decimal));
                ModelFluBen1Local.Columns.Add("MTO_FLUPEN", typeof(decimal));
                ModelFluBen1Local.Columns.Add("MTO_FLUSEP", typeof(decimal));
                ModelFluBen1Local.Columns.Add("MTO_FLUTOT", typeof(decimal));
                ModelFluBen1Local.Columns.Add("MTO_TPX", typeof(decimal));
                ModelFluBen1Local.Columns.Add("MTO_QXT", typeof(decimal));
                //ModelFluBen1Local.Columns.Add("MTO_PENSION_METAN", typeof(decimal));
                //ModelFluBen1Local.Columns.Add("MTO_FLUPEN_METAN", typeof(decimal));
                //ModelFluBen1Local.Columns.Add("MTO_FLUSEP_METAN", typeof(decimal));
                //ModelFluBen1Local.Columns.Add("MTO_FLUTOT_METAN", typeof(decimal));
                //ModelFluBen1Local.Columns.Add("MTO_TPX_METAN", typeof(decimal));
                //ModelFluBen1Local.Columns.Add("MTO_QXT_METAN", typeof(decimal));
                ModelFluBen1Local.Columns.Add("FEC_FLU", typeof(string));

                ModelFluTot2Local.Columns.Add("NUM_POLIZA", typeof(string));
                ModelFluTot2Local.Columns.Add("NUM_MESFLU", typeof(int));
                ModelFluTot2Local.Columns.Add("MTO_PENSION", typeof(decimal));
                ModelFluTot2Local.Columns.Add("MTO_FLUPEN", typeof(decimal));
                ModelFluTot2Local.Columns.Add("MTO_FLUSEP", typeof(decimal));
                ModelFluTot2Local.Columns.Add("MTO_FLUTOT", typeof(decimal));
                ModelFluTot2Local.Columns.Add("PRC_TASTCE", typeof(decimal));
                ModelFluTot2Local.Columns.Add("PRC_TLR", typeof(decimal));
                //ModelFluTot2Local.Columns.Add("MTO_PENSION_METAN", typeof(decimal));
                //ModelFluTot2Local.Columns.Add("MTO_FLUPEN_METAN", typeof(decimal));
                //ModelFluTot2Local.Columns.Add("MTO_FLUSEP_METAN", typeof(decimal));
                //ModelFluTot2Local.Columns.Add("MTO_FLUTOT_METAN", typeof(decimal));
                //ModelFluTot2Local.Columns.Add("PRC_TASTCE_METAN", typeof(decimal));
                ModelFluTot2Local.Columns.Add("FEC_FLU", typeof(string));

                ModelFluBen2Local.Columns.Add("NUM_POLIZA", typeof(string));
                ModelFluBen2Local.Columns.Add("NUM_ORDEN", typeof(int));
                ModelFluBen2Local.Columns.Add("NUM_EDAD", typeof(int));
                ModelFluBen2Local.Columns.Add("NUM_MESFLU", typeof(int));
                ModelFluBen2Local.Columns.Add("MTO_PENSION", typeof(decimal));
                ModelFluBen2Local.Columns.Add("MTO_FLUPEN", typeof(decimal));
                ModelFluBen2Local.Columns.Add("MTO_FLUSEP", typeof(decimal));
                ModelFluBen2Local.Columns.Add("MTO_FLUTOT", typeof(decimal));
                ModelFluBen2Local.Columns.Add("MTO_TPX", typeof(decimal));
                ModelFluBen2Local.Columns.Add("MTO_QXT", typeof(decimal));
                //ModelFluBen2Local.Columns.Add("MTO_PENSION_METAN", typeof(decimal));
                //ModelFluBen2Local.Columns.Add("MTO_FLUPEN_METAN", typeof(decimal));
                //ModelFluBen2Local.Columns.Add("MTO_FLUSEP_METAN", typeof(decimal));
                //ModelFluBen2Local.Columns.Add("MTO_FLUTOT_METAN", typeof(decimal));
                //ModelFluBen2Local.Columns.Add("MTO_TPX_METAN", typeof(decimal));
                //ModelFluBen2Local.Columns.Add("MTO_QXT_METAN", typeof(decimal));
                ModelFluBen2Local.Columns.Add("FEC_FLU", typeof(string));
                #endregion

                try
                {
                    #region Variables
                    string Mone, Cober, cplan;
                    string swg = "";
                    int Nben, Nap, Nmp, Ndp, Nad, Nmd, Ndd, TipRea;
                    int vlVecesCot = 1;
                    int l = 0;
                    string TipPen, TipRen, TipMod, ind_cob, FecCot, FecDev, DerCre, DerGra;
                    string fecha;
                    double tvmax;
                    double Valmon, MtoCic, GtoFun, PrcTaf;
                    double MesCostoTmp;
                    double new_prc;
                    double PenBase, tce = 0;
                    double tpr = 0;
                    int nmax;
                    int Mesdif;
                    int Mesgar;
                    int mescosto;
                    int EdaLim;
                    int mesdif1, pergar, pergar1, mesdifc, perdif;
                    int Fechap, Fechrv, FechaDv, mescon;
                    int ltot;
                    int mesdifgar;
                    int ltotCero;
                    int[] Orden = new int[20];
                    int[] Ncorbe = new int[20];
                    int[] Nanbe = new int[20];
                    int[] Nmnbe = new int[20];
                    int[] Ndnbe = new int[20];
                    string[] Sexob = new string[20];
                    string[] Coinb = new string[20];
                    double[] porcbe_ori = new double[20];
                    double[] Porcbe = new double[20];
                    double[] Porcbe_tram = new double[20];
                    double[] vl_FactorReajuste = new double[1400];
                    double[] facgratif = new double[1400];
                    double[] Flupen = new double[1400];
                    double[] Flucm = new double[1400];
                    double[] factual = new double[1400];
                    double[] fTramos = new double[1400];

                    //variables de Tablas Dinamicas
                    decimal[,] LxDin = new decimal[10, 1400];

                    //variables de tasas Curva
                    decimal[] ValCurva = new decimal[1400];
                    //para saber los pagos de las reservas
                    double[] fpagosRes = new double[1400];
                    //para flujos por beneficiario
                    double[,] FlujosBen = new double[10, 1400];
                    double MtoPen;
                    double prcTce;
                    int[] Edadben = new int[20];
                    string NumPol;
                    string vlFecFallCausante = "", vlIndFall;
                    string[] Estudi = new string[20];
                    string[] FecFallBen = new string[20]; //20012020
                    string[] Cod_Tope18 = new string[20];

                    bool Resultado = true;
                    int i = 0; //Para los ciclos de llenado de los DataTables de resultados que se insertarán en BD.
                    int j = 0; //Para los ciclos de llenado de los DataTables de resultados que se insertarán en BD.
                    double tci = 0; //Para poder recuperar el valor del método de TCE e insertarlo en BD.
                    double[] fqxy = new double[1400];
                    double[] fpy = new double[1400];
                    #endregion

                    if (ModelPol[0].NumeroCasoEspecial != 4)
                    {
                        foreach (var item in ModelPol)
                        {
                            #region CargaVariable
                            NumPol = item.NumPol;
                            Nben = Modelben.Count - 1;
                            Cober = item.TipPen;  //cober
                            TipPen = item.TipPen;  //TipoPension
                            TipRen = item.TipRen; //Indi
                            TipMod = item.TipMod; //alt
                            Mesgar = item.NumGar;
                            Mone = item.TipMon;
                            ind_cob = item.IndCob;

                            Valmon = Mone == "NS" ? 1 : ValTc;

                            MtoCic = Mone != "NS" ? (item.MtoPri / Valmon) : item.MtoPri;

                            FecCot = item.FecCot;
                            Nap = int.Parse(FecCal.Substring(0, 4));
                            Nmp = int.Parse(FecCal.Substring(4, 2));
                            Ndp = int.Parse(FecCal.Substring(6, 2));

                            Mesdif = item.NumDif;
                            FecDev = item.FecDev;
                            mesdifgar = Mesgar + Mesdif;
                            Nad = int.Parse(item.FecDev.Substring(0, 4)); //Nad
                            Nmd = int.Parse(item.FecDev.Substring(4, 2)); //Nmd
                            Ndd = int.Parse(item.FecDev.Substring(6, 2)); //Ndd
                            GtoFun = Math.Round(item.MtoGS / Valmon, 2);
                            DerCre = item.DerCre; //DerCrecer
                            DerGra = item.DerGra; //DerGratificacion
                            EdaLim = item.EdaLim; //EdaLim
                            TipRea = item.TipRea; //Tipajus
                            MtoPen = item.MtoPen;
                            PrcTaf = item.PrcTaf;
                            tvmax = item.PrcTas;
                            prcTce = item.PrcTasRes;

                            //GRATIFICACION
                            CargarTablaGratificacion(Nad, Nmd, DerGra, ref facgratif);

                            switch (TipRen)
                            {
                                case "1":
                                    TipRen = "I";
                                    break;
                                case "2":
                                    TipRen = "D";
                                    break;
                                case "3":
                                    TipRen = "M";
                                    break;
                                case "5":
                                    TipRen = "C";
                                    break;
                                case "6":
                                    TipRen = "E";
                                    break;

                            };

                            switch (TipMod)
                            {
                                case "1":
                                    TipMod = "S";
                                    break;
                                case "3":
                                    TipMod = "G";
                                    break;
                                case "4":
                                    TipMod = "F";
                                    break;
                            }

                            switch (TipPen)
                            {
                                case "08":
                                    TipPen = "S";
                                    break;
                                case "07":
                                    TipPen = "P";
                                    break;
                                case "06":
                                    TipPen = "I";
                                    break;
                                case "05":
                                    TipPen = "V";
                                    break;
                                case "04":
                                    TipPen = "V";
                                    break;
                            }

                            //CARGA LOS DATOS DE LOS BENEFICIARIOS EN LAS VARIABLES
                            if (vlVecesCot == 1)
                            {
                                vlVecesCot = 2;

                                foreach (var itemben2 in Modelben)
                                {
                                    Orden[l] = itemben2.NumOrd;
                                    Ncorbe[l] = int.Parse(itemben2.CodPar);
                                    if (TipPen == "S")
                                    {
                                        if (Ncorbe[l] == 99)
                                        {
                                            goto Next;
                                        }
                                    }
                                    if ((Ncorbe[l] == 99) || (Ncorbe[l] == 0))
                                    {
                                        vlFecFallCausante = itemben2.FacFal;
                                    }

                                    porcbe_ori[l] = (itemben2.PrcLeg / 100);
                                    Porcbe[l] = (itemben2.PrcPen / 100);
                                    fecha = itemben2.FecNac;
                                    Nanbe[l] = int.Parse(fecha.Substring(0, 4)); //'aa_nac
                                    Nmnbe[l] = int.Parse(fecha.Substring(4, 2)); //'mm_nac
                                    Ndnbe[l] = int.Parse(fecha.Substring(6, 2)); //'mm_nac

                                    Sexob[l] = itemben2.TipSex;
                                    Coinb[l] = itemben2.TipInv;

                                    Estudi[l] = itemben2.Estudi;
                                    Cod_Tope18[l] = itemben2.Tope18;
                                    FecFallBen[l] = itemben2.FacFal;
                                    Next:
                                    l++;
                                }
                            }

                            //aqui identifica si la poliza es de un solo titular y tiene periodo garantizado
                            vlIndFall = "N";
                            if (l == 1)
                            {
                                if (vlFecFallCausante.Length != 0)
                                {
                                    vlIndFall = "S";
                                }
                            }
                            else
                            {
                                if (TipPen != "S" && vlFecFallCausante != "")
                                {
                                    vlIndFall = "S";
                                }
                            }
                            //aqui identifica si la poliza es de un solo titular y tiene periodo garantizado


                            Resultado = CargaTasaProm(ModelTasPro, Mone, TipRea, ref tpr, ref msj);
                            if ((!Resultado) && msj != "")
                            {
                                throw new System.ArgumentException(msj);
                            }

                            Resultado = CargaCurvas(ModelTasCurva, Mone, TipRea, ref ValCurva, ref msj);
                            if ((!Resultado) && msj != "")
                            {
                                throw new System.ArgumentException(msj);
                            }

                            #endregion

                            CargaTablasDinamicas(ModelMor, Nanbe, Coinb, Sexob, Nben, ref LxDin);

                            #region Inicializa y setea variables
                            //'inicialización de variables
                            nmax = 0;
                            cplan = TipPen;

                            Fechap = Nad * 12 + Nmd;
                            Fechrv = Nad * 12 + Nmd + Mesdif;
                            FechaDv = Nad * 12 + Nmd;

                            perdif = 0;
                            mesdif1 = Mesdif; //'* 12 'debe venir en meses
                            pergar = Mesgar;
                            pergar1 = Mesgar; // '* 12
                            mesdifc = mesdif1;
                            mescon = ((Nap * 12) + Nmp) - ((Nad * 12) + Nmd);

                            //OBTIENE LOS MESESCOSTOS
                            if (mescon < (mesdif1 + pergar1))
                            {
                                mescosto = mescon;
                            }
                            else
                            {
                                if (mescon >= mesdif1)
                                {
                                    Mesdif = 0;
                                }
                                mescosto = mescon - (Mesdif + Mesgar);
                            }
                            MesCostoTmp = mescosto;

                            //'Periodo Diferido
                            if (mescon > mesdif1)
                            {
                                Mesdif = 0;
                            }
                            else
                            {
                                Mesdif = mesdif1 > mescon ? mesdif1 : (mescon - mesdif1);
                            }

                            //'Periodo Garantizado
                            if (mescon > (pergar1 + mesdif1))
                            {
                                pergar = 0;
                            }
                            else
                            {
                                pergar = (mescon < mesdif1) ? pergar1 : (pergar1 + mesdif1) - mescon;
                            }
                            Mesgar = pergar;   // '/ 12

                            if (TipRen == "E")
                            {
                                ltot = Mesgar;
                                mesdif1 = 0;
                            }
                            else
                            {
                                ltot = Mesgar + Mesdif;
                            }
                            perdif = Mesdif;
                            

                            ltotCero = 0;

                            if (mescon >= mesdif1)
                            {
                                ltotCero = mescon;
                            }
                            else
                            {
                                ltotCero = (Mesdif == 0) ? mesdif1 - mescon : mesdif1;
                            }
                            #endregion

                            CargarTipoAjuste(ModelFacVac, Mone, Nben, Ncorbe, Coinb, Porcbe, Porcbe_tram, Nanbe, Nmnbe, Fechrv, EdaLim, Nad, Nmd, Ndd, TipRea, TipPen, MesCostoTmp, mescon, facgratif, ref vl_FactorReajuste);

                            #region Calcula Flujos de Pension
                            new_prc = 1;
                            //flujos para tramos

                            if (TipRen == "E")
                            {
                                for (int g = 1; g < 1400; g++)
                                {
                                    fTramos[g] = (g > Mesdif) ? PrcTaf / 100 : 1;
                                }
                            }
                            else
                            {
                                for (int g = 1; g < 1400; g++)
                                {
                                    fTramos[g] = 1;
                                }
                            }
                            //flujos para tramos

                            if (TipPen != "S")
                            {
                                Resultado = Flujos(Nben, ind_cob, vlIndFall, Fechap, ltot, mescon, GtoFun, mescosto, Mesdif, mesdif1, pergar1, Coinb, Porcbe, Ncorbe, Nanbe, Nmnbe, Estudi, LxDin, facgratif, vl_FactorReajuste, fTramos, ref nmax, ref Flupen, ref Flucm, ref FlujosBen, ref fpy, ref fqxy, ref msj);
                            }
                            else //'Calculo de flujos de Sobrevivencia
                            {
                                //SOBREVIVENCIA+
                                Resultado = FlujosSobrevivencia(Nben, swg, TipMod, TipPen, Fechrv, perdif, vlIndFall, Fechap, ltot, mescosto, Mesdif, mesdif1, pergar1, Coinb, Porcbe, Ncorbe, Nanbe, Nmnbe, Estudi, FecFallBen, LxDin, facgratif, vl_FactorReajuste, ref nmax, ref Flupen, ref FlujosBen, ref fpy, ref fqxy, ref msj, Cod_Tope18, mesdifgar);
                            }

                            if ((!Resultado) && msj != "")
                            {
                                throw new System.ArgumentException(msj);
                            }

                            #endregion

                            LimpiarFlujosExcedentes(Nben, TipRen, TipPen, mescon, nmax, ltot, ltotCero, ref FlujosBen, ref Flupen, ref Flucm);

                            if (nmax == 0)
                            {
                                msj = "Tir mayor a 100%. ";
                                throw new System.ArgumentException(msj);
                                //goto SiguientePol;
                            }

                            PenBase = CalculoCurvaTasa(MtoCic, mescosto, ValCurva, Flupen, Flucm, ref nmax, ref factual);
                            PenBase = MtoPen; //PR

                            #region Calcula TCE
                            Resultado = CalculoTCE(Stock, prcTce, nmax, mescosto, PenBase, tvmax, tpr, factual, Flupen, Flucm, ref tce, ref fpagosRes, ref tci, ref msj, item.NumPol);
                            if ((!Resultado) && msj != "")
                            {
                                throw new System.ArgumentException(msj);
                            }
                            #endregion

                            #region CargaTablaResultados

                            if (TipPen != "S")
                            {
                                if (ind_cob == "S")
                                {
                                    if (Coinb[0] == "T")
                                    {
                                        new_prc = 0.7;
                                    }
                                    if (Coinb[0] == "P")
                                    {
                                        new_prc = 0.5;
                                    }
                                    MtoPen = MtoPen / new_prc;
                                }
                            }

                            //SiguientePol:

                            if (TipTbl == "1")
                            {
                                //SACA LOS FLUJOS TOTALES
                                for (i = 1; i <= nmax; i++)
                                {
                                    Console.WriteLine("Memory used before collection:       {0:N0}",
                                    GC.GetTotalMemory(false));

                                    DataRow row = ModelFluTot1Local.NewRow();

                                    row["NUM_POLIZA"] = NumPol;
                                    row["NUM_MESFLU"] = i;
                                    row["MTO_PENSION"] = MtoPen; //Convert.ToDecimal(String.Format("{0:0.00}", MtoPen));
                                    row["MTO_FLUPEN"] = Convert.ToDecimal(String.Format("{0:0.00000000}", Flupen[i]));
                                    row["MTO_FLUSEP"] = Convert.ToDecimal(String.Format("{0:0.00000000}", Flucm[i])); //Flucm[i];
                                    row["MTO_FLUTOT"] = Convert.ToDecimal(String.Format("{0:0.00000000}", (MtoPen * Flupen[i]) + Flucm[i]));
                                    row["PRC_TASTCE"] = Convert.ToDecimal(String.Format("{0:0.00000000}", tce));
                                    row["PRC_TLR"] = Convert.ToDecimal(String.Format("{0:0.00000000}", tci));
                                    //row["MTO_PENSION_METAN"] = Convert.ToDecimal(String.Format("{0:0.00}", 0));
                                    //row["MTO_FLUPEN_METAN"] = Convert.ToDecimal(String.Format("{0:0.00000000}", 0));
                                    //row["MTO_FLUSEP_METAN"] = Convert.ToDecimal(String.Format("{0:0.00000000}", 0));
                                    //row["MTO_FLUTOT_METAN"] = Convert.ToDecimal(String.Format("{0:0.00000000}", 0));
                                    //row["PRC_TASTCE_METAN"] = Convert.ToDecimal(String.Format("{0:0.00}", 0));
                                    row["FEC_FLU"] = vlFechaDev.AddMonths(i - 1).ToString("yyyyMM");

                                    ModelFluTot1Local.Rows.Add(row);
                                }
                                ListModelFluTot1.Add(ModelFluTot1Local);

                                //if (Stock == "N") { queryTas += "UPDATE PP_TMAE_POLIZA SET PRC_TASARES = " + tce + " WHERE NUM_POLIZA = '" + NumPol + "' \n"; }

                                GC.Collect();
                                Console.WriteLine("Memory used after full collection:   {0:N0}",
                                                    GC.GetTotalMemory(true));
                                GCSettings.LargeObjectHeapCompactionMode = GCLargeObjectHeapCompactionMode.CompactOnce;
                                GC.Collect(2, GCCollectionMode.Forced, true, true);

                            }
                            else
                            {
                                //SACA LOS FLUJOS TOTALES
                                for (i = 1; i <= nmax; i++)
                                {
                                    Console.WriteLine("Memory used before collection:       {0:N0}",
                                    GC.GetTotalMemory(false));

                                    DataRow row = ModelFluTot2Local.NewRow();

                                    row["NUM_POLIZA"] = NumPol;
                                    row["NUM_MESFLU"] = i;
                                    row["MTO_PENSION"] = MtoPen; //Convert.ToDecimal(String.Format("{0:0.00}", MtoPen));
                                    row["MTO_FLUPEN"] = Convert.ToDecimal(String.Format("{0:0.00000000}", Flupen[i]));
                                    row["MTO_FLUSEP"] = Convert.ToDecimal(String.Format("{0:0.00000000}", Flucm[i])); //Flucm[i];
                                    row["MTO_FLUTOT"] = Convert.ToDecimal(String.Format("{0:0.00000000}", (MtoPen * Flupen[i]) + Flucm[i]));
                                    row["PRC_TASTCE"] = Convert.ToDecimal(String.Format("{0:0.00000000}", tce));
                                    row["PRC_TLR"] = Convert.ToDecimal(String.Format("{0:0.00000000}", tci));
                                    //row["MTO_PENSION_METAN"] = Convert.ToDecimal(String.Format("{0:0.00}", 0));
                                    //row["MTO_FLUPEN_METAN"] = Convert.ToDecimal(String.Format("{0:0.00000000}", 0));
                                    //row["MTO_FLUSEP_METAN"] = Convert.ToDecimal(String.Format("{0:0.00000000}", 0));
                                    //row["MTO_FLUTOT_METAN"] = Convert.ToDecimal(String.Format("{0:0.00000000}", 0));
                                    //row["PRC_TASTCE_METAN"] = Convert.ToDecimal(String.Format("{0:0.00}", 0));
                                    row["FEC_FLU"] = vlFechaDev.AddMonths(i - 1).ToString("yyyyMM");

                                    ModelFluTot2Local.Rows.Add(row);
                                }
                                ListModelFluTot2.Add(ModelFluTot2Local);

                                //if (Stock == "N") { queryTas += "UPDATE PP_TMAE_POLIZA SET PRC_TASARES = " + tce + " WHERE NUM_POLIZA = '" + NumPol + "' \n"; }

                                GC.Collect();
                                Console.WriteLine("Memory used after full collection:   {0:N0}",
                                                    GC.GetTotalMemory(true));
                                GCSettings.LargeObjectHeapCompactionMode = GCLargeObjectHeapCompactionMode.CompactOnce;
                                GC.Collect(2, GCCollectionMode.Forced, true, true);
                            }

                            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                            if (TipTbl == "1")
                            {
                                //SACA LOS FLUJOS POR BENEFICIARIOS
                                for (j = 0; j <= Nben; j++)
                                {
                                    Console.WriteLine("Memory used before collection:       {0:N0}",
                                    GC.GetTotalMemory(false));
                                    for (i = 1; i <= nmax; i++)
                                    {
                                        DataRow row = ModelFluBen1Local.NewRow();

                                        row["NUM_POLIZA"] = NumPol;
                                        row["NUM_ORDEN"] = j + 1;
                                        row["NUM_EDAD"] = Edadben[j];
                                        row["NUM_MESFLU"] = i;
                                        row["MTO_PENSION"] = MtoPen; //Convert.ToDecimal(String.Format("{0:0.00}", MtoPen));
                                        row["MTO_FLUPEN"] = Convert.ToDecimal(String.Format("{0:0.00000000}", FlujosBen[j, i]));
                                        row["MTO_FLUSEP"] = Convert.ToDecimal(String.Format("{0:0.00000000}", Flucm[i])); //Flucm[i];
                                        if ((j + 1) == 1)
                                        {
                                            row["MTO_FLUTOT"] = Convert.ToDecimal(String.Format("{0:0.00000000}", ((MtoPen * FlujosBen[j, i]) + Flucm[i])));
                                        }
                                        else
                                        {
                                            row["MTO_FLUTOT"] = Convert.ToDecimal(String.Format("{0:0.00000000}", (MtoPen * FlujosBen[j, i])));
                                        }
                                        row["MTO_TPX"] = Convert.ToDecimal(String.Format("{0:0.00000000}", fpy[i]));
                                        row["MTO_QXT"] = Convert.ToDecimal(String.Format("{0:0.00000000}", fqxy[i]));
                                        //row["MTO_PENSION_METAN"] = Convert.ToDecimal(String.Format("{0:0.00}", 0));
                                        //row["MTO_FLUPEN_METAN"] = Convert.ToDecimal(String.Format("{0:0.00000000}", 0));
                                        //row["MTO_FLUSEP_METAN"] = Convert.ToDecimal(String.Format("{0:0.00000000}", 0));
                                        //row["MTO_FLUTOT_METAN"] = Convert.ToDecimal(String.Format("{0:0.00000000}", 0));
                                        //row["MTO_TPX_METAN"] = Convert.ToDecimal(String.Format("{0:0.00000000}", 0));
                                        //row["MTO_QXT_METAN"] = Convert.ToDecimal(String.Format("{0:0.00000000}", 0));
                                        row["FEC_FLU"] = vlFechaDev.AddMonths(i - 1).ToString("yyyyMM");

                                        ModelFluBen1Local.Rows.Add(row);
                                    }
                                }
                                ListModelFluBen1.Add(ModelFluBen1Local);

                                GC.Collect();
                                Console.WriteLine("Memory used after full collection:   {0:N0}",
                                                    GC.GetTotalMemory(true));
                                GCSettings.LargeObjectHeapCompactionMode = GCLargeObjectHeapCompactionMode.CompactOnce;
                                GC.Collect(2, GCCollectionMode.Forced, true, true);
                            }
                            else
                            {
                                //SACA LOS FLUJOS POR BENEFICIARIOS
                                for (j = 0; j <= Nben; j++)
                                {
                                    Console.WriteLine("Memory used before collection:       {0:N0}",
                                    GC.GetTotalMemory(false));
                                    for (i = 1; i <= nmax; i++)
                                    {
                                        DataRow row = ModelFluBen2Local.NewRow();

                                        row["NUM_POLIZA"] = NumPol;
                                        row["NUM_ORDEN"] = j + 1;
                                        row["NUM_EDAD"] = Edadben[j];
                                        row["NUM_MESFLU"] = i;
                                        row["MTO_PENSION"] = MtoPen; //Convert.ToDecimal(String.Format("{0:0.00}", MtoPen));
                                        row["MTO_FLUPEN"] = Convert.ToDecimal(String.Format("{0:0.00000000}", FlujosBen[j, i]));
                                        row["MTO_FLUSEP"] = Convert.ToDecimal(String.Format("{0:0.00000000}", Flucm[i])); //Flucm[i];
                                        if ((j + 1) == 1)
                                        {
                                            row["MTO_FLUTOT"] = Convert.ToDecimal(String.Format("{0:0.00000000}", ((MtoPen * FlujosBen[j, i]) + Flucm[i])));
                                        }
                                        else
                                        {
                                            row["MTO_FLUTOT"] = Convert.ToDecimal(String.Format("{0:0.00000000}", (MtoPen * FlujosBen[j, i])));
                                        }
                                        row["MTO_TPX"] = Convert.ToDecimal(String.Format("{0:0.00000000}", fpy[i]));
                                        row["MTO_QXT"] = Convert.ToDecimal(String.Format("{0:0.00000000}", fqxy[i]));
                                        //row["MTO_PENSION_METAN"] = Convert.ToDecimal(String.Format("{0:0.00000000}", 0));
                                        //row["MTO_FLUPEN_METAN"] = Convert.ToDecimal(String.Format("{0:0.00000000}", 0));
                                        //row["MTO_FLUSEP_METAN"] = Convert.ToDecimal(String.Format("{0:0.00000000}", 0));
                                        //row["MTO_FLUTOT_METAN"] = Convert.ToDecimal(String.Format("{0:0.00000000}", 0));
                                        //row["MTO_TPX_METAN"] = Convert.ToDecimal(String.Format("{0:0.00000000}", 0));
                                        //row["MTO_QXT_METAN"] = Convert.ToDecimal(String.Format("{0:0.00000000}", 0));
                                        row["FEC_FLU"] = vlFechaDev.AddMonths(i - 1).ToString("yyyyMM");

                                        ModelFluBen2Local.Rows.Add(row);
                                    }
                                }
                                ListModelFluBen2.Add(ModelFluBen2Local);

                                GC.Collect();
                                Console.WriteLine("Memory used after full collection:   {0:N0}",
                                                    GC.GetTotalMemory(true));
                                GCSettings.LargeObjectHeapCompactionMode = GCLargeObjectHeapCompactionMode.CompactOnce;
                                GC.Collect(2, GCCollectionMode.Forced, true, true);

                            }

                            //Limpiar estos dos arreglos de la rutina
                            Array.Clear(fpy, 0, 1400);
                            Array.Clear(fqxy, 0, 1400);

                            #endregion
                        }
                    }
                    else
                    {
                        #region Para Casos Especiales
                        if (TipTbl == "1")
                        {
                            foreach (var item in ModelPol)
                            {
                                DataRow row = ModelFluTot1Local.NewRow();

                                row["NUM_POLIZA"] = item.NumPol;
                                row["NUM_MESFLU"] = 0;
                                row["MTO_PENSION"] = 0.00;
                                row["MTO_FLUPEN"] = 0.00000000;
                                row["MTO_FLUSEP"] = 0.00000000;
                                row["MTO_FLUTOT"] = 0.00000000;
                                row["PRC_TASTCE"] = 0.00000000;
                                row["PRC_TLR"] = 0.00000000;
                                //row["MTO_PENSION_METAN"] = 0.00;
                                //row["MTO_FLUPEN_METAN"] = 0.00000000;
                                //row["MTO_FLUSEP_METAN"] = 0.00000000;
                                //row["MTO_FLUTOT_METAN"] = 0.00000000;
                                //row["PRC_TASTCE_METAN"] = 0.00;
                                row["FEC_FLU"] = "";

                                ModelFluTot1Local.Rows.Add(row);

                                foreach (var itemBen in Modelben)
                                {
                                    DataRow row2 = ModelFluBen1Local.NewRow();

                                    row2["NUM_POLIZA"] = itemBen.NumPol;
                                    row2["NUM_ORDEN"] = itemBen.NumOrd;
                                    row2["NUM_EDAD"] = 0;
                                    row2["NUM_MESFLU"] = 0;
                                    row2["MTO_PENSION"] = 0.00000000;
                                    row2["MTO_FLUPEN"] = 0.00000000;
                                    row2["MTO_FLUSEP"] = 0.00000000;
                                    row2["MTO_FLUTOT"] = 0.00000000;
                                    row2["MTO_TPX"] = 0.00000000;
                                    row2["MTO_QXT"] = 0.00000000;
                                    //row2["MTO_PENSION_METAN"] = 0.00;
                                    //row2["MTO_FLUPEN_METAN"] = 0.00000000;
                                    //row2["MTO_FLUSEP_METAN"] = 0.00000000;
                                    //row2["MTO_FLUTOT_METAN"] = 0.00000000;
                                    //row2["MTO_TPX_METAN"] = 0.00000000;
                                    //row2["MTO_QXT_METAN"] = 0.00000000;
                                    row2["FEC_FLU"] = "";

                                    ModelFluBen1Local.Rows.Add(row2);
                                }

                            }
                            ListModelFluTot1.Add(ModelFluTot1Local);
                            ListModelFluBen1.Add(ModelFluBen1Local);
                        }
                        else
                        {
                            foreach (var item in ModelPol)
                            {
                                DataRow row = ModelFluTot2Local.NewRow();

                                row["NUM_POLIZA"] = item.NumPol;
                                row["NUM_MESFLU"] = 0;
                                row["MTO_PENSION"] = 0.00;
                                row["MTO_FLUPEN"] = 0.00000000;
                                row["MTO_FLUSEP"] = 0.00000000;
                                row["MTO_FLUTOT"] = 0.00000000;
                                row["PRC_TASTCE"] = 0.00000000;
                                row["PRC_TLR"] = 0.00000000;
                                //row["MTO_PENSION_METAN"] = 0.00;
                                //row["MTO_FLUPEN_METAN"] = 0.00000000;
                                //row["MTO_FLUSEP_METAN"] = 0.00000000;
                                //row["MTO_FLUTOT_METAN"] = 0.00000000;
                                //row["PRC_TASTCE_METAN"] = 0.00;
                                row["FEC_FLU"] = "";

                                ModelFluTot2Local.Rows.Add(row);

                                foreach (var itemBen in Modelben)
                                {
                                    DataRow row2 = ModelFluBen2Local.NewRow();

                                    row2["NUM_POLIZA"] = itemBen.NumPol;
                                    row2["NUM_ORDEN"] = itemBen.NumOrd;
                                    row2["NUM_EDAD"] = 0;
                                    row2["NUM_MESFLU"] = 0;
                                    row2["MTO_PENSION"] = 0.00000000;
                                    row2["MTO_FLUPEN"] = 0.00000000;
                                    row2["MTO_FLUSEP"] = 0.00000000;
                                    row2["MTO_FLUTOT"] = 0.00000000;
                                    row2["MTO_TPX"] = 0.00000000;
                                    row2["MTO_QXT"] = 0.00000000;
                                    //row2["MTO_PENSION_METAN"] = 0.00;
                                    //row2["MTO_FLUPEN_METAN"] = 0.00000000;
                                    //row2["MTO_FLUSEP_METAN"] = 0.00000000;
                                    //row2["MTO_FLUTOT_METAN"] = 0.00000000;
                                    //row2["MTO_TPX_METAN"] = 0.00000000;
                                    //row2["MTO_QXT_METAN"] = 0.00000000;
                                    row2["FEC_FLU"] = "";

                                    ModelFluBen2Local.Rows.Add(row2);
                                }

                            }
                            ListModelFluTot2.Add(ModelFluTot2Local);
                            ListModelFluBen2.Add(ModelFluBen2Local);
                        }
                        #endregion
                    }

                }
                catch (Exception ex)
                {
                    Console.WriteLine("{0}", "Error en la Rutina - " + ex.Message);
                    _log.Info("Error en Póliza No.: " + ModelPol[0].NumPol + " - " + msj);
                    _log.Info("Error en Tarea de Rutina Nueva: " + ex.Message);
                }


            });
            #endregion
        }

        #region FuncionesPropias
        public double amax0(double arg1, double arg2)
        {
            double xx = 0;
            if (arg1 >= arg2)
            {
                xx = arg1;
            }
            else
            {
                xx = arg2;
            }
            return xx;
        }

        public double amin0(double arg1, double arg2)
        {
            double xx = 0;
            if (arg1 <= arg2)
            {
                xx = arg1;
            }
            else
            {
                xx = arg2;
            }
            return xx;
        }

        public double amin1(double arg1, double arg2, double arg3)
        {
            double xx = 0;
            if (arg1 <= arg2 && arg1 <= arg3)
            {
                xx = arg1;
            }

            if (arg2 <= arg1 && arg2 <= arg3)
            {
                xx = arg2;
            }

            if (arg3 <= arg1 && arg3 <= arg2)
            {
                xx = arg3;
            }
            return xx;
        }
        #endregion

        //Métodos de la rutina(ahora es de esta forma debido a la mejora que se le realizó a la rutina).
        public void CargarTablaGratificacion(int Nad, int Nmd, string DerGra, ref double[] facgratif)
        {
            DateTime fecha1 = new DateTime(Nad, Nmd, 1);
            for (int g = 1; g < 1400; g++)
            {
                facgratif[g] = 1;
                if ((fecha1.Month == 7 || fecha1.Month == 12) && DerGra == "S")
                {
                    facgratif[g] = 2;
                }
                fecha1 = fecha1.AddMonths(1);
            };
        }

        public bool CargaTasaProm(List<beTasasPromedio> ModelTasPro, string Mone, int TipRea, ref double tpr, ref string msj)
        {

            bool vlExisteTA = false;
            var TasPromFil = ModelTasPro.Where(x => x.COD_MONEDA == Mone && x.COD_TIPPREAJUSTE == TipRea).ToList();
            foreach (var itemTP in TasPromFil)
            {
                tpr = itemTP.MTO_VTAPROM;
                tpr = (Math.Pow((1 + (tpr / 100)), (double)Exp)) - 1;
                vlExisteTA = true; //'Si encuentra los parametros de la TM
                break;
            }
            if (!vlExisteTA)
            {
                msj = "Problemas al hallar tasas de Anclaje.";
            }
            return vlExisteTA;
        }

        public bool CargaCurvas(List<beCurvaTasas> ModelTasCurva, string Mone, int TipRea, ref decimal[] ValCurva, ref string msj)
        {
            int intFila = 1;
            bool res = true;
            var CurvaTasas = ModelTasCurva.Where(x => x.COD_MONEDA == Mone && x.COD_TIPPREAJUSTE == TipRea).ToList();
            if (CurvaTasas == null)
            {
                msj = "Problemas al hallar la Curva de Tasas";
                res = false;
            }
            foreach (var itemCT in CurvaTasas)
            {
                intFila = itemCT.NUM_MES;
                ValCurva[intFila] = (decimal)itemCT.MTO_VALOR / 100;
            }
            return res;
        }

        public void CargaTablasDinamicas(List<beMortalidadDinVal> ModelMor, int[] Nanbe, string[] Coinb, string[] Sexob, int Nben, ref decimal[,] LxDin)
        {
            #region Variables
            int i;
            int j;
            int k;
            int anio, Aux;
            int ni = 0;
            int ns = 0;
            string inv, sex;
            decimal FxQx, FxAx, FxQxF, FxQxFm, FxLxF;
            decimal ExpQ, ExoQ;
            double[,,] Lxb = new double[3, 3, 111];
            double[,,] Ax = new double[3, 3, 111];
            decimal[] FxQxFmV = new decimal[1400];
            decimal[] FxLxFV = new decimal[1400];
            #endregion

            #region Tablas de mortalidad
            foreach (var itemMor in ModelMor)
            {
                i = itemMor.i;
                j = itemMor.j;
                k = itemMor.k;

                Lxb[i, j, k] = (double)itemMor.MtoLx;
                Ax[i, j, k] = (double)itemMor.MtoAx;
            }
            #endregion

            #region Tablas de mortalidad por Beneficiario
            for (int b = 0; b <= Nben; b++)
            {
                //OBTIENE FECHA DEL BENEFICICARIO
                anio = Nanbe[b]; //'aa_nac
                inv = Coinb[b];
                sex = Sexob[b];

                if (inv == "S" || inv == "T" || inv == "I" || inv == "P") { ni = 1; };
                if (inv == "N") { ni = 2; };
                if (sex == "M") { ns = 1; };
                if (sex == "F") { ns = 2; };

                // for anual de las tablas
                Aux = 0;
                for (int au = 0; au <= 110; au++)
                {
                    FxQx = (decimal)Lxb[ns, ni, au];
                    FxAx = (decimal)Ax[ns, ni, au];
                    ExoQ = (1 - FxAx);
                    ExpQ = (anio + au - anoBasTM);
                    if (ExpQ < 0) { ExpQ = 0; };
                    FxQxF = FxQx * (decimal)(Math.Pow((double)ExoQ, (double)ExpQ));

                    //for mensual de las tasas
                    for (long am = 0; am <= 11; am++)
                    {
                        FxQxFm = Exp * FxQxF / ((12 - am * FxQxF) / 12);
                        if (Aux == 0)
                        {
                            FxLxF = 100000;
                        }
                        else
                        {
                            FxLxF = FxLxFV[Aux - 1] * (1 - FxQxFmV[Aux - 1]);
                        }
                        LxDin[b, Aux] = Math.Round(FxLxF, 9);
                        FxQxFmV[Aux] = FxQxFm;
                        FxLxFV[Aux] = FxLxF;
                        Aux = Aux + 1;
                    }
                }
            }
            #endregion

        }

        public void CargarTipoAjuste(List<beTasaFacVac> ModelFacVac, string Mone, int Nben, int[] Ncorbe, string[] Coinb, double[] Porcbe, double[] Porcbe_tram, int[] Nanbe, int[] Nmnbe, long Fechrv, long EdaLim, int Nad, int Nmd, int Ndd, int TipRea, string TipPen, double MesCostoTmp, long mescon, double[] facgratif, ref double[] vl_FactorReajuste)
        {

            #region Variables
            long valpx, MesHijoDif_EdaLim;
            long edabe = 0, edaberv = 0;
            int ax = 0, Fechan;
            int ki = 0, ki2 = 0, paso1 = 0;
            DateTime FecCot1, FecCot2, FecCot3;
            double TEM = 0;
            double TET = 0;
            double sumCostosN, sumaCostTotalMes, mescotoAlt;
            //Calcula el factor indexado
            double FxVacPri = 0, FxVacMes = 0, valIPCMen = 1, FxVacMesAnt = 0;
            #endregion

            if (TipRea == 0)
            {
                for (int li = 1; li < 1400; li++)
                {
                    vl_FactorReajuste[li] = 1;
                }
            }
            else if (TipRea == 2)
            {
                TEM = 1 + ((Math.Pow((1 + 0.02), (double)Exp)) - 1);
                TET = 1.00496293157320;

                FecCot1 = new DateTime(Nad, Nmd, Ndd);

                switch (FecCot1.Month)
                {
                    case 1:
                    case 4:
                    case 7:
                    case 10:
                        ki2 = 3;
                        paso1 = 3;
                        break;
                    case 2:
                    case 5:
                    case 8:
                    case 11:
                        ki2 = 2;
                        paso1 = 3;
                        break;
                    case 3:
                    case 6:
                    case 9:
                    case 12:
                        ki2 = 1;
                        paso1 = 2;
                        break;
                }
                for (ki = 1; ki < 1400; ki++)
                {
                    if (ki == 1)
                    {
                        vl_FactorReajuste[ki] = 1;
                    }
                    else
                    {
                        if (ki <= ki2)
                        {
                            vl_FactorReajuste[ki] = vl_FactorReajuste[ki - 1];// *TEM;
                        }
                        else
                        {
                            if (ki <= paso1)
                            {
                                vl_FactorReajuste[ki] = vl_FactorReajuste[ki - 1] * Math.Pow(TEM, ki2);
                            }
                            else
                            {
                                if (FecCot1.Month == 4 || FecCot1.Month == 7 || FecCot1.Month == 10 || FecCot1.Month == 1)
                                {
                                    vl_FactorReajuste[ki] = vl_FactorReajuste[ki - 1] * TET;
                                }
                                else
                                {
                                    vl_FactorReajuste[ki] = vl_FactorReajuste[ki - 1];
                                }
                            }
                        }
                    }
                    FecCot1 = FecCot1.AddMonths(1);
                }

                ki2 = 0;
                sumCostosN = 0;
                sumaCostTotalMes = 0;
                mescotoAlt = 0;
                //'RRR OBTIENE LOS MESES COSTOS SOLO EN CASO DE SOBREVIVENCIA 31/08/2012 **************************************

                if (TipPen == "S")
                {
                    valpx = 1;
                    if (MesCostoTmp > 0)
                    {
                        for (ax = 0; ax <= MesCostoTmp - 1; ax++)
                        {
                            sumCostosN = 0;
                            sumaCostTotalMes = 0;
                            for (int j = 0; j <= Nben; j++)
                            {
                                if (Ncorbe[j] == 30)
                                {
                                    Fechan = Nanbe[j] * 12 + Nmnbe[j];
                                    edabe = ((Nad * 12 + Nmd) - Fechan) + ax;
                                    edaberv = Fechrv - Fechan; //'((Nad * 12 + Nmd + Mesdif) - Fechan)
                                    if (edabe > Fintab)
                                    {
                                        edabe = Fintab;
                                    }
                                    else
                                    {
                                        if (edabe < 1) { edabe = 1; };
                                    }
                                    if (!(edaberv >= EdaLim && Coinb[j] == "N"))
                                    {
                                        if (Coinb[j] != "N")
                                        {
                                            sumCostosN = 1 * Porcbe[j] * vl_FactorReajuste[ax + 1] * facgratif[ax + 1];
                                        }
                                        else
                                        {
                                            MesHijoDif_EdaLim = EdaLim - edaberv;
                                            if (MesHijoDif_EdaLim < 0) { MesHijoDif_EdaLim = 0; };
                                            if (MesHijoDif_EdaLim == 0) { valpx = 0; };
                                            if (mescon > MesHijoDif_EdaLim)
                                            {
                                                sumCostosN = valpx * Porcbe_tram[j] * vl_FactorReajuste[ax + 1] * facgratif[ax + 1];
                                            }
                                            else
                                            {
                                                sumCostosN = 1 * Porcbe[j] * vl_FactorReajuste[ax + 1] * facgratif[ax + 1];
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    sumCostosN = 1 * Porcbe[j] * vl_FactorReajuste[ax + 1] * facgratif[ax + 1];
                                }
                                sumaCostTotalMes = sumaCostTotalMes + sumCostosN;
                            }
                            mescotoAlt = mescotoAlt + sumaCostTotalMes;
                        }
                    }
                }
                //'************************************************************************************************************
            }
            else if (TipRea == 1)
            {
                FecCot3 = new DateTime(Nad, Nmd, 1);
                FecCot1 = new DateTime(Nad, Nmd, 1);
                FecCot1 = FecCot1.AddMonths(-1);
                FecCot2 = FecCot1;
                for (ki = 1; ki <= Fintab; ki++)
                {
                    FxVacPri = ModelFacVac.Where(x => x.FEC_IPC == FecCot1).Select(x => x.MTO_IPC).SingleOrDefault();
                    FxVacMes = ModelFacVac.Where(x => x.FEC_IPC == FecCot2).Select(x => x.MTO_IPC).SingleOrDefault();

                    if (FxVacPri == 0)
                    {
                        FxVacPri = 1;
                        FxVacMes = 1;
                    }
                    if (FxVacMes == 0)
                    {
                        if (FecCot3.Month == 4 || FecCot3.Month == 7 || FecCot3.Month == 10 || FecCot3.Month == 1)
                        {
                            valIPCMen = FxVacMesAnt / FxVacPri;
                        }
                        vl_FactorReajuste[ki] = valIPCMen;

                        goto sale;
                    }
                    else
                    {
                        if (FecCot3.Month == 4 || FecCot3.Month == 7 || FecCot3.Month == 10 || FecCot3.Month == 1)
                        {
                            valIPCMen = FxVacMes / FxVacPri;
                        }
                        vl_FactorReajuste[ki] = valIPCMen;
                    }
                    FxVacMesAnt = FxVacMes;

                    sale:
                    FecCot2 = FecCot2.AddMonths(1);
                    FecCot3 = FecCot3.AddMonths(1);
                }

                sumCostosN = 0;
                sumaCostTotalMes = 0;
                mescotoAlt = 0;

                if (TipPen == "S")
                {
                    if (Mone == "NS")
                    {
                        valpx = 1;
                        if (mescon > 0)
                        {
                            for (ax = 0; ax <= mescon - 1; ax++)
                            {
                                sumCostosN = 0;
                                sumaCostTotalMes = 0;
                                for (int j = 0; j <= Nben; j++)
                                {
                                    if (Ncorbe[j] == 30)
                                    {
                                        Fechan = Nanbe[j] * 12 + Nmnbe[j];
                                        edabe = ((Nad * 12 + Nmd) - Fechan) + ax;
                                        edaberv = Fechrv - Fechan;
                                        if (edabe > Fintab)
                                        {
                                            edabe = Fintab;
                                        }
                                        else
                                        {
                                            if (edabe < 1) { edabe = 1; };
                                        }
                                        if (edaberv >= EdaLim && Coinb[j] == "N")
                                        {

                                        }
                                        else
                                        {
                                            if (Coinb[j] != "N")
                                            {
                                                sumCostosN = 1 * Porcbe[j] * vl_FactorReajuste[ax + 1] * facgratif[ax + 1];
                                            }
                                            else
                                            {
                                                MesHijoDif_EdaLim = EdaLim - edaberv;
                                                if (MesHijoDif_EdaLim < 0) { MesHijoDif_EdaLim = 0; };
                                                if (MesHijoDif_EdaLim == 0) { valpx = 0; };
                                                if (mescon > MesHijoDif_EdaLim)
                                                {
                                                    sumCostosN = valpx * Porcbe_tram[j] * vl_FactorReajuste[ax + 1] * facgratif[ax + 1];
                                                }
                                                else
                                                {
                                                    sumCostosN = 1 * Porcbe[j] * vl_FactorReajuste[ax + 1] * facgratif[ax + 1];
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        sumCostosN = 1 * Porcbe[j] * vl_FactorReajuste[ax + 1] * facgratif[ax + 1];
                                    }
                                    sumaCostTotalMes = sumaCostTotalMes + sumCostosN;
                                }
                                mescotoAlt = mescotoAlt + sumaCostTotalMes;
                            }
                        }
                    }
                }
                else
                {
                    sumCostosN = 0;
                    sumaCostTotalMes = 0;
                    mescotoAlt = 0;
                    if (Mone == "NS")
                    {
                        if (mescon > 0)
                        {
                            for (ax = 0; ax <= mescon - 1; ax++)
                            {
                                sumCostosN = 1 * Porcbe[0] * vl_FactorReajuste[ax + 1] * facgratif[ax + 1];
                                mescotoAlt = mescotoAlt + sumCostosN;
                            }
                        }
                    }
                }
            }
        }

        public bool Flujos(int Nben, string ind_cob, string vlIndFall, int Fechap, int ltot, int mescon, double GtoFun, int mescosto, int Mesdif, int mesdif1, int pergar1, string[] Coinb, double[] Porcbe, int[] Ncorbe, int[] Nanbe, int[] Nmnbe, string[] Estudi, decimal[,] LxDin, double[] facgratif, double[] vl_FactorReajuste, double[] fTramos, ref int nmax, ref double[] Flupen, ref double[] Flucm, ref double[,] FlujosBen, ref double[] pfpy, ref double[] pfqxy, ref string msj)
        {
            #region Variables
            double px, py = 0, qx = 0, qxt, tpx;
            double valorTemp;
            double mdif, nmdif;
            double[] fqxt = new double[1400];
            double[] fqxy = new double[1400];
            double[] ftpx = new double[1400];
            double[] fpy = new double[1400];
            double[] fpx = new double[1400];
            double[] Penben = new double[20];
            double[] Edadben = new double[20];
            double new_prc = 0, EdaLim;
            int Fechan;
            int edaca = 0, edacai, edacas, edabe, edalbe, nmdiga, edacax, edadRN;
            int limite, limite1, limite2, imas1;
            bool res = true;
            #endregion

            for (int j = 0; j <= Nben; j++)
            {
                for (int g = 0; g < 1400; g++)
                {
                    ftpx[g] = 0;
                    fqxt[g] = 0;
                    fpy[g] = 0;
                    fqxy[g] = 0;
                }
                nmdiga = mesdif1 + pergar1;
                if (ind_cob == "S")
                {
                    if (Coinb[0] == "T")
                    {
                        new_prc = 0.7;
                    }
                    if (Coinb[0] == "P")
                    {
                        new_prc = 0.5;
                    }
                    Penben[j] = Porcbe[j] * new_prc;
                }
                else
                {
                    Penben[j] = Porcbe[j];
                }

                if (Ncorbe[j] == 99 && j == 0)
                {
                    Fechan = Nanbe[j] * 12 + Nmnbe[j];
                    edaca = Fechap - Fechan;
                    Edadben[j] = edaca;

                    if (edaca <= 0 || edaca > Fintab)
                    {
                        msj = "Error Edad es mayor a final de tabla Mortal y menor a 0. ";
                        res = false;
                        return res;
                    }
                    limite1 = (vlIndFall == "S") ? ltot - 1 : Fintab - edaca;
                    nmax = limite1;

                    if (vlIndFall == "S") ////29/12/2020
                    {
                        limite1 = (ltot + mescon) - 1;
                        nmax = limite1;
                    }
                    try
                    {
                        for (int i = 0; i <= limite1; i++)
                        {
                            imas1 = i + 1;
                            edacax = edaca + i;
                            edacai = edacax + 1;
                            edacai = (int)amin0(edacai, 1334);

                            if (i < mescosto)
                            {
                                tpx = 1;
                                qxt = 0;
                                px = 1;
                                fpx[i] = 1;
                                fqxt[i] = 1;
                            }
                            else
                            {
                                if (LxDin[j, edacai] == 0)
                                {
                                    qxt = 1;
                                }
                                else
                                {
                                    qxt = (double)(1 - (LxDin[j, edacai] / LxDin[j, edacax]));
                                }


                                if (i == 0)
                                {
                                    tpx = 1;
                                    px = tpx;
                                }
                                else
                                {
                                    tpx = ftpx[imas1 - 1] * (1 - fqxt[imas1 - 1]);

                                    if (i < ltot)
                                    {
                                        px = 1;
                                    }
                                    else
                                    {
                                        px = tpx;
                                        if (i >= Mesdif && i < nmdiga) { px = 1; };
                                    }
                                }

                            }
                            valorTemp = px * Penben[j] * facgratif[imas1] * vl_FactorReajuste[imas1] * fTramos[imas1];
                            Flupen[imas1] = Flupen[imas1] + valorTemp;

                            if (vlIndFall != "S")
                            {
                                //saca el gasto de Sepelio
                                if (i > 0)
                                {
                                    if (i <= mescon)
                                    {
                                        qx = 0;
                                    }
                                    else
                                    {
                                        qx = ftpx[imas1 - 1] - tpx;
                                    }
                                }
                                Flucm[imas1] = Flucm[imas1] + GtoFun * qx;
                                
                            }
                            else
                            {
                                tpx = 0; //29/12/2020
                            }

                            ftpx[imas1] = tpx;
                            fqxt[imas1] = qxt;
                            fpx[imas1] = tpx;
                            edacas = edacai + 1;
                            if (edacas == 1334)
                            {
                                break;
                            };

                            //aca debe ir listado de resultado
                            FlujosBen[j, imas1] = valorTemp;
                        }
                    }
                    catch (Exception ex)
                    {
                        msj = "Problemas en los Flujos de Titular de la Rutina. ";
                        res = false;
                        return res;
                    }
                }
                if (Ncorbe[j] != 99)
                {
                    //Penben[j] = porcbe_ori[j];
                    Fechan = (Nanbe[j] * 12 + Nmnbe[j]);
                    edabe = Fechap - Fechan;
                    edadRN = 0;
                    if (edabe < 1) { edadRN = edabe; edabe = 0; };
                    Edadben[j] = edabe;
                    if (edabe > Fintab)
                    {
                        msj = "Error Edad es mayor a final de tabla Mortal y menor a 0. ";
                        //ListaResultador.Mensaje = msj;
                        res = false;
                        return res;
                    }
                    if (Ncorbe[j] == 10 || Ncorbe[j] == 11 || Ncorbe[j] == 20 || Ncorbe[j] == 21 || Ncorbe[j] == 41 || Ncorbe[j] == 42 || ((Ncorbe[j] >= 30 && Ncorbe[j] < 40) && (Coinb[j] != "N")))
                    {
                        edacai = 0;
                        limite1 = Fintab - edabe - 1;
                        nmax = (int)amax0(nmax, limite1);
                        try
                        {
                            for (int i = 0; i <= limite1; i++)
                            {
                                imas1 = i + 1;
                                edalbe = edabe + i;
                                edacai = edalbe + 1;
                                edacai = (int)amin0(edacai, Fintab);

                                if (i < mescosto)
                                {
                                    tpx = 1;
                                    qxt = 0;
                                    py = 0;
                                    fpy[i] = 1;
                                    fqxy[i] = 1;
                                }
                                else
                                {
                                    //qxt = 1 - (Ly[nsbe, nibe, edacai] / Ly[nsbe, nibe, edalbe]);
                                    qxt = (double)(1 - (LxDin[j, edacai] / LxDin[j, edalbe]));
                                    if (i == 0)
                                    {
                                        tpx = 1;
                                        py = tpx;
                                    }
                                    else
                                    {
                                        tpx = fpy[i - 1] * (1 - fqxy[i - 1]);

                                        if (i < ltot)
                                        {
                                            py = 0;
                                        }
                                        else
                                        {
                                            py = tpx;
                                            if (i >= Mesdif && i < nmdiga) { py = 0; };
                                        }
                                    }

                                }
                                fpy[i] = tpx;
                                fqxy[i] = qxt;
                                valorTemp = py * (1 - fpx[imas1]) * facgratif[imas1] * Penben[j] * vl_FactorReajuste[imas1] * fTramos[imas1];
                                Flupen[imas1] = Flupen[imas1] + valorTemp;

                                //aca debe ir listado de resultado
                                FlujosBen[j, imas1] = valorTemp;
                            }
                        }
                        catch (Exception ex)
                        {
                            msj = "Problemas en los Flujos de Conyugue, Padres o hijos Invalidos de la Rutina. ";
                            res = false;
                            return res;
                        }
                    }
                    else
                    {
                        if (Ncorbe[j] >= 30 && Ncorbe[j] < 40)
                        {
                            //ActualizaXMLDET(pathD, j + 1, "PRC_PENSIONREP", CStr(Penben(j) * 100))
                            if (Estudi[j] == "N")
                            {
                                EdaLim = 18 * 12;
                            }
                            else
                            {
                                EdaLim = 28 * 12;
                            }

                            if (edabe > EdaLim)
                            {
                                //NO HACE NADA
                            }
                            else
                            {
                                mdif = EdaLim - edabe;
                                nmdif = mdif + edadRN;
                                limite2 = Fintab - edaca;
                                limite = (int)amin0(nmdif, limite2) - 1;
                                nmax = (int)amax0(nmax, limite);
                                try
                                {
                                    for (int i = 0; i <= mdif - 1; i++)
                                    {
                                        imas1 = i + 1;
                                        edalbe = edabe + i;
                                        edacai = edalbe + 1;
                                        edacai = (int)amin0(edacai, Fintab);

                                        if (i < mescosto)
                                        {
                                            tpx = 1;
                                            qxt = 0;
                                            py = 0;
                                            fpy[i] = 1;
                                            fqxy[i] = 1;
                                        }
                                        else
                                        {
                                            //qxt = 1 - (Ly[nsbe, nibe, edacai] / Ly[nsbe, nibe, edalbe]);
                                            qxt = (double)(1 - (LxDin[j, edacai] / LxDin[j, edalbe]));
                                            if (i == 0)
                                            {
                                                tpx = 1;
                                            }
                                            else
                                            {
                                                tpx = fpy[i - 1] * (1 - fqxy[i - 1]);

                                                if (i < ltot)
                                                {
                                                    py = 0;
                                                }
                                                else
                                                {
                                                    py = tpx;
                                                    if (i >= Mesdif && i < nmdiga) { py = 0; };
                                                }
                                            }

                                        }
                                        fpy[i] = tpx;
                                        fqxy[i] = qxt;
                                        valorTemp = py * (1 - fpx[imas1]) * facgratif[imas1] * Penben[j] * vl_FactorReajuste[imas1] * fTramos[imas1];
                                        Flupen[imas1] = Flupen[imas1] + valorTemp;

                                        //aca debe ir listado de resultado
                                        FlujosBen[j, imas1] = valorTemp;
                                    }
                                }
                                catch (Exception ex)
                                {
                                    msj = "Problemas en los Flujos de hijos Sanos de la Rutina. ";
                                    res = false;
                                    return res;
                                }
                            }
                        }
                    }
                }
            }
            pfpy = fpy;
            pfqxy = fqxy;
            return res;
        }

        public bool FlujosSobrevivencia(int Nben, string swg, string TipMod, string TipPen, int Fechrv, int perdif, string vlIndFall, int Fechap, int ltot, int mescosto, int Mesdif, int mesdif1, int pergar1, string[] Coinb, double[] Porcbe, int[] Ncorbe, int[] Nanbe, int[] Nmnbe, string[] Estudi, string[] FecFallBen, decimal[,] LxDin, double[] facgratif, double[] vl_FactorReajuste, ref int nmax, ref double[] Flupen, ref double[,] FlujosBen, ref double[] pfpy, ref double[] pfqxy, ref string msj, string[] Cod_Tope18, int mesdifgar)
        {

            #region Variables
            string Alt = "";
            double py, qxt, tpx;
            double valorTemp;
            double mdif, nmdif;
            double[] fqxy = new double[1400];
            double[] fpy = new double[1400];
            double[] Penben = new double[20];
            double[] Edadben = new double[20];
            double EdaLim;
            int Fechan, an;
            int edacai, edabe, edalbe = 0, nmdiga, edadRN, edaberv;
            int limite1, imas1;
            bool res = true;
            #endregion

            int limiteA;
            for (int j = 0; j <= Nben; j++)
            {
                for (long g = 0; g < 1400; g++)
                {
                    fpy[g] = 0;
                    fqxy[g] = 0;
                }
                if (Ncorbe[j] == 99)
                {
                    goto Next;
                }

                nmdiga = mesdif1 + pergar1;

                Penben[j] = Porcbe[j];
                Fechan = Nanbe[j] * 12 + Nmnbe[j];
                edabe = Fechap - Fechan;
                edadRN = 0;
                if (edabe < 0) { edadRN = edabe * -1; edabe = 0; };
                if ((edabe + ltot) > Fintab)
                {
                    Fintab = 1400;
                }
                else
                {
                    Fintab = 1332;
                }
                Edadben[j] = edabe;
                edaberv = Fechrv - Fechan;

                if (edabe > Fintab)
                {
                    //'Mensaje = "Error en Edad del beneficiario es mayor al limite de la tabla de mortalidad"
                    msj = "Error Edad es mayor a final de tabla Mortal y menor a 0. ";
                    res = false;
                    return res;
                }

                //'calculo de renta vitalicias
                if (Ncorbe[j] == 10 || Ncorbe[j] == 11 || Ncorbe[j] == 20 || Ncorbe[j] == 21 || Ncorbe[j] == 41 || Ncorbe[j] == 42 || ((Ncorbe[j] >= 30 && Ncorbe[j] < 40) && (Coinb[j]) != "N"))
                {
                    limite1 = Fintab - edabe - 1;
                    nmax = (int)amax0(nmax, limite1);
                    if (vlIndFall == "S")
                    {
                        limite1 = ltot;
                        nmax = limite1;
                    }
                    if (string.IsNullOrEmpty(FecFallBen[j]) == false)
                    {
                        limite1 = nmdiga - 1;
                    }
                    tpx = 1;
                    for (int i = 0; i <= limite1; i++)
                    {
                        imas1 = i + 1;
                        edalbe = edabe + i;
                        edacai = edalbe + 1;
                        edacai = (int)amin0(edacai, Fintab);

                        if (i < mescosto)
                        {
                            tpx = 1;
                            qxt = 0;
                            py = 1;
                            fpy[i] = 1;
                            fqxy[i] = 1;
                        }
                        else
                        {
                            if (LxDin[j, edalbe] == 0)
                            {
                                qxt = 1;
                            }
                            else
                            {
                                qxt = (double)(1 - (LxDin[j, edacai] / LxDin[j, edalbe]));
                            }

                            if (i == 0)
                            {
                                tpx = 1;
                                py = tpx;
                            }
                            else
                            {
                                tpx = fpy[i - 1] * (1 - fqxy[i - 1]);

                                if (i < ltot)
                                {
                                    py = 1;
                                }
                                else
                                {
                                    py = tpx;
                                    if (i >= Mesdif && i < nmdiga) { py = 1; };
                                }
                            }
                        }

                        fpy[i] = tpx;
                        fqxy[i] = qxt;
                        valorTemp = py * Penben[j] * facgratif[imas1] * vl_FactorReajuste[imas1];
                        Flupen[imas1] = Flupen[imas1] + valorTemp;

                        //aca debe ir listado de resultado
                        FlujosBen[j, imas1] = valorTemp;
                    }
                }
                else
                {
                    if (Ncorbe[j] >= 30 && Ncorbe[j] < 40)
                    {
                        //if (Estudi[j] == "N")
                        if (Cod_Tope18[j] == "N")
                        {
                            //EdaLim = 18 * 12;
                            EdaLim = 28 * 12;
                        }
                        else
                        {
                            //EdaLim = 28 * 12;
                            EdaLim = 18 * 12;
                        }

                        if (Cod_Tope18[j] == "N")
                        {
                            if ((edaberv > EdaLim && Coinb[j] == "N"))
                            {
                                Penben[j] = 0;
                            }
                            else
                            {
                                mdif = EdaLim - edabe;
                                nmdif = mdif + edadRN;
                                limiteA = (int)amax0(nmdiga, nmdif);
                                nmax = (int)amax0(limiteA, nmax);
                                if (TipMod == "G" && TipPen == "S")
                                {
                                    limiteA = (int)amax0(nmdiga, nmdif);
                                    nmax = (int)amax0(limiteA, nmax);
                                }
                                if (vlIndFall == "S")
                                {
                                    limite1 = ltot;
                                    nmax = limite1;
                                }
                                //***
                                an = 1;
                                for (int i = 0; i <= limiteA - 1; i++)
                                {
                                    imas1 = i + 1;
                                    if (edadRN != 0)
                                    {
                                        if (i <= (edadRN))
                                        {
                                            edacai = 0;
                                        }
                                        else
                                        {
                                            edalbe = edabe + an;
                                            edacai = edalbe + 1;
                                            an++;
                                        }
                                    }
                                    else
                                    {
                                        edalbe = edabe + i;
                                        edacai = edalbe + 1;
                                    }
                                    edacai = (int)amin0(edacai, Fintab);

                                    if (i < mescosto)
                                    {
                                        tpx = 1;
                                        qxt = 0;
                                        py = 1;
                                        fpy[i] = 1;
                                        fqxy[i] = 1;
                                    }
                                    else
                                    {
                                        qxt = (double)(1 - (LxDin[j, edacai] / LxDin[j, edalbe]));
                                        if (i == 0)
                                        {
                                            tpx = 1;
                                            py = tpx;
                                        }
                                        else
                                        {
                                            if (edadRN != 0)
                                            {
                                                tpx = fpy[edalbe - 1] * (1 - fqxy[edalbe - 1]);
                                            }
                                            else
                                            {
                                                tpx = fpy[i - 1] * (1 - fqxy[i - 1]);
                                            }
                                            if (i < ltot)
                                            {
                                                py = 1;
                                            }
                                            else
                                            {
                                                py = tpx;
                                                if (swg == "S" && i < nmdiga) { py = 1; }
                                                if (i < nmdiga) { py = 1; }
                                                if (i < perdif) { py = 0; }

                                            }
                                        }
                                    }
                                    if (edadRN != 0)
                                    {
                                        fpy[edalbe] = tpx;
                                        fqxy[edalbe] = qxt;
                                    }
                                    else
                                    {
                                        fpy[i] = tpx;
                                        fqxy[i] = qxt;
                                    }

                                    valorTemp = py * Penben[j] * facgratif[imas1] * vl_FactorReajuste[imas1];
                                    Flupen[imas1] = Flupen[imas1] + valorTemp;

                                    //aca debe ir listado de resultado
                                    FlujosBen[j, imas1] = valorTemp;
                                }
                            }
                        }
                        else
                        {
                            mdif = EdaLim - edabe;
                            nmdif = mdif + edadRN;
                            limiteA = (int)amax0(nmdiga, nmdif);
                            nmax = (int)amax0(limiteA, nmax);
                            if (TipMod == "G" && TipPen == "S")
                            {
                                limiteA = mesdifgar;//(int)amax0(nmdiga, nmdif);
                                nmax = (int)amax0(limiteA, nmax);
                            }
                            if (vlIndFall == "S")
                            {
                                limite1 = ltot;
                                nmax = limite1;
                            }
                            //***
                            an = 1;
                            for (int i = 0; i <= limiteA - 1; i++)
                            {
                                imas1 = i + 1;
                                if (edadRN != 0)
                                {
                                    if (i <= (edadRN))
                                    {
                                        edacai = 0;
                                    }
                                    else
                                    {
                                        edalbe = edabe + an;
                                        edacai = edalbe + 1;
                                        an++;
                                    }
                                }
                                else
                                {
                                    edalbe = edabe + i;
                                    edacai = edalbe + 1;
                                }
                                edacai = (int)amin0(edacai, Fintab);

                                if (i < mescosto)
                                {
                                    tpx = 1;
                                    qxt = 0;
                                    py = 1;
                                    fpy[i] = 1;
                                    fqxy[i] = 1;
                                }
                                else
                                {
                                    qxt = (double)(1 - (LxDin[j, edacai] / LxDin[j, edalbe]));
                                    if (i == 0)
                                    {
                                        tpx = 1;
                                        py = tpx;
                                    }
                                    else
                                    {
                                        if (edadRN != 0)
                                        {
                                            tpx = fpy[edalbe - 1] * (1 - fqxy[edalbe - 1]);
                                        }
                                        else
                                        {
                                            tpx = fpy[i - 1] * (1 - fqxy[i - 1]);
                                        }
                                        if (i < ltot)
                                        {
                                            py = 1;
                                        }
                                        else
                                        {
                                            py = tpx;
                                            if (swg == "S" && i < nmdiga) { py = 1; }
                                            if (i < nmdiga) { py = 1; }
                                            if (i < perdif) { py = 0; }

                                        }
                                    }
                                }
                                if (edadRN != 0)
                                {
                                    fpy[edalbe] = tpx;
                                    fqxy[edalbe] = qxt;
                                }
                                else
                                {
                                    fpy[i] = tpx;
                                    fqxy[i] = qxt;
                                }

                                valorTemp = py * Penben[j] * facgratif[imas1] * vl_FactorReajuste[imas1];
                                Flupen[imas1] = Flupen[imas1] + valorTemp;

                                //aca debe ir listado de resultado
                                FlujosBen[j, imas1] = valorTemp;
                            }
                        }
                        }
                }
                Next:
                Alt = "";
            }
            pfpy = fpy;
            pfqxy = fqxy;
            return res;
        }

        public void LimpiarFlujosExcedentes(int Nben, string TipRen, string TipPen, int mescon, int nmax, int ltot, int ltotCero, ref double[,] FlujosBen, ref double[] Flupen, ref double[] Flucm)
        {
            if (TipRen != "E")
            {
                if (TipPen == "S" || TipPen == "I" || TipPen == "V" || TipPen == "A" || TipPen == "P")
                {
                    for (int i = 1; i <= nmax; i++)
                    {
                        if (i <= (ltot + 1))
                        {
                            if (i <= (ltotCero))
                            {
                                for (int j = 0; j <= Nben; j++)
                                {
                                    FlujosBen[j, i] = 0;
                                    Flupen[i] = 0;
                                    Flucm[i] = 0;
                                }
                            }
                        }
                        else
                        {
                            if (i <= (ltotCero))
                            {
                                for (int j = 0; j <= Nben; j++)
                                {
                                    FlujosBen[j, i] = 0;
                                    Flupen[i] = 0;
                                    Flucm[i] = 0;
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                for (int i = 1; i <= nmax; i++)
                {
                    if (i <= (mescon))
                    {
                        for (int j = 0; j <= Nben; j++)
                        {
                            FlujosBen[j, i] = 0;
                            Flupen[i] = 0;
                            Flucm[i] = 0;
                        }
                    }
                }
            }
        }

        public double CalculoCurvaTasa(double MtoCic, int mescosto, decimal[] ValCurva, double[] Flupen, double[] Flucm, ref int nmax, ref double[] factual)
        {
            nmax = nmax + 1;
            double sumapx = 0;
            double sumaqx = 0;
            double Expf;
            double actual;
            int cr = 1;
            for (int i = 0; i <= nmax; i++)
            {
                if (i >= mescosto)
                {
                    Expf = (double)cr / 12;
                    actual = Math.Pow((double)(1 + ValCurva[cr]), Expf);
                    sumapx = sumapx + (double)(Flupen[i + 1] / actual);
                    factual[cr] = actual;
                    cr = cr + 1;
                }
                sumaqx = sumaqx + Flucm[i + 1];
            }

            return (sumapx <= 0) ? 0 : (MtoCic - sumaqx) / sumapx;
        }

        public bool CalculoTCE(string Stock, double prcTce, int nmax, int mescosto, double PenBase, double tvmax, double tpr, double[] factual, double[] Flupen, double[] Flucm, ref double tce, ref double[] fpagosRes, ref double ptci, ref string msj, string numPol)
        {

            #region Variables
            double tci;
            double vpte = 0;
            double vpte2 = 0;
            double difres;
            double tir = 0;
            double tinc = 0.00001;
            double Tasa;
            int i, cr;
            bool res = true;
            double[] ArrTci = new double[1400];

            #endregion

            if (Stock == "S")
            {
                tce = prcTce;
            }
            else
            {
                //CalTce:
                //EMPIEZA LA RUTINA DEL CALCULO DE TCE 

                Tasa = (tir / 100);
                cr = 1;
                for (i = 0; i <= nmax; i++)
                {
                    if (i >= mescosto)
                    {
                        fpagosRes[cr] = (Flupen[i + 1] * PenBase + Flucm[i + 1]) / Math.Pow((1 + Tasa), cr);
                        vpte2 = vpte2 + (((Flupen[i + 1] * PenBase) + Flucm[i + 1]) / factual[cr]);
                        cr = cr + 1;
                    }
                }
                ArrTci[0] = vpte2;

                cr = 0;
                for (i = 0; i <= nmax; i++)
                {
                    if (i >= mescosto)
                    {
                        vpte = (Flupen[i + 1] * PenBase + Flucm[i + 1]); //((Flupen[i + 1] * PenBase + Flucm[i + 1]) / Math.Pow((1 + Tasa), cr));
                        if (cr == 0)
                        {
                            ArrTci[cr] = vpte - vpte2;
                        }
                        else
                        {
                            ArrTci[cr] = vpte;
                        }
                        cr = cr + 1;
                    }
                }

                //vpte es columna DS 
                //vpte2 es columna DR7

                double guess = 0.0025;
                tci = Financial.IRR(ref ArrTci, guess);
                //tci = Math.Pow(1 + tci, (double)Exp) - 1;
                ptci = tci; //Se agregó esta línea para retornar en el parámetro este valor que se ha estado insertando en BD.
                tvmax = ((Math.Pow((1 + (tvmax / 100)), (double)Exp)) - 1);
                tce = amin1(tvmax, tci, tpr);
                tce = ((Math.Pow((1 + tce), 12)) - 1) * 100;
            }
            return res;
        }

        public string DataTableToJSONWithStringBuilder(DataTable table, string nombre, string ruta)
        {
            var JSONString = new StringBuilder();
            if (table.Rows.Count > 0)
            {
                JSONString.Append("[");
                for (int i = 0; i < table.Rows.Count; i++)
                {
                    JSONString.Append("{");
                    for (int j = 0; j < table.Columns.Count; j++)
                    {
                        if (j < table.Columns.Count - 1)
                        {
                            JSONString.Append("\"" + table.Columns[j].ColumnName.ToString() + "\":" + "\"" + table.Rows[i][j].ToString() + "\",");
                        }
                        else if (j == table.Columns.Count - 1)
                        {
                            JSONString.Append("\"" + table.Columns[j].ColumnName.ToString() + "\":" + "\"" + table.Rows[i][j].ToString() + "\"");
                        }
                    }
                    if (i == table.Rows.Count - 1)
                    {
                        JSONString.Append("}");
                    }
                    else
                    {
                        JSONString.Append("},\n");
                    }
                }
                JSONString.Append("]");
            }
            
            string pathfile = ruta + "/" + nombre + ".json";
            System.IO.File.WriteAllText(pathfile, JSONString.ToString());

            string time = DateTime.Now.ToString("hmmss");
            _log.Info("Se creo el Json:_" + nombre + "_fin:" +  time);

            return "Se creo el Json";//JSONString.ToString();
        }

        //public string DataTableToJSONWithJavaScriptSerializer(DataTable table, string nombre, string calfec)
        //{
        //    JavaScriptSerializer jsSerializer = new JavaScriptSerializer();
        //    List<Dictionary<string, object>> parentRow = new List<Dictionary<string, object>>();
        //    Dictionary<string, object> childRow;
        //    foreach (DataRow row in table.Rows)
        //    {
        //        childRow = new Dictionary<string, object>();
        //        foreach (DataColumn col in table.Columns)
        //        {
        //            childRow.Add(col.ColumnName, row[col]);
        //        }
        //        parentRow.Add(childRow);
        //    }
        //    return jsSerializer.Serialize(parentRow);
        //}

        //public string DataTableToJSONWithJSONNet(DataTable table)
        //{
        //    string JSONString = string.Empty;
        //    JSONString = JSONConvert.SerializeObject(table);
        //    return JSONString;
        //}
    }
}
