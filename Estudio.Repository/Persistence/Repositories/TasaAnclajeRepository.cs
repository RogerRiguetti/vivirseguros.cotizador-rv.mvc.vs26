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
    public class TasaAnclajeRepository
    {
        /// <summary>
        /// Osvaldo Valdez Carrillo
        /// 2018-08-10 
        /// </summary>
        /// <param name="vlMoneda">valor de la moneda</param>
        /// <param name="vlReajuste">valor del reajuste</param>
        /// <returns>Lista de Periodos</returns>
        public List<Periodo> ListaPeriodos(string vlMoneda, int vlReajuste)
        {
            try
            {
                var parameters = new List<SqlParameter>();
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pClave", SqlDbType.VarChar, "TIPOSPERIODOS", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@vlMoneda", SqlDbType.VarChar, vlMoneda, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@vlReajuste", SqlDbType.Int, vlReajuste, ParameterDirection.Input));
                return VCEDBContext<Periodo>.CallStoreProcedure(StoredProcedures.CO_CatalogosTasaAnclaje, parameters, x => new Periodo
                {
                    IdPeriodo = DateTime.ParseExact(x.GetString(0), "yyyyMMdd", CultureInfo.InvariantCulture).ToString("dd/MM/yyyy"),
                    Elemento = DateTime.ParseExact(x.GetString(0), "yyyyMMdd", CultureInfo.InvariantCulture).ToString("dd/MM/yyyy") + " * " + DateTime.ParseExact(x.GetString(1), "yyyyMMdd", CultureInfo.InvariantCulture).ToString("dd/MM/yyyy")
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
        /// 2018-08-10 
        /// </summary>
        /// <param name="strFecIni">fecha de incio</param>
        /// <param name="vlMoneda">valor de la moneda</param>
        /// <param name="vlReajuste">valor del reajuste</param>
        /// <returns>Consulta la vigencia</returns>
        public TasaAnclaje BuscarVigencia(DateTime strFecIni, string vlMoneda, int vlReajuste)
        {
            try
            {
                var parameters = new List<SqlParameter>();
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pClave", SqlDbType.VarChar, "BUSCARVIGENCIA", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@vstrFecIni", SqlDbType.VarChar, strFecIni.ToString("yyyyMMdd"), ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@vlMoneda", SqlDbType.VarChar, vlMoneda, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@vlReajuste", SqlDbType.Int, vlReajuste, ParameterDirection.Input));

                return VCEDBContext<TasaAnclaje>.CallStoreProcedure(StoredProcedures.CO_CatalogosTasaAnclaje, parameters, x => new TasaAnclaje
                {
                    FechaTermino = DateTime.ParseExact(x.GetString(0), "yyyyMMdd", CultureInfo.InvariantCulture).ToString("yyyy-MM-dd"),
                    Tasa = x.GetDecimal(1),
                    FechaInicial = strFecIni.ToString("yyyy-MM-dd")
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
        /// 2018-08-14
        /// </summary>
        /// <param name="strFecIni">fecha inicio</param>
        /// <param name="strFecFin">fecha fin </param>
        /// <param name="vlMoneda">valor de la moneda</param>
        /// <param name="vlReajuste">valor del reaujuste</param>
        /// <param name="tasa">valor de la tasa</param>
        /// <param name="clave">clave para la consulta</param>
        /// <returns>retorna la consulta solicitada</returns>
        public TasaAnclaje GrabarTasaAnclaje(DateTime strFecIni, DateTime strFecFin, string vlMoneda, int vlReajuste, decimal tasa,string clave)
        {
            try
            {
                var parameters = new List<SqlParameter>();
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pClave", SqlDbType.VarChar, clave, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@vstrFecIni", SqlDbType.VarChar, strFecIni.ToString("yyyyMMdd"), ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@vstrFecTer", SqlDbType.VarChar, strFecFin.ToString("yyyyMMdd"), ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@vlMoneda", SqlDbType.VarChar, vlMoneda, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@vlReajuste", SqlDbType.Int, vlReajuste, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@vlTasa", SqlDbType.VarChar, tasa.ToString("G"), ParameterDirection.Input));

                return VCEDBContext<TasaAnclaje>.CallStoreProcedure(StoredProcedures.CO_CatalogosTasaAnclaje, parameters, x => new TasaAnclaje
                {
                    FechaInicial = DateTime.ParseExact(x.GetString(0), "yyyyMMdd", CultureInfo.InvariantCulture).ToString("yyyy-MM-dd")
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
        /// 2018-08-15
        /// </summary>
        /// <param name="strFecIni">fecha inicio</param>
        /// <param name="strFecFin">fecha fin</param>
        /// <param name="vlMoneda">valor de la moneda</param>
        /// <param name="vlReajuste">valor del reajuste de la moneda</param>
        /// <param name="clave">clave para la consulta</param>
        /// <returns>retorna la consulta solicitada</returns>
        public TasaAnclaje EliminarTasaAnclaje(DateTime strFecIni, DateTime strFecFin, string vlMoneda, int vlReajuste, string clave)
        {
            try
            {
                var parameters = new List<SqlParameter>();
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pClave", SqlDbType.VarChar, clave, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@vstrFecIni", SqlDbType.VarChar, strFecIni.ToString("yyyyMMdd"), ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@vstrFecTer", SqlDbType.VarChar, strFecFin.ToString("yyyyMMdd"), ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@vlMoneda", SqlDbType.VarChar, vlMoneda, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@vlReajuste", SqlDbType.Int, vlReajuste, ParameterDirection.Input));

                return VCEDBContext<TasaAnclaje>.CallStoreProcedure(StoredProcedures.CO_CatalogosTasaAnclaje, parameters, x => new TasaAnclaje
                {
                    FechaInicial = DateTime.ParseExact(x.GetString(0), "yyyyMMdd", CultureInfo.InvariantCulture).ToString("yyyy-MM-dd")
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
        /// 2018-08-16
        /// </summary>
        /// <param name="vlMoneda">valor de la moneda</param>
        /// <param name="vlReajuste">valor del reajuste</param>
        /// <param name="moneda">valor del texto del combo moneda</param>
        /// <returns>retorna la consulta</returns>
        public List<TasaAnclaje> ConsultaRpt(string vlMoneda, int vlReajuste, string moneda)
        {
            try
            {
                var parameters = new List<SqlParameter>();
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pClave", SqlDbType.VarChar, "REPORTE", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@vlMoneda", SqlDbType.VarChar, vlMoneda, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@vlReajuste", SqlDbType.Int, vlReajuste, ParameterDirection.Input));
                return VCEDBContext<TasaAnclaje>.CallStoreProcedure(StoredProcedures.CO_CatalogosTasaAnclaje, parameters, x => new TasaAnclaje
                {
                    FechaInicial = DateTime.ParseExact(x.GetString(0), "yyyyMMdd", CultureInfo.InvariantCulture).ToString("dd-MM-yyyy"),
                    FechaTermino = DateTime.ParseExact(x.GetString(1), "yyyyMMdd", CultureInfo.InvariantCulture).ToString("dd-MM-yyyy"),
                    Tasa = x.GetDecimal(2),
                    valorMoneda= moneda//+" "+vlMoneda+" # "+vlReajuste
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
