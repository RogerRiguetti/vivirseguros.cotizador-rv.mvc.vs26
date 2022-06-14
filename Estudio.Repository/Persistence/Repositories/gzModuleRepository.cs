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
public class gzModuleRepository : Repository<gzModule>, IgzModuleRepository
{
public gzModuleRepository(ApplicationContext _context) : base(_context) { }
public ApplicationContext applicationContext { get { return Context as ApplicationContext; } }
        //public List<ViewgzModule> ExecuteProcedure(int parameter)
        //{
        //List<ViewgzModule> result = applicationContext.Database
        //.SqlQuery<ViewgzModule>("ProcedureName @parameter", new SqlParameter("parameter", parameter))
        //.ToList();
        //return result;
        //}
        public List<ModuleView> Search(string description, string active)
        {
            return (from qry in applicationContext.ModuleRoutines.Where(o => o.Active == active && o.Description.Contains(description))
                    select new ModuleView
                    {
                        Id = qry.Id,
                        Description = qry.Description,
                        Status = (qry.Active == EnumValues.ActiveValue ? EnumValues.ActiveText : EnumValues.InactiveText)
                    }).ToList();
        }
        public List<ModuleView> Search(string description)
        {
            return (from qry in applicationContext.ModuleRoutines.Where(o => o.Description.Contains(description))
                    select new ModuleView
                    {
                        Id = qry.Id,
                        Description = qry.Description,
                        Status = (qry.Active == EnumValues.ActiveValue ? EnumValues.ActiveText : EnumValues.InactiveText)
                    }).ToList();
        }

        public List<ModuleView> GetModulesBySystem(int systemId)
        {
            return (from qry in applicationContext.ModuleRoutines.Where(o => o.SystemId==systemId)
                    select new ModuleView
                    {
                        Id = qry.Id,
                        Description = qry.Description
                    }).ToList();
        }
    }
}
