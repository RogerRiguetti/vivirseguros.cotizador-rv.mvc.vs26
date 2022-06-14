using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Estudio.Repository.Core.Domain;
using Estudio.Repository.Core.Domain.Views;
using Estudio.Repository.Core.Repositories;
using Estudio.Repository.Helpers;
using System.Data.SqlClient;
namespace Estudio.Repository.Persistence.Repositories
{
public class gzPermissionRepository : Repository<gzPermission>, IgzPermissionRepository
{
public gzPermissionRepository(ApplicationContext _context) : base(_context) { }
public ApplicationContext applicationContext { get { return Context as ApplicationContext; } }
        //public List<ViewgzPermission> ExecuteProcedure(int parameter)
        //{
        //List<ViewgzPermission> result = applicationContext.Database
        //.SqlQuery<ViewgzPermission>("ProcedureName @parameter", new SqlParameter("parameter", parameter))
        //.ToList();
        //return result;
        //}

        public List<PermissionView> Search(string description, string active)
        {
            return (from qry in applicationContext.PermissionRoutines.Where(o => o.Active == active && o.Description.Contains(description))
                    select new PermissionView
                    {
                        Id = qry.Id,
                        Description = qry.Description,
                        Status = (qry.Active == EnumValues.ActiveValue ? EnumValues.ActiveText : EnumValues.InactiveText)
                    }).ToList();
        }
        public List<PermissionView> Search(string description)
        {
            return (from qry in applicationContext.PermissionRoutines.Where(o => o.Description.Contains(description))
                    select new PermissionView
                    {
                        Id = qry.Id,
                        Description = qry.Description,
                        Status = (qry.Active == EnumValues.ActiveValue ? EnumValues.ActiveText : EnumValues.InactiveText)
                    }).ToList();
        }

        public List<PermissionView> GetPermissions(int pageId)
        {
            return (from qry in applicationContext.PermissionRoutines.Where(o => o.Active == EnumValues.ActiveValue)
                     join pagper in applicationContext.PagePermissionRoutines.Where(o => o.PageId == pageId)
                         on qry.Id equals pagper.PermissionId
                     into tbl_pagper
                     from tbl_pagper_dat in tbl_pagper.DefaultIfEmpty()
                     select new PermissionView
                     {
                         Id = qry.Id,
                         Description = qry.Description,
                         Status = (qry.Active == EnumValues.ActiveValue ? EnumValues.ActiveText : EnumValues.InactiveText),
                         Unassigned = tbl_pagper_dat.PageId
                     }).ToList();
        }
    }
}
