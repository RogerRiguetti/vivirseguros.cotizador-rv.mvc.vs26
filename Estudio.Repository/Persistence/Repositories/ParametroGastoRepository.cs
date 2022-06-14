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
    public class ParametroGastoRepository
    {
        /// <summary>
        /// Consulta y retorna los periodos para el tipo de moneda.
        /// José Hernández Alvarado.
        /// 31-08-2018
        /// </summary>
        /// <param name="vlMoneda"></param>
        /// <param name="vlReajuste"></param>
        /// <returns>Retorna lista con periodos de la BD.</returns>
        public List<Periodo> ListaPeriodos(string vlMoneda, int vlReajuste)
        {
            try
            {
                var parameters = new List<SqlParameter>();
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pClave", SqlDbType.VarChar, "PERIODOS", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pValMoneda", SqlDbType.VarChar, vlMoneda, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pValReajuste", SqlDbType.Int, vlReajuste, ParameterDirection.Input));

                return VCEDBContext<Periodo>.CallStoreProcedure(StoredProcedures.CO_CatalogoParametrosGasto, parameters, x => new Periodo
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
        /// Consulta los valores para llenar los campos de la vista.
        /// José Hernández Alvarado.
        /// 04-09-2018
        /// </summary>
        /// <param name="strFecIni">Fecha de Inicio de Vigencia.</param>
        /// <param name="vlMoneda">Tipo de Moneda.</param>
        /// <param name="vlReajuste">Valor de reajuste.</param>
        /// <returns>Valores del periodo seleccionado o ingresado.</returns>
        public ParametroGasto BuscarVigencia(DateTime strFecIni, string vlMoneda, int vlReajuste)
        {
            try
            {
                var parameters = new List<SqlParameter>();
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pClave", SqlDbType.VarChar, "BUSCARVIGENCIA", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pFecInicio", SqlDbType.VarChar, strFecIni.ToString("yyyyMMdd"), ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pValMoneda", SqlDbType.VarChar, vlMoneda, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pValReajuste", SqlDbType.Int, vlReajuste, ParameterDirection.Input));

                return VCEDBContext<ParametroGasto>.CallStoreProcedure(StoredProcedures.CO_CatalogoParametrosGasto, parameters, x => new ParametroGasto
                {
                    FechaTermino = DateTime.ParseExact(x.GetString(0), "yyyyMMdd", CultureInfo.InvariantCulture).ToString("yyyy-MM-dd"),
                    Mto_GastosAdmin = x.GetDecimal(1),
                    PRC_GastosCtrlSuper = x.GetDecimal(2),
                    Mto_GastosEmi = x.GetDecimal(3),
                    Cto_Capital = x.GetDecimal(4),
                    PRC_Endeudamiento = x.GetDecimal(5),
                    FechaInicial = strFecIni.ToString("yyyy-MM-dd")
                }).FirstOrDefault();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public ParametroGasto GrabarParametro(string vlMoneda, int vlReajuste, DateTime strFecIni, DateTime strFecFin, decimal gastosCS, decimal gastosA, decimal gastosE, decimal ctoCapital, decimal nivelE, string usuario, string clave)
        {
            try
            {
                DateTime fecha = DateTime.Now;

                var parameters = new List<SqlParameter>();
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pClave", SqlDbType.VarChar, clave, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pValMoneda", SqlDbType.VarChar, vlMoneda, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pValReajuste", SqlDbType.Int, vlReajuste, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pFecInicio", SqlDbType.VarChar, strFecIni.ToString("yyyyMMdd"), ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pFecTermino", SqlDbType.VarChar, strFecFin.ToString("yyyyMMdd"), ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pGastoCS", SqlDbType.Decimal, gastosCS, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pGastoA", SqlDbType.Decimal, gastosA, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pGastoE", SqlDbType.Decimal, gastosE, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pCtoCap", SqlDbType.Decimal, ctoCapital, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pEndeuda", SqlDbType.Decimal, nivelE, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pUsuario", SqlDbType.VarChar, usuario, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pFecCrea", SqlDbType.VarChar, fecha.ToString("yyyyMMdd"), ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pHoraCrea", SqlDbType.VarChar, fecha.ToString("hhmmss"), ParameterDirection.Input));

                return VCEDBContext<ParametroGasto>.CallStoreProcedure(StoredProcedures.CO_CatalogoParametrosGasto, parameters, x => new ParametroGasto
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

        public ParametroGasto EliminarParametro(DateTime strFecIni, DateTime strFecFin, string vlMoneda, int vlReajuste, string clave)
        {
            try
            {
                var parameters = new List<SqlParameter>();
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pClave", SqlDbType.VarChar, clave, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pFecInicio", SqlDbType.VarChar, strFecIni.ToString("yyyyMMdd"), ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pFecTermino", SqlDbType.VarChar, strFecFin.ToString("yyyyMMdd"), ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pValMoneda", SqlDbType.VarChar, vlMoneda, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pValReajuste", SqlDbType.Int, vlReajuste, ParameterDirection.Input));

                return VCEDBContext<ParametroGasto>.CallStoreProcedure(StoredProcedures.CO_CatalogoParametrosGasto, parameters, x => new ParametroGasto
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
        /// Consulta y retorna el valor de la Tasa de Mercado.
        /// José Hernández Alvarado.
        /// 04-09-2018
        /// </summary>
        /// <param name="vlMoneda">Tipo de moneda.</param>
        /// <param name="vlReajuste">Valor de reajuste.</param>
        /// <returns>Valor de Tasa de Mercado</returns>
        public ParametroGasto ConsultarTM(string vlMoneda, int vlReajuste)
        {
            try
            {
                //Consulta el año máximo y lo retorna para poder consultar la tasa.
                var parameters = new List<SqlParameter>();
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pClave", SqlDbType.VarChar, "YEARTM", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pValMoneda", SqlDbType.VarChar, vlMoneda, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pValReajuste", SqlDbType.Int, vlReajuste, ParameterDirection.Input));

                ParametroGasto Year = new ParametroGasto();
                Year = VCEDBContext<ParametroGasto>.CallStoreProcedure(StoredProcedures.CO_CatalogoParametrosGasto, parameters, x => new ParametroGasto
                {
                    YearTM = x.GetInt32(0)
                }).FirstOrDefault();

                if (Year == null)
                {
                    Year = new ParametroGasto();
                    Year.PRC_TasaMercado = 0;
                    return Year;
                }
                else
                {
                    
                    string mes = DateTime.Now.Month.ToString();//Obtener el mes del día actual.

                    //Consulta el valor de la tasa de mercado.
                    parameters = new List<SqlParameter>();
                    parameters.Add(VCEDBContext<RowAffected>.AddParams("@pClave", SqlDbType.VarChar, "MESTM", ParameterDirection.Input));
                    parameters.Add(VCEDBContext<RowAffected>.AddParams("@vMesTM", SqlDbType.VarChar, "PRC_MES" + mes, ParameterDirection.Input));
                    parameters.Add(VCEDBContext<RowAffected>.AddParams("@pValMoneda", SqlDbType.VarChar, vlMoneda, ParameterDirection.Input));
                    parameters.Add(VCEDBContext<RowAffected>.AddParams("@pValReajuste", SqlDbType.Int, vlReajuste, ParameterDirection.Input));
                    parameters.Add(VCEDBContext<RowAffected>.AddParams("@pYearTM", SqlDbType.Int, Year.YearTM, ParameterDirection.Input));
                    ParametroGasto valorTM = new ParametroGasto();
                    valorTM = VCEDBContext<ParametroGasto>.CallStoreProcedure(StoredProcedures.CO_CatalogoParametrosGasto, parameters, x => new ParametroGasto
                    {
                        PRC_TasaMercado = x.GetDecimal(0)
                    }).FirstOrDefault();

                    return valorTM;
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Consulta y retorna el Impuesto de la Renta.
        /// José Hernández Alvarado.
        /// 05-09-2018
        /// </summary>
        /// <param name="strFecIni">Fecha de inicio de vigencia.</param>
        /// <returns>Retorna el valor del impuesto de la renta.</returns>
        public ParametroGasto ConsultarIR(DateTime strFecIni)
        {
            try
            {
                var parameters = new List<SqlParameter>();
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pClave", SqlDbType.VarChar, "IMPUESTORENTA", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pFecInicio", SqlDbType.VarChar, strFecIni.ToString("yyyyMMdd"), ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pValMoneda", SqlDbType.VarChar, "", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pValReajuste", SqlDbType.Int, 0, ParameterDirection.Input));

                return VCEDBContext<ParametroGasto>.CallStoreProcedure(StoredProcedures.CO_CatalogoParametrosGasto, parameters, x => new ParametroGasto
                {
                    PRC_ImpuestoRenta = x.GetDecimal(0)
                }).FirstOrDefault();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public List<ParametroGasto> ConsultaRptPG(string vlMoneda, string strMoneda, int vlReajuste, DateTime strFecIni)
        {
            try
            {
                var parameters = new List<SqlParameter>();
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pClave", SqlDbType.VarChar, "REPORTEPG", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pValMoneda", SqlDbType.VarChar, vlMoneda, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pValReajuste", SqlDbType.Int, vlReajuste, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pFecInicio", SqlDbType.VarChar, strFecIni.ToString("yyyyMMdd"), ParameterDirection.Input));

                return VCEDBContext<ParametroGasto>.CallStoreProcedure(StoredProcedures.CO_CatalogoParametrosGasto, parameters, x => new ParametroGasto
                {
                    FechaInicial = DateTime.ParseExact(x.GetString(0), "yyyyMMdd", CultureInfo.InvariantCulture).ToString("dd/MM/yyyy"),
                    FechaTermino = DateTime.ParseExact(x.GetString(1), "yyyyMMdd", CultureInfo.InvariantCulture).ToString("dd/MM/yyyy"),
                    Mto_GastosAdmin = x.GetDecimal(2),
                    Mto_GastosEmi = x.GetDecimal(3),
                    PRC_GastosCtrlSuper = x.GetDecimal(4),
                    PRC_Endeudamiento = x.GetDecimal(5),
                    Moneda = strMoneda
                }).ToList();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public ParametroGasto ConsultarGastos()
        {
            try
            {
                List<ParametroGasto> lista = new List<ParametroGasto>();
                var parameters = new List<SqlParameter>();
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pClave", SqlDbType.VarChar, "CONSULTARGASTOS", ParameterDirection.Input));

                lista = VCEDBContext<ParametroGasto>.CallStoreProcedure(StoredProcedures.CO_CatalogoParametrosGasto, parameters, x => new ParametroGasto
                {
                    Cod_tipren = Convert.ToInt32(x.GetString(0)),
                    PrccomS1 = x.GetDecimal(1),
                    Prcfaclab1 = x.GetDecimal(2)
                }).ToList();

                ParametroGasto datos = new ParametroGasto();
                foreach (var item in lista)
                {
                    if (item.Cod_tipren == 1)
                    {
                        datos.PrccomS1 = item.PrccomS1;
                        datos.Prcfaclab1 = item.Prcfaclab1;
                    }
                    else
                    {
                        datos.PrccomS2 = item.PrccomS1;
                        datos.Prcfaclab2 = item.Prcfaclab1;
                    }
                }

                return datos;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public ParametroGasto GuardarOtrosGastos(decimal COMSUP1, decimal COMSUP2, decimal PRCFAC1, decimal PRCFAC2)
        {
            try
            {

                var parameters = new List<SqlParameter>();
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pClave", SqlDbType.VarChar, "GUARDAROTROSGASTOS", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pCOMSUP1", SqlDbType.Decimal, COMSUP1, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pCOMSUP2", SqlDbType.Decimal, COMSUP2, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pPRCFAC1", SqlDbType.Decimal, PRCFAC1, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pPRCFAC2", SqlDbType.Decimal, PRCFAC2, ParameterDirection.Input));

                return VCEDBContext<ParametroGasto>.CallStoreProcedure(StoredProcedures.CO_CatalogoParametrosGasto, parameters, x => new ParametroGasto
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
