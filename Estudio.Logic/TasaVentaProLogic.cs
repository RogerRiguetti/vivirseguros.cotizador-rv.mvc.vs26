using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Estudio.Repository.Persistence.Repositories;
using Estudio.Repository.Helpers;
using Estudio.Repository.Core.Domain;

namespace Estudio.Logic
{
    public class TasaVentaProLogic
    {
        TasaVentaProRepository _ValoresM = new TasaVentaProRepository();
        TasaVentaProRepository _TasaVentaProRepository = new TasaVentaProRepository();
        public List<TasaVentaPro> TiposMoneda()
        {
            return _ValoresM.TiposMoneda();
        }
        public List<TasaVentaPro> RangosTasa()
        {
            return _ValoresM.RangosTasa();
        }
        public Response CargarTabla(string vlMoneda, string tipReajuste, string cod_Pres)
        {
            try
            {
                Response res = new Response();
                res.IsOk = true;
                res.Object = _TasaVentaProRepository.CargarTabla(vlMoneda, tipReajuste, cod_Pres);
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
        public Response Guardar(string vlMoneda, string tipReajuste, string cod_Pres, decimal prom, string fecha)
        {
            try
            {
                Response res = new Response();
                res.IsOk = true;
                res.Object = _TasaVentaProRepository.Guardar(vlMoneda, tipReajuste, cod_Pres, prom, fecha);
                res.Message = "Información actualizada con éxito";
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
        /// José Hernández Alvarado.
        /// 02-01-2018
        /// </summary>
        /// <param name="vlMoneda"></param>
        /// <param name="tipReajuste"></param>
        /// <param name="cod_Pres"></param>
        /// <param name="fecha"></param>
        /// <returns></returns>
        public Response BuscarTasa(string vlMoneda, string tipReajuste, string cod_Pres, string fecha)
        {
            try
            {
                Response res = new Response();
                List<TasaVentaPro> numReg = new List<TasaVentaPro>();
                res.IsOk = true;
                numReg = _TasaVentaProRepository.BuscarTasa(vlMoneda, tipReajuste, cod_Pres, fecha);
                res.Object = numReg;

                if (numReg.Count == 0)
                {
                    res.Message = "No se encontro información";
                }
                else
                {
                    res.Message = "Información encontrada con éxito";
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


        public Response NuevaTasaVta(string vlMoneda, string tipReajuste, string cod_Pres, string fecha, decimal prom, string usuario)
        {
            try
            {
                Response res = new Response();
                TasaVentaPro numReg = new TasaVentaPro();
                res.IsOk = true;
                numReg = _TasaVentaProRepository.NuevaTasaVta(vlMoneda, tipReajuste, cod_Pres, fecha, prom, usuario);
                res.Object = numReg;
                if (numReg == null)
                {
                    res.Message = "No se insertó la información correctamente.";
                }
                else
                {
                    res.Message = "Información insertada con éxito.";
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
    }
}
