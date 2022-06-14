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
    public class GastosSepelioRepository
    {
        /// <summary>
        /// Osvaldo Valdez Carrillo
        /// 2018-08-20
        /// </summary>
        /// <returns>Retorna la lista con las fechas</returns>
        public List<GastoSepelio> CargaFechas()
        {
            try
            {
                var parameters = new List<SqlParameter>();
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pClave", SqlDbType.VarChar, "CARGARTABLA", ParameterDirection.Input));

                return VCEDBContext<GastoSepelio>.CallStoreProcedure(StoredProcedures.CO_CatalogosGastosSepelio, parameters, x => new GastoSepelio
                {
                    FechaInicial = DateTime.ParseExact(x.GetString(0), "yyyyMMdd", CultureInfo.InvariantCulture).ToString("dd/MM/yyyy"),
                    FechaTermino = DateTime.ParseExact(x.GetString(1), "yyyyMMdd", CultureInfo.InvariantCulture).ToString("dd/MM/yyyy"),
                    Valor= x.GetDecimal(2)
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
        /// 2018-08-20
        /// </summary>
        /// <param name="strFecIni">Fecha inicial</param>
        /// <returns>Retorna la informacion de la fecha consultada</returns>
        public GastoSepelio ConsultaFecha(DateTime strFecIni)
        {
            try
            {
                var parameters = new List<SqlParameter>();
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pClave", SqlDbType.VarChar, "CONSULTAFECHA", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@vstrFecIni", SqlDbType.VarChar, strFecIni.ToString("yyyyMMdd"), ParameterDirection.Input));

                return VCEDBContext<GastoSepelio>.CallStoreProcedure(StoredProcedures.CO_CatalogosGastosSepelio, parameters, x => new GastoSepelio
                {
                    FechaInicial = DateTime.ParseExact(x.GetString(0), "yyyyMMdd", CultureInfo.InvariantCulture).ToString("dd/MM/yyyy"),
                    FechaTermino = DateTime.ParseExact(x.GetString(1), "yyyyMMdd", CultureInfo.InvariantCulture).ToString("dd/MM/yyyy"),
                    Valor = x.GetDecimal(2)
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
        /// 2018-08-21
        /// </summary>
        /// <param name="Clave">Clave para realizar una accion determinada en el store</param>
        /// <param name="strFecIni">fecha inicial</param>
        /// <param name="strFecTer">fecha de termino</param>
        /// <param name="usuario">nombre del usuario</param>
        /// <param name="vlGasto">monto del gasto</param>
        /// <returns>retorna la fecha fin y termino</returns>
        public GastoSepelio GrabarSepelio(string Clave,DateTime strFecIni,DateTime strFecTer,string usuario, decimal vlGasto)
        {
            try
            {
                DateTime fecha = DateTime.Now;
                var parameters = new List<SqlParameter>();
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pClave", SqlDbType.VarChar, Clave, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@vstrFecIni", SqlDbType.VarChar, strFecIni.ToString("yyyyMMdd"), ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@vstrFecTer", SqlDbType.VarChar, strFecTer.ToString("yyyyMMdd"), ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@vgUsuario", SqlDbType.VarChar, usuario, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@vfec_crea", SqlDbType.VarChar, fecha.ToString("yyyyMMdd"), ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@vhor_crea", SqlDbType.VarChar, fecha.ToString("hhmmss"), ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@vlGasto", SqlDbType.VarChar, vlGasto.ToString("G"), ParameterDirection.Input));

                return VCEDBContext<GastoSepelio>.CallStoreProcedure(StoredProcedures.CO_CatalogosGastosSepelio, parameters, x => new GastoSepelio
                {
                    FechaInicial = DateTime.ParseExact(x.GetString(0), "yyyyMMdd", CultureInfo.InvariantCulture).ToString("dd/MM/yyyy"),
                    FechaTermino = DateTime.ParseExact(x.GetString(1), "yyyyMMdd", CultureInfo.InvariantCulture).ToString("dd/MM/yyyy")
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
        /// 2018-08-22
        /// </summary>
        /// <param name="Clave">Clave para realizar una accion determinada en el store</param>
        /// <param name="strFecIni">fecha inicial</param>
        /// <param name="strFecTer">fecha de termino</param>
        /// <returns>retorna la fecha fin y termino<</returns>
        public GastoSepelio EliminarSepelio(DateTime strFecIni, DateTime strFecTer, string Clave)
        {
            try
            {
                DateTime fecha = DateTime.Now;
                var parameters = new List<SqlParameter>();
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pClave", SqlDbType.VarChar, Clave, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@vstrFecIni", SqlDbType.VarChar, strFecIni.ToString("yyyyMMdd"), ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@vstrFecTer", SqlDbType.VarChar, strFecTer.ToString("yyyyMMdd"), ParameterDirection.Input));

                return VCEDBContext<GastoSepelio>.CallStoreProcedure(StoredProcedures.CO_CatalogosGastosSepelio, parameters, x => new GastoSepelio
                {
                    FechaInicial = DateTime.ParseExact(x.GetString(0), "yyyyMMdd", CultureInfo.InvariantCulture).ToString("dd/MM/yyyy"),
                    FechaTermino = DateTime.ParseExact(x.GetString(1), "yyyyMMdd", CultureInfo.InvariantCulture).ToString("dd/MM/yyyy")
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
        /// 2018-08-22
        /// </summary>
        /// <returns>retorna la lista para mostrar en el reporte</returns>
        public List<GastoSepelio> ConsultaRpt()
        {
            try
            {
                DateTime fecha = DateTime.Now;
                var parameters = new List<SqlParameter>();
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pClave", SqlDbType.VarChar, "REPORTE", ParameterDirection.Input));

                return VCEDBContext<GastoSepelio>.CallStoreProcedure(StoredProcedures.CO_CatalogosGastosSepelio, parameters, x => new GastoSepelio
                {
                    FechaInicial = DateTime.ParseExact(x.GetString(0), "yyyyMMdd", CultureInfo.InvariantCulture).ToString("dd/MM/yyyy"),
                    FechaTermino = DateTime.ParseExact(x.GetString(1), "yyyyMMdd", CultureInfo.InvariantCulture).ToString("dd/MM/yyyy"),
                    Valor= x.GetDecimal(2),
                    valorMoneda= "NS - Nuevos Soles"
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
