using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Data;
using System.IO;
using Estudio.Repository.Core.Domain;
using log4net;
using System.Reflection;
using log4net.Config;
using System.Runtime;
using Estudio.Repository.Persistence.Repositories;

namespace Estudio.Process
{
    public class RutinaFlujosNoSbs
    {
        #region Clase de Rutina Antigüa
        //public string msj { get; set; }
        private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        ReservasRepository _ReservasRepository = new ReservasRepository();

        #region Variables para proceso de tareas.
        bool Resultado = true;
        string Tip = "";
        int numPol = 1;
        string TipPen = "";


        List<Task> tareasFlujosAnt = new List<Task>();
        List<beDatosBen> LisTabBenPar = new List<beDatosBen>();
        List<beDatosPol> LisTabPolPar = new List<beDatosPol>();

        //DataTables para las inserciones.
        public DataTable ModelFluBen1Ant = new DataTable();
        public DataTable ModelFluTot1Ant = new DataTable();
        public DataTable ModelFluBen2Ant = new DataTable();
        public DataTable ModelFluTot2Ant = new DataTable();

        // Lista de DataTable para las inserciones
        public List<DataTable> ListModelFluBen1 = new List<DataTable>();
        public List<DataTable> ListModelFluTot1 = new List<DataTable>();
        public List<DataTable> ListModelFluBen2 = new List<DataTable>();
        public List<DataTable> ListModelFluTot2 = new List<DataTable>();


        #region Querys para actualización de registros.
        string TblsTemporales = "CREATE TABLE TBL_TEMP_FLUPOL1 (NUM_POLIZA VARCHAR(10) NULL, NUM_MESFLU INT NULL, MTO_PENSION NUMERIC(18, 2) NULL, MTO_FLUPEN NUMERIC(18, 8) NULL, MTO_FLUSEP NUMERIC(18, 8) NULL, " +
                                "MTO_FLUTOT NUMERIC(18, 8) NULL, PRC_TASTCE NUMERIC(18, 2) NULL) \n\n" +
                                "CREATE TABLE TBL_TEMP_FLUPOL2 (NUM_POLIZA VARCHAR(10) NULL, NUM_MESFLU INT NULL, MTO_PENSION NUMERIC(18, 2) NULL, MTO_FLUPEN NUMERIC(18, 8) NULL, MTO_FLUSEP NUMERIC(18, 8) NULL, " +
                                "MTO_FLUTOT NUMERIC(18, 8) NULL, PRC_TASTCE NUMERIC(18, 2) NULL) \n\n" +
                                "CREATE TABLE TBL_TEMP_FLUBEN1 (NUM_POLIZA VARCHAR(10) NULL, NUM_ORDEN INT NULL, NUM_EDAD INT NULL, NUM_MESFLU INT NULL, MTO_PENSION NUMERIC(18, 2) NULL, MTO_FLUPEN NUMERIC(18, 8) NULL, " +
                                "MTO_FLUSEP NUMERIC(18, 8) NULL, MTO_FLUTOT NUMERIC(18, 8) NULL, MTO_TPX NUMERIC(18, 8) NULL, MTO_QXT NUMERIC(18, 8) NULL) \n\n" +
                                "CREATE TABLE TBL_TEMP_FLUBEN2 (NUM_POLIZA VARCHAR(10) NULL, NUM_ORDEN INT NULL, NUM_EDAD INT NULL, NUM_MESFLU INT NULL, MTO_PENSION NUMERIC(18, 2) NULL, MTO_FLUPEN NUMERIC(18, 8) NULL, " +
                                "MTO_FLUSEP NUMERIC(18, 8) NULL, MTO_FLUTOT NUMERIC(18, 8) NULL, MTO_TPX NUMERIC(18, 8) NULL, MTO_QXT NUMERIC(18, 8) NULL)";

        string UpdatePol1 = "UPDATE T SET T.MTO_PENSION_METAN = Temp.MTO_PENSION, " +
                           "T.MTO_FLUPEN_METAN = Temp.MTO_FLUPEN, " +
                           "T.MTO_FLUSEP_METAN = Temp.MTO_FLUSEP, " +
                           "T.MTO_FLUTOT_METAN = Temp.MTO_FLUTOT, " +
                           "T.PRC_TASTCE_METAN = Temp.PRC_TASTCE FROM PR_TTMP_FLUPOL1 T INNER JOIN TBL_TEMP_FLUPOL1 Temp ON T.NUM_POLIZA = Temp.NUM_POLIZA AND T.NUM_MESFLU = Temp.NUM_MESFLU";

        string UpdatePol2 = "UPDATE T SET T.MTO_PENSION_METAN = Temp.MTO_PENSION, " +
                           "T.MTO_FLUPEN_METAN = Temp.MTO_FLUPEN, " +
                           "T.MTO_FLUSEP_METAN = Temp.MTO_FLUSEP, " +
                           "T.MTO_FLUTOT_METAN = Temp.MTO_FLUTOT, " +
                           "T.PRC_TASTCE_METAN = Temp.PRC_TASTCE FROM PR_TTMP_FLUPOL2 T INNER JOIN TBL_TEMP_FLUPOL2 Temp ON T.NUM_POLIZA = Temp.NUM_POLIZA AND T.NUM_MESFLU = Temp.NUM_MESFLU";

        string UpdateBen1 = "UPDATE T SET T.MTO_PENSION_METAN = Temp.MTO_PENSION, " +
                           "T.MTO_FLUPEN_METAN = Temp.MTO_FLUPEN, " +
                           "T.MTO_FLUSEP_METAN = Temp.MTO_FLUSEP, " +
                           "T.MTO_FLUTOT_METAN = Temp.MTO_FLUTOT, " +
                           "T.MTO_TPX_METAN = Temp.MTO_TPX, " +
                           "T.MTO_QXT_METAN = Temp.MTO_QXT FROM PR_TTMP_FLUBEN1 T INNER JOIN TBL_TEMP_FLUBEN1 Temp ON T.NUM_POLIZA = Temp.NUM_POLIZA AND T.NUM_MESFLU = Temp.NUM_MESFLU AND T.NUM_ORDEN = Temp.NUM_ORDEN";

        string UpdateBen2 = "UPDATE T SET T.MTO_PENSION_METAN = Temp.MTO_PENSION, " +
                           "T.MTO_FLUPEN_METAN = Temp.MTO_FLUPEN, " +
                           "T.MTO_FLUSEP_METAN = Temp.MTO_FLUSEP, " +
                           "T.MTO_FLUTOT_METAN = Temp.MTO_FLUTOT, " +
                           "T.MTO_TPX_METAN = Temp.MTO_TPX, " +
                           "T.MTO_QXT_METAN = Temp.MTO_QXT FROM PR_TTMP_FLUBEN2 T INNER JOIN TBL_TEMP_FLUBEN2 Temp ON T.NUM_POLIZA = Temp.NUM_POLIZA AND T.NUM_MESFLU = Temp.NUM_MESFLU AND T.NUM_ORDEN = Temp.NUM_ORDEN";

        string deleteTbls = "DROP TABLE TBL_TEMP_FLUPOL1 \n" +
                            "DROP TABLE TBL_TEMP_FLUPOL2 \n" +
                            "DROP TABLE TBL_TEMP_FLUBEN1 \n" +
                            "DROP TABLE TBL_TEMP_FLUBEN2 \n";
        #endregion

        #endregion

