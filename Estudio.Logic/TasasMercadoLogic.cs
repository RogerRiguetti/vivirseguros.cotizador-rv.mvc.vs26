using Estudio.Repository.Core.Domain;
using Estudio.Repository.Helpers;
using Estudio.Repository.Persistence.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Estudio.Logic
{
    public class TasasMercadoLogic
    {
        CatalogosOficialesRepository _CatalogosSegurosCO = new CatalogosOficialesRepository();
        TasasMercadoRepository _TasasMercadoRepository = new TasasMercadoRepository();
        /// <summary>
        /// Osvaldo Valdez Carrillo
        /// 2018-08-08
        /// </summary>
        /// <returns> Regresa una lista con los tipo de moneda </returns>
        public List<Moneda> TiposMoneda()
        {
            return _CatalogosSegurosCO.TiposMoneda();
        }
        /// <summary>
        /// Osvaldo Valdez Carrillo
        /// 2018-08-10 
        /// </summary>
        /// <param name="vlMoneda">valor de la moneda</param>
        /// <param name="vlReajuste">valor del reajuste</param>
        /// <returns>Lista de años</returns>
        public Response ListaYear(string vlMoneda, int vlReajuste)
        {
            try
            {
                Response res = new Response();
                res.IsOk = true;
                res.Object = _TasasMercadoRepository.ListaYear(vlMoneda, vlReajuste);
                res.Message = "Información cargada con éxito";
                return res;
            }
            catch (Exception ex)
            {
                Response res = new Response();
                res.IsOk = false;
                res.Message = ex.Message;
                return res;
            }
        }
        /// <summary>
        /// Osvaldo Valdez Carrillo
        /// 2018-08-10 
        /// </summary>
        /// <param name="vlMoneda">valor de la moneda</param>
        /// <param name="vlReajuste">valor del reajuste</param>
        /// <param name="vlAnno">valor del año</param>
        /// <returns>Información del año</returns>
        public Response BuscarYear(string vlMoneda, int vlReajuste, int vlAnno)
        {
            try
            {
                Response res = new Response();
                res.IsOk = true;
                res.Object = _TasasMercadoRepository.BuscarYear(vlMoneda, vlReajuste, vlAnno);
                if (res.Object == null)
                {
                    TasaMercado InformacionIncial = new TasaMercado();
                    InformacionIncial.Year = vlAnno;
                    InformacionIncial.Enero = 0;
                    InformacionIncial.Febrero = 0;
                    InformacionIncial.Marzo = 0;
                    InformacionIncial.Abril = 0;
                    InformacionIncial.Marzo = 0;
                    InformacionIncial.Junio = 0;
                    InformacionIncial.Julio = 0;
                    InformacionIncial.Agosto = 0;
                    InformacionIncial.Septiembre = 0;
                    InformacionIncial.Octubre = 0;
                    InformacionIncial.Noviembre = 0;
                    InformacionIncial.Diciembre = 0;
                    res.Object = InformacionIncial;
                    res.Message = "No se encontro información";
                }
                else
                {
                    res.Message = "Información cargada con éxito";
                }
                return res;
            }
            catch (Exception ex)
            {
                Response res = new Response();
                res.IsOk = false;
                res.Message = ex.Message;
                return res;
            }
        }
        /// <summary>
        /// Osvaldo Valdez Carrillo
        /// 2018-08-10 
        /// </summary>
        /// <param name="vlMoneda">valor de la moneda</param>
        /// <param name="vlReajuste">valor del reajuste</param>
        /// <param name="meses">Arreglo con la informacion del mes</param>
        /// <param name="bandera">para activar el guardado o modificación</param>
        /// <returns>retorna la lista de los años</returns>
        public Response GrabarTasaMercado(string vlMoneda, int vlReajuste, TasaMercado meses, bool bandera)
        {
            try
            {
                Response res = new Response();
                if (bandera)
                {//graba la nueva informacion
                    res.IsOk = true;
                    res.Object = _TasasMercadoRepository.GrabarTasaMercado(vlMoneda, vlReajuste, meses, "GRABARYEAR");
                    if (res.Object == null)
                    {
                        List<TasaMercado> InformacionIncial = new List<TasaMercado>();
                        TasaMercado Informacion = new TasaMercado();
                        Informacion.Year = 0;
                        InformacionIncial.Add(Informacion);
                        res.Object = InformacionIncial;
                        res.Message = "No se encontro información";
                    }
                    else
                    {
                        res.Message = "Información cargada con éxito";
                    }
                    return res;
                }
                else
                {//actualiza el año
                    res.IsOk = true;
                    res.Object = _TasasMercadoRepository.GrabarTasaMercado(vlMoneda, vlReajuste, meses, "ACTUALIZARYEAR");
                    res.Message = "Información cargada con éxito";
                    return res;
                }
            }
            catch (Exception ex)
            {
                Response res = new Response();
                res.IsOk = false;
                res.Message = ex.Message;
                return res;
            }

        }
        /// <summary>
        /// Osvaldo Valdez Carrillo
        /// 2018-08-10 
        /// </summary>
        /// <param name="vlMoneda">valor de la moneda</param>
        /// <param name="vlReajuste">valor del reajuste</param>
        /// <param name="year">valor del año</param>
        /// <returns>lista actual de años</returns>
        public Response EliminarTasaMercado(string vlMoneda, int vlReajuste, int year)
        {
            try {
                Response res = new Response();
                res.IsOk = true;
                res.Object = _TasasMercadoRepository.EliminarTasaMercado(vlMoneda, vlReajuste, year);
                if (res.Object == null)
                {
                    List<TasaMercado> InformacionIncial = new List<TasaMercado>();
                    TasaMercado Informacion = new TasaMercado();
                    Informacion.Year = 0;
                    InformacionIncial.Add(Informacion);
                    res.Object = InformacionIncial;
                    res.Message = "No se encontro información";
                }
                else
                {
                    res.Message = "Información cargada con éxito";
                }
                return res;
            }
            catch (Exception ex)
            {
                Response res = new Response();
                res.IsOk = false;
                res.Message = ex.Message;
                return res;
            }
            
            
        }
        /// <summary>
        /// Osvaldo Valdez Carrillo
        /// 2018-08-10 
        /// </summary>
        /// <param name="vlMoneda">valor de la moneda</param>
        /// <param name="vlReajuste">valor del reajuste</param>
        /// <param name="moneda">valor de la moneda</param>
        /// <returns>lista con la información del reporte</returns>
        public List<TasaMercado> ConsultaRpt(string vlMoneda, int vlReajuste, string moneda)
        {
            try
            {
                List<TasaMercado> registros = _TasasMercadoRepository.ConsultaRpt(vlMoneda, vlReajuste, moneda);
                return registros;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

    }
}
