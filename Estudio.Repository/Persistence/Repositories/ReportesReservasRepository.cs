using Estudio.Repository.Core.Domain;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using log4net;
using System.Reflection;
using log4net.Config;
using System.Text;
using System.Threading.Tasks;

namespace Estudio.Repository.Persistence.Repositories
{
    public class ReportesReservasRepository
    {
        //Globales
        private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Consultar información en base de datos para llenar las celdas del archivo(Flujos de Pasivos).
        /// José Hernández Alvarado
        /// 28-10-2019
        /// </summary>
        /// <param name="pFechaPeriodo">Fecha del periodo abierto o periodo a buscar.</param>
        /// <param name="pBandera">Parámetro para ubicar script a ejectutar en store procedure en base de datos.</param>
        /// <returns>Lista con información consultada</returns>
        public List<ReportesReservas> ConsultarFlujosPasivos (string pFechaPeriodo, string pBandera)
        {
            XmlConfigurator.Configure();
            try
            {
                var parameters = new List<SqlParameter>();
                parameters.Add(SRVDBContext<RowAffected>.AddParams("@pBandera", SqlDbType.VarChar, pBandera, ParameterDirection.Input));
                parameters.Add(SRVDBContext<RowAffected>.AddParams("@pFechaCalculo", SqlDbType.VarChar, pFechaPeriodo.Substring(0, 6), ParameterDirection.Input));
                parameters.Add(SRVDBContext<RowAffected>.AddParams("@pFechaPeriodo", SqlDbType.VarChar, pFechaPeriodo, ParameterDirection.Input));

                return SRVDBContext<ReportesReservas>.CallStoreProcedure(StoredProcedures.CR_ConsultasReportesReservas, parameters, x => new ReportesReservas
                {
                    AnoMes = x.IsDBNull(0) ? "" : x.GetString(0),
                    Fila = Convert.ToInt32(x.GetDecimal(1)),
                    Ano = x.IsDBNull(2) ? 0 : Convert.ToInt32(x.GetString(2)),
                    Mes = x.IsDBNull(3) ? 0 : Convert.ToInt32(x.GetString(3)),
                    Jubilacion_SolesIndexados = (double)x.GetDecimal(4),
                    Jubilacion_Dolares = (double)x.GetDecimal(5),
                    Jubilacion_SolesAjustados = (double)x.GetDecimal(6),
                    Jubilacion_GastosSepelio = (double)x.GetDecimal(7),
                    Sobrevivencia_SolesIndexados = (double)x.GetDecimal(8),
                    Sobrevivencia_Dolares = (double)x.GetDecimal(9),
                    Sobrevivencia_SolesAjustados = (double)x.GetDecimal(10),
                    Invalidez_SolesIndexados = (double)x.GetDecimal(11),
                    Invalidez_Dolares = (double)x.GetDecimal(12),
                    Invalidez_SolesAjustados = (double)x.GetDecimal(13),
                    Invalidez_GastosSepelio = (double)x.GetDecimal(14),
                    GastosOperacion = x.GetInt32(15)
                }).ToList();
            }
            catch (Exception ex)
            {
                _log.Info("Error al consultar información para el reporte de Flujos de Pasivos(" + pBandera + "): " + ex.Message);
                return null;
            }
        }

        /// <summary>
        /// Consultar información en Base de datos para llenar el archivo de texto(Flujos de Pasivos).
        /// José Hernández Alvarado
        /// 28-10-2019
        /// </summary>
        /// <param name="pFechaPeriodo">Fecha de periodo abierto o periodo a buscar.</param>
        /// <param name="pBandera">Parámetro para ubicar script a ejectutar en store procedure en base de datos.</param>
        /// <returns>Lista con información a insertar en el archivo</returns>
        public List<ReportesReservas> ConsultarDatosArchivoTxt(string pFechaPeriodo, string pBandera)
        {
            XmlConfigurator.Configure();
            try
            {
                var parameters = new List<SqlParameter>();
                parameters.Add(SRVDBContext<RowAffected>.AddParams("@pBandera", SqlDbType.VarChar, pBandera, ParameterDirection.Input));
                parameters.Add(SRVDBContext<RowAffected>.AddParams("@pFechaCalculo", SqlDbType.VarChar, pFechaPeriodo.Substring(0, 6), ParameterDirection.Input));
                parameters.Add(SRVDBContext<RowAffected>.AddParams("@pFechaPeriodo", SqlDbType.VarChar, pFechaPeriodo, ParameterDirection.Input));

                _log.Info("Se consultará la información para generación del archivo de texto, por favor espere...");
                return SRVDBContext<ReportesReservas>.CallStoreProcedure(StoredProcedures.CR_ConsultasReportesReservas, parameters, x => new ReportesReservas
                {
                    LineaArchivoTxt = x.GetString(1)
                }).ToList();
            }
            catch (Exception ex)
            {
                _log.Info("Error al consultar información para el archivo de texto(" + pBandera + "): " + ex.Message);
                return null;
            }
        }

