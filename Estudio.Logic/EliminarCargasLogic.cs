using Estudio.Repository.Helpers;
using Estudio.Repository.Persistence.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Estudio.Logic
{
    public class EliminarCargasLogic
    {
        EliminarCargasRepository _eliCargasRep = new EliminarCargasRepository();
        public Response busqueda(string caso, string numArch, string nomArch, string tipoArchivo)
        {
            Response res = new Response();
            try
            {
                return _eliCargasRep.busqueda(caso, numArch, nomArch, tipoArchivo); ;
            }
            catch (Exception ex)
            {
                res.Message = ex.Message;
                res.IsOk = false;
                return res;
            }
        }

        public Response eliminar(string caso, string numArch, string numArchS)
        {
            Response res = new Response();
            try
            {
                return _eliCargasRep.eliminar(caso, numArch, numArchS); ;
            }
            catch (Exception ex)
            {
                res.Message = ex.Message;
                res.IsOk = false;
                return res;
            }
        }
        public Response busquedaDeNumsArchs(string fecha, string caso)
        {
            try
            {
                Response res = new Response();
                //ManFecAceptacionCotizacion Datos = new ManFecAceptacionCotizacion();

                var Datos = _eliCargasRep.busquedaDeNumsArchs(fecha, caso);

                res.Object = Datos;
                res.Message = "";
                res.IsOk = true;
                return res;
            }
            catch (Exception ex)
            {
                Response res2 = new Response();
                res2.IsOk = false;
                res2.Message = ex.Message;
                return res2;
            }
        }

    }
}
