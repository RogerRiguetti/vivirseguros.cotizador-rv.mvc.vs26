using System;
using System.Collections.Generic;
using Estudio.Repository.Core.Domain;
using Estudio.Repository.Persistence.Repositories;
using Estudio.Repository.Helpers;
using log4net;
using System.Reflection;
using log4net.Config;
using System.Globalization;

namespace Estudio.Logic
{
    public class PeriodosLogic
    {
        PeriodosRepository _PeriodosRepository = new PeriodosRepository();
        private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Asignar y enviar query para consulta de periodos existentes en Base de Datos.
        /// José Hernández Alvarado.
        /// 06-12-2019
        /// </summary>
        /// <returns>Lista y mensaje de respuesta erroneo o satisfactorio.</returns>
        public Response ConsultarPeriodos()
        {
            XmlConfigurator.Configure();
            Response _response = new Response();
            List<Periodos> lstPeriodos = new List<Periodos>();
            string strQueryPeriodos = "SELECT FEC_CALCULO, COD_ESTPERIODO, NUM_TOTALPOLCAR, NUM_TOTALBENCAR \n" +
                                      "FROM PR_TMAE_PROCALDEF \n" +
                                      "WHERE COD_CLIENTE = 1 \n" +
                                      "AND FEC_CALCULO < (SELECT FEC_CALCULO FROM PR_TMAE_PROCALDEF WHERE COD_ESTPERIODO = 'A') \n" +
                                      "OR FEC_CALCULO = (SELECT FEC_CALCULO FROM PR_TMAE_PROCALDEF WHERE COD_ESTPERIODO = 'A') \n" + 
                                      "ORDER BY FEC_CALCULO DESC";

            //string strQueryPeriodos = "SELECT FEC_CALCULO, COD_ESTPERIODO, NUM_TOTALPOLCAR, NUM_TOTALBENCAR FROM PR_TMAE_PROCALDEF WHERE COD_CLIENTE = 1 ORDER BY FEC_CALCULO DESC";
            
            try
            {
                _log.Info("CONSULTA DE PERIODOS: se consultarán los periodos para mostrarlos en pantalla.");
                lstPeriodos = _PeriodosRepository.ConsultarPeriodos(strQueryPeriodos);
                _response.Object = lstPeriodos;

                if (lstPeriodos != null)
                {
                    _log.Info("CONSULTA DE PERIODOS: consulta de periodos exitosa.");
                    _response.IsOk = true;
                    _response.Message = "Consulta Exitosa";
                }
                else
                {
                    _log.Info("ERROR: Lista de periodos vacía.");
                    _response.IsOk = false;
                    _response.Message = "Lista de periodos vacía, favor de verificar.";
                }
            }
            catch (Exception ex)
            {
                _log.Info("Error en consulta de periodos: " + ex.Message);
                _response.IsOk = false;
                _response.Object = lstPeriodos;
                _response.Message = "Error al consultar periodos. Operación cancelada.";
            }
            
            return _response;
        }

