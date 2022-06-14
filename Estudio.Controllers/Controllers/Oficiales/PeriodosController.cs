using System;
using System.Web.Mvc;
using Estudio.Logic;

namespace Estudio.Controllers.Controllers.Oficiales
{
    public class PeriodosController : Controller
    {
        PeriodosLogic _PeriodosLogic = new PeriodosLogic();
        public ActionResult Index()
        {
            string res = Convert.ToString(this.Session["encryptedTicket"]);
            if (String.IsNullOrEmpty(res))
                return RedirectToAction("Login", "Estudio");

            return View();
        }

        public ActionResult ConsultarPeriodos()
        {
            var resultado = _PeriodosLogic.ConsultarPeriodos();
            return Json(resultado);
        }

        public ActionResult GuardarNuevoPeriodoRepository(string fecha)
        {
            DateTime Fec = new DateTime();
            Fec = DateTime.Now;
            string strUsuario = getUsuario();
            var resultado = _PeriodosLogic.AbrirNuevoPeriodo(fecha, Fec.ToString("yyyyMMdd"), strUsuario);
            return Json(resultado);
        }

        public ActionResult ReabrirPeriodo (string fecha)
        {
            var resultado = _PeriodosLogic.ReabrirPeriodo(fecha);
            return Json(resultado);
        }

        public string getUsuario()
        {
            try
            {
                string usuario = "";
                if (Convert.ToString(this.Session["Account"]).Length < 10)
                {
                    usuario = Convert.ToString(this.Session["Account"]);
                }
                else
                {
                    usuario = Convert.ToString(this.Session["Account"]).Substring(0, 10);
                }
                return usuario;
            }
            catch (Exception) { return null; }
        }
    }
}