using Estudio.Repository.Core.Domain;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Estudio.Repository.Persistence.Repositories
{
    public class TasasMercadoRepository
    {
        /// <summary>
        /// Osvaldo Valdez Carrillo
        /// 2018-08-10 
        /// </summary>
        /// <param name="vlMoneda">valor de la moneda</param>
        /// <param name="vlReajuste">valor del reajuste</param>
        /// <returns>Lista de años</returns>
        public List<TasaMercado> ListaYear(string vlMoneda, int vlReajuste)
        {
            try
            {
                var parameters = new List<SqlParameter>();
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pClave", SqlDbType.VarChar, "LISTAYEAR", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@vlMoneda", SqlDbType.VarChar, vlMoneda, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@vlReajuste", SqlDbType.Int, vlReajuste, ParameterDirection.Input));

                return VCEDBContext<TasaMercado>.CallStoreProcedure(StoredProcedures.CO_CatalogosTasaMercadoHistoricas, parameters, x => new TasaMercado
                {
                    Year = x.GetInt32(0)
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
        /// <param name="vlMoneda">valor de la moneda</param>
        /// <param name="vlReajuste">valor del reajuste</param>
        /// <param name="vlAnno">valor del año</param>
        /// <returns>Información del año</returns>
        public TasaMercado BuscarYear(string vlMoneda, int vlReajuste, int vlAnno)
        {
            try
            {
                var parameters = new List<SqlParameter>();
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pClave", SqlDbType.VarChar, "BUSCARYEAR", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@vlMoneda", SqlDbType.VarChar, vlMoneda, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@vlReajuste", SqlDbType.Int, vlReajuste, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@vlAnno", SqlDbType.Int, vlAnno, ParameterDirection.Input));

                return VCEDBContext<TasaMercado>.CallStoreProcedure(StoredProcedures.CO_CatalogosTasaMercadoHistoricas, parameters, x => new TasaMercado
                {
                    Year = x.GetInt32(0),
                    Enero=x.GetDecimal(1),
                    Febrero = x.GetDecimal(2),
                    Marzo = x.GetDecimal(3),
                    Abril = x.GetDecimal(4),
                    Mayo = x.GetDecimal(5),
                    Junio = x.GetDecimal(6),
                    Julio = x.GetDecimal(7),
                    Agosto = x.GetDecimal(8),
                    Septiembre = x.GetDecimal(9),
                    Octubre = x.GetDecimal(10),
                    Noviembre = x.GetDecimal(11),
                    Diciembre = x.GetDecimal(12),

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
        /// 2018-08-10 
        /// </summary>
        /// <param name="vlMoneda">valor de la moneda</param>
        /// <param name="vlReajuste">valor del reajuste</param>
        /// <param name="meses">Arreglo con la informacion del mes</param>
        /// <param name="clave">clave para activar el guardado o modificación</param>
        /// <returns>retorna la lista de los años</returns>
        public List<TasaMercado> GrabarTasaMercado(string vlMoneda, int vlReajuste, TasaMercado meses, string clave)
        {
            try
            {
                var parameters = new List<SqlParameter>();
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pClave", SqlDbType.VarChar, clave, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@vlMoneda", SqlDbType.VarChar, vlMoneda, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@vlReajuste", SqlDbType.Int, vlReajuste, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@vlAnno", SqlDbType.Int, meses.Year, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@vl01", SqlDbType.VarChar, meses.Enero, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@vl02", SqlDbType.VarChar, meses.Febrero, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@vl03", SqlDbType.VarChar, meses.Marzo, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@vl04", SqlDbType.VarChar, meses.Abril, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@vl05", SqlDbType.VarChar, meses.Mayo, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@vl06", SqlDbType.VarChar, meses.Junio, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@vl07", SqlDbType.VarChar, meses.Julio, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@vl08", SqlDbType.VarChar, meses.Agosto, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@vl09", SqlDbType.VarChar, meses.Septiembre, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@vl10", SqlDbType.VarChar, meses.Octubre, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@vl11", SqlDbType.VarChar, meses.Noviembre, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@vl12", SqlDbType.VarChar, meses.Diciembre, ParameterDirection.Input));

                return VCEDBContext<TasaMercado>.CallStoreProcedure(StoredProcedures.CO_CatalogosTasaMercadoHistoricas, parameters, x => new TasaMercado
                {
                    Year = x.GetInt32(0)
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
        /// <param name="vlMoneda">valor de la moneda</param>
        /// <param name="vlReajuste">valor del reajuste</param>
        /// <param name="moneda">valor de la moneda</param>
        /// <returns>lista con la información del reporte</returns>
        public List<TasaMercado> ConsultaRpt(string vlMoneda, int vlReajuste, string moneda)
        {
            try
            {
                var parameters = new List<SqlParameter>();
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pClave", SqlDbType.VarChar, "REPORTE", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@vlMoneda", SqlDbType.VarChar, vlMoneda, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@vlReajuste", SqlDbType.Int, vlReajuste, ParameterDirection.Input));

                return VCEDBContext<TasaMercado>.CallStoreProcedure(StoredProcedures.CO_CatalogosTasaMercadoHistoricas, parameters, x => new TasaMercado
                {
                    Year = x.GetInt32(0),
                    Enero = x.GetDecimal(1),
                    Febrero = x.GetDecimal(2),
                    Marzo = x.GetDecimal(3),
                    Abril = x.GetDecimal(4),
                    Mayo = x.GetDecimal(5),
                    Junio = x.GetDecimal(6),
                    Julio = x.GetDecimal(7),
                    Agosto = x.GetDecimal(8),
                    Septiembre = x.GetDecimal(9),
                    Octubre = x.GetDecimal(10),
                    Noviembre = x.GetDecimal(11),
                    Diciembre = x.GetDecimal(12),
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
        /// 2018-08-10 
        /// </summary>
        /// <param name="vlMoneda">valor de la moneda</param>
        /// <param name="vlReajuste">valor del reajuste</param>
        /// <param name="year">valor del año</param>
        /// <returns>lista actual de años</returns>
        public List<TasaMercado> EliminarTasaMercado(string vlMoneda, int vlReajuste, int year)
        {
            try
            {
                var parameters = new List<SqlParameter>();
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pClave", SqlDbType.VarChar, "BORRARYEAR", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@vlMoneda", SqlDbType.VarChar, vlMoneda, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@vlReajuste", SqlDbType.Int, vlReajuste, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@vlAnno", SqlDbType.Int,year, ParameterDirection.Input));

                return VCEDBContext<TasaMercado>.CallStoreProcedure(StoredProcedures.CO_CatalogosTasaMercadoHistoricas, parameters, x => new TasaMercado
                {
                    Year = x.GetInt32(0)
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