        /// <summary>
        /// Abrir nuevo periodo para Reservas.
        /// José Hernández Alvarado.
        /// 10-12-2019
        /// </summary>
        /// <param name="pFechaPeriodoNuevo">Fecha de periodo a abrir.</param>
        /// <param name="pFechaProceso">Fecha en que se realizó el proceso.</param>
        /// /// <param name="pStrUsuario">Nombre del usuario en el sistema.</param>
        /// <returns>Respuesta de confirmación o erronea.</returns>
        public Response AbrirNuevoPeriodo(string pFechaPeriodoNuevo, string pFechaProceso, string pStrUsuario)
        {
            XmlConfigurator.Configure();
            Response respuesta = new Response();
            Periodos nuevoPeriodo = new Periodos();
            Periodos periodoAbiertoActual = new Periodos();
            string strQueryNuevoPeriodo = "";
            DateTime dtmFechaPeriodoNuevo = new DateTime();
            DateTime dtmFechaMesActual = new DateTime();
            DateTime dtmFechaPeriodoAbierto = new DateTime();

            try
            {
                #region Validaciones de fechas para apertura de periodo.
                strQueryNuevoPeriodo = "SELECT FEC_CALCULO, COD_ESTPERIODO, NUM_TOTALPOLCAR, NUM_TOTALBENCAR FROM PR_TMAE_PROCALDEF WHERE COD_CLIENTE = 1 AND COD_ESTPERIODO = 'A' ";
                periodoAbiertoActual = _PeriodosRepository.ConsultaPeriodo(strQueryNuevoPeriodo);

                dtmFechaPeriodoNuevo = DateTime.ParseExact(pFechaPeriodoNuevo.Substring(0, 6) + "01", "yyyyMMdd", CultureInfo.InvariantCulture);
                dtmFechaMesActual = DateTime.ParseExact(pFechaProceso.Substring(0, 6) + "01", "yyyyMMdd", CultureInfo.InvariantCulture);
                dtmFechaPeriodoAbierto = DateTime.ParseExact(periodoAbiertoActual.strFec_Calculo.Substring(0, 6) + "01", "yyyyMMdd", CultureInfo.InvariantCulture);


                //Se valida que el periodo nuevo por abrir sea anterior al mes de la fecha actual el sistema.
                if (dtmFechaPeriodoNuevo == dtmFechaMesActual || dtmFechaPeriodoNuevo > dtmFechaMesActual)
                {
                    respuesta.IsOk = false;
                    respuesta.Message = "No es posible abrir el periodo todavía, ya que se encuentra dentro del mes de la fecha actual o es posterior al mismo.";
                    _log.Info("ERROR: aún no puede abrir este periodo, ya que se encuentra dentro de las fechas actuales o es posterior a las mismas.");

                    return respuesta;
                }

                //Se valida si el periodo por abrir ya se encuentra cerrado.
                if (dtmFechaPeriodoNuevo < dtmFechaPeriodoAbierto)
                {
                    respuesta.IsOk = false;
                    respuesta.Message = "El periodo ya se encuentra cerrado, favor de usar la opción de reabrir periodo.";
                    _log.Info("ERROR: el periodo ya se encuentra cerrado, usar la opción de reabrir periodo.");

                    return respuesta;
                }

                int diferencia = 0;
                int mesPeriodoAbierto = Convert.ToInt32(dtmFechaPeriodoAbierto.ToString("yyyyMMdd").Substring(4, 2));
                int mesPeriodoNuevo = Convert.ToInt32(dtmFechaPeriodoNuevo.ToString("yyyyMMdd").Substring(4, 2));
                int yearPeriodoAbierto = Convert.ToInt32(dtmFechaPeriodoAbierto.ToString("yyyyMMdd").Substring(0, 4));
                int yearPeriodoNuevo = Convert.ToInt32(dtmFechaPeriodoNuevo.ToString("yyyyMMdd").Substring(0, 4));

                //Validar la diferencia de años entre el periodo abierto y el nuevo solicitado para abrir.
                if ((yearPeriodoNuevo - yearPeriodoAbierto) > 1)
                {
                    respuesta.IsOk = false;
                    respuesta.Message = "No es posible abrir el periodo solicitado.";
                    _log.Info("ERROR: no es posible abrir el periodo solicitado debido a que no es el siguiente mes al abierto actualmente, favor de verificarlo con administrador.");

                    return respuesta;
                }

                //Validación de diferencia entre meses, sino es el mes siguiente al actual abierto no se puede abrir.
                diferencia = yearPeriodoNuevo == yearPeriodoAbierto ? (mesPeriodoNuevo - mesPeriodoAbierto) : ((mesPeriodoNuevo - mesPeriodoAbierto) + 12);

                if (diferencia > 1)
                {
                    respuesta.IsOk = false;
                    respuesta.Message = "No es posible abrir el periodo solicitado.";
                    _log.Info("ERROR: no es posible abrir el periodo solicitado debido a que no es el siguiente mes al abierto actualmente, favor de verificarlo con administrador.");

                    return respuesta;
                }
                #endregion
            }
            catch (Exception ex)
            {
                respuesta.IsOk = false;
                respuesta.Message = "Error en proceso de validación de fechas para apertura de periodo.";
                _log.Info("Eror en validaciones de fechas para la apertura de periodos, favor de acudir al área de sistemas.");
                _log.Info("ERROR: " + ex.Message);

                return respuesta;
            }

            //Validar si el periodo ya existe.
            try
            {
                _log.Info("Se validará si el periodo ya existe.");
                strQueryNuevoPeriodo = "";
                strQueryNuevoPeriodo = "SELECT FEC_CALCULO, COD_ESTPERIODO, NUM_TOTALPOLCAR, NUM_TOTALBENCAR FROM PR_TMAE_PROCALDEF WHERE COD_CLIENTE = 1 AND FEC_CALCULO ='" + pFechaPeriodoNuevo + "'";
                nuevoPeriodo = _PeriodosRepository.ConsultaPeriodo(strQueryNuevoPeriodo);

                if (nuevoPeriodo != null && nuevoPeriodo.Cod_EstPeriodo == "A")
                {
                    respuesta.IsOk = false;
                    respuesta.Message = "El periodo ya se encuentra abierto. Operación cancelada.";
                    _log.Info("La apertura no se puede realizar, el periodo ya se encuentra abierto.");

                    return respuesta;
                }
            }
            catch (Exception ex)
            {
                respuesta.IsOk = false;
                respuesta.Message = "Error al comprobar si el periodo por abrir ya existe. " + ex.Message;
                _log.Info("Error al comprobar si el periodo por abrir ya existe: " + ex.Message);

                return respuesta;
            }

            try
            {
                _log.Info("Se procederá a realizar la apertura del periodo, por favor espere...");
                if (nuevoPeriodo != null)
                {
                    #region Apertura de periodo actualizando el mismo, debido a que ya existe en la tabla.
                    strQueryNuevoPeriodo = "";
                    strQueryNuevoPeriodo = "UPDATE PR_TMAE_PROCALDEF SET COD_ESTPERIODO = 'C' WHERE  COD_CLIENTE = 1 AND COD_ESTPERIODO = 'A' \n" +
                                           "UPDATE PR_TMAE_PROCALDEF SET COD_ESTPERIODO = 'A', " + 
                                                                        "NUM_TOTALPOLCAR = 0 ," + 
                                                                        "NUM_TOTALBENCAR = 0 ," + 
                                                                        "COD_USUARIOMODI = '" + pStrUsuario.ToUpper() + "', " +
                                                                        "FEC_MODI = '" + pFechaProceso + "', " +
                                                                        "HOR_MODI = '" + DateTime.Now.ToString("hhmmss") + "' " +
                                                                        "WHERE  COD_CLIENTE = 1 AND FEC_CALCULO = '" + pFechaPeriodoNuevo + "' \n" +
                                           "SELECT FEC_CALCULO, COD_ESTPERIODO, NUM_TOTALPOLCAR, NUM_TOTALBENCAR FROM PR_TMAE_PROCALDEF " +
                                           "WHERE COD_CLIENTE = 1 AND FEC_CALCULO = '" + pFechaPeriodoNuevo + "'";

                    nuevoPeriodo = new Periodos();
                    nuevoPeriodo = _PeriodosRepository.ConsultaPeriodo(strQueryNuevoPeriodo);

                    if (nuevoPeriodo != null && nuevoPeriodo.Cod_EstPeriodo == "A")
                    {
                        respuesta.IsOk = true;
                        respuesta.Message = "Periodo abierto correctamente.";
                        _log.Info("Periodo abierto correctamente.");
                    }
                    else
                    {
                        respuesta.IsOk = false;
                        respuesta.Message = "El periodo no se abrió correctamente.";
                        _log.Info("El periodo no se abrió correctamente, favor de consultar con el área de sistemas.");
                    }
                    #endregion
                }
                else
                {
                    #region Inserción de periodo si no existe en la tabla de Base de Datos(cuando es completamente nuevo).
                    strQueryNuevoPeriodo = "";
                    strQueryNuevoPeriodo = "UPDATE PR_TMAE_PROCALDEF SET COD_ESTPERIODO = 'C' WHERE  COD_CLIENTE = 1 AND COD_ESTPERIODO = 'A' \n" +
                                           "INSERT PR_TMAE_PROCALDEF(COD_CLIENTE, " +
                                                                    "FEC_CALCULO, " +
                                                                    "NUM_PERIODO, " +
                                                                    "COD_ESTPERIODO, " +
                                                                    "COD_BAS, " +
                                                                    "COD_FIJ, " +
                                                                    "COD_FIN, " +
                                                                    "COD_TASA, " +
                                                                    "IND_TRABAU, " +
                                                                    "IND_TRABAUREA, " +
                                                                    "NUM_CASOSBAU, " +
                                                                    "NUM_CASOSBAUREA, " + 
                                                                    "FEC_PROCESO, " + 
                                                                    "COD_USUARIOCREA, " +
                                                                    "FEC_CREA, " +
                                                                    "HOR_CREA) \n" +
                                                             "VALUES('1', " +
                                                                    "'" + pFechaPeriodoNuevo + "', " +
                                                                    "'852963', " +
                                                                    "'A',  " +
                                                                    "'N', " +
                                                                    "'S', " +
                                                                    "'N', " +
                                                                    "'S', " +
                                                                    "'N', " +
                                                                    "'S', " +
                                                                    "'0', " +
                                                                    "'0', " +
                                                                    "'" + pFechaProceso + "', " +
                                                                    "'" + pStrUsuario.ToUpper() + "', " +
                                                                    "'" + pFechaProceso + "', " +
                                                                    "'" + DateTime.Now.ToString("hhmmss") + "') \n" +
                                           "SELECT FEC_CALCULO, COD_ESTPERIODO, NUM_TOTALPOLCAR, NUM_TOTALBENCAR FROM PR_TMAE_PROCALDEF " +
                                           "WHERE COD_CLIENTE = 1 AND FEC_CALCULO = '" + pFechaPeriodoNuevo + "'";

                    nuevoPeriodo = new Periodos();
                    nuevoPeriodo = _PeriodosRepository.ConsultaPeriodo(strQueryNuevoPeriodo);

                    if (nuevoPeriodo != null && nuevoPeriodo.Cod_EstPeriodo == "A")
                    {
                        respuesta.IsOk = true;
                        respuesta.Message = "Periodo abierto correctamente.";
                    }
                    else
                    {
                        respuesta.IsOk = false;
                        respuesta.Message = "El periodo no se abrió correctamente.";
                    }
                    #endregion
                }

            }
            catch (Exception ex)
            {
                respuesta.IsOk = false;
                respuesta.Message = "Error al realizar apertura de periodo. " + ex.Message;
                _log.Info("Error al realizar apertura de periodo, al ejecutar los scripts: " + ex.Message);
            }

            return respuesta;
        }

