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
    public class LimiteCotizacionExtraOficialRepository
    {

        /// <summary>
        /// osvaldo valdez
        /// 29-08-18
        /// </summary>
        /// <returns>retorna los tipo de departamento</returns>
        public List<Departamento> TipoDepartamento()
        {
            try
            {
                var parameters = new List<SqlParameter>();
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pClave", SqlDbType.VarChar, "CMBDEPARTAMENTO", ParameterDirection.Input));

                return VCEDBContext<Departamento>.CallStoreProcedure(StoredProcedures.CO_CatalogosLimiteCotizacionExtraOficial, parameters, x => new Departamento
                {
                    ClaveDepartamento = x.GetString(0),
                    Elemento = x.GetString(0) + " - " + x.GetString(1)
                    //mto_minimo
                    //mto_maximo
                }).ToList();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
        /// <summary>
        /// osvaldo valdez
        /// 29-08-18
        /// </summary>
        /// <returns>retorna los rangos de tasa</returns>
        public List<RangosTasaVenta> RangosTasa()
        {
            try
            {
                var parameters = new List<SqlParameter>();
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pClave", SqlDbType.VarChar, "CMBRANGOTASA", ParameterDirection.Input));

                return VCEDBContext<RangosTasaVenta>.CallStoreProcedure(StoredProcedures.CO_CatalogosLimiteCotizacionExtraOficial, parameters, x => new RangosTasaVenta
                {
                    IdRangoTasa = x.GetString(0),
                    Elemento = x.GetString(0) + " - " + x.GetString(1)
                }).ToList();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
        /// <summary>
        /// osvaldo valdez
        /// 29-08-18
        /// </summary>
        /// <param name="fechaIni">fecha inicial</param>
        /// <param name="codMoneda">codigo de moneda</param>
        /// <param name="reajuste">codigo de reajuste</param>
        /// <param name="departamento">codigo de departamento</param>
        /// <returns></returns>
        public LimiteCotizacionExtraOficial ConsultaInfo(DateTime fechaIni, string codMoneda, int reajuste, int departamento)
        {
            try
            {
                var parameters = new List<SqlParameter>();
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pClave", SqlDbType.VarChar, "CONSULTA", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@vFechaIni", SqlDbType.VarChar, fechaIni.ToString("yyyyMMdd"), ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@vCodMoneda", SqlDbType.VarChar, codMoneda, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@vReajuste", SqlDbType.Int, reajuste, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@vDepartamento", SqlDbType.Int, departamento, ParameterDirection.Input));

                return VCEDBContext<LimiteCotizacionExtraOficial>.CallStoreProcedure(StoredProcedures.CO_CatalogosLimiteCotizacionExtraOficial, parameters, x => new LimiteCotizacionExtraOficial
                {
                    Minimo_Tir = x.GetDecimal(0),
                    Maximo_Per = x.GetDecimal(1),
                    Minimo_Com = x.GetDecimal(2),
                    Maximo_Com = x.GetDecimal(3),
                    fechaInicial = DateTime.ParseExact(x.GetString(4), "yyyyMMdd", CultureInfo.InvariantCulture).ToString("yyyy-MM-dd"),
                    FechaFinal = DateTime.ParseExact(x.GetString(5), "yyyyMMdd", CultureInfo.InvariantCulture).ToString("yyyy-MM-dd")
                }).FirstOrDefault();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
        /// <summary>
        ///  osvaldo valdez
        /// 29-08-18
        /// </summary>
        /// <param name="clave">clave para la accion de guardado o actualizar</param>
        /// <param name="informacion">informacion que se va actualizar o guardar</param>
        /// <param name="fechaIni">fecha inicio</param>
        /// <param name="fechaFin">fecha fin</param>
        /// <param name="usuario">nombre del usuario</param>
        /// <returns>retorna las fechas para su validacion </returns>
        public LimiteCotizacionExtraOficial Grabar(string clave, LimiteCotizacionExtraOficial informacion, DateTime fechaIni, DateTime fechaFin, string usuario)
        {
            try
            {
                DateTime fecha = DateTime.Now;
                var parameters = new List<SqlParameter>();
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pClave", SqlDbType.VarChar, clave, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@vFechaIni", SqlDbType.VarChar, fechaIni.ToString("yyyyMMdd"), ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@vFechaFin", SqlDbType.VarChar, fechaFin.ToString("yyyyMMdd"), ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@vCodMoneda", SqlDbType.VarChar, informacion.CodMoneda, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@vReajuste", SqlDbType.Int, informacion.CodReajuste, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@vDepartamento", SqlDbType.Int, informacion.CodDepartamento, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@vCodRegion", SqlDbType.VarChar, informacion.CodDepartamento, ParameterDirection.Input));
                //datos TIR
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@vMinimo_Tir", SqlDbType.Decimal, informacion.Minimo_Tir, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@vMaximo_Tir", SqlDbType.Decimal, informacion.Maximo_Tir, ParameterDirection.Input));
                //usuario
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@vCodUsuario", SqlDbType.VarChar, usuario, ParameterDirection.Input));
                //fecha y hora de creacion
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@vfec_crea", SqlDbType.VarChar, fecha.ToString("yyyyMMdd"), ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@vhora_crea", SqlDbType.VarChar, fecha.ToString("hhmmss"), ParameterDirection.Input));
                //datos Comision 
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@vMinimo_Com", SqlDbType.Decimal, informacion.Minimo_Com, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@vMaximo_Com", SqlDbType.Decimal, informacion.Maximo_Com, ParameterDirection.Input));
                //datos Perdida 
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@vMinimo_Per", SqlDbType.Decimal, informacion.Minimo_Per, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@vMaximo_Per", SqlDbType.Decimal, informacion.Maximo_Per, ParameterDirection.Input));


                return VCEDBContext<LimiteCotizacionExtraOficial>.CallStoreProcedure(StoredProcedures.CO_CatalogosLimiteCotizacionExtraOficial, parameters, x => new LimiteCotizacionExtraOficial
                {
                    fechaInicial = DateTime.ParseExact(x.GetString(0), "yyyyMMdd", CultureInfo.InvariantCulture).ToString("yyyy-MM-dd"),
                    FechaFinal = DateTime.ParseExact(x.GetString(1), "yyyyMMdd", CultureInfo.InvariantCulture).ToString("yyyy-MM-dd")
                }).FirstOrDefault();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
        /// <summary>
        /// osvaldo valdez
        /// 29-08-18
        /// </summary>
        /// <param name="clave">clave para la accion de guardado o actualizar</param>
        /// <param name="informacion">informacion que se va actualizar o guardar</param>
        /// <param name="fechaIni">fecha inicio</param>
        /// <param name="fechaFin">fecha fin</param>
        /// <param name="usuario">nombre del usuario</param>
        /// <param name="CodPension">codigo de la pension</param>
        /// <param name="Minimo_TV">minimo de tasa venta</param>
        /// <param name="Maximo_TV">maximo de tasa venta</param>
        /// <returns>retorna las fechas para su validacion</returns>
        public LimiteCotizacionExtraOficial GrabarTasaVenta(string clave, LimiteCotizacionExtraOficial informacion, DateTime fechaIni, DateTime fechaFin, string usuario, string CodPension, decimal Minimo_TV, decimal Maximo_TV)
        {
            try
            {
                DateTime fecha = DateTime.Now;
                var parameters = new List<SqlParameter>();
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pClave", SqlDbType.VarChar, clave, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@vFechaIni", SqlDbType.VarChar, fechaIni.ToString("yyyyMMdd"), ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@vFechaFin", SqlDbType.VarChar, fechaFin.ToString("yyyyMMdd"), ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@vCodMoneda", SqlDbType.VarChar, informacion.CodMoneda, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@vReajuste", SqlDbType.Int, informacion.CodReajuste, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@vDepartamento", SqlDbType.Int, informacion.CodDepartamento, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@vCodRegion", SqlDbType.VarChar, informacion.CodDepartamento, ParameterDirection.Input));
                //datos TASA VENTA
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@vCodPension", SqlDbType.VarChar, CodPension, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@vMinimo_TV", SqlDbType.Decimal, Minimo_TV, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@vMaximo_TV", SqlDbType.Decimal, Maximo_TV, ParameterDirection.Input));
                //usuario
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@vCodUsuario", SqlDbType.VarChar, usuario, ParameterDirection.Input));
                //fecha y hora de creacion
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@vfec_crea", SqlDbType.VarChar, fecha.ToString("yyyyMMdd"), ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@vhora_crea", SqlDbType.VarChar, fecha.ToString("hhmmss"), ParameterDirection.Input));


                return VCEDBContext<LimiteCotizacionExtraOficial>.CallStoreProcedure(StoredProcedures.CO_CatalogosLimiteCotizacionExtraOficial, parameters, x => new LimiteCotizacionExtraOficial
                {
                    fechaInicial = DateTime.ParseExact(x.GetString(0), "yyyyMMdd", CultureInfo.InvariantCulture).ToString("yyyy-MM-dd"),
                    FechaFinal = DateTime.ParseExact(x.GetString(1), "yyyyMMdd", CultureInfo.InvariantCulture).ToString("yyyy-MM-dd")
                }).FirstOrDefault();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
        /// <summary>
        /// osvaldo valdez
        /// 29-08-18 
        /// </summary>
        /// <param name="vlMoneda">valor moneda</param>
        /// <param name="vlReajuste">valor del reajuste</param>
        /// <param name="departamento">valor del departamento</param>
        /// <returns>lista de periodos</returns>
        public List<Periodo> ListaPeriodos(string vlMoneda, int vlReajuste, int departamento)
        {
            try
            {
                var parameters = new List<SqlParameter>();
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pClave", SqlDbType.VarChar, "PERIODOS", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@vCodMoneda", SqlDbType.VarChar, vlMoneda, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@vReajuste", SqlDbType.Int, vlReajuste, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@vDepartamento", SqlDbType.Int, departamento, ParameterDirection.Input));

                return VCEDBContext<Periodo>.CallStoreProcedure(StoredProcedures.CO_CatalogosLimiteCotizacionExtraOficial, parameters, x => new Periodo
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
        /// osvaldo valdez
        /// 31/08/2018
        /// </summary>
        /// <param name="codMoneda">codigo de moneda</param>
        /// <param name="reajuste">valor del reajuste</param>
        /// <param name="moneda">valor de la moneda</param>
        /// <param name="departamento">valor del departamento</param>
        /// <param name="nomDepartamento">nombre del departamento</param>
        /// <param name="fechaInicial">fecha inicial</param>
        /// <returns>retorna la informacion para el reporte</returns>
        public List<LimiteCotizacionExtraOficial> ConsultaRpt(string codMoneda, int reajuste, string moneda, string departamento, string nomDepartamento, DateTime fechaInicial)
        {
            try
            {
                var parameters = new List<SqlParameter>();
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pClave", SqlDbType.VarChar, "REPORTE", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@vFechaIni", SqlDbType.VarChar, fechaInicial.ToString("yyyyMMdd"), ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@vCodMoneda", SqlDbType.VarChar, codMoneda, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@vReajuste", SqlDbType.Int, reajuste, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@vCodRegion", SqlDbType.VarChar, departamento, ParameterDirection.Input));

                return VCEDBContext<LimiteCotizacionExtraOficial>.CallStoreProcedure(StoredProcedures.CO_CatalogosLimiteCotizacionExtraOficial, parameters, x => new LimiteCotizacionExtraOficial
                {
                    //Nombre de la moneda
                    DescMoneda = moneda,
                    //nombre del departamento
                    CodDepartamento = nomDepartamento,
                    //tir minima
                    Minimo_Tir = x.GetDecimal(0),
                    //perdida maxima
                    Maximo_Per = x.GetDecimal(1),
                    //comision minima y maxima
                    Minimo_Com = x.GetDecimal(2),
                    Maximo_Com = x.GetDecimal(3),
                    //rango de fechas
                    fechaInicial = DateTime.ParseExact(x.GetString(4), "yyyyMMdd", CultureInfo.InvariantCulture).ToString("dd/MM/yyyy"),
                    FechaFinal = DateTime.ParseExact(x.GetString(5), "yyyyMMdd", CultureInfo.InvariantCulture).ToString("dd/MM/yyyy"),
                    //Tasa de venta
                    CodPension = x.GetString(6),
                    NombrePension = x.GetString(7),
                    Maximo_TV = x.GetDecimal(8)

                }).ToList();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        /// <summary>
        /// osvaldo valdez
        /// 29-08-18
        /// </summary>
        /// <param name="fechaIni">fecah inicial</param>
        /// <param name="codMoneda">codigo de moneda</param>
        /// <param name="reajuste">codigo de reajuste</param>
        /// <param name="departamento">codigo de departamento</param>
        /// <returns>lista de los rangos de tasa de venta</returns>
        public List<LimiteCotizacionExtraOficial> RangosTasaVenta(DateTime fechaIni, string codMoneda, int reajuste, int departamento)
        {
            try
            {
                var parameters = new List<SqlParameter>();
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pClave", SqlDbType.VarChar, "CONSULTATV", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@vFechaIni", SqlDbType.VarChar, fechaIni.ToString("yyyyMMdd"), ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@vCodMoneda", SqlDbType.VarChar, codMoneda, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@vReajuste", SqlDbType.Int, reajuste, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@vDepartamento", SqlDbType.Int, departamento, ParameterDirection.Input));

                return VCEDBContext<LimiteCotizacionExtraOficial>.CallStoreProcedure(StoredProcedures.CO_CatalogosLimiteCotizacionExtraOficial, parameters, x => new LimiteCotizacionExtraOficial
                {
                    CodMoneda = x.GetString(0),
                    CodPension = x.GetString(1),
                    Minimo_TV = x.GetDecimal(2),
                    Maximo_TV = x.GetDecimal(3)
                }).ToList();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
        /// <summary>
        /// osvaldo valdez
        /// 29-08-18
        /// </summary>
        /// <param name="clave">clave de eliminacion</param>
        /// <param name="fechaIni">fecha inicial</param>
        /// <param name="fechaFin">fecha fin</param>
        /// <param name="informacion">informacion a eliminar</param>
        /// <returns>retorna las fechas a actualizar despues de eliminar</returns>
        public LimiteCotizacionExtraOficial EliminarCorizacionMejorada(string clave, DateTime fechaIni, DateTime fechaFin, LimiteCotizacionExtraOficial informacion)
        {
            try
            {
                var parameters = new List<SqlParameter>();
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pClave", SqlDbType.VarChar, clave, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@vFechaIni", SqlDbType.VarChar, fechaIni.ToString("yyyyMMdd"), ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@vFechaFin", SqlDbType.VarChar, fechaFin.ToString("yyyyMMdd"), ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@vCodMoneda", SqlDbType.VarChar, informacion.CodMoneda, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@vReajuste", SqlDbType.Int, informacion.CodReajuste, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@vDepartamento", SqlDbType.Int, informacion.CodDepartamento, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@vCodRegion", SqlDbType.VarChar, informacion.CodDepartamento, ParameterDirection.Input));

                return VCEDBContext<LimiteCotizacionExtraOficial>.CallStoreProcedure(StoredProcedures.CO_CatalogosLimiteCotizacionExtraOficial, parameters, x => new LimiteCotizacionExtraOficial
                {
                    fechaInicial = DateTime.ParseExact(x.GetString(0), "yyyyMMdd", CultureInfo.InvariantCulture).ToString("yyyy-MM-dd"),
                    FechaFinal = DateTime.ParseExact(x.GetString(1), "yyyyMMdd", CultureInfo.InvariantCulture).ToString("yyyy-MM-dd")
                }).FirstOrDefault();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
        /// <summary>
        /// Omar Figueroa Flores
        /// 30-01-2019
        /// </summary>
        public List<LimiteCotizacionExtraOficial> getCodRegion(string glsRegion)
        {
            try
            {
                var parameters = new List<SqlParameter>();
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pClave", SqlDbType.VarChar, "GETCODREGION", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@glsRegion", SqlDbType.VarChar, glsRegion, ParameterDirection.Input));

                return VCEDBContext<LimiteCotizacionExtraOficial>.CallStoreProcedure(StoredProcedures.CO_CatalogosLimiteCotizacionExtraOficial, parameters, x => new LimiteCotizacionExtraOficial
                {
                    codRegion = x.GetInt32(0).ToString(),
                    FechaFinal = x.GetString(1)
                }).ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }
        /// <summary>
        /// Omar Figueroa Flores
        /// 30-01-2019
        /// </summary>
        public void EjecutarScript(string script)
        {
            try
            {
                List<Parametro> DatosCon = new List<Parametro>();
                string queryCon = "SELECT ClaveParametro, Parametro FROM Parametros WHERE DescripcionParametro = 'CONQA'";

                DatosCon = VCEDBContext<Parametro>.CallSelectStatement(queryCon, x => new Parametro
                {
                    ClaveParametro = x.GetString(0),
                    Elemento = x.GetString(1),
                }).ToList();

                string pass = (from passw in DatosCon where passw.ClaveParametro == "PASS" select passw.Elemento).FirstOrDefault();

                string ip = (from ips in DatosCon where ips.ClaveParametro == "IPSERVQA" select ips.Elemento).FirstOrDefault();

                string us = (from user in DatosCon where user.ClaveParametro == "USERSERQA" select user.Elemento).FirstOrDefault();
                string BD = (from bd in DatosCon where bd.ClaveParametro == "BD" select bd.Elemento).FirstOrDefault();

                string conexion = "Data Source=" + ip + "; Initial Catalog="+BD+";uid=" + us + ";pwd=" + pass + "";
                
                VCEDBContext<LimiteCotizacionExtraOficial>.CallSelectStatementConection(conexion, script, x => new LimiteCotizacionExtraOficial
                {
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
