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
    public class MantenedorIPCLogic
    {
        MantenedorIPCRepository _MantenedorIPCRepository = new MantenedorIPCRepository();
        /// <summary>
        /// Osvaldo Valdez Carrillo
        /// 2018-08-23
        /// </summary>
        /// <returns>retorna la informacion para cargar la tabla inicial</returns>
        public Response CargarTabla()
        {
            try
            {
                Response res = new Response();
                res.IsOk = true;
                res.Object = _MantenedorIPCRepository.CargarTabla();
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
        /// 2018-08-24
        /// </summary>
        /// <param name="fecha">fecha de busqueda</param>
        /// <returns>retorna la informacion si existe</returns>
        public Response Consulta(DateTime fecha)
        {
            try
            {
                Response res = new Response();
                res.IsOk = true;
                res.Object = _MantenedorIPCRepository.Consulta(fecha);
                if (res.Object == null)
                {
                    MantenedorIPC info = new MantenedorIPC();
                    info.Codigo = "";
                    res.Object = info;
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
        ///  Osvaldo Valdez Carrillo
        /// 2018-08-24
        /// </summary>
        /// <param name="fecha">fecha IPC</param>
        /// <param name="valorIPC">valor ipc</param>
        /// <param name="variacionIPC"> variacion ipc</param>
        /// <param name="codigo">codigo N</param>
        /// <param name="usuario">Nombre del usuario</param>
        /// <param name="bandera">true o false</param>
        /// <returns>retorna si se pudo grabar o actualizar</returns>
        public Response GrabarMantenedorIPC(DateTime fecha, decimal valorIPC, decimal variacionIPC, string codigo, string usuario, bool bandera)
        {
              try
              {
                    Response res = new Response();
                    res.IsOk = true;
                    MantenedorIPC InformacionIncial = new MantenedorIPC();
                    if (bandera)
                    {//registra
                        InformacionIncial = _MantenedorIPCRepository.GrabarMantenedorIPC("GRABAR", fecha, valorIPC, variacionIPC, codigo, usuario);
                        if (InformacionIncial == null)
                        {
                            InformacionIncial = new MantenedorIPC();
                            InformacionIncial.Codigo = "N";
                            res.Object = InformacionIncial;
                            res.Message = "Error al guardar la información";
                        }
                        else
                        {
                            res.Message = "Información cargada con éxito";

                        }
                    }
                    else
                    {//actualiza
                        InformacionIncial = _MantenedorIPCRepository.GrabarMantenedorIPC("ACTUALIZAR", fecha, valorIPC, variacionIPC, codigo, usuario);
                        if (InformacionIncial == null)
                        {
                            InformacionIncial = new MantenedorIPC();
                            InformacionIncial.Codigo = "N";
                            res.Object = InformacionIncial;
                            res.Message = "Error al guardar la información";
                        }
                        else
                        {
                            res.Message = "Información cargada con éxito";

                        }
                    }
                    res.Object = InformacionIncial;
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
        /// 2018-08-24
        /// </summary>
        /// <returns>genera el reporte</returns>
        public List<MantenedorIPC> ConsultaRpt()
        {
            try
            {
                List<MantenedorIPC> registros = _MantenedorIPCRepository.ConsultaRpt();
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
        /// 2018-08-24
        /// </summary>
        /// <param name="fecha">fecha IPC</param>
        /// <returns>retorna si se elimino el registro</returns>
        public Response EliminarMantenedorIPC(DateTime fecha)
        {
            try
            {
                Response res = new Response();
                res.IsOk = true;
                res.Object = _MantenedorIPCRepository.EliminarMantenedorIPC(fecha);
                if (res.Object == null)
                {
                    MantenedorIPC info = new MantenedorIPC();
                    info.Codigo = "";
                    res.Object = info;
                    res.Message = "La Eliminación de Datos fue realizada Correctamente.";
                }
                else
                {
                    res.Message = "La Eliminación de Datos no fue realizada Correctamente.";
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