        /// <summary>
        /// Consultar información en Base de Datos para llenar las celdas del archivo(Resumen Adecuación).
        /// José Hernández Alvarado.
        /// 28-10-2019
        /// </summary>
        /// <param name="pFechaPeriodo">Fecha de periodo abierto o periodo a buscar.</param>
        /// <param name="pBandera">Parámetro para ubicar script a ejectutar en store procedure en base de datos.</param>
        /// <returns>Lista con información consultada</returns>
        public List<Reservas> ConsultarReporteResumenAdecuacion(string pFechaPeriodo, string pBandera)
        {
            XmlConfigurator.Configure();
            try
            {
                var parameters = new List<SqlParameter>();
                parameters.Add(SRVDBContext<RowAffected>.AddParams("@pBandera", SqlDbType.VarChar, pBandera, ParameterDirection.Input));
                parameters.Add(SRVDBContext<RowAffected>.AddParams("@pFechaCalculo", SqlDbType.VarChar, pFechaPeriodo.Substring(0, 6), ParameterDirection.Input));
                parameters.Add(SRVDBContext<RowAffected>.AddParams("@pFechaPeriodo", SqlDbType.VarChar, pFechaPeriodo, ParameterDirection.Input));

                return SRVDBContext<Reservas>.CallStoreProcedure(StoredProcedures.CR_ConsultasReportesReservas, parameters, x => new Reservas
                {
                    Num_Poliza = x.GetString(0),
                    Renta = x.GetString(1),
                    Moneda = x.GetString(2),
                    Mtos_TotalesRes = (double)x.GetDecimal(3)
                }).ToList();
            }
            catch (Exception ex)
            {
                _log.Info("Error al consultar información para reporte de Resumen Adecuación(" + pBandera + "): " + ex.Message);
                return null;
            }
        }

        /// <summary>
        /// Consultar información en Base de Datos para llenar las celdas del archivo(Resumen Reservas).
        /// José Hernández Alvarado.
        /// 28-10-2019
        /// </summary>
        /// <param name="pFechaPeriodo">Fecha de periodo abierto o periodo a buscar.</param>
        /// <param name="pBandera">Parámetro para ubicar script a ejectutar en store procedure en base de datos.</param>
        /// <param name="pFechaPeriodoAnterior">Fecha del mes anterior al mes del periodo abierto.</param>
        /// <returns>Lista con información consultada</returns>
        public List<ReportesReservas> ConsultarReporteResumenReservas(string pFechaPeriodo, string pBandera, string pFechaPeriodoAnterior)
        {
            XmlConfigurator.Configure();
            try
            {
                var parameters = new List<SqlParameter>();
                parameters.Add(SRVDBContext<RowAffected>.AddParams("@pBandera", SqlDbType.VarChar, pBandera, ParameterDirection.Input));
                parameters.Add(SRVDBContext<RowAffected>.AddParams("@pFechaCalculo", SqlDbType.VarChar, pFechaPeriodo.Substring(0, 6), ParameterDirection.Input));
                parameters.Add(SRVDBContext<RowAffected>.AddParams("@pFechaPeriodo", SqlDbType.VarChar, pFechaPeriodo, ParameterDirection.Input));
                parameters.Add(SRVDBContext<RowAffected>.AddParams("@pFechaPeriodoAnterior", SqlDbType.VarChar, pFechaPeriodoAnterior, ParameterDirection.Input));

                return SRVDBContext<ReportesReservas>.CallStoreProcedure(StoredProcedures.CR_ConsultasReportesReservas, parameters, x => new ReportesReservas
                {
                    CodigoMoneda = x.GetString(0),
                    CodigoReajuste = x.GetString(1),
                    Prestacion = x.GetString(2),
                    MontoReservaBase = x.GetDecimal(3),
                    NumeroPolizas = x.GetInt32(4)
                }).ToList();
            }
            catch (Exception ex)
            {
                _log.Info("Error al consultar información para reporte de Resumen Reservas(" + pBandera + "): " + ex.Message);
                return null;
            }
        }

