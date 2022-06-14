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
    public class ValoresMonedaRepository
    {
        /// <summary>
        /// José Hernández Alvarado.
        /// 2018-08-21
        /// </summary>
        /// <param name="vlMoneda">Valor de moneda seleccionado en combo</param>
        /// <param name="fecVM"></param>
        /// <returns>Retorna lista de registros en BD para llenar la tabla</returns>
        public List<ValoresMoneda> ListaValoresVM(string vlMoneda)
        {
            try
            {
                var parameters = new List<SqlParameter>();
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pClave", SqlDbType.VarChar, "BUSCARVALOR", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pTipoMoneda", SqlDbType.VarChar, vlMoneda, ParameterDirection.Input));

                return VCEDBContext<ValoresMoneda>.CallStoreProcedure(StoredProcedures.CO_ValoresMoneda, parameters, x => new ValoresMoneda
                {
                    FechaVM = DateTime.ParseExact(x.GetString(0), "yyyyMMdd", CultureInfo.InvariantCulture).ToString("dd/MM/yyyy"),
                    ValorVM = x.GetDecimal(1)
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
        /// 2018-08-22
        /// </summary>
        /// <param name="vlMoneda">Valor de la moneda seleccionada en el combo</param>
        /// <param name="fecVM">Fecha del registro(fila) elegido en la tabla</param>
        /// <returns>Consulta si existe el registro en base de datos</returns>
        public ValoresMoneda BuscarValorVM(string vlMoneda, string fecVM)
        {
            try
            {
                var parameters = new List<SqlParameter>();
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pClave", SqlDbType.VarChar, "BUSCARREG", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pTipoMoneda", SqlDbType.VarChar, vlMoneda, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pFechaVM", SqlDbType.VarChar, fecVM, ParameterDirection.Input));

                return VCEDBContext<ValoresMoneda>.CallStoreProcedure(StoredProcedures.CO_ValoresMoneda, parameters, x => new ValoresMoneda
                {
                    FechaVM = DateTime.ParseExact(x.GetString(0), "yyyyMMdd", CultureInfo.InvariantCulture).ToString("dd/MM/yyyy"),
                    ValorVM = x.GetDecimal(1)
                }).FirstOrDefault();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        /// <summary>
        /// José Hernández Alvarado.
        /// 2018-08-23
        /// Modifica y guarda el valor de la moneda modificado por el usuario.
        /// </summary>
        /// <param name="vlMoneda">Valor de la moneda</param>
        /// <param name="fecVM">Fecha del registro a modificar</param>
        /// <param name="valorM">Valor de la moneda a modificar</param>
        /// <returns></returns>
        public ValoresMoneda SaveValorVM(string vlMoneda, string fecVM, decimal valorM)
        {
            try
            {
                var parameters = new List<SqlParameter>();
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pClave", SqlDbType.VarChar, "MODIFICAVM", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pTipoMoneda", SqlDbType.VarChar, vlMoneda, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pValorVM", SqlDbType.Decimal, valorM, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pFechaVM", SqlDbType.VarChar, fecVM, ParameterDirection.Input));

                return VCEDBContext<ValoresMoneda>.CallStoreProcedure(StoredProcedures.CO_ValoresMoneda, parameters, x => new ValoresMoneda
                {
                    FechaVM = DateTime.ParseExact(x.GetString(0), "yyyyMMdd", CultureInfo.InvariantCulture).ToString("dd/MM/yyyy"),
                    ValorVM = x.GetDecimal(1)
                }).FirstOrDefault();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        /// <summary>
        /// José Hernández Alvarado.
        /// 2018-08-23
        /// Inserta registros nuevos en la BD.
        /// </summary>
        /// <param name="vlMoneda">Valor de moneda</param>
        /// <param name="fecVM">Fecha del registro seleccionado</param>
        /// <param name="valorM">Valor de moneda</param>
        /// <returns></returns>
        public ValoresMoneda InsertValorVM(string vlMoneda, string fecVM, decimal valorM)
        {
            try
            {
                var parameters = new List<SqlParameter>();
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pClave", SqlDbType.VarChar, "INSERTAVM", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pTipoMoneda", SqlDbType.VarChar, vlMoneda, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pValorVM", SqlDbType.Decimal, valorM, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pFechaVM", SqlDbType.VarChar, fecVM, ParameterDirection.Input));

                return VCEDBContext<ValoresMoneda>.CallStoreProcedure(StoredProcedures.CO_ValoresMoneda, parameters, x => new ValoresMoneda
                {
                    FechaVM = DateTime.ParseExact(x.GetString(0), "yyyyMMdd", CultureInfo.InvariantCulture).ToString("dd/MM/yyyy"),
                    ValorVM = x.GetDecimal(1)
                }).FirstOrDefault();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        /// <summary>
        /// José Hernández Alvarado.
        /// 2018-08-24
        /// Elimina registros de la BD.
        /// </summary>
        /// <param name="vlMoneda">Valor de la moneda.</param>
        /// <param name="fecVM">Fecha del registro a eliminar.</param>
        /// <returns></returns>
        public ValoresMoneda DeleteValorVM(string vlMoneda, string fecVM)
        {
            try
            {
                var parameters = new List<SqlParameter>();
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pClave", SqlDbType.VarChar, "ELIMINARVM", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pTipoMoneda", SqlDbType.VarChar, vlMoneda, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pFechaVM", SqlDbType.VarChar, fecVM, ParameterDirection.Input));

                return VCEDBContext<ValoresMoneda>.CallStoreProcedure(StoredProcedures.CO_ValoresMoneda, parameters, x => new ValoresMoneda
                {
                    FechaVM = DateTime.ParseExact(x.GetString(0), "yyyyMMdd", CultureInfo.InvariantCulture).ToString("dd/MM/yyyy"),
                    ValorVM = x.GetDecimal(1)
                }).FirstOrDefault();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public List<ValoresMoneda> ConsultaRptVM(string vlMoneda, string strMoneda)
        {
            try
            {
                var parameters = new List<SqlParameter>();
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pClave", SqlDbType.VarChar, "BUSCARVALOR", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pTipoMoneda", SqlDbType.VarChar, vlMoneda, ParameterDirection.Input));

                return VCEDBContext<ValoresMoneda>.CallStoreProcedure(StoredProcedures.CO_ValoresMoneda, parameters, x => new ValoresMoneda
                {
                    FechaVM = DateTime.ParseExact(x.GetString(0), "yyyyMMdd", CultureInfo.InvariantCulture).ToString("dd/MM/yyyy"),
                    ValorVM = x.GetDecimal(1),
                    Moneda = strMoneda
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
