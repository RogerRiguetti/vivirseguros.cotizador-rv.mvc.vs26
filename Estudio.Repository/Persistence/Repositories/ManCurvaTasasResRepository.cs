using System.Collections.Generic;
using System.Linq;
using Estudio.Repository.Core.Domain;
using System.Reflection;
using log4net;
using log4net.Config;
using System;

namespace Estudio.Repository.Persistence.Repositories
{
    public class ManCurvaTasasResRepository
    {
        //Globales
        private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Ejecutar querys de inserción de Curva de Tasas de Reservas.
        /// José Hernández Alvarado.
        /// 20-12-2019
        /// </summary>
        /// <param name="pQuerys">Querys de inserción en base de datos en SQLServer.</param>
        /// <param name="pCadenaConexion">Cadena de conexión directa a base de datos de SeguroRV.</param>
        public bool ActualizarCurvaTasasReservas(string pQuerys, string pCadenaConexion)
        {
            XmlConfigurator.Configure();
            bool ejecucionScripts = true;

            try
            {
                VCEDBContext<AsignacionIntermediario>.CallSelectStatementConection(pCadenaConexion, pQuerys, x => new AsignacionIntermediario
                { }).FirstOrDefault();

                ejecucionScripts = true;
            }
            catch (Exception ex)
            {
                _log.Info("Error en ejecución de script para Carga Masica de Tasas de Reservas: " + ex.Message);
                ejecucionScripts = false;
            }

            return ejecucionScripts;
        }

        /// <summary>
        /// Ejecutar query de consulta de Tasas de Reservas.
        /// José Hernández Alvarado.
        /// 20-12-2019
        /// </summary>
        /// <param name="pQueryConsulta">Query de consulta de base de datos en SQLServer.</param>
        /// <returns>Lista de datos consultada.</returns>
        public List<ManCurvaTasasRes> CargaTableRepository(string pQueryConsulta)
        {
            return SRVDBContext<ManCurvaTasasRes>.CallSelectStatement(pQueryConsulta, x => new ManCurvaTasasRes
            {
                Num_Mes = x.GetInt32(0),
                Mto_valor = x.GetDecimal(1),
                COD_SCOMP = x.GetString(2)
            }).ToList();
        }

    }
}
