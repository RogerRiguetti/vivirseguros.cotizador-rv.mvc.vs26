using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Estudio.Repository.Core.Domain;
using System.Data.SqlClient;

namespace Estudio.Repository.Persistence.Repositories
{
    public class ParametrosRepository
    {
        /// <summary>
        /// Antonio Quezada
        /// 2018-03-27
        /// Regresa la información del parámetro consultado
        /// </summary>
        /// <param name="claveParametro"> Clave del parámetro a consultar </param>
        /// <returns> Regresa un objeto de la clase Parametro que contiene la información del parámetro </returns>

        public Parametro ConsultaParametro(string claveParametro)
        {
            var parameters = new List<SqlParameter>();
            parameters.Add(VCEDBContext<SqlParameter>.AddParams("@pBandera", System.Data.SqlDbType.Char, 'D', System.Data.ParameterDirection.Input));
            parameters.Add(VCEDBContext<SqlParameter>.AddParams("@pClaveParametro", System.Data.SqlDbType.VarChar, claveParametro, System.Data.ParameterDirection.Input));

            return VCEDBContext<Parametro>.CallStoreProcedure(StoredProcedures.VCE_CatalogoParametros, parameters, x => new Parametro
            {
                IdParametro = x.GetInt32(0),
                ClaveParametro = x.GetString(1),
                Elemento = x.GetString(2),
                DescripcionParametro = x.GetString(3),
                Estado = x.GetByte(4)
            }).FirstOrDefault();
        }
    }
}
