using CrystalDecisions.CrystalReports.Engine;
using Estudio.Logic;
using Estudio.Repository.Core.Domain;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Estudio.Controllers.Controllers.Oficiales
{
    public class MantenedorFiltrosController : Controller
    {
        MantenedorFiltrosLogic _mantenedor = new MantenedorFiltrosLogic();
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult filtros_Load()
        {
            try
            {
                var res = _mantenedor.getInfo();
                return Json(res);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public ActionResult buscar(string caso)
        {
            try
            {
                var res = _mantenedor.buscar(caso);
                return Json(res);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public ActionResult aceptar()
        {
            try
            {
                var res = _mantenedor.aceptar();
                return Json(res);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public ActionResult listasSeleccionadas(List<string[]> combos, string edadDesde, string edadHasta, string primaDesde, string primaHasta, string caso)
        {
            try
            {
                var usuario = Convert.ToString(this.Session["Account"]);
                var res = _mantenedor.listasSeleccionadas(combos, edadDesde, edadHasta, primaDesde, primaHasta, caso, usuario);
                return Json(res);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}