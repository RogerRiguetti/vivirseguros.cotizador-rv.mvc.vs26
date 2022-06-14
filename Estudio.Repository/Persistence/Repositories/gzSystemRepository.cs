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
public class gzSystemRepository : Repository<gzSystem>, IgzSystemRepository
{
public gzSystemRepository(ApplicationContext _context) : base(_context) { }
public ApplicationContext applicationContext { get { return Context as ApplicationContext; } }
        //public List<ViewgzSystem> ExecuteProcedure(int parameter)
        //{
        //List<ViewgzSystem> result = applicationContext.Database
        //.SqlQuery<ViewgzSystem>("ProcedureName @parameter", new SqlParameter("parameter", parameter))
        //.ToList();
        //return result;
        //}


        public List<SystemView> Search(string description, string active)
        {
            return (from qry in applicationContext.SystemRoutines.Where(o => o.Active == active && o.Description.Contains(description))
                    select new SystemView
                    {
                        Id = qry.Id,
                        Description = qry.Description,
                        Status = (qry.Active == EnumValues.ActiveValue ? EnumValues.ActiveText : EnumValues.InactiveText)
                    }).ToList();
        }
        public List<SystemView> Search(string description)
        {
            return (from qry in applicationContext.SystemRoutines.Where(o => o.Description.Contains(description))
                    select new SystemView
                    {
                        Id = qry.Id,
                        Description = qry.Description,
                        Status = (qry.Active == EnumValues.ActiveValue ? EnumValues.ActiveText : EnumValues.InactiveText)
                    }).ToList();
        }

    }
}
