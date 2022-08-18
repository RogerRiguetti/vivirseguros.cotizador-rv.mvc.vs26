using Estudio.Repository.Core.Domain;
using Estudio.Repository.Helpers;
using Estudio.Repository.Persistence.Repositories;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Estudio.Logic
{
    public class LimiteCotizacionMejoradaLogic
    {
        LimiteCotizacionMejoradaRepository _LimiteCotizacionMejoradaRepository = new LimiteCotizacionMejoradaRepository();
        CatalogosOficialesRepository _CatalogosSegurosRV = new CatalogosOficialesRepository();
        /// <summary>
        /// osvaldo valdez
        /// 27/08/2018
        /// </summary>
        /// <returns>retorna el tipo de moneda</returns>
        public List<Moneda> TiposMoneda()
        {
            return _CatalogosSegurosRV.TiposMoneda();

        }
        /// <summary>
        /// osvaldo valdez
        /// 27/08/2018
        /// </summary>
        /// <returns>retorna el tipo de departamento</returns>
        public List<Departamento> TipoDepartamento()
        {
            return _LimiteCotizacionMejoradaRepository.TipoDepartamento();
        }
        /// <summary>
        /// osvaldo valdez
        /// 27/08/2018
        /// </summary>
        /// <returns>retorna el tipo de rango tasas</returns>
        public List<RangosTasaVenta> RangosTasa()
        {
            return _LimiteCotizacionMejoradaRepository.RangosTasa();
        }
        /// <summary>
        /// osvaldo valdez
        /// 27/08/2018
        /// </summary>
        /// <param name="fechaIni">fecha de inicio</param>
        /// <param name="codMoneda">codigo de moneda</param>
        /// <param name="reajuste">valor del reajuste</param>
        /// <param name="departamento">codigo del departamento</param>
        /// <returns>retorna la informacion para mostrar en pantalla</returns>
        public Response Consulta(DateTime fechaIni, string codMoneda, int reajuste, int departamento)
        {
            try
            {
                Response res = new Response();
                LimiteCotizacionMejorada informacion = new LimiteCotizacionMejorada();
                informacion = _LimiteCotizacionMejoradaRepository.ConsultaInfo(fechaIni, codMoneda, reajuste, departamento);
                if (informacion == null)
                {
                    informacion = new LimiteCotizacionMejorada();
                    informacion.FechaFinal = "9999-12-31";
                    informacion.fechaInicial = fechaIni.ToString("yyyy-MM-dd");
                    res.Object = informacion;
                    res.Message = "No se encontro información";
                }
                else
                {
                    List<RangosTasaVenta> rangosTasa = new List<RangosTasaVenta>();

                    informacion.RangosTasaVenta = _LimiteCotizacionMejoradaRepository.RangosTasaVenta(fechaIni, codMoneda, reajuste, departamento);
                    rangosTasa = _LimiteCotizacionMejoradaRepository.RangosTasa();

                    foreach (var item in informacion.RangosTasaVenta)
                        item.NombrePension = (from r in rangosTasa where r.IdRangoTasa == item.CodPension select r.Elemento).FirstOrDefault();

                    res.Object = informacion;
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
        /// osvaldo valdez
        /// 27/08/2018
        /// </summary>
        /// <param name="vlMoneda">valor de la moneda</param>
        /// <param name="vlReajuste">valor del reajuste</param>
        /// <param name="departamento">valor del departamento</param>
        /// <returns>retorna la lista de periodos</returns>
        public Response ListaPeriodos(string vlMoneda, int vlReajuste, int departamento)
        {
            try
            {
                Response res = new Response();
                res.IsOk = true;
                res.Object = _LimiteCotizacionMejoradaRepository.ListaPeriodos(vlMoneda, vlReajuste, departamento);
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
        /// osvaldo valdez
        /// 28/08/2018
        /// </summary>
        /// <param name="informacion">entiti con la informacion de la pantalla</param>
        /// <param name="strFecIni">fecha inicio</param>
        /// <param name="bandera">bandera para actualizar o grabar</param>
        /// <param name="usuario">nombre del usuario</param>
        /// <returns>retorna la respuesta a la pantalla del proceso</returns>
        public Response Grabar(LimiteCotizacionMejorada informacion, DateTime strFecIni, Boolean bandera,string usuario)
        {
            try
            {
                Response res = new Response();
                LimiteCotizacionMejorada InformacionIncial = new LimiteCotizacionMejorada();
                DateTime fechaFinal = Convert.ToDateTime("9999-12-31");
                if (bandera)
                {   //realiza la inserción 
                    InformacionIncial = _LimiteCotizacionMejoradaRepository.Grabar("GRABAR", informacion, strFecIni, fechaFinal,usuario);
                    if (InformacionIncial == null)
                    {
                        InformacionIncial = new LimiteCotizacionMejorada();
                        //verificamos si existe una fecha siguiente
                        InformacionIncial = _LimiteCotizacionMejoradaRepository.Grabar("VIGENCIAINTERMEDIA", informacion, strFecIni, fechaFinal, usuario);
                        if (InformacionIncial == null)
                        {//insertamos los datos al ser el primer registro
                            foreach (var item in informacion.RangosTasaVenta)
                            {
                                _LimiteCotizacionMejoradaRepository.GrabarTasaVenta("GRABARTV", informacion, strFecIni, fechaFinal, usuario, item.CodPension, item.Minimo_TV, item.Maximo_TV);
                            }
                            InformacionIncial = new LimiteCotizacionMejorada();
                            InformacionIncial.fechaInicial = strFecIni.ToString("yyyy-MM-dd");
                            InformacionIncial.FechaFinal = fechaFinal.ToString("yyyy-MM-dd");
                        }
                        else {
                            //CAMBIA LA FECHA DEL TERMINO DE VIGENCIA DEL REGISTRO INGRESADDO
                            DateTime fechaInicial = Convert.ToDateTime(InformacionIncial.fechaInicial);
                            //recuperamos la fecha siguiente y restamos 1 dia a la fecha siguiente para acualizar la fecha fin del registro ingresado
                            _LimiteCotizacionMejoradaRepository.Grabar("ACTUALIZARVIGENCIA", informacion, strFecIni, fechaInicial.AddDays(-1), usuario);
                            //insertamos los datos al ser el primer registro
                            foreach (var item in informacion.RangosTasaVenta)
                            {
                                _LimiteCotizacionMejoradaRepository.GrabarTasaVenta("GRABARTV", informacion, strFecIni, fechaInicial.AddDays(-1), usuario, item.CodPension, item.Minimo_TV, item.Maximo_TV);
                            }
                            InformacionIncial.fechaInicial = strFecIni.ToString("yyyy-MM-dd");
                            InformacionIncial.FechaFinal = fechaInicial.AddDays(-1).ToString("yyyy-MM-dd");
                        }
                        res.Object = InformacionIncial;
                        res.Message = "La Información se ha guardado Satisfactoriamente";
                        res.IsOk = true;
                        return res;
                    }
                    else
                    {
                        //Actualiza la fecha de termino del periodo anterior 
                        DateTime fechaInicial = Convert.ToDateTime(InformacionIncial.fechaInicial);
                        //recuperamos la fecha anterior y restamos 1 dia a la fecha ingresada para generar el periodo
                        _LimiteCotizacionMejoradaRepository.Grabar("ACTUALIZARVIGENCIA", informacion, fechaInicial, strFecIni.AddDays(-1), usuario);
                        //recuperamos la lista de las TASA de venta anterior
                        InformacionIncial.RangosTasaVenta = _LimiteCotizacionMejoradaRepository.RangosTasaVenta(fechaInicial, informacion.CodMoneda, informacion.CodReajuste ,Convert.ToInt32(informacion.CodDepartamento));
                        //actualiamos la fecha de las tasa de ventas anterior
                        foreach (var item in InformacionIncial.RangosTasaVenta)
                        {
                            _LimiteCotizacionMejoradaRepository.GrabarTasaVenta("ACTUALIZARVIGENCIATV", informacion, fechaInicial, strFecIni.AddDays(-1), usuario, item.CodPension,0 ,0);
                        }
                        InformacionIncial.FechaFinal = fechaFinal.ToString("yyyy-MM-dd");
                        res.Message = "La Información se ha guardado Satisfactoriamente";
                        //actualiza la fecha de termino del periodo siguiente si existe
                        //fechas intermedias
                        InformacionIncial = _LimiteCotizacionMejoradaRepository.Grabar("VIGENCIAINTERMEDIA", informacion, strFecIni, fechaFinal, usuario);
                        if (InformacionIncial == null)
                        {//ya que no existen fechas intermedias realiza la insercion de las tasa de venta siendo esta el tope
                            foreach (var item in informacion.RangosTasaVenta)
                            {
                                _LimiteCotizacionMejoradaRepository.GrabarTasaVenta("GRABARTV", informacion, strFecIni, fechaFinal, usuario, item.CodPension, item.Minimo_TV, item.Maximo_TV);
                            }
                            InformacionIncial = new LimiteCotizacionMejorada();
                            InformacionIncial.FechaFinal = fechaFinal.ToString("yyyy-MM-dd");
                            InformacionIncial.fechaInicial = strFecIni.ToString("yyyy-MM-dd");
                            res.Object = InformacionIncial;
                            res.IsOk = true;
                            return res;
                        }
                        else
                        {
                            //CAMBIA LA FECHA DEL TERMINO DE VIGENCIA DEL REGISTRO INGRESADDO
                            fechaInicial = Convert.ToDateTime(InformacionIncial.fechaInicial);
                            //recuperamos la fecha siguiente y restamos 1 dia a la fecha siguiente para acualizar la fecha fin del registro ingresado
                            _LimiteCotizacionMejoradaRepository.Grabar("ACTUALIZARVIGENCIA", informacion, strFecIni, fechaInicial.AddDays(-1), usuario);
                            //realizamos la insercion de los datos de tasa venta
                            foreach (var item in informacion.RangosTasaVenta)
                            {
                                _LimiteCotizacionMejoradaRepository.GrabarTasaVenta("GRABARTV", informacion, strFecIni, fechaInicial.AddDays(-1), usuario, item.CodPension, item.Minimo_TV, item.Maximo_TV);
                            }
                            InformacionIncial.FechaFinal = fechaInicial.AddDays(-1).ToString("yyyy-MM-dd");

                        }
                        res.Object = InformacionIncial;
                        res.IsOk = true;


                    } 
                    res.Object = InformacionIncial;
                    res.IsOk = true;
                    return res;

                }
                else
                {//update
                    InformacionIncial = _LimiteCotizacionMejoradaRepository.Grabar("ACTUALIZARINFO", informacion, strFecIni, fechaFinal, usuario);
                    foreach (var item in informacion.RangosTasaVenta)
                    {
                        _LimiteCotizacionMejoradaRepository.GrabarTasaVenta("ACTUALIZARINFOTV", informacion, strFecIni, fechaFinal, usuario, item.CodPension, item.Minimo_TV, item.Maximo_TV);
                    }
                    if (InformacionIncial == null)
                    {
                        InformacionIncial = new LimiteCotizacionMejorada();
                        res.Object = InformacionIncial;
                        res.Message = "Error al guardar la información";
                    }
                    else
                    {
                        res.Object = InformacionIncial;
                        res.Message = "La Información se ha actualizado Satisfactoriamente";
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
        /// osvaldo valdez
        /// 28/08/2018
        /// </summary>
        /// <param name="fechaIni">fecha inicio</param>
        /// <param name="codMoneda">codido de la moneda</param>
        /// <param name="reajuste">codigo de reajuste</param>
        /// <param name="departamento">codigo del departamento</param>
        /// <param name="usuario">nombre del usuario</param>
        /// <returns>elimina el periodo seleccionado y la informacion relacionada</returns>
        public object EliminarCorizacionMejorada(DateTime fechaIni, string codMoneda, int reajuste, string departamento,string usuario)
        {
            try
            {
                Response res = new Response();
                res.Message = "La Información se ha Eliminado Satisfactoriamente";
                LimiteCotizacionMejorada InformacionIncial = new LimiteCotizacionMejorada();
                LimiteCotizacionMejorada InformacionBorrar = new LimiteCotizacionMejorada();
                LimiteCotizacionMejorada InformacionIntermedia = new LimiteCotizacionMejorada();

                //buscamos la informacion que se va a borrar 
                InformacionBorrar = _LimiteCotizacionMejoradaRepository.ConsultaInfo(fechaIni, codMoneda, reajuste,Convert.ToInt32(departamento));
                InformacionBorrar.CodMoneda = codMoneda;
                InformacionBorrar.CodReajuste = reajuste;
                InformacionBorrar.CodDepartamento = departamento;
                //'Devuelve EL DIA ANTES DE LA FECHA DE INICIO QUE ESTAMOS ELIMINANDO
                InformacionIncial = _LimiteCotizacionMejoradaRepository.EliminarCorizacionMejorada("BORRARVIGENCIA", fechaIni, fechaIni.AddDays(-1), InformacionBorrar);
                if (InformacionIncial == null)
                {//si se borran todos los registros
                    InformacionIncial = new LimiteCotizacionMejorada();
                    InformacionIncial.FechaFinal = fechaIni.ToString("yyyy-MM-dd");
                    InformacionIncial.fechaInicial = fechaIni.ToString("yyyy-MM-dd");
                    res.Object = InformacionIncial;
                }
                else
                {
                    DateTime vgTopeFecFin = Convert.ToDateTime("9999-12-31");
                    DateTime vgTopeFecFinal = Convert.ToDateTime(InformacionBorrar.FechaFinal);//obtenermos la fecha fin de la fecha que se borro
                    DateTime fechaInicial = Convert.ToDateTime(InformacionIncial.fechaInicial);//obtenemos la fecha anterior a la que se borro

                    if (vgTopeFecFinal != vgTopeFecFin)
                    {
                        //fechas intermedias
                        InformacionIntermedia = _LimiteCotizacionMejoradaRepository.Grabar("VIGENCIAINTERMEDIA", InformacionBorrar, fechaInicial, fechaIni, usuario);
                        if (InformacionIntermedia != null)
                        {
                            //CAMBIA LA FECHA DEL TERMINO DE VIGENCIA DEL REGISTRO 
                            vgTopeFecFinal = Convert.ToDateTime(InformacionIntermedia.fechaInicial);
                            //recuperamos la fecha siguiente y restamos 1 dia a la fecha siguiente para acualizar la fecha fin del registro ingresado
                            _LimiteCotizacionMejoradaRepository.Grabar("ACTUALIZARVIGENCIA", InformacionBorrar, fechaInicial, vgTopeFecFinal.AddDays(-1), usuario);

                            InformacionIncial.RangosTasaVenta = _LimiteCotizacionMejoradaRepository.RangosTasaVenta(fechaInicial, InformacionBorrar.CodMoneda, InformacionBorrar.CodReajuste, Convert.ToInt32(InformacionBorrar.CodDepartamento));
                            //actualiamos la fecha de las tasa de ventas anterior
                            foreach (var item in InformacionIncial.RangosTasaVenta)
                            {
                                _LimiteCotizacionMejoradaRepository.GrabarTasaVenta("ACTUALIZARVIGENCIATV", InformacionBorrar, fechaInicial, vgTopeFecFinal.AddDays(-1), usuario, item.CodPension, 0, 0);
                            }
                            InformacionIncial.FechaFinal = fechaInicial.AddDays(-1).ToString("yyyy-MM-dd");
                        }
                    }
                    else
                    {
                        _LimiteCotizacionMejoradaRepository.Grabar("ACTUALIZARVIGENCIA", InformacionBorrar, fechaInicial, vgTopeFecFin, usuario);
                        InformacionIncial.RangosTasaVenta = _LimiteCotizacionMejoradaRepository.RangosTasaVenta(fechaInicial, InformacionBorrar.CodMoneda, InformacionBorrar.CodReajuste, Convert.ToInt32(InformacionBorrar.CodDepartamento));
                        //actualiamos la fecha de las tasa de ventas anterior
                        foreach (var item in InformacionIncial.RangosTasaVenta)
                        {
                            _LimiteCotizacionMejoradaRepository.GrabarTasaVenta("ACTUALIZARVIGENCIATV", InformacionBorrar, fechaInicial, vgTopeFecFin, usuario, item.CodPension, 0, 0);
                        }
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
        /// <summary>
        /// osvaldo valdez
        /// 31/08/2018
        /// </summary>
        /// <param name="codMoneda">codigo de moneda</param>
        /// <param name="reajuste">valor del reajuste</param>
        /// <param name="moneda">valor de la moneda</param>
        /// <param name="departamento">valor del departamento</param>
        /// <param name="nomDepartamento">nombre del departamento</param>
        /// <param name="fechaInicial">fecha inicial</param>
        /// <returns>retorna la informacion para el reporte</returns>
        public List<LimiteCotizacionMejorada> ConsultaRpt(string codMoneda, int reajuste, string moneda, string departamento, string nomDepartamento, DateTime fechaInicial)
        {
            try
            {
                List<LimiteCotizacionMejorada> registros = _LimiteCotizacionMejoradaRepository.ConsultaRpt(codMoneda, reajuste, moneda, departamento, nomDepartamento, fechaInicial);
                return registros;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
        /// <summary>
        /// Omar Figueroa Flores
        /// 29-01-2019
        /// </summary>
        public Response gurdarDatosMasivos(List<LimiteCotizacionMejorada> datos, string usuario, string tipoRenta)
        {
            Response res = new Response();
            try
            {
                string querys = "";
                for (int i = 0; i < datos.Count; i++)
                {
                    if (datos[i].accion == "I")
                    {
                        if(i == 0)
                        {
                            DateTime fecha = DateTime.Now;
                            querys = "UPDATE PT_TVAL_MINMAXTIR_MEJ SET FEC_TERMINMAX = '" + fecha.ToString("yyyyMMdd") + "' WHERE FEC_TERMINMAX = '99991231'";
                            querys = "\n"+" UPDATE PT_TVAL_MINMAXPER_MEJ SET FEC_TERMINMAX = '" + fecha.ToString("yyyyMMdd") + "' WHERE FEC_TERMINMAX = '99991231'";
                            querys = "\n"+ " UPDATE PT_TVAL_MINMAXTAS_MEJ SET FEC_TERMINMAX = '" + fecha.ToString("yyyyMMdd") + "' WHERE FEC_TERMINMAX = '99991231'";
                        }
                        querys += "\n" + insertCargaMasiva(datos[i], usuario, tipoRenta);
                    }
                    else
                    {
                        querys += "\n" + updateCargaMasiva(datos[i], usuario, tipoRenta);
                    }
                }
                _LimiteCotizacionMejoradaRepository.EjecutarScript(querys);
                res.Message = "Carga masiva realizada con exito";
                res.IsOk = true;
                return res;
            }
            catch (Exception ex)
            {
                res.IsOk = false;
                res.Message = ex.Message;
                return res;
            }
        }
        /// <summary>
        /// Omar Figueroa Flores
        /// 01-02-2019
        /// </summary>
        public string insertCargaMasiva(LimiteCotizacionMejorada datos, string usuario, string tipoRenta)
        {
            DateTime fecha = DateTime.Now;
            string codMoneda = "";
            string codTipReajuste = "";
            string tir = "";
            switch (datos.moneda)
            {
                case "Soles Ajustados": codMoneda = "NS"; codTipReajuste = "2"; break;
                case "Soles Indexados": codMoneda = "NS"; codTipReajuste = "1"; break;
                case "Dólares Ajustados": codMoneda = "US"; codTipReajuste = "2"; break;
                case "Dólares Nominales": codMoneda = "US"; codTipReajuste = "0"; break;
            }


            if (tipoRenta == "1")
            {
                tir = "INSERT INTO PT_TVAL_MINMAXTIR_MEJ (COD_REGION,COD_MONEDA,COD_TIPREAJUSTE,FEC_INIMINMAX,FEC_TERMINMAX,prc_minimo_1,PRC_MAXIMO,COD_USUARIOCREA,FEC_CREA,HOR_CREA) " +
                        "VALUES ('" + datos.codRegion + "','" + codMoneda + "','" + codTipReajuste + "','" + fecha.ToString("yyyyMMdd") + "','99991231'," + datos.tir + ", 999.99,'" + usuario + "','" + fecha.ToString("yyyyMMdd") + "','" + fecha.ToString("hhmmss") + "')";
            }
            else
            {
                tir = "INSERT INTO PT_TVAL_MINMAXTIR_MEJ (COD_REGION,COD_MONEDA,COD_TIPREAJUSTE,FEC_INIMINMAX,FEC_TERMINMAX,PRC_MINIMO,PRC_MAXIMO,COD_USUARIOCREA,FEC_CREA,HOR_CREA) " +
                             "VALUES ('" + datos.codRegion + "','" + codMoneda + "','" + codTipReajuste + "','" + fecha.ToString("yyyyMMdd") + "','99991231'," + datos.tir + ", 999.99,'" + usuario + "','" + fecha.ToString("yyyyMMdd") + "','" + fecha.ToString("hhmmss") + "')";
            }
            string prd = "\nINSERT INTO PT_TVAL_MINMAXPER_MEJ (COD_REGION,COD_MONEDA,COD_TIPREAJUSTE,FEC_INIMINMAX,FEC_TERMINMAX,PRC_MINIMO,PRC_MAXIMO,COD_USUARIOCREA,FEC_CREA,HOR_CREA) " +
                        "VALUES ('" + datos.codRegion + "','" + codMoneda + "','" + codTipReajuste + "','" + fecha.ToString("yyyyMMdd") + "','99991231',0.00," + datos.prd + ",'" + usuario + "','" + fecha.ToString("yyyyMMdd") + "','" + fecha.ToString("hhmmss") + "')";

            string it = "\nINSERT INTO PT_TVAL_MINMAXTAS_MEJ (COD_REGION,COD_MONEDA,COD_TIPREAJUSTE,COD_TIPPENSION,FEC_INIMINMAX,FEC_TERMINMAX,PRC_MINIMO,PRC_MAXIMO,COD_USUARIOCREA,FEC_CREA,HOR_CREA) " +
                        "VALUES ('" + datos.codRegion + "','" + codMoneda + "','" + codTipReajuste + "','06','" + fecha.ToString("yyyyMMdd") + "','99991231',0.00," + datos.it + ",'" + usuario + "','" + fecha.ToString("yyyyMMdd") + "','" + fecha.ToString("hhmmss") + "')";

            string ip = "\nINSERT INTO PT_TVAL_MINMAXTAS_MEJ (COD_REGION,COD_MONEDA,COD_TIPREAJUSTE,COD_TIPPENSION,FEC_INIMINMAX,FEC_TERMINMAX,PRC_MINIMO,PRC_MAXIMO,COD_USUARIOCREA,FEC_CREA,HOR_CREA) " +
                        "VALUES ('" + datos.codRegion + "','" + codMoneda + "','" + codTipReajuste + "','07','" + fecha.ToString("yyyyMMdd") + "','99991231',0.00," + datos.ip + ",'" + usuario + "','" + fecha.ToString("yyyyMMdd") + "','" + fecha.ToString("hhmmss") + "')";

            string s = "\nINSERT INTO PT_TVAL_MINMAXTAS_MEJ (COD_REGION,COD_MONEDA,COD_TIPREAJUSTE,COD_TIPPENSION,FEC_INIMINMAX,FEC_TERMINMAX,PRC_MINIMO,PRC_MAXIMO,COD_USUARIOCREA,FEC_CREA,HOR_CREA) " +
                        "VALUES ('" + datos.codRegion + "','" + codMoneda + "','" + codTipReajuste + "','08','" + fecha.ToString("yyyyMMdd") + "','99991231',0.00," + datos.s + ",'" + usuario + "','" + fecha.ToString("yyyyMMdd") + "','" + fecha.ToString("hhmmss") + "')";

            string ja = "\nINSERT INTO PT_TVAL_MINMAXTAS_MEJ (COD_REGION,COD_MONEDA,COD_TIPREAJUSTE,COD_TIPPENSION,FEC_INIMINMAX,FEC_TERMINMAX,PRC_MINIMO,PRC_MAXIMO,COD_USUARIOCREA,FEC_CREA,HOR_CREA) " +
                        "VALUES ('" + datos.codRegion + "','" + codMoneda + "','" + codTipReajuste + "','04','" + fecha.ToString("yyyyMMdd") + "','99991231',0.00," + datos.ja + ",'" + usuario + "','" + fecha.ToString("yyyyMMdd") + "','" + fecha.ToString("hhmmss") + "')";

            string jl = "\nINSERT INTO PT_TVAL_MINMAXTAS_MEJ (COD_REGION,COD_MONEDA,COD_TIPREAJUSTE,COD_TIPPENSION,FEC_INIMINMAX,FEC_TERMINMAX,PRC_MINIMO,PRC_MAXIMO,COD_USUARIOCREA,FEC_CREA,HOR_CREA) " +
                        "VALUES ('" + datos.codRegion + "','" + codMoneda + "','" + codTipReajuste + "','05','" + fecha.ToString("yyyyMMdd") + "','99991231',0.00," + datos.jl + ",'" + usuario + "','" + fecha.ToString("yyyyMMdd") + "','" + fecha.ToString("hhmmss") + "')";

            return tir + prd + it + ip + s + ja + jl;
        }
        /// <summary>
        /// Omar Figueroa Flores
        /// 01-02-2019
        /// </summary>
        public string updateCargaMasiva(LimiteCotizacionMejorada datos, string usuario, string tipoRenta)
        {
            DateTime fecha = DateTime.Now;
            string codMoneda = "";
            string codTipReajuste = "";
            string tir = "";

            switch (datos.moneda)
            {
                case "Soles Ajustados": codMoneda = "NS"; codTipReajuste = "2"; break;
                case "Soles Indexados": codMoneda = "NS"; codTipReajuste = "1"; break;
                case "Dólares Ajustados": codMoneda = "US"; codTipReajuste = "2"; break;
                case "Dólares Nominales": codMoneda = "US"; codTipReajuste = "0"; break;
            }

            if (tipoRenta == "1")
            {
                tir = "UPDATE PT_TVAL_MINMAXTIR_MEJ SET PRC_MINIMO_1 = " + datos.tir + " ,COD_USUARIOMODI = '" + usuario + "' ,FEC_MODI = '" + fecha.ToString("yyyyMMdd") + "' ,HOR_MODI = '" + fecha.ToString("hhmmss") +
                                          "' WHERE COD_REGION = '" + datos.codRegion + "' AND COD_MONEDA = '" + codMoneda + "' AND COD_TIPREAJUSTE = '" + codTipReajuste + "' AND FEC_TERMINMAX = '" + datos.accion + "'";
            }
            else
            {
                tir = "UPDATE PT_TVAL_MINMAXTIR_MEJ SET PRC_MINIMO = " + datos.tir + " ,COD_USUARIOMODI = '" + usuario + "' ,FEC_MODI = '" + fecha.ToString("yyyyMMdd") + "' ,HOR_MODI = '" + fecha.ToString("hhmmss") +
                              "' WHERE COD_REGION = '" + datos.codRegion + "' AND COD_MONEDA = '" + codMoneda + "' AND COD_TIPREAJUSTE = '" + codTipReajuste + "' AND FEC_TERMINMAX = '" + datos.accion + "'";
            }
            string prd = "\nUPDATE PT_TVAL_MINMAXPER_MEJ SET PRC_MAXIMO = " + datos.prd + " ,COD_USUARIOMODI = '" + usuario + "' ,FEC_MODI = '" + fecha.ToString("yyyyMMdd") + "' ,HOR_MODI = '" + fecha.ToString("hhmmss") +
                          "' WHERE COD_REGION = '" + datos.codRegion + "' AND COD_MONEDA = '" + codMoneda + "' AND COD_TIPREAJUSTE = '" + codTipReajuste + "' AND FEC_TERMINMAX = '" + datos.accion + "'";

            string it = "\nUPDATE PT_TVAL_MINMAXTAS_MEJ SET PRC_MAXIMO = " + datos.it + " ,COD_USUARIOMODI = '" + usuario + "' ,FEC_MODI = '" + fecha.ToString("yyyyMMdd") + "' ,HOR_MODI = '" + fecha.ToString("hhmmss") +
                        "' WHERE COD_REGION = '" + datos.codRegion + "' AND COD_MONEDA = '" + codMoneda + "' AND COD_TIPREAJUSTE = '" + codTipReajuste + "' AND FEC_TERMINMAX = '" + datos.accion + "' AND COD_TIPPENSION = '06'";

            string ip = "\nUPDATE PT_TVAL_MINMAXTAS_MEJ SET PRC_MAXIMO = " + datos.ip + " ,COD_USUARIOMODI = '" + usuario + "' ,FEC_MODI = '" + fecha.ToString("yyyyMMdd") + "' ,HOR_MODI = '" + fecha.ToString("hhmmss") +
                        "' WHERE COD_REGION = '" + datos.codRegion + "' AND COD_MONEDA = '" + codMoneda + "' AND COD_TIPREAJUSTE = '" + codTipReajuste + "' AND FEC_TERMINMAX = '" + datos.accion + "' AND COD_TIPPENSION = '07'";

            string s = "\nUPDATE PT_TVAL_MINMAXTAS_MEJ SET PRC_MAXIMO = " + datos.s + " ,COD_USUARIOMODI = '" + usuario + "' ,FEC_MODI = '" + fecha.ToString("yyyyMMdd") + "' ,HOR_MODI = '" + fecha.ToString("hhmmss") +
                        "' WHERE COD_REGION = '" + datos.codRegion + "' AND COD_MONEDA = '" + codMoneda + "' AND COD_TIPREAJUSTE = '" + codTipReajuste + "' AND FEC_TERMINMAX = '" + datos.accion + "' AND COD_TIPPENSION = '08'";

            string ja = "\nUPDATE PT_TVAL_MINMAXTAS_MEJ SET PRC_MAXIMO = " + datos.ja + " ,COD_USUARIOMODI = '" + usuario + "' ,FEC_MODI = '" + fecha.ToString("yyyyMMdd") + "' ,HOR_MODI = '" + fecha.ToString("hhmmss") +
                        "' WHERE COD_REGION = '" + datos.codRegion + "' AND COD_MONEDA = '" + codMoneda + "' AND COD_TIPREAJUSTE = '" + codTipReajuste + "' AND FEC_TERMINMAX = '" + datos.accion + "' AND COD_TIPPENSION = '04'";

            string jl = "\nUPDATE PT_TVAL_MINMAXTAS_MEJ SET PRC_MAXIMO = " + datos.jl + " ,COD_USUARIOMODI = '" + usuario + "' ,FEC_MODI = '" + fecha.ToString("yyyyMMdd") + "' ,HOR_MODI = '" + fecha.ToString("hhmmss") +
                        "' WHERE COD_REGION = '" + datos.codRegion + "' AND COD_MONEDA = '" + codMoneda + "' AND COD_TIPREAJUSTE = '" + codTipReajuste + "' AND FEC_TERMINMAX = '" + datos.accion + "' AND COD_TIPPENSION = '05'";

            return tir + prd + it + ip + s + ja + jl;
        }
        /// <summary>
        /// Omar Figueroa Flores
        /// 01-02-2019
        /// </summary>
        public List<LimiteCotizacionMejorada> getCodRegion(string glsRegion)
        {
            try
            {
                return _LimiteCotizacionMejoradaRepository.getCodRegion(glsRegion);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
