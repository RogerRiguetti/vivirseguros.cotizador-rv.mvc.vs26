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
    public class ModuleModel
    {
        #region Transactional
        public Response Create(gzModule module)
        {
            Response _res = new Response();
            try
            {
                using (var unitOfWork = new UnitOfWork(new ApplicationContext()))
                {
                    module.Active = EnumValues.ActiveValue;
                    unitOfWork.ModuleRoutines.Add(module);
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
        public Response Edit(gzModule module)
        {
            Response _res = new Response();
            try
            {
                using (var unitOfWork = new UnitOfWork(new ApplicationContext()))
                {
                    var obj = unitOfWork.ModuleRoutines.Single(o => o.Id == module.Id);
                    obj.Description = module.Description;
                    obj.SystemId = module.SystemId;
                    unitOfWork.ModuleRoutines.Attach(obj);
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
                    gzModule Module = unitOfWork.ModuleRoutines.Single(o => o.Id == id);
                    Module.Active = EnumValues.InactiveValue;
                    unitOfWork.ModuleRoutines.Attach(Module);
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
        public List<ModuleView> Search(string description, string active)
        {
            try
            {
                using (var unitOfWork = new UnitOfWork(new ApplicationContext()))
                {
                    if (active == String.Empty)
                        return unitOfWork.ModuleRoutines.Search(description);
                    else
                        return unitOfWork.ModuleRoutines.Search(description, active);
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public gzModule Details(int id)
        {
            using (var unitOfWork = new UnitOfWork(new ApplicationContext()))
            {
                gzModule usuario = unitOfWork.ModuleRoutines.Single(x => x.Id == id);
                if (usuario == null)
                {
                    return null;
                }
                return usuario;
            }
        }

        public List<ModuleView> GetModulesBySystem(int systemId)
        {
            try
            {
                using (var unitOfWork = new UnitOfWork(new ApplicationContext()))
                {
                    return unitOfWork.ModuleRoutines.GetModulesBySystem(systemId);
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        #endregion

    }
}