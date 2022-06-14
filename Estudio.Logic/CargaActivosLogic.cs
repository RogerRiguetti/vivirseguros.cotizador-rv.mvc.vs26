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
    public class CargaActivosLogic
    {
        //Globales
        private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        ReservasRepository _reservasRepository = new ReservasRepository();
        CargaActivosRepository _cargaActivosRepository = new CargaActivosRepository();

        /// <summary>
        /// Cargar e insertar Carga de Activos de Reservas.
        /// José Hernández Alvarado.
        /// 20-12-2019
        /// </summary>
        /// <param name="pQuerys">Querys generados para inserción en base de datos.</param>
        /// <returns>Mensaje de respuesta de inserción correcta o erronea.</returns>
        public Response CargarCargaActivos(string pQuerys)
        {
            XmlConfigurator.Configure();
            Response _respuesta = new Response();
            bool scriptsUpdate = false;

            try
            {
                _log.Info("INICIA PROCESO DE CARGA DE ACTIVOS DE RESERVAS.");
                _log.Info("Se obtendrá la cadena de conexión directa...");
                string strConexionDirecta = _reservasRepository.cadena_conexion(); //Se obtiene cadena de conexión directa a SeguroRV.
                _log.Info("Se obtuvo la cadena de conexión correctamente.");

                _log.Info("Se procederá a insertar la información en base de datos.");
                scriptsUpdate = _cargaActivosRepository.ActualizarCargaActivos(pQuerys, strConexionDirecta); //Se ejecutan los querys de inserción en base de datos.

                if (scriptsUpdate == true)
                {
                    _log.Info("CARGA DE ACTIVOS DE RESERVAS EXITOSA.");

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
                _log.Info("ERROR EN CARGA DE ACTIVOS DE RESERVAS.");
                _log.Info("Error al obtener cadena de conexión: " + ex.Message);
                _respuesta.IsOk = false;
                _respuesta.Message = ex.Message;
                return _respuesta;
            }
        }

        /// <summary>
        /// Consultar información de Carga de Activos de Reservas con fecha ingresada.
        /// José Hernández Alvarado.
        /// 20-12-2019
        /// </summary>
        /// <param name="pFecha">Fecha ingresada en pantalla para consultar Carga de Activos de Reservas.</param>
        /// <returns>Lista de datos consultada y mensaje de respuesta.</returns>
        public Response CargarTablaCargaActivosReservas(string pFecha)
        {
            XmlConfigurator.Configure();
            Response _respuesta = new Response();
            List<CargaActivos> lstCargaActivos = new List<CargaActivos>();

            string strQueryConsultaCargaActivos = "SELECT DISTINCT T.NUM_MES, T.MTO_VALOR, M.COD_SCOMP FROM PR_TMAE_ACTIVOS_ASA T " +
                                                  "INNER JOIN MA_TPAR_MONEDATIPOREAJU M ON T.COD_MONEDA = M.COD_MONEDA AND T.COD_TIPREAJUSTE = M.COD_TIPREAJUSTE " +
                                                  "WHERE FEC_INIVIG = '" + pFecha + "' AND FEC_TERVIG = '99991231' ORDER BY T.NUM_MES ASC ";

            try
            {
                _log.Info("Se consultará información para carga de tabla de Carga de Activos de Reservas.");
                lstCargaActivos = _cargaActivosRepository.CargaTableRepository(strQueryConsultaCargaActivos); //Se ejecuta query de consulta de información de Carga de Activos de Reservas.
                _log.Info("Se ejecutó la consulta correctamente.");

                if (lstCargaActivos.Count != 0)
                {
                    _respuesta.Object = lstCargaActivos;
                    _respuesta.IsOk = true;
                    return _respuesta;
                }

                _log.Info("No existe información en base de datos con la fecha ingresada.");
                _respuesta.Object = lstCargaActivos;
                _respuesta.IsOk = false;
                _respuesta.Message = "No existen datos con la fecha ingresada.";

                return _respuesta;
            }
            catch (Exception ex)
            {
                _log.Info("Error al consultar información de Carga de Activos en base de datos: " + ex.Message);
                _respuesta.IsOk = false;
                _respuesta.Message = "Error al consultar: " + ex.Message;
                return _respuesta;
            }
        }
    }
}
