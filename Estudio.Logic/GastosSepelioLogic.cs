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
    public class GastosSepelioLogic
    {
        GastosSepelioRepository _GastosSepelioRepository = new GastosSepelioRepository();
        /// <summary>
        /// Osvaldo Valdez Carrillo
        /// 2018-08-20
        /// </summary>
        /// <returns>Retorna la lista con las fechas</returns>
        public Response CargaFechas()
        {
            try
            {
                Response res = new Response();
                res.IsOk = true;
                res.Object = _GastosSepelioRepository.CargaFechas();
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
        /// 2018-08-20
        /// </summary>
        /// <param name="strFecIni">Fecha inicial</param>
        /// <returns>Retorna la informacion de la fecha consultada</returns>
        public Response ConsultaFecha(DateTime strFecIni)
        {
            try
            {
                Response res = new Response();
                res.IsOk = true;
                res.Object = _GastosSepelioRepository.ConsultaFecha(strFecIni);
                if (res.Object == null)
                {
                    GastoSepelio Informacion = new GastoSepelio();
                    Informacion.FechaInicial = strFecIni.ToString("yyyy-MM-dd");
                    Informacion.FechaTermino = "9999-12-31";
                    Informacion.Valor = 0;
                    res.Object = Informacion;
                    res.Message = "No se encontro información";
                }
                else {

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
        /// 2018-08-21
        /// </summary>
        /// <param name="strFecIni">>Fecha inicial</param>
        /// <param name="vlGasto">Monto</param>
        /// <param name="bandera">True(graba)  o False(modifica)</param>
        /// <param name="Usuario">Nombre del Usuario</param>
        /// <param name="strFecFin">Fecha Termino</param>
        /// <returns>Retotna la respuesta al grabar o Modificar</returns>
        public Response GrabarSepelio(DateTime strFecIni, decimal vlGasto, Boolean bandera, string Usuario,DateTime strFecFin)
        {
            try
            {
                Response res = new Response();
                GastoSepelio InformacionIncial = new GastoSepelio();
                if (bandera)
                {//nuevo registro
                    res.IsOk = true;
                    DateTime fechaFinal = Convert.ToDateTime("9999-12-31");
                    InformacionIncial = _GastosSepelioRepository.GrabarSepelio("GRABARFECHA", strFecIni, fechaFinal, Usuario, vlGasto);
                    if (InformacionIncial == null)
                    {
                        InformacionIncial = new GastoSepelio();
                        //verificamos si existe una fecha siguiente
                        InformacionIncial = _GastosSepelioRepository.GrabarSepelio("VIGENCIAINTERMEDIA", strFecIni, fechaFinal, "", 0);
                        if (InformacionIncial == null)
                        {
                            InformacionIncial = new GastoSepelio();
                            InformacionIncial.FechaTermino = fechaFinal.ToString("yyyy-MM-dd");
                            InformacionIncial.Valor = vlGasto;
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
                            _GastosSepelioRepository.GrabarSepelio("ACTUALIZARVIGENCIA", strFecIni, fechaInicial.AddDays(-1), "", 0);
                            InformacionIncial.FechaTermino = fechaInicial.AddDays(-1).ToString("yyyy-MM-dd");
                        }
                        InformacionIncial.FechaInicial = strFecIni.ToString("yyyy-MM-dd");
                        InformacionIncial.Valor = vlGasto;
                        res.Object = InformacionIncial;
                        res.Message = "Información cargada con éxito";
                    }
                    else
                    {
                        //Actualiza la fecha de termino del periodo anterior 
                        DateTime fechaInicial = Convert.ToDateTime(InformacionIncial.FechaInicial);
                        //recuperamos la fecha anterior y restamos 1 dia a la fecha ingresada para generar el periodo
                        _GastosSepelioRepository.GrabarSepelio("ACTUALIZARVIGENCIA", fechaInicial, strFecIni.AddDays(-1), "",0);
                        InformacionIncial.FechaTermino = fechaFinal.ToString("yyyy-MM-dd");
                        res.Message = "Información cargada con éxito";
                        //actualiza la fecha de termino del periodo siguiente si existe
                        //fechas intermedias
                        InformacionIncial = _GastosSepelioRepository.GrabarSepelio("VIGENCIAINTERMEDIA", strFecIni, fechaFinal, "", 0);
                        if (InformacionIncial == null)
                        {
                            InformacionIncial = new GastoSepelio();
                            InformacionIncial.FechaTermino = fechaFinal.ToString("yyyy-MM-dd");
                            InformacionIncial.Valor = vlGasto;
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
                            _GastosSepelioRepository.GrabarSepelio("ACTUALIZARVIGENCIA", strFecIni, fechaInicial.AddDays(-1), "", 0);
                            InformacionIncial.FechaTermino = fechaInicial.AddDays(-1).ToString("yyyy-MM-dd");
                        }
                        res.Object = InformacionIncial;
                        res.IsOk = true;
                    }


                }
                else {
                    //update
                    InformacionIncial = _GastosSepelioRepository.GrabarSepelio("ACTUALIZARCUOTA", strFecIni, strFecFin, "", vlGasto);
                    if (InformacionIncial == null)
                    {
                        InformacionIncial = new GastoSepelio();
                        InformacionIncial.FechaTermino = strFecFin.ToString("yyyy-MM-dd");
                        InformacionIncial.Valor = vlGasto;
                        InformacionIncial.FechaInicial = strFecIni.ToString("yyyy-MM-dd");
                        res.Object = InformacionIncial;
                        res.Message = "Error al guardar la información";
                    }
                    else
                    {
                        InformacionIncial.FechaTermino = strFecFin.ToString("yyyy-MM-dd");
                        InformacionIncial.Valor = vlGasto;
                        res.Object = InformacionIncial;
                        res.Message = "Información cargada con éxito";
                    }
                    res.IsOk = true;
                    return res;




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
        /// 2018-08-22
        /// </summary>
        /// <returns>retorna la lista para mostrar en el reporte</returns>
        public List<GastoSepelio> ConsultaRpt()
        {
            try
            {
                List<GastoSepelio> registros = _GastosSepelioRepository.ConsultaRpt();
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
        /// 2018-08-22
        /// </summary>
        /// <param name="strFecIni">fecha de Inicio</param>
        /// <returns>Retotna la respuesta al eliminar</returns>
        public object EliminarSepelio(DateTime strFecIni)
        {
            try
            {
                Response res = new Response();
                res.Message = "La Información se ha Eliminado Satisfactoriamente";
                GastoSepelio InformacionIncial = new GastoSepelio();
                GastoSepelio InformacionBorrar = new GastoSepelio();
                GastoSepelio InformacionIntermedia = new GastoSepelio();

                //buscamos la informacion que se va a borrar 
                InformacionBorrar = _GastosSepelioRepository.ConsultaFecha(strFecIni);
                //'Devuelve EL DIA ANTES DE LA FECHA DE INICIO QUE ESTAMOS ELIMINANDO
                InformacionIncial = _GastosSepelioRepository.EliminarSepelio(strFecIni, strFecIni.AddDays(-1), "BORRARVIGENCIA");
                if (InformacionIncial == null)
                {//si se borran todos los registros
                    InformacionIncial = new GastoSepelio();
                    InformacionIncial.FechaTermino = strFecIni.ToString("yyyy-MM-dd");
                    InformacionIncial.FechaInicial = strFecIni.ToString("yyyy-MM-dd");
                    res.Object = InformacionIncial;
                }
                else
                {
                    DateTime vgTopeFecFin = Convert.ToDateTime("9999-12-31");
                    DateTime vgTopeFecFinal = Convert.ToDateTime(InformacionBorrar.FechaTermino);//obtenermos la fecha fin de la fecha que se borro
                    DateTime fechaInicial = Convert.ToDateTime(InformacionIncial.FechaInicial);//obtenemos la fecha anterior a la que se borro

                    if (vgTopeFecFinal != vgTopeFecFin)
                    {
                        //fechas intermedias
                        InformacionIntermedia = _GastosSepelioRepository.GrabarSepelio("VIGENCIAINTERMEDIA", fechaInicial, strFecIni, "", 0);
                        if (InformacionIntermedia != null)
                        {
                            //CAMBIA LA FECHA DEL TERMINO DE VIGENCIA DEL REGISTRO INGRESADDO
                            vgTopeFecFinal = Convert.ToDateTime(InformacionIntermedia.FechaInicial);
                            //recuperamos la fecha siguiente y restamos 1 dia a la fecha siguiente para acualizar la fecha fin del registro ingresado
                            _GastosSepelioRepository.GrabarSepelio("ACTUALIZARVIGENCIA", fechaInicial, vgTopeFecFinal.AddDays(-1), "", 0);
                        }
                    }
                    else
                    {
                        _GastosSepelioRepository.GrabarSepelio("ACTUALIZARVIGENCIA", fechaInicial, vgTopeFecFin, "", 0);
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
