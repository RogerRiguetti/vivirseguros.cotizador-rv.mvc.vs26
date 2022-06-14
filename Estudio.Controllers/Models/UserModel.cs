using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Estudio.Repository.Helpers;
using Estudio.Repository.Core.Domain;
using Estudio.Repository.Core.Domain.Views;
using Estudio.Repository.Persistence;
using Estudio.Controllers.Assets;
using Estudio.Logic;
namespace Estudio.Controllers.Models
{
    public class UserModel
    {
        UserLogic _userLogic = new UserLogic();

        #region Transactional



        public Response UpdateRoles(int Id, string strId)

        {
            Response _res = null;

            //Se valida que al menos una pantalla sea marcada.
            if (Utils.ValidateStr(strId))
            {
                _res = RemoveRole(Id);
                if (_res.IsOk)
                    _res = AddRole(Id, strId);
            }
            else
            {
                _res.IsOk = false;
                _res.Message = "required";
            }
            return _res;

        }
        private static Response RemoveRole(int Id)
        {
            Response _res = new Response();
            try
            {
                using (var unitOfWork = new UnitOfWork(new ApplicationContext()))
                {
                    IEnumerable<Authorization> objRemove = unitOfWork.AuthorizationRoutines.Find(x => x.UserId == Id);
                    unitOfWork.AuthorizationRoutines.RemoveRange(objRemove);
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
        private static Response AddRole(int Id, string strId)
        {
            Response _res = new Response();
            try
            {
                using (var unitOfWork = new UnitOfWork(new ApplicationContext()))
                {
                    string[] values = strId.Split(',');
                    Authorization obj = null;
                    for (var i = 0; i < values.Length; i++)
                    {
                        obj = new Authorization();
                        obj.UserId = Id;
                        obj.RolePagePermissionId = Convert.ToInt32(values[i]);
                        unitOfWork.AuthorizationRoutines.Add(obj);
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

        public Response Create(gzUser user)
        {
            Response _res = new Response();
            try
            {
                using (var unitOfWork = new UnitOfWork(new ApplicationContext()))
                {



                    user.Password = Utils.CreatePassword(user.Password);
                    user.DateCreated = DateTime.Now;
                    user.DateModified = DateTime.Now;
                    user.Active = EnumValues.ActiveValue;
                    unitOfWork.UserRoutines.Add(user);
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



        public Response Edit(gzUser user)
        {
            Response _res = new Response();
            try
            {
                using (var unitOfWork = new UnitOfWork(new ApplicationContext()))
                {

                    var obj = unitOfWork.UserRoutines.Single(o => o.Id == user.Id);
                    obj.DateModified = DateTime.Now;
                    obj.Account = user.Account;
                    if (user.Password != "-1")
                        obj.Password = Utils.CreatePassword(user.Password);

                    unitOfWork.UserRoutines.Attach(obj);
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
                    gzUser user = unitOfWork.UserRoutines.Single(o => o.Id == id);
                    user.Active = EnumValues.InactiveValue;
                    unitOfWork.UserRoutines.Attach(user);
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

        public List<RoleView> GetRolesByUser(int userId)
        {
            try
            {
                using (var unitOfWork = new UnitOfWork(new ApplicationContext()))
                    return unitOfWork.UserRoutines.GetRolesByUser(userId);
            }
            catch (Exception)
            {
                return null;
            }

        }



        #endregion

        //public List<UserView> Search()
        //{
        //    try
        //    {
        //        return _userLogic.Index();
        //    }
        //    catch (Exception ex)
        //    {
        //        return null;
        //    }
        //}
    }
}