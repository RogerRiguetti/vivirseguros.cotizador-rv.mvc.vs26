using Estudio.Repository.Core.Domain;
using Estudio.Repository.Helpers;
using Estudio.Repository.Persistence.Repositories;
using System;
using System.Collections.Generic;
using Estudio.Process.Muestra;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml;
using System.Net;
using System.IO;
using System.Data;
using System.Runtime;
using log4net.Config;
using log4net;
using System.Reflection;

namespace Estudio.Logic
{
    public class CalcularAsignacionIntermediarioLogic
    {
        private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public string queryGlobal = String.Empty;
        public async Task<Response> CalcularAsignacionIntermediario(List<SolicitudesCotizacion> informacion,int Num_Archivo, string usuario)
        {
            
            List<AsignacionIntermediario> ListdatosAI = new List<AsignacionIntermediario>();
            CalcularAsignacionIntermediarioRepository _CalcularAsignacionIntermediarioRepository = new CalcularAsignacionIntermediarioRepository();
            PruebasRutinaOficiales _pruebasRutinaOficiales = new PruebasRutinaOficiales();
            RutinaOficialesRepository _rutinaOficialesRepository = new RutinaOficialesRepository();
            SISCORepository _SISCORepository = new SISCORepository();
            try
            {
                Response res = new Response();
                #region CODIGO COMENTADO VALIDACIONES SEACSA 
                //DateTime fechaActual = DateTime.Now;
                //DateTime fechaActualMesYear = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM"));
                ////Valida que exista IPC a la Fecha de Cálculo
                //if (CalcularAsignacionIntermediarioRepository.ValidaIPC(fechaActualMesYear.AddMonths(-1)) == false) {
                //    res.Message = "No existe IPC a la Fecha de Cálculo";
                //    res.Object = informacion;
                //    res.IsOk = false;
                //    return res;
                //}
                ////Valida que exista Tasa de Mercado a la Fecha de Cálculo
                //if (CalcularAsignacionIntermediarioRepository.ValidaTM(fechaActualMesYear) == false)
                //{
                //    res.Message = "No existe Tasa de Mercado a la Fecha de Cálculo, para US y/o NS.";
                //    res.Object = informacion;
                //    res.IsOk = false;
                //    return res;
                //}

                ////Verifica si existe el Departamento Estándar y sus parámetros
                //if (CalcularAsignacionIntermediarioRepository.ValidaDeptoEstandar("C", fechaActual) == false)
                //{
                //    res.Message = "No se encuentra definido el Departamento Estándar o No existen parámetros para la fecha";
                //    res.Object = informacion;
                //    res.IsOk = false;
                //    return res;
                //}
                ////Permite obtener el Indicador de si se cálculan todas las modalidades cuando exista a lo menos una en Soles
                //string fgBuscarIndCalcularModSoles = CalcularAsignacionIntermediarioRepository.fgBuscarIndCalcularModSoles();

                //List<string> Reajuste = new List<string>();
                ////Obtiene la Tasa de Reajuste en Dólares
                //Reajuste = CalcularAsignacionIntermediarioRepository.BuscarValReajuste(fechaActual, "US");
                //if (Reajuste.Count == 0) {
                //    res.Message = "No se ha ingresado el % Reajuste Fijo para Dólares.";
                //    res.Object = informacion;
                //    res.IsOk = false;
                //    return res;
                //}
                //Double Prc_ReajusteUS_Tri = Convert.ToDouble(Reajuste[0]);
                //Double Prc_ReajusteUS_Men = Convert.ToDouble(Reajuste[1]);
                //Double Prc_ReajusteUS_Anu = Convert.ToDouble(Reajuste[2]);
                ////Obtiene la Tasa de Reajuste en Soles
                //Reajuste = CalcularAsignacionIntermediarioRepository.BuscarValReajuste(fechaActual, "NS");
                //if (Reajuste.Count == 0)
                //{
                //    res.Message = "No se ha ingresado el % Reajuste Fijo para Soles.";
                //    res.Object = informacion;
                //    res.IsOk = false;
                //    return res;
                //}
                //Double Prc_ReajusteNS_Tri = Convert.ToDouble(Reajuste[0]);
                //Double Prc_ReajusteNS_Men = Convert.ToDouble(Reajuste[1]);
                //Double Prc_ReajusteNS_Anu = Convert.ToDouble(Reajuste[2]);

                //Buscar los Nros de Cotizacion Sin Error y que coincidan con el Filtro Seleccionado de Tipo de Pensión3
                //
                //'BUSCA LOS NROS DE COTIZACION SIN RECHAZO
                //CalcularAsignacionIntermediarioRepository.Busca_Num_Cot(Num_Archivo);


                /*List<AsignacionIntermediario> TblMortalidad = new List<AsignacionIntermediario>(); //Tabla de mortalidad
                List<AsignacionIntermediario> TblGastos = new List<AsignacionIntermediario>(); //Tabla Gastos
                List<AsignacionIntermediario> dtTA = new List<AsignacionIntermediario>(); //Tasa de Anclaje TA
                List<AsignacionIntermediario> TblTM = new List<AsignacionIntermediario>(); //Tabla Tasa de Mercado
                List<AsignacionIntermediario> dtBen = new List<AsignacionIntermediario>(); //llena dt de beneficiarios para el cálculo
                List<AsignacionIntermediario> dtMatriz = new List<AsignacionIntermediario>();
                List<AsignacionIntermediario> TblBeneficiario = new List<AsignacionIntermediario>(); //Tabla de Beneficiarios
                */
                #endregion

                List<List<beResultados>> rutinaOficiales = new List<List<beResultados>>(); //Lista de resultados de rutina
                if (informacion.Count > 0)
                { //Si hay Nros de Cotizaciones a calcular

                    //Se carga lista con registros de tabla mortalidad.

                    /* TblMortalidad = _CalcularAsignacionIntermediarioRepository.Tbl_Mortalidad("T");

                     //TblGastos = _CalcularAsignacionIntermediarioRepository.Tbl_Gastos(DateTime.Today, "C", usuario);
                     TblTM = _CalcularAsignacionIntermediarioRepository.Tbl_TM();

                     DateTime fecha = DateTime.Now;
                     dtTA = _CalcularAsignacionIntermediarioRepository.LLenar_dt_TA(fecha.ToString("yyyyMMdd"));
                     */

                    List<beMortalidadDin> LisTabDin = new List<beMortalidadDin>();
                    List<beMortalidadDinDet> LisTabMD = new List<beMortalidadDinDet>();
                    List<beDatosModalidad> VarMTGS = new List<beDatosModalidad>();
                    List<beDatosModalidad> VarAFP = new List<beDatosModalidad>();
                    List<beDatosModalidad> VarREG = new List<beDatosModalidad>();
                    List<bePorcenLegales> LisTabPL = new List<bePorcenLegales>();
                    List<beTasaAnclaje> ListaTA = new List<beTasaAnclaje>();
                    List<beCPK> ListaCPK = new List<beCPK>();
                    List<beRentabilidad> ListaRen = new List<beRentabilidad>();
                    List<beTasasPromedio> ListaTasProm = new List<beTasasPromedio>();
                    List<beCurvaTasas> ListaCurvaTasas = new List<beCurvaTasas>();
                   // List<SISCO> CotSISCO = new List<SISCO>();

                    LisTabDin = _rutinaOficialesRepository.ConsultaTablaMortalidadDinamicas("");
                    LisTabMD = _rutinaOficialesRepository.ConsultaDetTablaMortalidadDin();
                    VarMTGS = _rutinaOficialesRepository.ConsultaGastoSepelioMes("");
                    VarAFP = _rutinaOficialesRepository.ConsultaRentabilidadAfp("");
                    VarREG = _rutinaOficialesRepository.ConsultaRegionTasas(0);
                    LisTabPL = _rutinaOficialesRepository.ConsultaPorcentaje("");
                    ListaTA = _rutinaOficialesRepository.ConsultaTasaAnclaje("");
                    ListaCPK = _rutinaOficialesRepository.ConsultaCPKS("");
                    ListaRen = _rutinaOficialesRepository.ConsultaRentabilidad("");
                    ListaTasProm = _rutinaOficialesRepository.ConsultaTasasPromedio("");
                    ListaCurvaTasas = _rutinaOficialesRepository.ConsultaCurvaTasas("");
                    //string montoPenSISCO = "";

                    for (int i = 0; i < informacion.Count; i++)
                    {
                        _pruebasRutinaOficiales = new PruebasRutinaOficiales();
                        _CalcularAsignacionIntermediarioRepository.updateAsesor(informacion[i].idAsignado.ToString(), informacion[i].intNumOpe.ToString(), Convert.ToDouble(informacion[i].strMonPro));
                        #region Código comentado
                        /*AsignacionIntermediario datosAI = new AsignacionIntermediario();
                        datosAI.strNumCot = informacion[i].strNumCot;
                        AsignacionIntermediario flDatosDetalle = _CalcularAsignacionIntermediarioRepository.flDatosDetalle(datosAI.strNumCot); //Busca tipo pension, AFP y fechas de devengue y proceso
                        datosAI.strFecDev = flDatosDetalle.strFecDev;
                        datosAI.strTipPen = flDatosDetalle.strTipPen;
                        datosAI.strIndCob = flDatosDetalle.strIndCob;
                        datosAI.strAfp = flDatosDetalle.strAfp;
                        string anio = fecha.ToString("yyyyMMdd");
                        datosAI.strFecSol = anio; //Fecha Cálculo

                        AsignacionIntermediario flBusDatBen = _CalcularAsignacionIntermediarioRepository.flBusDatBen(datosAI.strNumCot); //Busca sexo y fecha de nacimiento del causante
                        datosAI.strSexo = flBusDatBen.strSexo;
                        datosAI.strFecNac = flBusDatBen.strFecNac;
                        datosAI.strFecSol = anio; //Fecha Cálculo
                        AsignacionIntermediario flCalculaDatos = _CalcularAsignacionIntermediarioRepository.flCalculaDatos(datosAI.strFecNac, datosAI.strSexo, datosAI.strNumCot, datosAI.strAfp, datosAI.strFecDev);//Calcula Edad Actual, Tipo de Vejez
                        datosAI.vlEdad = flCalculaDatos.vlEdad;
                        datosAI.vlAnnoJub = flCalculaDatos.vlAnnoJub;
                        datosAI.vlNumBenef = flCalculaDatos.vlNumBenef;
                        datosAI.vgPalabra = flCalculaDatos.vgPalabra;
                        datosAI.vgRentabilidadAFP = flCalculaDatos.vgRentabilidadAFP;
                        datosAI.douCuoMor = flCalculaDatos.douCuoMor;

                        datosAI.vlCalculoBono = true; //Sw de cálculo de Bono en caso de Meler no se calcula bono, siempre es true
                        
                        TblBeneficiario = _CalcularAsignacionIntermediarioRepository.Tbl_Beneficiario(datosAI.strNumCot);*/
                        #endregion

                        Console.WriteLine("Memory used before collection:       {0:N0}",
                        GC.GetTotalMemory(false));

                        rutinaOficiales.Add(await _pruebasRutinaOficiales.RutinaOficiales(informacion[i].intNumOpe, informacion[i].strNumCot, Num_Archivo, LisTabDin, LisTabMD, VarMTGS, VarAFP, VarREG, LisTabPL, ListaTA, ListaCPK, ListaRen, ListaTasProm, ListaCurvaTasas, null, "", informacion[i].idAsignado));
                        //_SISCORepository.ConsultaSISCO(informacion[i].intNumOpe.ToString(), informacion[i].strCussp);
                        //SISCO valSISCO = new SISCO();

                        //valSISCO = _SISCORepository.ConsultaSISCO(informacion[i].intNumOpe.ToString(), informacion[i].strCussp);
                        //CotSISCO.Add(valSISCO);
                        
                        
                        // Collect all generations of memory.
                        GC.Collect();
                        Console.WriteLine("Memory used after full collection:   {0:N0}",
                                          GC.GetTotalMemory(true));
                        GCSettings.LargeObjectHeapCompactionMode = GCLargeObjectHeapCompactionMode.CompactOnce;
                        GC.Collect(2, GCCollectionMode.Forced, true, true);
                    }
                    List<Task> tareas = new List<Task>();
                    string query = String.Empty;
                    foreach (var item in rutinaOficiales)
                    {
                        //var task = GenerarScript(item);
                        //tareas.Add(task);
                        ////GC.Collect();
                        //////GC.WaitForPendingFinalizers();
                        ////Console.WriteLine("Memory used after full collection:   {0:N0}",
                        ////GC.GetTotalMemory(true));
                        foreach (var producto in item)
                        {
                            if (producto.Cod_Rechazo == "0")
                            {
                                query = "UPDATE PT_TMAE_DETCOTIZACION " +
                                        "SET " +
                                        "MTO_AJUSTEIPC = " + producto.MTO_AJUSTEIPC + ", \n" +
                                        "MTO_CTAINDAFP = " + producto.MTO_CTAINDAFP + ", \n" +
                                        "MTO_PENSION = " + producto.MTO_PENSION + ", \n" +
                                        "MTO_PRIUNIDIF = " + producto.MTO_PRIUNIDIF + ", \n" +
                                        "MTO_RENTATMPAFP = " + producto.MTO_RENTATMPAFP + ", \n" +
                                        "MTO_RESMAT = " + producto.MTO_RESMAT + ", \n" +
                                        "PRC_PERCON = " + producto.PRC_PERCON + ", \n" +
                                        "PRC_TASATCE = " + producto.PRC_TASATCE + ", \n" +
                                        "PRC_TASATIR = " + producto.PRC_TASATIR + ", \n" +
                                        "PRC_TIRINI = " + producto.PRC_TASATIR + ", \n" +
                                        "PRC_TASAVTA = " + producto.PRC_TASAVTA + ", \n" +
                                        "FEC_CALCULO = '" + producto.FecCal + "', \n" +
                                        "PRC_CORCOM = 0, \n" +
                                        "--IND_SISCO = 0, \n" +
                                        "--IND_FILTROCOTIZA = 'S', \n" +
                                        "COD_ESTCOT = 'S', \n" +
                                        "MTO_PENANUAL = " + producto.MTO_PENANUAL + ", \n " +
                                        "MTO_PENSIONGAR = " + producto.MTO_PENSIONGAR + ", \n " +
                                        "MTO_PRIUNISIM = " + producto.MTO_PRIUNISIM + ", \n " +
                                        "MTO_RMGTOSEP = " + producto.MTO_RMGTOSEP + ", \n " +
                                        "MTO_RMGTOSEPRV = " + producto.MTO_RMGTOSEPRV + ", \n	" +
                                        "MTO_VALREAJUSTEMEN = (CASE WHEN(cod_tipreajuste = 2) THEN 0.16515813 ELSE 0 END), \n" +
                                        "MTO_VALREAJUSTETRI = (CASE WHEN(cod_tipreajuste = 2) THEN 0.49629316 ELSE 0 END), \n" +
                                        "MTO_VALPREPENTMP = CASE WHEN(" + producto.MTO_RENTATMPAFP + " = 0) THEN 0  \n" +
                                        "ELSE ROUND((" + producto.MTO_CTAINDAFP + " / " + producto.MTO_RENTATMPAFP + "), 2) END \n" +
                                        "WHERE NUM_CORRELATIVO = " + producto.NUM_CORRELATIVO + " AND NUM_OPERACION = '" + producto.NUM_COTESTUDIO + "';";
                            }
                            else
                            {
                                query = "UPDATE PT_TMAE_DETCOTIZACION " +
                                        "SET COD_RECHAZO = '" + producto.Cod_Rechazo + "', \n " +
                                        "ERR_DESCRIP = '" + producto.Mensaje + "', \n" +
                                        "FEC_CALCULO = '" + producto.FecCal + "', \n" +
                                        "PRC_CORCOM = 0, \n" +
                                        "--IND_FILTROCOTIZA = 'N', \n " +
                                        "COD_ESTCOT = 'S' \n" +
                                        "WHERE NUM_CORRELATIVO = " + producto.NUM_CORRELATIVO + " AND NUM_OPERACION = '" + producto.NUM_COTESTUDIO + "';";
                            }

                            queryGlobal += query + " \n";
                        }

                    }

                    //await Task.WhenAll(tareas);
                    _log.Info("Se guardara la siguiente informacion " + queryGlobal);
                    
                    _CalcularAsignacionIntermediarioRepository.RegistroRutinaOficiales(queryGlobal);

                    _log.Info("Termino de guardarse correctamente ");

                    string queryUPSis = String.Empty;

                    
                    queryUPSis = "UPDATE PT_TMAE_DETCOTIZACION SET " +
                                      "IND_FILTROCOTIZA = 'S' WHERE NUM_ARCHIVO = " + Num_Archivo + " AND IND_SISCO = 1 ;";

                    _log.Info("Se modificara el filtro cotiza " + queryUPSis);

                    _CalcularAsignacionIntermediarioRepository.RegistroRutinaOficiales(queryUPSis);

                    _log.Info("Termino de guardarse correctamente el filtro cotiza");
                }
                #region Código comentado
                //UPDATE EN TABLAS
                //_CalcularAsignacionIntermediarioRepository.insertRutina(rutinaOficiales, usuario);
                /*AsignacionIntermediario fgCargarVariablesMortal = _CalcularAsignacionIntermediarioRepository.fgCargarVariablesMortal(datosAI.strFecSol);

                datosAI.vgNumeroTotalTablas = _CalcularAsignacionIntermediarioRepository.vgNumeroTotalTablas;
                datosAI.Cod_Tippension = datosAI.strTipPen;
                datosAI.Num_Cot = datosAI.strNumCot;
                var band = _CalcularAsignacionIntermediarioRepository.calculaFPE(datosAI.Num_Cot); //'calcular Factor Pensando en ella

                datosAI.cod_Par = _CalcularAsignacionIntermediarioRepository.cod_Par;
                datosAI.Num_Cor = _CalcularAsignacionIntermediarioRepository.Num_Cor;
                datosAI.MtoFacPenElla = _CalcularAsignacionIntermediarioRepository.MtoFacPenElla;
                datosAI.PrcFacPenElla = _CalcularAsignacionIntermediarioRepository.PrcFacPenElla;

                if (band == false)
                {
                   Response resBand = new Response();
                   resBand.IsOk = false;
                   resBand.Message = "Ha Ocurrido un Error al Calcular El Factor Pensando en Ella";
                   return res;
                }
                dtBen = _CalcularAsignacionIntermediarioRepository.llenar_Dt_Ben_Calculo(datosAI.Num_Cot, datosAI.Cod_Tippension, "08");

                AsignacionIntermediario fgFinTabAnual = _CalcularAsignacionIntermediarioRepository.fgFinTabAnual(fgCargarVariablesMortal, datosAI.vgNumeroTotalTablas, TblMortalidad);
                datosAI.FinTab = fgFinTabAnual.FinTab;

                dtMatriz = _CalcularAsignacionIntermediarioRepository.fgBuscarMortalidad(TblMortalidad, fgCargarVariablesMortal, datosAI, "A");
                AsignacionIntermediario fgBuscarMortalidad = _CalcularAsignacionIntermediarioRepository.local;

                List<AsignacionIntermediario> dtPar = new List<AsignacionIntermediario>(); //llena dt datos de cotizacion

                //llenar dt de datos de cotizacion
                dtPar = _CalcularAsignacionIntermediarioRepository.LLenar_dt_Cotiz(datosAI.FinTab, "C", "N", datosAI.Num_Cot, datosAI.Num_Cor);

                    if (dtBen.Count == 0 || TblGastos.Count == 0 || dtMatriz.Count == 0 || dtPar.Count == 0 || TblTM.Count == 0 || dtTA.Count == 0)
                    {
                        datosAI.vlCalcular = false;
                    }
                    else
                    {
                        datosAI.vlCalcular = true;
                    }
                    if (datosAI.vlCalcular == true)
                    {
                        //calcular la tasa
                        datosAI.Cod_TipTir = "I";
                        datosAI.Cod_DeptoEstandar = validaDeptoEstandar.Cod_DeptoEstandar;
                        //Calcula Tarifa *************************************************************************
                        List<AsignacionIntermediario> dtTarifaTodo = _CalcularAsignacionIntermediarioRepository.tarifa("C", datosAI.strNumCot, dtBen,dtPar,dtMatriz,TblGastos,TblTM,dtTA,usuario);
                        int posicion = dtTarifaTodo.Count - 1;
                        if (posicion >=0 && dtTarifaTodo[posicion].x != "")
                        {
                            res.Object = ListdatosAI;
                            res.Message = dtTarifaTodo[posicion].x;
                            res.IsOk = false;
                            return res;
                        }
                        if (dtTarifaTodo == null || dtTarifaTodo.Count == 0)
                        {
                            datosAI.vlCodError = "1024"; //No se cálculo la Tasa
                            _CalcularAsignacionIntermediarioRepository.actualiza_Rechazo_Gral(datosAI.vlCodError, datosAI.strNumCot,usuario,0);
                        }else
                        {
                            for (int j=0; j<dtTarifaTodo.Count; j++)
                            {
                                datosAI.prc_tasatce = dtTarifaTodo[j].prc_tasatce;
                                datosAI.PRC_TASAVTA = dtTarifaTodo[j].PRC_TASAVTA;
                                datosAI.prc_tasatir = dtTarifaTodo[j].prc_tasatir;
                                datosAI.mto_resmat = dtTarifaTodo[j].mto_resmat;
                                datosAI.NUM_CORRELATIVO = dtTarifaTodo[j].NUM_CORRELATIVO;
                                datosAI.num_cotestudio = dtTarifaTodo[j].num_cotestudio;
                                datosAI.mto_penanual = dtTarifaTodo[j].mto_penanual;
                                datosAI.mto_rmgtosep = dtTarifaTodo[j].mto_rmgtosep;
                                datosAI.mto_rmpension = dtTarifaTodo[j].mto_rmpension;
                                datosAI.prc_percon = dtTarifaTodo[j].prc_percon;
                                datosAI.prc_reajustetri = dtTarifaTodo[j].prc_reajustetri;
                                datosAI.prc_reajustemen = dtTarifaTodo[j].prc_reajustemen;
                                if (dtTarifaTodo[j].x != "")
                                {
                                    datosAI.Cod_Rechazo = "1001";
                                    _CalcularAsignacionIntermediarioRepository.actualiza_Rechazo_Gral(datosAI.vlCodError, datosAI.strNumCot, usuario, 0);
                                }
                            }
                            //Obtiene la Edad Final de las Tablas de Mortalidad
                            AsignacionIntermediario fgFinTabMensual = new AsignacionIntermediario();
                            fgFinTabMensual = _CalcularAsignacionIntermediarioRepository.fgFinTabMensual(fgCargarVariablesMortal, datosAI.vgNumeroTotalTablas, TblMortalidad);
                            datosAI.FinTab = fgFinTabMensual.FinTab;
                            dtMatriz = new List<AsignacionIntermediario>();
                            //llenar matriz
                            dtMatriz = _CalcularAsignacionIntermediarioRepository.fgBuscarMortalidad(TblMortalidad, fgCargarVariablesMortal, datosAI, "M");
                            datosAI.Num_Cot = datosAI.strNumCot;
                            dtPar = _CalcularAsignacionIntermediarioRepository.LLenar_dt_Cotiz(datosAI.FinTab, "C", "N", datosAI.Num_Cot, datosAI.Num_Cor);
                            if (dtMatriz.Count == 0 || dtPar.Count == 0)
                            {
                                datosAI.vlCodError = "1022";
                            }
                            else
                            {
                                //Calcula Renta Vitalicia ***************************************************
                                List<AsignacionIntermediario>  dtRtaVit = _CalcularAsignacionIntermediarioRepository.renta_vit("C", datosAI.strNumCot, dtBen, dtPar, dtMatriz, TblGastos, TblTM, dtTA, usuario);
                                int posicion2 = dtRtaVit.Count - 1;
                                if (posicion2 >= 0 && dtRtaVit[posicion2].x != "")
                                {
                                    res.Object = ListdatosAI;
                                    res.Message = dtRtaVit[posicion2].x;
                                    res.IsOk = false;
                                    return res;
                                }
                                if (dtRtaVit.Count == 0)
                                {
                                    datosAI.vlCodError = "1025";
                                    _CalcularAsignacionIntermediarioRepository.actualiza_Rechazo_Gral(datosAI.vlCodError, datosAI.strNumCot, usuario, 0);
                                }else
                                {
                                    for (int j=0; j< dtRtaVit.Count; j++)
                                    {
                                        datosAI.mto_priunisim = dtRtaVit[j].mto_priunisim;
                                        datosAI.mto_priunidif = dtRtaVit[j].mto_priunidif;
                                        datosAI.mto_pension = dtRtaVit[j].mto_pension;
                                        datosAI.mto_ctaindafp = dtRtaVit[j].mto_ctaindafp;
                                        datosAI.mto_rentatmpafp = dtRtaVit[j].mto_rentatmpafp;
                                        datosAI.mto_valprepentmp = dtRtaVit[j].mto_valprepentmp;
                                        datosAI.mto_rmgtoseprv  = dtRtaVit[j].mto_rmgtoseprv;
                                        datosAI.mto_sumpension = dtRtaVit[j].mto_sumpension;
                                        datosAI.mto_ajusteipc = dtRtaVit[j].mto_ajusteipc;
                                        datosAI.NUM_CORRELATIVO = dtRtaVit[j].NUM_CORRELATIVO;
                                        datosAI.marcasob = dtRtaVit[j].marcasob;
                                        datosAI.prc_reajustetri = dtRtaVit[j].prc_reajustetri;
                                        datosAI.prc_reajustemen = dtRtaVit[j].prc_reajustemen;
                                        string actMontos = _CalcularAsignacionIntermediarioRepository.actualizar_Montos(datosAI, usuario);
                                        if (actMontos != "")
                                        {
                                            datosAI.Cod_Rechazo = "1001";
                                            _CalcularAsignacionIntermediarioRepository.actualiza_Rechazo_Gral(datosAI.vlCodError, datosAI.strNumCot, usuario, 0);
                                        }
                                        bool calculaMtoPensionGar = _CalcularAsignacionIntermediarioRepository.calculaMtoPensionGar(datosAI.Num_Cot, datosAI.Num_Correlativo, datosAI.mto_pension);
                                        if (calculaMtoPensionGar == false)
                                        {
                                            datosAI.Cod_Rechazo = "1011";
                                            _CalcularAsignacionIntermediarioRepository.actualiza_Rechazo_Gral(datosAI.vlCodError, datosAI.strNumCot, usuario, 0);
                                        }
                                        string strFecSeg;
                                        int strEtapa;
                                        bool bolSw;
                                        datosAI.cod_etapa = "20";
                                        datosAI.Num_Cot = datosAI.strNumCot;
                                        datosAI.Num_Correlativo = dtRtaVit[j].NUM_CORRELATIVO;
                                        strFecSeg = _CalcularAsignacionIntermediarioRepository.busca_FechaServidor();
                                        datosAI.fec_Ini = strFecSeg;
                                        datosAI.Hor_Ini = _CalcularAsignacionIntermediarioRepository.busca_HoraSer();
                                        strEtapa = Convert.ToInt32(_CalcularAsignacionIntermediarioRepository.etapa_Anterior(datosAI.Num_Cot, datosAI.Num_Correlativo));
                                        if(strEtapa > 20)
                                        {
                                            //Elimina las etapas posteriores al calculo masivo
                                            _CalcularAsignacionIntermediarioRepository.eliminaEtapaPosterior(datosAI.Num_Cot, datosAI.Num_Correlativo, datosAI.cod_etapa);
                                            strEtapa = 0;
                                        }
                                        datosAI.Cod_EtapaAnt = strEtapa;
                                        //validar si se ha registrado anteriormente la misma etapa
                                        bolSw = _CalcularAsignacionIntermediarioRepository.validar(datosAI.cod_etapa, datosAI.Num_Cot, datosAI.Num_Correlativo);
                                        if (bolSw==false)
                                        {
                                            _CalcularAsignacionIntermediarioRepository.insert_Ingreso(datosAI.Num_Cot, datosAI.Num_Correlativo, datosAI.cod_etapa, datosAI.fec_Ini, datosAI.Hor_Ini, usuario, Convert.ToString(datosAI.Cod_EtapaAnt));
                                        }else
                                        {
                                            _CalcularAsignacionIntermediarioRepository.actualizar(datosAI.fec_Ini, datosAI.Hor_Ini, usuario, datosAI.Num_Cot, datosAI.Num_Correlativo, datosAI.cod_etapa, Convert.ToString(datosAI.Cod_EtapaAnt));
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        datosAI.vlCodError = "1022";
                        _CalcularAsignacionIntermediarioRepository.actualiza_Rechazo_Gral(datosAI.vlCodError, datosAI.strNumCot, usuario, 0);
                    }
                    ListdatosAI.Add(datosAI);
                }
                _CalcularAsignacionIntermediarioRepository.buscar(Num_Archivo, usuario);
                //ACTUALIZAR LA CUENTA CORRIENTE
                //buscar mayor pérdida contable de cada oferta
            }
            else
            {
                res.Object = ListdatosAI;
                res.Message = "No Existen Cotizaciones a Calcular";
                res.IsOk = false;
                return res;
            }*/
                #endregion

                res.Object = ListdatosAI;
                res.Message = "Proceso de Cálculo Masivo Finalizado";
                res.IsOk = true;
                return res;
            }
            catch (Exception ex)
            {
                Response res = new Response();
                res.IsOk = false;
                res.Message = ex.Message;
                return res;
            }
        }

