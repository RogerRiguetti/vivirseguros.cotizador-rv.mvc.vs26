using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Estudio.Logic;
using Estudio.Repository.Core.Domain;
using CrystalDecisions.CrystalReports.Engine;
using System.IO;

namespace Estudio.Controllers.Controllers.Oficiales
{
    public class TasaCalceController : Controller
    {
        TasaCalceLogic _tasaCalceLogic = new TasaCalceLogic();

        #region No Transaccionales

        /// <summary>
        /// Antonio Quezada
        /// 2018-08-14
        /// Carga la pantalla de Tasa de Calce
        /// </summary>
        /// <returns> Regresa la vista que se mostrará al usuario </returns>

        public ActionResult Index()
        {
            string res = Convert.ToString(this.Session["encryptedTicket"]);
            if (String.IsNullOrEmpty(res))
                return RedirectToAction("Login", "Estudio");

            List<Moneda> tiposMoneda = _tasaCalceLogic.TiposMoneda();
           // string codigoMoneda = (from tp in tiposMoneda select tp.CodigoMoneda).First();
            string codigoMoneda = (from tp in tiposMoneda select tp.ClaveMoneda).First();
            
            List<Periodo> periodos = _tasaCalceLogic.ConsultaPeriodos(codigoMoneda);
            //ViewBag.TipoMoneda = new SelectList(tiposMoneda, "ClaveMoneda", "Elemento", codigoMoneda);
            ViewBag.TipoMoneda = new SelectList(tiposMoneda, "ClaveMoneda", "Elemento");
            ViewBag.Periodos = new SelectList("");
            DateTime thisDay = DateTime.Today;
            ViewBag.fecha = thisDay.ToString("yyyy-MM-dd");
            return View();
        }

        #endregion

        #region Transaccionales

        /// <summary>
        /// Antonio Quezada
        /// 2018-08-15
        /// Consuolta los periodos por Código de Moneda y Tipo de Reajuste
        /// </summary>
        /// <param name="tipoMoneda"> Contiene el Tipo de la Moneda y el Tipo de Reajuste </param>
        /// <returns> Regresa una lista de Periodos </returns>

        public ActionResult ConsultaPeriodos(string tipoMoneda)
        {
            try
            {
                return Json(_tasaCalceLogic.ConsultaPeriodosRes(tipoMoneda), JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// Antonio Quezada
        /// 2018-08-16
        /// Obtiene los registros de la Tasa de Descuento Anual
        /// </summary>
        /// <param name="periodo"> Periodo seleccionado </param>
        /// <param name="tipoMoneda"> Contiene el Tipo de la Moneda y el Tipo de Reajuste </param>
        /// <returns> Regresa las Tasas de Descuento Anual </returns>

        public ActionResult ConsultaTasaDescAnual(string periodo, string tipoMoneda)
        {
            try
            {
                return Json(_tasaCalceLogic.ConsultaTasaDescAnual(periodo, tipoMoneda), JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// Antonio Quezada
        /// 2018-08-20
        /// Consulta de la información por Tramo
        /// </summary>
        /// <param name="tramo"> Número de Año (Tramo) </param>
        /// <param name="periodo"> Periodo seleccionado </param>
        /// <param name="tipoMoneda"> Contiene el Tipo de la Moneda y el Tipo de Reajuste </param>
        /// <returns> Regresa un objeto que contiene la información del tramo en un objeto </returns>

        public ActionResult ConsultaTramo(int tramo, string periodo, string tipoMoneda)
        {
            try
            {
                return Json(_tasaCalceLogic.ConsultaTramo(tramo, periodo, tipoMoneda), JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return null;
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

        public ActionResult ConsultaPeriodoVigencia(DateTime fechaInicio, string tipoMoneda)
        {
            try
            {
                return Json(_tasaCalceLogic.ConsultaPeriodoVigencia(fechaInicio, tipoMoneda), JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return null;
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

        public ActionResult VerificacionTasaCalce(string tipoMoneda, string periodo, int eliminar)
        {
            try
            {
                return Json(_tasaCalceLogic.VerificacionTasaCalce(tipoMoneda, periodo, eliminar), JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return null;
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
        /// <param name="tasas"> Lista de información que será registrada o modificada </param>
        /// <returns> Regresa un objeto que indica si se realizó la transacción </returns>

        public ActionResult RegistrarModificarTasaCalce(string clave, string tipoMoneda, string periodo, List<TasaDescuentoAnual> tasas)
        {
            try
            {
                string usuario = this.Session["Account"].ToString();
                return Json(_tasaCalceLogic.RegistrarModificarTasaCalce(clave,tipoMoneda, periodo, usuario, tasas), JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// Antonio Quezada
        /// 2018-08-27
        /// Elimina los Tramos del Periodo seleccionado
        /// </summary>
        /// <param name="tipoMoneda"> Tipo de la Moneda </param>
        /// <param name="periodo"> Periodo seleccionado </param>
        /// <returns> Regresa un objeto que indica si se realizó la transacción </returns>

        public ActionResult EliminarTasaCalce(string tipoMoneda, string periodo)
        {
            try
            {
                return Json(_tasaCalceLogic.EliminarTasaCalce(tipoMoneda, periodo), JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// Antonio Quezada
        /// 2018-08-28
        /// Genera el reporte con Crystal Reports
        /// </summary>
        /// <param name="fechaInicio"> Fecha de Inicio del Periodo </param>
        /// <param name="codigoMoneda"> Código de la Moneda </param>
        /// <param name="reajuste"> Tipo de Reajuste de la Moneda </param>
        /// <returns> Muestra el reporte con formato PDF en una pestaña nueva </returns>

        public ActionResult Reporte(string fechaInicio, string codigoMoneda, string reajuste)
        {
            var resultExportacion = _tasaCalceLogic.ConsultaRpt(fechaInicio, codigoMoneda, reajuste);

            ReportDocument rpt = new ReportDocument();
            //rpt.FileName = Server.MapPath("~/Resources/reports/CotizacionReporte.rpt");
            rpt.Load(Server.MapPath("~/Resources/reports/TasaCalceReporte.rpt"));
            rpt.SetDataSource(resultExportacion);
            try
            {
                Stream stream = rpt.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                //stream.Seek(0, SeekOrigin.Begin);
                return File(stream, "application/pdf");
            }
            catch (Exception)
            {

                throw;
            }
        }

        #endregion
    }
}