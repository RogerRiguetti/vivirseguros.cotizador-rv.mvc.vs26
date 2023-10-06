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
    public class ValoresMonedaMensualLogic
    {
        ValoresMonedaMensualRepository _ValoresMonedaMensualRepository = new ValoresMonedaMensualRepository();
        CatalogosOficialesRepository _CatalogosOficiales = new CatalogosOficialesRepository();
        /// <summary>
        ///  Osvaldo Valdez Carrillo
        /// 22/08/2018
        /// </summary>
        /// <returns>la informacion de los combos</returns>
        public List<Moneda> TiposValor()
        {
            return _CatalogosOficiales.cmbValor();
        }
        /// <summary>
        ///  Osvaldo Valdez Carrillo
        /// 22/08/2018
        /// </summary>
        /// <returns>la informacion de los combos</returns>
        public List<Moneda> TiposMoneda()
        {
            return _CatalogosOficiales.cmbMoneda();
        }
        /// <summary>
        /// Osvaldo Valdez Carrillo
        /// 22/08/2018
        /// </summary>
        /// <param name="vlMoneda">valor de la moneda</param>
        /// <param name="cod_tipmon">valor del tipo valor</param>
        /// <returns>retorna la info para la tabla</returns>
        public Response CargarTabla(string vlMoneda, string cod_tipmon)
        {
            try
            {
                Response res = new Response();
                res.IsOk = true;
                res.Object = _ValoresMonedaMensualRepository.CargarTabla(vlMoneda,cod_tipmon);
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
        ///  Osvaldo Valdez Carrillo
        /// 23/08/2018
        /// </summary>
        /// <param name="vlMoneda">valor de la moneda</param>
        /// <param name="cod_tipmon">valor del tipo valor</param>
        /// <param name="fec_moneda">valor de la fecha</param>
        /// <returns>retorna el resultado de la consulta</returns>
        public Response Consulta(string vlMoneda, string cod_tipmon, DateTime fec_moneda)
        {
            try
            {
                Response res = new Response();
                res.IsOk = true;
                res.Object = _ValoresMonedaMensualRepository.Consulta(vlMoneda, cod_tipmon, fec_moneda.ToString("yyyyMM"));
                if (res.Object == null)
                {
                    MonedaMensual info = new MonedaMensual();
                    info.Valor = 0;
                    info.FechaInicial = "";
                    res.Object = info;
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
        /// 23/08/2018
        /// </summary>
        /// <param name="vlMoneda">valor de la moneda</param>
        /// <param name="cod_tipmon">valor del tipo valor</param>
        /// <param name="fec_moneda">valor de la fecha</param>
        /// <returns>retorna el resultado de la eliminacion</returns>
        public Response EliminarValoresMoneda(string vlMoneda, string cod_tipmon, DateTime fec_moneda)
        {
            try
            {
                Response res = new Response();
                res.IsOk = true;
                res.Object = _ValoresMonedaMensualRepository.EliminarValoresMoneda(vlMoneda, cod_tipmon, fec_moneda.ToString("yyyyMM"));
                if (res.Object == null)
                {//si se borran todos los registros
                    MonedaMensual info = new MonedaMensual();
                    info.Valor = 0;
                    info.FechaInicial = "";
                    res.Object = info;
                    res.Message = "La Información se ha Eliminado Satisfactoriamente";
                }
                else
                {

                    res.Message = "La Información se ha Eliminado Satisfactoriamente";
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
        /// 23/08/2018
        /// </summary>
        /// <param name="vlMoneda">valor de la moneda</param>
        /// <param name="cod_tipmon">valor del tipo valor</param>
        /// <param name="moneda">valor del texto de la moneda</param>
        /// <returns>retorna la consulta del reporte</returns>
        public List<MonedaMensual> ConsultaRpt(string vlMoneda, string cod_tipmon, string moneda)
        {
            try
            {
                List<MonedaMensual> registros = _ValoresMonedaMensualRepository.ConsultaRpt(vlMoneda, cod_tipmon, moneda);
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
        /// 23/08/2018
        /// </summary>
        /// <param name="vlMoneda">valor de la moneda</param>
        /// <param name="cod_tipmon">valor del tipo valor</param>
        /// <param name="fec_moneda">valor de la fecha</param>
        /// <param name="valor">valore del periodo</param>
        /// <param name="bandera">bandera para actualizar o guardar</param>
        /// <returns>retorna el resultado de guardar o actualizar</returns>
        public Response GrabarValoresMoneda(string vlMoneda, string cod_tipmon, DateTime fec_moneda, decimal valor, bool bandera, string usuario)
        {
            try
            {
                Response res = new Response();
                MonedaMensual info = new MonedaMensual();
                if (bandera)
                {

                    res.Object = _ValoresMonedaMensualRepository.GrabarValoresMoneda(vlMoneda, cod_tipmon, fec_moneda.ToString("yyyyMM"), valor, "GRABAR", usuario);
                    if (res.Object == null)
                    {
                        info = new MonedaMensual();
                        info.Valor = 0;
                        info.FechaInicial = "";
                        res.Object = info;
                        res.Message = "Error al guardar la información";
                    }
                    else
                    {

                        res.Message = "Información cargada con éxito";
                    }
                }
                else {
                    res.Object = _ValoresMonedaMensualRepository.GrabarValoresMoneda(vlMoneda, cod_tipmon, fec_moneda.ToString("yyyyMM"), valor, "MODIFICAR", usuario);
                    if (res.Object == null)
                    {
                        info = new MonedaMensual();
                        info.Valor = 0;
                        info.FechaInicial = "";
                        res.Object = info;
                        res.Message = "Error al guardar la información";
                    }
                    else
                    {
                        res.Message = "Información cargada con éxito";
                    }

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