        async Task GenerarScript(List<beResultados> cotizacion)
        {
            string query = String.Empty;
            await Task.Factory.StartNew(() =>
            {
                foreach (var producto in cotizacion)
                {
                    if(producto.Cod_Rechazo == "0")
                    {
                        query = "UPDATE PT_TMAE_DETCOTIZACION " +
                                    "SET " +
                                    "MTO_AJUSTEIPC = " + producto.MTO_AJUSTEIPC + ", \n" +
                                    "MTO_CTAINDAFP = " + producto.MTO_CTAINDAFP + ", \n" +
                                    "MTO_PENSION = " + producto.MTO_PENSION + ", \n" +
                                    "MTO_PRIUNIDIF = " + producto.MTO_PRIUNIDIF + ", \n" +
                                    "MTO_RENTATMPAFP = " + producto.MTO_RENTATMPAFP + ", \n" +
                                    "MTO_RESMAT = " + producto.MTO_RESMAT + ", \n" +
                                    "PRC_PERCON = " + producto.PRC_PERCON + ", \n" +
                                    "PRC_TASATCE = " + producto.PRC_TASATCE + ", \n" +
                                    "PRC_TASATIR = " + producto.PRC_TASATIR + ", \n" +
                                    "PRC_TIRINI = " + producto.PRC_TASATIR + ", \n" +
                                    "PRC_TASAVTA = " + producto.PRC_TASAVTA + ", \n" +
                                    "FEC_CALCULO = '" + producto.FecCal + "', \n" +
                                    "PRC_CORCOM = 0, \n" +
                                    "--IND_SISCO = 0, \n" +
                                    "--IND_FILTROCOTIZA = 'S', \n" +
                                    "COD_ESTCOT = 'S', \n" +
                                    "MTO_PENANUAL = " + producto.MTO_PENANUAL + ", \n " +
                                    "MTO_PENSIONGAR = " + producto.MTO_PENSIONGAR + ", \n " +
                                    "MTO_PRIUNISIM = " + producto.MTO_PRIUNISIM + ", \n " +
                                    "MTO_RMGTOSEP = " + producto.MTO_RMGTOSEP + ", \n " +
                                    "MTO_RMGTOSEPRV = " + producto.MTO_RMGTOSEPRV + ", \n	" +
                                    "MTO_VALREAJUSTEMEN = (CASE WHEN(cod_tipreajuste = 2) THEN 0.16515813 ELSE 0 END), \n" +
                                    "MTO_VALREAJUSTETRI = (CASE WHEN(cod_tipreajuste = 2) THEN 0.49629316 ELSE 0 END), \n" +
                                    "MTO_VALPREPENTMP = CASE WHEN(" + producto.MTO_RENTATMPAFP + " = 0) THEN 0  \n" +
                                    "ELSE ROUND((" + producto.MTO_CTAINDAFP + " / " + producto.MTO_RENTATMPAFP + "), 2) END \n" +
                                    "WHERE NUM_CORRELATIVO = " + producto.NUM_CORRELATIVO + " AND NUM_OPERACION = '" + producto.NUM_COTESTUDIO + "';";
                    }
                    else
                    {
                        query = "UPDATE PT_TMAE_DETCOTIZACION " +
                                "SET COD_RECHAZO = '" + producto.Cod_Rechazo + "', \n " +
                                "ERR_DESCRIP = '" + producto.Mensaje + "', \n" +
                                "FEC_CALCULO = '" + producto.FecCal + "', \n" +
                                "PRC_CORCOM = 0, \n" +
                                "--IND_FILTROCOTIZA = 'N', \n " +
                                "COD_ESTCOT = 'S' \n" +
                                "WHERE NUM_CORRELATIVO = " + producto.NUM_CORRELATIVO + " AND NUM_OPERACION = '" + producto.NUM_COTESTUDIO + "';";
                    }

                    queryGlobal += query + " \n";
                }
            });
        }



