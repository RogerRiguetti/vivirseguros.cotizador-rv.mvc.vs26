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
    public class CalculoCotizacionRepository
    {
        /// <summary>
        /// José Hernández Alvarado.
        /// 09-10-2018
        /// </summary>
        /// <param name="numArchivo">Número de archivo XML.</param>
        /// <param name="nomArch">Nombre del archivo XML.</param>
        /// <returns>Lista con cotizacines "Calculadas".</returns>
        public List<CalculoCotizaciones> RptCalculadas(int numArchivo, string nomArch)
        {
            try
            {
                var parameters = new List<SqlParameter>();
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pClave", SqlDbType.VarChar, "RTPCALCULADAS", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@vNumArchivo", SqlDbType.Int, numArchivo, ParameterDirection.Input));
                return VCEDBContext<CalculoCotizaciones>.CallStoreProcedure(StoredProcedures.CO_CatalogosCalcularAsignacionIntermediario, parameters, x => new CalculoCotizaciones
                {
                    Num_Cotizacion = x.GetString(0),
                    Num_Corr = x.GetInt32(1),
                    Num_Operacion = Convert.ToInt32(x.GetDecimal(2)),
                    CUSPP = x.GetString(3),
                    Tipo_Pension = x.GetString(23),
                    Tipo_Renta = x.GetString(21),
                    Years_Dif = x.GetInt32(6),
                    Modalidad = x.GetString(22),
                    Years_Gar = x.GetInt32(8),
                    Cob_Cony = x.GetString(9),
                    D_Crecer = x.GetString(10),
                    D_Gratif = x.GetString(11),
                    Moneda = x.GetString(12),
                    TIR = x.GetDecimal(13),
                    Renta_Esc = x.GetDecimal(14),
                    Tasa_Venta = x.GetDecimal(15),
                    Mto_Pension = x.GetDecimal(16),
                    Tasa_RT = x.GetDecimal(24),
                    Mto_PensionRT = x.GetDecimal(16) * 2,
                    Prima_Unica = (float)(x.GetDecimal(17)),
                    Perdida_Contable = x.GetDecimal(18),
                    Intermediario = x.GetString(19),
                    PRC_Com = x.GetDecimal(20),
                    Num_Archivo = numArchivo.ToString(),
                    Nom_Archivo = nomArch,
                    codTipReajuste = x.GetString(25),
                    tipCambio = x.GetDecimal(26),
                    Mto_Cic = x.GetDecimal(27),
                    Ind_SISCO = x.GetInt32(28),
                    Cod_TipRen = x.GetString(5),
                    Cod_TipPen = x.GetString(4),
                    Ind_Mej = "N"
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
        /// 09-10-2018
        /// </summary>
        /// <param name="numArchivo">Número de archivo XML.</param>
        /// <param name="nomArch">Nombre del archivo XML.</param>
        /// <returns>Lista con cotizacines "No Calculadas".</returns>
        public List<CalculoCotizaciones> RptNoCalculadas(int numArchivo, string nomArch)
        {
            try
            {
                var parameters = new List<SqlParameter>();
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pClave", SqlDbType.VarChar, "RTPNOCALCULADAS", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@vNumArchivo", SqlDbType.Int, numArchivo, ParameterDirection.Input));
                return VCEDBContext<CalculoCotizaciones>.CallStoreProcedure(StoredProcedures.CO_CatalogosCalcularAsignacionIntermediario, parameters, x => new CalculoCotizaciones
                {
                    Num_Cotizacion = x.GetString(0),
                    Num_Corr = x.GetInt32(1),
                    Num_Operacion = Convert.ToInt32(x.GetDecimal(2)),
                    CUSPP = x.GetString(3),
                    Tipo_Pension = x.GetString(19),
                    Tipo_Renta = x.GetString(17),
                    Years_Dif = x.GetInt32(6),
                    Modalidad = x.GetString(18),
                    Years_Gar = x.GetInt32(8),
                    Cob_Cony = "S",
                    D_Crecer = x.GetString(10),
                    D_Gratif = x.GetString(11),
                    Moneda = x.GetString(12),
                    Renta_Esc = x.GetDecimal(13),
                    Prima_Unica = (float)x.GetDecimal(14),
                    Intermediario = x.GetString(15),
                    Error_NoCotizadas = x.GetString(16),
                    Num_Archivo = numArchivo.ToString(),
                    Nom_Archivo = nomArch,
                    codTipReajuste = x.GetString(20),
                    MensajeErr = x.GetString(21),
                    Mto_Cic = x.GetDecimal(22)
                }).ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
        public string homologarMoneda(string moneda, string reajuste)
        {
            try
            {
                CalculoCotizaciones obj = new CalculoCotizaciones();
                var parameters = new List<SqlParameter>();
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pClave", SqlDbType.VarChar, "HOMOLOGARMONEDA", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@codMoneda", SqlDbType.VarChar, moneda, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@codTipReajuste", SqlDbType.VarChar, reajuste, ParameterDirection.Input));
                obj = VCEDBContext<CalculoCotizaciones>.CallStoreProcedure(StoredProcedures.CO_CatalogosCalcularAsignacionIntermediario, parameters, x => new CalculoCotizaciones
                {
                    Moneda = x.GetString(0)
                }).FirstOrDefault();
                return obj.Moneda;
            }
            catch (Exception)
            {
                return "";
            }
        }
        public List<CalculoCotizaciones> getBeneficiarios(int numArch)
        {
            try
            {
                var parameters = new List<SqlParameter>();
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pClave", SqlDbType.VarChar, "GETBEN", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@vNumArchivo", SqlDbType.Int, numArch, ParameterDirection.Input));
                return VCEDBContext<CalculoCotizaciones>.CallStoreProcedure(StoredProcedures.CO_CatalogosCalcularAsignacionIntermediario, parameters, x => new CalculoCotizaciones
                {
                    Num_Cotizacion = x.GetString(0),
                    Num_Corr = x.GetInt32(1),
                    Num_Operacion = Convert.ToInt32(x.GetDecimal(2)),
                    CUSPP = x.GetString(3),
                    Tipo_Pension = x.GetString(4),
                    Tipo_Renta = x.GetString(5),
                    Years_Dif = x.GetInt32(6),
                    Modalidad = x.GetString(7),
                    Years_Gar = x.GetInt32(8),
                    Cob_Cony = "S",
                    D_Crecer = x.GetString(10),
                    D_Gratif = x.GetString(11),
                    Moneda = x.GetString(12),
                    TIR = x.GetDecimal(13),
                    Renta_Esc = x.GetDecimal(14),
                    Tasa_Venta = x.GetDecimal(15),
                    Mto_Pension = x.GetDecimal(16),
                    Tasa_RT = x.GetDecimal(24),
                    Mto_PensionRT = x.GetDecimal(16) * 2,
                    Prima_Unica = (float)(x.GetDecimal(17)),
                    Perdida_Contable = x.GetDecimal(18),
                    Intermediario = x.GetString(19),
                    PRC_Com = x.GetDecimal(20),
                    Num_Archivo = numArch.ToString(),
                    codTipReajuste = x.GetString(25),
                    prc_Pension = x.GetDecimal(26)
                }).ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public List<CalculoCotizaciones> getBeneficiariosRutina(int numOpera)
        {
            try
            {
                var parameters = new List<SqlParameter>();
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pClave", SqlDbType.VarChar, "GETBENRUT", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pNumOpera", SqlDbType.Int, numOpera, ParameterDirection.Input));
                return VCEDBContext<CalculoCotizaciones>.CallStoreProcedure(StoredProcedures.CO_CatalogosCalcularAsignacionIntermediario, parameters, x => new CalculoCotizaciones
                {
                    Num_Cotizacion = x.GetString(0),
                    Num_Corr = x.GetInt32(1),
                    Num_Operacion = Convert.ToInt32(x.GetDecimal(2)),
                    CUSPP = x.GetString(3),
                    Tipo_Pension = x.GetString(4),
                    Tipo_Renta = x.GetString(5),
                    Years_Dif = x.GetInt32(6),
                    Modalidad = x.GetString(7),
                    Years_Gar = x.GetInt32(8),
                    Cob_Cony = "S",
                    D_Crecer = x.GetString(10),
                    D_Gratif = x.GetString(11),
                    Moneda = x.GetString(12),
                    TIR = x.GetDecimal(13),
                    Renta_Esc = x.GetDecimal(14),
                    Tasa_Venta = x.GetDecimal(15),
                    Mto_Pension = x.GetDecimal(16),
                    Tasa_RT = x.GetDecimal(24),
                    Mto_PensionRT = x.GetDecimal(16) * 2,
                    Prima_Unica = (float)(x.GetDecimal(17)),
                    Perdida_Contable = x.GetDecimal(18),
                    Intermediario = x.GetString(19),
                    PRC_Com = x.GetDecimal(20),
                    codTipReajuste = x.GetString(25),
                    prc_Pension = x.GetDecimal(26)
                }).ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        #region Consultas para Excel de Solicitudes Calculadas y No Calculadas.
        /// <summary>
        /// José Hernández Alvarado.
        /// 05-08-2019
        /// Método que consulta en BD Solicitudes Calculadas.
        /// </summary>
        /// <param name="numArchivo">Número del archivo cargado.</param>
        /// <returns>Lsita con registros de Solicitudes Calculadas.</returns>
        public List<AsignacionIntermediario> ExportarExcelCalculadas(int numArchivo)
        {
            var parameters = new List<SqlParameter>();
            parameters.Add(VCEDBContext<RowAffected>.AddParams("@pClave", SqlDbType.VarChar, "CONEXCELCALCULADAS", ParameterDirection.Input));
            parameters.Add(VCEDBContext<RowAffected>.AddParams("@vNumArchivo", SqlDbType.Int, numArchivo, ParameterDirection.Input));

            return VCEDBContext<AsignacionIntermediario>.CallStoreProcedure(StoredProcedures.CO_CatalogosCalcularAsignacionIntermediario, parameters, x => new AsignacionIntermediario
            {
                Num_Orden = Convert.ToInt32(x.GetInt64(0)),
                Num_Cot = x.GetString(1),
                Num_Correlativo = x.GetInt32(2),
                Num_Operacion = Convert.ToInt32(x.GetDecimal(3)),
                CUSPP = x.GetString(4),
                Tipo_Pension = x.GetString(5),
                Tipo_Renta = x.GetString(6),
                MesesDif = x.GetInt32(7),
                Modalidad = x.GetString(8),
                MesesGar = x.GetInt32(9),
                CobCony = x.GetInt32(10),
                Cod_DerCre = x.GetString(11),
                Cod_DerGra = x.GetString(12),
                Cod_Moneda = x.GetString(13),
                Prc_MinimoTir = x.GetDecimal(14), //TasaTIR
                Prc_RentaEsc = (double)x.GetDecimal(15),
                TasaV = x.GetDecimal(16), //Prc_TasaVta
                Prc_Pension = x.GetDecimal(17), //Mto_Pension
                Prc_TasaRPRT = (double)x.GetDecimal(18),
                Mto_PensionRT = (double)x.GetDecimal(19),
                Prima_Unica = (double)x.GetDecimal(20),
                Prc_PerCon = (double)x.GetDecimal(21),
                Intermediario = x.GetString(22),
                Prc_CorCom = (double)x.GetDecimal(23),
                Ind_Mej = x.GetString(24),
                Gls_Region = x.GetString(25),
                CIC = (double)x.GetDecimal(26),
                DepartamentoAsignado = x.GetString(27),
                Asesor = x.GetString(28),
                Supervisor = x.GetString(29),
            }).ToList();
        }

        /// <summary>
        /// José Hernández Alvarado.
        /// 05-08-2019
        /// Método que consulta en BD Solicitudes No Calculadas.
        /// </summary>
        /// <param name="numArchivo">Número del archivo cargado.</param>
        /// <returns>Lsita con registros de Solicitudes No Calculadas.</returns>
        public List<AsignacionIntermediario> ExportarExcelNoCalculadas(int numArchivo)
        {
            var parameters = new List<SqlParameter>();
            parameters.Add(VCEDBContext<RowAffected>.AddParams("@pClave", SqlDbType.VarChar, "CONEXCELNOCALCULADAS", ParameterDirection.Input));
            parameters.Add(VCEDBContext<RowAffected>.AddParams("@vNumArchivo", SqlDbType.Int, numArchivo, ParameterDirection.Input));

            return VCEDBContext<AsignacionIntermediario>.CallStoreProcedure(StoredProcedures.CO_CatalogosCalcularAsignacionIntermediario, parameters, x => new AsignacionIntermediario
            {
                Num_Cot = x.GetString(0),
                Num_Correlativo = x.GetInt32(1),
                Num_Operacion = Convert.ToInt32(x.GetDecimal(2)),
                CUSPP = x.GetString(3),
                Tipo_Pension = x.GetString(4),
                Tipo_Renta = x.GetString(5),
                MesesDif = x.GetInt32(6),
                Modalidad = x.GetString(7),
                MesesGar = x.GetInt32(8),
                CIC = (double)x.GetDecimal(9),
                Cod_Moneda = x.GetString(10),
                Prc_RentaEsc = (double)x.GetDecimal(11),
                Prima_Unica = (double)x.GetDecimal(12),
                Intermediario = x.GetString(13),
                Cod_Rechazo = x.GetString(14),
                Error_Descrip = x.GetString(15),
                Gls_Region = x.GetString(16),
                DepartamentoAsignado = x.GetString(17),
                Asesor = x.GetString(18),
                Supervisor = x.GetString(19),
            }).ToList();
        }
        #endregion
    }
}
