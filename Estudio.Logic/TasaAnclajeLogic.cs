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
    public class TasaAnclajeLogic
    {
        TasaAnclajeRepository _TasaAnclajeRepository = new TasaAnclajeRepository();
        CatalogosOficialesRepository _CatalogosOficiales = new CatalogosOficialesRepository();
        /// <summary>
        /// Osvaldo Valdez Carrillo
        /// 2018-08-08
        /// </summary>
        /// <returns> Regresa una lista con los tipo de moneda </returns>
        public List<Moneda> TiposMoneda()
        {
            return _CatalogosOficiales.TiposMoneda();
        }
        /// <summary>
        /// Osvaldo Valdez Carrillo
        /// 2018-08-13
        /// </summary>
        /// <param name="vlMoneda">valor de la moneda</param>
        /// <param name="vlReajuste">valor del reajuste de la moneda</param>
        /// <returns>retorna la lista de los periodos a la vista</returns>
        public Response ListaPeriodos(string vlMoneda, int vlReajuste)
        {
            try
            {
                Response res = new Response();
                res.IsOk = true;
                res.Object = _TasaAnclajeRepository.ListaPeriodos(vlMoneda, vlReajuste);
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
        /// 2018-08-13
        /// </summary>
        /// <param name="strFecIni">fecha de inicio</param>
        /// <param name="vlMoneda">valor de la moneda</param>
        /// <param name="vlReajuste">valor del reajuste de la moneda</param>
        /// <returns>retorna los valores para el llenado de la vista</returns>
        public Response BuscarVigencia(DateTime strFecIni, string vlMoneda, int vlReajuste)
        {
            try
            {
                Response res = new Response();
                res.Object = _TasaAnclajeRepository.BuscarVigencia(strFecIni, vlMoneda, vlReajuste);
                if (res.Object == null)
                {
                    TasaAnclaje InformacionIncial = new TasaAnclaje();
                    InformacionIncial.FechaTermino = "9999-12-31";
                    InformacionIncial.Tasa = 0;
                    InformacionIncial.FechaInicial = strFecIni.ToString("yyyy-MM-dd");
                    res.Object = InformacionIncial;
                    res.Message = "No se encontro información";
                }
                else
                {
                    res.Message = "Información cargada con éxito";
                }
                res.IsOk = true;
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
        /// 2018-08-14
        /// </summary>
        /// <param name="strFecIni">fecha de Inicio</param>
        /// <param name="strFecFin">fecha fin</param>
        /// <param name="vlMoneda">valor de la moneda</param>
        /// <param name="vlReajuste">valor del reajuste de la moneda</param>
        /// <param name="tasa">valor de la tasa</param>
        /// <param name="bandera">bandera para saber si se va a guardar o actualizar</param>
        /// <returns>retorna un json con la respuesta</returns>
        public Response GrabarTasaAnclaje(DateTime strFecIni, DateTime strFecFin, string vlMoneda, int vlReajuste, decimal tasa, bool bandera)
        {
            try
            {
                Response res = new Response();
                TasaAnclaje InformacionIncial = new TasaAnclaje();

                if (bandera)
                {
                    //realiza la inserción
                    InformacionIncial = _TasaAnclajeRepository.GrabarTasaAnclaje(strFecIni, strFecFin, vlMoneda, vlReajuste, tasa, "GRABARVIGENCIA");
                    if (InformacionIncial == null)
                    {
                        InformacionIncial = new TasaAnclaje();
                        //fechas intermedias
                        InformacionIncial = _TasaAnclajeRepository.GrabarTasaAnclaje(strFecIni, strFecFin, vlMoneda, vlReajuste, 0, "VIGENCIAINTERMEDIA");
                        if (InformacionIncial == null)
                        {
                            InformacionIncial = new TasaAnclaje();
                            InformacionIncial.FechaTermino = strFecFin.ToString("yyyy-MM-dd");
                            InformacionIncial.Tasa = tasa;
                            InformacionIncial.FechaInicial = strFecIni.ToString("yyyy-MM-dd");
                            res.Object = InformacionIncial;
                            res.IsOk = true;
                            return res;
                        }
                        else
                        {
                            //CAMBIA LA FECHA DEL TERMINO DE VIGENCIA DEL REGISTRO INGRESADDO
                            DateTime fechaInicial = Convert.ToDateTime(InformacionIncial.FechaInicial);
                            //recuperamos la fecha siguiente y restamos 1 dia a la fecha siguiente para acualizar la fecha fin del registro ingresado
                            _TasaAnclajeRepository.GrabarTasaAnclaje(strFecIni, fechaInicial.AddDays(-1), vlMoneda, vlReajuste, 0, "ACTUALIZARVIGENCIA");
                            InformacionIncial.FechaTermino = fechaInicial.AddDays(-1).ToString("yyyy-MM-dd");
                        }
                        InformacionIncial.Tasa = tasa;
                        InformacionIncial.FechaInicial = strFecIni.ToString("yyyy-MM-dd");
                        res.Object = InformacionIncial;
                        res.Message = "Información cargada con éxito";
                        res.IsOk = true;
                        return res;
                    }
                    else
                    {
                        //Actualiza la fecha de termino del periodo anterior 
                        DateTime fechaInicial = Convert.ToDateTime(InformacionIncial.FechaInicial);
                        //recuperamos la fecha anterior y restamos 1 dia a la fecha ingresada para generar el periodo
                        _TasaAnclajeRepository.GrabarTasaAnclaje(fechaInicial, strFecIni.AddDays(-1), vlMoneda, vlReajuste, 0, "ACTUALIZARVIGENCIA");

                        InformacionIncial.FechaTermino = strFecFin.ToString("yyyy-MM-dd");
                        res.Message = "Información cargada con éxito";
                        //actualiza la fecha de termino del periodo siguiente si existe
                        //fechas intermedias
                        InformacionIncial = _TasaAnclajeRepository.GrabarTasaAnclaje(strFecIni, strFecFin, vlMoneda, vlReajuste, 0, "VIGENCIAINTERMEDIA");
                        if (InformacionIncial == null)
                        {
                            InformacionIncial = new TasaAnclaje();
                            InformacionIncial.FechaTermino = strFecFin.ToString("yyyy-MM-dd");
                            InformacionIncial.Tasa = tasa;
                            InformacionIncial.FechaInicial = strFecIni.ToString("yyyy-MM-dd");
                            res.Object = InformacionIncial;
                            res.IsOk = true;
                            return res;
                        }
                        else
                        {
                            //CAMBIA LA FECHA DEL TERMINO DE VIGENCIA DEL REGISTRO INGRESADDO
                            fechaInicial = Convert.ToDateTime(InformacionIncial.FechaInicial);
                            //recuperamos la fecha siguiente y restamos 1 dia a la fecha siguiente para acualizar la fecha fin del registro ingresado
                            _TasaAnclajeRepository.GrabarTasaAnclaje(strFecIni, fechaInicial.AddDays(-1), vlMoneda, vlReajuste, 0, "ACTUALIZARVIGENCIA");
                            InformacionIncial.FechaTermino = fechaInicial.AddDays(-1).ToString("yyyy-MM-dd");
                        }
                    }
                    res.Object = InformacionIncial;
                    res.IsOk = true;
                    return res;

                }
                else
                {//update
                    InformacionIncial = _TasaAnclajeRepository.GrabarTasaAnclaje(strFecIni, strFecFin, vlMoneda, vlReajuste, tasa, "ACTUALIZARTASA");
                    if (InformacionIncial == null)
                    {//al guardar la ultima fecha
                        InformacionIncial = new TasaAnclaje();
                        InformacionIncial.FechaTermino = strFecFin.ToString("yyyy-MM-dd");
                        InformacionIncial.Tasa = tasa;
                        InformacionIncial.FechaInicial = strFecIni.ToString("yyyy-MM-dd");
                        res.Object = InformacionIncial;
                        res.Message = "Información cargada con éxito";
                    }
                    else
                    {
                        InformacionIncial.FechaTermino = strFecFin.ToString("yyyy-MM-dd");
                        res.Object = InformacionIncial;
                        res.Message = "Información cargada con éxito";
                    }
                    res.IsOk = true;
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
        /// 2018-08-16
        /// </summary>
        /// <param name="vlMoneda">valor de la moneda</param>
        /// <param name="vlReajuste">valor del reajuste</param>
        /// <param name="moneda">valor del texto del combo moneda</param>
        /// <returns>retorna la consulta</returns>
        public List<TasaAnclaje> ConsultaRpt(string vlMoneda, int vlReajuste, string moneda)
        {
            try
            {
                List<TasaAnclaje> registros = _TasaAnclajeRepository.ConsultaRpt(vlMoneda, vlReajuste,moneda);
                return registros;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
        /// <summary>
        /// Osvaldo Valdez Carrillo
        /// 2018-08-15
        /// </summary>
        /// <param name="strFecIni">fecha inicio</param>
        /// <param name="vlMoneda">valor de la moneda</param>
        /// <param name="vlReajuste">valor del reajuste de la moneda</param>
        /// <returns>retorna un json con la respuesta</returns>
        public Response EliminarTasaAnclaje(DateTime strFecIni, string vlMoneda, int vlReajuste)
        {
            try
            {
                Response res = new Response();
                res.Message = "La Información se ha Eliminado Satisfactoriamente";
                TasaAnclaje InformacionIncial = new TasaAnclaje();
                TasaAnclaje InformacionBorrar = new TasaAnclaje();
                TasaAnclaje InformacionIntermedia = new TasaAnclaje();

                //buscamos la informacion que se va a borrar 
                InformacionBorrar= _TasaAnclajeRepository.BuscarVigencia(strFecIni, vlMoneda, vlReajuste);
                //'Devuelve EL DIA ANTES DE LA FECHA DE INICIO QUE ESTAMOS ELIMINANDO
                InformacionIncial = _TasaAnclajeRepository.EliminarTasaAnclaje(strFecIni, strFecIni.AddDays(-1),vlMoneda,vlReajuste, "BORRARVIGENCIA");
                if (InformacionIncial == null)
                {//si se borran todos los registros
                    InformacionIncial = new TasaAnclaje();
                    InformacionIncial.FechaTermino = strFecIni.ToString("yyyy-MM-dd");
                    InformacionIncial.FechaInicial = strFecIni.ToString("yyyy-MM-dd");
                    res.Object = InformacionIncial;
                }
                else {
                    DateTime vgTopeFecFin = Convert.ToDateTime("9999-12-31");
                    DateTime vgTopeFecFinal = Convert.ToDateTime(InformacionBorrar.FechaTermino);//obtenermos la fecha fin de la fecha que se borro
                    DateTime fechaInicial = Convert.ToDateTime(InformacionIncial.FechaInicial);//obtenemos la fecha anterior a la que se borro

                    if (vgTopeFecFinal != vgTopeFecFin)
                    {
                        //fechas intermedias
                        InformacionIntermedia = _TasaAnclajeRepository.GrabarTasaAnclaje(fechaInicial, strFecIni, vlMoneda, vlReajuste, 0, "VIGENCIAINTERMEDIA");
                        if (InformacionIntermedia != null)
                        {
                            //CAMBIA LA FECHA DEL TERMINO DE VIGENCIA DEL REGISTRO INGRESADDO
                            vgTopeFecFinal = Convert.ToDateTime(InformacionIntermedia.FechaInicial);
                            //recuperamos la fecha siguiente y restamos 1 dia a la fecha siguiente para acualizar la fecha fin del registro ingresado
                            _TasaAnclajeRepository.GrabarTasaAnclaje(fechaInicial, vgTopeFecFinal.AddDays(-1), vlMoneda, vlReajuste, 0, "ACTUALIZARVIGENCIA");
                        }
                    }
                    else {
                        _TasaAnclajeRepository.GrabarTasaAnclaje(fechaInicial, vgTopeFecFin, vlMoneda, vlReajuste, 0, "ACTUALIZARVIGENCIA");
                    }
                    
                    res.Object = InformacionIncial;
                }
                res.IsOk = true;
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
    }
}
