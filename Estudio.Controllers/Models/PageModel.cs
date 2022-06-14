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
    public class PageModel
    {
        #region Transactional
        public Response Create(gzPage page)
        {
            Response _res = new Response();
            try
            {
                using (var unitOfWork = new UnitOfWork(new ApplicationContext()))
                {
                    page.Active = EnumValues.ActiveValue;
                    unitOfWork.PageRoutines.Add(page);
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
        public Response Edit(gzPage page)
        {
            Response _res = new Response();
            try
            {
                using (var unitOfWork = new UnitOfWork(new ApplicationContext()))
                {
                    var obj = unitOfWork.PageRoutines.Single(o => o.Id == page.Id);
                    obj.Description = page.Description;
                    obj.ModuleId = page.ModuleId;
                    unitOfWork.PageRoutines.Attach(obj);
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
                    gzPage Page = unitOfWork.PageRoutines.Single(o => o.Id == id);
                    Page.Active = EnumValues.InactiveValue;
                    unitOfWork.PageRoutines.Attach(Page);
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
        public List<PageView> Search(string description, string active)
        {
            try
            {
                using (var unitOfWork = new UnitOfWork(new ApplicationContext()))
                {
                    if (active == String.Empty)
                        return unitOfWork.PageRoutines.Search(description);
                    else
                        return unitOfWork.PageRoutines.Search(description, active);
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public gzPage Details(int id)
        {
            using (var unitOfWork = new UnitOfWork(new ApplicationContext()))
            {
                gzPage usuario = unitOfWork.PageRoutines.Single(x => x.Id == id);
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