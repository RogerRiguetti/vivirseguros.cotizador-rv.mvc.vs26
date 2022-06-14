using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Estudio.Repository.Helpers;
using Estudio.Repository.Core.Domain;
using Estudio.Repository.Core.Domain.Views;
using Estudio.Repository.Persistence;
using Estudio.Controllers.Assets;

namespace Estudio.Controllers.Models
{
    public class PermissionModel
    {

        #region Transactional
        public Response UpdatePermissions(int Id, string strPermissionId)

        {
            Response _res = null;

            //Se valida que al menos una pantalla sea marcada.
            if (Utils.ValidateStr(strPermissionId))
            {
                _res = RemovePermission(Id);
                if (_res.IsOk)
                    _res = AddPermission(Id, strPermissionId);
            }
            else
            {
                _res.IsOk = false;
                _res.Message = "required";
            }
            return _res;

        }
        private static Response RemovePermission(int Id)
        {
            Response _res = new Response();
            try
            {
                using (var unitOfWork = new UnitOfWork(new ApplicationContext()))
                {
                    IEnumerable<PagePermission> objRemove = unitOfWork.PagePermissionRoutines.Find(x => x.PageId == Id);
                    unitOfWork.PagePermissionRoutines.RemoveRange(objRemove);
                    unitOfWork.Complete();
                }

                return _res;
            }
            catch (Exception e)
            {
                _res.IsOk = false;
                _res.Message = e.Message;
                return _res;
            }
        }
        private static Response AddPermission(int Id, string strPermissionId)
        {
            Response _res = new Response();
            try
            {
                using (var unitOfWork = new UnitOfWork(new ApplicationContext()))
                {
                    string[] values = strPermissionId.Split(',');
                    PagePermission obj = null;
                    for (var i = 0; i < values.Length; i++)
                    {
                        obj = new PagePermission();
                        obj.PageId = Id;
                        obj.PermissionId = Convert.ToInt32(values[i]);
                        unitOfWork.PagePermissionRoutines.Add(obj);
                    }
                    unitOfWork.Complete();
                    _res.Object = obj;
                }

                return _res;
            }
            catch (Exception e)
            {
                _res.IsOk = false;
                _res.Message = e.Message;
                return _res;
            }
        }

        public Response Create(gzPermission permission)
        {
            Response _res = new Response();
            try
            {
                using (var unitOfWork = new UnitOfWork(new ApplicationContext()))
                {
                    permission.Active = EnumValues.ActiveValue;
                    unitOfWork.PermissionRoutines.Add(permission);
                    unitOfWork.Complete();
                }
                return _res;
            }
            catch (Exception e)
            {
                _res.IsOk = false;
                _res.Message = e.Message;
                return _res;
            }
        }
        public Response Edit(gzPermission permission)
        {
            Response _res = new Response();
            try
            {
                using (var unitOfWork = new UnitOfWork(new ApplicationContext()))
                {
                    var obj = unitOfWork.PermissionRoutines.Single(o => o.Id == permission.Id);
                    obj.Description = permission.Description;
                    unitOfWork.PermissionRoutines.Attach(obj);
                    unitOfWork.Complete();
                }
                return _res;
            }
            catch (Exception e)
            {
                _res.IsOk = false;
                _res.Message = e.Message;
                return _res;
            }
        }
        public Response Delete(int id)
        {
            Response _res = new Response();
            try
            {
                using (var unitOfWork = new UnitOfWork(new ApplicationContext()))
                {
                    gzPermission Permission = unitOfWork.PermissionRoutines.Single(o => o.Id == id);
                    Permission.Active = EnumValues.InactiveValue;
                    unitOfWork.PermissionRoutines.Attach(Permission);
                    unitOfWork.Complete();
                    return _res;
                }
            }
            catch (Exception e)
            {
                _res.IsOk = false;
                _res.Message = e.Message;
                return _res;
            }

        }
        #endregion

        #region No Transactional
        public List<PermissionView> Search(string description, string active)
        {
            try
            {
                using (var unitOfWork = new UnitOfWork(new ApplicationContext()))
                {
                    if (active == String.Empty)
                        return unitOfWork.PermissionRoutines.Search(description);
                    else
                        return unitOfWork.PermissionRoutines.Search(description, active);
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public gzPermission Details(int id)
        {
            using (var unitOfWork = new UnitOfWork(new ApplicationContext()))
            {
                gzPermission usuario = unitOfWork.PermissionRoutines.Single(x => x.Id == id);
                if (usuario == null)
                {
                    return null;
                }
                return usuario;
            }
        }
        public List<AuthorizationView> GetPermissionsAdministration()
        {
            return null;
        }

        public List<PermissionView> GetPermissions(int pageId)
        {
            try
            {
                using (var unitOfWork = new UnitOfWork(new ApplicationContext()))
                    return unitOfWork.PermissionRoutines.GetPermissions(pageId);
            }
            catch (Exception)
            {
                return null;
            }

        }


        #endregion

    }
}