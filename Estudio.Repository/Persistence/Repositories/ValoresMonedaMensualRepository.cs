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
    public class ValoresMonedaMensualRepository
    {
        /// <summary>
        /// Osvaldo Valdez Carrillo
        /// 22/08/2018
        /// </summary>
        /// <param name="vlMoneda">valor de la moneda</param>
        /// <param name="cod_tipmon">valor del tipo valor</param>
        /// <returns>retorna la info para la tabla</returns>
        public List<MonedaMensual> CargarTabla(string vlMoneda, string cod_tipmon)
        {
            try
            {
                var parameters = new List<SqlParameter>();
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pClave", SqlDbType.VarChar, "CARGARTABLA", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@vlMoneda", SqlDbType.VarChar, vlMoneda, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@vcod_tipmon", SqlDbType.VarChar, cod_tipmon, ParameterDirection.Input));

                return VCEDBContext<MonedaMensual>.CallStoreProcedure(StoredProcedures.CO_CatalogosValoresMonedaMensual, parameters, x => new MonedaMensual
                {
                    FechaInicial= x.GetString(0).Insert(4,"/"),
                    Valor = x.GetDecimal(1)
                }).ToList();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
        /// <summary>
        ///  Osvaldo Valdez Carrillo
        /// 23/08/2018
        /// </summary>
        /// <param name="vlMoneda">valor de la moneda</param>
        /// <param name="cod_tipmon">valor del tipo valor</param>
        /// <param name="fec_moneda">valor de la fecha</param>
        /// <returns>retorna el resultado de la consulta</returns>
        public MonedaMensual Consulta(string vlMoneda, string cod_tipmon, string fec_moneda)
        {
            try
            {
                var parameters = new List<SqlParameter>();
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pClave", SqlDbType.VarChar, "CONSULTAFECHA", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@vlMoneda", SqlDbType.VarChar, vlMoneda, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@vcod_tipmon", SqlDbType.VarChar, cod_tipmon, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@vfec_moneda", SqlDbType.VarChar, fec_moneda, ParameterDirection.Input));

                return VCEDBContext<MonedaMensual>.CallStoreProcedure(StoredProcedures.CO_CatalogosValoresMonedaMensual, parameters, x => new MonedaMensual
                {
                    FechaInicial = x.GetString(0).Insert(4, "/"),
                    Valor = x.GetDecimal(1)
                }).FirstOrDefault();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
        /// <summary>
        /// Osvaldo Valdez Carrillo
        /// 23/08/2018
        /// </summary>
        /// <param name="vlMoneda">valor de la moneda</param>
        /// <param name="cod_tipmon">valor del tipo valor</param>
        /// <param name="fec_moneda">valor de la fecha</param>
        /// <returns>retorna el resultado de la eliminacion</returns>
        public MonedaMensual EliminarValoresMoneda(string vlMoneda, string cod_tipmon, string fec_moneda)
        {
            try
            {
                var parameters = new List<SqlParameter>();
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pClave", SqlDbType.VarChar, "ELIMINAR", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@vlMoneda", SqlDbType.VarChar, vlMoneda, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@vcod_tipmon", SqlDbType.VarChar, cod_tipmon, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@vfec_moneda", SqlDbType.VarChar, fec_moneda, ParameterDirection.Input));

                return VCEDBContext<MonedaMensual>.CallStoreProcedure(StoredProcedures.CO_CatalogosValoresMonedaMensual, parameters, x => new MonedaMensual
                {
                    FechaInicial = x.GetString(0).Insert(4, "/"),
                    Valor = x.GetDecimal(1)
                }).FirstOrDefault();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
        /// <summary>
        /// Osvaldo Valdez Carrillo
        /// 23/08/2018
        /// </summary>
        /// <param name="vlMoneda">valor de la moneda</param>
        /// <param name="cod_tipmon">valor del tipo valor</param>
        /// <param name="moneda">valor del texto de la moneda</param>
        /// <returns>retorna la consulta del reporte</returns>
        public List<MonedaMensual> ConsultaRpt(string vlMoneda, string cod_tipmon, string moneda)
        {
            try
            {
                var parameters = new List<SqlParameter>();
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pClave", SqlDbType.VarChar, "CARGARTABLA", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@vlMoneda", SqlDbType.VarChar, vlMoneda, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@vcod_tipmon", SqlDbType.VarChar, cod_tipmon, ParameterDirection.Input));

                return VCEDBContext<MonedaMensual>.CallStoreProcedure(StoredProcedures.CO_CatalogosValoresMonedaMensual, parameters, x => new MonedaMensual
                {
                    FechaInicial = x.GetString(0).Insert(4, "/"),
                    Valor = x.GetDecimal(1),
                    valorMoneda= moneda
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
        /// 23/08/2018
        /// </summary>
        /// <param name="vlMoneda">valor de la moneda</param>
        /// <param name="cod_tipmon">valor del tipo valor</param>
        /// <param name="fec_moneda">valor de la fecha</param>
        /// <param name="valor">valore del periodo</param>
        /// <param name="clave">clave para actualizar o guardar</param>
        /// <returns>retorna el resultado de guardar o actualizar</returns>
        public MonedaMensual GrabarValoresMoneda(string vlMoneda, string cod_tipmon, string fec_moneda, decimal valor, string clave, string usuario)
        {
            try
            {
                var parameters = new List<SqlParameter>();
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pClave", SqlDbType.VarChar, clave, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@vlMoneda", SqlDbType.VarChar, vlMoneda, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@vcod_tipmon", SqlDbType.VarChar, cod_tipmon, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@vfec_moneda", SqlDbType.VarChar, fec_moneda, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@vValorVM", SqlDbType.Decimal, valor, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pUsuario", SqlDbType.VarChar, usuario, ParameterDirection.Input));

                return VCEDBContext<MonedaMensual>.CallStoreProcedure(StoredProcedures.CO_CatalogosValoresMonedaMensual, parameters, x => new MonedaMensual
                {
                    FechaInicial = x.GetString(0).Insert(4, "/"),
                    Valor = x.GetDecimal(1)
                }).FirstOrDefault();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
    }
}
