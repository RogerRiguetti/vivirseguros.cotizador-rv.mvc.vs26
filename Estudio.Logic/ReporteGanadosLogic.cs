using Estudio.Repository.Core.Domain;
using Estudio.Repository.Helpers;
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
    public class ReporteGanadosLogic
    {
        ReporteGanadosRepository _reporteGanadosRepository = new ReporteGanadosRepository();
        private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public List<Departamento> Departamentos()
        {
            return _reporteGanadosRepository.Departamentos();
        }

        public List<Pension> Pensiones()
        {
            return _reporteGanadosRepository.Pensiones();
        }

        public List<Moneda> Monedas()
        {
            return _reporteGanadosRepository.Monedas();
        }
        public List<TipoRenta> TiposRenta()
        {
            return _reporteGanadosRepository.TiposRenta();
        }

        public Response BusquedaInformacion(FiltrosTotalesReportesGanados filtros)
        {
            Response res = new Response();
            List<ReporteGanados> ganadosCompania = new List<ReporteGanados>();
            List<ReporteGanados> ultimaCompania = new List<ReporteGanados>();

            ReporteGanados reporteVidaCamara = new ReporteGanados();

            if (filtros.Prestacion == null)
            {
                res.IsOk = false;
                res.Message = "Debe seleccionar al menos una Prestación.";
                return res;
            }
            else if(filtros.Modalidad == null)
            {
                res.IsOk = false;
                res.Message = "Debe seleccionar al menos una Modalidad.";
                return res;
            }
            else if(filtros.Hasta <= 0)
            {
                res.IsOk = false;
                res.Message = "El monto Hasta desde ser mayor a 0.";
                return res;
            }
            else if(filtros.FechaDesde == null)
            {
                res.IsOk = false;
                res.Message = "La fecha desde no puede ir vacia.";
                return res;
            }
            else if (filtros.FechaHasta == null)
            {
                res.IsOk = false;
                res.Message = "La fecha hasta no puede ir vacia.";
                return res;
            }
            else
            {
                filtros.PrestacionStr = filtros.Prestacion[0];

                for (int i = 1; i < filtros.Prestacion.Count(); i++)
                    filtros.PrestacionStr = filtros.PrestacionStr + "," + filtros.Prestacion[i];

                filtros.ModalidadStr = filtros.Modalidad[0];

                for (int i = 1; i < filtros.Modalidad.Count(); i++)
                    filtros.ModalidadStr = filtros.ModalidadStr + "," + filtros.Modalidad[i];
            }
                try
                {
                if (filtros.Moneda == null || filtros.Moneda == "T")
                {
                   
                    res.IsOk = false;
                    res.Message = "Debe seleccionar una Moneda";
                    return res;
                }
                else
                {
                    ganadosCompania = _reporteGanadosRepository.BusquedaInformacion(filtros);

                    if (ganadosCompania.Count == 0)
                    {
                        res.IsOk = false;
                        res.Message = "No se encontró información";
                        return res;
                    }

                    reporteVidaCamara = (from gc in ganadosCompania where gc.Compania == "4158" select gc).FirstOrDefault();

                    reporteVidaCamara.color = reporteVidaCamara.NumeroRegistro >= 4 ? "#E85663" : "#54CEA4";

                    filtros.TotalNumeroCotizado = ganadosCompania.Sum(x => x.NumeroCotizado);
                    filtros.TotalPension = ganadosCompania.Sum(x => x.Pension) / ganadosCompania.Count;
                    filtros.TotalTasaVenta = ganadosCompania.Sum(x => x.TasaVenta) / ganadosCompania.Count;


                    res.Object = new { GanadosCompania = ganadosCompania, Totales = filtros };
                    res.IsOk = true;
                    res.Message = "Datos encontrados con exito";
                    return res;
                }
                }
            catch (Exception ex)
            {
                res.IsOk = false;
                res.Message = ex.Message;
                return res;
            }
        }

        public List<ReporteGanados> BusquedaInformacionExcel(FiltrosTotalesReportesGanados filtros)
        {
            XmlConfigurator.Configure();

            try
            {
                filtros.PrestacionStr = filtros.Prestacion[0];

                for (int i = 1; i < filtros.Prestacion.Count(); i++)
                    filtros.PrestacionStr = filtros.PrestacionStr + "," + filtros.Prestacion[i];

                filtros.ModalidadStr = filtros.Modalidad[0];

                for (int i = 1; i < filtros.Modalidad.Count(); i++)
                    filtros.ModalidadStr = filtros.ModalidadStr + "," + filtros.Modalidad[i];
                
                return _reporteGanadosRepository.BusquedaInformacion(filtros);
            }
            catch (Exception ex)
            {
                _log.Info("Error al consultar la información de Reporte Ganados: " + ex.Message);
                Console.WriteLine(ex.Message);
                return null;
            }
        }
    }
}
