using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Estudio.Repository.Core.Domain;
using Estudio.Repository.Core.Domain.Views;

namespace Estudio.Repository.Core.Repositories
{
    public interface IgzPageRepository : IRepository<gzPage>
    {
        List<PageView> Search(string description, string active);
        List<PageView> Search(string description);
    }
}
