using System.Collections.Generic;
using System.Linq;
using Estudio.Repository.Core.Domain;

namespace Estudio.Repository.Persistence.Repositories
{
    public class PeriodosRepository
    {
        //Globales


        /// <summary>
        /// Consultar periodos de la base de datos para mostrarlos en pantalla.
        /// José Hernándes Alvarado.
        /// 05-12-2019
        /// </summary>
        /// <param name="pQueryPeriodos">Query para consultar periodos existentes.</param>
        /// <returns>Lista de periodos consultados en Base de Datos.</returns>
        public List<Periodos> ConsultarPeriodos(string pQueryPeriodos)
        {
            return SRVDBContext<Periodos>.CallSelectStatement(pQueryPeriodos, x => new Periodos
            {
                Fec_Calculo = (x.GetString(0).Substring(0, 4) + "/" + x.GetString(0).Substring(4, 2) + "/" + x.GetString(0).Substring(6, 2)),
                strFec_Calculo = x.GetString(0),
                Cod_EstPeriodo = x.GetString(1),
                Num_TotalPolcar = x.GetInt32(2),
                Num_TotalBencar = x.GetInt32(3)
            }).ToList();
        }

        /// <summary>
        /// Ejecutar querys de Base de Datos para consultas de periodos de Reservas.
        /// José Hernández Alvarado.
        /// 10-12-2019
        /// </summary>
        /// <param name="pQuerysConsultas">Variable de texto con scripts a ejecutar en Base de Datos.</param>
        /// <returns>Objeto de Periodos con información encontrada o null en caso contrario.</returns>
        public Periodos RegistrarNuevoPeriodo(string pQuerysConsultas)
        {
            return SRVDBContext<Periodos>.CallSelectStatement(pQuerysConsultas, x => new Periodos
            {
                Fec_Calculo = (x.GetString(0).Substring(0, 4) + "/" + x.GetString(0).Substring(4, 2) + "/" + x.GetString(0).Substring(6, 2)),
                Cod_EstPeriodo = x.GetString(1),
                Num_TotalPolcar = x.GetInt32(2),
                Num_TotalBencar = x.GetInt32(3)
            }).FirstOrDefault();
        }

        /// <summary>
        /// Consultar periodo con fecha específica.
        /// José Hernández Alvarado.
        /// 10-12-2019
        /// </summary>
        /// <param name="pQueryPeriodo">Script de ejecución en SQLServer para consulta.</param>
        /// <returns>Objeto con registro de datos consultados.</returns>
        public Periodos ConsultaPeriodo(string pQueryPeriodo)
        {
            return SRVDBContext<Periodos>.CallSelectStatement(pQueryPeriodo, x => new Periodos
            {
                Fec_Calculo = (x.GetString(0).Substring(0, 4) + "/" + x.GetString(0).Substring(4, 2) + "/" + x.GetString(0).Substring(6, 2)),
                strFec_Calculo = x.GetString(0),
                Cod_EstPeriodo = x.GetString(1),
                Num_TotalPolcar = x.GetInt32(2),
                Num_TotalBencar = x.GetInt32(3)
            }).FirstOrDefault();
        }

        /// <summary>
        /// Ejecutar Script en SQLServer.
        /// José Hernández Alvarado.
        /// 10-12-2019
        /// </summary>
        /// <param name="pQuery">Script a ejecutar en Base de Datos.</param>
        public void EjecutarScript(string pQuery)
        {
            SRVDBContext<Periodos>.CallSelectStatement(pQuery, x => new Periodos
            { }).FirstOrDefault();
        }

    }
}
