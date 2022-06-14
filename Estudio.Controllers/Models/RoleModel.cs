using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Estudio.Repository.Helpers;
using Estudio.Controllers.Assets;
using Estudio.Repository.Core.Domain;
using Estudio.Repository.Core.Domain.Views;
using Estudio.Repository.Persistence;

namespace Estudio.Controllers.Models
{
    public class RoleModel
    {


        #region Transaccionales

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
                    IEnumerable<RolePagePermission> objRemove = unitOfWork.RolePagePermissionRoutines.Find(x => x.RoleId == Id);
                    unitOfWork.RolePagePermissionRoutines.RemoveRange(objRemove);
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
                    RolePagePermission obj = null;
                    for (var i = 0; i < values.Length; i++)
                    {
                        obj = new RolePagePermission();
                        obj.RoleId = Id;
                        obj.PagePermissionId = Convert.ToInt32(values[i]);
                        unitOfWork.RolePagePermissionRoutines.Add(obj);
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


        public Response Create(gzRole role)
        {
            Response _res = new Response();
            try
            {
                using (var unitOfWork = new UnitOfWork(new ApplicationContext()))
                {


                    role.Active = EnumValues.ActiveValue;
                    unitOfWork.RoleRoutines.Add(role);
                    unitOfWork.Complete();
                    _res.Object = role;
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
        public Response Activate(int roleId)
        {
            Response _res = new Response();
            try
            {
                using (var unitOfWork = new UnitOfWork(new ApplicationContext()))
                {
                    gzRole rol = unitOfWork.RoleRoutines.Single(x => x.Id == roleId);
                    rol.Active = EnumValues.ActiveValue;
                    unitOfWork.RoleRoutines.Attach(rol);
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
        public Response Edit(gzRole role)
        {
            Response _res = new Response();
            try
            {
                using (var unitOfWork = new UnitOfWork(new ApplicationContext()))
                {

                    gzRole oRole = unitOfWork.RoleRoutines.Single(o => o.Id == role.Id);



                    oRole.Description = role.Description;


                    unitOfWork.RoleRoutines.Attach(oRole);
                    unitOfWork.Complete();
                    _res.Object = oRole;
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
        public Response Delete(int id)
        {
            Response _res = new Response();
            try
            {
                using (var unitOfWork = new UnitOfWork(new ApplicationContext()))
                {
                    gzRole obj = unitOfWork.RoleRoutines.Single(o => o.Id == id);
                    obj.Active = EnumValues.InactiveValue;
                    unitOfWork.RoleRoutines.Attach(obj);
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

        #region No Transaccionales


      
        public List<RoleView> Search(string description, string active)
        {
            try
            {
                using (var unitOfWork = new UnitOfWork(new ApplicationContext()))
                {

                    if (active == String.Empty)
                        return unitOfWork.RoleRoutines.Search(description);
                    else
                        return unitOfWork.RoleRoutines.Search(description, active);


                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public List<AuthorizationView> GetPermissionsByRole(int roleId)
        {
            try
            {
                using (var unitOfWork = new UnitOfWork(new ApplicationContext()))
                    return unitOfWork.RoleRoutines.GetPermissionsByRole(roleId);
            }
            catch (Exception)
            {
                return null;
            }

        }



        #endregion

    }
}