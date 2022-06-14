using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Estudio.Repository.Helpers;
using Estudio.Controllers.Models;
using Estudio.Repository.Core.Domain;
using Estudio.Controllers.Assets;
using Estudio.Logic;
using Estudio.Repository.Core.Domain.Views;

namespace Estudio.Controllers.Controllers
{
    public class RoleController : Controller
    {
        RolesLogic _rolesLogic = new RolesLogic();
        #region Transaccionales
        #region Pantalla Index
        /// <summary>
        ///  Autor: Heber Solis
        /// este metodo se conecta con la capa logica para efectuar la eliminacion del rol y mostrarlo a la vista
        /// </summary>
        /// <param name="id">id del rol</param>
        /// <param name="active">estado del rol</param>
        /// <returns>regresa en un json la respuesta del metodo de la capa logica</returns>
        [HttpPost]
        public ActionResult Delete(int id, int active)
        {
            try
            {
                Response _res = new Response();

                int idUsuario = Convert.ToInt32(this.Session["UserId"]);

                if (ModelState.IsValid)
                {
                    _res = _rolesLogic.DeleteRole(id, active, idUsuario);
                }
                return Json(_res, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {

                throw;
            }
           
        }
        #endregion
        #region Pantalla Create
        /// <summary>
        ///  Autor: Heber Solis
        /// este metodo se conecta a la capa logica para efectuar la creacion del nuevo rol 
        /// daniel mercado
        /// se agrega nuevo parametro Clave
        /// </summary>
        /// <param name="description">el nombre del nuevo rol</param>
        /// <param name="Clave">la clave del rol creado</param>
        /// <returns>regresa en un json la respuesta del metodo de la capa logica</returns>
        [HttpPost]
        public ActionResult Create(string description, string Clave)
        {
            Response _res = new Response();

            int idUsuario = Convert.ToInt32(this.Session["UserId"]);

            if (ModelState.IsValid)
            {
                _res = _rolesLogic.CreateRole(description, Clave, idUsuario);
            }
            return Json(_res, JsonRequestBehavior.AllowGet);
        }

        #endregion
        #region Pantalla Edit
        /// <summary>
        /// Autor: Heber Solis
        /// Metodo encargado de hacer la edicion de un rol se conecta con la capa logica para efectuar la operacion
        /// </summary>
        /// <param name="id">id del rol</param>
        /// <param name="description">nueva descripcion</param>
       
        /// <returns>regresa un json que contiene los nuevos datos</returns>       
        [HttpPost]
        
        public ActionResult Edit(int id, string description, gzRole role)
        {
            
            Response _res = new Response();

            int idUsuario = Convert.ToInt32(this.Session["UserId"]);

            if (ModelState.IsValid)
            {
                _res = _rolesLogic.EditRole(id, description, idUsuario);
            }
            return Json(_res, JsonRequestBehavior.AllowGet);
        }
        #endregion
      
        #endregion

        #region No Transaccionales

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
            ViewBag.modal_delete_header = Utils.modal_delete_body;
            ViewBag.modal_delete_body = Utils.modal_delete_body;

            this.Session["IdTipoDocumento"] = null;
            this.Session["Documento"] = null;
            this.Session["Nombres"] = null;
            this.Session["Apellidos"] = null;
            this.Session["Asesor"] = null;

            return View();
        }
    /// <summary>
    /// efectua la primera consulta al ingresar a la pantalla de roles
    /// </summary>
    /// <returns>regresa la lista de roles, en una tabla</returns>
        public List <RoleView> Search()
        {
            return _rolesLogic.Search();
        }

        #endregion
        #region Pantalla Create
        /// <summary>
        /// se asegura que haya una sesion iniciada, en caso contrario regresa a la pantalla de login
        /// </summary>
        /// <returns>regresa una vista</returns>
        [HttpGet]
        public ActionResult Create()
        {
            if (this.Session["Account"] == null)
                return RedirectToAction("Login", "Application");
            ViewBag.UserLogin = this.Session["Account"].ToString();
            ViewBag.PermissionsAdministration = new PermissionModel().GetPermissionsAdministration();


            return View();
        }
        #endregion

        #region Pantalla Details
        /// <summary>
        /// se asegura que haya una sesion iniciada, en caso contrario regresa a la pantalla de login en caso de que si este logueado el usuario, muestra los detalles del rol
        /// </summary>
        /// <returns>regresa una vista</returns>
        [HttpGet]
        public ActionResult Details(int id)
        {
            string res = Convert.ToString(this.Session["encryptedTicket"]);
            if (String.IsNullOrEmpty(res))
                return RedirectToAction("Login", "Estudio");
            
            return View(_rolesLogic.DetailsRole(id));
        }
        #endregion
        #region Pantallas Edit
        /// <summary>
        /// este metodo regresa la consulta del rol a editar, antes de que se de en el boton de save
        /// </summary>
        /// <param name="id">id del rol a editar</param>
        /// <param name="description">la descripcion actual del rol</param>
        /// <returns>regresa la vista donde se muestran los elementos</returns>
        [HttpGet]
        public ActionResult Edit(int id, string description)
        {
            string res = Convert.ToString(this.Session["encryptedTicket"]);
            if (String.IsNullOrEmpty(res))
                return RedirectToAction("Login", "Estudio");
          
            return View(_rolesLogic.DetailsRole(id));
        }
        #endregion
      
        #endregion

    }
}