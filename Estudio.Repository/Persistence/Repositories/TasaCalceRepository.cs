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
    public class TasaCalceRepository
    {
        /// <summary>
        /// Antonio Quezada
        /// 2018-08-14
        /// Carga el combo de Periodos de Tasa Calce
        /// </summary>
        /// <param name="codigoMoneda"> Código de la Moneda </param>
        /// <param name="reajuste"> Tipo de Reajuste de la Moneda </param>
        /// <returns> Regresa una lista de Periodos para Tasa Calce </returns>

        public List<Periodo> ConsultaPeriodos(string codigoMoneda, string reajuste)
        {
            try
            {
                var parameters = new List<SqlParameter>();
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pClave", SqlDbType.VarChar, "PERIODOS", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pCodigoMoneda", SqlDbType.VarChar, codigoMoneda, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pTipoReajuste", SqlDbType.VarChar, reajuste, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pFechaInicio", SqlDbType.VarChar, "", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pFechaTermino", SqlDbType.VarChar, "", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pTramo", SqlDbType.Int, 0, ParameterDirection.Input));

                return VCEDBContext<Periodo>.CallStoreProcedure(StoredProcedures.CO_ConsultasTasaCalce, parameters, x => new Periodo
                {
                    Elemento = DateTime.ParseExact(x.GetString(0), "yyyyMMdd", CultureInfo.InvariantCulture).ToString("dd/MM/yyyy") + " * " + DateTime.ParseExact(x.GetString(1), "yyyyMMdd", CultureInfo.InvariantCulture).ToString("dd/MM/yyyy")
                }).ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Antonio Quezada
        /// 2018-08-16
        /// Obtiene los registros de la Tasa de Descuento Anual
        /// </summary>
        /// <param name="fechaInicio"> Fecha de inicio del Periodo </param>
        /// <param name="codigoMoneda"> Código de la Moneda </param>
        /// <param name="reajuste"> Tipo de Reajuste </param>
        /// <returns> Regresa una lista de Tasas </returns>

        public List<TasaDescuentoAnual> ConsultaTasaDescAnual(string fechaInicio, string codigoMoneda, string reajuste)
        {
            try
            {
                var parameters = new List<SqlParameter>();
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pClave", SqlDbType.VarChar, "DESCANUAL", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pCodigoMoneda", SqlDbType.VarChar, codigoMoneda, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pTipoReajuste", SqlDbType.VarChar, reajuste, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pFechaInicio", SqlDbType.VarChar, fechaInicio, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pFechaTermino", SqlDbType.VarChar, "", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pTramo", SqlDbType.Int, 0, ParameterDirection.Input));

                return VCEDBContext<TasaDescuentoAnual>.CallStoreProcedure(StoredProcedures.CO_ConsultasTasaCalce, parameters, x => new TasaDescuentoAnual
                {
                    Tramo = x.GetInt32(0),
                    AK = x.GetDecimal(1),
                    BK = x.GetDecimal(2),
                    CK = x.GetDecimal(3),
                    CPK = x.GetDecimal(4).ToString("N8"),
                    FechaInicioStr = x.GetString(5).Substring(0, 4) + "-" + x.GetString(5).Substring(4, 2) + "-" + x.GetString(5).Substring(6, 2),
                    FechaTerminoStr = x.GetString(6).Substring(0,4) + "-" + x.GetString(6).Substring(4, 2) + "-" + x.GetString(6).Substring(6, 2),
                    Periodo = x.GetString(5).Substring(0, 4) + "-" + x.GetString(5).Substring(4, 2) + "-" + x.GetString(5).Substring(6, 2) + " * " + x.GetString(6).Substring(0, 4) + "-" + x.GetString(6).Substring(4, 2) + "-" + x.GetString(6).Substring(6, 2),
                    TipoMoneda = x.GetString(7) + " - " + x.GetString(8)
                }).ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Antonio Quezada
        /// 2018-08-20
        /// Consulta de la información por Tramo
        /// </summary>
        /// <param name="tramo"> Número de Año (Tramo) </param>
        /// <param name="fechaInicio"> Fecha de inicio del Periodo </param>
        /// <param name="codigoMoneda"> Código de la Moneda </param>
        /// <param name="reajuste"> Tipo de Reajuste </param>
        /// <returns> Regresa la información del tramo en un objeto </returns>

        public TasaDescuentoAnual ConsultaTramo(int tramo, string fechaInicio, string codigoMoneda, string reajuste)
        {
            try
            {
                var parameters = new List<SqlParameter>();
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pClave", SqlDbType.VarChar, "TRAMO", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pCodigoMoneda", SqlDbType.VarChar, codigoMoneda, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pTipoReajuste", SqlDbType.VarChar, reajuste, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pFechaInicio", SqlDbType.VarChar, fechaInicio, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pFechaTermino", SqlDbType.VarChar, "", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pTramo", SqlDbType.Int, tramo, ParameterDirection.Input));

                return VCEDBContext<TasaDescuentoAnual>.CallStoreProcedure(StoredProcedures.CO_ConsultasTasaCalce, parameters, x => new TasaDescuentoAnual
                {
                    Tramo = x.GetInt32(0),
                    AK = Convert.ToInt16(x.GetDecimal(1)),
                    BK = Convert.ToInt16(x.GetDecimal(2)),
                    CK = Convert.ToInt16(x.GetDecimal(3)),
                    CPK = x.GetDecimal(4).ToString("N8")
                }).FirstOrDefault();
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Antonio Quezada
        /// 2018-08-22
        /// Registra o modifica la información de Tasa de Calce (Tramo)
        /// </summary>
        /// <param name="clave"> Clave de la sentencia a ejecutar (INSERT o UPDATE) </param>
        /// <param name="tasa"> Información que será registrada o modificada </param>

        public void RegistrarModificarTasaCalce(string clave, TasaDescuentoAnual tasa)
        {
            try
            {
                var parameters = new List<SqlParameter>();
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pClave", SqlDbType.VarChar, clave, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pCodigoMoneda", SqlDbType.VarChar, tasa.CodigoMoneda, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pTipoReajuste", SqlDbType.VarChar, tasa.TipoReajuste, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pFechaInicio", SqlDbType.VarChar, tasa.FechaInicioStr, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pFechaTermino", SqlDbType.VarChar, tasa.FechaTerminoStr, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pTramo", SqlDbType.Int, tasa.Tramo, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pAK", SqlDbType.Decimal, tasa.AK, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pBK", SqlDbType.Decimal, tasa.BK, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pCK", SqlDbType.Decimal, tasa.CK, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pCPK", SqlDbType.Decimal, tasa.CPK, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pUsuario", SqlDbType.VarChar, tasa.Usuario, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pFecha", SqlDbType.VarChar, tasa.Fecha, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pHora", SqlDbType.VarChar, tasa.Hora, ParameterDirection.Input));

                VCEDBContext<DataTable>.CallStoreProcedureDt(StoredProcedures.CO_CatalogoTasaCalce, parameters);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Antonio Quezada
        /// 2018-08-23
        /// Consulta las fechas de Inicio de Periodo
        /// </summary>
        /// <param name="clave"> Clave de la consulta a ejecutar 
        /// ANTERIOR = FECHA DE TERMINO DEL REGISTRO ANTERIOR 
        /// REGISTRADO = FECHA DEL TERMINO DE VIGENCIA DEL REGISTRO INGRESADDO </param>
        /// <param name="fechaInicio"> Fecha de inicio del Periodo </param>
        /// <param name="codigoMoneda"> Código de la Moneda </param>
        /// <param name="reajuste"> Tipo de Reajuste </param>
        /// <returns> Regresa una lista de fechas </returns>

        public TasaDescuentoAnual ConsultaFechasInicio(string clave, DateTime fechaInicio,DateTime fechaFin, string codigoMoneda, string reajuste)
        {
            try
            {
                var parameters = new List<SqlParameter>();
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pClave", SqlDbType.VarChar, clave, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pCodigoMoneda", SqlDbType.VarChar, codigoMoneda, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pTipoReajuste", SqlDbType.VarChar, reajuste, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pFechaInicio", SqlDbType.VarChar, fechaInicio.ToString("yyyyMMdd"), ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pFechaTermino", SqlDbType.VarChar, fechaFin.ToString("yyyyMMdd"), ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pTramo", SqlDbType.Int, 0, ParameterDirection.Input));

                return VCEDBContext<TasaDescuentoAnual>.CallStoreProcedure(StoredProcedures.CO_ConsultasTasaCalce, parameters, x => new TasaDescuentoAnual
                {
                    FechaInicioStr = DateTime.ParseExact(x.GetString(0), "yyyyMMdd", CultureInfo.InvariantCulture).ToString("yyyy-MM-dd")
                }).FirstOrDefault();
            }
            catch (Exception)
            {
                throw;
            }
        }


        /// <summary>
        /// Antonio Quezada
        /// 2018-08-27
        /// Elimina los Tramos del Periodo seleccionado
        /// </summary>
        /// <param name="fechaInicio"> Fecha de Inicio del Periodo </param>
        /// <param name="codigoMoneda"> Código de la Moneda </param>
        /// <param name="reajuste"> Tipo de Reajuste </param>

        public void EliminarTasaCalce(string fechaInicio, string codigoMoneda, string reajuste)
        {
            try
            {
                var parameters = new List<SqlParameter>();
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pClave", SqlDbType.VarChar, "ELIMINAR", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pCodigoMoneda", SqlDbType.VarChar, codigoMoneda, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pTipoReajuste", SqlDbType.VarChar, reajuste, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pFechaInicio", SqlDbType.VarChar, fechaInicio, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pFechaTermino", SqlDbType.VarChar, "", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pTramo", SqlDbType.Int, 0, ParameterDirection.Input));

                VCEDBContext<DataTable>.CallStoreProcedureDt(StoredProcedures.CO_ConsultasTasaCalce, parameters);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
