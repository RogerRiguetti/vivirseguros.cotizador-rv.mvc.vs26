using Estudio.Repository.Core.Domain;
using log4net;
using log4net.Config;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Estudio.Repository.Persistence.Repositories
{
    public class ReportesRepository
    {
        private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public List<ReporteCasosGanadosRV> GenerarReporteGanadosRV(string FechaDesde, string FechaHasta, string parametroRV)
        {
            List<ReporteCasosGanadosRV> ganadosCompania = new List<ReporteCasosGanadosRV>();

            XmlConfigurator.Configure();

            _log.Info("Comenzara la consulta de Reporte Ganados ");

            try
            {
                FechaDesde = FechaDesde.Replace("-", "");
                FechaHasta = FechaHasta.Replace("-", "");
                if (parametroRV == "Sus")
                    parametroRV = "CONSULTARVSUS";
                else
                    parametroRV = "CONSULTARVTRA";

                _log.Info("Se recibieron correctamente los parametros  ");

                var parameters = new List<SqlParameter>();
                parameters.Add(SRVDBContext<RowAffected>.AddParams("@pBandera", SqlDbType.VarChar, parametroRV, ParameterDirection.Input));
                parameters.Add(SRVDBContext<RowAffected>.AddParams("@pFecDesde", SqlDbType.VarChar, FechaDesde, ParameterDirection.Input));
                parameters.Add(SRVDBContext<RowAffected>.AddParams("@pFecHasta", SqlDbType.VarChar, FechaHasta, ParameterDirection.Input));
                
                _log.Info("Se ejecutara la busqueda");
                ganadosCompania = SRVDBContext<ReporteCasosGanadosRV>.CallStoreProcedure(StoredProcedures.ReporteGanados, parameters, x => new ReporteCasosGanadosRV
                {
                    num_poliza = x.GetString(0),
                    cuspp = x.GetString(1),
                    fechaAdjudicacion = Convert.ToDateTime(x.GetString(2).Substring(0,4)+"-"+ x.GetString(2).Substring(4, 2)+"-"+ x.GetString(2).Substring(6, 2)).ToString("dd/MM/yyyy"),
                    fechaTransferencia = Convert.ToDateTime(x.GetString(3).Substring(0, 4) + "-" + x.GetString(3).Substring(4, 2) + "-" + x.GetString(3).Substring(6, 2)).ToString("dd/MM/yyyy"),
                    fechaCotizacion = Convert.ToDateTime(x.GetString(4).Substring(0, 4) + "-" + x.GetString(4).Substring(4, 2) + "-" + x.GetString(4).Substring(6, 2)).ToString("dd/MM/yyyy"),
                    comisionAsesorCotizado = x.GetDecimal(5),
                    comisionSupervisorCotizado = x.GetDecimal(6),
                    prestacion = x.GetString(7),
                    modalidad = x.GetString(8),
                    moneda = x.GetString(9),
                    añosDiferidos = x.GetInt32(10).ToString(),
                    añosGarantizados = x.GetInt32(11).ToString(),
                    primerTramo = x.GetInt32(12).ToString(),
                    segundoTramo = x.GetDecimal(13),
                    gratificacion = x.GetString(14),
                    cobertura = x.GetString(15),
                    cicCotizacion = x.GetDecimal(16),
                    primaCotizacion = x.GetDecimal(17),
                    rentaCotizacion = x.GetDecimal(18),
                    tasaAFP = x.GetDecimal(19),
                    tcCotizacion = x.GetDecimal(20),
                    tv = x.GetDecimal(21),
                    tir = x.GetDecimal(22),
                    perdida = x.GetDecimal(23),
                    tce = x.GetDecimal(24),
                    tlr = x.GetDecimal(25),
                    tasaMercado = (decimal)x.GetInt32(26),
                    duracion = (decimal)x.GetInt32(27),
                    spread = (decimal)x.GetInt32(28),
                    puesto = x.GetInt64(29).ToString(),
                    mejora = x.GetString(30),
                    nombreAfiliado = x.GetString(31),
                    nombreAsesor = x.GetString(32),
                    nombreSupervisor = x.GetString(33),
                    Departamento = x.GetString(34),
                    cicRealSoles = x.GetDecimal(35),
                    primaCSV = x.GetDecimal(36),
                    rentaRealMoneda = x.GetDecimal(37),
                    rentaReferencia = x.GetDecimal(38),
                    rentaReferenciaActualizada = x.GetDecimal(39),
                    ComisionAsesorReal = x.GetDecimal(40),
                    ComisionSupervisorReal = x.GetDecimal(41),
                    parrilla = x.GetInt32(42).ToString()
                }).ToList();
                _log.Info("Termino correctamente");
                return ganadosCompania;
            }
            catch (Exception ex)
            {
                _log.Info("ERROR" + ex);
                return ganadosCompania;
            }
        }
        public List<ReporteCasosGanadosRP> GenerarReporteGanadosRP(string FechaDesde, string FechaHasta, string parametroRP)
        {
            List<ReporteCasosGanadosRP> ganadosCompania = new List<ReporteCasosGanadosRP>();

            XmlConfigurator.Configure();

            _log.Info("Comenzara la consulta de Reporte Ganados ");

            try
            {
                //FechaDesde = FechaDesde.Replace("-", "");
                //FechaHasta = FechaHasta.Replace("-", "");
                if (parametroRP == "Sol")
                    parametroRP = "CONSULTARPSOL";
                else
                    parametroRP = "CONSULTARPABO";

                _log.Info("Se recibieron correctamente los parametros  ");

                var parameters = new List<SqlParameter>();
                parameters.Add(SRVDBContext<RowAffected>.AddParams("@pBandera", SqlDbType.VarChar, parametroRP, ParameterDirection.Input));
                parameters.Add(SRVDBContext<RowAffected>.AddParams("@pFecDesde", SqlDbType.VarChar, FechaDesde, ParameterDirection.Input));
                parameters.Add(SRVDBContext<RowAffected>.AddParams("@pFecHasta", SqlDbType.VarChar, FechaHasta, ParameterDirection.Input));

                _log.Info("Se ejecutara la busqueda");
                ganadosCompania = SRVDBContext<ReporteCasosGanadosRP>.CallStoreProcedure(StoredProcedures.ReporteGanados, parameters, x => new ReporteCasosGanadosRP
                {
                    num_poliza = x.GetInt32(0).ToString(),
                    fechaAbono = x.GetDateTime(1).ToString("dd/MM/yyyy"),
                    comisionAsesorCotizado = x.GetDecimal(2),
                    comisionSupervisorCotizado = x.GetDecimal(3),
                    moneda = x.GetString(4),
                    añosPlazo = x.GetDecimal(5).ToString(),
                    añosDiferido = x.GetDecimal(6).ToString(),
                    primaMoneda = x.GetDecimal(7),
                    rentaMoneda = x.GetDecimal(8),
                    tv = x.GetDecimal(9),
                    tir = x.GetDecimal(10),
                    perdida = x.GetDecimal(11),
                    tasaInversion = x.GetDecimal(12),
                    duracion = x.GetDecimal(13),
                    spread = (decimal)x.GetInt32(14),
                    mejora = x.GetByte(15).ToString(),
                    nombreAsegurado = x.GetString(16),
                    nombreAsesor = x.GetString(17),
                    nombreSupervisor = x.GetString(18),
                    departamento = x.GetString(19),
                    comisionAsesorReal = x.GetDecimal(20).ToString(),
                    comisionSupervisorReal = x.GetDecimal(21).ToString(),
                    parrilla = x.GetDecimal(22).ToString()
                }).ToList();
                _log.Info("Termino correctamente");
                return ganadosCompania;
            }
            catch (Exception ex)
            {
                _log.Info("ERROR" + ex);
                return ganadosCompania;
            }
        }
    }
}
