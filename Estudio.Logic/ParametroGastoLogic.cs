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
    public class ParametroGastoLogic
    {
        CatalogosOficialesRepository _CatalogosSegurosRV = new CatalogosOficialesRepository();
        ParametroGastoRepository _ParametroGastoRepository = new ParametroGastoRepository();

        /// <summary>
        /// Llenar combo de tipo de moneda.
        /// José Hernández Alvarado.
        /// 30-08-2018
        /// </summary>
        /// <returns>Regresa una lista con los tipos de moneda.</returns>
        public List<Moneda> TiposMoneda()
        {
            return _CatalogosSegurosRV.TiposMoneda();
        }

        /// <summary>
        /// Llena combo de periodos.
        /// José Hernández Alvarado.
        /// 31-08-2018
        /// </summary>
        /// <param name="vlMoneda">Tipo de moneda.</param>
        /// <param name="vlReajuste">Valor de reajuste.</param>
        /// <returns>Retorna lista con periodos del tipo de moneda.</returns>
        public Response ListaPeriodos(string vlMoneda, int vlReajuste)
        {
            try
            {
                Response res = new Response();
                res.IsOk = true;
                res.Object = _ParametroGastoRepository.ListaPeriodos(vlMoneda, vlReajuste);
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
        /// Llena campos de la vista con los valores del periodo.
        /// José Hernández Alvarado.
        /// 04-09-2018
        /// </summary>
        /// <param name="strFecIni">Fecha de inicio de vigencia.</param>
        /// <param name="vlMoneda">Tipo de moneda.</param>
        /// <param name="vlReajuste">Valor de reajuste</param>
        /// <returns>Retorna los valores para mostrarlos en pantalla.</returns>
        public Response BuscarVigencia(DateTime strFecIni, string vlMoneda, int vlReajuste)
        {
            try
            {
                Response res = new Response();
                res.Object = _ParametroGastoRepository.BuscarVigencia(strFecIni, vlMoneda, vlReajuste);
                if (res.Object == null)
                {
                    ParametroGasto InformacionIncial = new ParametroGasto();
                    InformacionIncial.FechaTermino = "9999-12-31";
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
        /// Inserta un nuevo periodo en caso de no existir o modifica el ya existente.
        /// José Hernández Alvarado.
        /// 06-09-2018
        /// </summary>
        /// <param name="vlMoneda">Tipo de la moneda.</param>
        /// <param name="vlReajuste">Valor de reajuste.</param>
        /// <param name="strFecIni">Fecha iniciod del periodo.</param>
        /// <param name="strFecFin">Fecha fin del periodo.</param>
        /// <param name="gastosCS">Gastos de Control de Supervivencia.</param>
        /// <param name="gastosA">Gastos de Administración.</param>
        /// <param name="gastosE">Gastos de Emisión.</param>
        /// <param name="ctoCapital">Valor de Capital.</param>
        /// <param name="nivelE">Nivel de Endeudamiento.</param>
        /// <param name="usuario">Usuario logeado.</param>
        /// <param name="bandera">Bandera.</param>
        /// <returns></returns>
        public Response GrabarParametro(string vlMoneda, int vlReajuste, DateTime strFecIni, DateTime strFecFin, decimal gastosCS, decimal gastosA, decimal gastosE, decimal ctoCapital, decimal nivelE, string usuario, Boolean bandera)
        {
            try
            {
                Response res = new Response();
                ParametroGasto InformacionIncial = new ParametroGasto();

                if (bandera)
                {
                    //realiza la inserción
                    InformacionIncial = _ParametroGastoRepository.GrabarParametro(vlMoneda, vlReajuste, strFecIni, strFecFin, gastosCS, gastosA, gastosE, ctoCapital, nivelE, usuario, "GRABARVIGENCIA");
                    if (InformacionIncial == null)
                    {
                        InformacionIncial = new ParametroGasto();
                        //fechas intermedias
                        InformacionIncial = _ParametroGastoRepository.GrabarParametro(vlMoneda, vlReajuste, strFecIni, strFecFin, 0, 0, 0, 0, 0, usuario, "VIGENCIAINTERMEDIA");
                        if (InformacionIncial == null)
                        {
                            InformacionIncial = new ParametroGasto();
                            InformacionIncial.FechaTermino = strFecFin.ToString("yyyy-MM-dd");
                            InformacionIncial.PRC_GastosCtrlSuper = gastosCS;
                            InformacionIncial.Mto_GastosAdmin = gastosA;
                            InformacionIncial.Mto_GastosEmi = gastosE;
                            InformacionIncial.Cto_Capital = ctoCapital;
                            InformacionIncial.PRC_Endeudamiento = nivelE;
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
                            _ParametroGastoRepository.GrabarParametro(vlMoneda, vlReajuste, strFecIni, fechaInicial.AddDays(-1), 0, 0, 0, 0, 0, usuario, "ACTUALIZARVIGENCIA");
                            InformacionIncial.FechaTermino = fechaInicial.AddDays(-1).ToString("yyyy-MM-dd");
                        }
                        InformacionIncial.PRC_GastosCtrlSuper = gastosCS;
                        InformacionIncial.Mto_GastosAdmin = gastosA;
                        InformacionIncial.Mto_GastosEmi = gastosE;
                        InformacionIncial.Cto_Capital = ctoCapital;
                        InformacionIncial.PRC_Endeudamiento = nivelE;
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
                        _ParametroGastoRepository.GrabarParametro(vlMoneda, vlReajuste, fechaInicial, strFecIni.AddDays(-1), 0, 0, 0, 0, 0, usuario, "ACTUALIZARVIGENCIA");
                        InformacionIncial.FechaTermino = strFecFin.ToString("yyyy-MM-dd");
                        res.Message = "Información cargada con éxito";

                        //actualiza la fecha de termino del periodo siguiente si existe
                        //fechas intermedias
                        InformacionIncial = _ParametroGastoRepository.GrabarParametro(vlMoneda, vlReajuste, strFecIni, strFecFin, 0, 0, 0, 0, 0, usuario, "VIGENCIAINTERMEDIA");
                        if (InformacionIncial == null)
                        {
                            InformacionIncial = new ParametroGasto();
                            InformacionIncial.FechaTermino = strFecFin.ToString("yyyy-MM-dd");
                            InformacionIncial.PRC_GastosCtrlSuper = gastosCS;
                            InformacionIncial.Mto_GastosAdmin = gastosA;
                            InformacionIncial.Mto_GastosEmi = gastosE;
                            InformacionIncial.Cto_Capital = ctoCapital;
                            InformacionIncial.PRC_Endeudamiento = nivelE;
                            InformacionIncial.FechaInicial = strFecIni.ToString("yyyy-MM-dd");
                            res.Object = InformacionIncial;
                            res.IsOk = true;
                            return res;
                        }
                        else
                        {
                            //CAMBIA LA FECHA DEL TERMINO DE VIGENCIA DEL REGISTRO INGRESADDO
                            fechaInicial = Convert.ToDateTime(InformacionIncial.FechaInicial);
                            

                            _ParametroGastoRepository.GrabarParametro(vlMoneda, vlReajuste, strFecIni, fechaInicial.AddDays(-1), 0, 0, 0, 0, 0, usuario, "ACTUALIZARVIGENCIA");
                            InformacionIncial.FechaTermino = fechaInicial.AddDays(-1).ToString("yyyy-MM-dd");
                        }
                    }
                    res.Object = InformacionIncial;
                    res.IsOk = true;
                    return res;

                }
                else
                {//update
                    InformacionIncial = _ParametroGastoRepository.GrabarParametro(vlMoneda, vlReajuste, strFecIni, strFecFin, gastosCS, gastosA, gastosE, ctoCapital, nivelE, usuario, "ACTUALIZARPARAMETROS");
                    if (InformacionIncial == null)
                    {
                        InformacionIncial = new ParametroGasto();
                        InformacionIncial.FechaTermino = strFecFin.ToString("yyyy-MM-dd");
                        InformacionIncial.FechaTermino = strFecFin.ToString("yyyy-MM-dd");
                        InformacionIncial.PRC_GastosCtrlSuper = gastosCS;
                        InformacionIncial.Mto_GastosAdmin = gastosA;
                        InformacionIncial.Mto_GastosEmi = gastosE;
                        InformacionIncial.Cto_Capital = ctoCapital;
                        InformacionIncial.PRC_Endeudamiento = nivelE;
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
        /// Elimina el registro de la base de datos y actualiza las fechas de los periodos antes o después del que se elimina.
        /// José Hernández Alvarado.
        /// 07-09-2018
        /// </summary>
        /// <param name="vlMoneda">Tipo de la moneda.</param>
        /// <param name="vlReajuste">Valor de reajuste.</param>
        /// <param name="strFecIni">Fecha de inicio del periodo.</param>
        /// <param name="usuario">Usuario logeado.</param>
        /// <returns></returns>
        public Response EliminarParametro(string vlMoneda, int vlReajuste, DateTime strFecIni,  string usuario)
        {
            try
            {
                Response res = new Response();
                res.Message = "La Información se ha Eliminado Satisfactoriamente";
                ParametroGasto InformacionIncial = new ParametroGasto();
                ParametroGasto InformacionBorrar = new ParametroGasto();
                ParametroGasto InformacionIntermedia = new ParametroGasto();

                //buscamos la informacion que se va a borrar 
                InformacionBorrar = _ParametroGastoRepository.BuscarVigencia(strFecIni, vlMoneda, vlReajuste);

                //'Devuelve EL DIA ANTES DE LA FECHA DE INICIO QUE ESTAMOS ELIMINANDO
                InformacionIncial = _ParametroGastoRepository.EliminarParametro(strFecIni, strFecIni.AddDays(-1), vlMoneda, vlReajuste, "ELIMINARVIGENCIA");

                if (InformacionIncial == null)
                {//si se borran todos los registros
                    InformacionIncial = new ParametroGasto();
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
                        InformacionIntermedia = _ParametroGastoRepository.GrabarParametro(vlMoneda, vlReajuste, fechaInicial, strFecIni, 0, 0, 0, 0, 0, usuario, "VIGENCIAINTERMEDIA");
                        if (InformacionIntermedia != null)
                        {
                            //CAMBIA LA FECHA DEL TERMINO DE VIGENCIA DEL REGISTRO INGRESADDO
                            vgTopeFecFinal = Convert.ToDateTime(InformacionIntermedia.FechaInicial);
                            //recuperamos la fecha siguiente y restamos 1 dia a la fecha siguiente para acualizar la fecha fin del registro ingresado
                            _ParametroGastoRepository.GrabarParametro(vlMoneda, vlReajuste, fechaInicial, vgTopeFecFinal.AddDays(-1), 0, 0, 0, 0, 0, usuario, "ACTUALIZARVIGENCIA");
                        }
                    }
                    else
                    {
                        _ParametroGastoRepository.GrabarParametro(vlMoneda, vlReajuste, fechaInicial, vgTopeFecFinal, 0, 0, 0, 0, 0, usuario, "ACTUALIZARVIGENCIA");

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
        /// Llenar campo de Tasa de Mercado.
        /// José Hernández Alvarado.
        /// 05-09-2018
        /// </summary>
        /// <param name="vlMoneda">Tipo de moneda.</param>
        /// <param name="vlReajuste">Valor de reajuste.</param>
        /// <returns>Retorna valor de la tasa de mercado.</returns>
        public Response TasaMercado(string vlMoneda, int vlReajuste)
        {
            try
            {
                Response res = new Response();
                res.Object = _ParametroGastoRepository.ConsultarTM(vlMoneda, vlReajuste);
                if (res.Object == null)
                {
                    ParametroGasto InformacionIncial = new ParametroGasto();
                    InformacionIncial.PRC_TasaMercado = 0;
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
        /// Llenar campo de Impuesto de la Renta.
        /// José Hernández Alvarado.
        /// 05-09-2018
        /// </summary>
        /// <param name="strFecIni">Fecha de inicio de vigencia.</param>
        /// <returns>Retorna el impuesto de la renta.</returns>
        public Response ImpuestoRenta(DateTime strFecIni)
        {
            try
            {
                Response res = new Response();
                res.Object = _ParametroGastoRepository.ConsultarIR(strFecIni);
                if (res.Object == null)
                {
                    ParametroGasto InformacionIncial = new ParametroGasto();
                    InformacionIncial.PRC_ImpuestoRenta = 0;
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

        public List<ParametroGasto> ReportePG(string vlMoneda, string strMoneda, int vlReajuste, DateTime strFecIni)
        {
            try
            {

                return _ParametroGastoRepository.ConsultaRptPG(vlMoneda, strMoneda, vlReajuste, strFecIni);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public Response ConsultarGastos()
        {
            try
            {
                Response res = new Response();
                res.Object = _ParametroGastoRepository.ConsultarGastos();
                if (res.Object == null)
                {
                    ParametroGasto InformacionIncial = new ParametroGasto();
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

        public Response GuardarOtrosGastos(decimal COMSUP1, decimal COMSUP2, decimal PRCFAC1, decimal PRCFAC2)
        {
            try
            {
                Response res = new Response();
                res.Object = _ParametroGastoRepository.GuardarOtrosGastos(COMSUP1, COMSUP2, PRCFAC1, PRCFAC2);
                if (res.Object == null)
                {
                    res.Object = null;
                    res.Message = res.Message;
                }
                else
                {
                    res.Message = "Información guardada con éxito";
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
