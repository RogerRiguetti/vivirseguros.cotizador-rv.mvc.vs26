using System.Collections.Generic;
using System.Linq;
using Estudio.Repository.Core.Domain;
using System.Reflection;
using log4net;
using System;
using log4net.Config;

namespace Estudio.Repository.Persistence.Repositories
{
    public class CargaActivosRepository
    {
        //Globales
        private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Ejecutar querys de inserción de Carga de Activos de Reservas.
        /// José Hernández Alvarado.
        /// 20-12-2019
        /// </summary>
        /// <param name="pQuerys">Querys de inserción en base de datos en SQLServer.</param>
        /// <param name="pCadenaConexion">Cadena de conexión directa a base de datos de SeguroRV.</param>
        public bool ActualizarCargaActivos(string pQuerys, string pCadenaConexion)
        {
            XmlConfigurator.Configure();
            bool ejecucionScripts = true;
            try
            {
                VCEDBContext<CargaActivos>.CallSelectStatementConection(pCadenaConexion, pQuerys, x => new CargaActivos
                { }).FirstOrDefault();

                ejecucionScripts = true;
            }
            catch (Exception ex)
            {
                _log.Info("Error al ejecutar los scripts para Carga Masiva de Activos: " + ex.Message);
                ejecucionScripts = false;
            }
            return ejecucionScripts;
        }

        /// <summary>
        /// Ejecutar query de consulta de Carga de Activos de Reservas.
        /// José Hernández Alvarado.
        /// 20-12-2019
        /// </summary>
        /// <param name="pQueryConsulta">Query de consulta de base de datos en SQLServer.</param>
        /// <returns>Lista de datos consultada.</returns>
        public List<CargaActivos> CargaTableRepository(string pQueryConsulta)
        {
            return SRVDBContext<CargaActivos>.CallSelectStatement(pQueryConsulta, x => new CargaActivos
            {
                Num_Mes = x.GetInt32(0),
                Mto_valor = x.GetDecimal(1),
                COD_SCOMP = x.GetString(2)
            }).ToList();
        }

    }
}
