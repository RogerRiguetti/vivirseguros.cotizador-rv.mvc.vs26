using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Estudio.Repository.Core.Domain;
using Estudio.Repository.Core.Domain.Views;

namespace Estudio.Repository.Core.Repositories
{
    public interface IgzSystemRepository : IRepository<gzSystem>
    {
        List<SystemView> Search(string account, string active);
        List<SystemView> Search(string account);
    }
}