        public Response asesores()
        {
            CalcularAsignacionIntermediarioRepository _calcular = new CalcularAsignacionIntermediarioRepository();
            Response resultado = new Response();
            try
            {
                var res = _calcular.asesores();
                resultado.Object = res;
                resultado.IsOk = true;
            }
            catch (Exception ex)
            {
                resultado.Message = ex.ToString();
                resultado.IsOk = false;
            }

            return resultado;
        }

        public string datosCabeza(int numArch)
        {
            CalcularAsignacionIntermediarioRepository _calcular = new CalcularAsignacionIntermediarioRepository();
            Response resultado = new Response();
            try
            {
                return _calcular.datosCabeza(numArch);
            }
            catch (Exception ex)
            {
                return ex.Message.ToString();
            }
        }

        public Response obtenerDatos(int numArch)
        {
            CalcularAsignacionIntermediarioRepository _calcular = new CalcularAsignacionIntermediarioRepository();
            Response resultado = new Response();
            try
            {
                var res = _calcular.obtenerDatos(numArch);
                resultado.Object = res;
                resultado.IsOk = true;
            }
            catch (Exception ex)
            {
                resultado.Message = ex.ToString();
                resultado.IsOk = false;
            }

            return resultado;
        }

        public async Task<string> wsCalcularAsignacionIntermediario(List<SolicitudesCotizacion> informacion, int Num_Archivo, string usuario)
        {

            List<AsignacionIntermediario> ListdatosAI = new List<AsignacionIntermediario>();
            CalcularAsignacionIntermediarioRepository _CalcularAsignacionIntermediarioRepository = new CalcularAsignacionIntermediarioRepository();
            PruebasRutinaOficiales _pruebasRutinaOficiales = new PruebasRutinaOficiales();
            RutinaOficialesRepository _rutinaOficialesRepository = new RutinaOficialesRepository();
            try
            {
                Response res = new Response();

                #region validaciones SEACSA 
                //DateTime fechaActual = DateTime.Now;
                //DateTime fechaActualMesYear = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM"));
                ////Valida que exista IPC a la Fecha de Cálculo
                //if (CalcularAsignacionIntermediarioRepository.ValidaIPC(fechaActualMesYear.AddMonths(-1)) == false)
                //{
                //    return "No existe IPC a la Fecha de Cálculo";
                //}
                ////Valida que exista Tasa de Mercado a la Fecha de Cálculo
                //if (CalcularAsignacionIntermediarioRepository.ValidaTM(fechaActualMesYear) == false)
                //{
                //    return "No existe Tasa de Mercado a la Fecha de Cálculo, para US y/o NS.";
                //}

                ////Verifica si existe el Departamento Estándar y sus parámetros
                //if (CalcularAsignacionIntermediarioRepository.ValidaDeptoEstandar("C", fechaActual) == false)
                //{
                //    return "No se encuentra definido el Departamento Estándar o No existen parámetros para la fecha";
                //}
                ////Permite obtener el Indicador de si se cálculan todas las modalidades cuando exista a lo menos una en Soles
                //string fgBuscarIndCalcularModSoles = CalcularAsignacionIntermediarioRepository.fgBuscarIndCalcularModSoles();

                //List<string> Reajuste = new List<string>();
                ////Obtiene la Tasa de Reajuste en Dólares
                //Reajuste = CalcularAsignacionIntermediarioRepository.BuscarValReajuste(fechaActual, "US");
                //if (Reajuste.Count == 0)
                //{
                //    return "No se ha ingresado el % Reajuste Fijo para Dólares.";
                //}
                //Double Prc_ReajusteUS_Tri = Convert.ToDouble(Reajuste[0]);
                //Double Prc_ReajusteUS_Men = Convert.ToDouble(Reajuste[1]);
                //Double Prc_ReajusteUS_Anu = Convert.ToDouble(Reajuste[2]);
                ////Obtiene la Tasa de Reajuste en Soles
                //Reajuste = CalcularAsignacionIntermediarioRepository.BuscarValReajuste(fechaActual, "NS");
                //if (Reajuste.Count == 0)
                //{
                //    return "No se ha ingresado el % Reajuste Fijo para Soles.";
                //}
                //Double Prc_ReajusteNS_Tri = Convert.ToDouble(Reajuste[0]);
                //Double Prc_ReajusteNS_Men = Convert.ToDouble(Reajuste[1]);
                //Double Prc_ReajusteNS_Anu = Convert.ToDouble(Reajuste[2]);
                #endregion
                List<List<beResultados>> rutinaOficiales = new List<List<beResultados>>(); //Lista de resultados de rutina
                if (informacion.Count > 0)
                { //Si hay Nros de Cotizaciones a calcular
                    List<beMortalidadDin> LisTabDin = new List<beMortalidadDin>();
                    List<beMortalidadDinDet> LisTabMD = new List<beMortalidadDinDet>();
                    List<beDatosModalidad> VarMTGS = new List<beDatosModalidad>();
                    List<beDatosModalidad> VarAFP = new List<beDatosModalidad>();
                    List<beDatosModalidad> VarREG = new List<beDatosModalidad>();
                    List<bePorcenLegales> LisTabPL = new List<bePorcenLegales>();
                    List<beTasaAnclaje> ListaTA = new List<beTasaAnclaje>();
                    List<beCPK> ListaCPK = new List<beCPK>();
                    List<beRentabilidad> ListaRen = new List<beRentabilidad>();
                    List<beTasasPromedio> ListaTasProm = new List<beTasasPromedio>();
                    List<beCurvaTasas> ListaCurvaTasas = new List<beCurvaTasas>();
                    LisTabDin = _rutinaOficialesRepository.ConsultaTablaMortalidadDinamicas("");
                    LisTabMD = _rutinaOficialesRepository.ConsultaDetTablaMortalidadDin();
                    VarMTGS = _rutinaOficialesRepository.ConsultaGastoSepelioMes("");
                    VarAFP = _rutinaOficialesRepository.ConsultaRentabilidadAfp("");
                    VarREG = _rutinaOficialesRepository.ConsultaRegionTasas(0);
                    LisTabPL = _rutinaOficialesRepository.ConsultaPorcentaje("");
                    ListaTA = _rutinaOficialesRepository.ConsultaTasaAnclaje("");
                    ListaCPK = _rutinaOficialesRepository.ConsultaCPKS("");
                    ListaRen = _rutinaOficialesRepository.ConsultaRentabilidad("");
                    ListaTasProm = _rutinaOficialesRepository.ConsultaTasasPromedio("");
                    ListaCurvaTasas = _rutinaOficialesRepository.ConsultaCurvaTasas("");

                    for (int i = 0; i < informacion.Count; i++)
                    {
                        _CalcularAsignacionIntermediarioRepository.updateAsesor(informacion[i].numAgente.ToString(), informacion[i].intNumOpe.ToString(), Convert.ToDouble(informacion[i].strMtoCIC));
                        Console.WriteLine("Memory used before collection:       {0:N0}",
                        GC.GetTotalMemory(false));

                        rutinaOficiales.Add(await _pruebasRutinaOficiales.RutinaOficiales(informacion[i].intNumOpe, informacion[i].strNumCot, Num_Archivo, LisTabDin, LisTabMD, VarMTGS, VarAFP, VarREG, LisTabPL, ListaTA, ListaCPK, ListaRen, ListaTasProm, ListaCurvaTasas, null, "WS", informacion[i].numAgente));

                        // Collect all generations of memory.
                        GC.Collect();
                        Console.WriteLine("Memory used after full collection:   {0:N0}",
                                          GC.GetTotalMemory(true));
                        GCSettings.LargeObjectHeapCompactionMode = GCLargeObjectHeapCompactionMode.CompactOnce;
                        GC.Collect(2, GCCollectionMode.Forced, true, true);
                    }
                    //List<Task> tareas = new List<Task>();
                    //foreach (var item in rutinaOficiales)
                    //{
                    //    var task = GenerarScript(item);
                    //    tareas.Add(task);
                    //    GC.Collect();
                    //    //GC.WaitForPendingFinalizers();
                    //    Console.WriteLine("Memory used after full collection:   {0:N0}",
                    //    GC.GetTotalMemory(true));
                    //}

                    //await Task.WhenAll(tareas);
                    string query = String.Empty;
                    foreach (var item in rutinaOficiales)
                    {
                        //var task = GenerarScript(item);
                        //tareas.Add(task);
                        ////GC.Collect();
                        //////GC.WaitForPendingFinalizers();
                        ////Console.WriteLine("Memory used after full collection:   {0:N0}",
                        ////GC.GetTotalMemory(true));
                        foreach (var producto in item)
                        {
                            if (producto.Cod_Rechazo == "0")
                            {
                                query = "UPDATE PT_TMAE_DETCOTIZACION " +
                                            "SET " +
                                            "MTO_AJUSTEIPC = " + producto.MTO_AJUSTEIPC + ", \n" +
                                            "MTO_CTAINDAFP = " + producto.MTO_CTAINDAFP + ", \n" +
                                            "MTO_PENSION = " + producto.MTO_PENSION + ", \n" +
                                            "MTO_PRIUNIDIF = " + producto.MTO_PRIUNIDIF + ", \n" +
                                            "MTO_RENTATMPAFP = " + producto.MTO_RENTATMPAFP + ", \n" +
                                            "MTO_RESMAT = " + producto.MTO_RESMAT + ", \n" +
                                            "PRC_PERCON = " + producto.PRC_PERCON + ", \n" +
                                            "PRC_TASATCE = " + producto.PRC_TASATCE + ", \n" +
                                            "PRC_TASATIR = " + producto.PRC_TASATIR + ", \n" +
                                            "PRC_TIRINI = " + producto.PRC_TASATIR + ", \n" +
                                            "PRC_TASAVTA = " + producto.PRC_TASAVTA + ", \n" +
                                            "FEC_CALCULO = '" + producto.FecCal + "', \n" +
                                            "PRC_CORCOM = 0, \n" +
                                            "--IND_SISCO = 0, \n" +
                                            "--IND_FILTROCOTIZA = 'S', \n" +
                                            "COD_ESTCOT = 'S', \n" +
                                            "MTO_PENANUAL = " + producto.MTO_PENANUAL + ", \n " +
                                            "MTO_PENSIONGAR = " + producto.MTO_PENSIONGAR + ", \n " +
                                            "MTO_PRIUNISIM = " + producto.MTO_PRIUNISIM + ", \n " +
                                            "MTO_RMGTOSEP = " + producto.MTO_RMGTOSEP + ", \n " +
                                            "MTO_RMGTOSEPRV = " + producto.MTO_RMGTOSEPRV + ", \n	" +
                                            "MTO_VALREAJUSTEMEN = (CASE WHEN(cod_tipreajuste = 2) THEN 0.16515813 ELSE 0 END), \n" +
                                            "MTO_VALREAJUSTETRI = (CASE WHEN(cod_tipreajuste = 2) THEN 0.49629316 ELSE 0 END), \n" +
                                            "MTO_VALPREPENTMP = CASE WHEN(" + producto.MTO_RENTATMPAFP + " = 0) THEN 0  \n" +
                                            "ELSE ROUND((" + producto.MTO_CTAINDAFP + " / " + producto.MTO_RENTATMPAFP + "), 2) END \n" +
                                            "WHERE NUM_CORRELATIVO = " + producto.NUM_CORRELATIVO + " AND NUM_OPERACION = '" + producto.NUM_COTESTUDIO + "';";
                            }
                            else
                            {
                                query = "UPDATE PT_TMAE_DETCOTIZACION " +
                                        "SET COD_RECHAZO = '" + producto.Cod_Rechazo + "', \n " +
                                        "ERR_DESCRIP = '" + producto.Mensaje + "', \n" +
                                        "FEC_CALCULO = '" + producto.FecCal + "', \n" +
                                        "PRC_CORCOM = 0, \n" +
                                        "--IND_FILTROCOTIZA = 'N', \n " +
                                        "COD_ESTCOT = 'S' \n" +
                                        "WHERE NUM_CORRELATIVO = " + producto.NUM_CORRELATIVO + " AND NUM_OPERACION = '" + producto.NUM_COTESTUDIO + "';";
                            }

                            queryGlobal += query + " \n";
                        }

                    }
                    
                    _CalcularAsignacionIntermediarioRepository.RegistroRutinaOficiales(queryGlobal);
                    _log.Info("TERMINO DE GUARDAR LA INFORMACIÓN");
                }
            }
            catch (Exception)
            {
                throw;
            }
            return "";
        }

    }

}
