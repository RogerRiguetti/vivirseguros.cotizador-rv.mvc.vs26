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
public class gzPageRepository : Repository<gzPage>, IgzPageRepository
{
public gzPageRepository(ApplicationContext _context) : base(_context) { }
public ApplicationContext applicationContext { get { return Context as ApplicationContext; } }
        //public List<ViewgzPage> ExecuteProcedure(int parameter)
        //{
        //List<ViewgzPage> result = applicationContext.Database
        //.SqlQuery<ViewgzPage>("ProcedureName @parameter", new SqlParameter("parameter", parameter))
        //.ToList();
        //return result;
        //}
        public List<PageView> Search(string description, string active)
        {
            return (from qry in applicationContext.PageRoutines.Where(o => o.Active == active && o.Description.Contains(description))
                    join mod in applicationContext.ModuleRoutines.Where(o => o.Active==EnumValues.ActiveValue) on qry.ModuleId equals mod.Id
                    join sys in applicationContext.SystemRoutines.Where(o => o.Active==EnumValues.ActiveValue) on mod.SystemId equals sys.Id
                    select new PageView
                    {
                        Id = qry.Id,
                        Description = qry.Description,
                        System = sys.Description,
                        Module = mod.Description,
                        Status = (qry.Active == EnumValues.ActiveValue ? EnumValues.ActiveText : EnumValues.InactiveText)
                    }).ToList();
        }
        public List<PageView> Search(string description)
        {
            return (from qry in applicationContext.PageRoutines.Where(o => o.Description.Contains(description))
                    join mod in applicationContext.ModuleRoutines.Where(o => o.Active == EnumValues.ActiveValue) on qry.ModuleId equals mod.Id
                    join sys in applicationContext.SystemRoutines.Where(o => o.Active == EnumValues.ActiveValue) on mod.SystemId equals sys.Id
                    select new PageView
                    {
                        Id = qry.Id,
                        Description = qry.Description,
                        System = sys.Description,
                        Module = mod.Description,
                        Status = (qry.Active == EnumValues.ActiveValue ? EnumValues.ActiveText : EnumValues.InactiveText)
                    }).ToList();
        }
    }
}
