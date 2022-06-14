using System;
using System.Collections.Generic;
using System.IO;
using System.Web.Mvc;
using System.Linq;
using Estudio.Logic;
using Estudio.Repository.Core.Domain;
using Estudio.Repository;
using Estudio.Repository.Helpers;
using log4net;
using System.Reflection;
using log4net.Config;
namespace Estudio.Controllers.Controllers
{
    public class MantenedorPerfilesController : Controller
    {
        MantenedorPerfilesLogic _MantenedorPerfilesLogic = new MantenedorPerfilesLogic();
        private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);


        #region Transaccionales

        #region Pantalla Index

        /// <summary>
        /// Daniel Mercado Alvarado 
        /// 07/06/2021 
        /// En este metodo hace update si Existe el permiso o inserta los nuevos permismos
        /// </summary>
        /// <param name="strgInsertUpdate">Parametro que contiene una lista con el id y clave del permiso selecionado </param>
        /// <returns>Regresa un Json con la información que se guardaron cambios del Permiso</returns>
        [HttpPost]
        public ActionResult UpdatePermiso(string [] ListaPermisos )
        {
            Response _res = new Response();

            int idUsuario = Convert.ToInt32(this.Session["UserId"]);

            if (ModelState.IsValid)
            {
                _res = _MantenedorPerfilesLogic.UpdatePermiso(ListaPermisos, idUsuario);
                ListaPermisos = null;
            }
            return Json(_res, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Daniel Mercado Alvarado 
        /// 02/06/2021 
        /// En este metodo se obtiene la informacion de la parte logica para activar o desactivar un permiso
        /// </summary>
        /// <param name="id">Parametro que contiene el id del permiso a desactivar o activar </param>
        /// <param name="active">Parametro que contiene la informacion del status del permiso (Activado o Desactivado)</param>
        /// <returns>Regresa un Json con la información que se cambio del Permiso</returns>
        [HttpPost]
        public ActionResult Delete(int id, int active)
        {
            Response _res = new Response();

            int idUsuario = Convert.ToInt32(this.Session["UserId"]);

            if (ModelState.IsValid)
            {
                _res = _MantenedorPerfilesLogic.Delete(id, active, idUsuario);
            }
            return Json(_res, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// Daniel Mercado Alvarado 
        /// 02/06/2021 
        /// En este Metodo nos obtiene la informacion de todos los permisos para posteriormente mandarlos a la vista de Index.
        /// </summary>
        /// <returns>Regresa la vista con la informacion de los permisos </returns>
        public ActionResult Search(int idSistema, string clavePermiso)
        {
            try
            {
                return Json(_MantenedorPerfilesLogic.Index(idSistema, clavePermiso), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return null;
            }
        }


        #endregion
        #region Pantalla Create

        /// <summary>
        /// Daniel Mercado
        /// 14/02/2018 En este metodo se realiza la creación de nuevos permisos 
        /// </summary>
        /// <param name="NamePantalla">Parametro que guardara el nombre de la pantalla</param>
        /// <param name="Clave">Parametro que guardara el nombre del nuevo permiso</param>
        /// <param name="DescripcionLarga">Parametro que guardara la descripcion larga del permiso</param>
        /// <param name="Sistemas_Idsistema">Parametro que guardara el id del sistema</param>
        ///  <param name="nodoPadre">Parametro que guardara el nodoPadre</param>
        /// <returns> Regresa la informacion agregada en la vista para madarla a la base de datos y guardarla </returns>

        [HttpPost]
        public JsonResult Create(string NamePantalla, string Clave, string DescripcionLarga, int Sistemas_Idsistema, string nodoPadre)
        {
            Response _res = null;

            int idUsuario = Convert.ToInt32(this.Session["UserId"]);

            if (ModelState.IsValid)
            {
                _res = new MantenedorPerfilesLogic().Create(NamePantalla,Clave, DescripcionLarga, Sistemas_Idsistema, idUsuario,nodoPadre);
            }
            return Json(_res, JsonRequestBehavior.AllowGet);
        }

        #endregion
        #region Pantalla Edit
        /// <summary>
        /// Daniel Mercado 03/06/2021
        /// Metodo en el cual se editará la informacíon del permiso seleccionado 
        /// </summary>
        /// <param name="Id">Parametro el cual obtiene el id del Permiso a modificar</param>
        /// <param name="namePantalla">Parametro que guardara el nombre del la pantalla</param>
        /// <param name="clave">Parametro que guardara el nombre del nuevo permiso</param>
        /// <param name="descripcionLarga">Parametro que guardara la descripcion larga del permiso</param>
        /// <param name="sistemas_Idsistema">Parametro que guardara el id del sistema</param>
        ///  <param name="nodoPadre">Parametro que guardara el nodoPadre</param>
        /// <returns>Regresa un Json con la informacion a guardar del permiso</returns>

        [HttpPost]
        public JsonResult Edit(int Id,string namePantalla,string clave, string descripcionLarga, int sistemas_Idsistema, string nodoPadre)
        {
            Response _res = null;

            int idUsuario = Convert.ToInt32(this.Session["UserId"]);

            if (ModelState.IsValid)
            {
                _res = new MantenedorPerfilesLogic().Edit(Id, namePantalla, descripcionLarga, sistemas_Idsistema, idUsuario, nodoPadre);
            }
            return Json(_res, JsonRequestBehavior.AllowGet);
        }



        #endregion

        #endregion

        #region No Transaccionales
        #region Pantalla Details
        /// <summary>
        /// Daniel Mercado
        /// 2021-09-01
        /// se asegura que haya una sesion iniciada, en caso contrario regresa a la pantalla de login en caso de que si este logueado el usuario, muestra los detalles del rol
        /// </summary>
        /// <returns>regresa una vista</returns>
        [HttpGet]
        public ActionResult Details(int id)
        {
            string res = Convert.ToString(this.Session["encryptedTicket"]);
            if (String.IsNullOrEmpty(res))
                return RedirectToAction("Login", "Estudio");

            MantenedorPerfiles permiso = _MantenedorPerfilesLogic.DetailsMantenedorPerfiles(id);
            ViewBag.cmbxSistemas = new SelectList(_MantenedorPerfilesLogic.ObtenerSistemas(), "IdSistema", "NombreSistema", permiso.IdSistema);
           
            return View(permiso);
        }
        #endregion
        #region Create
        /// <summary>
        /// Daniel Mercado
        /// 2021-06-01
        /// </summary>
        /// <returns> Regresa la pantalla de Registrar Permisos </returns>

        [HttpGet]
        public ActionResult Create()
        {
            string res = Convert.ToString(this.Session["encryptedTicket"]);
            if (String.IsNullOrEmpty(res))
                return RedirectToAction("Login", "Estudio");
            return View();
        }

        #endregion
        #region Pantalla Index
        /// <summary>
        /// asegura que la sesión sea válida y en caso contrario regresa a la pantalla de login
        /// </summary>
        /// <returns>regresa una vista</returns>
        [HttpGet]
        public ActionResult Index()
        {
            string res = Convert.ToString(this.Session["encryptedTicket"]);
            if (String.IsNullOrEmpty(res))
                return RedirectToAction("Login", "Estudio");

            ViewBag.UserLogin = this.Session["Account"].ToString();

            this.Session["IdTipoDocumento"] = null;
            this.Session["Documento"] = null;
            this.Session["Nombres"] = null;
            this.Session["Apellidos"] = null;
            this.Session["Asesor"] = null;

            return View();
        }

        /// <summary>
        /// Daniel Mercado 
        /// 11/06/2021 
        /// En este metodo retorna los roles existentes 
        /// </summary>
        /// <returns> Regresa los roles Existentes </returns>

        [HttpPost]
        public JsonResult ConsultarRoles()
        {
            Response _res = new Response();

            if (ModelState.IsValid)
            {
                _res = _MantenedorPerfilesLogic.ConsultarRoles();
            }
            return Json(_res, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region Pantallas Edit
        /// <summary>
        /// Daniel Mercado 
        /// 2021-06-01
        /// Mostrara la vista correspondiente para editar el permiso
        /// </summary>
        /// <param name="id">id del permiso a modificar</param>
        /// <returns>Nos regresara la vista con la informacion actual del permiso para posteriormente se modifique </returns>
        [HttpGet]
        public ActionResult Edit(int id)
        {
            string res = Convert.ToString(this.Session["encryptedTicket"]);
            if (String.IsNullOrEmpty(res))
                return RedirectToAction("Login", "Estudio");

            MantenedorPerfiles permiso = _MantenedorPerfilesLogic.Details(id);
            ViewBag.cmbxSistemas = new SelectList(_MantenedorPerfilesLogic.ObtenerSistemas(), "IdSistema", "NombreSistema", permiso.IdSistema);
            return View(permiso);
        }
        #endregion

        public List<Sistemas> ObtenerSistemas()
        {
            return _MantenedorPerfilesLogic.ObtenerSistemas();
        }
        public List<UserProfile> ObtenerRoles()
        {
            return _MantenedorPerfilesLogic.ObtenerRoles();
        }
        #endregion
    }
}