        /// <summary>
        /// Consultar información en Base de Datos para llenar las celdas del archivo(Reporte SBS).
        /// José Hernández Alvarado.
        /// 28-10-2019
        /// </summary>
        /// <param name="pFechaPeriodo">Fecha de periodo abierto o periodo a buscar.</param>
        /// <param name="pBandera">Parámetro para ubicar script a ejectutar en store procedure en base de datos.</param>
        /// <returns>Tabla con información consultada</returns>
        public DataTable ConsultarReporteSBS(string pFechaPeriodo, string pBandera)
        {
            var parameters = new List<SqlParameter>();
            parameters.Add(SRVDBContext<RowAffected>.AddParams("@pBandera", SqlDbType.VarChar, pBandera, ParameterDirection.Input));
            parameters.Add(SRVDBContext<RowAffected>.AddParams("@pFechaCalculo", SqlDbType.VarChar, pFechaPeriodo.Substring(0, 6), ParameterDirection.Input));
            parameters.Add(SRVDBContext<RowAffected>.AddParams("@pFechaPeriodo", SqlDbType.VarChar, pFechaPeriodo, ParameterDirection.Input));

            var list = SRVDBContext<DataTable>.CallStoreProcedureDt(StoredProcedures.CR_ConsultasReportesReservas, parameters);
            return list;
        }

        /// <summary>
        /// Consultar información en Base de Datos para llenar las celdas del archivo(Resumen Reservas - Insuficiencia).
        /// José Hernández Alvarado.
        /// 28-10-2019
        /// </summary>
        /// <param name="pBandera">Parámetro para ubicar script a ejectutar en store procedure en base de datos.</param>
        /// <returns>Objeto con información consultada</returns>
        public ReportesReservas ConsultarInsuficiencia(string pBandera)
        {
            var parameters = new List<SqlParameter>();
            parameters.Add(SRVDBContext<RowAffected>.AddParams("@pBandera", SqlDbType.VarChar, pBandera, ParameterDirection.Input));

            return SRVDBContext<ReportesReservas>.CallStoreProcedure(StoredProcedures.CR_ConsultasReportesReservas, parameters, x => new ReportesReservas
            {
                SI_Insuficiencia = x.GetDecimal(0),
                SA_Insuficiencia = x.GetDecimal(1),
                DolaresEnSoles_Insuficiencia = x.GetDecimal(2)
            }).FirstOrDefault();
        }

        /// <summary>
        /// Consultar información en Base de Datos para llenar las celdas del archivo(Reporte Contable).
        /// José Hernández Alvarado.
        /// 28-10-2019
        /// </summary>
        /// <param name="pFechaPeriodo">Fecha de periodo abierto o periodo a buscar.</param>
        /// <param name="pBandera">Parámetro para ubicar script a ejectutar en store procedure en base de datos.</param>
        /// <returns>Lista con información consultada</returns>
        public List<ReportesReservas> ConsultarReporteContable(string pFechaPeriodo, string pBandera)
        {
            XmlConfigurator.Configure();
            try
            {
                var parameters = new List<SqlParameter>();
                parameters.Add(SRVDBContext<RowAffected>.AddParams("@pBandera", SqlDbType.VarChar, pBandera, ParameterDirection.Input));
                parameters.Add(SRVDBContext<RowAffected>.AddParams("@pFechaCalculo", SqlDbType.VarChar, pFechaPeriodo.Substring(0, 6), ParameterDirection.Input));
                parameters.Add(SRVDBContext<RowAffected>.AddParams("@pFechaPeriodo", SqlDbType.VarChar, pFechaPeriodo, ParameterDirection.Input));

                return SRVDBContext<ReportesReservas>.CallStoreProcedure(StoredProcedures.CR_ConsultasReportesReservas, parameters, x => new ReportesReservas
                {
                    Paquete = x.GetString(0),
                    Asiento = x.GetString(1),
                    FechaContable = x.GetDateTime(2),
                    TipoAsiento = x.GetString(3),
                    TipoContabilidad = x.GetString(4),
                    ClaseAsiento = x.GetString(5),
                    Fuente = x.GetString(6),
                    Referencia = x.GetString(7),
                    Contribuyente = x.GetString(8),
                    CentroCosto = x.GetString(9),
                    CuentaContable = x.GetString(10),
                    DebitoLocal = x.GetDecimal(11),
                    CreditoLocal = x.GetDecimal(12),
                    DebitoDolar = x.GetDecimal(13),
                    CreditoDolar = x.GetDecimal(14),
                    MontoUnidades = x.GetDecimal(15)
                }).ToList();
            }
            catch (Exception ex)
            {
                _log.Info("Error al consultar información para Reporte Contable(" + pBandera + "): " + ex.Message);
                return null;
            }
        }

    }
}
