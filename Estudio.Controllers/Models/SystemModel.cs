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
    public class SystemModel
    {

        #region Transactional
        public Response Create(gzSystem system)
        {
            Response _res = new Response();
            try
            {
                using (var unitOfWork = new UnitOfWork(new ApplicationContext()))
                {
                    system.Active = EnumValues.ActiveValue;
                    unitOfWork.SystemRoutines.Add(system);
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
        public Response Edit(gzSystem system)
        {
            Response _res = new Response();
            try
            {
                using (var unitOfWork = new UnitOfWork(new ApplicationContext()))
                {
                    var obj = unitOfWork.SystemRoutines.Single(o => o.Id == system.Id);
                    obj.Description = system.Description;
                    unitOfWork.SystemRoutines.Attach(obj);
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
                    gzSystem system = unitOfWork.SystemRoutines.Single(o => o.Id == id);
                    system.Active = EnumValues.InactiveValue;
                    unitOfWork.SystemRoutines.Attach(system);
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
        public List<SystemView> Search(string description, string active)
        {
            try
            {
                using (var unitOfWork = new UnitOfWork(new ApplicationContext()))
                {
                    if (active == String.Empty)
                        return unitOfWork.SystemRoutines.Search(description);
                    else
                        return unitOfWork.SystemRoutines.Search(description, active);
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public gzSystem Details(int id)
        {
            using (var unitOfWork = new UnitOfWork(new ApplicationContext()))
            {
                gzSystem usuario = unitOfWork.SystemRoutines.Single(x => x.Id == id);
                if (usuario == null)
                {
                    return null;
                }
                return usuario;
            }
        }
        #endregion

    }
}