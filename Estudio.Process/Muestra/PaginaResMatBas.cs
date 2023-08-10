using Estudio.Repository.Core.Domain;
using Estudio.Repository.Persistence.Repositories;
using log4net;
using log4net.Config;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Runtime;
using System.Text;
using System.Threading.Tasks;

namespace Estudio.Process.Muestra
{
    public class PaginaResMatBas
    {
        private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        ReservasRepository _ReservasRepository = new ReservasRepository();
        RutinaResMatBas RActuarial = new RutinaResMatBas();

        DataTable PR_TMAE_CALPOL1 = new DataTable();
        DataTable PR_TMAE_CALPOL2 = new DataTable();
        DataTable PR_TMAE_CALBEN1 = new DataTable();
        DataTable PR_TMAE_CALBEN2 = new DataTable();

        // Lista de DataTable para las inserciones
        public List<DataTable> lstTablaCALPOL1 = new List<DataTable>();
        public List<DataTable> lstTablaCALPOL2 = new List<DataTable>();
        public List<DataTable> lstTablaCALBEN1 = new List<DataTable>();
        public List<DataTable> lstTablaCALBEN2 = new List<DataTable>();

        #region Querys para actualización de datos
        string strTablasTemporales = "CREATE TABLE TBL_TEMP_CALPOL1 (NUM_POLIZA VARCHAR(10) NULL, COD_BASE VARCHAR(1) NULL, MTO_RESBAS NUMERIC(18, 2) NULL, MTO_RESBASRET NUMERIC(18, 8) NULL) \n\n" +
                                     "CREATE TABLE TBL_TEMP_CALPOL2 (NUM_POLIZA VARCHAR(10) NULL, COD_BASE VARCHAR(1) NULL, MTO_RESBAS NUMERIC(18, 2) NULL, MTO_RESBASRET NUMERIC(18, 8) NULL) \n\n" +
                                     "CREATE TABLE TBL_TEMP_CALBEN1 (NUM_POLIZA VARCHAR(10) NULL, COD_BASE VARCHAR(1) NULL, NUM_ORDEN INT NULL, NUM_EDAD INT NULL, MTO_CNUBAS NUMERIC(18, 2) NULL, MTO_CNABAS NUMERIC(18, 8) NULL, " +
                                     "MTO_CNGBAS NUMERIC(18, 8) NULL, MTO_CNTBAS NUMERIC(18, 8) NULL) \n\n" +
                                     "CREATE TABLE TBL_TEMP_CALBEN2 (NUM_POLIZA VARCHAR(10) NULL, COD_BASE VARCHAR(1) NULL, NUM_ORDEN INT NULL, NUM_EDAD INT NULL, MTO_CNUBAS NUMERIC(18, 2) NULL, MTO_CNABAS NUMERIC(18, 8) NULL, " +
                                     "MTO_CNGBAS NUMERIC(18, 8) NULL, MTO_CNTBAS NUMERIC(18, 8) NULL)";

        string strUpdateCALPOL1 = "UPDATE T SET T.COD_BASE = Temp.COD_BASE, " +
                                  "T.MTO_RESBAS = Temp.MTO_RESBAS, " +
                                  "T.MTO_RESBASRET = Temp.MTO_RESBASRET FROM PR_TMAE_CALPOL1 T INNER JOIN TBL_TEMP_CALPOL1 Temp ON T.NUM_POLIZA = Temp.NUM_POLIZA ";

        string strUpdateCALPOL2 = "UPDATE T SET T.COD_BASE = Temp.COD_BASE, " +
                                  "T.MTO_RESBAS = Temp.MTO_RESBAS, " +
                                  "T.MTO_RESBASRET = Temp.MTO_RESBASRET FROM PR_TMAE_CALPOL2 T INNER JOIN TBL_TEMP_CALPOL2 Temp ON T.NUM_POLIZA = Temp.NUM_POLIZA ";

        string strUpdateCALPOL1Antigua = "UPDATE T SET T.MTO_RESBAS_METAN = Temp.MTO_RESBAS, " +
                                         "T.MTO_RESBASRET_METAN = Temp.MTO_RESBASRET FROM PR_TMAE_CALPOL1 T INNER JOIN TBL_TEMP_CALPOL1 Temp ON T.NUM_POLIZA = Temp.NUM_POLIZA ";