        /// <summary>
        /// Reabrir periodo seleccionado en pantalla.
        /// José Hernández Alvarado.
        /// 10-12-2019
        /// </summary>
        /// <param name="pFecha">Fecha de periodo seleccionado en pantalla para reabrir.</param>
        /// <returns>Respueta exitosa o erronea según sea el caso.</returns>
        public Response ReabrirPeriodo(string pFecha)
        {
            XmlConfigurator.Configure();

            string strQueryConsultaPeriodo = "SELECT FEC_CALCULO, COD_ESTPERIODO, NUM_TOTALPOLCAR, NUM_TOTALBENCAR FROM PR_TMAE_PROCALDEF " +
                                             "WHERE COD_CLIENTE = 1 AND FEC_CALCULO = '" + pFecha + "'";

            string strQueryReabrirPeriodo = "UPDATE PR_TMAE_PROCALDEF SET COD_ESTPERIODO = 'C' WHERE  COD_CLIENTE = 1 AND COD_ESTPERIODO = 'A' \n" +
                                            "UPDATE PR_TMAE_PROCALDEF SET COD_ESTPERIODO = 'A' WHERE COD_CLIENTE = 1 AND FEC_CALCULO = '" + pFecha + "' ";

            Response _respuesta = new Response();
            Periodos periodoSeleccionado = new Periodos();

            try
            {
                _log.Info("Se consultará periodo seleccionada para reabrir.");
                periodoSeleccionado = _PeriodosRepository.ConsultaPeriodo(strQueryConsultaPeriodo);
                _log.Info("Se consultó correctamente.");

                if (periodoSeleccionado.Cod_EstPeriodo == "A")
                {
                    _log.Info("ERROR: el periodo que intenta reabrir ya se encuentra abierto");
                    _respuesta.IsOk = false;
                    _respuesta.Message = "El periodo que intenta reabrir ya se encuentra abierto. Operación Cancelada.";

                    return _respuesta;
                }
            }
            catch (Exception ex)
            {
                _log.Info("Error al consultar periodo seleccionado para reabrir: " + ex.Message);
                _respuesta.IsOk = false;
                _respuesta.Message = "Error al consultar periodo seleccionado para reabrir.";

                return _respuesta;
            }

            //Se reabre el periodo y se comprueba que se haya hecho correctamente.
            try
            {
                _log.Info("Se procederá a reabrir el periodo seleccionado");
                _PeriodosRepository.EjecutarScript(strQueryReabrirPeriodo);
            }
            catch (Exception ex)
            {
                _log.Info("Error al reabrir periodo seleccionado: " + ex.Message);
                _respuesta.IsOk = false;
                _respuesta.Message = "Error al reabrir el periodo seleccionado. Opereción Cancelada";

                return _respuesta;
            }

            periodoSeleccionado = _PeriodosRepository.ConsultaPeriodo(strQueryConsultaPeriodo);
            
            if (periodoSeleccionado.Cod_EstPeriodo == "A")
            {
                _log.Info("Periodo reabierto con éxito.");
                _respuesta.IsOk = true;
                _respuesta.Message = "Periodo reabierto con éxito.";
            }

            return _respuesta;
        }
    }
}
