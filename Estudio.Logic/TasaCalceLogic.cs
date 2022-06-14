using System;
using System.Collections.Generic;
using Estudio.Repository.Persistence.Repositories;
using Estudio.Repository.Helpers;
using Estudio.Repository.Core.Domain;
using System.Globalization;

namespace Estudio.Logic
{
    public class TasaCalceLogic
    {
        CatalogosOficialesRepository _catalogoOficialRepository = new CatalogosOficialesRepository();
        TasaCalceRepository _tasaCalceRepository = new TasaCalceRepository();

        /// <summary>
        /// Antonio Quezada
        /// 2018-08-15
        /// Carga el combo de Tipos de Moneda
        /// </summary>
        /// <returns> Regresa una lista con los Tipos de Moneda </returns>

        public List<Moneda> TiposMoneda()
        {
            return _catalogoOficialRepository.TiposMoneda();
        }

        /// <summary>
        /// Antonio Quezada
        /// 2018-08-14
        /// Carga el combo de Periodos de Tasa Calce
        /// </summary>
        /// <param name="tipoMoneda"> Tipo de la Moneda y Tipo de Reajuste</param>
        /// <returns> Regresa una lista de Periodos para Tasa Calce </returns>

        public List<Periodo> ConsultaPeriodos(string tipoMoneda)
        {
            string[] codigoVector = tipoMoneda.Split('#');
            string codigoMoneda = codigoVector[0];
            string reajuste = codigoVector[1];

            return _tasaCalceRepository.ConsultaPeriodos(codigoMoneda, reajuste);
        }

        /// <summary>
        /// Antonio Quezada
        /// 2018-08-15
        /// Obtiene los periodos según el combo de Tipo de Moneda seleccionado
        /// </summary>
        /// <param name="tipoMoneda"> Tipo de la Moneda y Tipo de Reajuste</param>
        /// <returns> Regresa un objeto que contiene la lista de Periodos </returns>

