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
   public class TasaRentabilidadRepository
    {

        #region Catalogos
        public List<beRentabilidad> ListaPeriodos(string codigoMoneda, string reajuste)
        {
            try
            {
                var parameters = new List<SqlParameter>();
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pClave", SqlDbType.VarChar, "TIPOSPERIODOS", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pCodigoMoneda", SqlDbType.VarChar, codigoMoneda, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pValorReajuste", SqlDbType.VarChar, reajuste, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pFecIni", SqlDbType.VarChar, "", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pNumAnno", SqlDbType.Int, 0, ParameterDirection.Input));

                return VCEDBContext<beRentabilidad>.CallStoreProcedure(StoredProcedures.CO_ConsultasTasaRentabilidad, parameters, x => new beRentabilidad
                {
                    IDPeriodo = DateTime.ParseExact(x.GetString(0), "yyyyMMdd", CultureInfo.InvariantCulture).ToString("yyyy-MM-dd"),
                    Periodo = DateTime.ParseExact(x.GetString(0), "yyyyMMdd", CultureInfo.InvariantCulture).ToString("dd/MM/yyyy") + " * " + DateTime.ParseExact(x.GetString(1), "yyyyMMdd", CultureInfo.InvariantCulture).ToString("dd/MM/yyyy")
                }).ToList();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public List<beRentabilidad> ListaRentabilidad(string codigoMoneda, string reajuste, DateTime fecIni)
        {
            try
            {
                var parameters = new List<SqlParameter>();
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pClave", SqlDbType.VarChar, "TIPOSRENTABILIDAD", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pCodigoMoneda", SqlDbType.VarChar, codigoMoneda, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pValorReajuste", SqlDbType.VarChar, reajuste, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pFecIni", SqlDbType.VarChar, fecIni.ToString("yyyyMMdd"), ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pNumAnno", SqlDbType.Int, 0, ParameterDirection.Input));

                return VCEDBContext<beRentabilidad>.CallStoreProcedure(StoredProcedures.CO_ConsultasTasaRentabilidad, parameters, x => new beRentabilidad
                {
                    NUM_ANNO = x.GetInt32(0),
                    PRC_TASAREN = (double)x.GetDecimal(1)
                }).ToList();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public beRentabilidad ConsultaTasaRen(string codigoMoneda, string reajuste, DateTime fecIni, int numAnno)
        {
            try
            {
                var parameters = new List<SqlParameter>();
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pClave", SqlDbType.VarChar, "CONSULTATASAREN", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pCodigoMoneda", SqlDbType.VarChar, codigoMoneda, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pValorReajuste", SqlDbType.VarChar, reajuste, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pFecIni", SqlDbType.VarChar, fecIni.ToString("yyyyMMdd"), ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pNumAnno", SqlDbType.Int, numAnno, ParameterDirection.Input));

                return VCEDBContext<beRentabilidad>.CallStoreProcedure(StoredProcedures.CO_ConsultasTasaRentabilidad, parameters, x => new beRentabilidad
                {
                    NUM_ANNO = x.GetInt32(0),
                    PRC_TASAREN = (double)x.GetDecimal(1)
                }).FirstOrDefault();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public beRentabilidad ConsultaFechasInicio(string clave, DateTime fechaInicio, DateTime fechaTermino, string codigoMoneda, string reajuste)
        {
            try
            {
                var parameters = new List<SqlParameter>();
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pClave", SqlDbType.VarChar, clave, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pCodigoMoneda", SqlDbType.VarChar, codigoMoneda, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pValorReajuste", SqlDbType.VarChar, reajuste, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pFecIni", SqlDbType.VarChar, fechaInicio.ToString("yyyyMMdd"), ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pFechaTermino", SqlDbType.VarChar, fechaTermino.ToString("yyyyMMdd"), ParameterDirection.Input));

                return VCEDBContext<beRentabilidad>.CallStoreProcedure(StoredProcedures.CO_ConsultasTasaRentabilidad, parameters, x => new beRentabilidad
                {
                    FEC_INIVIG = DateTime.ParseExact(x.GetString(0), "yyyyMMdd", CultureInfo.InvariantCulture).ToString("yyyy-MM-dd")
                }).FirstOrDefault();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void RegistrarModificar(string clave, beRentabilidad tasa)
        {
            try
            {
                DateTime fecha = DateTime.Now;
                var parameters = new List<SqlParameter>();
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pClave", SqlDbType.VarChar, clave, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pMoneda", SqlDbType.VarChar, tasa.COD_MONEDA, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pCodRea", SqlDbType.VarChar, tasa.COD_TIPREAJUSTE, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pFecIni", SqlDbType.VarChar, tasa.FEC_INIVIG, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pFecTer", SqlDbType.VarChar, tasa.FEC_TERVIG, ParameterDirection.Input));

                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pNumAnno", SqlDbType.Int, tasa.NUM_ANNO, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pTasaRen", SqlDbType.Decimal, tasa.PRC_TASAREN, ParameterDirection.Input));

                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pUsuario", SqlDbType.VarChar, tasa.Usuario, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pFecha", SqlDbType.VarChar, fecha.ToString("yyyyMMdd"), ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pHora", SqlDbType.VarChar, fecha.ToString("hhmmss"), ParameterDirection.Input));

                VCEDBContext<DataTable>.CallStoreProcedureDt(StoredProcedures.CO_CatalogoTasaRentabilidad, parameters);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<beRentabilidad> ConsultaRpt(string codMoneda, int reajuste, string moneda, DateTime fechaInicial)
        {
            try
            {
                var parameters = new List<SqlParameter>();
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pClave", SqlDbType.VarChar, "REPORTE", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pCodigoMoneda", SqlDbType.VarChar, codMoneda, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pValorReajuste", SqlDbType.VarChar, reajuste, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pFecIni", SqlDbType.VarChar, fechaInicial.ToString("yyyyMMdd"), ParameterDirection.Input));

                return VCEDBContext<beRentabilidad>.CallStoreProcedure(StoredProcedures.CO_ConsultasTasaRentabilidad, parameters, x => new beRentabilidad
                {
                    COD_MONEDA= moneda,
                    NUM_ANNO = x.GetInt32(0),
                    PRC_TASAREN = (double)x.GetDecimal(1),
                    FEC_INIVIG = DateTime.ParseExact(x.GetString(2), "yyyyMMdd", CultureInfo.InvariantCulture).ToString("dd/MM/yyyy"),
                    FEC_TERVIG = DateTime.ParseExact(x.GetString(3), "yyyyMMdd", CultureInfo.InvariantCulture).ToString("dd/MM/yyyy")
                   
                }).ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void EliminarTasaRentabilidad(DateTime fechaInicio, string codigoMoneda, string reajuste)
        {
            try
            {
                var parameters = new List<SqlParameter>();
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pClave", SqlDbType.VarChar, "ELIMINAR", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pMoneda", SqlDbType.VarChar, codigoMoneda, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pCodRea", SqlDbType.VarChar, reajuste, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pFecIni", SqlDbType.VarChar, fechaInicio.ToString("yyyyMMdd"), ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pFecTer", SqlDbType.VarChar, "", ParameterDirection.Input));

                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pNumAnno", SqlDbType.Int, 0, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pTasaRen", SqlDbType.Decimal, 0, ParameterDirection.Input));

                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pUsuario", SqlDbType.VarChar, "", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pFecha", SqlDbType.VarChar, "", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pHora", SqlDbType.VarChar,"", ParameterDirection.Input));

                VCEDBContext<DataTable>.CallStoreProcedureDt(StoredProcedures.CO_CatalogoTasaRentabilidad, parameters);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
        #endregion

    }
}
