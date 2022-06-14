using Estudio.Repository.Core.Domain;
using Estudio.Repository.Helpers;
using Estudio.Repository.Persistence.Repositories;
using System;
using System.Collections.Generic;
using log4net;
using log4net.Config;
using System.Reflection;

namespace Estudio.Logic
{
    public class ManCurvaTasasResLogic
    {
        //Globales
        private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        ReservasRepository _reservasRepository = new ReservasRepository();
        ManCurvaTasasResRepository _manCurvaTasasResRepository = new ManCurvaTasasResRepository();

        /// <summary>
        /// Cargar e insertar Curva de Tasas de Reservas.
        /// José Hernández Alvarado.
        /// 20-12-2019
        /// </summary>
        /// <param name="pQuerys">Querys generados para inserción en base de datos.</param>
        /// <returns>Mensaje de respuesta de inserción correcta o erronea.</returns>
        public Response CargarCurvaTasasReservas(string pQuerys)
        {
            XmlConfigurator.Configure();
            Response _respuesta = new Response();
            bool scriptsUpdate = true;

            try
            {
                _log.Info("INICIA PROCESO DE CURVA DE TASAS DE RESERVAS.");
                _log.Info("Se obtendrá la cadena de conexión directa...");
                string strConexionDirecta = _reservasRepository.cadena_conexion(); //Se obtiene cadena de conexión directa a SeguroRV.
                _log.Info("Se obtuvo la cadena de conexión correctamente.");

                _log.Info("Se procederá a insertar la información en base de datos.");
                scriptsUpdate = _manCurvaTasasResRepository.ActualizarCurvaTasasReservas(pQuerys, strConexionDirecta); //Se ejecutan los querys de inserción en base de datos.

                if (scriptsUpdate == true)
                {
                    _log.Info("CARGA DE CURVA DE TASAS DE RESERVAS EXITOSA.");

                    _respuesta.IsOk = true;
                    _respuesta.Message = "Carga terminada con éxito.";
                }
                else
                {
                    _log.Info("Error al ejecutar scripts de carga masiva.");

                    _respuesta.IsOk = false;
                    _respuesta.Message = "Error en ejecución de scripts para carga de excel.";
                }

                return _respuesta;
            }
            catch (Exception ex)
            {
                _log.Info("ERROR EN CARGA DE CURVA DE TASAS DE RESERVAS.");
                _log.Info("Error al obtener cadena de conexión: " + ex.Message);
                _respuesta.IsOk = false;
                _respuesta.Message = ex.Message;
                return _respuesta;
            }
        }

        /// <summary>
        /// Consultar información de Curva de Tasas de Reservas con fecha ingresada.
        /// José Hernández Alvarado.
        /// 20-12-2019
        /// </summary>
        /// <param name="pFecha">Fecha ingresada en pantalla para consultar Tasas de Reservas.</param>
        /// <returns>Lista de datos consultada y mensaje de respuesta.</returns>
        public Response CargarTablaCurvaTasasReservas(string pFecha)
        {
            XmlConfigurator.Configure();
            Response _respuesta = new Response();
            List<ManCurvaTasasRes> lstCurvaTasasReservas = new List<ManCurvaTasasRes>();

            string strQueryConsultaTasas = "SELECT DISTINCT T.NUM_MES, T.MTO_VALOR, M.COD_SCOMP FROM PR_TVAL_CURVA_TASAS T " +
                                           "INNER JOIN MA_TPAR_MONEDATIPOREAJU M ON T.COD_MONEDA = M.COD_MONEDA AND T.COD_TIPREAJUSTE = M.COD_TIPREAJUSTE " +
                                           "WHERE FEC_INIVIG = '" + pFecha + "' AND FEC_TERVIG = '99991231' ORDER BY T.NUM_MES ASC ";

            try
            {
                _log.Info("Se consultará información para carga de tabla de Curva de Tasas de Reservas.");
                lstCurvaTasasReservas = _manCurvaTasasResRepository.CargaTableRepository(strQueryConsultaTasas); //Se ejecuta query de consulta de información de Tasas de Reservas.
                _log.Info("Se ejecutó la consulta correctamente.");

                if (lstCurvaTasasReservas.Count != 0)
                {
                    _respuesta.Object = lstCurvaTasasReservas;
                    _respuesta.IsOk = true;
                    return _respuesta;
                }

                _log.Info("No existe información en base de datos con la fecha ingresada.");
                _respuesta.Object = lstCurvaTasasReservas;
                _respuesta.IsOk = false;
                _respuesta.Message = "No existen datos con la fecha ingresada.";

                return _respuesta;
            }
            catch (Exception ex)
            {
                _log.Info("Error al consultar información de tasas en base de datos: " + ex.Message);
                _respuesta.IsOk = false;
                _respuesta.Message = "Error al consultar: " + ex.Message;
                return _respuesta;
            }
        }
    }
}