        string strUpdateCALPOL2Antigua = "UPDATE T SET T.MTO_RESBAS_METAN = Temp.MTO_RESBAS, " +
                                         "T.MTO_RESBASRET_METAN = Temp.MTO_RESBASRET FROM PR_TMAE_CALPOL2 T INNER JOIN TBL_TEMP_CALPOL2 Temp ON T.NUM_POLIZA = Temp.NUM_POLIZA ";

        string strUpdateCALBEN1 = "UPDATE T SET T.COD_BASE = Temp.COD_BASE, " +
                                  "T.NUM_EDAD = Temp.NUM_EDAD, " +
                                  "T.MTO_CNUBAS = Temp.MTO_CNUBAS, " +
                                  "T.MTO_CNABAS = Temp.MTO_CNABAS, " +
                                  "T.MTO_CNGBAS = Temp.MTO_CNGBAS, " +
                                  "T.MTO_CNTBAS = Temp.MTO_CNTBAS FROM PR_TMAE_CALBEN1 T INNER JOIN TBL_TEMP_CALBEN1 Temp ON T.NUM_POLIZA = Temp.NUM_POLIZA AND T.NUM_ORDEN = Temp.NUM_ORDEN";

        string strUpdateCALBEN2 = "UPDATE T SET T.COD_BASE = Temp.COD_BASE, " +
                                  "T.NUM_EDAD = Temp.NUM_EDAD, " +
                                  "T.MTO_CNUBAS = Temp.MTO_CNUBAS, " +
                                  "T.MTO_CNABAS = Temp.MTO_CNABAS, " +
                                  "T.MTO_CNGBAS = Temp.MTO_CNGBAS, " +
                                  "T.MTO_CNTBAS = Temp.MTO_CNTBAS FROM PR_TMAE_CALBEN2 T INNER JOIN TBL_TEMP_CALBEN2 Temp ON T.NUM_POLIZA = Temp.NUM_POLIZA AND T.NUM_ORDEN = Temp.NUM_ORDEN";

        string strUpdateCALBEN1Antigua = "UPDATE T SET T.MTO_CNUBAS_METAN = Temp.MTO_CNUBAS, " +
                                          "T.MTO_CNABAS_METAN = Temp.MTO_CNABAS, " +
                                          "T.MTO_CNGBAS_METAN = Temp.MTO_CNGBAS, " +
                                          "T.MTO_CNTBAS_METAN = Temp.MTO_CNTBAS FROM PR_TMAE_CALBEN1 T INNER JOIN TBL_TEMP_CALBEN1 Temp ON T.NUM_POLIZA = Temp.NUM_POLIZA AND T.NUM_ORDEN = Temp.NUM_ORDEN";

        string strUpdateCALBEN2Antigua = "UPDATE T SET T.MTO_CNUBAS_METAN = Temp.MTO_CNUBAS, " +
                                          "T.MTO_CNABAS_METAN = Temp.MTO_CNABAS, " +
                                          "T.MTO_CNGBAS_METAN = Temp.MTO_CNGBAS, " +
                                          "T.MTO_CNTBAS_METAN = Temp.MTO_CNTBAS FROM PR_TMAE_CALBEN2 T INNER JOIN TBL_TEMP_CALBEN2 Temp ON T.NUM_POLIZA = Temp.NUM_POLIZA AND T.NUM_ORDEN = Temp.NUM_ORDEN";

        string strDeleteTablas = "DROP TABLE TBL_TEMP_CALPOL1 \n" +
                                 "DROP TABLE TBL_TEMP_CALPOL2 \n" +
                                 "DROP TABLE TBL_TEMP_CALBEN1 \n" +
                                 "DROP TABLE TBL_TEMP_CALBEN2 \n";
        #endregion