        public Response ConsultaPeriodosRes(string tipoMoneda)
        {
            try
            {
                Response res = new Response();

                string[] codigoVector = tipoMoneda.Split('#');
                string codigoMoneda = codigoVector[0];
                string reajuste = codigoVector[1];

                res.IsOk = true;
                res.Message = "Operación realizada con éxito";
                res.Object = _tasaCalceRepository.ConsultaPeriodos(codigoMoneda, reajuste);

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
        /// Antonio Quezada
        /// 2018-08-16
        /// Obtiene los registros de la Tasa de Descuento Anual
        /// </summary>
        /// <param name="periodo"> Periodo seleccionado </param>
        /// <param name="tipoMoneda"> Tipo de la Moneda </param>
        /// <returns> Regresa un objeto que contiene las Tasas de Descuento Anual </returns>

        public Response ConsultaTasaDescAnual(string periodo, string tipoMoneda)
        {
            try
            {
                Response res = new Response();
                List<TasaDescuentoAnual> tasasDescuentoAnual = new List<TasaDescuentoAnual>();
                int valMax = 10;

                if (periodo != "" && tipoMoneda != "")
                {
                    string[] codigoVector = tipoMoneda.Split('#');
                    string codigoMoneda = codigoVector[0];
                    string reajuste = codigoVector[1];

                    string[] periodoVector = periodo.Split('*');
                    string fechaInicio = Convert.ToDateTime(periodoVector[0]).ToString("yyyyMMdd");

                    tasasDescuentoAnual = _tasaCalceRepository.ConsultaTasaDescAnual(fechaInicio, codigoMoneda, reajuste);
                }

                if (tasasDescuentoAnual.Count == 0)
                {
                    for (int i = 1; i <= valMax; i++)
                    {
                        TasaDescuentoAnual tasa = new TasaDescuentoAnual() { Tramo = i, CPK = "0.00000000" };
                        tasasDescuentoAnual.Add(tasa);
                    }
                }

                res.IsOk = true;
                res.Message = "Operación realizada con éxito";
                res.Object = tasasDescuentoAnual;

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
        /// Antonio Quezada
        /// 2018-08-20
        /// Consulta de la información por Tramo
        /// </summary>
        /// <param name="tramo"> Número de Año (Tramo) </param>
        /// <param name="periodo"> Periodo seleccionado </param>
        /// <param name="tipoMoneda"> Tipo de la Moneda </param>
        /// <returns> Regresa un objeto que contiene la información del tramo en un objeto </returns>

        public Response ConsultaTramo(int tramo, string periodo, string tipoMoneda)
        {
            try
            {
                Response res = new Response();
                TasaDescuentoAnual tasaDesc = new TasaDescuentoAnual();

                if (periodo != "" && tipoMoneda != "")
                {
                    string[] codigoVector = tipoMoneda.Split('#');
                    string codigoMoneda = codigoVector[0];
                    string reajuste = codigoVector[1];

                    string[] periodoVector = periodo.Split('*');
                    string fechaInicio = Convert.ToDateTime(periodoVector[0]).ToString("yyyyMMdd");

                    tasaDesc = _tasaCalceRepository.ConsultaTramo(tramo, fechaInicio, codigoMoneda, reajuste);

                    if (tasaDesc == null)
                        tasaDesc = new TasaDescuentoAnual { Tramo = tramo, CPK = "0.00000000" };
                }

                res.IsOk = true;
                res.Message = "Operación realizada con éxito";
                res.Object = tasaDesc;

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
        /// Antonio Quezada
        /// 2018-08-21
        /// Obtiene el Periodo de Vigencia
        /// </summary>
        /// <param name="fechaInicio"> Fecha de Inicio de la Vigencia </param>
        /// <param name="tipoMoneda"> Tipo de la Moneda </param>
        /// <returns> Regresa la Fecha de Término de la Vigencia </returns>

        public Response ConsultaPeriodoVigencia(DateTime fechaInicio, string tipoMoneda)
        {
            try
            {
                Response res = new Response();
                List<TasaDescuentoAnual> tasasDescuentoAnual = new List<TasaDescuentoAnual>();

                string[] codigoVector = tipoMoneda.Split('#');
                string codigoMoneda = codigoVector[0];
                string reajuste = codigoVector[1];
                string fechaTermino = "";
                string fechaInicioStr = "";
                int valMax = 10;

                fechaInicioStr = fechaInicio.ToString("yyyyMMdd");
                tasasDescuentoAnual = _tasaCalceRepository.ConsultaTasaDescAnual(fechaInicioStr, codigoMoneda, reajuste);

                if (tasasDescuentoAnual.Count != 0)
                    fechaTermino = tasasDescuentoAnual[0].FechaTerminoStr;
                else
                {
                    for (int i = 1; i <= valMax; i++)
                    {
                        TasaDescuentoAnual tasa = new TasaDescuentoAnual() { Tramo = i, CPK = "0.00000000" };
                        tasasDescuentoAnual.Add(tasa);
                    }
                }

                res.IsOk = true;
                res.Message = "Operación realizada con éxito";
                res.Object = new { fechaTermino = fechaTermino, tasasDesc = tasasDescuentoAnual };

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
        /// Antonio Quezada
        /// 2018-08-22
        /// Consulta si existe el Rango registrado en Base de Datos
        /// </summary>
        /// <param name="tipoMoneda"> Tipo de la Moneda </param>
        /// <param name="periodo"> Periodo seleccionado </param>
        /// <param name="eliminar"> Indica si se intenta eliminar la información de la Tasa (0 = Insertar o Actualizar, 1 = Eliminar </param>
        /// <returns> Regresa un objeto que indica si se va a Registrar o Modificar </returns>

        public Response VerificacionTasaCalce(string tipoMoneda, string periodo, int eliminar)
        {
            try
            {
                Response res = new Response();
                List<TasaDescuentoAnual> consultaTasas = new List<TasaDescuentoAnual>();
                DateTime fechaActual = DateTime.Now;
                string clave = "";
                string mensaje = "";

                string[] codigoVector = tipoMoneda.Split('#');
                string codigoMoneda = codigoVector[0];
                string reajuste = codigoVector[1];

                string[] periodoVector = periodo.Split('*');
                string fechaInicio = Convert.ToDateTime(periodoVector[0]).ToString("yyyyMMdd");
                string fechaTermino = Convert.ToDateTime(periodoVector[1]).ToString("yyyyMMdd");

                consultaTasas = _tasaCalceRepository.ConsultaTasaDescAnual(fechaInicio, codigoMoneda, reajuste);

                if (consultaTasas.Count == 0)
                {
                    if (eliminar == 0)
                    {
                        clave = "INSERT";
                        mensaje = "¿Está seguro que desea Guardar la Información?";
                    }
                    else
                    {
                        clave = "Aviso";
                        mensaje = "El Periodo que esta intentando eliminar no se encuentra en la BD";
                    }
                }
                else
                {
                    if (eliminar == 0)
                    {
                        clave = "UPDATE";
                        mensaje = "¿Está seguro que desea Modificar la Información?";
                    }
                    else
                    {
                        clave = "Confirmar";
                        mensaje = "¿Está seguro que desea Eliminar la Información?";
                    }
                }

                res.IsOk = true;
                res.Message = mensaje;
                res.Object = clave;
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
        /// Antonio Quezada
        /// 2018-08-22
        /// Registra o modifica la información de Tasa de Calce (Tramo)
        /// </summary>
        /// <param name="clave"> Indica si se va a Registrar o Modificar </param>
        /// <param name="tipoMoneda"> Tipo de la Moneda </param>
        /// <param name="periodo"> Periodo seleccionado </param>
        /// <param name="usuario"> Usuario activo en la aplicación </param>
        /// <param name="tasas"> Lista de información que será registrada o modificada </param>
        /// <returns> Regresa un objeto que indica si se realizó la transacción </returns>

        public Response RegistrarModificarTasaCalce(string clave, string tipoMoneda, string periodo, string usuario, List<TasaDescuentoAnual> tasas)
        {
            try
            {//INSERT  UPDATE
                Response res = new Response();
                TasaDescuentoAnual consultaTasas = new TasaDescuentoAnual();
                DateTime fechaActual = DateTime.Now;
               
                string[] codigoVector = tipoMoneda.Split('#');
                string codigoMoneda = codigoVector[0];
                string reajuste = codigoVector[1];
                string[] periodoVector = periodo.Split('*');
                DateTime fechaInicio = Convert.ToDateTime(periodoVector[0]);
                DateTime fechaTermino = Convert.ToDateTime("31/12/9999");

                foreach (var item in tasas)
                {
                    item.CodigoMoneda = codigoMoneda;
                    item.TipoReajuste = reajuste;
                    item.FechaInicioStr = fechaInicio.ToString("yyyyMMdd");
                    item.FechaTerminoStr = fechaTermino.ToString("yyyyMMdd");
                    item.Fecha = fechaActual.ToString("yyyyMMdd");
                    item.Hora = fechaActual.ToString("HHmmss");
                    item.Usuario = usuario;

                    _tasaCalceRepository.RegistrarModificarTasaCalce(clave, item);
                }

                if (clave == "INSERT")
                {//verificamos si existen fechas menores
                    //recuperamos la fecha
                    consultaTasas = _tasaCalceRepository.ConsultaFechasInicio("ANTERIOR", fechaInicio, fechaTermino, codigoMoneda, reajuste);
                    if (consultaTasas == null)
                    {//sino existen fechas menores buscamos fechas mayores
                        consultaTasas = new TasaDescuentoAnual();
                        consultaTasas = _tasaCalceRepository.ConsultaFechasInicio("REGISTRADO", fechaInicio, fechaTermino, codigoMoneda, reajuste);
                        if (consultaTasas == null)
                        {//sino existen fechas mayores es el tope de las fechas
                            consultaTasas = new TasaDescuentoAnual();
                            consultaTasas.FechaTerminoStr = fechaTermino.ToString("yyyy-MM-dd");
                        }
                        else
                        {   //CAMBIA LA FECHA DEL TERMINO DE VIGENCIA DEL REGISTRO INGRESADDO
                            fechaTermino = Convert.ToDateTime(consultaTasas.FechaInicioStr);
                            //recuperamos la fecha siguiente y restamos 1 dia a la fecha siguiente para acualizar la fecha fin del registro ingresado
                            _tasaCalceRepository.ConsultaFechasInicio("ACTFECHA", fechaInicio, fechaTermino.AddDays(-1), codigoMoneda, reajuste);
                            consultaTasas.FechaTerminoStr = fechaTermino.AddDays(-1).ToString("yyyy-MM-dd");
                        }
                    }
                    else {
                        //Actualiza la fecha de termino del periodo anterior 
                        DateTime fechaInicial = Convert.ToDateTime(consultaTasas.FechaInicioStr);
                        //recuperamos la fecha anterior y restamos 1 dia a la fecha ingresada para generar el periodo
                        _tasaCalceRepository.ConsultaFechasInicio("ACTFECHA", fechaInicial, fechaInicio.AddDays(-1), codigoMoneda, reajuste);
                        consultaTasas.FechaTerminoStr = fechaTermino.ToString("yyyy-MM-dd");
                        //actualiza la fecha de termino del periodo siguiente si existe
                        consultaTasas = _tasaCalceRepository.ConsultaFechasInicio("REGISTRADO", fechaInicio, fechaTermino, codigoMoneda, reajuste);
                        if (consultaTasas == null)
                        {//sino existen fechas mayores es el tope de las fechas
                            consultaTasas = new TasaDescuentoAnual();
                            consultaTasas.FechaTerminoStr = fechaTermino.ToString("yyyy-MM-dd");
                        }
                        else
                        {   //CAMBIA LA FECHA DEL TERMINO DE VIGENCIA DEL REGISTRO INGRESADDO
                            fechaTermino = Convert.ToDateTime(consultaTasas.FechaInicioStr);
                            //recuperamos la fecha siguiente y restamos 1 dia a la fecha siguiente para acualizar la fecha fin del registro ingresado
                            _tasaCalceRepository.ConsultaFechasInicio("ACTFECHA", fechaInicio, fechaTermino.AddDays(-1), codigoMoneda, reajuste);
                            consultaTasas.FechaTerminoStr = fechaTermino.AddDays(-1).ToString("yyyy-MM-dd");
                        }
                    }
                }
                
                res.Object = consultaTasas;
                res.IsOk = true;
                res.Message = "La Información se ha guardado Satisfactoriamente";

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
        /// Antonio Quezada
        /// 2018-08-27
        /// Elimina los Tramos del Periodo seleccionado
        /// </summary>
        /// <param name="tipoMoneda"> Tipo de la Moneda </param>
        /// <param name="periodo"> Periodo seleccionado </param>

        public Response EliminarTasaCalce(string tipoMoneda, string periodo)
        {
            try
            {
                Response res = new Response();

                string[] codigoVector = tipoMoneda.Split('#');
                string codigoMoneda = codigoVector[0];
                string reajuste = codigoVector[1];

                string[] periodoVector = periodo.Split('*');
                string fechaIni = Convert.ToDateTime(periodoVector[0]).ToString("yyyyMMdd");
                DateTime fechaInicio = Convert.ToDateTime(periodoVector[0]);

                TasaDescuentoAnual InformacionIncial = new TasaDescuentoAnual();
                List<TasaDescuentoAnual> InformacionBorrar = new List<TasaDescuentoAnual>();
                DateTime FechaFinBorrada = new DateTime();
                TasaDescuentoAnual InformacionIntermedia = new TasaDescuentoAnual();
                //buscamos la informacion que se va a borrar 
                InformacionBorrar = _tasaCalceRepository.ConsultaTasaDescAnual(fechaIni, codigoMoneda, reajuste);
                foreach (var item in InformacionBorrar)
                {//se recuperan las fechas que se van a borrar
                    FechaFinBorrada = Convert.ToDateTime(item.FechaTerminoStr);
                    break;
                }
                //ejecuta la eliminacion de la fecha
                _tasaCalceRepository.EliminarTasaCalce(fechaIni, codigoMoneda, reajuste);

                //'Devuelve EL DIA ANTES DE LA FECHA DE INICIO QUE ESTAMOS ELIMINANDO
                InformacionIncial = _tasaCalceRepository.ConsultaFechasInicio("CONSULTA", fechaInicio, fechaInicio.AddDays(-1), codigoMoneda, reajuste);
                if (InformacionIncial != null)
                {
                    DateTime vgTopeFecFin = Convert.ToDateTime("9999-12-31");
                    DateTime vgTopeFecFinal = FechaFinBorrada;//obtenermos la fecha fin de la fecha que se borro
                    DateTime fechaInicial = Convert.ToDateTime(InformacionIncial.FechaInicioStr);//obtenemos la fecha anterior a la que se borro
                    if (vgTopeFecFinal != vgTopeFecFin)
                    {
                        //fechas intermedias
                        InformacionIntermedia = _tasaCalceRepository.ConsultaFechasInicio("REGISTRADO", fechaInicial, fechaInicial, codigoMoneda, reajuste);
                        if (InformacionIntermedia != null)
                        {
                            //CAMBIA LA FECHA DEL TERMINO DE VIGENCIA DEL REGISTRO INGRESADDO
                            vgTopeFecFinal = Convert.ToDateTime(InformacionIntermedia.FechaInicioStr);
                            //recuperamos la fecha siguiente y restamos 1 dia a la fecha siguiente para acualizar la fecha fin del registro ingresado
                            _tasaCalceRepository.ConsultaFechasInicio("ACTFECHA", fechaInicial, vgTopeFecFinal.AddDays(-1), codigoMoneda, reajuste);
                        }

                    }
                    else {
                        _tasaCalceRepository.ConsultaFechasInicio("ACTFECHA", fechaInicial, vgTopeFecFin, codigoMoneda, reajuste);
                    }

                }

                res.IsOk = true;
                res.Message = "La Información se ha eliminado Satisfactoriamente";

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
        /// Antonio Quezada
        /// 2018-08-28
        /// Obtiene la consulta para mostrar en el reporte
        /// </summary>
        /// <param name="fechaInicio"> Fecha de Inicio del Periodo </param>
        /// <param name="codigoMoneda"> Código de la Moneda </param>
        /// <param name="reajuste"> Tipo de Reajuste de la Moneda </param>
        /// <returns> Regresa una lista con la información de cada Tramo por Periodo y Tipo de Moneda </returns>

        public List<TasaDescuentoAnual> ConsultaRpt(string fechaInicio, string codigoMoneda, string reajuste)
        {
            try
            {
                List<TasaDescuentoAnual> tasasDescuentoAnual = new List<TasaDescuentoAnual>();
                int valMax = 10;

                tasasDescuentoAnual = _tasaCalceRepository.ConsultaTasaDescAnual(fechaInicio, codigoMoneda, reajuste);

                if (tasasDescuentoAnual.Count == 0)
                {
                    for (int i = 1; i <= valMax; i++)
                    {
                        TasaDescuentoAnual tasa = new TasaDescuentoAnual() { Tramo = i, CPK = "0" };
                        tasasDescuentoAnual.Add(tasa);
                    }
                }

                return tasasDescuentoAnual;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
    }
}
