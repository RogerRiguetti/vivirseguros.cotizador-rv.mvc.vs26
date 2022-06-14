using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using Estudio.Repository.Core.Domain;
using Estudio.Repository.Helpers;
using Estudio.Repository.Persistence.Repositories;
using System;
using System.Collections.Generic;
using Estudio.Process.Muestra;

namespace Estudio.Logic
{
    public class ManFecAceptacionCotizacionLogic
    {
        public Response BuscarArchivo(string cuspp, string NumOperacion, string NumCotizacion, string NumCorrelativo)
        {
            ManFecAceptacionCotizacionRepository _ManFecAceptacionCotizacionRepository = new ManFecAceptacionCotizacionRepository();
            try
            {
                Response res = new Response();
                //ManFecAceptacionCotizacion Datos = new ManFecAceptacionCotizacion();

                var Datos = _ManFecAceptacionCotizacionRepository.Busca_Informacion(cuspp,NumOperacion, NumCotizacion, NumCorrelativo);
                if (Datos[0] == "Número de Solicitud de Oferta No Existe")
                {
                    Response res2 = new Response();
                    res2.IsOk = false;
                    res2.Message = "Número de Solicitud de Oferta No Existe";
                    return res2;
                }

                //Obtiene el Código de la Compañia guardado en la BD
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

        public Response ActualizarArchivo(string NumOperacion, string NumCotizacion, string NumCorrelativo, string FecCierre)
        {
            ManFecAceptacionCotizacionRepository _ManFecAceptacionCotizacionRepository = new ManFecAceptacionCotizacionRepository();
            try
            {
                Response res = new Response();
                //ManFecAceptacionCotizacion Datos = new ManFecAceptacionCotizacion();

                var Datos = _ManFecAceptacionCotizacionRepository.Actualizar_Informacion(NumOperacion, NumCotizacion, NumCorrelativo, FecCierre);
                if (Datos == "Operación Cancelada.")
                {
                    Response res2 = new Response();
                    res2.IsOk = false;
                    res2.Message = "Ha Fallado la Conexion. \n\nOperación Cancelada.";
                    return res2;
                }

                //Obtiene el Código de la Compañia guardado en la BD
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
