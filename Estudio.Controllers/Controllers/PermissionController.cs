using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Estudio.Repository.Helpers;
using Estudio.Controllers.Models;
using Estudio.Repository.Core.Domain;
using Estudio.Controllers.Assets;

namespace Estudio.Controllers.Controllers
{
    public class PermissionController : Controller
    {
        #region Transaccionales
        #region Pantalla Index
        [HttpPost]
        public ActionResult Delete(int id)
        {
            Response _res = null;
            if (ModelState.IsValid)
            {
                _res = new PermissionModel().Delete(id);
            }
            return Json(_res, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region Pantalla Create
        [HttpPost]
        public ActionResult Create([Bind(Include = "Id, Description")] gzPermission Permission)
        {
            Response _res = null;

            if (ModelState.IsValid)
            {
                _res = new PermissionModel().Create(Permission);
            }
            return Json(_res, JsonRequestBehavior.AllowGet);
        }

        #endregion
        #region Pantalla Edit
        [HttpPost]
        public ActionResult Edit([Bind(Include = "Id, Description")] gzPermission Permission)
        {
            Response _res = null;

            if (ModelState.IsValid)
            {
                _res = new PermissionModel().Edit(Permission);
            }
            return Json(_res, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #endregion

        #region No Transaccionales
        #region Pantalla Index
        [HttpGet]
        public ActionResult Index()
        {

            ViewBag.UserLogin = new EstudioController().ValidateSession();
            ViewBag.modal_delete_header = Utils.modal_delete_header;
            ViewBag.modal_delete_body = Utils.modal_delete_body;
            return View();
        }
        [HttpGet]
        public JsonResult Search(string description, string active)
        {
            return Json(new { data = new PermissionModel().Search(description, active) }, JsonRequestBehavior.AllowGet);
        }

        #endregion
        #region Pantalla Create
        [HttpGet]
        public ActionResult Create()
        {
            ViewBag.UserLogin = new EstudioController().ValidateSession();
            return View();
        }

        #endregion
        #region Pantalla Details
        [HttpGet]
        public ActionResult Details(int id)
        {
            ViewBag.UserLogin = new EstudioController().ValidateSession();
            return View(new PermissionModel().Details(id));

        }
        #endregion
        #region Pantallas Edit
        [HttpGet]
        public ActionResult Edit(int id)
        {
            ViewBag.UserLogin = new EstudioController().ValidateSession();
            return View(new PermissionModel().Details(id));

        }
        #endregion
        #endregion        
    }
}