        [STAThread]
        public async Task<string> CalculaResMat(string FecCal, double TicCam, List<beResultadosFlujos> ListaFluPar,
                                    List<beDatosPol> ListaPolPar, List<beDatosBen> ListaBenPar, List<beCurvaTasas> ListaGtoSepelioPar, List<beResultadosFlujos> ListaFluParAnt)
        {
            XmlConfigurator.Configure();

            #region DataTables para actualización
            PR_TMAE_CALPOL1.Columns.Add("NUM_POLIZA", typeof(string));
            PR_TMAE_CALPOL1.Columns.Add("COD_BASE", typeof(string));
            PR_TMAE_CALPOL1.Columns.Add("MTO_RESBAS", typeof(decimal));
            PR_TMAE_CALPOL1.Columns.Add("MTO_RESBASRET", typeof(decimal));

            PR_TMAE_CALPOL2.Columns.Add("NUM_POLIZA", typeof(string));
            PR_TMAE_CALPOL2.Columns.Add("COD_BASE", typeof(string));
            PR_TMAE_CALPOL2.Columns.Add("MTO_RESBAS", typeof(decimal));
            PR_TMAE_CALPOL2.Columns.Add("MTO_RESBASRET", typeof(decimal));

            PR_TMAE_CALBEN1.Columns.Add("NUM_POLIZA", typeof(string));
            PR_TMAE_CALBEN1.Columns.Add("COD_BASE", typeof(string));
            PR_TMAE_CALBEN1.Columns.Add("NUM_ORDEN", typeof(int));
            PR_TMAE_CALBEN1.Columns.Add("NUM_EDAD", typeof(int));
            PR_TMAE_CALBEN1.Columns.Add("MTO_CNUBAS", typeof(decimal));
            PR_TMAE_CALBEN1.Columns.Add("MTO_CNABAS", typeof(decimal));
            PR_TMAE_CALBEN1.Columns.Add("MTO_CNGBAS", typeof(decimal));
            PR_TMAE_CALBEN1.Columns.Add("MTO_CNTBAS", typeof(decimal));

            PR_TMAE_CALBEN2.Columns.Add("NUM_POLIZA", typeof(string));
            PR_TMAE_CALBEN2.Columns.Add("COD_BASE", typeof(string));
            PR_TMAE_CALBEN2.Columns.Add("NUM_ORDEN", typeof(int));
            PR_TMAE_CALBEN2.Columns.Add("NUM_EDAD", typeof(int));
            PR_TMAE_CALBEN2.Columns.Add("MTO_CNUBAS", typeof(decimal));
            PR_TMAE_CALBEN2.Columns.Add("MTO_CNABAS", typeof(decimal));
            PR_TMAE_CALBEN2.Columns.Add("MTO_CNGBAS", typeof(decimal));
            PR_TMAE_CALBEN2.Columns.Add("MTO_CNTBAS", typeof(decimal));
            #endregion
            
            bool ResultadosRutina = true;
            beResultadosResBas ResultadoResBas = new beResultadosResBas();
            List<beResultadosResBasBen> ResultadoResBasBen = new List<beResultadosResBasBen>();

            List<beDatosPol> LisTabPolMatPar = new List<beDatosPol>();
            List<beDatosBen> LisTabBenMatPar = new List<beDatosBen>();
            List<beResultadosFlujos> LisFlujosPenPar = new List<beResultadosFlujos>();

            List<Task> lstTareasCalculoReservas = new List<Task>();

            string querysTot = "";
            string Tip = "";
            double TipCam = TicCam; //3.373; //TicCam
            string strConexionSeguroRV = _ReservasRepository.cadena_conexion();

            #region Carga Flujos de Pension

            #endregion

            #region Carga Poliza

            #endregion

            #region Carga Beneficiarios



            #endregion

            #region Datos Gastos Sepelio
            //obtiene EL GASTO DE SEPELIO DEL MES
            List<beCurvaTasas> ListaGtoSepelio = new List<beCurvaTasas>();
            double ValGS = 0;
            ListaGtoSepelio = (from GtoSep in ListaGtoSepelioPar where Convert.ToInt32(GtoSep.FEC_INICUOMOR) <= Convert.ToInt32(FecCal) && Convert.ToInt32(FecCal) <= Convert.ToInt32(GtoSep.FEC_TERCUOMOR) select GtoSep).ToList();
            ValGS = ListaGtoSepelio[0].MTO_CUOMOR;
            #endregion


            ////LLAMA LISTADO PAA OBTENER LOS DATOS DE COTIZACION
            try
            {
                //INICIA CÁLCULO DE RESERVA PARA FLUJOS NUEVOS
                for (var i = 0; i < ListaPolPar.Count; i++)
                {
                    Tip = "";

                    LisTabPolMatPar = (from pol in ListaPolPar where Convert.ToInt32(pol.NumPol) == Convert.ToInt32(ListaPolPar[i].NumPol) select pol).ToList();
                    if (LisTabPolMatPar.Count == 1)
                    {
                        LisTabBenMatPar = (from ben in ListaBenPar where ListaPolPar[i].NumPol == ben.NumPol select ben).ToList();
                        LisFlujosPenPar = (from flu in ListaFluPar where Convert.ToInt32(ListaPolPar[i].NumPol) == Convert.ToInt32(flu.numPol) select flu).ToList();

                        switch (LisTabPolMatPar[0].Tip)
                        {
                            case "1":
                                Tip = "1";
                                break;

                            case "2":
                                Tip = "2";
                                break;
                        }

                        Console.WriteLine("Memory used before collection:       {0:N0}",
                        GC.GetTotalMemory(false));
                        if ((LisTabPolMatPar.Count != 0) && (LisTabBenMatPar.Count != 0))
                        {
                            var task = EjecutarCalculoReservas(LisFlujosPenPar, LisTabPolMatPar, LisTabBenMatPar, FecCal, TipCam, ValGS, Tip, "N");
                            //querysTot += RActuarial.RutinaActResMat(LisFlujosPenPar, LisTabPolMatPar, LisTabBenMatPar, FecCal, TipCam, ValGS, Tip, "N");
                            //_log.Info("Se procesó la Póliza: " + LisTabPolMatPar[0].NumPol);
                            lstTareasCalculoReservas.Add(task);
                        }
                    }
                }
                while (lstTareasCalculoReservas.Count > 0)
                {
                    Task firstFinishedTask = await Task.WhenAny(lstTareasCalculoReservas);
                    lstTareasCalculoReservas.Remove(firstFinishedTask);
                }
                

                GC.Collect();
                Console.WriteLine("Memory used after full collection:   {0:N0}",
                                  GC.GetTotalMemory(true));
                GCSettings.LargeObjectHeapCompactionMode = GCLargeObjectHeapCompactionMode.CompactOnce;
                GC.Collect(2, GCCollectionMode.Forced, true, true);

                #region Llenado de tablas para actualización
                foreach (var dt in lstTablaCALPOL1)
                {
                    foreach (DataRow item in dt.Rows)
                    {
                        DataRow row = PR_TMAE_CALPOL1.NewRow();

                        row["NUM_POLIZA"] = item[0];
                        row["COD_BASE"] = item[1];
                        row["MTO_RESBAS"] = item[2];
                        row["MTO_RESBASRET"] = item[3];

                        PR_TMAE_CALPOL1.Rows.Add(row);
                    }
                }

                foreach (var dt in lstTablaCALPOL2)
                {
                    foreach (DataRow item in dt.Rows)
                    {
                        DataRow row = PR_TMAE_CALPOL2.NewRow();

                        row["NUM_POLIZA"] = item[0];
                        row["COD_BASE"] = item[1];
                        row["MTO_RESBAS"] = item[2];
                        row["MTO_RESBASRET"] = item[3];

                        PR_TMAE_CALPOL2.Rows.Add(row);
                    }
                }

                foreach (var dt in lstTablaCALBEN1)
                {
                    foreach (DataRow item in dt.Rows)
                    {
                        DataRow row = PR_TMAE_CALBEN1.NewRow();

                        row["NUM_POLIZA"] = item[0];
                        row["COD_BASE"] = item[1];
                        row["NUM_ORDEN"] = item[2];
                        row["NUM_EDAD"] = item[3];
                        row["MTO_CNUBAS"] = item[4];
                        row["MTO_CNABAS"] = item[5];
                        row["MTO_CNGBAS"] = item[6];
                        row["MTO_CNTBAS"] = item[7];

                        PR_TMAE_CALBEN1.Rows.Add(row);
                    }
                }

                foreach (var dt in lstTablaCALBEN2)
                {
                    foreach (DataRow item in dt.Rows)
                    {
                        DataRow row = PR_TMAE_CALBEN2.NewRow();

                        row["NUM_POLIZA"] = item[0];
                        row["COD_BASE"] = item[1];
                        row["NUM_ORDEN"] = item[2];
                        row["NUM_EDAD"] = item[3];
                        row["MTO_CNUBAS"] = item[4];
                        row["MTO_CNABAS"] = item[5];
                        row["MTO_CNGBAS"] = item[6];
                        row["MTO_CNTBAS"] = item[7];

                        PR_TMAE_CALBEN2.Rows.Add(row);
                    }
                }
                #endregion

                _log.Info("Se actualizarán los datos en BD. Por favor espere...");
                #region Creación e inserción en temporales y actualización de datos en tablas de flujos.
                //Lineas para crear las tablas en las cuales se guardará la información temporalmente.
                _ReservasRepository.Ejecuta_Query_Conn(strTablasTemporales, strConexionSeguroRV);

                //Lineas para insertar la información en las tablas.
                if (PR_TMAE_CALPOL1.Rows.Count != 0) { _ReservasRepository.BulkInsertFlujos(PR_TMAE_CALPOL1, "TBL_TEMP_CALPOL1", strConexionSeguroRV); }
                if (PR_TMAE_CALPOL2.Rows.Count != 0) { _ReservasRepository.BulkInsertFlujos(PR_TMAE_CALPOL2, "TBL_TEMP_CALPOL2", strConexionSeguroRV); }
                if (PR_TMAE_CALBEN1.Rows.Count != 0) { _ReservasRepository.BulkInsertFlujos(PR_TMAE_CALBEN1, "TBL_TEMP_CALBEN1", strConexionSeguroRV); }
                if (PR_TMAE_CALBEN2.Rows.Count != 0) { _ReservasRepository.BulkInsertFlujos(PR_TMAE_CALBEN2, "TBL_TEMP_CALBEN2", strConexionSeguroRV); }

                //Lineas para realizar la actualización de los registros en cada tabla directamente en SeguroRV.
                if (PR_TMAE_CALPOL1.Rows.Count != 0) { _ReservasRepository.Ejecuta_Query_Conn(strUpdateCALPOL1, strConexionSeguroRV); }
                if (PR_TMAE_CALPOL2.Rows.Count != 0) { _ReservasRepository.Ejecuta_Query_Conn(strUpdateCALPOL2, strConexionSeguroRV); }
                if (PR_TMAE_CALBEN1.Rows.Count != 0) { _ReservasRepository.Ejecuta_Query_Conn(strUpdateCALBEN1, strConexionSeguroRV); }
                if (PR_TMAE_CALBEN2.Rows.Count != 0) { _ReservasRepository.Ejecuta_Query_Conn(strUpdateCALBEN2, strConexionSeguroRV); }

                //Lineas para borrar las tablas temporales.
                _ReservasRepository.Ejecuta_Query_Conn(strDeleteTablas, strConexionSeguroRV);
                #endregion

                ////Se limpian los DataTables para utilizarlos con el cálculo antigüo 
                //PR_TMAE_CALPOL1.Clear();
                //PR_TMAE_CALPOL2.Clear();
                //PR_TMAE_CALBEN1.Clear();
                //PR_TMAE_CALBEN2.Clear();

                ////Se limpian las listas de DataTables para utilizarlos con el cálculo antigüo 
                //lstTablaCALPOL1.Clear();
                //lstTablaCALPOL2.Clear();
                //lstTablaCALBEN1.Clear();
                //lstTablaCALBEN2.Clear();

                ////_ReservasRepository.EjecutaScripts_CalculoFlujos(querysTot);
                //_log.Info("Se han actualizado los datos del cálculo nuevo correctamente.");

                ////await Task.WhenAll(tareasCalRes);
                ////INICIA CÁLCULO DE RESERVA PARA FLUJOS ANTIGÜOS
                //int limitPol = ListaPolPar.Count > 287 ? 287 : ListaPolPar.Count();
                //for (var i = 0; i < limitPol; i++)
                //{
                //    Tip = "";

                //    LisTabPolMatPar = (from pol in ListaPolPar where Convert.ToInt32(pol.NumPol) == Convert.ToInt32(ListaPolPar[i].NumPol) select pol).ToList();
                //    if (LisTabPolMatPar.Count == 1)
                //    {
                //        LisTabBenMatPar = (from ben in ListaBenPar where ListaPolPar[i].NumPol == ben.NumPol select ben).ToList();
                //        LisFlujosPenPar = (from flu in ListaFluParAnt where Convert.ToInt32(ListaPolPar[i].NumPol) == Convert.ToInt32(flu.numPol) select flu).ToList();

                //        switch (LisTabPolMatPar[0].Tip)
                //        {
                //            case "1":
                //                Tip = "1";
                //                break;

                //            case "2":
                //                Tip = "2";
                //                break;
                //        }

                //        Console.WriteLine("Memory used before collection:       {0:N0}",
                //        GC.GetTotalMemory(false));
                //        if ((LisTabPolMatPar.Count != 0) && (LisTabBenMatPar.Count != 0))
                //        {
                //            var task = EjecutarCalculoReservas(LisFlujosPenPar, LisTabPolMatPar, LisTabBenMatPar, FecCal, TipCam, ValGS, Tip, "A");
                //            //querysTot += RActuarial.RutinaActResMat(LisFlujosPenPar, LisTabPolMatPar, LisTabBenMatPar, FecCal, TipCam, ValGS, Tip, "A");
                //            //_log.Info("Se procesó la Póliza: " + LisTabPolMatPar[0].NumPol);
                //            lstTareasCalculoReservas.Add(task);
                //        }
                //    }
                //}

                //while (lstTareasCalculoReservas.Count > 0)
                //{
                //    Task firstFinishedTask = await Task.WhenAny(lstTareasCalculoReservas);
                //    lstTareasCalculoReservas.Remove(firstFinishedTask);
                //}
                

                //GC.Collect();
                //Console.WriteLine("Memory used after full collection:   {0:N0}",
                //                  GC.GetTotalMemory(true));
                //GCSettings.LargeObjectHeapCompactionMode = GCLargeObjectHeapCompactionMode.CompactOnce;
                //GC.Collect(2, GCCollectionMode.Forced, true, true);

                //#region Llenado de tablas para actualización
                //foreach (var dt in lstTablaCALPOL1)
                //{
                //    foreach (DataRow item in dt.Rows)
                //    {
                //        DataRow row = PR_TMAE_CALPOL1.NewRow();

                //        row["NUM_POLIZA"] = item[0];
                //        row["COD_BASE"] = item[1];
                //        row["MTO_RESBAS"] = item[2];
                //        row["MTO_RESBASRET"] = item[3];

                //        PR_TMAE_CALPOL1.Rows.Add(row);
                //    }
                //}

                //foreach (var dt in lstTablaCALPOL2)
                //{
                //    foreach (DataRow item in dt.Rows)
                //    {
                //        DataRow row = PR_TMAE_CALPOL2.NewRow();

                //        row["NUM_POLIZA"] = item[0];
                //        row["COD_BASE"] = item[1];
                //        row["MTO_RESBAS"] = item[2];
                //        row["MTO_RESBASRET"] = item[3];

                //        PR_TMAE_CALPOL2.Rows.Add(row);
                //    }
                //}

                //foreach (var dt in lstTablaCALBEN1)
                //{
                //    foreach (DataRow item in dt.Rows)
                //    {
                //        DataRow row = PR_TMAE_CALBEN1.NewRow();

                //        row["NUM_POLIZA"] = item[0];
                //        row["COD_BASE"] = item[1];
                //        row["NUM_ORDEN"] = item[2];
                //        row["NUM_EDAD"] = item[3];
                //        row["MTO_CNUBAS"] = item[4];
                //        row["MTO_CNABAS"] = item[5];
                //        row["MTO_CNGBAS"] = item[6];
                //        row["MTO_CNTBAS"] = item[7];

                //        PR_TMAE_CALBEN1.Rows.Add(row);
                //    }
                //}

                //foreach (var dt in lstTablaCALBEN2)
                //{
                //    foreach (DataRow item in dt.Rows)
                //    {
                //        DataRow row = PR_TMAE_CALBEN2.NewRow();

                //        row["NUM_POLIZA"] = item[0];
                //        row["COD_BASE"] = item[1];
                //        row["NUM_ORDEN"] = item[2];
                //        row["NUM_EDAD"] = item[3];
                //        row["MTO_CNUBAS"] = item[4];
                //        row["MTO_CNABAS"] = item[5];
                //        row["MTO_CNGBAS"] = item[6];
                //        row["MTO_CNTBAS"] = item[7];

                //        PR_TMAE_CALBEN2.Rows.Add(row);
                //    }
                //}
                //#endregion

                //_log.Info("Se actualizarán los datos en BD. Por favor espere...");
                //#region Creación e inserción en temporales y actualización de datos en tablas de flujos.
                ////Lineas para crear las tablas en las cuales se guardará la información temporalmente.
                //_ReservasRepository.Ejecuta_Query_Conn(strTablasTemporales, strConexionSeguroRV);

                ////Lineas para insertar la información en las tablas.
                //if (PR_TMAE_CALPOL1.Rows.Count != 0) { _ReservasRepository.BulkInsertFlujos(PR_TMAE_CALPOL1, "TBL_TEMP_CALPOL1", strConexionSeguroRV); }
                //if (PR_TMAE_CALPOL2.Rows.Count != 0) { _ReservasRepository.BulkInsertFlujos(PR_TMAE_CALPOL2, "TBL_TEMP_CALPOL2", strConexionSeguroRV); }
                //if (PR_TMAE_CALBEN1.Rows.Count != 0) { _ReservasRepository.BulkInsertFlujos(PR_TMAE_CALBEN1, "TBL_TEMP_CALBEN1", strConexionSeguroRV); }
                //if (PR_TMAE_CALBEN2.Rows.Count != 0) { _ReservasRepository.BulkInsertFlujos(PR_TMAE_CALBEN2, "TBL_TEMP_CALBEN2", strConexionSeguroRV); }

                ////Lineas para realizar la actualización de los registros en cada tabla directamente en SeguroRV.
                //if (PR_TMAE_CALPOL1.Rows.Count != 0) { _ReservasRepository.Ejecuta_Query_Conn(strUpdateCALPOL1Antigua, strConexionSeguroRV); }
                //if (PR_TMAE_CALPOL2.Rows.Count != 0) { _ReservasRepository.Ejecuta_Query_Conn(strUpdateCALPOL2Antigua, strConexionSeguroRV); }
                //if (PR_TMAE_CALBEN1.Rows.Count != 0) { _ReservasRepository.Ejecuta_Query_Conn(strUpdateCALBEN1Antigua, strConexionSeguroRV); }
                //if (PR_TMAE_CALBEN2.Rows.Count != 0) { _ReservasRepository.Ejecuta_Query_Conn(strUpdateCALBEN2Antigua, strConexionSeguroRV); }

                ////Lineas para borrar las tablas temporales.
                //_ReservasRepository.Ejecuta_Query_Conn(strDeleteTablas, strConexionSeguroRV);
                //#endregion

                ////_ReservasRepository.EjecutaScripts_CalculoFlujos(querysTot);
                //_log.Info("Se han actualizado los datos correctamente.");


                return "Reserva Base Matemática con éxito.";

            }
            catch (Exception ex)
            {
                _log.Info("Error en ciclo de Cálculo de Reservas: " + ex.Message.ToString());
                return "Error en el Cálculo de Reservas. " + ex.Message.ToString();
            }


        }

        async Task EjecutarCalculoReservas(List<beResultadosFlujos> pLstFlujosReservas, List<beDatosPol> pLstPolizas, List<beDatosBen> pLstBeneficiarios,
                                   string pFechaCalculo, double pTipoCambio, double pGastoSepelio, string pTipoTabla, string pRutina)
        {
            List<DataTable> lstTablasCalculoReservas = new List<DataTable>();

            await Task.Factory.StartNew(() =>
            {
                lstTablasCalculoReservas = RActuarial.RutinaActResMat(pLstFlujosReservas, pLstPolizas, pLstBeneficiarios, pFechaCalculo, pTipoCambio, pGastoSepelio, pTipoTabla, pRutina);

                if (pTipoTabla == "1")
                {
                    lstTablaCALPOL1.Add(lstTablasCalculoReservas[0]);
                    lstTablaCALBEN1.Add(lstTablasCalculoReservas[1]);
                }
                else
                {
                    lstTablaCALPOL2.Add(lstTablasCalculoReservas[0]);
                    lstTablaCALBEN2.Add(lstTablasCalculoReservas[1]);
                }
            });
        }

    }
}
