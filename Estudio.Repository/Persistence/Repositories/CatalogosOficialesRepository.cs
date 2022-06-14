using Estudio.Repository.Core.Domain;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Estudio.Repository.Persistence.Repositories
{
    public class CatalogosOficialesRepository
    {
        /// <summary>
        /// Osvaldo Valdez Carrillo
        /// 2018-08-08
        /// </summary>
        /// <returns> Regresa los tipo de Moneda </returns>
        public List<Moneda> TiposMoneda()
        {
            try
            {
                var parameters = new List<SqlParameter>();
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pClave", SqlDbType.VarChar, "TIPOSMONEDA", ParameterDirection.Input));
                return VCEDBContext<Moneda>.CallStoreProcedure(StoredProcedures.CO_CatalogosSeguros, parameters, x => new Moneda
                {
                    ClaveMoneda = x.GetString(0) + "#" + x.GetString(1),//cod_moneda+cod_tipreajuste
                    Elemento = x.GetString(2) + " - " + x.GetString(3)//cod_scomp+gls_descripcion
                }).ToList();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
        /// <summary>
        /// José Hernández Alvarado.
        /// 2018-08-21
        /// </summary>
        /// <returns>Retorna  los valores de moneda para el combo</returns>
        public List<Moneda> cmbMoneda()
        {
            try
            {
                var parameters = new List<SqlParameter>();
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pClave", SqlDbType.VarChar, "CMBMONEDA", ParameterDirection.Input));

                return VCEDBContext<Moneda>.CallStoreProcedure(StoredProcedures.CO_CatalogosSeguros, parameters, x => new Moneda
                {
                    ClaveMoneda = x.GetString(0),
                    Elemento = x.GetString(0) + " - " + x.GetString(1)//cod_scomp+gls_descripcion
                }).ToList();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
        /// <summary>
        /// Osvaldo Valdez Carrillo
        /// 2018-08-22
        /// </summary>
        /// <returns> Regresa los valores para los combos de la vista </returns>
        public List<Moneda> cmbValor()
        {
            try
            {
                var parameters = new List<SqlParameter>();
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pClave", SqlDbType.VarChar, "CMBVALOR", ParameterDirection.Input));

                return VCEDBContext<Moneda>.CallStoreProcedure(StoredProcedures.CO_CatalogosSeguros, parameters, x => new Moneda
                {
                    ClaveMoneda = x.GetString(0),
                    Elemento = x.GetString(0) + " - " + x.GetString(1)
                }).ToList();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

    }
}
