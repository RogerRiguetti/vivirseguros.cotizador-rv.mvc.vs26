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
    public class PageController : Controller
    {
        #region Transaccionales
        #region Pantalla Index
        [HttpPost]
        public ActionResult Delete(int id)
        {
            Response _res = null;
            if (ModelState.IsValid)
            {
                _res = new PageModel().Delete(id);
            }
            return Json(_res, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region Pantalla Create
        [HttpPost]
        public ActionResult Create([Bind(Include = "Id, Description, ModuleId")] gzPage page)
        {
            Response _res = null;

            if (ModelState.IsValid)
            {
                _res = new PageModel().Create(page);
            }
            return Json(_res, JsonRequestBehavior.AllowGet);
        }

        #endregion
        #region Pantalla Edit
        [HttpPost]
        public ActionResult Edit([Bind(Include = "Id, Description, ModuleId")] gzPage page)
        {
            Response _res = null;

            if (ModelState.IsValid)
            {
                _res = new PageModel().Edit(page);
            }
            return Json(_res, JsonRequestBehavior.AllowGet);
        }

        #endregion
        #region Pantallas Configure
        [HttpPost]
        public ActionResult Configure(int Id, string strPermissionId)
        {
            Response _res = null;
            if (ModelState.IsValid)
            {
                _res = new PermissionModel().UpdatePermissions(Id, strPermissionId);
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
            return Json(new { data = new PageModel().Search(description, active) }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetModulesBySystem(int systemId)
        {
            return Json(new ModuleModel().GetModulesBySystem(systemId), JsonRequestBehavior.AllowGet);
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
            return View(new PageModel().Details(id));

        }
        #endregion
        #region Pantallas Edit
        [HttpGet]
        public ActionResult Edit(int id)
        {
            ViewBag.UserLogin = new EstudioController().ValidateSession();
            return View(new PageModel().Details(id));

        }
        #endregion
        #region Pantallas Configure
        [HttpGet]
        public ActionResult Configure(int id)
        {
            ViewBag.UserLogin = new EstudioController().ValidateSession();
            ViewBag.Permissions = new PermissionModel().GetPermissions(id);
            return View(new PageModel().Details(id));

        }
        #endregion
        #endregion        
    }
}