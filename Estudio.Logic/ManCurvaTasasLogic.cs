using Estudio.Repository.Core.Domain;
using Estudio.Repository.Core.Domain.Views;
using Estudio.Repository.Helpers;
using Estudio.Repository.Persistence.Repositories;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;
using log4net;
using log4net.Config;
using System.Reflection;

namespace Estudio.Logic
{
    public class ManCurvaTasasLogic
    {
        private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public Response DatosQ(string query)
        {
            ManCurvaTasasRepository _DatosExcel = new ManCurvaTasasRepository();
            try
            {
                XmlConfigurator.Configure();
                _log.Info("COMENZARA PROCESO DE CURVA DE TASAS (Excel)");
                var respusta = _DatosExcel.DatosQ(query);
                _log.Info("Termino de realizar Query");
                return respusta;
            }
            catch (Exception ex)
            {
                Response res2 = new Response();
                res2.IsOk = false;
                res2.Message = ex.Message;
                return res2;
            }
        }

        public Response CargaTablaLogic(string fecha)
        {
            ManCurvaTasasRepository _CargarTabla = new ManCurvaTasasRepository();
            List<ManCurvaTasas> info = new List<ManCurvaTasas>();
            Response respuesta = new Response();
            try
            {
                XmlConfigurator.Configure();
                _log.Info("COMENZARA PROCESO DE CURVA DE TASAS (Busqueda)");
                info = _CargarTabla.CargaTableRepository(fecha);

                if (info.Count != 0)
                {
                    _log.Info("Se obtuvo la información" + info.Count);
                    respuesta.Object = info;
                    respuesta.IsOk = true;
                    return respuesta;
                }

                respuesta.Object = info;
                respuesta.IsOk = false;
                respuesta.Message = "No Existen Datos con esa Fecha.";
                return respuesta;
            }
            catch (Exception ex)
            {
                respuesta.IsOk = false;
                respuesta.Message = ex.Message;
                return respuesta;
            }
        }
    }
}
