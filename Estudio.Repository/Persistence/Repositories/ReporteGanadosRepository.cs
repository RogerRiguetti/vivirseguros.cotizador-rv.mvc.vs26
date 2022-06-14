
using Estudio.Repository.Core.Domain;
using Estudio.Repository.Helpers;
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
    public class ReporteGanadosRepository
    {
        private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public List<Departamento> Departamentos()
        {
            try
            {
                var parameters = new List<SqlParameter>();
                parameters.Add(SRVDBContext<RowAffected>.AddParams("@pClave", SqlDbType.VarChar, "DEPARTAMENTOS", ParameterDirection.Input));

                return SRVDBContext<Departamento>.CallStoreProcedure(StoredProcedures.VCE_Catalogos, parameters, x => new Departamento
                {
                    IdDepartamento = x.GetInt32(0),
                    Elemento = x.GetString(1)
                }).ToList();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public List<Pension> Pensiones()
        {
            try
            {
                var parameters = new List<SqlParameter>();
                parameters.Add(SRVDBContext<RowAffected>.AddParams("@pClave", SqlDbType.VarChar, "BUSQUEDAPRESTACION", ParameterDirection.Input));

                return SRVDBContext<Pension>.CallStoreProcedure(StoredProcedures.CO_ConsultasReporteGanados, parameters, x => new Pension
                {
                    Clave = x.GetString(0),
                    Elemento = x.GetString(1)
                }).ToList();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public List<Moneda> Monedas()
        {
            try
            {
                var parameters = new List<SqlParameter>();
                parameters.Add(SRVDBContext<RowAffected>.AddParams("@pClave", SqlDbType.VarChar, "BUSQUEDAMONEDA", ParameterDirection.Input));

                return SRVDBContext<Moneda>.CallStoreProcedure(StoredProcedures.CO_ConsultasReporteGanados, parameters, x => new Moneda
                {
                    CodigoMoneda = x.GetString(0),
                    Elemento = x.GetString(1)
                }).ToList();


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public List<TipoRenta> TiposRenta()
        {
            try
            {
                var parameters = new List<SqlParameter>();
                parameters.Add(SRVDBContext<RowAffected>.AddParams("@pClave", SqlDbType.VarChar, "BUSQUEDARENTA", ParameterDirection.Input));

                return SRVDBContext<TipoRenta>.CallStoreProcedure(StoredProcedures.CO_ConsultasReporteGanados, parameters, x => new TipoRenta
                {
                    Clave = x.GetString(0),
                    Elemento = x.GetString(1)
                }).ToList();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public List<ReporteGanados> BusquedaInformacion(FiltrosTotalesReportesGanados filtros)
        {
            List<ReporteGanados> ganadosCompania = new List<ReporteGanados>();

            XmlConfigurator.Configure();
             
            _log.Info("Comenzara la consulta de Reporte Ganados ");

            try
            {
                string departamento = filtros.Departamento == null ? "T" : filtros.Departamento;
                string decisionAfiliado = filtros.DecisionAfiliado;
                double cicDesde = filtros.Desde;
                double cicHasta = filtros.Hasta == 0 ? 500000000 : filtros.Hasta;
                string fechaDesde = filtros.FechaDesde;
                string fechaHasta = filtros.FechaHasta;
                int monedaReajuste = 0;

                string moneda = "";
                if (filtros.Moneda == "S/.Aj." || filtros.Moneda == "S/.")
                {
                    if (filtros.Moneda == "S/.Aj.")
                        monedaReajuste = 2;
                    else
                        monedaReajuste = 1;


                        moneda = "NS";
                    
                }
                else if (filtros.Moneda == null)
                {
                    filtros.Moneda = "T";
                }
                else
                {
                    if (filtros.Moneda == "US$Aj.")
                        monedaReajuste = 2;
                    else
                        monedaReajuste = 0;


                        moneda = "US";
                }
                string prestacion = filtros.PrestacionStr;
                string modalidad = filtros.ModalidadStr;
                string cotiza = filtros.Cotiza;
                string gana = filtros.Gana;
                _log.Info("Se recibieron correctamente los parametros  ");

                var parameters = new List<SqlParameter>();
                parameters.Add(SRVDBContext<RowAffected>.AddParams("@pClave", SqlDbType.VarChar, "BUSQUEDAINFORMACION", ParameterDirection.Input));
                parameters.Add(SRVDBContext<RowAffected>.AddParams("@pDepartamento", SqlDbType.VarChar, departamento, ParameterDirection.Input));
                parameters.Add(SRVDBContext<RowAffected>.AddParams("@pDecisionAfiliado", SqlDbType.VarChar, decisionAfiliado, ParameterDirection.Input));
                parameters.Add(SRVDBContext<RowAffected>.AddParams("@pRangoCicDe", SqlDbType.Decimal, cicDesde, ParameterDirection.Input));
                parameters.Add(SRVDBContext<RowAffected>.AddParams("@pRangoCicHasta", SqlDbType.Decimal, cicHasta, ParameterDirection.Input));
                parameters.Add(SRVDBContext<RowAffected>.AddParams("@pSeleccionDesde", SqlDbType.VarChar, fechaDesde, ParameterDirection.Input));
                parameters.Add(SRVDBContext<RowAffected>.AddParams("@pSeleccionHasta", SqlDbType.VarChar, fechaHasta, ParameterDirection.Input));
                parameters.Add(SRVDBContext<RowAffected>.AddParams("@pMoneda", SqlDbType.VarChar, moneda, ParameterDirection.Input));
                parameters.Add(SRVDBContext<RowAffected>.AddParams("@pMonedaReajuste", SqlDbType.Int, monedaReajuste, ParameterDirection.Input));
                parameters.Add(SRVDBContext<RowAffected>.AddParams("@pPrestacion", SqlDbType.VarChar, prestacion, ParameterDirection.Input));
                parameters.Add(SRVDBContext<RowAffected>.AddParams("@pTipoRenta", SqlDbType.VarChar, modalidad, ParameterDirection.Input));
                parameters.Add(SRVDBContext<RowAffected>.AddParams("@pCotiza", SqlDbType.VarChar, cotiza, ParameterDirection.Input));
                parameters.Add(SRVDBContext<RowAffected>.AddParams("@pGana", SqlDbType.VarChar, gana, ParameterDirection.Input));

                _log.Info("Se ejecutara la busqueda");
                ganadosCompania = SRVDBContext<ReporteGanados>.CallStoreProcedure(StoredProcedures.CO_ConsultasReporteGanados, parameters, x => new ReporteGanados
                {
                    CompañiaSegurosVitalicios = x.GetString(0),
                    NumeroCotizado = x.GetInt32(1),
                    Pension = (double)x.GetDecimal(2),
                    TasaVenta = (double)x.GetDecimal(3),
                    DiferenciaPension = (double) x.GetDecimal(4),
                    DiferenciaTasaVenta = (double) x.GetDecimal(5),
                    NumeroCasosGanados = (int)x.GetDecimal(6),
                    NumeroCasosTotales = (int)x.GetInt32(7),
                    ParticipacionMercado = (double) x.GetDecimal(8),
                    NumeroRegistro = (int)x.GetInt64(9),
                    Compania = x.GetString(10),
                    color = "#54CEA4"
                }).ToList();
                _log.Info("Termino correctamente");
                return ganadosCompania;
            }
            catch (Exception ex)
            {
                _log.Info("ERROR"+ ex);
                return ganadosCompania;
            }
        }
    }
}