        public async Task<string> RutinaActFlujosNoSbs(List<beDatosPol> ModelPol, List<beDatosBen> Modelben, List<beMortalidad> ModelMor, List<beTasaFacVac> ModelFacVac,
                    string FecCal, double ValTc, string conexion)
        {
            try
            {
                XmlConfigurator.Configure();

                #region Creación de DataTables para inserción.
                ModelFluTot1Ant.Columns.Add("NUM_POLIZA", typeof(string));
                ModelFluTot1Ant.Columns.Add("NUM_MESFLU", typeof(int));
                ModelFluTot1Ant.Columns.Add("MTO_PENSION_METAN", typeof(decimal));
                ModelFluTot1Ant.Columns.Add("MTO_FLUPEN_METAN", typeof(decimal));
                ModelFluTot1Ant.Columns.Add("MTO_FLUSEP_METAN", typeof(decimal));
                ModelFluTot1Ant.Columns.Add("MTO_FLUTOT_METAN", typeof(decimal));
                ModelFluTot1Ant.Columns.Add("PRC_TASTCE_METAN", typeof(decimal));

                ModelFluBen1Ant.Columns.Add("NUM_POLIZA", typeof(string));
                ModelFluBen1Ant.Columns.Add("NUM_ORDEN", typeof(int));
                ModelFluBen1Ant.Columns.Add("NUM_EDAD", typeof(int));
                ModelFluBen1Ant.Columns.Add("NUM_MESFLU", typeof(int));
                ModelFluBen1Ant.Columns.Add("MTO_PENSION_METAN", typeof(decimal));
                ModelFluBen1Ant.Columns.Add("MTO_FLUPEN_METAN", typeof(decimal));
                ModelFluBen1Ant.Columns.Add("MTO_FLUSEP_METAN", typeof(decimal));
                ModelFluBen1Ant.Columns.Add("MTO_FLUTOT_METAN", typeof(decimal));
                ModelFluBen1Ant.Columns.Add("MTO_TPX_METAN", typeof(decimal));
                ModelFluBen1Ant.Columns.Add("MTO_QXT_METAN", typeof(decimal));

                ModelFluTot2Ant.Columns.Add("NUM_POLIZA", typeof(string));
                ModelFluTot2Ant.Columns.Add("NUM_MESFLU", typeof(int));
                ModelFluTot2Ant.Columns.Add("MTO_PENSION_METAN", typeof(decimal));
                ModelFluTot2Ant.Columns.Add("MTO_FLUPEN_METAN", typeof(decimal));
                ModelFluTot2Ant.Columns.Add("MTO_FLUSEP_METAN", typeof(decimal));
                ModelFluTot2Ant.Columns.Add("MTO_FLUTOT_METAN", typeof(decimal));
                ModelFluTot2Ant.Columns.Add("PRC_TASTCE_METAN", typeof(decimal));

                ModelFluBen2Ant.Columns.Add("NUM_POLIZA", typeof(string));
                ModelFluBen2Ant.Columns.Add("NUM_ORDEN", typeof(int));
                ModelFluBen2Ant.Columns.Add("NUM_EDAD", typeof(int));
                ModelFluBen2Ant.Columns.Add("NUM_MESFLU", typeof(int));
                ModelFluBen2Ant.Columns.Add("MTO_PENSION_METAN", typeof(decimal));
                ModelFluBen2Ant.Columns.Add("MTO_FLUPEN_METAN", typeof(decimal));
                ModelFluBen2Ant.Columns.Add("MTO_FLUSEP_METAN", typeof(decimal));
                ModelFluBen2Ant.Columns.Add("MTO_FLUTOT_METAN", typeof(decimal));
                ModelFluBen2Ant.Columns.Add("MTO_TPX_METAN", typeof(decimal));
                ModelFluBen2Ant.Columns.Add("MTO_QXT_METAN", typeof(decimal));
                #endregion

                //Ciclo que recorre lista de pólizas llamando método que contiene la rutina y crea las tareas.
                #region Tareas

                int limitPol = ModelPol.Count > 287 ? 287 : ModelPol.Count();

                for (int i = 0; i < limitPol; i++)
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
                            var task = ResFlujoTodoAnt(LisTabPolPar, LisTabBenPar, ModelMor, ModelFacVac, FecCal, ValTc, Tip, numPol.ToString(), "");
                            //ResFlujoTodoAnt(LisTabPolPar, LisTabBenPar, ModelMor, ModelFacVac, FecCal, ValTc, Tip, conexion, numPol.ToString());
                            tareasFlujosAnt.Add(task);
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

                while (tareasFlujosAnt.Count > 0)
                {
                    Task firstFinishedTask = await Task.WhenAny(tareasFlujosAnt);
                    tareasFlujosAnt.Remove(firstFinishedTask);
                }

                GC.Collect();
                Console.WriteLine("Memory used after full collection:   {0:N0}",
                                  GC.GetTotalMemory(true));
                GCSettings.LargeObjectHeapCompactionMode = GCLargeObjectHeapCompactionMode.CompactOnce;
                GC.Collect(2, GCCollectionMode.Forced, true, true);

                //await Task.WhenAll(tareasFlujosAnt);

                try
                {
                    //Inserción de los registros añadidos a los DataTable
                    #region Llenado de DataTables para insertar en BD.
                    foreach (var dt in ListModelFluTot1)
                    {
                        foreach (DataRow item in dt.Rows)
                        {
                            DataRow row = ModelFluTot1Ant.NewRow();

                            row["NUM_POLIZA"] = item[0];
                            row["NUM_MESFLU"] = item[1];
                            row["MTO_PENSION_METAN"] = item[2];
                            row["MTO_FLUPEN_METAN"] = item[3];
                            row["MTO_FLUSEP_METAN"] = item[4];
                            row["MTO_FLUTOT_METAN"] = item[5];
                            row["PRC_TASTCE_METAN"] = item[6];

                            ModelFluTot1Ant.Rows.Add(row);
                        }
                    }

                    foreach (var dt in ListModelFluTot2)
                    {
                        foreach (DataRow item in dt.Rows)
                        {
                            DataRow row = ModelFluTot2Ant.NewRow();

                            row["NUM_POLIZA"] = item[0];
                            row["NUM_MESFLU"] = item[1];
                            row["MTO_PENSION_METAN"] = item[2];
                            row["MTO_FLUPEN_METAN"] = item[3];
                            row["MTO_FLUSEP_METAN"] = item[4];
                            row["MTO_FLUTOT_METAN"] = item[5];
                            row["PRC_TASTCE_METAN"] = item[6];

                            ModelFluTot2Ant.Rows.Add(row);
                        }
                    }

                    foreach (var dt in ListModelFluBen1)
                    {
                        foreach (DataRow item in dt.Rows)
                        {
                            DataRow row = ModelFluBen1Ant.NewRow();

                            row["NUM_POLIZA"] = item[0];
                            row["NUM_ORDEN"] = item[1];
                            row["NUM_EDAD"] = item[2];
                            row["NUM_MESFLU"] = item[3];
                            row["MTO_PENSION_METAN"] = item[4];
                            row["MTO_FLUPEN_METAN"] = item[5];
                            row["MTO_FLUSEP_METAN"] = item[6];
                            row["MTO_FLUTOT_METAN"] = item[7];
                            row["MTO_TPX_METAN"] = item[8];
                            row["MTO_QXT_METAN"] = item[9];

                            ModelFluBen1Ant.Rows.Add(row);
                        }
                    }

                    foreach (var dt in ListModelFluBen2)
                    {
                        foreach (DataRow item in dt.Rows)
                        {
                            DataRow row = ModelFluBen2Ant.NewRow();

                            row["NUM_POLIZA"] = item[0];
                            row["NUM_ORDEN"] = item[1];
                            row["NUM_EDAD"] = item[2];
                            row["NUM_MESFLU"] = item[3];
                            row["MTO_PENSION_METAN"] = item[4];
                            row["MTO_FLUPEN_METAN"] = item[5];
                            row["MTO_FLUSEP_METAN"] = item[6];
                            row["MTO_FLUTOT_METAN"] = item[7];
                            row["MTO_TPX_METAN"] = item[8];
                            row["MTO_QXT_METAN"] = item[9];

                            ModelFluBen2Ant.Rows.Add(row);
                        }
                    }
                    #endregion
                }
                catch (Exception ex)
                {
                    _log.Info("ERROR AL LLENAR DATATABLES PARA ACTUALIZACIÓN DE RESULTADOS DE RUTINA ANTIGÜA: " + ex.Message);
                    return "Error en llenado de DataTables para actualización de resultados de Rutina Antigüa.";
                }

                try
                {
                    #region Creación e inserción en temporales y actualización de datos en tablas de flujos.
                    //Lineas para crear las tablas en las cuales se guardará la información temporalmente.
                    _ReservasRepository.Ejecuta_Query_Conn(TblsTemporales, conexion);

                    //Lineas para insertar la información en las tablas.
                    if (ModelFluTot1Ant.Rows.Count != 0) { _ReservasRepository.BulkInsertFlujos(ModelFluTot1Ant, "TBL_TEMP_FLUPOL1", conexion); }
                    if (ModelFluTot2Ant.Rows.Count != 0) { _ReservasRepository.BulkInsertFlujos(ModelFluTot2Ant, "TBL_TEMP_FLUPOL2", conexion); }
                    if (ModelFluBen1Ant.Rows.Count != 0) { _ReservasRepository.BulkInsertFlujos(ModelFluBen1Ant, "TBL_TEMP_FLUBEN1", conexion); }
                    if (ModelFluBen2Ant.Rows.Count != 0) { _ReservasRepository.BulkInsertFlujos(ModelFluBen2Ant, "TBL_TEMP_FLUBEN2", conexion); }

                    //Lineas para realizar la actualización de los registros en cada tabla directamente en SeguroRV.
                    if (ModelFluTot1Ant.Rows.Count != 0) { _ReservasRepository.Ejecuta_Query_Conn(UpdatePol1, conexion); }
                    if (ModelFluTot2Ant.Rows.Count != 0) { _ReservasRepository.Ejecuta_Query_Conn(UpdatePol2, conexion); }
                    if (ModelFluBen1Ant.Rows.Count != 0) { _ReservasRepository.Ejecuta_Query_Conn(UpdateBen1, conexion); }
                    if (ModelFluBen2Ant.Rows.Count != 0) { _ReservasRepository.Ejecuta_Query_Conn(UpdateBen2, conexion); }
                    #endregion
                }
                catch (Exception ex)
                {
                    _log.Info("ERROR AL INSERTAR Y ACTUALIZAR RESULTADOS DE RUTINA ANTIGÜA.");
                    _log.Info("ERROR: " + ex.Message);
                    return "Error al actualizar resultados de Rutina Antigüa.";
                }
                //Lineas para borrar las tablas temporales.
                _ReservasRepository.Ejecuta_Query_Conn(deleteTbls, conexion);

                return "Flujos Guardados con éxito.";
            }
            catch (Exception ex)
            {
                Console.WriteLine("{0}", "Error en la Rutina Antigüa - " + ex.Message);
                _log.Info("Error en Clase Rutina Flujos NoSbs: " + ex.Message);

                return "Error en Flujos de Rutina Antigüa.";
            }
        }
        #endregion

