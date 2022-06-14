using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Estudio.Repository.Helpers;
using Estudio.Controllers.Models;
using Estudio.Repository.Core.Domain;
using Estudio.Controllers.Assets;
using Estudio.Repository.Core.Domain.Views;
using Estudio.Repository.Persistence;
using Estudio.Logic; 

namespace Estudio.Controllers.Controllers
{
    public class UserController : Controller
    {
        UserLogic _userLogic = new UserLogic();

        #region Transaccionales
        #region Pantalla Index
        /// <summary>
        /// Lizbeth Morales 15/02/2018
        /// En este metodo se obtiene la informacion de la parte logica para activar o desactivar un usuario
        /// </summary>
        /// <param name="id">Parametro que contiene el id del usuario a desactivar o activar </param>
        /// <param name="active">Parametro que contiene la informacion del status del usuario (Activado o Desactivado)</param>
        /// <returns>Regresa un Json con la información que se cambio del usuario</returns>
        [HttpPost]
        public ActionResult Delete(int id, int active)
        {
            Response _res = new Response();

            int idUsuario = Convert.ToInt32(this.Session["UserId"]);

            if (ModelState.IsValid)
            {
                _res = _userLogic.Delete(id, active, idUsuario);
            }
            return Json(_res, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// Lizbeth Morales 12/02/2018 En este Metodo nos obtiene la informacion de todos los usuarios para posteriormente mandarlos a la vista de Index.
        /// </summary>
        /// <returns>Regresa la vista con la informacion de los usuarios </returns>
        public ActionResult Search(string account, string names, string lastNames)
        {
            try
            {
                return Json(_userLogic.Index(account, names, lastNames), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        #endregion
        #region Pantalla Create
        /// <summary>
        /// Lizbeth Morales 14/02/2018 En este metodo se realiza la creación de nuevos usuarios, trae la informacion del usuario a agregar 
        /// 
        /// Antonio Quezada 2018-03-28
        /// Se agregan los parámetros de Correo, IdSupervisor, Names y LastNames
        /// Daniel Mercado 11/06/2021
        /// se agrega parametro UserProfileList
        /// </summary>
        /// <param name="Id">Parametro que guardara la id del nuevo usuario</param>
        /// <param name="Account">Parametro que guardara el nombre del nuevo usuario</param>
        /// <param name="Password">Parametro que guardara la password del nuevo usuario</param>
        /// <param name="UserProfileList">Parametro que almacena la lista de roles que desempeña el nuevo usuario</param>
        /// <param name="Correo"> Correo del usuario creado </param>
        /// <param name="IdSupervisor"> Id del supervisor relacionado al asesor </param>
        /// <param name="Names"> Nombres del usuario </param>
        /// <param name="LastNames"> Apellidos del usuario </param>
        /// <param name="NumeroAgente"> Número del usuario en la tabla agente de JUBILARE </param>
        /// <returns> Regresa la informacion agregada en la vista para madarla a la base de datos y guardarla </returns>

        [HttpPost]
        public ActionResult Create(int Id, string Account, string Password, int[] UserProfileList, string Correo, int IdSupervisor, string Names, string LastNames, int NumeroAgente)
        {
            Response _res = null;

            int idUsuario = Convert.ToInt32(this.Session["UserId"]);

            if (ModelState.IsValid)
            {
                _res = new UserLogic().Create(0, Account, Password, 1, UserProfileList, Correo, IdSupervisor, Names, LastNames, NumeroAgente, idUsuario);
            }
            return Json(_res, JsonRequestBehavior.AllowGet);
        }

        #endregion
        #region Pantalla Edit
        /// <summary>
        /// Lizbeth Morales 15/02/2018
        /// Metodo en el cual se editará la informacíon del usuario seleccionado 
        /// 
        /// Antonio Quezada 2018-03-28
        /// Se agregan los parámetros de Correo, IdSupervisor, Names y LastNames
        ///  Daniel Mercado 11/06/2021
        /// se agrega prametro UserProfileList
        /// </summary>
        /// <param name="Id">Parametro el cual obtiene el id del usuario a modificar</param>
        /// <param name="Account">Parametro que obtiene el nombre actual del usuario, el cual puede ser modificado</param>
        /// <param name="Password">Parametro en donde se pondra la contraseña, la contraseña no se muestra por seguridad por lo tanto tendra que agregarse una nueva o confirmar la misma</param>
        /// <param name="UserProfileList">Parametro que almacena la lista de roles que desempeña el nuevo usuario</param>
        /// <param name="Correo"> Correo del usuario creado </param>
        /// <param name="IdSupervisor"> Id del supervisor relacionado al asesor </param>
        /// <param name="Names"> Nombres del usuario </param>
        /// <param name="LastNames"> Apellidos del usuario </param>
        /// <param name="NumeroAgente"> Número del usuario en la tabla agente de JUBILARE </param>
        /// <returns>Regresa un Json con la informacion a guardar del usuario</returns>

        [HttpPost]
        public ActionResult Edit(int Id, string Account, string Password,int [] UserProfileList, string Correo, int IdSupervisor, string Names, string LastNames, int numeroAgente)
        {
            Response _res = null;

            int idUsuario = Convert.ToInt32(this.Session["UserId"]);

            if (ModelState.IsValid)
            {
                _res = new UserLogic().Edit(Id, Account, Password, UserProfileList, Correo, IdSupervisor, Names, LastNames, numeroAgente, idUsuario);
            }
            return Json(_res, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult CambiaClave(int Id, string Pass)
        {
            Response _res = null;

            int idUsuario = Convert.ToInt32(this.Session["UserId"]);

            if (ModelState.IsValid)
            {
                _res = new UserLogic().CambioClave(Id, Pass, idUsuario);
            }
            return Json(_res, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult ConsultaClave(int id)
        {
            string res = Convert.ToString(this.Session["encryptedTicket"]);
            if (String.IsNullOrEmpty(res))
                return RedirectToAction("Login", "Estudio");

            gzUser usuario = _userLogic.ConsultaClave(id);
            return Json(usuario, JsonRequestBehavior.AllowGet);
        }


        #endregion
        #endregion

        #region No Transaccionales
        #region Pantalla Index
        /// <summary>
        /// Metodo que contiene el inicio del sistema, en donde se valida si el usuario ingresado es correcto
        /// </summary>
        /// <returns>Regresa la vista de inicio (Index) </returns>
        [HttpGet]
        public ActionResult Index()
        {
            string res = Convert.ToString(this.Session["encryptedTicket"]);
            if (String.IsNullOrEmpty(res))
                return RedirectToAction("Login", "Estudio");

            this.Session["IdTipoDocumento"] = null;
            this.Session["Documento"] = null;
            this.Session["Nombres"] = null;
            this.Session["Apellidos"] = null;
            this.Session["Asesor"] = null;

            return View();
        }
        #endregion
        #region Pantlla CambioClave
        /// <summary>
        /// 04/09/2019
        /// metodo de carga de la pantalla de cambio de contraseña
        /// retornando el id y la contranseña actual.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult CambioClave()
        {
            string res = Convert.ToString(this.Session["encryptedTicket"]);
            if (String.IsNullOrEmpty(res))
                return RedirectToAction("Login", "Estudio");

            int id = Convert.ToInt32(this.Session["UserId"]);
            ViewBag.id = Convert.ToString(this.Session["UserId"]);
            gzUser info = new gzUser();
            info = _userLogic.ConsultaClave(id);
            ViewBag.password = info.Password;

            return View();
        }
        #endregion
        #region Pantalla Create

        /// <summary>
        /// Lizbeth Morales  14/02/2018 Metodo que nos mandara a la vista correpondiente de crear
        /// </summary>
        /// <returns>Nos regresa a la vista de Crear</returns>
        [HttpGet]
        public ActionResult Create()
        {
            string res = Convert.ToString(this.Session["encryptedTicket"]);
            if (String.IsNullOrEmpty(res))
                return RedirectToAction("Login", "Estudio");

            ViewBag.cmbxRoles = new SelectList(_userLogic.ObtenerRoles(), "UserId", "Name");

            return View();
        }

        public List<UserProfile> ObtenerRoles()
        {
            return _userLogic.ObtenerRoles();
        }
        /// <summary>
        /// Antonio Quezada
        /// 2018-03-27
        /// Obtiene todos los supervisores
        /// </summary>
        /// <returns> Regresa una lista de usuarios con los supervisores </returns>

        public List<gzUser> ObtenerSupervisores()
        {
            return _userLogic.ObtenerSupervisores();
        }

        #endregion
        #region Pantalla Details
        /// <summary>
        /// Lizbeth Morales 13/02/2018 Metodo que nos mandara al vista de lo detalles del usuario, ademas de que vuelve a revisar si el usuario que ingreso inicio sesion correctamente 
        /// </summary>
        /// <param name="id"> Id del usuario a visualizar</param>
        /// <returns>Nos regresara la vista con la informacion del usuario</returns>
        [HttpGet]
        public ActionResult Details(int id)
        {
            string res = Convert.ToString(this.Session["encryptedTicket"]);
            if (String.IsNullOrEmpty(res))
                return RedirectToAction("Login", "Estudio");

             ViewBag.cmbxRoles = new SelectList(_userLogic.ObtenerRolesUsuario(id), "UserId", "Name",0);
           // ViewBag.cmbxRoles = new MultiSelectList(_userLogic.ObtenerRolesUsuario(id), "UserId", "Name", new[] { 1, 7,3 });
            return View(_userLogic.Details(id));

        }

        #endregion
        #region Pantallas Edit

        /// <summary>
        /// Lizbeth Morales 15/02/2018 Mostrara la vista correspondiente para editar al usuario seleccionado
        /// </summary>
        /// <param name="id">id del usuario a modificar</param>
        /// <returns>Nos regresara la vista con la informacion actual del usuario para posteriormente ser modificada </returns>
        [HttpGet]
        public ActionResult Edit(int id)
        {
            string res = Convert.ToString(this.Session["encryptedTicket"]);
            if (String.IsNullOrEmpty(res))
                return RedirectToAction("Login", "Estudio");

            gzUser usuario = _userLogic.Details(id);
            ViewBag.cmbxRoles = new SelectList(_userLogic.ObtenerRolesUsuario(id), "UserId", "Name", usuario.UserProfile_UserId);
            ViewBag.cmbxSupervisores = new SelectList(_userLogic.ObtenerSupervisores(), "Id", "NombreCompleto", usuario.IdSupervisor);
            return View(usuario);
        }
        #endregion
        #endregion
 
        /// <summary>
        /// Antonio Quezada
        /// 2018-02-14
        /// </summary>
        /// <param name="userId"> Id del usuario </param>
        /// <returns> Regresa los permisos que tiene el usuario </returns>
        public Response GetPermissions(int userId)
        {
            return _userLogic.GetPermissions(userId);
        }




        /// <summary>
        /// Daniel Mercado 
        /// 11/06/2021 
        /// En este metodo retorna los roles que el usuario tiene asignados
        /// <param name="Id">Parametro id del usuario con el  cual obtiene el rol selecionado del usuario</param>
         /// </summary>
        /// <returns> Regresa los roles Existentes y los roles del usuario para mostrarlos en la vista </returns>

        [HttpPost]
        public JsonResult ConsultarRoles(int Id)
        {
            Response _res = new Response(); 

            if (ModelState.IsValid)
            {
                _res = _userLogic.ConsultarRoles(Id);
            }
            return Json(_res, JsonRequestBehavior.AllowGet);
        }



        /// <summary>
        /// Daniel Mercado 
        /// 11/06/2021 
        /// En este metodo retorna los roles existentes 
        /// </summary>
        /// <returns> Regresa los roles Existentes </returns>

        [HttpPost]
        public JsonResult ConsultarRolesCrear()
        {
            Response _res = new Response();

            if (ModelState.IsValid)
            {
                _res = _userLogic.ConsultarRolesCrear();
            }
            return Json(_res, JsonRequestBehavior.AllowGet);
        }
    }
}