using Estudio.Repository.Core.Domain;
using Estudio.Repository.Core.Domain.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Estudio.Logic;
using System.Threading;

namespace Estudio.Controllers.Controllers.Oficiales
{
    public class GenArMelerController : Controller
    {
        GenArMelerLogic _GenArMeler = new GenArMelerLogic();
        static object _globalValue;
        public static object GlobalValue
        {
            get
            {
                return _globalValue;
            }
            set
            {
                _globalValue = value;
            }
        }
        // GET: ProCarArchivo
        public ActionResult Index()
        {
            DateTime thisDay = DateTime.Today;
            ViewBag.fecha = thisDay.ToString("yyyy-MM-dd");

            cargarTabla();
            ViewBag.pagina = "ExcepcionesEx";
            ViewBag.Informacion = GlobalVar.GlobalValue;
            return View();
        }
        public void cargarTabla()
        {
            try
            {
                var resultado = _GenArMeler.getNumArchivos();
                GlobalVar.GlobalValue = resultado.Object;
            }
            catch (Exception)
            {
            }
        }
        public ActionResult refrescarGrilla()
        {
            try
            {
                var resultado = _GenArMeler.actualizarGrilla();
                GlobalVar.GlobalValue = resultado.Object;
                return Json(resultado);
            }
            catch (Exception)
            {
                return null;
            }
        }
        public ActionResult BuscarNumerosArchivo(string fecha)
        {
            try
            {
                var resultado = _GenArMeler.BuscarNumerosArchivo(fecha);
                GlobalVar.GlobalValue = resultado.Object;
                return Json(resultado);
            }
            catch (Exception)
            {
                return null;
            }
        }
        public ActionResult BuscarArchivo(string fecha, string numArchivo, string caso)
        {
            try
            {

                //var resultado = _ProCarArchivoLogic.cargarXML(__doc, archivo, nombre, us, tipo, fecha, hora);
                var resultado = _GenArMeler.BuscarArchivo(fecha, numArchivo, caso);
                return Json(resultado);
            }
            catch (Exception)
            {
                return null;
            }
        }
        
        public ActionResult GenerarXML(List<GenArMeler> informacion, List<GenArMeler> NoCotiza)
        {
            try
            {
                var resultado = _GenArMeler.GenerarXML(informacion, NoCotiza,"");
                return Json(resultado);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public ActionResult ExportarGenArch(string FecEnvio, int numArchivo)
        {
            try
            {
                //Nombre de archivo y path
                string path = Server.MapPath("\\Files\\");

                string nombreArchivo = "SolicitudesGeneradas";
                List<string> nombresHojas = new List<string>();

                ExportarExcel expExcel = new ExportarExcel();

                Random r = new Random();
                int aleatorio3 = r.Next(100, 999);

                var date = DateTime.Today;

                nombreArchivo = string.Format("{0}_{1}-{2}.xls", nombreArchivo, date.ToString("yyyyMMdd"), aleatorio3.ToString());

                Descargas datosDescarga = new Descargas();
                datosDescarga.NombreArchivo = nombreArchivo;
                datosDescarga.Path = path;
                datosDescarga.Estatus = 1; //descargando


                //inserttamos la descarga para el historial
                // descargaMetodos.InsertDescarga(datosDescarga); *****

                //Obtenemos la lista de la exportacion a excel
                var resultExportacion = _GenArMeler.ExportarExcelGenArch(FecEnvio, numArchivo);
                //var resultExportacionN = _CalculoCotizacionLogic.ExportarExcelNoCalculadas(num_Archivo);

                List<List<List<Dictionary<string, object>>>> listaResultado = new List<List<List<Dictionary<string, object>>>>();
                listaResultado.Add(resultExportacion);
                //listaResultado.Add(resultExportacionN);

                nombresHojas.Add("Solicitudes Generadas");
                //nombresHojas.Add("Solicitudes No Calculadas");

                if (resultExportacion.Count > 0)
                {
                    //Se ejecuta el metodo del excel dentro de un hilo
                    Thread myThread = new Thread(delegate ()
                    {

                        ExportarExcel exportar = new ExportarExcel();
                        exportar.ExportExcel(path + nombreArchivo, listaResultado, nombresHojas);

                    });

                    myThread.Start();
                    myThread.Join();
                }
                else
                {


                }

                byte[] fileBytes = System.IO.File.ReadAllBytes(path + nombreArchivo);
                return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, nombreArchivo);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

    }
}