        async Task ResFlujoTodoAnt(List<beDatosPol> ModelPol, List<beDatosBen> Modelben, List<beMortalidad> ModelMor, List<beTasaFacVac> ModelFacVac,
                                   string FecCal, double ValTc, string TipTbl, string numTbl, string msj)
        {
            #region Rutina
            await Task.Factory.StartNew(() =>
            {
                List<beResultadosFlujos> ListaResultados = new List<beResultadosFlujos>();
                List<beResultadosFlujosTot> ListaResultadosTot = new List<beResultadosFlujosTot>();

                #region DT Locales.
                //DataTables para las inserciones.
                DataTable ModelFluBen1AntLocal = new DataTable();
                DataTable ModelFluTot1AntLocal = new DataTable();
                DataTable ModelFluBen2AntLocal = new DataTable();
                DataTable ModelFluTot2AntLocal = new DataTable();

                ModelFluTot1AntLocal.Columns.Add("NUM_POLIZA", typeof(string));
                ModelFluTot1AntLocal.Columns.Add("NUM_MESFLU", typeof(int));
                ModelFluTot1AntLocal.Columns.Add("MTO_PENSION_METAN", typeof(decimal));
                ModelFluTot1AntLocal.Columns.Add("MTO_FLUPEN_METAN", typeof(decimal));
                ModelFluTot1AntLocal.Columns.Add("MTO_FLUSEP_METAN", typeof(decimal));
                ModelFluTot1AntLocal.Columns.Add("MTO_FLUTOT_METAN", typeof(decimal));
                ModelFluTot1AntLocal.Columns.Add("PRC_TASTCE_METAN", typeof(decimal));

                ModelFluBen1AntLocal.Columns.Add("NUM_POLIZA", typeof(string));
                ModelFluBen1AntLocal.Columns.Add("NUM_ORDEN", typeof(int));
                ModelFluBen1AntLocal.Columns.Add("NUM_EDAD", typeof(int));
                ModelFluBen1AntLocal.Columns.Add("NUM_MESFLU", typeof(int));
                ModelFluBen1AntLocal.Columns.Add("MTO_PENSION_METAN", typeof(decimal));
                ModelFluBen1AntLocal.Columns.Add("MTO_FLUPEN_METAN", typeof(decimal));
                ModelFluBen1AntLocal.Columns.Add("MTO_FLUSEP_METAN", typeof(decimal));
                ModelFluBen1AntLocal.Columns.Add("MTO_FLUTOT_METAN", typeof(decimal));
                ModelFluBen1AntLocal.Columns.Add("MTO_TPX_METAN", typeof(decimal));
                ModelFluBen1AntLocal.Columns.Add("MTO_QXT_METAN", typeof(decimal));

                ModelFluTot2AntLocal.Columns.Add("NUM_POLIZA", typeof(string));
                ModelFluTot2AntLocal.Columns.Add("NUM_MESFLU", typeof(int));
                ModelFluTot2AntLocal.Columns.Add("MTO_PENSION_METAN", typeof(decimal));
                ModelFluTot2AntLocal.Columns.Add("MTO_FLUPEN_METAN", typeof(decimal));
                ModelFluTot2AntLocal.Columns.Add("MTO_FLUSEP_METAN", typeof(decimal));
                ModelFluTot2AntLocal.Columns.Add("MTO_FLUTOT_METAN", typeof(decimal));
                ModelFluTot2AntLocal.Columns.Add("PRC_TASTCE_METAN", typeof(decimal));

                ModelFluBen2AntLocal.Columns.Add("NUM_POLIZA", typeof(string));
                ModelFluBen2AntLocal.Columns.Add("NUM_ORDEN", typeof(int));
                ModelFluBen2AntLocal.Columns.Add("NUM_EDAD", typeof(int));
                ModelFluBen2AntLocal.Columns.Add("NUM_MESFLU", typeof(int));
                ModelFluBen2AntLocal.Columns.Add("MTO_PENSION_METAN", typeof(decimal));
                ModelFluBen2AntLocal.Columns.Add("MTO_FLUPEN_METAN", typeof(decimal));
                ModelFluBen2AntLocal.Columns.Add("MTO_FLUSEP_METAN", typeof(decimal));
                ModelFluBen2AntLocal.Columns.Add("MTO_FLUTOT_METAN", typeof(decimal));
                ModelFluBen2AntLocal.Columns.Add("MTO_TPX_METAN", typeof(decimal));
                ModelFluBen2AntLocal.Columns.Add("MTO_QXT_METAN", typeof(decimal));
                #endregion

                try
                {
                    #region Variables
                    string vlFechaNacCausante, vlSexoCausante, Npolca, Mone, Depto, Cober, Alt, Indi, cplan, Sql, Numero, vlNumCot, cob, alt1, tip;
                    string MarcaSob;
                    //string[] Coinb = new string[20], Codcbe = new string[20], Sexob = new string[20], ;
                    double tvmax, salcta_eva, vppen = 0, vpcm, vppfactor, Vpptem, Add_porc_be, Totpofr = 0, Rete_sim = 0, pensim = 0;
                    double tasac_t = 0, gastos = 0, rdeuda = 0, timp = 0, penmax = 0, gasemi = 0, facdec = 0, PERMAX = 0;
                    bool vlExisteDepto = false;
                    double MontoFin = 0;
                    long mesdiftmp, mesconTmp;
                    int Fintab = 0, cuenta = 0;
                    int Nben, Nap, Nmp, Ndp, Nad, Nmd, Ndd, NumCor, TipRea;
                    string TipPen, TipRen, TipMod, ind_cob, FecCot, FecDev, FecSol, DerCre, DerGra, RegEst, FecRvt;
                    double Valmon, MtoCic, MtoPri, GtoFun, PrcAfp, PrcTaf, PrcCom, PrcTri, PrcMen, PrcAnu, mtoMinPri, prcRenCom, MinRC, RepRC, sumaporcsob = 0, sumaporcsobrv = 0, sumaporcsobdV = 0;
                    long EdaLim;
                    long iRes;
                    int vlVecesCot = 1;
                    double Totpor;
                    string fecha;
                    int l = 0;
                    long r = 0;
                    int i = 0;
                    int j;
                    int t;
                    int k;
                    long nmax;
                    double mto, mtoax, px = 0, py = 0, qx = 0, qxt = 0, tpx = 0;
                    double tasac;
                    long Mesdif;
                    long Mesgar;
                    long mescosto;
                    double tmm;
                    double tm3, gto_supervivencia = 0, tirvta = 0, tinc = 0, sumaex = 0, sumaex1 = 0, tirmax = 0;
                    double MesCostoTmp;
                    long mesdif1, pergar, pergar1, mesdifc, perdif;
                    long Fechap, Fechrv, FechaDv, mescon;
                    int Tipajus = 0;
                    double sumCostosN, sumaCostTotalMes, mescotoAlt;
                    double vgFactorAjusteIPC = 0;
                    double vl_FactorMensual = 0;
                    double vl_FactorTrimestral = 0;
                    double factorPorPen = 0;
                    int ni = 0;
                    int ns = 0;
                    long edaca = 0, edalca = 0, edacai = 0, edacas = 0, edabe = 0, edalbe = 0, edaberv = 0, edadedv = 0, nibe = 0, nsbe = 0, edbedi = 0, nmdiga = 0, Ebedif = 0, edacax = 0;
                    double rmpol, relres, pension;
                    long limite, limite1, limite2, limite3, limite4, imas1 = 0, nt = 0, kdif, numbep = 0, nmdifi;
                    double sumapenben, penanuAFP, facfam, FxVac, PenBase, vppenAfp, ival, SalCtaAfp, sumapx, sumaqx, penanu, actual, actua1 = 0, tce;
                    double tci, vpte, vpte2, vppenres, vpcmres, tasatirc, difres, difre1, tir, TINC1, vlSumaPension, penanuFinal, vlSumPension = 0;
                    string DerCrecer = "", swg = "", DerGratificacion = "";
                    long nmdif = 0, valpx, ax, MesHijoDif_EdaLim;
                    double new_prc = 0, vld_saldo = 0, Tasa;
                    long mdif = 0;
                    double reserva, PERDI, vld_comision, vld_impuesto, vld_puesta, flupag, gto_inicial, Comision, margen, vlMargenDespuesImpuesto, rend, vld_gtosbs, vlPenGar = 0;
                    double resfin = 0, varrm, resant, tastceM, gto, tirmax_ori, TTirMax, RM, NewPen, perdis, tassim, tasa_tir, tasa_vta, tasa_tce, tprc_per, tasa_tci, Tasa_pro;
                    double vld_PensionAnual = 0, vld_ReservaSepelio = 0, vld_ReservaPensiones = 0;
                    double tasanc = 0.00001;
                    int icont10 = 0, icont20, icont11, icont21, icont30, icont35, icont40, icont77, icont30Inv;
                    long vlContarMaximo;
                    long Fechan;
                    int at;
                    int IM_a = 0, IM_m = 0, IM_d = 0;
                    long edhm = 0;
                    double porfam = 0;
                    int[] Orden = new int[20];
                    int[] Ncorbe = new int[20];
                    int[] Nanbe = new int[20];
                    int[] Nmnbe = new int[20];
                    int[] Ndnbe = new int[20];
                    int[] Ijam = new int[20];
                    int[] Ijmn = new int[20];
                    int[] Ijdn = new int[20];
                    int[] isuc = new int[20];
                    double[] porcbe_ori = new double[20];
                    string[] Sexob = new string[20];
                    string[] Coinb = new string[20];
                    string[] Codcb = new string[20];
                    double[,,] Lx = new double[3, 4, 1400];
                    double[,,] Ly = new double[3, 4, 1400];
                    double[] Penben = new double[20];
                    double[] Porcbe = new double[20];
                    double[] PorcbeSob = new double[20];
                    double[] Porcbe_tram = new double[20];
                    double[] fpxAfp = new double[1400];
                    double[] fpx = new double[1400];
                    double[] valotemp = new double[1400];
                    double[] fpy = new double[1400];
                    double[] valotempRC = new double[1400];
                    double[] fqx = new double[1400];
                    double[] PensionBenef = new double[1400];
                    double[] vl_FactorReajuste = new double[1400];
                    double[] facgratif = new double[1400];
                    double[] Cp = new double[1400];
                    double[] Prodin = new double[1400];
                    double[] Flupen = new double[1400];
                    double[] Flucm = new double[1400];
                    double[] Exced = new double[1400];
                    double[] FlupenVal = new double[1400];
                    double[] valPY = new double[1400];
                    double[] FlupenRC = new double[1400];
                    double[] Arr_Tasas = new double[1400];
                    double[] factual = new double[1400];
                    double[] factualqx = new double[1400];
                    double[] fqxt = new double[1400];
                    double[] fqxy = new double[1400];
                    double[] ftpx = new double[1400];
                    double[] fcru = new double[1400];
                    double[] fcruGS = new double[1400];
                    double[] fRestci = new double[1400];
                    double[] fTramos = new double[1400];
                    double[] vl_FactorReaPaso = new double[1400];
                    double[,,] Lxb = new double[3, 3, 111];
                    double[,,] Ax = new double[3, 3, 111];
                    double vl_sumacosto = 0;

                    double[] Fluvpte2 = new double[1400];

                    long ltot = 0;
                    bool vlExisteTM = false;
                    bool vlExisteTA = false;
                    double tm = 0;
                    double ta = 0;
                    double tpr = 0;
                    double prcPen = 0;
                    decimal Exp = (decimal)1 / 12;
                    decimal Expf = 0;
                    //string msj = "";
                    int cr = 0;
                    double dUtilImp = 0, dCapit = 0, dvarCap = 0, dComis = 0, dGasMan = 0, dPagos = 0, dVarRes = 0, dProdInv = 0, dImp = 0, dMarSol = (6.75 / 100), dResTCI = 0;
                    double dvarCapAnt = 0, resfinAnt = 0, dflupag = 0;
                    int an = 0;
                    int anoBasTM = 0;

                    //variables de Tablas Dinamicas
                    int ord, ano, mes, dia, Aux = 0;
                    string inv, sex;
                    decimal FxQx = 0, FxAx = 0, FxQxF = 0, FxQxFm = 0, FxLxF = 0;
                    decimal ExpQ = 0, ExoQ = 0;
                    decimal[] FxQxFmV = new decimal[1400];
                    decimal[] FxLxFV = new decimal[1400];
                    decimal[,] LxDin = new decimal[10, 1400];

                    //variables de tasas Curva
                    decimal[] ValCurva = new decimal[1400];
                    //para saber los pagos de las reservas
                    double[] fpagosRes = new double[1400];
                    bool TitMayor = false;
                    //para flujos por beneficiario
                    double[,] FlujosBen = new double[10, 1400];
                    string NumPol;
                    double MtoPen = 0;
                    long[] Edadben = new long[20];
                    msj = "";
                    double prcTce = 0;
                    string vlFecFallCausante = "", vlIndFall = "";
                    string[] Tope18 = new string[20];
                    string[] Estudi = new string[20];
                    long edadRN = 0;
                    string[] FecFallBen = new string[20]; //20012020
                    #endregion

                    if (ModelPol[0].NumeroCasoEspecial != 4)
                    {
                        foreach (var item in ModelPol)
                        {

                            #region CargaVariable
                            NumPol = item.NumPol;
                            Fintab = 1332; // item.FinTab;
                            Nben = Modelben.Count - 1;
                            Cober = item.TipPen;  //cober
                            TipPen = item.TipPen;  //TipoPension
                            TipRen = item.TipRen; //Indi
                            TipMod = item.TipMod; //alt
                            Mesgar = item.NumGar;
                            Mone = item.TipMon;
                            ind_cob = item.IndCob;
                            Valmon = ValTc; //MtoMoneda
                            FecCot = item.FecCot;
                            Nap = int.Parse(FecCal.Substring(0, 4));
                            Nmp = int.Parse(FecCal.Substring(4, 2));
                            Ndp = int.Parse(FecCal.Substring(6, 2));
                            if (Mone != "NS") { MtoPri = item.MtoPri / Valmon; } else { MtoPri = item.MtoPri; };
                            MtoCic = MtoPri; //SalCta
                            Mesdif = item.NumDif;
                            FecDev = item.FecDev;
                            //FecSol = item.DevSol; //FecDevSol
                            Nad = int.Parse(item.FecDev.Substring(0, 4)); //Nad
                            Nmd = int.Parse(item.FecDev.Substring(4, 2)); //Nmd
                            Ndd = int.Parse(item.FecDev.Substring(6, 2)); //Ndd
                            if (Mone != "NS") { GtoFun = item.MtoGS / Valmon; } else { GtoFun = item.MtoGS; };
                            DerCre = item.DerCre; //DerCrecer
                            DerGra = item.DerGra; //DerGratificacion
                            EdaLim = item.EdaLim; //EdaLim
                            TipRea = item.TipRea; //Tipajus
                            PrcTri = item.PrcTri; //vl_FactorTrimestral
                            PrcMen = item.PrcMen; //vl_FactorMensual
                            MtoPen = item.MtoPen;
                            PrcTaf = item.PrcTaf;
                            tvmax = item.PrcTas;
                            tce = item.PrcTce;
                            anoBasTM = 2017;

                            //GRATIFICACION
                            DateTime fecha1 = new DateTime(Nad, Nmd, 1);

                            for (int g = 1; g < 1400; g++)
                            {
                                facgratif[g] = 1;
                                if ((fecha1.Month == 7 || fecha1.Month == 12) && DerGra == "S")
                                {
                                    facgratif[g] = 2;
                                }
                                fecha1 = fecha1.AddMonths(1);
                                //fecha1 = new DateTime(Nap, Nmp + 1, 1);
                                if (g == 1400)
                                {
                                    break;
                                }
                            };
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
                                Totpor = 0;

                                foreach (var itemben2 in Modelben)
                                {
                                    Orden[l] = itemben2.NumOrd;
                                    Ncorbe[l] = int.Parse(itemben2.CodPar);
                                    if (TipPen == "S")
                                    {
                                        if (Ncorbe[l] == 99)
                                        {
                                            goto Next;
                                            //sale de aca
                                        }
                                    }
                                    if ((Ncorbe[l] == 99) || (Ncorbe[l] == 0))
                                    {
                                        vlFechaNacCausante = itemben2.FecNac;
                                        vlSexoCausante = itemben2.TipSex;
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
                                    Codcb[l] = itemben2.DerPen;

                                    if (itemben2.NacHM != "")
                                    {
                                        fecha = "";
                                        fecha = itemben2.NacHM;
                                        Ijam[l] = int.Parse(fecha.Substring(0, 4));    //'aa_hijom
                                        Ijmn[l] = int.Parse(fecha.Substring(4, 2));    //'mm_hijom
                                        Ijdn[l] = int.Parse(fecha.Substring(6, 2));    //'mm_hijom
                                    }
                                    else
                                    {
                                        Ijam[l] = 0;    //'aa_hijom
                                        Ijmn[l] = 0;    //'mm_hijom
                                        Ijdn[l] = 0;    //'mm_hijom
                                    }
                                    isuc[l] = 0;
                                    Tope18[l] = itemben2.Tope18;
                                    Estudi[l] = itemben2.Estudi;
                                    FecFallBen[l] = itemben2.FacFal;
                                Next:
                                    l = l + 1;
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

                            #endregion

                            #region Crea Tablas de Mortalidad del Conjunto familiar

                            //CARGA TABLAS DE MORTALIDAD
                            foreach (var itemMor in ModelMor)
                            {
                                i = itemMor.i;
                                j = itemMor.j;
                                t = itemMor.h;
                                k = itemMor.k;
                                mto = (double)itemMor.MtoLx;

                                if (t == 1)
                                {
                                    Lx[i, j, k] = mto;
                                }
                                else
                                {
                                    Ly[i, j, k] = mto;
                                }
                            }
                            #endregion

                            #region Inicializa y setea variables
                            //'inicialización de variables
                            //PrcCom = PrcCom / 100;
                            //tasac = tasac / 100;
                            timp = timp / 100;
                            tmm = (1 + (tm / 100));
                            tm3 = (1 + (ta / 100));
                            facdec = facdec / 100;
                            gto_supervivencia = gto_supervivencia / 100;
                            tirvta = 0;
                            tinc = 0;
                            sumaex = 0;
                            sumaex1 = 0;
                            tirmax = 0;
                            nmax = 0;
                            cplan = TipPen;
                            for (int ir = 1; ir < 1400; ir++)
                            {
                                Exced[ir] = 0;
                                Flupen[ir] = 0;
                                Flucm[ir] = 0;
                            }

                            //Fechap = Nap * 12 + Nmp;
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
                                if (mescon > mesdif1)
                                {
                                    //mescosto = mescon - mesdif1;
                                    mescosto = mescon;
                                }
                                else
                                {
                                    if (TipRen == "E")
                                    {
                                        mescosto = mescon;
                                    }
                                    else
                                    {
                                        mescosto = mescon;
                                    }
                                }
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
                                if (mesdif1 > mescon)
                                {
                                    //Mesdif = (mesdif1 - mescon);
                                    Mesdif = mesdif1;
                                }
                                else
                                {
                                    Mesdif = (mescon - mesdif1);
                                }
                            }

                            //'Periodo Garantizado
                            if (mescon > (pergar1 + mesdif1))
                            {
                                pergar = 0;
                            }
                            else
                            {
                                if (mescon < mesdif1)
                                {
                                    pergar = pergar1;
                                    //Mesgar = pergar - MesCostoTmp; 
                                }
                                else
                                {
                                    pergar = (pergar1 + mesdif1) - mescon;
                                    //pergar = (pergar1 + mesdif1);
                                }
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
                            MarcaSob = "N";

                            long ltotCero = 0;

                            if (mescon >= mesdif1)
                            {
                                ltotCero = mescon;
                            }
                            else
                            {
                                if (Mesdif == 0)
                                {
                                    ltotCero = mesdif1 - mescon;
                                }
                                else
                                {
                                    ltotCero = mesdif1;
                                }
                            }
                            #endregion

                            #region Tipo de Ajuste
                            if (TipRea == 0)
                            {
                                for (int li = 1; li < 1400; li++)
                                {
                                    vl_FactorReajuste[i] = 1;
                                }
                            }
                            vgFactorAjusteIPC = 1;

                            k = 0;

                            int ki = 0, ki2 = 0, paso1 = 0;
                            DateTime FecCot1, FecCot2, FecCot3;
                            double TEM = 0;
                            double TET = 0;
                            double vl_FacRea = 1;

                            ax = 0;
                            if (TipRea == 2)
                            {
                                //TEM = 1 + PrcMen / 100;
                                //TET = 1 + PrcTri / 100;
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
                                    //FecCot1 = new DateTime(Nad, Nmd + 1, 1);
                                }




                                for (ki = 1; ki < 1400; ki++)
                                {
                                    vl_FactorReaPaso[ki] = vl_FactorReajuste[ki];
                                }
                                vl_sumacosto = 0;
                                ki2 = 0;
                                factorPorPen = 1;
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
                                            for (j = 0; j <= Nben; j++)
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
                                                    if (edaberv >= EdaLim && Coinb[j] == "N")
                                                    {

                                                    }
                                                    else
                                                    {
                                                        if (Coinb[j] != "N")
                                                        {
                                                            sumCostosN = 1 * Porcbe[j] * vl_FactorReaPaso[ax + 1] * facgratif[ax + 1];
                                                        }
                                                        else
                                                        {
                                                            MesHijoDif_EdaLim = EdaLim - edaberv;
                                                            if (MesHijoDif_EdaLim < 0) { MesHijoDif_EdaLim = 0; };
                                                            if (MesHijoDif_EdaLim == 0) { valpx = 0; };
                                                            if (mescon > MesHijoDif_EdaLim)
                                                            {
                                                                sumCostosN = valpx * Porcbe_tram[j] * vl_FactorReaPaso[ax + 1] * facgratif[ax + 1];
                                                            }
                                                            else
                                                            {
                                                                sumCostosN = 1 * Porcbe[j] * vl_FactorReaPaso[ax + 1] * facgratif[ax + 1];
                                                            }
                                                        }
                                                    }


                                                }
                                                else
                                                {
                                                    sumCostosN = 1 * Porcbe[j] * vl_FactorReaPaso[ax + 1] * facgratif[ax + 1];
                                                }
                                                sumaCostTotalMes = sumaCostTotalMes + sumCostosN;
                                            }
                                            mescotoAlt = mescotoAlt + sumaCostTotalMes;
                                        }
                                        //mescosto = mescotoAlt;
                                    }
                                }
                                //'************************************************************************************************************
                            }

                            //Calcula el factor indexado
                            double FxVacPri = 0, FxVacMes = 0, valIPCMen = 1, FxVacMesAnt = 0;
                            if (TipRea == 1)
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
                                            vl_FactorReajuste[ki] = valIPCMen;
                                        }
                                        else
                                        {
                                            vl_FactorReajuste[ki] = valIPCMen;
                                        }

                                        goto sale;
                                    }
                                    else
                                    {
                                        if (FecCot3.Month == 4 || FecCot3.Month == 7 || FecCot3.Month == 10 || FecCot3.Month == 1)
                                        {
                                            valIPCMen = FxVacMes / FxVacPri;
                                            vl_FactorReajuste[ki] = valIPCMen;
                                        }
                                        else
                                        {
                                            vl_FactorReajuste[ki] = valIPCMen;
                                        }
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
                                                for (j = 0; j <= Nben; j++)
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
                                            //mescosto = mescotoAlt;
                                        }
                                    }
                                    else
                                    {

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
                                            //mescosto = mescotoAlt;
                                        }
                                    }
                                    else
                                    {
                                        //
                                    }
                                }
                            }
                            #endregion

                            #region Calcula Flujos de Pension
                            new_prc = 1;
                            //flujos para tramos
                            for (long g = 0; g < 1400; g++)
                            {
                                fTramos[g] = 1;
                            }

                            if (TipRen == "E")
                            {
                                for (long g = 1; g < 1400; g++)
                                {
                                    if (g > Mesdif)
                                    {
                                        fTramos[g] = PrcTaf / 100;
                                    }
                                }
                            }
                            //flujos para tramos

                            if (TipPen != "S")
                            {
                                facfam = 1;
                                for (j = 0; j <= Nben; j++)
                                {
                                    for (long g = 0; g < 1400; g++)
                                    {
                                        ftpx[g] = 0;
                                        fqxt[g] = 0;
                                        fpy[g] = 0;
                                        fqxy[g] = 0;
                                        valotemp[g] = 0;
                                    }

                                    Penben[j] = Porcbe[j];
                                    numbep = numbep + 1;
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
                                        Penben[j] = Penben[j] * new_prc;
                                    }

                                    if (Ncorbe[j] == 99 && j == 0)
                                    {
                                        ni = 0;
                                        if (Coinb[j] == "S" || Coinb[j] == "T" || Coinb[j] == "I") { ni = 1; };
                                        if (Coinb[j] == "N") { ni = 2; };
                                        if (Coinb[j] == "P") { ni = 3; };
                                        if (ni == 0)
                                        {
                                            msj = "Error en variable ni==0. ";
                                            //ListaResultador.Mensaje = msj;
                                            Resultado = false;
                                            throw new System.ArgumentException(msj);
                                            //salir de funcion
                                        }
                                        ns = 0;
                                        if (Sexob[j] == "M") { ns = 1; };
                                        if (Sexob[j] == "F") { ns = 2; };
                                        if (ns == 0)
                                        {
                                            msj = "Error en variable ns==0. ";
                                            //ListaResultador.Mensaje = msj;
                                            Resultado = false;
                                            throw new System.ArgumentException(msj);
                                            //salir de funcion
                                        }

                                        Fechan = Nanbe[j] * 12 + Nmnbe[j];
                                        edaca = FechaDv - Fechan;
                                        Edadben[j] = edaca;
                                        edacax = edaca;

                                        if (edaca < 780 && ns == 1 && ni == 2) { cplan = "A"; };
                                        if (edaca < 720 && ns == 2 && ni == 2) { cplan = "A"; };

                                        if (edaca <= 0 || edaca > Fintab)
                                        {
                                            msj = "Error Edad es mayor a final de tabla Mortal y menor a 0. ";
                                            //ListaResultador.Mensaje = msj;
                                            Resultado = false;
                                            throw new System.ArgumentException(msj);
                                            //salir de funcion
                                        }
                                        //limite1 = Fintab - edaca - 1;
                                        limite1 = Fintab - edaca;
                                        nmax = limite1;
                                        if (vlIndFall == "S") ////29/12/2020
                                        {
                                            limite1 = (ltot + mescon) - 1;
                                            nmax = limite1;
                                        }
                                        //fin29/12/2020
                                        try
                                        {
                                            for (i = 0; i <= limite1; i++)
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
                                                    qxt = 1 - (Lx[ns, ni, edacai] / Lx[ns, ni, edacax]);
                                                    //qxt = (double)(1 - (LxDin[j, edacai] / LxDin[j, edacax]));

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
                                                valotemp[imas1] = px * Penben[j] * facgratif[imas1] * vl_FactorReajuste[imas1] * fTramos[imas1];
                                                Flupen[imas1] = Flupen[imas1] + px * Penben[j] * facgratif[imas1] * vl_FactorReajuste[imas1] * fTramos[imas1];

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
                                                fqx[imas1] = qx;
                                                edacas = edacai + 1;
                                                if (edacas == 1334)
                                                {
                                                    break;
                                                };

                                                //aca debe ir listado de resultado
                                                FlujosBen[j, imas1] = valotemp[imas1];
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            msj = "Problemas en los Flujos de Titular de la Rutina. ";
                                            Resultado = false;
                                            throw new System.ArgumentException(msj);
                                        }
                                    }
                                    if (Ncorbe[j] != 99)
                                    {
                                        //Penben[j] = porcbe_ori[j];

                                        nibe = 0;
                                        if (Coinb[j] == "S" || Coinb[j] == "T" || Coinb[j] == "I") { nibe = 1; };
                                        if (Coinb[j] == "N") { nibe = 2; };
                                        if (Coinb[j] == "P") { nibe = 3; };
                                        if (nibe == 0)
                                        {
                                            msj = "Error en variable nibe==0. ";
                                            //ListaResultador.Mensaje = msj;
                                            Resultado = false;
                                            throw new System.ArgumentException(msj);
                                            //salir de funcion
                                        }
                                        nsbe = 0;
                                        if (Sexob[j] == "M") { nsbe = 1; };
                                        if (Sexob[j] == "F") { nsbe = 2; };
                                        if (nsbe == 0)
                                        {
                                            msj = "Error en variable nibe==0. ";
                                            //ListaResultador.Mensaje = msj;
                                            Resultado = false;
                                            throw new System.ArgumentException(msj);
                                            //salir de funcion
                                        }
                                        Fechan = (Nanbe[j] * 12 + Nmnbe[j]);
                                        edabe = FechaDv - Fechan;
                                        //if (edabe < 1) { edabe = 1; };
                                        edadRN = 0;
                                        if (edabe < 1) { edadRN = edabe; edabe = 0; };
                                        Edadben[j] = edabe;
                                        if (edabe > Fintab)
                                        {
                                            msj = "Error Edad es mayor a final de tabla Mortal y menor a 0. ";
                                            //ListaResultador.Mensaje = msj;
                                            Resultado = false;
                                            throw new System.ArgumentException(msj);
                                        }
                                        if (Ncorbe[j] == 10 || Ncorbe[j] == 11 || Ncorbe[j] == 20 || Ncorbe[j] == 21 || Ncorbe[j] == 41 || Ncorbe[j] == 42 || ((Ncorbe[j] >= 30 && Ncorbe[j] < 40) && (Coinb[j] != "N")))
                                        {
                                            edacai = 0;
                                            limite1 = Fintab - edabe - 1;
                                            nmax = (long)amax0(nmax, limite1);
                                            int a = 0;
                                            try
                                            {
                                                for (i = 0; i <= limite1; i++)
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
                                                        qxt = 1 - (Ly[nsbe, nibe, edacai] / Ly[nsbe, nibe, edalbe]);
                                                        //qxt = (double)(1 - (LxDin[j, edacai] / LxDin[j, edalbe]));
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
                                                    valotemp[imas1] = py * (1 - fpx[imas1]) * facgratif[imas1] * Penben[j] * vl_FactorReajuste[imas1] * fTramos[imas1];
                                                    Flupen[imas1] = Flupen[imas1] + valotemp[imas1];

                                                    //aca debe ir listado de resultado
                                                    FlujosBen[j, imas1] = valotemp[imas1];
                                                }
                                            }
                                            catch (Exception ex)
                                            {
                                                msj = "Problemas en los Flujos de Conyugue, Padres o hijos Invalidos de la Rutina. ";
                                                Resultado = false;
                                                throw new System.ArgumentException(msj);
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
                                                    limite = (long)amin0(nmdif, limite2) - 1;
                                                    nmax = (long)amax0(nmax, limite);

                                                    try
                                                    {
                                                        for (i = 0; i <= mdif - 1; i++)
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
                                                                qxt = 1 - (Ly[nsbe, nibe, edacai] / Ly[nsbe, nibe, edalbe]);
                                                                //qxt = (double)(1 - (LxDin[j, edacai] / LxDin[j, edalbe]));
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
                                                            valotemp[imas1] = py * (1 - fpx[imas1]) * facgratif[imas1] * Penben[j] * vl_FactorReajuste[imas1] * fTramos[imas1];
                                                            Flupen[imas1] = Flupen[imas1] + valotemp[imas1];

                                                            //aca debe ir listado de resultado
                                                            FlujosBen[j, imas1] = valotemp[imas1];
                                                        }
                                                    }
                                                    catch (Exception ex)
                                                    {
                                                        msj = "Problemas en los Flujos de hijos Sanos de la Rutina. ";
                                                        Resultado = false;
                                                        throw new System.ArgumentException(msj);
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }

                            }
                            else //'Calculo de flujos de Sobrevivencia
                            {
                                //SOBREVIVENCIA+
                                long limiteA = 0;
                                for (j = 0; j <= Nben; j++)
                                {
                                    for (long g = 0; g < 1400; g++)
                                    {
                                        fpy[g] = 0;
                                        fqxy[g] = 0;
                                        valotemp[g] = 0;
                                    }

                                    if (Ncorbe[j] == 99)
                                    {
                                        goto Next;
                                    }

                                    Mesgar = pergar;
                                    if (TipMod == "S") { Mesgar = 0; };
                                    //nmdiga = perdif + Mesgar;
                                    nmdiga = mesdif1 + pergar1;

                                    Penben[j] = Porcbe[j];
                                    numbep = numbep + 1;
                                    nibe = 0;

                                    if (Coinb[j] == "S" || Coinb[j] == "T" || Coinb[j] == "I") { nibe = 1; };
                                    if (Coinb[j] == "N") { nibe = 2; };
                                    if (nibe == 0)
                                    {
                                        msj = "Error en variable nibe==0. ";
                                        //ListaResultador.Mensaje = msj;
                                        Resultado = false;
                                        throw new System.ArgumentException(msj);
                                    }
                                    nsbe = 0;
                                    if (Sexob[j] == "M") { nsbe = 1; };
                                    if (Sexob[j] == "F") { nsbe = 2; };
                                    if (nsbe == 0)
                                    {
                                        msj = "Error en variable nsbe==0. ";
                                        //ListaResultador.Mensaje = msj;
                                        Resultado = false;
                                        throw new System.ArgumentException(msj);
                                    }

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
                                    edadedv = FechaDv - Fechan;

                                    if (edabe > Fintab)
                                    {
                                        //'Mensaje = "Error en Edad del beneficiario es mayor al limite de la tabla de mortalidad"
                                        //Exit Function
                                        msj = "Error Edad es mayor a final de tabla Mortal y menor a 0. ";
                                        //ListaResultador.Mensaje = msj;
                                        Resultado = false;
                                        throw new System.ArgumentException(msj);
                                    }
                                    //if (edabe < 1) { edabe = 1; };
                                    //'calculo de renta vitalicias
                                    if (Ncorbe[j] == 10 || Ncorbe[j] == 11 || Ncorbe[j] == 20 || Ncorbe[j] == 21 || Ncorbe[j] == 41 || Ncorbe[j] == 42 || ((Ncorbe[j] >= 30 && Ncorbe[j] < 40) && (Coinb[j]) != "N"))
                                    {
                                        limite1 = Fintab - edabe - 1;
                                        nmax = (long)amax0(nmax, limite1);
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
                                        for (i = 0; i <= limite1; i++)
                                        {
                                            imas1 = i + 1;
                                            edalbe = edabe + i;
                                            edacai = edalbe + 1;
                                            edacai = (int)amin0(edacai, Fintab);

                                            //if (i == 179)
                                            //{
                                            //    an = 1;
                                            //}

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
                                                qxt = 1 - (Ly[nsbe, nibe, edacai] / Ly[nsbe, nibe, edalbe]);

                                                //qxt = (double)(1 - (LxDin[j, edacai] / LxDin[j, edalbe]));
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
                                            valPY[imas1] = py;
                                            valotemp[imas1] = py * Penben[j] * facgratif[imas1] * vl_FactorReajuste[imas1];
                                            Flupen[imas1] = Flupen[imas1] + py * Penben[j] * facgratif[imas1] * vl_FactorReajuste[imas1];

                                            //aca debe ir listado de resultado
                                            FlujosBen[j, imas1] = valotemp[imas1];
                                        }
                                    }
                                    else
                                    {
                                        if (Ncorbe[j] >= 30 && Ncorbe[j] < 40)
                                        {
                                            if (Estudi[j] == "N")
                                            {
                                                EdaLim = 18 * 12;
                                            }
                                            else
                                            {
                                                EdaLim = 28 * 12;
                                            }

                                            if ((edaberv > EdaLim && Coinb[j] == "N"))
                                            {
                                                Penben[j] = 0;

                                                //ActualizaXMLDET(pathB, j + 1, "PRC_PENSIONSOBDIF", "0")
                                            }
                                            else
                                            {
                                                mdif = EdaLim - edabe;
                                                nmdif = mdif + edadRN;
                                                limiteA = nmdif;
                                                limiteA = (long)amax0(nmdiga, nmdif);
                                                nmax = (long)amax0(limiteA, nmax);
                                                if (TipMod == "G" && TipPen == "S")
                                                {
                                                    limiteA = (long)amax0(nmdiga, nmdif);
                                                    nmax = (long)amax0(limiteA, nmax);
                                                }
                                                if (vlIndFall == "S")
                                                {
                                                    limite1 = ltot;
                                                    nmax = limite1;
                                                }
                                                //***
                                                an = 1;
                                                for (i = 0; i <= limiteA - 1; i++)
                                                {
                                                    imas1 = i + 1;
                                                    if (edadRN != 0)
                                                    {
                                                        if (i <= (edadRN))
                                                        {
                                                            //edalbe = edabe + i;
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
                                                        qxt = 1 - (Ly[nsbe, nibe, edacai] / Ly[nsbe, nibe, edalbe]);
                                                        //qxt = (double)(1 - (LxDin[j, edacai] / LxDin[j, edalbe]));
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
                                                        valPY[edalbe] = py;
                                                        fpy[edalbe] = tpx;
                                                        fqxy[edalbe] = qxt;
                                                    }
                                                    else
                                                    {
                                                        valPY[i] = py;
                                                        fpy[i] = tpx;
                                                        fqxy[i] = qxt;
                                                    }
                                                    valotemp[imas1] = py * Penben[j] * facgratif[imas1] * vl_FactorReajuste[imas1];
                                                    Flupen[imas1] = Flupen[imas1] + py * Penben[j] * facgratif[imas1] * vl_FactorReajuste[imas1];

                                                    //aca debe ir listado de resultado
                                                    FlujosBen[j, imas1] = valotemp[imas1];
                                                }
                                            }
                                        }

                                    }
                                    //aca debe ir listado de resultado
                                    FlujosBen[j, imas1] = valotemp[imas1];
                                    Next:
                                    Alt = "";
                                }
                            }
                            //Limpia todas las variables
                            for (long g = 0; g < 1400; g++)
                            {
                                FlupenVal[g] = 0;
                                valPY[g] = 0;
                                //fpy[g] = 0;
                                //fqxy[g] = 0;
                                ftpx[g] = 0;
                                fqxt[g] = 0;
                                //fpx[imas1] = tpx;
                            }
                            #endregion

                            #region Limpia flujos excedentes
                            if (TipRen != "E")
                            {
                                if (TipPen == "S" || TipPen == "I" || TipPen == "V" || TipPen == "A" || TipPen == "P")
                                {
                                    for (i = 1; i <= nmax; i++)
                                    {
                                        if (i <= (ltot + 1))
                                        {
                                            if (i <= (ltotCero))
                                            {
                                                for (j = 0; j <= Nben; j++)
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
                                                for (j = 0; j <= Nben; j++)
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
                                for (i = 1; i <= nmax; i++)
                                {
                                    if (i <= (mescon))
                                    {
                                        for (j = 0; j <= Nben; j++)
                                        {
                                            FlujosBen[j, i] = 0;
                                            Flupen[i] = 0;
                                            Flucm[i] = 0;
                                        }
                                    }
                                }

                            }

                            if (nmax == 0)
                            {
                                msj = "Tir mayor a 100%. ";
                                throw new System.ArgumentException(msj);
                                //goto SiguientePol;
                            }
                            #endregion

                            #region Calcula TCE
                            //    tci = 0;
                            //    tce = 0;
                            //    vpte = 0;
                            //    vpte2 = 0;
                            //    vppenres = 0;
                            //    vpcmres = 0;
                            //    tasatirc = 0;
                            //    difres = 0;
                            //    difre1 = 0;
                            //    tir = 0;
                            //    tinc = 0.00001;
                            //    TINC1 = 0.00001;
                            //CalTce:
                            //    //EMPIEZA LA RUTINA DEL CALCULO DE TCE 
                            //    Tasa = (tir / 100);
                            //    i = 1;
                            //    cr = 1;
                            //    for (i = 0; i <= nmax; i++)
                            //        for (i = 0; i <= nmax; i++)
                            //        {
                            //            if (i < mescosto)
                            //            {
                            //                //
                            //            }
                            //            else
                            //            {
                            //                fpagosRes[cr] = (Flupen[i + 1] * PenBase + Flucm[i + 1]) / Math.Pow((1 + Tasa), cr);
                            //                vpte = vpte + ((Flupen[i + 1] * PenBase + Flucm[i + 1]) / Math.Pow((1 + Tasa), cr));
                            //                vpte2 = vpte2 + (((Flupen[i + 1] * PenBase) + Flucm[i + 1]) / factual[cr]);
                            //                cr = cr + 1;
                            //            }
                            //        }
                            //    //vpte = vpte;
                            //    difres = vpte - vpte2;
                            //    if (difres >= 0)
                            //    {
                            //        tir = tir + tinc;
                            //        if (tir > 100)
                            //        {
                            //            msj = "Tasa TIR mayor o igual a 100%. ";
                            //            Resultado = false;
                            //        }
                            //        difre1 = difres;
                            //        vpte = 0;
                            //        vpte2 = 0;
                            //        goto CalTce;
                            //    }
                            //    tci = (tir / 100);

                            //tce = amin1(tvmax, tci, tpr);
                            //tce = ((Math.Pow((1 + tce), 12)) - 1) * 100;

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

                            //SiguientePol;


                            if (TipTbl == "1")
                            {
                                //SACA LOS FLUJOS TOTALES
                                for (i = 1; i <= nmax; i++)
                                {
                                    Console.WriteLine("Memory used before collection:       {0:N0}",
                                    GC.GetTotalMemory(false));

                                    DataRow row = ModelFluTot1AntLocal.NewRow();

                                    row["NUM_POLIZA"] = NumPol;
                                    row["NUM_MESFLU"] = i;
                                    row["MTO_PENSION_METAN"] = MtoPen; //Convert.ToDecimal(String.Format("{0:0.00}", MtoPen));
                                    row["MTO_FLUPEN_METAN"] = Convert.ToDecimal(String.Format("{0:0.00000000}", Flupen[i]));
                                    row["MTO_FLUSEP_METAN"] = Convert.ToDecimal(String.Format("{0:0.00000000}", Flucm[i])); //Flucm[i];
                                    row["MTO_FLUTOT_METAN"] = (MtoPen * Flupen[i]) + Flucm[i];
                                    row["PRC_TASTCE_METAN"] = tce;

                                    ModelFluTot1AntLocal.Rows.Add(row);
                                }
                                ListModelFluTot1.Add(ModelFluTot1AntLocal);

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

                                    DataRow row = ModelFluTot2AntLocal.NewRow();

                                    row["NUM_POLIZA"] = NumPol;
                                    row["NUM_MESFLU"] = i;
                                    row["MTO_PENSION_METAN"] = MtoPen; //Convert.ToDecimal(String.Format("{0:0.00}", MtoPen));
                                    row["MTO_FLUPEN_METAN"] = Convert.ToDecimal(String.Format("{0:0.00000000}", Flupen[i]));
                                    row["MTO_FLUSEP_METAN"] = Convert.ToDecimal(String.Format("{0:0.00000000}", Flucm[i])); //Flucm[i];
                                    row["MTO_FLUTOT_METAN"] = (MtoPen * Flupen[i]) + Flucm[i];
                                    row["PRC_TASTCE_METAN"] = tce;

                                    ModelFluTot2AntLocal.Rows.Add(row);
                                }
                                ListModelFluTot2.Add(ModelFluTot2AntLocal);

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
                                        DataRow row = ModelFluBen1AntLocal.NewRow();

                                        row["NUM_POLIZA"] = NumPol;
                                        row["NUM_ORDEN"] = j + 1;
                                        row["NUM_EDAD"] = Edadben[j];
                                        row["NUM_MESFLU"] = i;
                                        row["MTO_PENSION_METAN"] = MtoPen; // Convert.ToDecimal(String.Format("{0:0.00}", MtoPen));
                                        row["MTO_FLUPEN_METAN"] = Convert.ToDecimal(String.Format("{0:0.00000000}", FlujosBen[j, i]));
                                        row["MTO_FLUSEP_METAN"] = Convert.ToDecimal(String.Format("{0:0.00000000}", Flucm[i])); //Flucm[i];
                                        if ((j + 1) == 1)
                                        {
                                            row["MTO_FLUTOT_METAN"] = (MtoPen * FlujosBen[j, i]) + Flucm[i];
                                        }
                                        else
                                        {
                                            row["MTO_FLUTOT_METAN"] = (MtoPen * FlujosBen[j, i]);
                                        }
                                        row["MTO_TPX_METAN"] = fpy[i];
                                        row["MTO_QXT_METAN"] = fqxy[i];

                                        ModelFluBen1AntLocal.Rows.Add(row);
                                    }
                                }
                                ListModelFluBen1.Add(ModelFluBen1AntLocal);

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
                                        DataRow row = ModelFluBen2AntLocal.NewRow();

                                        row["NUM_POLIZA"] = NumPol;
                                        row["NUM_ORDEN"] = j + 1;
                                        row["NUM_EDAD"] = Edadben[j];
                                        row["NUM_MESFLU"] = i;
                                        row["MTO_PENSION_METAN"] = MtoPen; //Convert.ToDecimal(String.Format("{0:0.00}", MtoPen));
                                        row["MTO_FLUPEN_METAN"] = Convert.ToDecimal(String.Format("{0:0.00000000}", FlujosBen[j, i]));
                                        row["MTO_FLUSEP_METAN"] = Convert.ToDecimal(String.Format("{0:0.00000000}", Flucm[i])); //Flucm[i];
                                        if ((j + 1) == 1)
                                        {
                                            row["MTO_FLUTOT_METAN"] = (MtoPen * FlujosBen[j, i]) + Flucm[i];
                                        }
                                        else
                                        {
                                            row["MTO_FLUTOT_METAN"] = (MtoPen * FlujosBen[j, i]);
                                        }
                                        row["MTO_TPX_METAN"] = fpy[i];
                                        row["MTO_QXT_METAN"] = fqxy[i];

                                        ModelFluBen2AntLocal.Rows.Add(row);
                                    }
                                }
                                ListModelFluBen2.Add(ModelFluBen2AntLocal);

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
                }
                catch (Exception ex)
                {
                    Console.WriteLine("{0}", "Error en la Rutina Antigüa - " + ex.Message);
                    _log.Info("Error en Póliza No.: " + ModelPol[0].NumPol + " - " + msj);
                    _log.Info("Error en Tarea de Rutina Antigüa: " + ex.Message);
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
    }
}
