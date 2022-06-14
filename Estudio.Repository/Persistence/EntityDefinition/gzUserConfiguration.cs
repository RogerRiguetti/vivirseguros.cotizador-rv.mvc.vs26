using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity.ModelConfiguration;
using Estudio.Repository.Core.Domain;
namespace Estudio.Repository.Persistence.EntityDefinition
{
    public class gzUserConfiguration : EntityTypeConfiguration<gzUser>
    {
        public gzUserConfiguration()
        {
        }
    }
}
