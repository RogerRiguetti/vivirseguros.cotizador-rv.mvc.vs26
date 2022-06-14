using Estudio.Repository.Core.Domain;
using Estudio.Repository.Persistence.Repositories;
using log4net;
using log4net.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Estudio.Logic
{
    public class ReportesLogic
    {
        private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        ReportesRepository _reporteGanadosRepository = new ReportesRepository();

        public List<ReporteCasosGanadosRV> GenerarReporteGanadosRV(string FechaDesde, string FechaHasta, string parametroRV)
        {
            XmlConfigurator.Configure();

            try
            {
                List<ReporteCasosGanadosRV> rptGanadosRV = new List<ReporteCasosGanadosRV>();
                rptGanadosRV = _reporteGanadosRepository.GenerarReporteGanadosRV(FechaDesde, FechaHasta, parametroRV);

                return rptGanadosRV;
            }
            catch (Exception ex)
            {
                _log.Info("Error al consultar la información de Reporte Ganados RV: " + ex.Message);
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        public List<ReporteCasosGanadosRP> GenerarReporteGanadosRP(string FechaDesde, string FechaHasta, string parametroRP)
        {
            XmlConfigurator.Configure();

            try
            {
                List<ReporteCasosGanadosRP> rptGanadosRP = new List<ReporteCasosGanadosRP>();
                rptGanadosRP = _reporteGanadosRepository.GenerarReporteGanadosRP(FechaDesde, FechaHasta, parametroRP);

                return rptGanadosRP;
            }
            catch (Exception ex)
            {
                _log.Info("Error al consultar la información de Reporte Ganados RV: " + ex.Message);
                Console.WriteLine(ex.Message);
                return null;
            }
        }
    }
}
