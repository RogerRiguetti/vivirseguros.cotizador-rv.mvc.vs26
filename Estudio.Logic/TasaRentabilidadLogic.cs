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
   public class TasaRentabilidadLogic
    {
        CatalogosOficialesRepository _catalogosOficialesRepository = new CatalogosOficialesRepository();
        TasaRentabilidadRepository _TasaRentabilidadRepository = new TasaRentabilidadRepository();
        public List<Moneda> Moneda()
        {
            return _catalogosOficialesRepository.TiposMoneda();
        }

        public Response ListaPeriodos(string tipoMoneda)
        {
            try
            {
                Response res = new Response();
                string[] vectorMoneda = tipoMoneda.Split('#');
                string codigoMoneda = vectorMoneda[0];
                string reajuste = vectorMoneda[1];

                res.IsOk = true;
                res.Object = _TasaRentabilidadRepository.ListaPeriodos(codigoMoneda, reajuste);
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

        public Response ListaRentabilidad(string valoresMoneda, DateTime fechaIni)
        {
            try
            {
                Response res = new Response();
                string[] vectorMoneda = valoresMoneda.Split('#');
                string codigoMoneda = vectorMoneda[0];
                string reajuste = vectorMoneda[1];
                string fechaTermino = "";

                var datos = _TasaRentabilidadRepository.ListaRentabilidad(codigoMoneda, reajuste, fechaIni);
                if (datos.Count != 0)
                {
                    fechaTermino = datos[0].FEC_TERVIG;
                }
                res.IsOk = true;
                res.Object = new { fechaTermino = fechaTermino, tasasRent = datos };
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

        public Response ConsultaTasaRen(string valoresMoneda, DateTime fechaIni, int numAnno)
        {
            try
            {
                Response res = new Response();
                string[] vectorMoneda = valoresMoneda.Split('#');
                string codigoMoneda = vectorMoneda[0];
                string reajuste = vectorMoneda[1];

                res.IsOk = true;
                res.Object = _TasaRentabilidadRepository.ConsultaTasaRen(codigoMoneda, reajuste, fechaIni, numAnno);
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

        public Response Grabar(List<beRentabilidad> informacion, string tipoMoneda, DateTime fechaInicio, string clave, string usuario)
        {
            try
            {//INSERT  UPDATE
                Response res = new Response();
                beRentabilidad consultaTasas = new beRentabilidad();
                DateTime fechaActual = DateTime.Now;

                string[] codigoVector = tipoMoneda.Split('#');
                string codigoMoneda = codigoVector[0];
                string reajuste = codigoVector[1];
                DateTime fechaTermino = Convert.ToDateTime("31/12/9999");

                foreach (var item in informacion)
                {
                    item.COD_MONEDA = codigoMoneda;
                    item.COD_TIPREAJUSTE = Convert.ToInt32(reajuste);
                    item.FEC_INIVIG = fechaInicio.ToString("yyyyMMdd");
                    item.FEC_TERVIG = fechaTermino.ToString("yyyyMMdd");
                    item.Usuario = usuario;

                    _TasaRentabilidadRepository.RegistrarModificar(clave, item);
                }

               if (clave == "INSERT")
                {//verificamos si existen fechas menores
                    //recuperamos la fecha
                    consultaTasas = _TasaRentabilidadRepository.ConsultaFechasInicio("FECHAANT", fechaInicio, fechaTermino, codigoMoneda, reajuste);
                    if (consultaTasas == null)
                    {//sino existen fechas menores buscamos fechas mayores
                        consultaTasas = new beRentabilidad();
                        consultaTasas = _TasaRentabilidadRepository.ConsultaFechasInicio("FECHASIG", fechaInicio, fechaTermino, codigoMoneda, reajuste);
                        if (consultaTasas == null)
                        {//sino existen fechas mayores es el tope de las fechas
                            consultaTasas = new beRentabilidad();
                            consultaTasas.FEC_TERVIG = fechaTermino.ToString("yyyy-MM-dd");
                        }
                        else
                        {   //CAMBIA LA FECHA DEL TERMINO DE VIGENCIA DEL REGISTRO INGRESADDO
                            fechaTermino = Convert.ToDateTime(consultaTasas.FEC_INIVIG);
                            //recuperamos la fecha siguiente y restamos 1 dia a la fecha siguiente para acualizar la fecha fin del registro ingresado
                            _TasaRentabilidadRepository.ConsultaFechasInicio("ACTFECHA", fechaInicio, fechaTermino.AddDays(-1), codigoMoneda, reajuste);
                            consultaTasas.FEC_TERVIG = fechaTermino.AddDays(-1).ToString("yyyy-MM-dd");
                        }
                    }
                    else
                    {
                        //Actualiza la fecha de termino del periodo anterior 
                        DateTime fechaInicial = Convert.ToDateTime(consultaTasas.FEC_INIVIG);
                        //recuperamos la fecha anterior y restamos 1 dia a la fecha ingresada para generar el periodo
                        _TasaRentabilidadRepository.ConsultaFechasInicio("ACTFECHA", fechaInicial, fechaInicio.AddDays(-1), codigoMoneda, reajuste);
                        consultaTasas.FEC_TERVIG = fechaTermino.ToString("yyyy-MM-dd");
                        //actualiza la fecha de termino del periodo siguiente si existe
                        consultaTasas = _TasaRentabilidadRepository.ConsultaFechasInicio("FECHASIG", fechaInicio, fechaTermino, codigoMoneda, reajuste);
                        if (consultaTasas == null)
                        {//sino existen fechas mayores es el tope de las fechas
                            consultaTasas = new beRentabilidad();
                            consultaTasas.FEC_TERVIG = fechaTermino.ToString("yyyy-MM-dd");
                        }
                        else
                        {   //CAMBIA LA FECHA DEL TERMINO DE VIGENCIA DEL REGISTRO INGRESADDO
                            fechaTermino = Convert.ToDateTime(consultaTasas.FEC_INIVIG);
                            //recuperamos la fecha siguiente y restamos 1 dia a la fecha siguiente para acualizar la fecha fin del registro ingresado
                            _TasaRentabilidadRepository.ConsultaFechasInicio("ACTFECHA", fechaInicio, fechaTermino.AddDays(-1), codigoMoneda, reajuste);
                            consultaTasas.FEC_TERVIG = fechaTermino.AddDays(-1).ToString("yyyy-MM-dd");
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

        public List<beRentabilidad> ConsultaRpt(string codMoneda, int reajuste, string moneda, DateTime fechaInicial)
        {
            try
            {
                List<beRentabilidad> registros = _TasaRentabilidadRepository.ConsultaRpt(codMoneda, reajuste, moneda,fechaInicial);
                return registros;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public Response EliminarTasaRentabilidad(string tipoMoneda, DateTime fechaInicio)
        {
            try
            {
                Response res = new Response();

                string[] codigoVector = tipoMoneda.Split('#');
                string codigoMoneda = codigoVector[0];
                string reajuste = codigoVector[1];

                beRentabilidad InformacionIncial = new beRentabilidad();
                beRentabilidad InformacionBorrar = new beRentabilidad();
                beRentabilidad InformacionIntermedia = new beRentabilidad();
                //buscamos la informacion que se va a borrar 
                //////////////////////////////////////////////////////////////////consulta para traer la info a borrar
                InformacionBorrar = _TasaRentabilidadRepository.ConsultaFechasInicio("CONSULTADEL", fechaInicio, fechaInicio.AddDays(-1), codigoMoneda, reajuste);

                //ejecuta la eliminacion de la fecha
                _TasaRentabilidadRepository.EliminarTasaRentabilidad(fechaInicio, codigoMoneda, reajuste);

                //'Devuelve EL DIA ANTES DE LA FECHA DE INICIO QUE ESTAMOS ELIMINANDO
                InformacionIncial = _TasaRentabilidadRepository.ConsultaFechasInicio("CONSULTA", fechaInicio, fechaInicio.AddDays(-1), codigoMoneda, reajuste);
                if (InformacionIncial != null)
                {
                    DateTime vgTopeFecFin = Convert.ToDateTime("9999-12-31");
                    DateTime vgTopeFecFinal = Convert.ToDateTime(InformacionBorrar.FEC_INIVIG);//obtenermos la fecha fin de la fecha que se borro
                    DateTime fechaInicial = Convert.ToDateTime(InformacionIncial.FEC_INIVIG);//obtenemos la fecha anterior a la que se borro
                    if (vgTopeFecFinal != vgTopeFecFin)
                    {
                        //fechas intermedias
                        InformacionIntermedia = _TasaRentabilidadRepository.ConsultaFechasInicio("FECHASIG", fechaInicial, fechaInicial, codigoMoneda, reajuste);
                        if (InformacionIntermedia != null)
                        {
                            //CAMBIA LA FECHA DEL TERMINO DE VIGENCIA DEL REGISTRO INGRESADDO
                            vgTopeFecFinal = Convert.ToDateTime(InformacionIntermedia.FEC_INIVIG);
                            //recuperamos la fecha siguiente y restamos 1 dia a la fecha siguiente para acualizar la fecha fin del registro ingresado
                            _TasaRentabilidadRepository.ConsultaFechasInicio("ACTFECHA", fechaInicial, vgTopeFecFinal.AddDays(-1), codigoMoneda, reajuste);
                        }

                    }
                    else
                    {
                        _TasaRentabilidadRepository.ConsultaFechasInicio("ACTFECHA", fechaInicial, vgTopeFecFin, codigoMoneda, reajuste);
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
    }
}
