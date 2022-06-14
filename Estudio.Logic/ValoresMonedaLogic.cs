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
    public class ValoresMonedaLogic
    {
        ValoresMonedaRepository _ValoresMonedaRepository = new ValoresMonedaRepository();
        CatalogosOficialesRepository _CatalogosOficiales = new CatalogosOficialesRepository();

        public List<Moneda> TiposMoneda()
        {
            return _CatalogosOficiales.cmbMoneda();
        }

        public Response ListaValoresVM(string vlMoneda)
        {
            try
            {
                Response res = new Response();
                res.Object = _ValoresMonedaRepository.ListaValoresVM(vlMoneda);
                if (res.Object == null)
                {
                    List<ValoresMoneda> lista = new List<ValoresMoneda>();
                    ValoresMoneda InformacionIncial = new ValoresMoneda();
                    InformacionIncial.FechaVM = "";
                    lista.Add(InformacionIncial);
                    res.Object = lista;
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


        public Response BuscarValorVM(string vlMoneda, string fecVM)
        {
            try
            {
                Response res = new Response();
                res.Object = _ValoresMonedaRepository.BuscarValorVM(vlMoneda, fecVM);
                if (res.Object == null)
                {
                    List<ValoresMoneda> lista = new List<ValoresMoneda>();
                    ValoresMoneda InformacionIncial = new ValoresMoneda();
                    InformacionIncial.FechaVM = "";
                    lista.Add(InformacionIncial);
                    res.Object = lista;
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

        
        public Response GrabarValorVM(string vlMoneda, string fecVM, decimal valorM, bool bandera)
        {
            try
            {
                Response res = new Response();
                if (bandera)
                {
                    res.Object = _ValoresMonedaRepository.InsertValorVM(vlMoneda, fecVM, valorM);
                    if (res.Object == null)
                    {
                        List<ValoresMoneda> lista = new List<ValoresMoneda>();
                        ValoresMoneda InformacionIncial = new ValoresMoneda();
                        InformacionIncial.FechaVM = "";
                        lista.Add(InformacionIncial);
                        res.Object = lista;
                        res.Message = "No se encontro información";
                    }
                    else
                    {
                        res.Message = "Información cargada con éxito";
                    }
                    res.IsOk = true;
                    return res;
                }
                else
                {
                    res.Object = _ValoresMonedaRepository.SaveValorVM(vlMoneda, fecVM, valorM);
                    if (res.Object == null)
                    {
                        List<ValoresMoneda> lista = new List<ValoresMoneda>();
                        ValoresMoneda InformacionIncial = new ValoresMoneda();
                        InformacionIncial.FechaVM = "";
                        lista.Add(InformacionIncial);
                        res.Object = lista;
                        res.Message = "No se encontro información";
                    }
                    else
                    {
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

        public Response EliminaValorVM(string vlMoneda, string fecVM)
        {
            try
            {
                Response res = new Response();
                res.Object = _ValoresMonedaRepository.DeleteValorVM(vlMoneda, fecVM);
                if (res.Object == null)
                {
                    List<ValoresMoneda> lista = new List<ValoresMoneda>();
                    ValoresMoneda InformacionIncial = new ValoresMoneda();
                    InformacionIncial.FechaVM = "";
                    lista.Add(InformacionIncial);
                    res.Object = lista;
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

        public List<ValoresMoneda> ReporteVM(string vlMoneda, string strMoneda)
        {
            try
            {

                return _ValoresMonedaRepository.ConsultaRptVM(vlMoneda, strMoneda);
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
